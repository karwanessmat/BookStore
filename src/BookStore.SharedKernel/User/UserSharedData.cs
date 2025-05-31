namespace BookStore.SharedKernel.User;

public static class UserSharedData
{
    public static readonly List<UserSharedDto> Users =
    [
        new()
        {
            Id = Guid.Parse("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
            FullName = "Admin",
            Email = "admin@ni.iq",
            Password = "Admin@123",
            PhoneNumber = "07500000011",
            IsAdministrative = true,
            RoleName = RoleName.Admin
        }
    ];
    public static UserSharedDto User(string roleName) => Users.FirstOrDefault(x => x.RoleName == roleName)!;
}


