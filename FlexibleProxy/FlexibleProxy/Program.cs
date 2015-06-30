using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace FlexibleProxy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var pargs = new ArgumentsParser(args);
            var runServer = pargs.GetValue<bool>("Server");
            var runClient = pargs.GetValue<bool>("Client");

            var serverService = new FlexibleProxyServerService();
            var clientService = new FlexibleProxyClientService();
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Starting SERVER Service...");
                serverService.OnServiceStart(args);
                Console.WriteLine("SERVER is started in console.");

                Console.WriteLine("Starting CLIENT Service...");
                clientService.OnServiceStart(args);
                Console.WriteLine("CLIENT is started in console.");

                Console.WriteLine("Press ENTER to stop services and exit.");
                Console.ReadLine();

                serverService.OnServiceStop();
                clientService.OnServiceStop();
                Console.WriteLine("Both services are stopped.");
            }
            else
            {
                var servicesToRun = new List<ServiceBase>();
                if (runServer)
                    servicesToRun.Add(serverService);
                if (runClient)
                    servicesToRun.Add(clientService);
                ServiceBase.Run(servicesToRun.ToArray());
            }
        }
    }
}
