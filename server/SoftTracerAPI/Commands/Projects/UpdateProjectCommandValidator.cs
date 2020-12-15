using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Projects
{
    public class UpdateProjectCommandValidator : BaseValidator, IValidator<UpdateProjectCommand>
    {
        public ValidationError Validate(UpdateProjectCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name)) { _manager.AddError("Nome inválido"); }
            if (string.IsNullOrWhiteSpace(command.Resume)) { _manager.AddError("Resumo inválido"); }
            if (command.ProjectId < 0) { _manager.AddError("Projeto inválido"); }
            if ($"{command.Name}".Length > 255) { _manager.AddError("Nome possui muitos caracteres"); }
            if ($"{command.Resume}".Length > 4090) { _manager.AddError("Resumo possui muitos caracteres"); }
            return _manager.GetError();
        }
    }
}