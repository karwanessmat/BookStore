using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.ApplicationUsers.Entities;

public class ApplicationRole : IdentityRole<Guid>
{


    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName)
        : base(roleName)
    {
        
    }
}
