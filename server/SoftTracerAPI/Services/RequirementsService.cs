using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Models.Projects.Requirements;
using System.Collections.Generic;

namespace SofTracerAPI.Services
{
    public class RequirementsService
    {
        public List<Requirement> MapCommand(List<CreateRequirementsCommand> command)
        {
            List<Requirement> requirement = CreateCommandToRequirement(command);
            List<Requirement> everyRequirement = new List<Requirement>();
            PopulateIdentifierDictionary(requirement, everyRequirement);
            return requirement;
        }

        public List<Requirement> MapCommand(List<UpdateRequirementsCommand> command)
        {
            return CreateCommandToRequirement(command);
        }

        private static List<Requirement> CreateCommandToRequirement(List<CreateRequirementsCommand> command)
        {
            List<Requirement> result = new List<Requirement>();
            foreach (CreateRequirementsCommand item in command)
            {
                Requirement requirement = new Requirement()
                {
                    Completed = false,
                    Name = item.Name,
                    Description = item.Description,
                    Children = new List<Requirement>()
                };
                result.Add(requirement);
                if (item.Children != null)
                {
                    foreach (CreateRequirementsCommand child in item.Children)
                    {
                        requirement.Children.Add(CreateCommandToRequirement(child));
                    }
                }
            }

            return result;
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

        private static List<Requirement> CreateCommandToRequirement(List<UpdateRequirementsCommand> command)
        {
            List<Requirement> result = new List<Requirement>();
            foreach (UpdateRequirementsCommand item in command)
            {
                Requirement requirement = new Requirement()
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    Completed = false,
                    Name = item.Name,
                    Description = item.Description,
                    Children = new List<Requirement>()
                };
                result.Add(requirement);
                if (item.Children != null)
                {
                    foreach (UpdateRequirementsCommand child in item.Children)
                    {
                        requirement.Children.Add(CreateCommandToRequirement(child));
                    }
                }
            }

            return result;
        }

        private static Requirement CreateCommandToRequirement(UpdateRequirementsCommand command)
        {
            Requirement requirement = new Requirement()
            {
                Id = command.Id,
                ParentId = command.ParentId,
                Completed = false,
                Name = command.Name,
                Description = command.Description,
                Children = new List<Requirement>()
            };

            if (command.Children != null)
            {
                foreach(UpdateRequirementsCommand child in command.Children)
                {
                    requirement.Children.Add(CreateCommandToRequirement(child));
                }
            }
            return requirement;
        }

        private static void PopulateIdentifierDictionary(List<Requirement> requirements, List<Requirement> list)
        {
            foreach (Requirement requirement in requirements)
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