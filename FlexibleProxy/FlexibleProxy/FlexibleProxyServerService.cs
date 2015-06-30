using System.ServiceProcess;

namespace FlexibleProxy
{
    public partial class FlexibleProxyServerService : ServiceBase
    {
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

        public FlexibleProxyServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {
            
        }
    }
}
