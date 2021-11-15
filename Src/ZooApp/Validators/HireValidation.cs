using FluentValidation.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public interface IHireValidator
    {
        public ValidationResult ValidateEmployee(IEmployee employee, Zoo zoo);
    }

    public abstract class HireValidator<TEmployee> : AbstractValidator<TEmployee>
    {
        public abstract ValidationResult ValidateEmployee(IEmployee employee, Zoo zoo);
    }

    public class HireValidatorProvider
    {
        public static IHireValidator GetHireValidator(IEmployee employee)
        {
            switch (employee)
            {
                case ZooKeeper: return new ZooKeeperHireValidator();
                case Veterinarian: return new VeterinarianHireValidator();
                default: throw new NotImplementedException(nameof(employee));
            }
        }
    }
}
