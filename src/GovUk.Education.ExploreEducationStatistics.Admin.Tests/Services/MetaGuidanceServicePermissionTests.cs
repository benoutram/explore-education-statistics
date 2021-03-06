﻿using System;
using GovUk.Education.ExploreEducationStatistics.Admin.Security;
using GovUk.Education.ExploreEducationStatistics.Admin.Services;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Content.Security;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Services.Interfaces;
using Moq;
using Xunit;
using static GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils.PermissionTestUtils;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Services
{
    public class MetaGuidanceServicePermissionTests
    {
        private readonly Release _release = new Release
        {
            Id = Guid.NewGuid()
        };

        [Fact]
        public void Get()
        {
            PolicyCheckBuilder<ContentSecurityPolicies>()
                .ExpectResourceCheckToFail(_release, ContentSecurityPolicies.CanViewRelease)
                .AssertForbidden(
                    userService =>
                    {
                        var service = SetupMetaGuidanceService(userService: userService.Object);
                        return service.Get(_release.Id);
                    }
                );
        }

        [Fact]
        public void Update()
        {
            PolicyCheckBuilder<SecurityPolicies>()
                .ExpectResourceCheckToFail(_release, SecurityPolicies.CanUpdateSpecificRelease)
                .AssertForbidden(
                    userService =>
                    {
                        var service = SetupMetaGuidanceService(userService: userService.Object);
                        return service.Update(_release.Id, new MetaGuidanceUpdateViewModel());
                    }
                );
        }


        private MetaGuidanceService SetupMetaGuidanceService(
            ContentDbContext contentDbContext = null,
            StatisticsDbContext statisticsDbContext = null,
            IPersistenceHelper<ContentDbContext> contentPersistenceHelper = null,
            IMetaGuidanceSubjectService metaGuidanceSubjectService = null,
            IUserService userService = null,
            IReleaseDataFileRepository releaseDataFileRepository = null)
        {
            return new MetaGuidanceService(
                contentDbContext ?? new Mock<ContentDbContext>().Object,
                contentPersistenceHelper ?? DefaultPersistenceHelperMock().Object,
                metaGuidanceSubjectService ?? new Mock<IMetaGuidanceSubjectService>().Object,
                statisticsDbContext ?? new Mock<StatisticsDbContext>().Object,
                userService ?? new Mock<IUserService>().Object,
                releaseDataFileRepository ?? new ReleaseDataFileRepository(contentDbContext)
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
