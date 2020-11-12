using System;

namespace SofTracerAPI.Models.Projects
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Resume { get; set; }

        public DateTime OpeningDate { get; set; }

        public Guid Token{ get; set; }

    }
}