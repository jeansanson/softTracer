using System;

namespace SoftTracerAPI.Commands.Projects
{
    public class InsertUserCommand
    {

        public Guid ProjectToken { get; set; }

        public string UserId { get; set; }

    }
}