using System;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class ZooKeeperTest
    {
        [Fact]
        public void ShoulBeAbleToCrateZooKeeper()
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov");
            Assert.Equal("Ilya", zooKeeper.FirstName);
            Assert.Equal("Krasnoperov", zooKeeper.LastName);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddExperiencesWithNoAnimal(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            var exception = Assert.Throws<ArgumentNullException>(() => zooKeeper.AddAnimalExperiences(null));
            Assert.Equal("animal", exception.ParamName);
            if (console is not null)
            {
                Assert.Equal("The animal is not provided\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddExperiencesWhenAlreadyHasExperiences(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            zooKeeper.AnimalExperiences.Add("Parrot");
            Parrot parrot = new();
            zooKeeper.AddAnimalExperiences(parrot);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov is already experienced with Parrot.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToAddExperiences(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            Parrot parrot = new();
            zooKeeper.AddAnimalExperiences(parrot);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov is start experienced with Parrot.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToFeedAnimal(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            Parrot parrot = new();
            parrot.FeedSchedule.Add(5);
            parrot.FeedSchedule.Add(17);
            DateTime dateTime = Convert.ToDateTime("2005-05-05 17:12 PM");
            zooKeeper.AnimalExperiences.Add("Parrot");
            console?.Clear();
            zooKeeper.FeedAnimal(parrot, new Grass(), dateTime);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov fed Parrot <0> with Grass.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailFeedAnimalByAnimalIsNull(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            var exception = Assert.Throws<ArgumentNullException>(() => zooKeeper.FeedAnimal(null, new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM")));
            Assert.Equal("animal", exception.ParamName);
            if (console is not null)
            {
                Assert.Equal("The animal is not provided\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailFeedAnimalWithoutExperiences(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            Penguin penguin = new();
            zooKeeper.FeedAnimal(penguin, new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM"));
            if (console is not null)
            {
                Assert.Equal("You have no experience to feed Penguin\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailFeedAnimalBySchedule(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            zooKeeper.AnimalExperiences.Add("Penguin");
            Penguin penguin = new();
            penguin.FeedSchedule.Add(5);
            penguin.FeedSchedule.Add(16);
            zooKeeper.FeedAnimal(penguin, new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM"));
            if (console is not null)
            {
                Assert.Equal("It's not time to feed the Penguin\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailFeedAnimalByNullSchedule(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            zooKeeper.AnimalExperiences.Add("Penguin");
            Penguin penguin = new();
            var exception = Assert.Throws<ArgumentNullException>(() => zooKeeper.FeedAnimal(penguin, new Grass(), Convert.ToDateTime("2005-05-05 17:12 PM")));
            Assert.Equal("FeedSchedule", exception.ParamName);
            if (console is not null)
            {
                Assert.Equal("Your feed schedule was null or empty\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailFeedAnimalByDailyLimit(TestConsole console)
        {
            ZooKeeper zooKeeper = new("Ilya", "Krasnoperov", console);
            zooKeeper.AnimalExperiences.Add("Penguin");
            Penguin penguin = new();
            penguin.FeedSchedule.Add(5);
            penguin.FeedSchedule.Add(17);
            penguin.FeedTimes.Add(new FeedTime(Convert.ToDateTime("2005-05-05 05:12 PM"), zooKeeper));
            penguin.FeedTimes.Add(new FeedTime(Convert.ToDateTime("2005-05-05 17:12 PM"), zooKeeper));
            zooKeeper.FeedAnimal(penguin, new Grass(), Convert.ToDateTime("2005-05-05 17:15 PM"));
            if (console is not null)
            {
                Assert.Equal("Today the Penguin has already been fed 2 times\n", console.outputMessage);
            }
        }
    }
}
