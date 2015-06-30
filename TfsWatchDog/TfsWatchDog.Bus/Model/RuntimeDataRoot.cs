using System.Collections.Generic;
using TfsWatchDog.Bus.Model.Config;
using TfsWatchDog.Bus.Model.Processing;

namespace TfsWatchDog.Bus.Model
{
    public class RuntimeDataRoot
    {
        public WatchdogConfig Configuration { get; set; }

        public List<IterationInfo> Iterations { get; set; } 
    }
}
