using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Tests.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using static GovUk.Education.ExploreEducationStatistics.Admin.Tests.Services.DbUtils;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationErrorMessages;
using static GovUk.Education.ExploreEducationStatistics.Common.BlobContainers;
using static GovUk.Education.ExploreEducationStatistics.Common.Model.FileType;
using static GovUk.Education.ExploreEducationStatistics.Common.Services.FileStoragePathUtils;
using static GovUk.Education.ExploreEducationStatistics.Common.Services.FileStorageUtils;
using static GovUk.Education.ExploreEducationStatistics.Common.Tests.Utils.MockFormTestUtils;
using File = GovUk.Education.ExploreEducationStatistics.Content.Model.File;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Services
{
    public class ReleaseFileServiceTests
    {
        [Fact]
        public async Task Delete()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var chartFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "chart.png",
                    Type = Chart
                }
            };

            var imageFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "image.png",
                    Type = Image
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile, imageFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()))
                .Returns(Task.CompletedTask);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, ancillaryFile.File.Id);

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()), Times.Once);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.Null(
                    await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));

                // Check that other files remain untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(chartFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(chartFile.File.Id));
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(imageFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(imageFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_FileFromAmendment()
        {
            var release = new Release();

            var amendmentRelease = new Release
            {
                PreviousVersionId = release.Id
            };

            var ancillaryFile = new File
            {
                RootPath = Guid.NewGuid(),
                Filename = "ancillary.pdf",
                Type = Ancillary,
            };

            var releaseFile = new ReleaseFile
            {
                Release = release,
                File = ancillaryFile
            };

            var amendmentReleaseFile = new ReleaseFile
            {
                Release = amendmentRelease,
                File = ancillaryFile
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddRangeAsync(release, amendmentRelease);
                await contentDbContext.AddRangeAsync(ancillaryFile);
                await contentDbContext.AddRangeAsync(releaseFile, amendmentReleaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(amendmentRelease.Id, ancillaryFile.Id);

                Assert.True(result.IsRight);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the file is unlinked from the amendment
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(amendmentReleaseFile.Id));

                // Check that the file and link to the previous version remain untouched
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(releaseFile.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_InvalidFileType()
        {
            var release = new Release();

            var dataFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "data.csv",
                    Type = FileType.Data,
                    SubjectId = Guid.NewGuid()
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddAsync(dataFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, dataFile.File.Id);

                Assert.True(result.IsLeft);
                ValidationTestUtil.AssertValidationProblem(result.Left, FileTypeInvalid);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the file remains untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(dataFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(dataFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_ReleaseNotFound()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddAsync(ancillaryFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(Guid.NewGuid(), ancillaryFile.File.Id);

                result.AssertNotFound();
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the file remains untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_FileNotFound()
        {
            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, Guid.NewGuid());

                result.AssertNotFound();
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_MultipleFiles()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var chartFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "chart.png",
                    Type = Chart
                }
            };

            var imageFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "image.png",
                    Type = Image
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile, imageFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()))
                .Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()))
                .Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, imageFile.Path()))
                .Returns(Task.CompletedTask);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, new List<Guid>
                {
                    ancillaryFile.File.Id,
                    chartFile.File.Id,
                    imageFile.File.Id
                });

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, imageFile.Path()), Times.Once);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));

                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(chartFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(chartFile.File.Id));

                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(imageFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(imageFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_MultipleFilesWithAnInvalidFileType()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var dataFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "data.csv",
                    Type = FileType.Data,
                    SubjectId = Guid.NewGuid()
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile, dataFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, new List<Guid>
                {
                    ancillaryFile.File.Id,
                    dataFile.File.Id
                });

                Assert.True(result.IsLeft);
                ValidationTestUtil.AssertValidationProblem(result.Left, FileTypeInvalid);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that all the files remain untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));
                
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(dataFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(dataFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_MultipleFilesWithReleaseNotFound()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var chartFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "chart.png",
                    Type = Chart
                }
            };

            var imageFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "image.png",
                    Type = Image
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile, imageFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(Guid.NewGuid(), new List<Guid>
                {
                    ancillaryFile.File.Id,
                    chartFile.File.Id,
                    imageFile.File.Id
                });

                result.AssertNotFound();
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that all the files remain untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));

                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(chartFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(chartFile.File.Id));

                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(imageFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(imageFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_MultipleFilesWithAFileNotFound()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(release.Id, new List<Guid>
                {
                    ancillaryFile.File.Id,
                    // Include an unknown id
                    Guid.NewGuid()
                });

                result.AssertNotFound();
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the files remain untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));
            }
            
            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Delete_MultipleFilesWithAFileFromAmendment()
        {
            var release = new Release();

            var amendmentRelease = new Release
            {
                PreviousVersionId = release.Id
            };

            var ancillaryFile = new File
            {
                RootPath = Guid.NewGuid(),
                Filename = "ancillary.pdf",
                Type = Ancillary
            };

            var chartFile = new File
            {
                RootPath = Guid.NewGuid(),
                Filename = "chart.png",
                Type = Chart
            };

            var ancillaryReleaseFile = new ReleaseFile
            {
                Release = release,
                File = ancillaryFile
            };

            var ancillaryAmendmentReleaseFile = new ReleaseFile
            {
                Release = amendmentRelease,
                File = ancillaryFile
            };

            var chartAmendmentReleaseFile = new ReleaseFile
            {
                Release = amendmentRelease,
                File = chartFile
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddRangeAsync(release, amendmentRelease);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile);
                await contentDbContext.AddRangeAsync(ancillaryReleaseFile, ancillaryAmendmentReleaseFile,
                    chartAmendmentReleaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()))
                .Returns(Task.CompletedTask);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Delete(amendmentRelease.Id, new List<Guid>
                {
                    ancillaryFile.Id,
                    chartFile.Id
                });

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()), Times.Once);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the ancillary file is unlinked from the amendment
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(ancillaryAmendmentReleaseFile.Id));

                // Check that the ancillary file and link to the previous version remain untouched
                Assert.NotNull(
                    await contentDbContext.Files.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryReleaseFile.Id));

                // Check that the chart file and link to the amendment are removed
                Assert.Null(await contentDbContext.Files.FindAsync(chartFile.Id));
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(chartAmendmentReleaseFile.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task DeleteAll()
        {
            var release = new Release();

            var ancillaryFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var chartFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "chart.png",
                    Type = Chart
                }
            };

            var dataFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "data.csv",
                    Type = FileType.Data,
                    SubjectId = Guid.NewGuid()
                }
            };

            var imageFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "image.png",
                    Type = Image
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile, dataFile, imageFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()))
                .Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()))
                .Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, imageFile.Path()))
                .Returns(Task.CompletedTask);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.DeleteAll(release.Id);

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, ancillaryFile.Path()), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, imageFile.Path()), Times.Once);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(ancillaryFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(ancillaryFile.File.Id));

                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(chartFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(chartFile.File.Id));

                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(imageFile.Id));
                Assert.Null(await contentDbContext.Files.FindAsync(imageFile.File.Id));

                // Check that data files remain untouched
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(dataFile.Id));
                Assert.NotNull(await contentDbContext.Files.FindAsync(dataFile.File.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task DeleteAll_ReleaseNotFound()
        {
            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext())
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.DeleteAll(Guid.NewGuid());

                result.AssertNotFound();
            }
            
            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task DeleteAll_NoFiles()
        {
            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.DeleteAll(release.Id);

                Assert.True(result.IsRight);
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task DeleteAll_FileFromAmendment()
        {
            var release = new Release();

            var amendmentRelease = new Release
            {
                PreviousVersionId = release.Id
            };

            var ancillaryFile = new File
            {
                RootPath = Guid.NewGuid(),
                Filename = "ancillary.pdf",
                Type = Ancillary
            };

            var chartFile = new File
            {
                RootPath = Guid.NewGuid(),
                Filename = "chart.png",
                Type = Chart
            };

            var ancillaryReleaseFile = new ReleaseFile
            {
                Release = release,
                File = ancillaryFile
            };

            var ancillaryAmendmentReleaseFile = new ReleaseFile
            {
                Release = amendmentRelease,
                File = ancillaryFile
            };

            var chartAmendmentReleaseFile = new ReleaseFile
            {
                Release = amendmentRelease,
                File = chartFile
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddRangeAsync(release, amendmentRelease);
                await contentDbContext.AddRangeAsync(ancillaryFile, chartFile);
                await contentDbContext.AddRangeAsync(ancillaryReleaseFile, ancillaryAmendmentReleaseFile,
                    chartAmendmentReleaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);
            blobStorageService.Setup(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()))
                .Returns(Task.CompletedTask);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.DeleteAll(amendmentRelease.Id);

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.DeleteBlob(PrivateReleaseFiles, chartFile.Path()), Times.Once);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                // Check that the ancillary file is unlinked from the amendment
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(ancillaryAmendmentReleaseFile.Id));

                // Check that the ancillary file and link to the previous version remain untouched
                Assert.NotNull(await contentDbContext.Files.FindAsync(ancillaryFile.Id));
                Assert.NotNull(await contentDbContext.ReleaseFiles.FindAsync(ancillaryReleaseFile.Id));

                // Check that the chart file and link to the amendment are removed
                Assert.Null(await contentDbContext.Files.FindAsync(chartFile.Id));
                Assert.Null(await contentDbContext.ReleaseFiles.FindAsync(chartAmendmentReleaseFile.Id));
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task ListAll_NoFiles()
        {
            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.ListAll(release.Id, Ancillary, Chart);

                Assert.True(result.IsRight);
                Assert.Empty(result.Right);
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task ListAll_ReleaseNotFound()
        {
            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext())
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.ListAll(Guid.NewGuid(), Ancillary, Chart);

                result.AssertNotFound();
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task ListAll()
        {
            var release = new Release();

            var ancillaryFile1 = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary_1.pdf",
                    Type = Ancillary
                }
            };

            var ancillaryFile2 = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "Ancillary 2.pdf",
                    Type = Ancillary
                }
            };

            var chartFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "chart.png",
                    Type = Chart
                }
            };

            var dataFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "data.csv",
                    Type = FileType.Data,
                    SubjectId = Guid.NewGuid()
                }
            };

            var imageFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "image.png",
                    Type = Image
                }
            };
            
            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddRangeAsync(ancillaryFile1, ancillaryFile2,
                    chartFile, dataFile, imageFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                    mock.CheckBlobExists(PrivateReleaseFiles, It.IsIn(
                        ancillaryFile1.Path(),
                        ancillaryFile2.Path(),
                        chartFile.Path(),
                        imageFile.Path())))
                .ReturnsAsync(true);

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, ancillaryFile1.Path()))
                .ReturnsAsync(new BlobInfo(
                    path: ancillaryFile1.Path(),
                    size: "10 Kb",
                    contentType: "application/pdf",
                    contentLength: 0L,
                    meta: GetAncillaryFileMetaValues(
                        name: "Ancillary Test File 1"),
                    created: null));

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, ancillaryFile2.Path()))
                .ReturnsAsync(new BlobInfo(
                    path: ancillaryFile2.Path(),
                    size: "10 Kb",
                    contentType: "application/pdf",
                    contentLength: 0L,
                    meta: GetAncillaryFileMetaValues(
                        name: "Ancillary Test File 2"),
                    created: null));

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, chartFile.Path()))
                .ReturnsAsync(new BlobInfo(
                    path: chartFile.Path(),
                    size: "20 Kb",
                    contentType: "image/png",
                    contentLength: 0L,
                    meta: new Dictionary<string, string>(),
                    created: null));

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, imageFile.Path()))
                .ReturnsAsync(new BlobInfo(
                    path: imageFile.Path(),
                    size: "30 Kb",
                    contentType: "image/png",
                    contentLength: 0L,
                    meta: new Dictionary<string, string>(),
                    created: null));

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.ListAll(release.Id, Ancillary, Chart, Image);

                Assert.True(result.IsRight);

                blobStorageService.Verify(mock =>
                    mock.CheckBlobExists(PrivateReleaseFiles, It.IsIn(
                        ancillaryFile1.Path(),
                        ancillaryFile2.Path(),
                        chartFile.Path(),
                        imageFile.Path())), Times.Exactly(4));

                blobStorageService.Verify(mock =>
                    mock.GetBlob(PrivateReleaseFiles, It.IsIn(
                        ancillaryFile1.Path(),
                        ancillaryFile2.Path(),
                        chartFile.Path(),
                        imageFile.Path())), Times.Exactly(4));

                var fileInfoList = result.Right.ToList();
                Assert.Equal(4, fileInfoList.Count);

                Assert.Equal(ancillaryFile1.File.Id, fileInfoList[0].Id);
                Assert.Equal("pdf", fileInfoList[0].Extension);
                Assert.Equal("ancillary_1.pdf", fileInfoList[0].FileName);
                Assert.Equal("Ancillary Test File 1", fileInfoList[0].Name);
                Assert.Equal(ancillaryFile1.Path(), fileInfoList[0].Path);
                Assert.Equal("10 Kb", fileInfoList[0].Size);
                Assert.Equal(Ancillary, fileInfoList[0].Type);

                Assert.Equal(ancillaryFile2.File.Id, fileInfoList[1].Id);
                Assert.Equal("pdf", fileInfoList[1].Extension);
                Assert.Equal("Ancillary 2.pdf", fileInfoList[1].FileName);
                Assert.Equal("Ancillary Test File 2", fileInfoList[1].Name);
                Assert.Equal(ancillaryFile2.Path(), fileInfoList[1].Path);
                Assert.Equal("10 Kb", fileInfoList[1].Size);
                Assert.Equal(Ancillary, fileInfoList[1].Type);

                Assert.Equal(chartFile.File.Id, fileInfoList[2].Id);
                Assert.Equal("png", fileInfoList[2].Extension);
                Assert.Equal("chart.png", fileInfoList[2].FileName);
                Assert.Equal("chart.png", fileInfoList[2].Name);
                Assert.Equal(chartFile.Path(), fileInfoList[2].Path);
                Assert.Equal("20 Kb", fileInfoList[2].Size);
                Assert.Equal(Chart, fileInfoList[2].Type);

                Assert.Equal(imageFile.File.Id, fileInfoList[3].Id);
                Assert.Equal("png", fileInfoList[3].Extension);
                Assert.Equal("image.png", fileInfoList[3].FileName);
                Assert.Equal("image.png", fileInfoList[3].Name);
                Assert.Equal(imageFile.Path(), fileInfoList[3].Path);
                Assert.Equal("30 Kb", fileInfoList[3].Size);
                Assert.Equal(Image, fileInfoList[3].Type);
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Stream()
        {
            var release = new Release();

            var releaseFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddAsync(releaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            var blob = new BlobInfo(
                path: null,
                size: null,
                contentType: "application/pdf",
                contentLength: 0L,
                meta: null,
                created: null);

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, releaseFile.Path()))
                .ReturnsAsync(blob);

            blobStorageService.Setup(mock =>
                    mock.DownloadToStream(PrivateReleaseFiles, releaseFile.Path(),
                        It.IsAny<MemoryStream>()))
                .ReturnsAsync(new MemoryStream());

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Stream(release.Id, releaseFile.File.Id);

                Assert.True(result.IsRight);

                blobStorageService.Verify(
                    mock => mock.GetBlob(PrivateReleaseFiles, releaseFile.Path()),
                    Times.Once());

                blobStorageService.Verify(
                    mock => mock.DownloadToStream(PrivateReleaseFiles, releaseFile.Path(),
                        It.IsAny<MemoryStream>()), Times.Once());

                Assert.Equal("application/pdf", result.Right.ContentType);
                Assert.Equal("ancillary.pdf", result.Right.FileDownloadName);
                Assert.IsType<MemoryStream>(result.Right.FileStream);
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Stream_MixedCaseFilename()
        {
            var release = new Release();

            var releaseFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "Ancillary 1.pdf",
                    Type = Ancillary
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddAsync(releaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            var blob = new BlobInfo(
                path: null,
                size: null,
                contentType: "application/pdf",
                contentLength: 0L,
                meta: null,
                created: null);

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles, releaseFile.Path()))
                .ReturnsAsync(blob);

            blobStorageService.Setup(mock =>
                    mock.DownloadToStream(PrivateReleaseFiles, releaseFile.Path(),
                        It.IsAny<MemoryStream>()))
                .ReturnsAsync(new MemoryStream());

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Stream(release.Id, releaseFile.File.Id);

                Assert.True(result.IsRight);

                blobStorageService.Verify(
                    mock => mock.GetBlob(PrivateReleaseFiles, releaseFile.Path()),
                    Times.Once());

                blobStorageService.Verify(
                    mock => mock.DownloadToStream(PrivateReleaseFiles, releaseFile.Path(),
                        It.IsAny<MemoryStream>()), Times.Once());

                Assert.Equal("application/pdf", result.Right.ContentType);
                Assert.Equal("Ancillary 1.pdf", result.Right.FileDownloadName);
                Assert.IsType<MemoryStream>(result.Right.FileStream);
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Stream_ReleaseNotFound()
        {
            var release = new Release();

            var releaseFile = new ReleaseFile
            {
                Release = release,
                File = new File
                {
                    RootPath = Guid.NewGuid(),
                    Filename = "ancillary.pdf",
                    Type = Ancillary
                }
            };

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.AddAsync(releaseFile);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext())
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Stream(Guid.NewGuid(), releaseFile.File.Id);

                result.AssertNotFound();
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task Stream_FileNotFound()
        {
            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);

            await using (var contentDbContext = InMemoryApplicationDbContext())
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object);

                var result = await service.Stream(release.Id, Guid.NewGuid());

                result.AssertNotFound();
            }

            MockUtils.VerifyAllMocks(blobStorageService);
        }

        [Fact]
        public async Task UploadAncillary()
        {
            const string filename = "ancillary.pdf";
            const string uploadName = "Ancillary Test File";

            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var formFile = CreateFormFileMock(filename).Object;
            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);
            var fileUploadsValidatorService = new Mock<IFileUploadsValidatorService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                mock.UploadFile(PrivateReleaseFiles,
                    It.Is<string>(path =>
                        path.Contains(FilesPath(release.Id, Ancillary))),
                    formFile,
                    It.Is<IDictionary<string, string>>(metadata => 
                        metadata[BlobInfoExtensions.NameKey] == uploadName)
                )).Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles,
                        It.Is<string>(path =>
                            path.Contains(FilesPath(release.Id, Ancillary)))))
                .ReturnsAsync(new BlobInfo(
                    path: "ancillary/file/path",
                    size: "10 Kb",
                    contentType: "application/pdf",
                    contentLength: 0L,
                    meta: GetAncillaryFileMetaValues(
                        name: uploadName),
                    created: null));

            fileUploadsValidatorService.Setup(mock =>
                    mock.ValidateFileForUpload(formFile, Ancillary))
                .ReturnsAsync(Unit.Instance);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object,
                    fileUploadsValidatorService: fileUploadsValidatorService.Object);

                var result = await service.UploadAncillary(release.Id, formFile, uploadName);

                Assert.True(result.IsRight);

                fileUploadsValidatorService.Verify(mock =>
                    mock.ValidateFileForUpload(formFile, Ancillary), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.UploadFile(PrivateReleaseFiles,
                        It.Is<string>(path =>
                            path.Contains(FilesPath(release.Id, Ancillary))),
                        formFile,
                        It.Is<IDictionary<string, string>>(metadata =>
                            metadata[BlobInfoExtensions.NameKey] == uploadName)
                    ), Times.Once);

                blobStorageService.Verify(mock =>
                        mock.GetBlob(PrivateReleaseFiles,
                            It.Is<string>(path =>
                                path.Contains(FilesPath(release.Id, Ancillary)))),
                    Times.Once);

                Assert.True(result.Right.Id.HasValue);
                Assert.Equal("pdf", result.Right.Extension);
                Assert.Equal("ancillary.pdf", result.Right.FileName);
                Assert.Equal("Ancillary Test File", result.Right.Name);
                Assert.Contains(FilesPath(release.Id, Ancillary), result.Right.Path);
                Assert.Equal("10 Kb", result.Right.Size);
                Assert.Equal(Ancillary, result.Right.Type);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var releaseFile = await contentDbContext.ReleaseFiles
                    .Include(rf => rf.File)
                    .SingleOrDefaultAsync(rf =>
                    rf.ReleaseId == release.Id
                    && rf.File.Filename == filename
                    && rf.File.Type == Ancillary
                );

                Assert.NotNull(releaseFile);
            }

            MockUtils.VerifyAllMocks(blobStorageService, fileUploadsValidatorService);
        }

        [Fact]
        public async Task UploadChart()
        {
            const string filename = "chart.png";

            var release = new Release();

            var contentDbContextId = Guid.NewGuid().ToString();

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                await contentDbContext.AddAsync(release);
                await contentDbContext.SaveChangesAsync();
            }

            var formFile = CreateFormFileMock(filename).Object;
            var blobStorageService = new Mock<IBlobStorageService>(MockBehavior.Strict);
            var fileUploadsValidatorService = new Mock<IFileUploadsValidatorService>(MockBehavior.Strict);

            blobStorageService.Setup(mock =>
                mock.UploadFile(PrivateReleaseFiles,
                    It.Is<string>(path =>
                        path.Contains(FilesPath(release.Id, Chart))),
                    formFile,
                    null
                )).Returns(Task.CompletedTask);

            blobStorageService.Setup(mock =>
                    mock.GetBlob(PrivateReleaseFiles,
                        It.Is<string>(path =>
                            path.Contains(FilesPath(release.Id, Chart)))))
                .ReturnsAsync(new BlobInfo(
                    path: "chart/file/path",
                    size: "20 Kb",
                    contentType: "image/png",
                    contentLength: 0L,
                    meta: new Dictionary<string, string>(),
                    created: null));

            fileUploadsValidatorService.Setup(mock =>
                    mock.ValidateFileForUpload(formFile, Chart))
                .ReturnsAsync(Unit.Instance);

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var service = SetupReleaseFileService(contentDbContext: contentDbContext,
                    blobStorageService: blobStorageService.Object,
                    fileUploadsValidatorService: fileUploadsValidatorService.Object);

                var result = await service.UploadChart(release.Id, formFile);

                Assert.True(result.IsRight);

                fileUploadsValidatorService.Verify(mock =>
                    mock.ValidateFileForUpload(formFile, Chart), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.UploadFile(PrivateReleaseFiles, 
                        It.Is<string>(path => 
                            path.Contains(FilesPath(release.Id, Chart))),
                        formFile,
                        null
                    ), Times.Once);

                blobStorageService.Verify(mock =>
                    mock.GetBlob(PrivateReleaseFiles,
                        It.Is<string>(path => 
                            path.Contains(FilesPath(release.Id, Chart)))),
                    Times.Once);

                Assert.True(result.Right.Id.HasValue);
                Assert.Equal("png", result.Right.Extension);
                Assert.Equal("chart.png", result.Right.FileName);
                Assert.Equal("chart.png", result.Right.Name);
                Assert.Contains(FilesPath(release.Id, Chart), result.Right.Path);
                Assert.Equal("20 Kb", result.Right.Size);
                Assert.Equal(Chart, result.Right.Type);
            }

            await using (var contentDbContext = InMemoryApplicationDbContext(contentDbContextId))
            {
                var releaseFile = await contentDbContext.ReleaseFiles
                    .Include(rf => rf.File)
                    .SingleOrDefaultAsync(rf =>
                        rf.ReleaseId == release.Id
                        && rf.File.Filename == filename
                        && rf.File.Type == Chart
                    );

                Assert.NotNull(releaseFile);
            }

            MockUtils.VerifyAllMocks(blobStorageService, fileUploadsValidatorService);
        }

        private static ReleaseFileService SetupReleaseFileService(
            ContentDbContext contentDbContext,
            IPersistenceHelper<ContentDbContext> contentPersistenceHelper = null,
            IBlobStorageService blobStorageService = null,
            IFileRepository fileRepository = null,
            IFileUploadsValidatorService fileUploadsValidatorService = null,
            IReleaseFileRepository releaseFileRepository = null,
            IUserService userService = null)
        {
            return new ReleaseFileService(
                contentDbContext ?? new Mock<ContentDbContext>().Object,
                contentPersistenceHelper ?? new PersistenceHelper<ContentDbContext>(contentDbContext),
                blobStorageService ?? new Mock<IBlobStorageService>().Object,
                fileRepository ?? new FileRepository(contentDbContext),
                fileUploadsValidatorService ?? new Mock<IFileUploadsValidatorService>().Object,
                releaseFileRepository ?? new ReleaseFileRepository(contentDbContext),
                userService ?? MockUtils.AlwaysTrueUserService().Object
            );
        }
    }
}
