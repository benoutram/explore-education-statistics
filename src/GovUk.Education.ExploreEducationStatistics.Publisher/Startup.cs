﻿using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Common.Functions;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Publisher;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace GovUk.Education.ExploreEducationStatistics.Publisher
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddAutoMapper(typeof(Startup).Assembly)
                .AddMemoryCache()
                .AddDbContext<ContentDbContext>(options =>
                    options.UseSqlServer(ConnectionUtils.GetAzureSqlConnectionString("ContentDb")))
                .AddScoped<IFileStorageService, FileStorageService>()
                .AddScoped<IPublishingService, PublishingService>()
                .AddScoped<IContentCacheGenerationService, ContentCacheGenerationService>()
                .AddScoped<IContentService, ContentService>()
                .AddScoped<IReleaseService, ReleaseService>()
                .AddScoped<IPublicationService, PublicationService>()
                .AddScoped<IDownloadService, DownloadService>()
                .AddScoped<IMethodologyService, MethodologyService>()
                .BuildServiceProvider();
        }
    }
}