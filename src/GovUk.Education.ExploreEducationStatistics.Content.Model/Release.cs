using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Converters;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using Newtonsoft.Json;
using static System.DateTime;
using static GovUk.Education.ExploreEducationStatistics.Content.Model.PartialDate;
using static System.String;
using static GovUk.Education.ExploreEducationStatistics.Common.Model.TimeIdentifierCategory;

namespace GovUk.Education.ExploreEducationStatistics.Content.Model
{
    public class Release
    {
        public Guid Id { get; set; }

        public string Title => CoverageTitle + (IsNullOrEmpty(YearTitle) ? "" : " " + YearTitle);
        
        public string YearTitle
        {
            get
            {
                // Calendar year time identifiers we just use the year, all others we use a year range.
                // We express this range in the format e.g. 2019/20
                if (!IsNullOrEmpty(_releaseName) && YearRegex.Match(_releaseName).Success &&
                    !CalendarYear.GetTimeIdentifiers().Contains(TimePeriodCoverage))
                {
                    var releaseStartYear = Int32.Parse(_releaseName);
                    var releaseEndYear = (releaseStartYear % 100) + 1; // Only want the last two digits
                    return releaseStartYear + "/" + releaseEndYear;
                }
                // For calendar year time identifiers we just want the year not a range. If there is no year then we
                // just output the time period identifier
                return IsNullOrEmpty(ReleaseName) ? "" : ReleaseName;
            }
        }
        
        public string CoverageTitle => TimePeriodCoverage.GetEnumLabel(); 
        
        private string _releaseName;

        public string ReleaseName
        {
            get => _releaseName;
            set
            {
                if (value == null || YearRegex.Match(value).Success)
                {
                    _releaseName = value;
                }
                else
                {
                    throw new FormatException("The release name is invalid");
                }
            }
        }

        /**
         * The last date the release was published - this should be set when the PublishScheduled date is reached and
         * the release is published.
         */
        public DateTime? Published { get; set; }

        // The date that the release is scheduled to be published - when this time is reached then the release should
        // be published and the Published date set.
        public DateTime? PublishScheduled { get; set; }

        [NotMapped] public bool Live => Published.HasValue && (Compare(UtcNow, Published.Value) > 0);

        public string Slug { get; set; }

        public string Summary { get; set; }

        public Guid PublicationId { get; set; }

        public Publication Publication { get; set; }

        public List<Update> Updates { get; set; }

        public List<ContentSection> Content { get; set; }

        public DataBlock KeyStatistics { get; set; }

        public Guid? TypeId { get; set; }

        public ReleaseType Type { get; set; }

        [JsonConverter(typeof(TimeIdentifierJsonConverter))]
        public TimeIdentifier TimePeriodCoverage { get; set; }

        public int Order { get; set; }

        private PartialDate _nextReleaseDate;

        public PartialDate NextReleaseDate
        {
            get => _nextReleaseDate;
            set
            {
                if (value == null || value.IsValid())
                {
                    _nextReleaseDate = value;
                }
                else
                {
                    throw new FormatException("The next release date is invalid");
                }
            }
        }
    }
}