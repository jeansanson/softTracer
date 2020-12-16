using SofTracerAPI.Models.Tasks;
using System.Collections.Generic;

namespace SofTracerAPI.Commands.Tasks
{
    public class UpdateTaskCommand
    {
        public int Id { get; set; }

        public int RequirementId { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Responsibles { get; set; }

        public TaskStage Stage { get; set; }
    }
}