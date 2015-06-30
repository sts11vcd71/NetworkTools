using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace FlexibleProxy.Client
{
    partial class FlexibleProxyClientService : ServiceBase
    {
        #region Fields and Construction

        private HttpSelfHostServer _webApiServer;

        public FlexibleProxyClientService()
        {
            InitializeComponent();
        }

        #endregion

        #region For Running Service in Console

        public void OnServiceStart(string[] args)
        {
            OnStart(args);
        }

        public void OnServiceStop()
        {
            OnStop();
        }

        #endregion

        protected override void OnStart(string[] args)
        {
            var selfHostConfiguraiton = new HttpSelfHostConfiguration("http://localhost:9999/");
            selfHostConfiguraiton.Routes.MapHttpRoute(
                name: "ApiRoute",
                routeTemplate: "api/{controller}",
                defaults: null
            );
            selfHostConfiguraiton.MapHttpAttributeRoutes();
            selfHostConfiguraiton.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            _webApiServer = new HttpSelfHostServer(selfHostConfiguraiton);
            _webApiServer.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
            _webApiServer.CloseAsync();
            _webApiServer.Dispose();
        }

    }
}
