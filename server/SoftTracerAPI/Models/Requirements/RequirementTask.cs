using SofTracerAPI.Models.Tasks;

namespace SofTracerAPI.Models.Projects.Requirements
{
    public class RequirementTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TaskStage Stage { get; set; }

    }
}