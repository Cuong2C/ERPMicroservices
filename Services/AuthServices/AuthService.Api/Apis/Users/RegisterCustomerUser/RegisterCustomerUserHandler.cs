namespace AuthService.Api.Apis.Users.RegisterCustomerUser;

public record RegisterCustomerUserCommand(string Username, string Password, string? Email, string? PhoneNumber) : IRequest<RegisterCustomerUserResult>;
public record RegisterCustomerUserResult(Guid Id);

public class RegisterCustomerUserCommandValidator : AbstractValidator<RegisterCustomerUserCommand>
{
    public RegisterCustomerUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}

public class RegisterCustomerUserHandler(AuthServiceDbContext context) : IRequestHandler<RegisterCustomerUserCommand, RegisterCustomerUserResult>
{
    public async Task<RegisterCustomerUserResult> Handle(RegisterCustomerUserCommand request, CancellationToken cancellationToken)
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

        var user = new User
        {
            Username = request.Username,
            PasswordHash = CustomHasher.HashByArgon2(request.Password),
            Email = request.Email,  
            PhoneNumber = request.PhoneNumber
        };

        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == StaticDetail.ROLE_CUSTOMER);
        if (role != null)
        {
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterCustomerUserResult(user.Id);     
    }
}
