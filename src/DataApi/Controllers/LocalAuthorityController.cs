using System.Collections.Generic;
using System.Linq;
using DataApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers
{
    [Route("data/{publication}/geo-levels/local-authority")]
    [ApiController]
    public class LocalAuthorityController : ControllerBase
    {
        private readonly ICsvReader _csvReader;

        public LocalAuthorityController(ICsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        [HttpGet]
        public ActionResult<List<GeographicModel>> List(string publication,
            [FromQuery(Name = "schoolType")] string schoolType,
            [FromQuery(Name = "attributes")] List<string> attributes)
        {
            return _csvReader.GeoLevels(publication + "_geoglevels")
                .Where(x => x.Year == 201617 && x.Level.ToLower() == "local authority").ToList();
        }

        [HttpGet("{localAuthorityId}")]
        public ActionResult<GeographicModel> Get(string publication, string localAuthorityId,
            [FromQuery(Name = "schoolType")] string schoolType,
            [FromQuery(Name = "attributes")] List<string> attributes)
        {
            return _csvReader.GeoLevels(publication + "_geoglevels").FirstOrDefault(x =>
                (x.LocalAuthority.Code == localAuthorityId || x.LocalAuthority.Old_Code == localAuthorityId));
        }

        [HttpGet("{localAuthorityId}/schools")]
        public ActionResult<List<GeographicModel>> GetSchools(string publication, string localAuthorityId,
            [FromQuery(Name = "schoolType")] string schoolType,
            [FromQuery(Name = "attributes")] List<string> attributes)
        {
            return _csvReader.GeoLevels(publication + "_geoglevels").Where(x =>
                (x.LocalAuthority.Code == localAuthorityId || x.LocalAuthority.Old_Code == localAuthorityId) &&
                x.Level.ToLower() == "school").ToList();
        }
    }
}