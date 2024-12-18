namespace App.Application.Features.Login.Dto;

public record UserDto(int id,string Username,string PasswordHash,string Role);

