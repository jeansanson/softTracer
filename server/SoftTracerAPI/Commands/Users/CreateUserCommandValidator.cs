using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Users
{
    public class CreateUserCommandValidator : BaseValidator, IValidator<CreateUserCommand>
    {
        public ValidationError Validate(CreateUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserId)) { _manager.AddError("Usuário inválido"); }
            if (string.IsNullOrWhiteSpace(command.Password)) { _manager.AddError("Senha inválida"); }
            if (string.IsNullOrWhiteSpace(command.Email)) { _manager.AddError("E-mail inválido"); }
            if (string.IsNullOrWhiteSpace(command.DisplayName)) { _manager.AddError("Nome inválido"); }
            return _manager.GetError();
        }
    }
}