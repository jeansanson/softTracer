using System.Collections.Generic;

namespace SofTracerAPI.Commands.Projects.Requirements
{
    public class CreateRequirementsCommandValidator : BaseValidator, IValidator<List<CreateRequirementsCommand>>
    {
        public ValidationError Validate(List<CreateRequirementsCommand> command)
        {
            foreach(CreateRequirementsCommand item in command)
            {
            ValidateCommand(item);
            }
            return _manager.GetError();
        }

        private void AddErrors(CreateRequirementsCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                string namelessError = "Há requisitos sem nome";
                if (!_manager.HasError(namelessError))
                {
                    _manager.AddError(namelessError);
                }
            }
            if ($"{command.Name}".Length > 200) { _manager.AddError($"Nome '{command.Name}' possui muitos caracteres"); }
            if ($"{command.Description}".Length > 4000) { _manager.AddError($"Descrição do requisito '{command.Name}' possui muitos caracteres"); }
        }

        private void ValidateCommand(CreateRequirementsCommand command)
        {
            AddErrors(command);
            if (command.Children != null)
            {
                foreach (CreateRequirementsCommand child in command.Children)
                {
                    ValidateCommand(child);
                }
            }
        }
    }
}