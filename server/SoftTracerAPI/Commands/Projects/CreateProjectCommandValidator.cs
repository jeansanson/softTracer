using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Projects
{
    public class CreateUserCommandValidator : BaseValidator, IValidator<CreateProjectCommand>
    {
        public ValidationError Validate(CreateProjectCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name)) { _manager.AddError("Nome inválido"); }
            if (string.IsNullOrWhiteSpace(command.Resume)) { _manager.AddError("Resumo inválido"); }
            if ($"{command.Name}".Length > 255) { _manager.AddError("Nome possui muitos caracteres"); }
            if ($"{command.Resume}".Length > 4090) { _manager.AddError("Resumo possui muitos caracteres"); }
            return _manager.GetError();
        }
    }
}