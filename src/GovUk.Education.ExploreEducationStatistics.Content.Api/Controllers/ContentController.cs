using System.Collections.Generic;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Content.Api.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Content.Api.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Content.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IPublicationService _publicationService;
        private readonly IReleaseService _releaseService;

        public ContentController(
            IContentService contentService, 
            IPublicationService publicationService,
            IReleaseService releaseService)
        {
            _contentService = contentService;
            _publicationService = publicationService;
            _releaseService = releaseService;
        }
        
        // GET
        [HttpGet("tree")]
        public ActionResult<List<ThemeTree>> GetContentTree()
        {
            var tree = _contentService.GetContentTree();
            
            if (tree.Any())
            {
                return tree;
            }

            return NoContent();
        }

        // GET api/publication/5
        [HttpGet("publication/{id}")]
        public ActionResult<Publication> GetPublication(string id)
        {
            var publication = _publicationService.GetPublication(id);
            
            if (publication != null)
            {
                return publication;
            }
            return NotFound();
        }

        // GET api/publication/5/latest
        [HttpGet("publication/{id}/latest")]
        public ActionResult<Release> GetLatestRelease(string id)
        {
            var release = _releaseService.GetLatestRelease(id);

            if (release != null)
            {
                return release;
            }
            return NotFound();

        }

        // GET api/release/5
        [HttpGet("release/{id}")]
        public ActionResult<Release> GetRelease(string id)
        {
            var release = _releaseService.GetRelease(id);
            
            if (release != null)
            {
                return release;
            }
            return NotFound();
        }
    }
}