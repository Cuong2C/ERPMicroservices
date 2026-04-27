namespace AuthService.Api.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Type).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Action).IsRequired().HasMaxLength(255);

        builder.HasOne(c => c.Action)
               .WithMany()
               .HasForeignKey(c => c.PermissionActionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
