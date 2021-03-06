﻿using System;
using GovUk.Education.ExploreEducationStatistics.Admin.Security;
using GovUk.Education.ExploreEducationStatistics.Admin.Services;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using static GovUk.Education.ExploreEducationStatistics.Admin.Security.SecurityPolicies;
using static GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils.PermissionTestUtils;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Services
{
    public class ReleaseImageServicePermissionTests
    {
        private readonly Release _release = new Release
        {
            Id = Guid.NewGuid()
        };

        [Fact]
        public void Upload()
        {
            PolicyCheckBuilder<SecurityPolicies>()
                .ExpectResourceCheckToFail(_release, CanUpdateSpecificRelease)
                .AssertForbidden(
                    userService =>
                    {
                        var service = SetupReleaseImageService(userService: userService.Object);
                        return service.Upload(releaseId: _release.Id,
                            formFile: new Mock<IFormFile>().Object);
                    }
                );
        }

        private ReleaseImageService SetupReleaseImageService(
            ContentDbContext contentDbContext = null,
            IPersistenceHelper<ContentDbContext> contentPersistenceHelper = null,
            IBlobStorageService blobStorageService = null,
            IFileUploadsValidatorService fileUploadsValidatorService = null,
            IReleaseFileRepository releaseFileRepository = null,
            IUserService userService = null)
        {
            return new ReleaseImageService(
                contentDbContext ?? new Mock<ContentDbContext>().Object,
                contentPersistenceHelper ?? DefaultPersistenceHelperMock().Object,
                blobStorageService ?? new Mock<IBlobStorageService>().Object,
                fileUploadsValidatorService ?? new Mock<IFileUploadsValidatorService>().Object,
                releaseFileRepository ?? new ReleaseFileRepository(contentDbContext),
                userService ?? new Mock<IUserService>().Object
            );
        }

        private Mock<IPersistenceHelper<ContentDbContext>> DefaultPersistenceHelperMock()
        {
            var mock = MockUtils.MockPersistenceHelper<ContentDbContext, Release>();
            MockUtils.SetupCall(mock, _release.Id, _release);
            return mock;
        }
    }
}
