using System.Collections.Generic;

namespace SofTracerAPI.Models.Tasks
{
    public class Task
    {
        public int Id { get; set; }

        public int RequirementId { get; set; }

        public string RequirementName { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Responsibles { get; set; }

        public TaskStage Stage { get; set; }

    }
}