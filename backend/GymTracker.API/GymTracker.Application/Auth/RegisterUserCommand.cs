using GymTracker.Application.DTOs;
using GymTracker.Domain.Common;
using MediatR;

namespace GymTracker.Application.Auth;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<Result<string>>;