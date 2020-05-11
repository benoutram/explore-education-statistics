﻿using System;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Controllers.Api
{
    public class PreReleaseControllerTests
    {
        [Fact]
        public async void GetPreReleaseSummaryAsync_Returns_Ok()
        {
            var (preReleaseContactsService, preReleaseService) = Mocks();

            var preReleaseSummaryViewModel =
                new PreReleaseSummaryViewModel("PreRelease Publication", "Autumn and Spring Term 2020/21");
            var releaseId = Guid.NewGuid();

            preReleaseService
                .Setup(s => s.GetPreReleaseSummaryViewModelAsync(It.Is<Guid>(id => id == releaseId)))
                .ReturnsAsync(preReleaseSummaryViewModel);

            var controller = new PreReleaseController(preReleaseContactsService.Object, preReleaseService.Object);

            var result = await controller.GetPreReleaseSummaryAsync(releaseId);
            var unboxed = AssertOkResult(result);

            Assert.Equal(preReleaseSummaryViewModel.PublicationTitle, unboxed.PublicationTitle);
            Assert.Equal(preReleaseSummaryViewModel.ReleaseTitle, unboxed.ReleaseTitle);
        }

        private static (Mock<IPreReleaseContactsService> PreReleaseContactsService,
            Mock<IPreReleaseService> PreReleaseService) Mocks()
        {
            return (new Mock<IPreReleaseContactsService>(),
                new Mock<IPreReleaseService>());
        }

        private static T AssertOkResult<T>(ActionResult<T> result) where T : class
        {
            Assert.IsAssignableFrom<T>(result.Value);
            return result.Value;
        }
    }
}