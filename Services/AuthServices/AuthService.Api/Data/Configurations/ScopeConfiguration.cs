namespace AuthService.Api.Data.Configurations;

public class ScopeConfiguration : IEntityTypeConfiguration<Scope>
{
    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Resource)
            .WithMany()
            .HasForeignKey(s => s.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.ResourceId, s.Value }).IsUnique();
        builder.Property(s => s.ResourceId).IsRequired();
        builder.Property(s => s.Value).IsRequired().HasMaxLength(255);
    }
}
