using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooLab
{
    public abstract class Animal
    {
        private readonly IConsole _iConsole;

        public abstract int RequiredSpaceSqFt { get; }
        public abstract string[] FavoriteFood { get; }
        public  int MaxDailyFeedings { get; } = 2;
        public List<FeedTime> FeedTimes { get; protected set; } = new();
        public List<int> FeedSchedule { get; protected set; } = new();
        public bool IsSick = false;
        public int ID { get; protected set; }
        public bool IDExists { get; protected set; } = false;

        public Animal(IConsole console = null)
        {
            _iConsole = console;
        }

        public bool AssignID(int id)
        {
            if (IDExists)
            {
                return false;
            }
            ID = id; IDExists = true; return true;
        }
        public abstract bool IsFriendlyWith(Animal animal);
        public void Feed(Food food, ZooKeeper zooKeeper, DateTime dateTime)
        {
            if(food == null)
            {
                _iConsole?.WriteLine("You have no food to feed animal");
                throw new ArgumentNullException(nameof(food));
            }
            if (zooKeeper == null)
            {
                _iConsole?.WriteLine("There is no zoo keeper");
                throw new ArgumentNullException(nameof(zooKeeper));
            }
            string NameOfFood = food.GetType().Name;
            if (FavoriteFood.Contains(NameOfFood))
            {
                FeedTimes.Add(new FeedTime(dateTime, zooKeeper));
                _iConsole?.WriteLine($"{this.GetType().Name} <{ID}> was fed with {NameOfFood} in Enclose <<Name>>"); // ?question?
            }
            else
            {
                _iConsole?.WriteLine($"There are no '{NameOfFood}' that the {this.GetType().Name} loves. He did not eat.");
            }
        }
        public void AddFeedSchedule(List<int> hours)
        {
            if (hours.Count == 0)
            {
                _iConsole?.WriteLine("Cannot add empty schedule");
                throw new ArgumentNullException(nameof(hours));
            }
            hours.ForEach(hoursItem =>
            {
                if (hoursItem < 0 || hoursItem > 24)
                {
                    _iConsole?.WriteLine("Hours cannot be negative or more than 24");
                    throw new ArgumentException(nameof(hoursItem), nameof(hoursItem));
                }
            });
            FeedSchedule = hours;
            _iConsole?.WriteLine($"Added feeding schedule for {this.GetType().Name} <{ID}>:");
            hours.ForEach(hoursItem =>
            {
                _iConsole?.WriteLine($"{hoursItem}:00");
            });
        }
        public void Heal(Medicine medicine)
        {
            if(!IsSick)
            {
                _iConsole?.WriteLine($"{this.GetType().Name} <{ID}> don't need to heal");
                return;
            }
            if(medicine == null)
            {
                _iConsole?.WriteLine("You cannot heal without medicine.");
                throw new ArgumentNullException(nameof(medicine));
            }
            IsSick = false;
            _iConsole?.WriteLine($"{this.GetType().Name} <{ID}> was healed with {medicine.GetType().Name}");
        }
    }
    public class FeedTime
    {
        public DateTime feedTime { get; }
        public ZooKeeper FeedByZooKeeper { get; set; }
        public FeedTime(DateTime dateTime, ZooKeeper zooKeeper)
        {
            feedTime = dateTime;
            FeedByZooKeeper = zooKeeper;
        }
    }
}
