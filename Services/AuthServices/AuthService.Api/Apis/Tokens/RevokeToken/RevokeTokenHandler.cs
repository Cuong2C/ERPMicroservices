namespace AuthService.Api.Apis.Tokens.RevokeToken;

public record RevokeTokenCommand(string Jti) : IRequest<RevokeTokenResult>;

public record RevokeTokenResult(bool IsSuccess);

public class RevokeTokenHandler(AuthServiceDbContext context) : IRequestHandler<RevokeTokenCommand, RevokeTokenResult>
{
    public async Task<RevokeTokenResult> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var isRevoked = await context.RevokedTokens.AnyAsync(x => x.Jti == request.Jti, cancellationToken);
        if (isRevoked) throw new BadRequestException("Token is already revoked.", request.Jti);

        var revokedToken = new RevokedToken
        {
            Jti = request.Jti,
        };

        context.RevokedTokens.Add(revokedToken);
        await context.SaveChangesAsync(cancellationToken);

        return new RevokeTokenResult(true);
    }
}
