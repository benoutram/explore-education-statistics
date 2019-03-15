using System;
using System.Linq;
using System.Reflection;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Importer
{
    public static class DataFileAttributeExtensions
    {
        public static Type GetDataTypeFromDataFileAttributeOfEnumType(this Enum enumValue, Type enumType)
        {
            return enumType.GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DataFileAttribute>()
                .DataType;
        }
    }
}