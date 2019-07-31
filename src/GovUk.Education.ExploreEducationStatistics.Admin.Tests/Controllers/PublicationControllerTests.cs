using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Models;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Controllers
{
    public class PublicationControllerTests
    {

        [Fact] 
        public void Create_Publication_Returns_Ok()
        {
            var publicationService = new Mock<IPublicationService>();

            publicationService
                .Setup(s => s.CreatePublication(It.IsAny<CreatePublicationViewModel>()))
                .Returns<CreatePublicationViewModel>(p => Task.FromResult(new Either<ValidationResult, PublicationViewModel>(new PublicationViewModel {TopicId = p.TopicId})));
            var controller = new PublicationController(publicationService.Object);

            var topicId = Guid.NewGuid();
            // Method under test
            var result = controller.CreatePublication(new CreatePublicationViewModel(), topicId);
            Assert.IsAssignableFrom<PublicationViewModel>(result.Result.Value);
            Assert.Equal(topicId, result.Result.Value.TopicId);
        }
        
        [Fact] 
        public void Create_Publication_Validation_Failure()
        {
            var publicationService = new Mock<IPublicationService>();

            publicationService
                .Setup(s => s.CreatePublication(It.IsAny<CreatePublicationViewModel>()))
                .Returns<CreatePublicationViewModel>(p => Task.FromResult(new Either<ValidationResult, PublicationViewModel>(new ValidationResult("Slug Error", new List<string>{"Slug"}))));
            var controller = new PublicationController(publicationService.Object);

            var topicId = Guid.NewGuid();
            // Method under test
            var result = controller.CreatePublication(new CreatePublicationViewModel(), topicId);
            var badRequestObjectResult = result.Result.Result;
            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequestObjectResult);
            var validationProblemDetails = (badRequestObjectResult as BadRequestObjectResult)?.Value;  
            Assert.IsAssignableFrom<ValidationProblemDetails>(validationProblemDetails);
            var errors = (validationProblemDetails as ValidationProblemDetails)?.Errors;
            Assert.True(errors.Keys.Contains("Slug"));
            Assert.Contains("Slug Error", errors["Slug"]);
        }
    }
}