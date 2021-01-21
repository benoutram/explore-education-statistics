using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using Microsoft.AspNetCore.Http;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces
{
    public interface IHdfsService
    {
        Task PutFile(File file, IFormFile formFile);
    }
}