namespace AuthService.Api.Apis.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdResult>;
public record GetUserByIdResult(Guid Id, string Username, Status Status, bool IsLocked, DateTime? LockoutEnd, int AccessFailedCount, IEnumerable<RoleDto> Roles, IEnumerable<ClaimDto>? Claims, IEnumerable<ScopeDto>? Scopes, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetUserByIdHandler(AuthServiceDbContext context, ITenantGuard tenantGuard) : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserClaims)
            .Include(u => u.UserScopes).ThenInclude(us => us.Scope)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);

        if (user == null) throw new NotFoundException("User not found.");

        tenantGuard.EnsureCanAccess(user.TenantId);

        var roles = user.UserRoles.Select(ur => new RoleDto(ur.RoleId, ur.Role.Name));
        var claims = user.UserClaims.Select(uc => new ClaimDto(uc.ClaimId, uc.Claim.Type, uc.Claim.Value));
        var scopes = user.UserScopes.Select(us => new ScopeDto(us.Scope.Id, us.Scope.Type, us.Scope.Value));

        return new GetUserByIdResult(user.Id, user.Username, user.Status, user.IsLocked, user.LockoutEnd, user.AccessFailedCount, roles, claims, scopes, user.CreatedAt, user.CreatedBy, user.LastModifiedAt, user.LastModifiedBy);
    }
}
