using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Services;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Models;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services
{
    public class SplitFileService : ISplitFileService
    {
        private readonly IFileStorageService _fileStorageService;

        public SplitFileService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public void SplitDataFile(
            ICollector<ImportMessage> collector,
            ImportMessage message,
            SubjectData subjectData,
            BatchSettings batchSettings)
        {
            var batchCount = 1;
            var lines = subjectData.GetCsvLines();

            if (lines.Count() > batchSettings.RowsPerBatch + 1)
            {
                List<IFormFile> files = SplitFile(message, lines, batchSettings.RowsPerBatch);

                files.ForEach(async f =>
                {
                    await _fileStorageService.UploadDataFileAsync(message.Release.Id,
                        f, BlobUtils.GetMetaFileName(subjectData.DataBlob), BlobUtils.GetName(subjectData.DataBlob));

                    var iMessage = new ImportMessage
                    {
                        DataFileName = f.FileName,
                        Release = message.Release,
                        BatchNo = batchCount++,
                        NumBatches = files.Count,
                        RowsPerBatch = batchSettings.RowsPerBatch
                    };

                    collector.Add(iMessage);
                });
            }
            // Else perform any additional validation & pass on file to message queue for import
            else
            {
                collector.Add(message); 
            }
        }
        
        private static List<IFormFile> SplitFile(
            ImportMessage message,
            IEnumerable<string> csvLines,
            int rowsPerBatch)
        {
            var files = new List<IFormFile>();    
            var header = csvLines.First();
            var batches = csvLines.Skip(1).Batch(rowsPerBatch);
            var index = 1;
            
            foreach (var batch in batches)
            {
                var lines = batch.ToList();
                var mStream = new MemoryStream();
                var writer = new StreamWriter(mStream);
                writer.Flush();
                
                // Insert the header at the beginning of each file/batch
                writer.WriteLine(header);
                
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                    writer.Flush();
                }
                
                var f = new FormFile(mStream, 0, mStream.Length, message.DataFileName,
                    $"{FileStoragePathUtils.BatchesDir}/{message.DataFileName}_{index++:000000}") 
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };
                files.Add(f);
            }
            return files;
        }
        public static int GetNumBatches(int rows, int rowsPerBatch) {
            return (int)Math.Ceiling(rows / (double)rowsPerBatch);
        }
    }
}