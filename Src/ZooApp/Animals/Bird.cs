using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public abstract class Bird : Animal
    {

    }
    public class Parrot : Bird
    {
        private string[] favoriteFood = new[] { "Vegetable", "Grass" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 5;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Parrot or Bison or Elephant or Turtle;
        }
    }
    public class Penguin : Bird
    {
        private string[] favoriteFood = new[] { "Meet" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 10;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Penguin;
        }
    }
}
