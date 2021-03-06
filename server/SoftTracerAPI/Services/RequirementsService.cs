﻿using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Models.Projects.Requirements;
using System.Collections.Generic;

namespace SofTracerAPI.Services
{
    public class RequirementsService
    {

        public List<Requirement> MapCommand(List<CreateRequirementsCommand> command, int nextId)
        {
            List<Requirement> requirement = CreateCommandToRequirement(command);
            PopulateIdentifierDictionary(requirement,  ref nextId);
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
                    Completed = item.Completed,
                    Name = item.Name,
                    Description = item.Description,
                    Children = new List<Requirement>(),
                };
                if (item.ParentId > 0)
                {
                    requirement.ParentId = item.ParentId;
                }
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
                Completed = command.Completed,
                Name = command.Name,
                Description = command.Description,
                Children = new List<Requirement>()
            };
            if (command.ParentId > 0)
            {
                requirement.ParentId = command.ParentId;
            }
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
                    Completed = item.Completed,
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
                Completed = command.Completed,
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

        private void PopulateIdentifierDictionary(List<Requirement> requirements,  ref int id)
        {
            foreach (Requirement requirement in requirements)
            {
                requirement.Id = id;
                id++;
                if (requirement.Children != null)
                {
                    PopulateIdentifierDictionary(requirement.Children,  ref id);
                    foreach(Requirement children in requirement.Children)
                    {
                        if(children.ParentId == 0)
                        {
                        children.ParentId = requirement.Id;
                        }
                    }
                }
            }
        }

    }
}