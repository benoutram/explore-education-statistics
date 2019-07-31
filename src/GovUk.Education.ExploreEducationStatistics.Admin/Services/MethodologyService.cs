﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using Microsoft.EntityFrameworkCore;
using static GovUk.Education.ExploreEducationStatistics.Admin.Services.ModelMappers;
using TopicId = System.Guid;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class MethodologyService : IMethodologyService
    {
        private readonly ApplicationDbContext _context;

        public MethodologyService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<MethodologyViewModel>> ListAsync()
        {
            var result = await _context.Methodologies.ToListAsync();
            return MethodologyViewModelMapper.Map<List<MethodologyViewModel>>(result);
            
        }
        
        public async Task<List<MethodologyViewModel>> GetTopicMethodologiesAsync(TopicId topicId)
        {
            var methodologies = await _context.Publications
                .Where(p => p.TopicId == topicId)
                .Include(p => p.Methodology)
                .Select(p => p.Methodology)
                .Distinct()
                .ToListAsync();
            return MethodologyViewModelMapper.Map<List<MethodologyViewModel>>(methodologies);
        }
    }
}