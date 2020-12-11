using SofTracerAPI.Commands;
using System;

namespace SoftTracerAPI.Commands.Projects
{
    public class InsertUserCommandValidator : BaseValidator, IValidator<InsertUserCommand>
    {
        public ValidationError Validate(InsertUserCommand command)
        {
            if (command.ProjectToken == Guid.Empty) { _manager.AddError("Token inválido"); }
            return _manager.GetError();
        }
    }
}