using System;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public class VeterinarianHireValidator : HireValidator<Veterinarian>, IHireValidator
    {
        private List<string> AnimalsInHiringZoo;
		public VeterinarianHireValidator()
		{
			RuleFor(veterinarian => veterinarian.AnimalExperiences)
				.Must(animalExperiences => animalExperiences.Count > 0 && animalExperiences.TrueForAll(
					animalExperience => AnimalsInHiringZoo.Contains(animalExperience)))
				.WithMessage("Must have experience with all animals present in the hiring zoo.");
			RuleFor(zooKeeper => zooKeeper.LastName)
				.NotEmpty().WithMessage("Last name is required.");
		}

		public override ValidationResult ValidateEmployee(IEmployee employee, Zoo hiringZoo)
		{
			var veterinarian = (Veterinarian)employee;
			AnimalsInHiringZoo = hiringZoo.Animals;
			return Validate(veterinarian);
		}
    }
}
