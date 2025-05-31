using System.Globalization;
using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.SharedKernel.User;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Shared.Persistence;

public static class Seed
{
    #region ApplicationUser

    public static void ApplicationUserData(ModelBuilder modelBuilder)
    {
        // Convert shared users to domain users
        var sharedUsers = UserSharedData.Users;
        List<Domain.ApplicationUsers.ApplicationUser> domainUsers = MapToDomainUsers(sharedUsers);

        // Seed users
        modelBuilder.Entity<Domain.ApplicationUsers.ApplicationUser>().HasData(domainUsers);
        

        // Seed roles
        List<ApplicationRole> roles = GetRolesData();
        modelBuilder.Entity<ApplicationRole>().HasData(roles);


        // Create and seed user roles
        var userRoles = CreateUserRoles(domainUsers, roles);
        modelBuilder.Entity<UserRole>().HasData(userRoles);

    }

    private static List<ApplicationRole> GetRolesData()
    {
        return RoleName.GetAllRoles().Select(roleName => new ApplicationRole(roleName)
        {
            Id = Guid.NewGuid(),
            Name = roleName,
            NormalizedName = roleName.ToUpper(CultureInfo.InvariantCulture),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }).ToList();
    }

    private static Domain.ApplicationUsers.ApplicationUser MapToDomainUser(UserSharedDto sharedUser)
    {
        var domainUser = Domain.ApplicationUsers.ApplicationUser.Create(
            sharedUser.FullName,
            sharedUser.Email,
            sharedUser.PhoneNumber,
            sharedUser.Password).Value;

        domainUser.Id = sharedUser.Id;

        return domainUser;
    }

    private static List<Domain.ApplicationUsers.ApplicationUser> MapToDomainUsers(List<UserSharedDto> sharedUsers)
    {
        return sharedUsers.ToList().ConvertAll(MapToDomainUser);
    }

    private static List<Domain.ApplicationUsers.ApplicationUser> MapToDomainContractorUsers(IEnumerable<UserSharedDto> sharedUsers)
    {
        return sharedUsers.ToList().ConvertAll(MapToDomainUser);
    }

    private static IEnumerable<UserRole> CreateUserRoles(IEnumerable<Domain.ApplicationUsers.ApplicationUser> users, IEnumerable<ApplicationRole> roles)
    {
        var userDictionary = users.ToDictionary(u => u.FullName);
        var roleDictionary = roles.ToDictionary(r => r.Name);

        return new List<UserRole>()
        {
            new() { UserId = userDictionary["Admin"].Id, RoleId = roleDictionary[RoleName.Admin].Id }
        };
    }
    #endregion

}
