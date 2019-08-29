using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Data.Services.ViewModels.Meta;

namespace GovUk.Education.ExploreEducationStatistics.Data.Services.ViewModels
{
    public class ResultWithMetaViewModel
    {
        public SubjectMetaViewModel MetaData { get; set; }
        public IEnumerable<ObservationViewModel> Result { get; set; }

        public ResultWithMetaViewModel()
        {
            Result = new List<ObservationViewModel>();
        }
    }
}