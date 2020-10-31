using Org.BouncyCastle.Bcpg;
using System.Collections.Generic;
using System.Linq;

namespace SofTracerAPI.Commands
{
    public class ValidationErrorManager
    {
        private readonly List<string> _errors;

        public ValidationErrorManager()
        {
            _errors = new List<string>();
        }

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public ValidationError GetError()
        {
            return new ValidationError(_errors);
        }
    }
}