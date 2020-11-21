namespace SofTracerAPI.Commands.Projects.Requirements
{
    public class CreateRequirementsCommandValidator : BaseValidator, IValidator<CreateRequirementsCommand>
    {
        public ValidationError Validate(CreateRequirementsCommand command)
        {
            ValidateCommand(command);
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
            if ($"{command.Name}".Length > 255) { _manager.AddError($"Nome '{command.Name}' possui muitos caracteres"); }
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