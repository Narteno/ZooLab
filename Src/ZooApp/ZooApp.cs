using System;
using System.Collections.Generic;

namespace ZooLab
{
    public class ZooApp
    {
        public List<Zoo> Zoos { get; private set; } = new();
        private readonly IConsole _iConsole;
        public ZooApp(IConsole console = null)
        {
            _iConsole = console;
        }
        public void Initialize(DateTime dateTime)
        {
            var zoo1 = InitializeZoo("Tomsk, Russia", dateTime);
            var zoo2 = InitializeZoo("Novosibirsk, Russia", dateTime);
            AddZoo(zoo1); AddZoo(zoo2);
            foreach (var zoo in Zoos)
            {
                zoo.FeedAnimals(dateTime);
                zoo.HealAnimals();
            }
        }
        public Zoo InitializeZoo(string Location, DateTime dateTime)
        {
            var zoo = new Zoo(Location, _iConsole);
            InitializeEnclosures(zoo);
            InitializeAnimals(zoo, dateTime);
            InitializeZooKeepersAndVeterinarians(zoo);
            return zoo;
        }

        private void InitializeZooKeepersAndVeterinarians(Zoo zoo)
        {
            AddZooKeeperToZoo(zoo, "Ilya", "Krasnoperov");
            AddZooKeeperToZoo(zoo, "Ksenia", "Narusheva");
            AddVeterinatianToZoo(zoo, "Anton", "Antonov");
            AddVeterinatianToZoo(zoo, "Egor", "Egorov");
        }

        private void AddVeterinatianToZoo(Zoo zoo, string FirstName, string LastName)
        {
            var veterinarian = new Veterinarian(FirstName, LastName, _iConsole);
            foreach (var animal in zoo.Animals)
            {
                veterinarian.AddAnimalExperiences(GetTypeAnimalFromName(animal));
            }
            zoo.HireEmployee(veterinarian);
        }

        private void AddZooKeeperToZoo(Zoo zoo, string FirstName, string LastName)
        {
            var zooKeeper = new ZooKeeper(FirstName, LastName, _iConsole);
            foreach (var animal in zoo.Animals)
            {
                zooKeeper.AddAnimalExperiences(GetTypeAnimalFromName(animal));
            }
            zoo.HireEmployee(zooKeeper);
        }
        public Animal GetTypeAnimalFromName(string name)
        {
            switch(name)
            {
                case "Bison": return new Bison();
                case "Lion": return new Lion();
                case "Elephant": return new Elephant();
                case "Penguin": return new Penguin();
                case "Turtle": return new Turtle();
                case "Parrot": return new Parrot();
                default: return null;
            }
        }


        private void InitializeAnimals(Zoo zoo, DateTime dateTime)
        {
            List<List<int>> feedSchedule = new()
            {
                new List<int>() { 6, 18 },
                new List<int>() { 11, 23 },
                new List<int>() { 5, 17},
            };
            List<Animal> animals = new()
            {
                new Bison(), new Bison(), new Lion(), new Elephant(),
                new Penguin(), new Penguin(), new Parrot(), new Parrot(),
                new Turtle(), new Turtle(), new Parrot(), new Parrot()
            };
            Random rand = new Random();
            foreach(var animal in animals)
            {
                animal.AddFeedSchedule(feedSchedule[rand.Next(3)]);
                zoo.FindAviableEnclosure(animal).AddAnimals(animal);
            }
        }

        private void InitializeEnclosures(Zoo zoo)
        {
            zoo.AddEnclosure("First enclosure", 50000);
            zoo.AddEnclosure("Second enclosure", 10000);
            zoo.AddEnclosure("Third enclosure", 10000);
            zoo.AddEnclosure("Fourth enclosure", 10000);
            zoo.AddEnclosure("Fifth enclosure", 70000);
            zoo.AddEnclosure("Sixth enclosure", 70000);
            zoo.AddEnclosure("Seventh enclosure", 70000);
        }

        public void AddZoo(Zoo zoo)
        {
            if (zoo is null)
            {
                _iConsole?.WriteLine("The zoo is not provided.");
                throw new ArgumentNullException(nameof(zoo));
            }
            if (Zoos.Exists(z => z.Location == zoo.Location))
            {
                _iConsole?.WriteLine($"The zoo at location '{zoo.Location}' already added into ZooCorp");
                throw new ArgumentException(nameof(zoo));
            }
            Zoos.Add(zoo);
            _iConsole?.WriteLine($"Added the zoo at '{zoo.Location}'.");
        }

    }
}
