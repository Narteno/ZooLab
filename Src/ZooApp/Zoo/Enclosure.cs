using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public class Enclosure
    {
        public string Name {get; private set;}
        public List<Animal> Animals { get; private set; } = new();
        public Zoo ParentZoo { get; private set; }
        public int SquareFeet { get; private set; }
        public int FreeSquareFeet { get; private set; }
        private readonly IConsole _iConsole;
        public Enclosure(string name, int squareFeet, Zoo zoo, IConsole console = null)
        {
            Name = name; SquareFeet = squareFeet;
            ParentZoo = zoo; FreeSquareFeet = SquareFeet;
            _iConsole = console;
        }
        public void AddAnimals(Animal animal)
        {
            if(CanAddAnimal(animal))
            {
                if (animal.AssignID(ParentZoo.AnimalIdGenetator.GetID()))
                {
                    Animals.Add(animal);
                    FreeSquareFeet -= animal.RequiredSpaceSqFt;
                    if (!ParentZoo.Animals.Contains(animal.GetType().Name))
                        ParentZoo.Animals.Add(animal.GetType().Name);
                    _iConsole?.WriteLine($"Added {animal.GetType().Name} <{animal.ID}>.");
                }
                else throw new InvalidOperationException(nameof(animal.AssignID));
            }
        }
        public bool CanAddAnimal(Animal animal)
        {
            if(animal is null)
            {
                _iConsole?.WriteLine($"Animal is required");
                throw new ArgumentNullException(nameof(animal));
            }
            Animals.ForEach(ExistingAnimals =>
            {
                if(!ExistingAnimals.IsFriendlyWith(animal))
                {
                    _iConsole?.WriteLine($"There are no friendly animal with {animal.GetType().Name}");
                    throw new NoFriendlyAnimalException(animal.GetType().Name);
                }
            });
            if(animal.RequiredSpaceSqFt > FreeSquareFeet)
            {
                _iConsole?.WriteLine($"Not enough free square feet for {animal.GetType().Name}");
                throw new NoAvailableSpaceException(nameof(animal));
            }
            return true;
        }
    }
}
