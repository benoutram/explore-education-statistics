using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Common.Model;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Migrations.EES17
{
    public class EES17TableBuilderTableHeadersConfig
    {
        public IEnumerable<IEnumerable<LabelValue>> ColumnGroups { get; set; }
        public IEnumerable<LabelValue> Columns { get; set; }
        public IEnumerable<IEnumerable<LabelValue>> RowGroups { get; set; }
        public IEnumerable<LabelValue> Rows { get; set; }
    }
}