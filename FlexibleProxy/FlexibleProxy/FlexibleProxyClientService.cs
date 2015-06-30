using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleProxy
{
    partial class FlexibleProxyClientService : ServiceBase
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

        public FlexibleProxyClientService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

    }
}
