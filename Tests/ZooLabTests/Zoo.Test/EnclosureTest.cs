using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class EnclosureTest
    {
        [Fact]
        public void ShouldBeAbleToCreateEnclosure()
        {
            Zoo zoo = new("Tomsk, Russia");
            Enclosure enclosure = new("Barraks for lions",5000, zoo);
            Assert.Equal("Barraks for lions", enclosure.Name);
            Assert.Equal(5000, enclosure.SquareFeet);
            Assert.Equal("Tomsk, Russia", enclosure.ParentZoo.Location);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldBeAbleToAndAddAnimal(TestConsole console)
        {
            Zoo zoo = new("Tomsk, Russia");
            Enclosure enclosure = new("Barraks for lions", 5000, zoo, console);
            Lion lion = new Lion();
            Assert.True(!lion.IDExists);
            Assert.Empty(enclosure.Animals);
            enclosure.AddAnimals(lion);
            Assert.True(lion.IDExists);
            Assert.True(enclosure.Animals.Count == 1);
            Assert.Equal(4000, enclosure.FreeSquareFeet);
            Assert.Equal("Lion", zoo.Animals[0]);
            if (console is not null)
            {
                Assert.Equal("Added Lion <1>.\n", console.outputMessage);
            }
            console?.Clear();

            enclosure = new("Planet of birds", 1000, zoo, console);
            Penguin penguin = new();
            enclosure.AddAnimals(penguin);
            penguin = new();
            enclosure.AddAnimals(penguin);
            Assert.True(enclosure.Animals.Count == 2);

            enclosure = new("Planet of birds 2", 1000, zoo, console);
            Parrot parrot = new();
            enclosure.AddAnimals(parrot);
            parrot = new();
            enclosure.AddAnimals(parrot);
            Assert.True(enclosure.Animals.Count == 2);

            enclosure = new("Planet of bisons", 3000, zoo, console);
            Bison bison = new();
            enclosure.AddAnimals(bison);
            Elephant elephant = new();
            enclosure.AddAnimals(elephant);
            Assert.True(enclosure.Animals.Count == 2);

            enclosure = new("Planet of elephants", 3000, zoo, console);
            elephant = new();
            enclosure.AddAnimals(elephant);
            Turtle turtle = new();
            enclosure.AddAnimals(turtle);

            enclosure = new("Planet of reptiles", 3000, zoo, console);
            Snake snake = new();
            enclosure.AddAnimals(snake);
            snake = new();
            enclosure.AddAnimals(snake);

            enclosure = new("Planet of reptiles", 3000, zoo, console);
            turtle = new();
            enclosure.AddAnimals(turtle);
            turtle = new();
            enclosure.AddAnimals(turtle);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddAnimnalIfIDAssigned(TestConsole console)
        {
            var zoo = new Zoo("Tomsk, Russia");
            var enclosure = new Enclosure("Barraks for lions", 5000, zoo, console);
            Lion lion = new Lion();
            enclosure.AddAnimals(lion);
            Assert.True(lion.IDExists);
            var exception = Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimals(lion));
            Assert.Equal("AssignID", exception.Message);
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddAnimalByAnimalNull(TestConsole console)
        {
            var zoo = new Zoo("Tomsk, Russia");
            var enclosure = new Enclosure("Barraks for lions", 5000, zoo, console);
            var exception = Assert.Throws<ArgumentNullException>(() => enclosure.AddAnimals(null));
            Assert.Equal("animal", exception.ParamName);
            Assert.Empty(enclosure.Animals);
            if (console is not null)
            {
                Assert.Equal("Animal is required\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddAnimalWithUnfriendlyEnimal(TestConsole console)
        {
            var zoo = new Zoo("Tomsk, Russia");
            var enclosure = new Enclosure("Barraks for lions", 5000, zoo, console);
            Lion lion = new();
            enclosure.AddAnimals(lion);
            console?.Clear();
            Snake snake = new();
            var exception = Assert.Throws<NoFriendlyAnimalException>(() => enclosure.AddAnimals(snake));
            Assert.Equal("Snake", exception.Message);
            if (console is not null)
            {
                Assert.Equal("There are no friendly animal with Snake\n", console.outputMessage);
            }
        }
        [Theory]
        [ClassData(typeof(TestConsole.TestConsoleOrNull))]
        public void ShouldFailAddAnimalWithHugeSpace(TestConsole console)
        {
            var zoo = new Zoo("Tomsk, Russia");
            var enclosure = new Enclosure("Barraks for lions", 500, zoo, console);
            Elephant elephant = new();
            var exception = Assert.Throws<NoAvailableSpaceException>(() => enclosure.AddAnimals(elephant));
            Assert.Equal("animal", exception.Message);
            if (console is not null)
            {
                Assert.Equal("Not enough free square feet for Elephant\n", console.outputMessage);
            }
        }
    }
}
