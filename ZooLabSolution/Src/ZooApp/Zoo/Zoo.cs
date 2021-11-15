using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public class IdGenerator
    {
        public int NextID { get; private set; } = 0;
        public int GetID()
        {
            return ++NextID;
        }
    }
    public class Zoo
    {
        public List<Enclosure> Enclosures { get; private set; } = new();
        public List<IEmployee> Employees { get; private set; } = new();
        public List<string> Animals { get; private set; } = new();
        public string Location { get; private set; }
        private readonly IConsole _iConsole;
        public IdGenerator AnimalIdGenetator { get; } = new();
        public Zoo(string Location, IConsole console = null)
        {
            this.Location = Location; _iConsole = console;
        }
        public Enclosure AddEnclosure(string Name, int SquareFeet)
        {
            if(string.IsNullOrEmpty(Name))
            {
                _iConsole?.WriteLine($"'Name' cannot be null or empty");
                throw new ArgumentNullException(nameof(Name));
            }
            foreach(var enclosure in Enclosures)
            {
                if (enclosure.Name == Name)
                {
                    _iConsole?.WriteLine($"Enclosure with same name already exists");
                    throw new ArgumentException(nameof(Name));
                }
            } 
            if(SquareFeet <= 0)
            {
                _iConsole?.WriteLine($"'Square Feet' must be greater then 0");
                throw new ArgumentException(nameof(SquareFeet));
            }
            Enclosures.Add(new Enclosure(Name, SquareFeet, this, _iConsole));
            _iConsole?.WriteLine($"Enclosure {Name} was added.");
            return Enclosures.Last();
        }
        public Enclosure FindAviableEnclosure(Animal animal)
        {
            if(animal is null)
            {
                _iConsole?.WriteLine($"You should choose correct animal");
                throw new ArgumentNullException(nameof(animal));
            }
            List<Enclosure> aviableEnclosure = new();
            Enclosures.ForEach(enclosure =>
            {
                try
                {
                    if (enclosure.CanAddAnimal(animal))
                    {
                        aviableEnclosure.Add(enclosure);
                    }
                }
                catch { }
            });
            if(aviableEnclosure.Count == 0)
            {
                _iConsole?.WriteLine($"No aviable enclosures");
                throw new NoAvailableEnclosureException($"No aviable enclosures in the zoo at {Location}");
            }
            var bestOfEnclosures = aviableEnclosure.OrderBy(space => space.FreeSquareFeet).First();
            _iConsole?.WriteLine($"Found available {animal.GetType().Name} enclosure '{bestOfEnclosures.Name}'");
            return bestOfEnclosures;
            
        }
        public void HireEmployee(IEmployee employee)
        {
            if (employee is null)
            {
                _iConsole?.WriteLine($"Employee is not provided");
                throw new ArgumentNullException(nameof(employee));
            }
            var hireValidator = HireValidatorProvider.GetHireValidator(employee);
            var results = hireValidator.ValidateEmployee(employee, this);
            if(results.IsValid)
            {
                Employees.Add(employee);
                _iConsole?.WriteLine($"Hired an employee: {employee.GetType().Name} {employee.FirstName} {employee.LastName}.");
            }
            else
            {
                _iConsole?.WriteLine($"Cannot hire an employee: {employee.GetType().Name} {employee.FirstName} {employee.LastName}.");
                foreach(var error in results.Errors)
                {
                    _iConsole?.WriteLine($"{error.PropertyName} : {error.ErrorMessage}");
                }
            }
        }
        public void FeedAnimals(DateTime dateTime)
        {
            foreach(var ecnlosure in Enclosures)
            {
                foreach(var animal in ecnlosure.Animals)
                {
                    bool IsFed = false;
                    foreach (var employee in Employees)
                    {
                        if (employee is ZooKeeper)
                        {
                            var zooKeeper = employee as ZooKeeper;
                            if (zooKeeper.HasAnimalExperiences(animal))
                            {
                                var food = GetTypeFoodByTheName(animal.FavoriteFood[0]);
                                if(zooKeeper.FeedAnimal(animal, food, dateTime))
                                    IsFed = true;
                                break;
                            }
                        }
                    }
                    if(!IsFed)
                    {
                        _iConsole?.WriteLine($"The {animal.GetType().Name} from {ecnlosure.Name} was not fed.");
                    }
                }
            }
        }
        public Food GetTypeFoodByTheName(string foodName)
        {
            switch(foodName)
            {
                case "Meet": return new Meet();
                case "Vegetable": return new Vegetable();
                case "Grass": return new Grass();
                default: return null;
            }
        }
        public void HealAnimals()
        {
            foreach (var ecnlosure in Enclosures)
            {
                foreach (var animal in ecnlosure.Animals)
                {
                    if(animal.IsSick)
                    {
                        bool IsHealed = false;
                        foreach (var employee in Employees)
                        {
                            if (employee is Veterinarian)
                            {
                                var veterinarian = employee as Veterinarian;
                                if (veterinarian.HasAnimalExperiences(animal))
                                {
                                    if(veterinarian.HealAnimals(animal, new Antibiotics()))
                                        IsHealed = true;
                                    break;
                                }
                            }
                        }
                        if (!IsHealed)
                        {
                            _iConsole?.WriteLine($"The {animal.GetType().Name} from {ecnlosure.Name} was not healed : there are no Veterinarian that have experienced to heal it");
                        }
                    }
                }
            }
        }
    }
}
