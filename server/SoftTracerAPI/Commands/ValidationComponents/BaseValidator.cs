

using System.Collections.Generic;
using System.Configuration;

namespace SofTracerAPI.Commands
{
    public class BaseValidator
    {
        protected ValidationErrorManager _manager;
        public BaseValidator()
        {
            _manager = new ValidationErrorManager();
        }

    }
}