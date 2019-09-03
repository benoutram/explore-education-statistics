using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Controllers
{
    public class PublicationControllerTests
    {

        [Fact]
        public async void Create_Publication_Returns_Ok()
        {
            var publicationService = new Mock<IPublicationService>();

            publicationService
                .Setup(s => s.CreatePublicationAsync(It.IsAny<CreatePublicationViewModel>()))
                .Returns<CreatePublicationViewModel>(p => Task.FromResult(new Either<ValidationResult, PublicationViewModel>(new PublicationViewModel {TopicId = p.TopicId})));
            var controller = new PublicationController(publicationService.Object);

            var topicId = Guid.NewGuid();
            // Method under test
            var result = await controller.CreatePublicationAsync(new CreatePublicationViewModel(), topicId);
            Assert.IsAssignableFrom<PublicationViewModel>(result.Value);
            Assert.Equal(topicId, result.Value.TopicId);
        }
        
        [Fact] 
        public async void Create_Publication_Validation_Failure()
        {
            var publicationService = new Mock<IPublicationService>();

            publicationService
                .Setup(s => s.CreatePublicationAsync(It.IsAny<CreatePublicationViewModel>()))
                .Returns<CreatePublicationViewModel>(p => Task.FromResult(new Either<ValidationResult, PublicationViewModel>(new ValidationResult("Slug Error", new List<string>{"Slug"}))));
            var controller = new PublicationController(publicationService.Object);

            var topicId = Guid.NewGuid();
            // Method under test
            var result = await controller.CreatePublicationAsync(new CreatePublicationViewModel(), topicId);
            var badRequestObjectResult = result.Result;
            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequestObjectResult);
            var validationProblemDetails = (badRequestObjectResult as BadRequestObjectResult)?.Value;  
            Assert.IsAssignableFrom<ValidationProblemDetails>(validationProblemDetails);
            var errors = (validationProblemDetails as ValidationProblemDetails)?.Errors;
            Assert.True(errors.Keys.Contains("Slug"));
            Assert.Contains("Slug Error", errors["Slug"]);
        }
    }
}