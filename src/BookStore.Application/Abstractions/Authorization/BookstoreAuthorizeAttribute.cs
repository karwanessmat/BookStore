namespace BookStore.Application.Abstractions.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public  class BookstoreAuthorizeAttribute : Attribute
{
    public string? Permissions { get; set; }
    public string? Roles { get; set; }
}
