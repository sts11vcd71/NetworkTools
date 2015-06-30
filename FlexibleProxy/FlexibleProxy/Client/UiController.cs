using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Win32;

namespace FlexibleProxy.Client
{
    public class UiController : ApiController
    {
#if DEBUG
        private const string UiFilesRootPath = @"D:\Projects\Experimental\NetworkTools\FlexibleProxy\FlexibleProxy\ui\"; 
#else 
        private const string UiFilesRootPath = String.Empty; 
#endif

        public string GetContentType(string fname)
        {
            // set a default mimetype if not found.
            string contentType = "application/octet-stream";

            try
            {
                if (fname == null)
                    return contentType;
                // get the registry classes root
                var classes = Registry.ClassesRoot;

                // find the sub key based on the file extension
                var fileClass = classes.OpenSubKey(Path.GetExtension(fname));
                if (fileClass != null) contentType = fileClass.GetValue("Content Type").ToString();
            }
            catch
            { }

            return contentType;
        }

        [HttpGet]
        [Route("ui/{*fullPath}")]
        public HttpResponseMessage Get(string fullPath)
        {
            string filePhisicalPath = Path.Combine(UiFilesRootPath, fullPath);
            if (!File.Exists(filePhisicalPath))
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            var stream = new FileStream(filePhisicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            var mimeType = GetContentType(fullPath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            return result;
        }
    }
}
