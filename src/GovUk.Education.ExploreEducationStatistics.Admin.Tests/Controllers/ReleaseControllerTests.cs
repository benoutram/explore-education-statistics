using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Models;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationErrorMessages;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationUtils;
using FileInfo = GovUk.Education.ExploreEducationStatistics.Admin.Models.FileInfo;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Controllers
{
    public class ReleaseControllerTests
    {
        [Fact]
        public async void Create_Release_Returns_Ok()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();


            releaseService.Setup(s => s.CreateReleaseAsync(It.IsAny<CreateReleaseViewModel>()))
                .Returns(Task.FromResult(new ReleaseViewModel()));
            var controller =
                new ReleasesController(releaseService.Object, fileStorageService.Object, importService.Object);
            releaseService.Setup(s => s.CreateReleaseAsync(It.IsAny<CreateReleaseViewModel>()))
                .Returns(Task.FromResult(new ReleaseViewModel()));
        }
        
        [Fact]
        public async Task AddAncillaryFilesAsync_UploadsTheFiles_Returns_Ok()
        {
            var releaseId = Guid.NewGuid();
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();

            var ancillaryFile = MockFile("ancillaryFile.doc");

            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService
                .Setup(service => service.UploadFilesAsync(releaseId, ancillaryFile,"File name", ReleaseFileTypes.Ancillary))
                .Returns(Task.FromResult<Either<ValidationResult,IEnumerable<FileInfo>>>(new List<FileInfo>()));

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test
            var actionResult = await controller.AddAncillaryFilesAsync(releaseId, "File name", ancillaryFile);

            Assert.IsAssignableFrom<ActionResult<IEnumerable<FileInfo>>>(actionResult);
        }

        [Fact]
        public async Task AddAncillaryFilesAsync_UploadsTheFiles_Returns_NotFound()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();

            var ancillaryFile = MockFile("ancillaryFile.doc");

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test
            var actionResult = await controller.AddAncillaryFilesAsync(Guid.NewGuid(),  "File name", ancillaryFile );

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetAncillaryFilesAsync_Returns_A_List_Of_Files()
        {
            var releaseId = Guid.NewGuid();
            IEnumerable<FileInfo> testFiles = new []
            {
                new FileInfo
                {
                    Extension = "doc",
                    Name = "Ancillary 1",
                    Path = "file1.doc",
                    Size = "1 Kb"
                },
                new FileInfo
                {
                    Extension = "doc",
                    Name = "Ancillary 2",
                    Path = "file2.doc",
                    Size = "1 Kb"
                }
            };

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();


            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService.Setup(s => s.ListFilesAsync(releaseId, ReleaseFileTypes.Ancillary)).Returns(Task.FromResult(testFiles));


            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);
            // Call the method under test
            var actionResult = await controller.GetAncillaryFilesAsync(releaseId);

            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetAncillaryFilesAsync_Returns_NotFound()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test 
            var actionResult = await controller.GetAncillaryFilesAsync(Guid.NewGuid());

            Assert.Null(actionResult.Value);
            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public async Task AddDataFilesAsync_UploadsTheFiles_Returns_Ok()
        {
            var releaseId = Guid.NewGuid();

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();

            var dataFile = MockFile("datafile.csv");
            var metaFile = MockFile("metafile.csv");
            
            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService
                .Setup(service => service.UploadDataFilesAsync(releaseId, dataFile, metaFile, "Subject name"))
                .Returns(Task.FromResult<Either<ValidationResult, IEnumerable<FileInfo>>>(new List<FileInfo>()));

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            var actionResult = await controller.AddDataFilesAsync(releaseId, "Subject name", dataFile, metaFile);

            Assert.IsAssignableFrom<ActionResult<IEnumerable<FileInfo>>>(actionResult);
        }

        [Fact]
        public async Task AddDataFilesAsync_UploadsTheFiles_Returns_NotFound()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();

            var dataFile = MockFile("datafile.csv");
            var metaFile = MockFile("metafile.csv");

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            var actionResult = await controller.AddDataFilesAsync(Guid.NewGuid(), "Subject name", dataFile, metaFile);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public async Task AddDataFilesAsync_UploadsTheFiles_Returns_ValidationProblem()
        {
            var releaseId = Guid.NewGuid();

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();

            var dataFile = MockFile("datafile.csv");
            var metaFile = MockFile("metafile.csv");
            
            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService
                .Setup(service => service.UploadDataFilesAsync(releaseId, dataFile, metaFile, "Subject name"))
                .Returns(Task.FromResult<Either<ValidationResult, IEnumerable<FileInfo>>>(ValidationResult(CannotOverwriteFile)));

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test
            var actionResult = await controller.AddDataFilesAsync(releaseId, "Subject name", dataFile, metaFile);

            Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult.Result);
            Assert.IsAssignableFrom<ValidationProblemDetails>((actionResult.Result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task GetDataFilesAsync_Returns_A_List_Of_Files()
        {
            var releaseId = Guid.NewGuid();
            IEnumerable<FileInfo> testFiles = new []
            {
                new FileInfo
                {
                    Extension = "csv",
                    Name = "Release a file 1",
                    Path = "file1.csv",
                    Size = "1 Kb"
                },
                new FileInfo
                {
                    Extension = "csv",
                    Name = "Release a file 2",
                    Path = "file2.csv",
                    Size = "1 Kb"
                }
            };

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();


            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService.Setup(s => s.ListFilesAsync(releaseId, ReleaseFileTypes.Data)).Returns(Task.FromResult(testFiles));


            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            var actionResult = await controller.GetDataFilesAsync(releaseId);

            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetDataFilesAsync_Returns_NotFound()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            var actionResult = await controller.GetDataFilesAsync(Guid.NewGuid());

            Assert.Null(actionResult.Value);
            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public async Task DeleteDataFilesAsync_Returns_OK()
        {
            var releaseId = Guid.NewGuid();

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            
            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService
                .Setup(service => service.DeleteDataFileAsync(releaseId, "datafilename"))
                .Returns(Task.FromResult<Either<ValidationResult, IEnumerable<FileInfo>>>(new List<FileInfo>()));

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test
            var actionResult = await controller.DeleteDataFiles(releaseId, "datafilename");

            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }
        
        [Fact]
        public async Task DeleteDataFilesAsync_Returns_ValidationProblem()
        {
            var releaseId = Guid.NewGuid();

            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            
            releaseService.Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Release {Id = releaseId}));
            fileStorageService
                .Setup(service => service.DeleteDataFileAsync(releaseId, "datafilename"))
                .Returns(Task.FromResult<Either<ValidationResult, IEnumerable<FileInfo>>>(ValidationResult(UnableToFindMetadataFileToDelete)));

            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object,
                importService.Object);

            // Call the method under test
            var actionResult = await controller.DeleteDataFiles(releaseId, "datafilename");

            Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult.Result);
            Assert.IsAssignableFrom<ValidationProblemDetails>((actionResult.Result as BadRequestObjectResult)?.Value);
        }
        
        
        [Fact]
        public async void Edit_Release_Summary_Returns_Ok()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            var releaseId = new Guid("95bf7743-fe6f-4b85-a28f-49f6f6b8735a");

            releaseService
                .Setup(s => s.EditReleaseSummaryAsync(It.IsAny<EditReleaseSummaryViewModel>()))
                .Returns<EditReleaseSummaryViewModel>(e => Task.FromResult(new ReleaseViewModel
                {
                    Id = e.Id
                }));
            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object, importService.Object);

            // Method under test
            var result = await controller.EditReleaseSummaryAsync(new EditReleaseSummaryViewModel(), releaseId);
            Assert.IsAssignableFrom<ReleaseViewModel>(result.Value);
            Assert.Equal(result.Value.Id, releaseId);
        }
        
        [Fact]
        public async void Get_Release_Summary_Returns_Ok()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            var releaseId = new Guid("95bf7743-fe6f-4b85-a28f-49f6f6b8735a");
            releaseService
                .Setup(s => s.GetReleaseSummaryAsync(It.IsAny<Guid>()))
                .Returns<Guid>(id => Task.FromResult(new EditReleaseSummaryViewModel{Id = id}));
            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object, importService.Object);

            // Method under test
            var result = await controller.GetReleaseSummaryAsync(releaseId);
            Assert.IsAssignableFrom<EditReleaseSummaryViewModel>(result.Value);
            Assert.Equal(result.Value.Id, releaseId);
        }

        [Fact]
        public async void Get_Releases_For_Publication_Returns_Ok()
        {
            var releaseService = new Mock<IReleaseService>();
            var fileStorageService = new Mock<IFileStorageService>();
            var importService = new Mock<IImportService>();
            var releaseId = new Guid("fc570a6c-d230-40ae-a5e5-febab330fb12");
            releaseService
                .Setup(s => s.GetReleasesForPublicationAsync(It.Is<Guid>(id => id == releaseId)))
                .Returns<Guid>(x => Task.FromResult(new List<ReleaseViewModel>()));
            var controller = new ReleasesController(releaseService.Object, fileStorageService.Object, importService.Object);

            // Method under test
            var result = await controller.GetReleaseForPublicationAsync(releaseId);
            Assert.IsAssignableFrom<List<ReleaseViewModel>>(result.Value);
        }            
        
        private static IFormFile MockFile(string fileName)
        {
            var fileMock = new Mock<IFormFile>();
            const string content = "test content";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            return fileMock.Object;
        }
    }
}