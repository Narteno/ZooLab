using System;
using System.Runtime.Serialization;

namespace ZooLab
{
    [Serializable]
    public class NoNeededExperienceException : Exception
    {
        public NoNeededExperienceException(string message) : base(message)
        {
        }
    }
    public class NoAvailableEnclosureException : Exception
    {
        public NoAvailableEnclosureException(string message) : base(message)
        {
        }
    }
    public class NoAvailableSpaceException : Exception
    {
        public NoAvailableSpaceException(string message) : base(message)
        {
        }
    }
    public class NoFriendlyAnimalException : Exception
    {
        public NoFriendlyAnimalException(string message) : base(message)
        {
        }
    }
}