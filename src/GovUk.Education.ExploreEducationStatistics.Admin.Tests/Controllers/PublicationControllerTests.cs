using System;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using Moq;
using Xunit;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Controllers
{
    public class PublicationControllerTests
    {

        [Fact] public void Create_Publication_Returns_Ok()
        {
            var publicationService = new Mock<IPublicationService>();

            publicationService
                .Setup(s => s.CreatePublication(It.IsAny<CreatePublicationViewModel>()))
                .Returns<CreatePublicationViewModel>(p => new PublicationViewModel {TopicId = p.TopicId});
            var controller = new PublicationController(publicationService.Object);

            var topicId = Guid.NewGuid();
            // Method under test
            var result = controller.CreatePublication(new CreatePublicationViewModel(), topicId);
            Assert.IsAssignableFrom<PublicationViewModel>(result.Value);
            Assert.Equal(topicId, result.Value.TopicId);
        }
    }
}