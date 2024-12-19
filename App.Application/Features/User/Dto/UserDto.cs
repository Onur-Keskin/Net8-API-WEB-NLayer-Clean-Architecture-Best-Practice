namespace App.Application.Features.User.Dto;

public record UserDto(int id, string Username,string PasswordHash, string Role);

