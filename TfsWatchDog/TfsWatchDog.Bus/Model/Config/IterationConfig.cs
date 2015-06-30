using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TfsWatchDog.Bus.Utils;

namespace TfsWatchDog.Bus.Model.Config
{
    public class IterationConfig
    {
        public List<TeamMemberConfig> TeamMembers { get; set; }

        public List<string> TeamMembersNames { get; set; }

        public string Name { get; set; }

        public string TfsIterationPath { get; set; }

        public bool IsUnder { get; set; }

        public string Query { get; set; }

        public DateTime CodeStarDate { get; set; }

        public DateTime CodeCompleteDate { get; set; }

        public DateTime ProdPushDate { get; set; }

        public WatchdogConfig WatchDogRules { get; set; }
    }
}
