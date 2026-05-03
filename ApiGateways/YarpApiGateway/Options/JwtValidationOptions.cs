namespace YarpApiGateway.Options;

public class JwtValidationOptions
{
    public string Authority { get; set; } = default!;
    public bool RequireHttpsMetadata { get; set; }

    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;

    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }

    public int ClockSkewSeconds { get; set; }
}
