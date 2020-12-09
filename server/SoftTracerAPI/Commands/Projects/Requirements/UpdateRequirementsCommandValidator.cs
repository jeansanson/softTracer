using System.Collections.Generic;

namespace SofTracerAPI.Commands.Projects.Requirements
{
    public class UpdateRequirementsCommandValidator : BaseValidator, IValidator<List<UpdateRequirementsCommand>>
    {
        public ValidationError Validate(List<UpdateRequirementsCommand> command)
        {
            foreach (UpdateRequirementsCommand item in command)
            {
                ValidateCommand(item);
            }
            return _manager.GetError();
        }

        private void AddErrors(UpdateRequirementsCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                string namelessError = "Há requisitos sem nome";
                if (!_manager.HasError(namelessError))
                {
                    _manager.AddError(namelessError);
                }
            }
            if (command.Id == 0) { _manager.AddError($"Id indefinido"); }
            if ($"{command.Name}".Length > 200) { _manager.AddError($"Nome '{command.Name}' possui muitos caracteres"); }
            if ($"{command.Description}".Length > 4000) { _manager.AddError($"Descrição do requisito '{command.Name}' possui muitos caracteres"); }
        }

        private void ValidateCommand(UpdateRequirementsCommand command)
        {
            AddErrors(command);
            if (command.Children != null)
            {
                foreach (UpdateRequirementsCommand child in command.Children)
                {
                    ValidateCommand(child);
                }
            }
        }
    }
}