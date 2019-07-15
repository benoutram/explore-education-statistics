using System;

namespace GovUk.Education.ExploreEducationStatistics.Model.Database
{   
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumLabelValueAttribute : Attribute
    {
        public string Label { get; }
        public string Value { get; }

        public EnumLabelValueAttribute(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}