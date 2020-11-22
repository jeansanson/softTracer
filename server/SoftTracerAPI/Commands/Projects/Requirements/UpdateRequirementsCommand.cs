using System.Collections.Generic;

namespace SofTracerAPI.Commands.Projects.Requirements
{
    public class UpdateRequirementsCommand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }

        public string Description { get; set; }

        public List<UpdateRequirementsCommand> Children { get; set; }

        public int ParentId { get; set; }

    }
}