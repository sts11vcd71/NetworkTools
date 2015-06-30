using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Xml;

namespace TfsWatchDog.Bus
{
    public class ItemsHistoryProcessor
    {
        public WorkItemStore WorkItems { get; private set; }
        public Dictionary<DateTime, List<TimeSeriesPoint>> WorkItemsTimeSeries { get; set; }

        public void ConnectTfs()
        {
            var tfs = new TeamFoundationServer("http://tfsny:8080/tfs");
            tfs.Authenticate();
            WorkItems = new WorkItemStore(tfs);
        }

        public void GetAllPossibleColumns()
        {
            Project project = WorkItems.Projects["Retail"];
            var values = new Dictionary<string, string>();
            foreach (FieldDefinition fieldDefinition in project.WorkItemTypes["Task"].FieldDefinitions)
            {
                Console.WriteLine("{0},{1},{2}", fieldDefinition.Name, fieldDefinition.ReferenceName, fieldDefinition.FieldType);
            }
        }

        public void GetEverythingHistoricallyForScope(string title, string query, string sumColumn)
        {
            DateTime end = DateTime.Now;
            DateTime day = DateTime.Now.AddMonths(-2);

            while (day <= end)
            {
                Project project = WorkItems.Projects["Retail"];
                string qry122 = String.Format("SELECT [System.Id], [System.Title], [NSight.Workstation.TS.Fields.EstimatedManDays] FROM workitems WHERE ([System.IterationPath] UNDER 'Retail\\13.1.0.0' AND [System.WorkItemType] <> 'RFC') asof '{0}'", day);
                var coll122 = WorkItems.Query(qry122);
                string qry123 = String.Format("SELECT [System.Id], [System.Title], [NSight.Workstation.TS.Fields.EstimatedManDays] FROM workitems WHERE ([System.IterationPath] UNDER 'Retail\\13.1.0.0' AND [System.WorkItemType] <> 'RFC') asof '{0}'", day);
                var coll123 = WorkItems.Query(qry123);
                Console.WriteLine(String.Format("{0}\t{1}\t{2}", day, coll122.Count, coll123.Count));
                day = day.AddDays(1);
            }
        }

        public decimal CalculatedResidual(decimal? lev0, decimal? estimate, decimal? residual, int averageEstimate)
        {
            if (residual.HasValue)
                return residual.Value;
            else if (estimate.HasValue)
                return estimate.Value;
            else if (lev0.HasValue)
                return lev0.Value;
            else return averageEstimate;
        }

        public void BurnDownChart()
        {
            DateTime end = DateTime.Now;
            DateTime day = new DateTime(2014, 7, 21);

            List<string> allUsers = new List<string>();

            List<string> allStates = new List<string>();

            while (day <= end)
            {
                Project project = WorkItems.Projects["Retail"];
                string qry122 = String.Format("SELECT [System.Id], [Estimate.Level.0], [NSight.Workstation.TS.Fields.EstimatedManDays], [Residual.Work.Days], [System.AssignedTo], [System.State] FROM workitems WHERE ([System.IterationPath] UNDER 'Retail\\14.4.0.0' AND [System.WorkItemType] <> 'RFC') asof '{0}'", day);
                var coll122 = WorkItems.Query(qry122);

                decimal sumCalculatedResidual = 0;
                decimal sumCalculatedResidualPlusAverage = 0;
                int count = 0;

                foreach (WorkItem item in coll122)
                {
                    int id = (int) item["System.Id"];
                    decimal? estimateLevel0 = ToDecimal(item, "Estimate.Level.0");
                    decimal? estManDays = ToDecimal(item, "NSight.Workstation.TS.Fields.EstimatedManDays");
                    decimal? residualDays = ToDecimal(item, "Residual.Work.Days");
                    string assignedTo = (string)item["System.AssignedTo"];
                    string state = (string)item["System.State"];

                    if (!allUsers.Contains(assignedTo)) allUsers.Add(assignedTo);

                    if (!allStates.Contains(state)) allStates.Add(state);

                    sumCalculatedResidual += CalculatedResidual(estimateLevel0, estManDays, residualDays, 0);
                    sumCalculatedResidualPlusAverage += CalculatedResidual(estimateLevel0, estManDays, residualDays, 2);
                    count++;
                }

                Console.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}", day.Date, count, sumCalculatedResidual, sumCalculatedResidualPlusAverage));
                day = day.AddDays(1);
            }

