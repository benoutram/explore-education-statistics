using System;
using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Data.Api.Models.Query;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Models
{
    public abstract class TidyData : ITidyData
    {
        public long Id { get; set; }
        public Guid PublicationId { get; set; }
        public Release Release { get; set; }
        public long ReleaseId { get; set; }
        public int TimePeriod { get; set; }
        public string TimeIdentifier { get; set; }
        public Level Level { get; set; }
        public Country Country { get; set; }
        public SchoolType SchoolType { get; set; }
        public Dictionary<string, string> Indicators { get; set; }
    }
}