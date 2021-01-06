using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Areas.Identity.Data;
using GovUk.Education.ExploreEducationStatistics.Admin.Areas.Identity.Data.Models;
using GovUk.Education.ExploreEducationStatistics.Admin.Models;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationUtils;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationErrorMessages;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UsersAndRolesDbContext _usersAndRolesDbContext;
        private readonly IUserService _userService;
        private readonly ContentDbContext _contentDbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersistenceHelper<ContentDbContext> _persistenceHelper;

        public UserManagementService(
            UsersAndRolesDbContext usersAndRolesDbContext,
            IUserService userService,
            ContentDbContext contentDbContext,
            IEmailService emailService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IPersistenceHelper<ContentDbContext> persistenceHelper)
        {
            _usersAndRolesDbContext = usersAndRolesDbContext;
            _userService = userService;
            _contentDbContext = contentDbContext;
            _emailService = emailService;
            _configuration = configuration;
            _userManager = userManager;
            _persistenceHelper = persistenceHelper;
        }

        public async Task<Either<ActionResult, List<UserViewModel>>> ListAllUsers()
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(() =>
                {
                    return _usersAndRolesDbContext.Users
                        .Join(
                            _usersAndRolesDbContext.UserRoles,
                            user => user.Id,
                            userRole => userRole.UserId,
                            (user, userRole) => new
                            {
                                user,
                                userRoleId = userRole.RoleId
                            }
                        )
                        .Join(
                            _usersAndRolesDbContext.Roles,
                            prev => prev.userRoleId,
                            role => role.Id,
                            (prev, role) => new UserViewModel
                            {
                                Id = prev.user.Id,
                                Name = prev.user.FirstName + " " + prev.user.LastName,
                                Email = prev.user.Email,
                                Role = role.Name
                            }
                        )
                        .Where(uvm => uvm.Role != "Prerelease User")
                        .OrderBy(uvm => uvm.Name)
                        .ToListAsync();
                });
        }

        public async Task<Either<ActionResult, UserReleaseRoleViewModel>> AddUserReleaseRole(Guid userId,
            UserReleaseRoleRequest userReleaseRole)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(async () =>
                {
                    return await _persistenceHelper
                        .CheckEntityExists<Release>(userReleaseRole.ReleaseId)
                        .OnSuccess(() => ValidateUserReleaseRoleCanBeAdded(userId, userReleaseRole))
                        .OnSuccess(async () =>
                        {
                            var newReleaseRole = new UserReleaseRole
                            {
                                ReleaseId = userReleaseRole.ReleaseId,
                                Role = userReleaseRole.ReleaseRole,
                                UserId = userId
                            };

                            await _contentDbContext.AddAsync(newReleaseRole);
                            await _contentDbContext.SaveChangesAsync();

                            var response = await GetUserReleaseRole(newReleaseRole.Id);

                            SendNewReleaseRoleEmail(userId, response.Publication, response.Release,
                                response.ReleaseRole);

                            return response;
                        });
                });
        }

        public async Task<Either<ActionResult, Unit>> RemoveUserReleaseRole(Guid userReleaseRoleId)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(async () =>
                {
                    return await _persistenceHelper
                        .CheckEntityExists<UserReleaseRole>(userReleaseRoleId)
                        .OnSuccessVoid(async userReleaseRole =>
                        {
                            _contentDbContext.Remove(userReleaseRole);
                            await _contentDbContext.SaveChangesAsync();
                        });
                });
        }

        public async Task<Either<ActionResult, List<IdTitlePair>>> ListReleases()
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(_ =>
                {
                    return _contentDbContext.Releases
                        .Include(r => r.Publication)
                        .ToList()
                        .Where(r => IsLatestVersionOfRelease(r.Publication.Releases, r.Id))
                        .Select(r => new IdTitlePair
                        {
                            Id = r.Id,
                            Title = $"{r.Publication.Title} - {r.Title}",
                        })
                        .ToList();
                });
        }

        public async Task<Either<ActionResult, List<RoleViewModel>>> ListRoles()
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(async () =>
                {
                    return await _usersAndRolesDbContext.Roles.Select(r => new RoleViewModel()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            NormalizedName = r.NormalizedName
                        }).OrderBy(x => x.Name)
                        .ToListAsync();
                });
        }

        public Task<Either<ActionResult, List<EnumExtensions.EnumValue>>> ListReleaseRoles()
        {
            return _userService
                .CheckCanManageAllUsers()
                .OnSuccess(_ => EnumExtensions.GetValues<ReleaseRole>());
        }

        public async Task<List<UserViewModel>> ListPreReleaseUsersAsync()
        {
            var users = await _usersAndRolesDbContext.Users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    Email = u.Email
                }).OrderBy(x => x.Name)
                .ToListAsync();

            // Potentially user role could be null in the above result in an empty array so assign role afterwards
            foreach (var user in users)
            {
                user.Role = GetUserRoleName(user.Id);
            }

            return users.Where(u => u.Role == "Prerelease User").ToList();
        }

        public async Task<Either<ActionResult, UserViewModel>> GetUser(string userId)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess<ActionResult, Unit, UserViewModel>(async () =>
                {
                    var user = await _usersAndRolesDbContext.Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (user == null)
                    {
                        return ValidationActionResult(UserDoesNotExist);
                    }

                    var userReleaseRoles = await GetUserReleaseRoles(user.Id);

                    return new UserViewModel
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.Email,
                        Role = GetUserRoleId(user.Id),
                        UserReleaseRoles = userReleaseRoles.IsRight ? userReleaseRoles.Right : null
                    };
                });
        }

        public async Task<Either<ActionResult, List<UserViewModel>>> ListPendingInvites()
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(async () =>
                    await _usersAndRolesDbContext.UserInvites
                        .Where(ui => !ui.Accepted)
                        .OrderBy(ui => ui.Email)
                        .Include(ui => ui.Role)
                        .Select(ui => new UserViewModel
                        {
                            Email = ui.Email,
                            Role = ui.Role.Name
                        }).ToListAsync()
                );
        }

        public async Task<Either<ActionResult, UserInvite>> InviteUser(string email, string inviteCreatedByUser,
            string roleId)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess<ActionResult, Unit, UserInvite>(async () =>
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        return ValidationActionResult(InvalidEmailAddress);
                    }

                    if (_usersAndRolesDbContext.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                    {
                        return ValidationActionResult(UserAlreadyExists);
                    }

                    var role = await _usersAndRolesDbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (role == null)
                    {
                        return ValidationActionResult(InvalidUserRole);
                    }

                    var invite = new UserInvite
                    {
                        Email = email.ToLower(),
                        Created = DateTime.UtcNow,
                        CreatedBy = inviteCreatedByUser,
                        Role = role
                    };
                    await _usersAndRolesDbContext.UserInvites.AddAsync(invite);
                    await _usersAndRolesDbContext.SaveChangesAsync();
                    SendInviteEmail(email);
                    return invite;
                });
        }

        public async Task<Either<ActionResult, Unit>> CancelInvite(string email)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess<ActionResult, Unit, Unit>(async () =>
                {
                    var invite = await _usersAndRolesDbContext.UserInvites.FirstOrDefaultAsync(i => i.Email == email);
                    if (invite == null)
                    {
                        return ValidationActionResult(InviteNotFound);
                    }

                    _usersAndRolesDbContext.UserInvites.Remove(invite);
                    await _usersAndRolesDbContext.SaveChangesAsync();

                    return Unit.Instance;
                });
        }

        public async Task<Either<ActionResult, Unit>> UpdateUser(string userId, string roleId)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess<ActionResult, Unit, Unit>(async () =>
                {
                    var user = await _usersAndRolesDbContext.Users.FirstOrDefaultAsync(i => i.Id == userId);
                    if (user == null)
                    {
                        return ValidationActionResult(UserDoesNotExist);
                    }

                    var userRole = await _usersAndRolesDbContext.UserRoles.FirstOrDefaultAsync(i => i.UserId == userId);
                    if (userRole == null)
                    {
                        return ValidationActionResult(InvalidUserRole);
                    }

                    await _userManager.RemoveFromRoleAsync(user, GetRoleName(userRole.RoleId));
                    await _userManager.AddToRoleAsync(user, GetRoleName(roleId));

                    return Unit.Instance;
                });
        }

        public async Task<Either<ActionResult, List<UserReleaseRoleViewModel>>> GetUserReleaseRoles(string userId)
        {
            return await _userService
                .CheckCanManageAllUsers()
                .OnSuccess(_ =>
                {
                    return _contentDbContext.UserReleaseRoles
                        .Include(urr => urr.Release)
                        .ThenInclude(r => r.Publication)
                        .ToList()
                        .Where(urr => urr.UserId == Guid.Parse(userId) &&
                                      IsLatestVersionOfRelease(urr.Release.Publication.Releases, urr.Release.Id))
                        .Select(x => new UserReleaseRoleViewModel
                        {
                            Id = x.Id,
                            Publication = _contentDbContext.Publications
                                .Where(p => p.Releases.Any(r => r.Id == x.ReleaseId))
                                .Select(p => new IdTitlePair {Id = p.Id, Title = p.Title}).FirstOrDefault(),
                            Release = _contentDbContext.Releases
                                .Where(r => r.Id == x.ReleaseId)
                                .Select(r => new IdTitlePair {Id = r.Id, Title = r.Title}).FirstOrDefault(),
                            ReleaseRole = new EnumExtensions.EnumValue {Name = x.Role.GetEnumLabel(), Value = 0}
                        })
                        .ToList()
                        .OrderBy(x => x.Publication.Title)
                        .ThenBy(x => x.Release.Title)
                        .ToList();
                });
        }

        private async Task<UserReleaseRoleViewModel> GetUserReleaseRole(Guid userReleaseRoleId)
        {
            return await _contentDbContext.UserReleaseRoles
                .Where(x => x.Id == userReleaseRoleId)
                .Select(x => new UserReleaseRoleViewModel
                {
                    Id = x.Id,
                    Publication = _contentDbContext.Publications
                        .Where(p => p.Releases.Any(r => r.Id == x.ReleaseId))
                        .Select(p => new IdTitlePair {Id = p.Id, Title = p.Title}).FirstOrDefault(),
                    Release = _contentDbContext.Releases
                        .Where(r => r.Id == x.ReleaseId)
                        .Select(r => new IdTitlePair {Id = r.Id, Title = r.Title}).FirstOrDefault(),
                    ReleaseRole = new EnumExtensions.EnumValue {Name = x.Role.GetEnumLabel(), Value = 0}
                }).FirstOrDefaultAsync();
        }

        private string GetRoleName(string roleId)
        {
            var userRole = _usersAndRolesDbContext.Roles.FirstOrDefault(r => r.Id == roleId);

            return userRole?.Name;
        }

        private string GetUserRoleName(string userId)
        {
            var userRole = _usersAndRolesDbContext.UserRoles.FirstOrDefault(r => r.UserId == userId);

            return userRole == null
                ? null
                : _usersAndRolesDbContext.Roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name;
        }

        private string GetUserRoleId(string userId)
        {
            var userRole = _usersAndRolesDbContext.UserRoles.FirstOrDefault(r => r.UserId == userId);

            return userRole?.RoleId;
        }

        private void SendInviteEmail(string email)
        {
            var uri = _configuration.GetValue<string>("AdminUri");
            var template = _configuration.GetValue<string>("NotifyInviteTemplateId");


            var emailValues = new Dictionary<string, dynamic> {{"url", "https://" + uri}};

            _emailService.SendEmail(email, template, emailValues);
        }

        private void SendNewReleaseRoleEmail(Guid userId, IdTitlePair publication, IdTitlePair release,
            EnumExtensions.EnumValue role)
        {
            var uri = _configuration.GetValue<string>("AdminUri");
            var template = _configuration.GetValue<string>("NotifyReleaseRoleTemplateId");
            var email = _usersAndRolesDbContext.Users
                .First(x => x.Id == userId.ToString())
                .Email;

            var link = (role.Name == ReleaseRole.PrereleaseViewer.GetEnumLabel() ? "prerelease " : "summary");
            var emailValues = new Dictionary<string, dynamic>
            {
                {"url", $"https://{uri}/publication/{publication.Id}/release/{release.Id}/{link}"},
                {"role", role.Name},
                {"publication", publication.Title},
                {"release", release.Title}
            };

            _emailService.SendEmail(email, template, emailValues);
        }

        private async Task<Either<ActionResult, bool>> ValidateUserReleaseRoleCanBeAdded(Guid userId,
            UserReleaseRoleRequest userReleaseRole)
        {
            var existing = await _contentDbContext.UserReleaseRoles.FirstOrDefaultAsync(r =>
                r.UserId == userId && r.ReleaseId == userReleaseRole.ReleaseId &&
                r.Role == userReleaseRole.ReleaseRole);

            if (existing != null)
            {
                return ValidationActionResult(UserAlreadyHasReleaseRole);
            }

            return true;
        }

        private static bool IsLatestVersionOfRelease(IEnumerable<Release> releases, Guid releaseId)
        {
            return !releases.Any(r => r.PreviousVersionId == releaseId && r.Id != releaseId);
        }
    }
}
