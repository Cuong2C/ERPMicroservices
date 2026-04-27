namespace AuthService.Api.Apis.Users.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<DeleteUserResult>;
public record DeleteUserResult(Guid Id);

internal class DeleteUserHandler(AuthServiceDbContext context, ITenantGuard tenantGuard) : IRequestHandler<DeleteUserCommand, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync( u => u.Id == command.Id, cancellationToken);
        if (user == null) throw new NotFoundException("User not found.");

        tenantGuard.EnsureCanAccess(user.TenantId);

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteUserResult(user.Id);
    }
}
