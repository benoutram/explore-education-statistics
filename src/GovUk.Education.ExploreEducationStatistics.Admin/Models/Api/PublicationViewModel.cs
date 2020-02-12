using System;
using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Content.Model;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Models.Api
{
    public class PublicationViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public List<ReleaseViewModel> Releases { get; set; }

        public MethodologyViewModel Methodology { get; set; }

        public Guid TopicId { get; set; }

        public Guid ThemeId { get; set; }

        public Contact Contact { get; set; }
    }
}