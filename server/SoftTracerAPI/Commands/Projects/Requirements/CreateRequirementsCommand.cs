using System.Collections.Generic;

namespace SofTracerAPI.Commands.Projects.Requirements
{
    public class CreateRequirementsCommand
    {

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<CreateRequirementsCommand> Children { get; set; }

    }
}