            Console.WriteLine(String.Join(",", allUsers.ToArray()));
            Console.WriteLine(String.Join(",", allStates.ToArray()));
        }
        public string BurnDownChart(string iteration, DateTime startDate, DateTime endDate, int estimateAverage, List<string> developers)
        {
            StringBuilder result = new StringBuilder();
            DateTime end = endDate;
            DateTime day = startDate;

            result.AppendLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", "Date", "Count", "Count Dev", "Count Prod", "Count Opened", "Count Closed",
                "Sum Calc Residuals", "Sum Calc Residuals plus Avg", "Sum calc Res plus Avg DEV", "Sum calc Res plus Avg PROD"));

            while (day <= end)
            {
                Project project = WorkItems.Projects["Retail"];
                string qry122 = String.Format("SELECT [System.Id], [Estimate.Level.0], [NSight.Workstation.TS.Fields.EstimatedManDays], [Residual.Work.Days], [System.AssignedTo], [System.State] FROM workitems WHERE ([System.IterationPath] UNDER '"+ iteration +"' AND [System.WorkItemType] <> 'RFC') asof '{0}'", day);
                var coll122 = WorkItems.Query(qry122);

                decimal sumCalculatedResidual = 0;
                decimal sumCalculatedResidualPlusAverage = 0;
                decimal sumCalculatedResidualPAOnDev = 0;
                decimal sumCalculatedResidualPAOnProd = 0;
                int countOpened = 0;
                int countClosed = 0;
                int countDev = 0;
                int countProd = 0;
                int count = 0;

                foreach (WorkItem item in coll122)
                {
                    int id = (int) item["System.Id"];
                    decimal? estimateLevel0 = ToDecimal(item, "Estimate.Level.0");
                    decimal? estManDays = ToDecimal(item, "NSight.Workstation.TS.Fields.EstimatedManDays");
                    decimal? residualDays = ToDecimal(item, "Residual.Work.Days");
                    string assignedTo = (string)item["System.AssignedTo"];
                    string state = (string)item["System.State"];

                    count++;

                    if (state == "Resolved" || state == "Closed") countClosed++;
                    else countOpened++;

                    sumCalculatedResidual += CalculatedResidual(estimateLevel0, estManDays, residualDays, 0);
                    sumCalculatedResidualPlusAverage += CalculatedResidual(estimateLevel0, estManDays, residualDays, estimateAverage);

                    if (developers.Contains(assignedTo))
                    {
                        sumCalculatedResidualPAOnDev += CalculatedResidual(estimateLevel0, estManDays, residualDays,
                            estimateAverage);
                        countDev++;
                    }
                    else
                    {
                        sumCalculatedResidualPAOnProd += CalculatedResidual(estimateLevel0, estManDays, residualDays,
                            estimateAverage);
                        countProd++;
                    }

                }

                result.AppendLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", day.Date, count, countDev, countProd, countOpened, countClosed, 
                    sumCalculatedResidual, sumCalculatedResidualPlusAverage, sumCalculatedResidualPAOnDev, sumCalculatedResidualPAOnProd));
                day = day.AddDays(1);
            }
            return result.ToString();
        }

        [Serializable]
        public class BurnDownChartItem
        {
            public DateTime Date { get; set; }
            public decimal? SumCalculatedResidual { get; set; }
            public decimal? SumCalculatedResidualPlusAverage { get; set; }
            public decimal? SumCalculatedResidualPAOnDev { get; set; }
            public decimal? SumCalculatedResidualPAOnProd { get; set; }
            public int? CountOpened { get; set; }
            public int? CountClosed { get; set; }
            public int? CountDev { get; set; }
            public int? CountProd { get; set; }
            public int? Count { get; set; }
            public int Capacity { get; set; }
        }

        public static int BusinessDaysUntil(DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }

        public List<BurnDownChartItem> BurnDownChartToList(string iteration,
            DateTime startDate,
            DateTime endDate,
            int estimateAverage,
            List<string> developers,
            int NumberOfPeople,
            Dictionary<DateTime, int> vacation,
            bool includeSupportTasks)
        {
            List<BurnDownChartItem> result = new List<BurnDownChartItem>();
            DateTime end = endDate;
            DateTime day = startDate;

            while (day <= end)
            {
                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                {
                    day = day.AddDays(1);
                    continue;
                }
                
                int totalVacationsRemains = vacation.Where(x => x.Key.Date >= day.Date).Sum(x => x.Value);
                int daysRemains = BusinessDaysUntil(day, endDate);
                int capacity = daysRemains*NumberOfPeople - totalVacationsRemains;

                Project project = WorkItems.Projects["Retail"];

                string oper = !includeSupportTasks?"==":"Under";

                string qry122 = String.Format("SELECT [System.Id], [Estimate.Level.0], [NSight.Workstation.TS.Fields.EstimatedManDays]," +
                    " [Residual.Work.Days], [System.AssignedTo], [System.State] FROM workitems WHERE ([System.IterationPath] {0} '{1}'" +
                    " AND [System.WorkItemType] <> 'RFC') asof '{2}'", oper, iteration, day);
                
                var coll122 = WorkItems.Query(qry122);

                decimal sumCalculatedResidual = 0;
                decimal sumCalculatedResidualPlusAverage = 0;
                decimal sumCalculatedResidualPAOnDev = 0;
                decimal sumCalculatedResidualPAOnProd = 0;
                int countOpened = 0;
                int countClosed = 0;
                int countDev = 0;
                int countProd = 0;
                int count = 0;

                foreach (WorkItem item in coll122)
                {
                    int id = (int) item["System.Id"];
                    decimal? estimateLevel0 = ToDecimal(item, "Estimate.Level.0");
                    decimal? estManDays = ToDecimal(item, "NSight.Workstation.TS.Fields.EstimatedManDays");
                    decimal? residualDays = ToDecimal(item, "Residual.Work.Days");
                    string assignedTo = item["System.AssignedTo"].ToString();
                    string state = item["System.State"].ToString();

                    count++;

                    if (state == "Resolved" || state == "Closed") countClosed++;
                    else countOpened++;

                    sumCalculatedResidual += CalculatedResidual(estimateLevel0, estManDays, residualDays, 0);
                    sumCalculatedResidualPlusAverage += CalculatedResidual(estimateLevel0, estManDays, residualDays, estimateAverage);

                    if (developers.Contains(assignedTo))
                    {
                        sumCalculatedResidualPAOnDev += CalculatedResidual(estimateLevel0, estManDays, residualDays,
                            estimateAverage);
                        countDev++;
                    }
                    else
                    {
                        sumCalculatedResidualPAOnProd += CalculatedResidual(estimateLevel0, estManDays, residualDays,
                            estimateAverage);
                        countProd++;
                    }

                }

                if (day.Date <= DateTime.Now.Date)
                {
                    result.Add(new BurnDownChartItem()
                    {
                        Date = day.Date,
                        Count = count,
                        CountDev = countDev,
                        CountProd = countProd,
                        CountOpened = countOpened,
                        CountClosed = countClosed,
                        SumCalculatedResidual = sumCalculatedResidual,
                        SumCalculatedResidualPAOnDev = sumCalculatedResidualPAOnDev,
                        SumCalculatedResidualPAOnProd = sumCalculatedResidualPAOnProd,
                        SumCalculatedResidualPlusAverage = sumCalculatedResidualPlusAverage,
                        Capacity = capacity
                    });
                }
                else
                {
                    result.Add(new BurnDownChartItem{
                        Date = day.Date,
                        Capacity = capacity
                    });
                }
                day = day.AddDays(1);
            }
            return result;
        }

        public const string BURNDOWN_DATA_FILE = "RbbBurndownData.bin";

        private static decimal? ToDecimal(WorkItem item, string fieldName)
        {
            return item[fieldName] == null ? (decimal?)null : Convert.ToDecimal(item[fieldName]);
        }
    }
}
