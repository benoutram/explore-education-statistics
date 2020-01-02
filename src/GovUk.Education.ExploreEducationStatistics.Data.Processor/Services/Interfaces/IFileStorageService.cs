using System;
using System.IO;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Models;
using Microsoft.Azure.Storage.Blob;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<SubjectData> GetSubjectData(ImportMessage importMessage);

        Task<bool> UploadDataFileAsync(
            Guid releaseId,
            MemoryStream stream,
            string metaFileName,
            string name,
            string fileName,
            string contentType,
            int numRows);

        void DeleteDatafile(string releaseId, string dataFileName);

        void DeleteBatches(string releaseId, string dataFileName);

        CloudBlockBlob GetBlobReference(string releaseId, string dataFileName);

        Task<string> GetLeaseId(CloudBlockBlob cloudBlockBlob);
    }
}