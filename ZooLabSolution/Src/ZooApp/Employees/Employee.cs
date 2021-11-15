using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public interface IEmployee
    {
        public string FirstName { get; }
        public string LastName { get; }
    }
    public class ZooKeeper : IEmployee
    {
        private readonly IConsole _iConsole;
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public List<string> AnimalExperiences { get; private set; } = new();
        public ZooKeeper(string firstName, string lastName, IConsole console = null)
        {
            FirstName = firstName; LastName = lastName;
            _iConsole = console;
        }
        public void AddAnimalExperiences(Animal animal)
        {
            if(animal is null)
            {
                _iConsole?.WriteLine($"The animal is not provided");
                throw new ArgumentNullException(nameof(animal));
            }
            if(HasAnimalExperiences(animal))
            {
                _iConsole?.WriteLine($"{FirstName} {LastName} is already experienced with {animal.GetType().Name}.");
                return;
            }
            else
            {
                AnimalExperiences.Add(animal.GetType().Name);
                _iConsole?.WriteLine($"{FirstName} {LastName} is start experienced with {animal.GetType().Name}.");
            }
        }
        public bool HasAnimalExperiences(Animal animal)
        {
            if (AnimalExperiences.Contains(animal.GetType().Name)) return true;
            return false;   
        }
        public bool FeedAnimal(Animal animal, Food food, DateTime dateTime)
        {
            if(CanFeed(animal, food, dateTime))
            {
                animal.Feed(food, this, dateTime);
                _iConsole?.WriteLine($"{FirstName} {LastName} fed {animal.GetType().Name} <{animal.ID}> with {food.GetType().Name}.");
                return true;
            }
            return false;
        }
        public bool CanFeed(Animal animal, Food food, DateTime dateTime)
        {
            if (animal is null)
            {
                _iConsole?.WriteLine($"The animal is not provided");
                throw new ArgumentNullException(nameof(animal));
            }
            if(!HasAnimalExperiences(animal))
            {
                _iConsole?.WriteLine($"You have no experience to feed {animal.GetType().Name}");
                return false;
            }
            if(!CouldFeedByFeedSchedule(animal, dateTime))
            {
                _iConsole?.WriteLine($"It's not time to feed the {animal.GetType().Name}");
                return false;
            }
            if(!CouldFeedByDailyLimit(animal, dateTime))
            {
                _iConsole?.WriteLine($"Today the {animal.GetType().Name} has already been fed 2 times");
                return false;
            }
            return true;
        }
        private bool CouldFeedByDailyLimit(Animal animal, DateTime dateTime)
        {
            if (animal.FeedTimes.Count >= 2)
            {
                if (animal.FeedTimes.Last().feedTime.ToString("d") == dateTime.ToString("d"))
                    if (animal.FeedTimes[animal.FeedTimes.Count - 2].feedTime.ToString("d") == dateTime.ToString("d"))
                        return false;
            } return true;
        }
        public bool CouldFeedByFeedSchedule(Animal animal, DateTime dateTime)
        {
            if(animal.FeedSchedule.Count == 0)
            {
                _iConsole?.WriteLine($"Your feed schedule was null or empty");
                throw new ArgumentNullException(nameof(animal.FeedSchedule));
            }
            if (animal.FeedSchedule.Contains(dateTime.Hour))
                return true;
            return false;
        }
    }
    public class Veterinarian : IEmployee
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private readonly IConsole _iConsole;
        public List<string> AnimalExperiences { get; private set; } = new();
        public Veterinarian(string firstName, string lastName, IConsole console = null)
        {
            FirstName = firstName; LastName = lastName;
            _iConsole = console;
        }
        public void AddAnimalExperiences(Animal animal)
        {
            if (animal is null)
            {
                _iConsole?.WriteLine($"The animal is not provided");
                throw new ArgumentNullException(nameof(animal), $"Animal experiences with null '{nameof(animal)}'");
            }
            if (HasAnimalExperiences(animal))
            {
                _iConsole?.WriteLine($"{FirstName} {LastName} is already experienced with {animal.GetType().Name}.");
                return;
            }
            else
            {
                AnimalExperiences.Add(animal.GetType().Name);
                _iConsole?.WriteLine($"{FirstName} {LastName} is start experienced with {animal.GetType().Name}.");
            }
        }
        public bool HasAnimalExperiences(Animal animal)
        {
            if (AnimalExperiences.Contains(animal.GetType().Name)) return true;
            return false;
        }
        public bool HealAnimals(Animal animal, Medicine medicine)
        {
            if(CanHeal(animal))
            {
                animal.Heal(medicine);
                _iConsole?.WriteLine($"{FirstName} {LastName} heals {animal.GetType().Name} <{animal.ID}> with {medicine.GetType().Name}.");
                return true;
            }
            return false;
        }
        public bool CanHeal(Animal animal)
        {
            if (animal is null)
            {
                _iConsole?.WriteLine($"The animal is not provided");
                throw new ArgumentNullException(nameof(animal));
            }
            if(!HasAnimalExperiences(animal))
            {
                _iConsole?.WriteLine($"{FirstName} {LastName} has no experiences with {animal.GetType().Name}");
                throw new NoNeededExperienceException(animal.GetType().Name);
            }
            if(!animal.IsSick)
            {
                _iConsole?.WriteLine($"The {animal.GetType().Name} is not sick.");
                return false;
            }
            return true;
        }
    }
}
