using SofTracerAPI.Models.Tasks;

namespace SofTracerAPI.Commands.Tasks
{
    public class CreateTaskCommandValidator : BaseValidator, IValidator<CreateTaskCommand>
    {
        public ValidationError Validate(CreateTaskCommand command)
        {
            if (command.ProjectId <= 0) { _manager.AddError("Projeto inválido"); }
            if (string.IsNullOrWhiteSpace(command.Name)) { _manager.AddError("Nome inválido"); }
            if ($"{command.Name}".Length > 255) { _manager.AddError("Nome possui muitos caracteres"); }
            if ($"{command.Description}".Length > 4090) { _manager.AddError("Descrição possui muitos caracteres"); }
            if (command.Stage == TaskStage.Undefined) { _manager.AddError("Estágio inválido"); }
            return _manager.GetError();
        }
    }
}