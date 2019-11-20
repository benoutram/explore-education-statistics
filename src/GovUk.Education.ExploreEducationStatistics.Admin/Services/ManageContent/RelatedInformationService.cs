﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.ManageContent;
using GovUk.Education.ExploreEducationStatistics.Admin.Validators;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationUtils;
using static GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Utils.ReleaseUtils;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services.ManageContent
{
    public class RelatedInformationService : IRelatedInformationService
    {
        private readonly ContentDbContext _context;

        public RelatedInformationService(ContentDbContext context)
        {
            _context = context;
        }
        
        public Task<Either<ValidationResult, List<BasicLink>>> GetRelatedInformationAsync(Guid releaseId)
        {
            return CheckReleaseExists(_context, releaseId, release => release.RelatedInformation);
        }

        public Task<Either<ValidationResult, List<BasicLink>>> AddRelatedInformationAsync(Guid releaseId, CreateUpdateLinkRequest request)
        {
            return CheckReleaseExists(_context, releaseId, async release =>
            {
                release.RelatedInformation.Add(new BasicLink
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Url = request.Url
                });

                _context.Releases.Update(release);
                await _context.SaveChangesAsync();
                return release.RelatedInformation;
            });
        }
        
        public Task<Either<ValidationResult, List<BasicLink>>> UpdateRelatedInformationAsync(
            Guid releaseId, Guid relatedInformationId, CreateUpdateLinkRequest request)
        {
            return CheckReleaseExists(_context, releaseId, async release =>
            {
                var toUpdate = release.RelatedInformation.Find(item => item.Id == relatedInformationId);

                if (toUpdate == null)
                {
                    return ValidationResult(ValidationErrorMessages.RelatedInformationItemNotFound);
                }

                toUpdate.Title = request.Title;
                toUpdate.Url = request.Url;

                _context.Releases.Update(release);
                await _context.SaveChangesAsync();
                return new Either<ValidationResult, List<BasicLink>>(release.RelatedInformation);
            });
        }
        
        public Task<Either<ValidationResult, List<BasicLink>>> DeleteRelatedInformationAsync(Guid releaseId, Guid relatedInformationId)
        {
            return CheckReleaseExists(_context, releaseId, async release =>
            {
                release.RelatedInformation.Remove(
                    release.RelatedInformation.Find(item => item.Id == relatedInformationId));

                _context.Releases.Update(release);
                await _context.SaveChangesAsync();
                return release.RelatedInformation;
            });
        }
    }
}