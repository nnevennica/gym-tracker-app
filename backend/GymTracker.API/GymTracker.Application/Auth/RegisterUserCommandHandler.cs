using GymTracker.Application.Interfaces;
using GymTracker.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GymTracker.Application.Auth;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(UserManager<IdentityUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<string>.Failure("Korisnik sa datim email-om već postoji.");
        }

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure(errors);
        }

        // slanje mail-a
        await _emailService.SendEmailAsync(
            user.Email,
            "Dobrodošli u GymTracker!",
            $"Zdravo {request.FirstName},\n\nVaš nalog za praćenje treninga je uspešno kreiran!",
            cancellationToken
        );

        return Result<string>.Success(user.Id);
    }
}