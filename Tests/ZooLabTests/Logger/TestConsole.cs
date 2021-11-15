using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZooLab;

namespace ZooLabTests
{
    public class TestConsole : IConsole
    {
        private readonly StringBuilder _Output;
        public string outputMessage => _Output.ToString();
        TestConsole()
        {
            _Output = new();
        }
        public void Clear()
        {
            _Output.Clear();
        }
        public void WriteLine(string message)
        {
            _Output.Append($"{message}\n");
        }
        public class TestConsoleOrNull : TheoryData<TestConsole>
        {
            public TestConsoleOrNull()
            {
                Add(null);
                Add(new TestConsole());
            }
        }
    }
}
