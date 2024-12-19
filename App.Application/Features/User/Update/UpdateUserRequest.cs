namespace App.Application.Features.User.Update;
public class UpdateUserRequest()
{
    public int Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    //public string Role { get; set; } = default!;
}

