using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Publisher.Models;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task CopyReleaseFilesToPublicContainer(CopyReleaseFilesCommand copyReleaseFilesCommand);

        Task DeleteAllContentAsyncExcludingStaging();

        Task DeleteDownloadFilesForPreviousVersion(Release release);

        Task DeletePublicBlobs(string directoryPath, string excludePattern = null);

        Task DeletePublicBlob(string path);

        Task<(CloudBlockBlob blob, string id)> AcquireLease(string blobName);

        Task<IEnumerable<FileInfo>> ListPublicFiles(string publication, string release);

        Task MovePublicDirectory(string containerName, string sourceDirectoryPath, string destinationDirectoryPath);

        Task UploadAsJson(string filePath, object value, JsonSerializerSettings settings = null);
    }
}