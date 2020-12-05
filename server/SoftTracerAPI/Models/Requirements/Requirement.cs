using System.Collections.Generic;

namespace SofTracerAPI.Models.Projects.Requirements
{
    public class Requirement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }

        public string Description { get; set; }

        public List<Requirement> Children { get; set; }

        public int ParentId { get; set; }
    }
}