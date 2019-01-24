using System;
using System.Collections.Generic;
using GovUk.Education.ExploreEducationStatistics.Data.Api.Models.Query;
using GovUk.Education.ExploreEducationStatistics.Data.Api.Models.TableBuilder;
using GovUk.Education.ExploreEducationStatistics.Data.Api.Services.TableBuilder;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TableBuilderController : ControllerBase
    {
        private readonly TableBuilderService _tableBuilderService;
        
        public TableBuilderController(TableBuilderService tableBuilderService)
        {
            _tableBuilderService = tableBuilderService;
        }

        [HttpGet("geographic/{publicationId}/{schoolType}/{level}")]
        public ActionResult<TableBuilderResult> GetGeographic(Guid publicationId,
            SchoolType schoolType,
            [FromQuery(Name = "year")] ICollection<int> years,
            [FromQuery(Name = "attribute")] ICollection<string> attributes,
            string level = "National")
        {   
            var query = new GeographicQueryContext
            {
                Attributes = attributes,
                Level = Levels.getLevel(level),
                PublicationId = publicationId,
                SchoolType = schoolType,
                Years = years
            };
            
            return _tableBuilderService.GetGeographic(query);
        }
        
        [HttpGet("characteristics/local-authority/{publicationId}/{schoolType}")]
        public ActionResult<TableBuilderResult> GetLocalAuthority(Guid publicationId,
            SchoolType schoolType,
            [FromQuery(Name = "year")] ICollection<int> years,
            [FromQuery(Name = "attribute")] ICollection<string> attributes,
            [FromQuery(Name = "characteristic")] ICollection<string> characteristics)
        {
            var query = new LaQueryContext
            {
                Attributes = attributes,
                Characteristics = characteristics,
                PublicationId = publicationId,
                SchoolType = schoolType,
                Years = years
            };
            
            return _tableBuilderService.GetLocalAuthority(query);
        }
        
        [HttpGet("characteristics/national/{publicationId}/{schoolType}")]
        public ActionResult<TableBuilderResult> GetNational(Guid publicationId,
            SchoolType schoolType,
            [FromQuery(Name = "year")] ICollection<int> years,
            [FromQuery(Name = "attribute")] ICollection<string> attributes,
            [FromQuery(Name = "characteristic")] ICollection<string> characteristics)
        {
            var query = new NationalQueryContext
            {
                Attributes = attributes,
                Characteristics = characteristics,
                PublicationId = publicationId,
                SchoolType = schoolType,
                Years = years
            };
            
            return _tableBuilderService.GetNational(query);
        }
    }
}