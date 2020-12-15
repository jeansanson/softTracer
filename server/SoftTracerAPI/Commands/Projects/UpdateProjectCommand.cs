namespace SoftTracerAPI.Commands.Projects
{
    public class UpdateProjectCommand
    {
        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Resume { get; set; }
    }
}