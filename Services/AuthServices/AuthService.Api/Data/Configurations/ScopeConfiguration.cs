namespace AuthService.Api.Data.Configurations;

public class ScopeConfiguration : IEntityTypeConfiguration<Scope>
{
    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Type).IsRequired().HasMaxLength(255);
        builder.Property(s => s.Value).IsRequired().HasMaxLength(255);
    }
}
