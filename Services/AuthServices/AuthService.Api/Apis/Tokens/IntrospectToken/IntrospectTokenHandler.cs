using MediatR;

namespace AuthService.Api.Apis.Tokens.IntrospectToken;

public record IntrospectTokenQuery(string Jti) : IRequest<IntrospectTokenResult>;

public record IntrospectTokenResult(bool Active, string? Message);

public class IntrospectTokenQueryValidator : AbstractValidator<IntrospectTokenQuery>
{
    public IntrospectTokenQueryValidator()
    {
        RuleFor(x => x.Jti).NotEmpty().Must(x => Guid.TryParse(x, out _)).WithMessage("Invalid JTI format");
    }
}

public class IntrospectTokenHandler(AuthServiceDbContext context) : IRequestHandler<IntrospectTokenQuery, IntrospectTokenResult>
{
    public async Task<IntrospectTokenResult> Handle(IntrospectTokenQuery request, CancellationToken cancellationToken)
    {
        // Check if token is revoked
        var isRevoked = await context.RevokedTokens.AnyAsync(
            rt => rt.Jti.ToString() == request.Jti,
            cancellationToken
        );

        if (isRevoked)
        {
            return new IntrospectTokenResult(Active: false, Message: "Token has been revoked");
        }

        return new IntrospectTokenResult(Active: true, Message: null);
    }
}
