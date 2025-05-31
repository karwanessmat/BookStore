using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.Domain.ApplicationUsers.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.ApplicationUser.Persistence.Configurations;

public class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder
            .ToTable("RevokedTokens", SchemaNames.Security);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("RevokedTokenId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,               
                value => RevokedTokenId.Create(value)); 

        builder.HasIndex(x => x.Jti).IsUnique();
        builder.Property(x => x.Jti)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(x => x.ExpiryUtc)
            .HasDatabaseName("IX_RevokedTokens_ExpiryUtc");
        builder.Property(x => x.ExpiryUtc)
            .IsRequired();
    }
}