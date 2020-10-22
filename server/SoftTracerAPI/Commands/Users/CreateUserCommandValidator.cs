namespace SoftTracerAPI.Commands.Users
{
    public class CreateUserCommandValidator
    {

        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}