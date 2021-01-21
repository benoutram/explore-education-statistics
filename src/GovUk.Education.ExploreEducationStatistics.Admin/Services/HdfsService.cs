using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using File = GovUk.Education.ExploreEducationStatistics.Content.Model.File;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class HdfsService : IHdfsService
    {
        public HttpClient Client { get; }

        private const string PutRequest =
            "/webhdfs/v1/tmp/statistics/<releaseId>/<filename>?user.name=admin&op=CREATE" +
            "&namenoderpcaddress=sandbox-hdp.hortonworks.com:8020&createflag=&createparent=true&overwrite=true";

        public HdfsService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://sandbox-hdp.hortonworks.com:50070");
            Client = client;
        }

        public async Task PutFile(File file, IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                var ms = new MemoryStream();
                await formFile.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var ignoreFirstLine = await new StreamReader(ms, Encoding.UTF8).ReadLineAsync();
                var firstLineByteCount = UTF8Encoding.UTF8.GetByteCount(ignoreFirstLine + "\n\r");
                ms.Seek(firstLineByteCount, SeekOrigin.Begin);

                var httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

                var requestUri = PutRequest
                    .Replace("<releaseId>", file.RootPath.ToString())
                    .Replace("<filename>", file.Filename);

                var response = await Client.PutAsync(requestUri, httpContent);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
