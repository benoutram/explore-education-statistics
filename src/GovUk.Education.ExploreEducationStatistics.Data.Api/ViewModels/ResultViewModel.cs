using System;
using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Data.Api.ViewModels.Meta;
using GovUk.Education.ExploreEducationStatistics.Data.Api.ViewModels.TableBuilder;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.ViewModels
{
    public class ResultViewModel
    {
        public Guid PublicationId { get; set; }
        public long ReleaseId { get; set; }
        public long SubjectId { get; set; }
        public DateTime ReleaseDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GeographicLevel GeographicLevel { get; set; }

        public IEnumerable<TableBuilderObservationViewModel> Result { get; set; }
        
        public SubjectMetaViewModel MetaData { get; set; }  

        public ResultViewModel()
        {
            Result = new List<TableBuilderObservationViewModel>();
        }
    }
}