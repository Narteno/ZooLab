using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace ZooLab
{
    public class ZooKeeperHireValidator : HireValidator<ZooKeeper>, IHireValidator
    {
        private List<string> AnimalsInHiringZoo;
        public ZooKeeperHireValidator()
        {
            RuleFor(zooKeeper => zooKeeper.AnimalExperiences)
                .Must(animalExperiences => animalExperiences.Count > 0 && animalExperiences.Exists(
                    animalExperience => AnimalsInHiringZoo.Contains(animalExperience)))
                .WithMessage("Must have experience with any animals in the hiring zoo.");
            RuleFor(zooKeeper => zooKeeper.LastName)
                .NotEmpty().WithMessage("Last name is required.");
        }
        public override ValidationResult ValidateEmployee(IEmployee employee, Zoo hiringZoo)
        {
            var zooKeeper = (ZooKeeper)employee;
            AnimalsInHiringZoo = hiringZoo.Animals;
            return Validate(zooKeeper);
        }
    }
}
