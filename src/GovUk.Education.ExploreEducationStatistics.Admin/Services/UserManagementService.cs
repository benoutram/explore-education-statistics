using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Areas.Identity.Data;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UsersAndRolesDbContext _context;

        public UserManagementService(UsersAndRolesDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserViewModel>> ListAsync()
        {
            var users = await _context.Users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    Email = u.Email
                }).OrderBy(x => x.Name)
                .ToListAsync();

            return users;
        }

        public async Task<bool> InviteAsync(string email)
        {
            if (_context.Users.Any(u => u.Email == email) || string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                // TODO: add user to user invite table
                
                // TODO: send invite email via notify
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}