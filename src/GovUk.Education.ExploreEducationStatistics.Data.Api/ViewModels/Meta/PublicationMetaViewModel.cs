using System;
using System.Collections.Generic;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.ViewModels.Meta
{
    public class PublicationMetaViewModel
    {
        public Guid PublicationId { get; set; }
        public Dictionary<string, IEnumerable<IndicatorMetaViewModel>> Indicators { get; set; }
        public Dictionary<string, IEnumerable<CharacteristicMetaViewModel>> Characteristics { get; set; }
        public ObservationalUnitsMetaViewModel ObservationalUnits { get; set; }
    }
}