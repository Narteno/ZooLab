using System;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class VeterinarianTest
    {
        [Fact]
        public void ShoulBeAbleToCrateVeterinarian()
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov");
            Assert.Equal("Ilya", veterinarian.FirstName);
            Assert.Equal("Krasnoperov", veterinarian.LastName);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddExperiencesWithNoAnimal(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            var exception = Assert.Throws<ArgumentNullException>(() => veterinarian.AddAnimalExperiences(null));
            Assert.Equal("animal", exception.ParamName);
            if(console is not null)
            {
                Assert.Equal("The animal is not provided\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddExperiencesWhenHasExperiences(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            veterinarian.AnimalExperiences.Add("Lion");
            Lion lion = new();
            veterinarian.AddAnimalExperiences(lion);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov is already experienced with Lion.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToAddExperiences(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            Assert.Empty(veterinarian.AnimalExperiences);
            Lion lion = new();
            veterinarian.AddAnimalExperiences(lion);
            Assert.Equal("Lion", veterinarian.AnimalExperiences[0]);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov is start experienced with Lion.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToHealAnimal(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            veterinarian.AnimalExperiences.Add("Snake");
            Snake snake = new();
            snake.IsSick = true;
            veterinarian.HealAnimals(snake, new Antibiotics());
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov heals Snake <0> with Antibiotics.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailToHealAnimalWhenVeterinarianHasNoExperiences(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            Snake snake = new();
            snake.IsSick = true;
            var exception = Assert.Throws<NoNeededExperienceException>(()=> veterinarian.HealAnimals(snake, new Antibiotics()));
            Assert.Equal("Snake", exception.Message);
            if (console is not null)
            {
                Assert.Equal("Ilya Krasnoperov has no experiences with Snake\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailToHealAnimalThatHealthy(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            veterinarian.AnimalExperiences.Add("Snake");
            Snake snake = new();
            veterinarian.HealAnimals(snake, new Antibiotics());
            if (console is not null)
            {
                Assert.Equal("The Snake is not sick.\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailToHealAnimalWithNoAnimal(TestConsole console)
        {
            Veterinarian veterinarian = new("Ilya", "Krasnoperov", console);
            var exception = Assert.Throws<ArgumentNullException>(() => veterinarian.HealAnimals(null, new Antibiotics()));
            Assert.Equal("animal", exception.ParamName);
            if (console is not null)
            {
                Assert.Equal("The animal is not provided\n", console.outputMessage);
            }
        }
    }
}
