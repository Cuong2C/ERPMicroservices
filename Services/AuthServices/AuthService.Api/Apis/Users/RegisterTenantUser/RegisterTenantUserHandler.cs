namespace AuthService.Api.Apis.Users.RegisterTenantUser;

public record RegisterTenantUserCommand(string Username, string Password, string? Email, string? PhoneNumber, Guid TenantId) : IRequest<RegisterTenantUserResult>;
public record RegisterTenantUserResult(Guid Id);

public class RegisterTenantUserCommandValidator : AbstractValidator<RegisterTenantUserCommand>
{
    public RegisterTenantUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        RuleFor(x => x.TenantId).NotEmpty();
    }
}

public class RegisterTenantUserHandler(AuthServiceDbContext context) : IRequestHandler<RegisterTenantUserCommand, RegisterTenantUserResult>
{
    public async Task<RegisterTenantUserResult> Handle(RegisterTenantUserCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.PhoneNumber)) 
            throw new BadRequestException("Either email or phone number must be provided.");

        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingEmail = await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (existingEmail) throw new BadRequestException("Email already exists.");
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            var existingPhoneNumber = await context.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
            if (existingPhoneNumber) throw new BadRequestException("Phone number already exists.");
        }

        var existingUser = await context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (existingUser) throw new BadRequestException("Username already exists.");

        var existingTenant = await context.Tenants.AnyAsync(t => t.Id == request.TenantId, cancellationToken);
        if (!existingTenant) throw new BadRequestException("Tenant does not exist.");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = CustomHasher.HashByArgon2(request.Password),
            Email = request.Email,  
            PhoneNumber = request.PhoneNumber
        };

        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == StaticDetail.ROLE_SHOP_OWNER);

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role!.Id
        });

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterTenantUserResult(user.Id);     
    }
}
