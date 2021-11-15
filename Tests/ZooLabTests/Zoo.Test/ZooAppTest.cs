using System;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class ZooAppTest
    {
        [Fact]
        public void ShouldBeAbleToInitializeZoos()
        {
            ZooApp zoos = new();
            zoos.Initialize(DateTime.Now);
            Assert.NotEmpty(zoos.Zoos);
            Assert.NotEmpty(zoos.Zoos[0].Enclosures);
            Assert.NotEmpty(zoos.Zoos[0].Employees);
            Assert.NotEmpty(zoos.Zoos[0].Enclosures[0].Animals);
            Assert.NotEmpty(zoos.Zoos[0].Enclosures[1].Animals);
            Assert.NotEmpty(zoos.Zoos[0].Enclosures[2].Animals);
            Assert.NotEmpty(zoos.Zoos[1].Enclosures[0].Animals);
            Assert.NotEmpty(zoos.Zoos[1].Enclosures[1].Animals);
            var exception = Assert.Throws<ArgumentNullException>(() => zoos.AddZoo(null));
            Assert.Equal("zoo", exception.ParamName);
            var exception2 = Assert.Throws<ArgumentException>(() => zoos.AddZoo(new Zoo("Tomsk, Russia")));
            Assert.Equal("zoo", exception.ParamName);
            Assert.Null(zoos.GetTypeAnimalFromName("Potato animal"));
        }
    }
}
