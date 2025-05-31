using BookStore.Domain.ApplicationUsers.Services;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.AspNetCore.Identity;


// ReSharper disable FieldCanBeMadeReadOnly.Local

#pragma warning disable CS8618

namespace BookStore.Domain.ApplicationUsers;

public sealed class ApplicationUser : IdentityUser<Guid>, IAuditableEntity, IUser
{


    private ApplicationUser()
    {

    }

    private ApplicationUser(
        string fullName,
        string email,
        string phoneNumber)
    {
        Id = Guid.NewGuid();
        FullName = fullName; 

        Email = email.ToLower();
        NormalizedEmail = email.ToUpper();
        EmailConfirmed = true;

        UserName = email.ToLower();
        NormalizedUserName = email.ToUpper();

        PhoneNumber = phoneNumber;



        SecurityStamp = Guid.NewGuid().ToString(); 


    }

    public bool IsActive { get; private set; } = true;
    public string FullName { get; private set; }


    public DateTimeOffset LastLogin { get; private set; }


    public Guid? CreatedBy { get; private set; }
    public DateTimeOffset? CreatedDateTimeOnUtc { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public DateTimeOffset? UpdatedDateTimeOnUtc { get; private set; }


    public static Result<ApplicationUser> Create(
        string fullName,
        string email,
        string phoneNumber,
        string? password ="",
        Guid? id =null)
    {
        
        var user = new ApplicationUser(fullName, email, phoneNumber);

        if (id is not null)
        {
            user.Id = id.Value;
        }

        if (!string.IsNullOrEmpty(password))
        {
            PasswordFactory.Create(user, password);
        }

        return user;
    }


    public void UpdatePersonalInformation(string fullName, string phoneNumber)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;

    }

    public void UpdateEmail(string email)
    {
        Email = email;
        NormalizedEmail = email.ToUpper();
    }


    public void UpdateIsActive(bool requestIsActive)
    {
        IsActive = requestIsActive;
    }



    public void UpdatePassword(string newPassword)
    {
        // PasswordFactory.Create sets PasswordHash + SecurityStamp
        PasswordFactory.Create(this, newPassword);
    }
}