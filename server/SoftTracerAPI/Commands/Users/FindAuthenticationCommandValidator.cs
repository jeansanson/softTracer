using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Users
{
    public class FindTokenCommandValidator : BaseValidator, IValidator<FindAuthenticationCommand>
    {
        public ValidationError Validate(FindAuthenticationCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserId)) { _manager.AddError("Usuário vazio"); }
            if (string.IsNullOrWhiteSpace(command.Password)) { _manager.AddError("Senha vazia"); }
            return _manager.GetError();
        }
    }
}