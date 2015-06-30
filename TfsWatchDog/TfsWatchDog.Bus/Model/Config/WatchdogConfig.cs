using System.Collections.Generic;

namespace TfsWatchDog.Bus.Model.Config
{
    public class WatchdogConfig
    {
        public string SendBroadcastEmailTo { get; set; }
        public string SendTeamleadEmailTo { get; set; }
        public List<IterationConfig> Iterations { get; set; } 
        public TfsServerConfig TfsSettings { get; set; } 
        public List<TeamMemberConfig> Team { get; set; } 
        public List<DogAlertConfig> AvailableAlerts { get; set; } 
    }
}
