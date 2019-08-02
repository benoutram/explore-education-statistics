using System;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Areas.Tools.Controllers
{
    [Area("Tools")]
    [ApiExplorerSettings(IgnoreApi=true)]
    [Authorize]
    public class FileUploadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IImportService _importService;

        public FileUploadController(ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IImportService importService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _importService = importService;
        }

        public IActionResult Upload()
        {
            ViewData["ReleaseId"] = new SelectList(_context.Releases.OrderBy(r => r.Title), "Id", "Title");
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Post(Guid releaseId, string name, IFormFile file, IFormFile metaFile)
        {
            var release = _context.Releases
                .Where(r => r.Id.Equals(releaseId))
                .Include(r => r.Publication)
                .FirstOrDefault();

            await _fileStorageService.UploadFilesAsync(release.Publication.Slug, release.Slug, file, metaFile, name);

            _importService.Import(file.FileName, release.Id);
            
            return RedirectToAction("List", "File");
        }
    }
}