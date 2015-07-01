using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Microsoft.Win32;

namespace FlexibleProxy.Client
{
    public class UiController : ApiController
    {
#if DEBUG
        private const string UiFilesRootPath = @"..\..\ui\"; 
#else 
        private const string UiFilesRootPath = String.Empty; 
#endif
        private const string NotFound404Content = @"<html><body><h1>Not found!</h1></body></html>";

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
            Stream stream;
            HttpStatusCode statusCode;
            if (File.Exists(filePhisicalPath))
            {
                stream = new FileStream(filePhisicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite,
                    0x1000, FileOptions.SequentialScan);
                statusCode = HttpStatusCode.OK;
            }
            else
            {
                stream = new MemoryStream(Encoding.UTF8.GetBytes(NotFound404Content));
                statusCode = HttpStatusCode.NotFound;
            }
            var result = new HttpResponseMessage(statusCode) {Content = new StreamContent(stream)};
            var mimeType = GetContentType(fullPath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            return result;
        }
    }
}
