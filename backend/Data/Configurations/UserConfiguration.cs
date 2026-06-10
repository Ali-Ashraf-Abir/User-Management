using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4.Models;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
                builder.HasIndex(x => x.Email)
               .IsUnique().IsDescending();;
                builder.Property(x => x.RegistrationTime).HasDefaultValueSql("now()");
    }
}