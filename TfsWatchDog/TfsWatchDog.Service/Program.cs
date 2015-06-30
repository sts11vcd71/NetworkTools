using System;
using System.ServiceProcess;

namespace TfsWatchDog.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var service = new WatchdogService();
            if (Environment.UserInteractive)
            {
                service.OnServiceStart(args);
                Console.WriteLine("TfsWatchdogService is started in console. Press ENTER to stop service.");
                Console.ReadLine();
                service.OnServiceStop();
                Console.WriteLine("TfsWatchdogService is started stopped.");
            }
            else
            {
                ServiceBase[] servicesToRun = {new WatchdogService()};
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
