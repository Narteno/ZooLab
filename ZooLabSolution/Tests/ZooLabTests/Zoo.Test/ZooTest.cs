using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class ZooTest
    {
        [Fact]
        public void ShouldBeAbleToCreateZoo()
        {
            Zoo zoo = new("Tomsk, Russia");
            Assert.Equal("Tomsk, Russia", zoo.Location);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToAddEnclosure(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            var exceptionString = Assert.Throws<ArgumentNullException>(() => zoo.AddEnclosure("", 5000));
            Assert.Equal("Name", exceptionString.ParamName);
            if (console is not null) 
                Assert.Equal("'Name' cannot be null or empty\n", console.outputMessage);
            console?.Clear();

            var exceptionInt = Assert.Throws<ArgumentException>(() => zoo.AddEnclosure("Enlosure one", -700));
            Assert.Equal("SquareFeet", exceptionInt.Message);
            if (console is not null)
                Assert.Equal("'Square Feet' must be greater then 0\n", console.outputMessage);

            zoo.AddEnclosure("Enclosure for lions", 5000);
            console?.Clear();
            exceptionInt = Assert.Throws<ArgumentException>(() => zoo.AddEnclosure("Enclosure for lions", 2000));
            Assert.Equal("Name", exceptionInt.Message);
            if (console is not null)
                Assert.Equal("Enclosure with same name already exists\n", console.outputMessage);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToFindAviableEnclosures(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            Bison bison = new();
            var exception = Assert.Throws<ArgumentNullException>(()=> zoo.FindAviableEnclosure(null));
            Assert.Equal("animal", exception.ParamName);
            if (console is not null)
                Assert.Equal("You should choose correct animal\n", console.outputMessage);
            console?.Clear();

            var exceptionEnclosure = Assert.Throws<NoAvailableEnclosureException>(() => zoo.FindAviableEnclosure(bison));
            if (console is not null)
                Assert.Equal("No aviable enclosures\n", console.outputMessage);

            zoo.AddEnclosure("Enlosure one", 5000);
            zoo.AddEnclosure("Enlosure two", 500);
            zoo.AddEnclosure("Enlosure three", 2000);
            zoo.AddEnclosure("Enlosure four", 7000);
            console?.Clear();
            zoo.FindAviableEnclosure(bison);
            if (console is not null)
                Assert.Equal("Not enough free square feet for Bison\n" +
                    "Found available Bison enclosure 'Enlosure three'\n", console.outputMessage);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToHireEmployee(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            var exception = Assert.Throws<ArgumentNullException>(() => zoo.HireEmployee(null));
            Assert.Equal("employee", exception.ParamName);
            if (console is not null)
                Assert.Equal("Employee is not provided\n", console.outputMessage);

            ZooKeeper zooKeeper = new("Ilya", "Kra", console);
            console?.Clear();
            zoo.HireEmployee(zooKeeper);
            if (console is not null)
                Assert.Equal("Cannot hire an employee: ZooKeeper Ilya Kra.\n" +
                    "AnimalExperiences : Must have experience with any animals in the hiring zoo.\n", console.outputMessage);

            zoo.AddEnclosure("Space", 3000);
            zoo.Enclosures[0].AddAnimals(new Bison());
            zooKeeper.AddAnimalExperiences(new Bison());
            console?.Clear();
            zoo.HireEmployee(zooKeeper);
            if (console is not null)
                Assert.Equal("Hired an employee: ZooKeeper Ilya Kra.\n", console.outputMessage);

            Veterinarian veterinarian = new("Ilya", "Kra", console);
            console?.Clear();
            zoo.HireEmployee(veterinarian);
            if (console is not null)
                Assert.Equal("Cannot hire an employee: Veterinarian Ilya Kra.\n" +
                    "AnimalExperiences : Must have experience with all animals present in the hiring zoo.\n", console.outputMessage);
            veterinarian.AddAnimalExperiences(new Bison());
            console?.Clear();
            zoo.HireEmployee(veterinarian);
            if (console is not null)
                Assert.Equal("Hired an employee: Veterinarian Ilya Kra.\n", console.outputMessage);


        }
        class NonExistenEmployee : IEmployee 
        {
            public string FirstName { get; private set; }
            public string LastName { get; private set; }
            public NonExistenEmployee(string firstName, string lastName)
            {
                FirstName = firstName; LastName = lastName;
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeFailToHireNonExistenEmployee(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            NonExistenEmployee nonExistenEmployee = new("Ilya", "Kra");
            var exception = Assert.Throws<NotImplementedException>(()=> zoo.HireEmployee(nonExistenEmployee));
            Assert.Equal("employee", exception.Message);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeToFeedAllAnimals(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            ZooKeeper zooKeeper = new("Ilya", "Kra", console);
            zoo.AddEnclosure("Eclosure for Lions", 3000);
            zoo.AddEnclosure("Eclosure for Bison and Elephants", 5000);
            zoo.AddEnclosure("Eclosure for Elephant and Turtle", 2000);
            List<int> hours = new() { 5, 17 };
            zoo.Enclosures[0].AddAnimals(new Lion());
            zoo.Enclosures[0].AddAnimals(new Lion());
            zoo.Enclosures[0].Animals[0].AddFeedSchedule(hours);
            zoo.Enclosures[0].Animals[1].AddFeedSchedule(hours);
            zoo.Enclosures[1].AddAnimals(new Bison());
            zoo.Enclosures[1].AddAnimals(new Elephant());
            zoo.Enclosures[1].Animals[0].AddFeedSchedule(hours);
            zoo.Enclosures[1].Animals[1].AddFeedSchedule(hours);
            zoo.Enclosures[2].AddAnimals(new Elephant());
            zoo.Enclosures[2].AddAnimals(new Turtle());
            zoo.Enclosures[2].Animals[0].AddFeedSchedule(hours);
            zoo.Enclosures[2].Animals[1].AddFeedSchedule(hours);
            zooKeeper.AddAnimalExperiences(new Bison()); zooKeeper.AddAnimalExperiences(new Lion());
            zooKeeper.AddAnimalExperiences(new Elephant()); zooKeeper.AddAnimalExperiences(new Turtle());
            zoo.HireEmployee(zooKeeper);
            zooKeeper.FeedAnimal(zoo.Enclosures[2].Animals[1], new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM"));
            zooKeeper.FeedAnimal(zoo.Enclosures[2].Animals[1], new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM"));
            Assert.True(zoo.Enclosures[2].Animals[1].FeedTimes.Count == 2);
            console?.Clear();
            zoo.FeedAnimals(Convert.ToDateTime("2005-05-05 17:12 PM"));
            Assert.True(zoo.Enclosures[2].Animals[1].FeedTimes.Count == 2);
            if (console is not null)
                Assert.Equal("Ilya Kra fed Lion <1> with Meet.\n" +
                    "Ilya Kra fed Lion <2> with Meet.\n" +
                    "Ilya Kra fed Bison <3> with Meet.\n" +
                    "Ilya Kra fed Elephant <4> with Vegetable.\n" +
                    "Ilya Kra fed Elephant <5> with Vegetable.\n" +
                    "Today the Turtle has already been fed 2 times\n" +
                    "The Turtle from Eclosure for Elephant and Turtle was not fed.\n", console.outputMessage);
            zoo.GetTypeFoodByTheName("Soup");
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeToHealAllAnimals(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia", console);
            Veterinarian veterinarian = new("Ilya", "Kra", console);
            veterinarian.AddAnimalExperiences(new Bison()); veterinarian.AddAnimalExperiences(new Lion());
            veterinarian.AddAnimalExperiences(new Elephant());
            zoo.AddEnclosure("Eclosure for Lions", 3000);
            zoo.AddEnclosure("Eclosure for Bison and Elephants", 5000);
            zoo.AddEnclosure("Eclosure for Elephant and Turtle", 2000);
            List<int> hours = new() { 5, 17 };
            zoo.Enclosures[0].AddAnimals(new Lion());
            zoo.Enclosures[0].AddAnimals(new Lion());
            zoo.Enclosures[0].Animals[0].IsSick = true;
            zoo.Enclosures[0].Animals[1].IsSick = true;
            zoo.Enclosures[1].AddAnimals(new Bison());
            zoo.Enclosures[1].AddAnimals(new Elephant());
            zoo.Enclosures[1].Animals[0].AddFeedSchedule(hours);
            zoo.Enclosures[1].Animals[1].IsSick = true;
            zoo.Enclosures[2].AddAnimals(new Elephant());
            zoo.Enclosures[2].AddAnimals(new Turtle());
            zoo.Enclosures[2].Animals[0].AddFeedSchedule(hours);
            zoo.Enclosures[2].Animals[1].IsSick = true;
            zoo.HireEmployee(veterinarian);
            Assert.NotEmpty(zoo.Employees);
            console?.Clear();
            zoo.HealAnimals();
            if (console is not null)
                Assert.Equal("Ilya Kra heals Lion <1> with Antibiotics.\n" +
                    "Ilya Kra heals Lion <2> with Antibiotics.\n" +
                    "Ilya Kra heals Elephant <4> with Antibiotics.\n" +
                    "The Turtle from Eclosure for Elephant and Turtle was not healed : there are no Veterinarian that have experienced to heal it\n", console.outputMessage);
        }
    }
}
