using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces;
using static GovUk.Education.ExploreEducationStatistics.Common.BlobContainerNames;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBlobStorageService _blobStorageService;

        public BatchService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<int> GetNumBatchesRemaining(File dataFile)
        {
            var batchFiles = await GetBatchFilesForDataFile(dataFile);
            return batchFiles.Count;
        }

        public async Task<List<BlobInfo>> GetBatchFilesForDataFile(File dataFile)
        {
            var blobs = await _blobStorageService.ListBlobs(
                PrivateFilesContainerName,
                dataFile.BatchesPath());

            return blobs.ToList();
        }
    }
}
