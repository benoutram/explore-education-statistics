﻿namespace GovUk.Education.ExploreEducationStatistics.Admin.Migrations.ContentMigrations.Temp
{
    public class EES17TableRowGroupOption
    {
        public string label { get; set; }
        public string level { get; set; }
        public string value { get; set; }

        public EES17TableRowGroupOption()
        {
        }

        public EES17TableRowGroupOption(string label, string level, string value)
        {
            this.label = label;
            this.level = level;
            this.value = value;
        }
    }
}