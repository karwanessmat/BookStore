using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.ApplicationUser.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<Domain.ApplicationUsers.ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<Domain.ApplicationUsers.ApplicationUser> builder)
        {
            RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder)builder, "Users", SchemaNames.Security);

        }
    }
}
