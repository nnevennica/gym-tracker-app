namespace GymTracker.Application.DTOs;

public record RegisterRequestDto(string Email, string Password, string FirstName, string LastName);
public record LoginRequestDto(string Email, string Password);
public record AuthResponseDto(string Token, string UserId, string Email);