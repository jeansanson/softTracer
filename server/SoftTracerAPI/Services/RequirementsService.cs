using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Models.Projects.Requirements;
using System.Collections.Generic;

namespace SofTracerAPI.Services
{
    public class RequirementsService
    {
        public Requirement MapCommand(CreateRequirementsCommand command)
        {
            Requirement requirement = CreateCommandToRequirement(command);
            List<Requirement> list = new List<Requirement>();
            PopulateIdentifierDictionary(requirement, list);
            return requirement;
        }

        private static Requirement CreateCommandToRequirement(CreateRequirementsCommand command)
        {
            Requirement requirement = new Requirement()
            {
                Completed = false,
                Name = command.Name,
                Description = command.Description,
                Children = new List<Requirement>()
            };
            if (command.Children != null)
            {
                foreach (CreateRequirementsCommand child in command.Children)
                {
                    requirement.Children.Add(CreateCommandToRequirement(child));
                }
            }
            return requirement;
        }

        private static void PopulateIdentifierDictionary(Requirement requirement, List<Requirement> list)
        {
            requirement.Id = list.Count + 1;
            list.Add(requirement);
            if (requirement.Children != null)
            {
                foreach (Requirement child in requirement.Children)
                {
                    child.ParentId = requirement.Id;
                    PopulateIdentifierDictionary(child, list);
                }
            }
        }

    }
}