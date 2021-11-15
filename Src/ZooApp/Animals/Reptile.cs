using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public abstract class Reptile : Animal
    {

    }
    public class Snake : Reptile
    {
        private string[] favoriteFood = new[] { "Meet" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 2;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Snake;
        }
    }
    public class Turtle : Reptile
    {
        private string[] favoriteFood = new[] { "Grass" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 5;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Turtle or Parrot or Bison or Elephant;
        }
    }

}
