using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class AnimalTest
    {
        [Fact]
        public void ShoulBeAbleToCreateMammals()
        {
            Bison bison = new Bison();
            Lion lion = new Lion();
            Elephant elephant = new Elephant();
        }

        [Fact]
        public void ShouldBeAbleToFeedAnimalWithNoFood()
        {
            Bison bison = new();
            var exception = Assert.Throws<ArgumentNullException>(() => bison.Feed(null, new ZooKeeper("a", "b"), DateTime.Now));
            Assert.Empty(bison.FeedTimes);
            Assert.Equal("food", exception.ParamName);
        }
        [Fact]
        public void ShouldBeAbleToFeedAnimalWithNoZooKeeper()
        {
            Lion lion = new();
            var exception = Assert.Throws<ArgumentNullException>(() => lion.Feed(new Grass(),null, DateTime.Now));
            Assert.Empty(lion.FeedTimes);
            Assert.Equal("zooKeeper", exception.ParamName);
        }
        [Fact]
        public void ShouldBeAbleToFeedAnimalWithNoFavoriteFood()
        {
            Elephant elephant = new();
            elephant.Feed(new Grass(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.Empty(elephant.FeedTimes);

            Bison bison = new();
            bison.Feed(new Meet(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.True(bison.FeedTimes.Count == 1);

            Lion lion = new();
            lion.Feed(new Meet(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.True(lion.FeedTimes.Count == 1);

            Snake snake = new();
            snake.Feed(new Meet(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.True(snake.FeedTimes.Count == 1);

            Turtle turtle = new();
            turtle.Feed(new Meet(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.Empty(turtle.FeedTimes);
        }
        [Fact]
        public void ShouldBeAbleToFeedAnimalWithFavoriteFood()
        {
            Penguin penguin = new();
            penguin.Feed(new Meet(), new ZooKeeper("a", "b"), DateTime.Now);
            Assert.True(penguin.FeedTimes.Count == 1);
        }
        [Fact]
        public void ShouldBeAbleToAddScheduleWithNullListOfSchedule()
        {
            Parrot parrot = new();
            var exception = Assert.Throws<ArgumentNullException>(() => parrot.AddFeedSchedule(new List<int> { }));
            Assert.Empty(parrot.FeedSchedule);
            Assert.Equal("hours", exception.ParamName);
        }
        [Fact]
        public void ShouldBeAbleToAddScheduleWithWrongListOfSchedule()
        {
            Snake snake = new();
            var exception = Assert.Throws<ArgumentException>(() => snake.AddFeedSchedule(new List<int> { 4, 25 }));
            Assert.Empty(snake.FeedSchedule);
            Assert.Equal("hoursItem", exception.ParamName);
        }
        [Fact]
        public void ShouldBeAbleToAddSchedule()
        {
            Turtle turtle = new();
            turtle.AddFeedSchedule(new List<int> { 4, 8 });
            Assert.True(turtle.FeedSchedule.Count == 2);
        }
        [Fact]
        public void ShouldBeAbleToHealNotSickAnimal()
        {
            Turtle turtle = new();
            turtle.IsSick = false;
            turtle.Heal(new Antibiotics());
            Assert.True(!turtle.IsSick);
        }
        [Fact]
        public void ShouldBeAbleToHealAnimalWithNullMedicine()
        {
            Turtle turtle = new();
            turtle.IsSick = true;
            var exception = Assert.Throws<ArgumentNullException>(() => turtle.Heal(null));
            Assert.True(turtle.IsSick);
        }
        [Fact]
        public void ShouldBeAbleToHealAnimal()
        {
            Turtle turtle = new();
            turtle.IsSick = true;
            turtle.Heal(new Antibiotics());
            Assert.True(!turtle.IsSick);
        }
    }
}
