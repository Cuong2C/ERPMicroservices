namespace AuthService.Api.Data.Configurations;

public class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Jti).IsRequired();
    }
}
