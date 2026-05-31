namespace AuthService.Api.Apis.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdResult>;
public record GetUserByIdResult(Guid Id, string Username, Status Status, string Fullname, string? Email, string? Address, string? City, string? Region, int? PostalCode, string? Country, string? PhoneNumber, bool IsLocked, DateTime? LockoutEnd, int AccessFailedCount, IEnumerable<RoleDto> Roles, IEnumerable<PermissionDto>? Claims, IEnumerable<ScopeDto>? Scopes, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetUserByIdHandler(AuthServiceDbContext context, IUserGuard userGuard) : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserPermissions)
            .Include(u => u.UserScopes).ThenInclude(us => us.Scope).ThenInclude(s => s.Resource)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);

        if (user == null) throw new NotFoundException("User not found.");

        userGuard.EnsureCanAccess(user.Id);

        var roles = user.UserRoles.Select(ur => new RoleDto(ur.RoleId, ur.Role.Name));
        var claims = user.UserPermissions.Select(uc => new PermissionDto(uc.PermissionId, uc.Permission.Type, uc.Permission.PermissionAction));
        var scopes = user.UserScopes.Select(us => new ScopeDto(us.Scope.Id, us.Scope.ResourceId, us.Scope.Resource.Name, us.Scope.Value));

        return new GetUserByIdResult(user.Id, user.Username, user.Status, user.Fullname ?? "", user.Email, user.Address, user.City, user.Region, user.PostalCode, user.Country, user.PhoneNumber, user.IsLocked, user.LockoutEnd, user.AccessFailedCount, roles, claims, scopes, user.CreatedAt, user.CreatedBy, user.LastModifiedAt, user.LastModifiedBy);
    }
}
