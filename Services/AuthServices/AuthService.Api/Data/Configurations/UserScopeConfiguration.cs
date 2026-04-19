namespace AuthService.Api.Data.Configurations;

public class UserScopeConfiguration : IEntityTypeConfiguration<UserScope>
{
    public void Configure(EntityTypeBuilder<UserScope> builder)
    {
        builder.HasKey(us => new { us.UserId, us.ScopeId });

        builder.HasOne(us => us.User)
               .WithMany(u => u.UserScopes)
               .HasForeignKey(us => us.UserId);

        builder.HasOne(us => us.Scope)
               .WithMany()
               .HasForeignKey(us => us.ScopeId);
    }
}
