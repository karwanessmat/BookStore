using BookStore.Domain.ApplicationUsers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.ApplicationUser.Persistence.Configurations;

public class ApplicationClaimConfiguration : IEntityTypeConfiguration<ApplicationClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationClaim> builder) =>
        builder.ToTable("UserClaims", SchemaNames.Security);
}