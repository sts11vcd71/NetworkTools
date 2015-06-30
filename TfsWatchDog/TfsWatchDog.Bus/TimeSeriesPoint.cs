using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsWatchDog.Bus
{
    public class TimeSeriesPoint
    {
        public string Project { get; set; }
        public string IterationPath { get; set; }
        public List<WorkItem> WorkItems { get; set; }
    }
}