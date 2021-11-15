using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public abstract class Mammal : Animal
    {

    }
    public class Bison : Mammal
    {
        private string[] favoriteFood = new[] { "Meet" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 1000;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Elephant;
        }
    }
    public class Elephant : Mammal
    {
        private string[] favoriteFood = new[] { "Vegetable" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 1000;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Bison or Parrot or Turtle;
        }
    }
    public class Lion : Mammal
    {
        private string[] favoriteFood = new[] { "Meet" };
        public override string[] FavoriteFood => favoriteFood;
        public override int RequiredSpaceSqFt => 1000;
        public override bool IsFriendlyWith(Animal animal)
        {
            return animal is Lion;
        }
    }
}
