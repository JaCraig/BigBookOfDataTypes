using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace BigBook.Tests
{
    public class TaskQueueTests
    {
        private readonly double EPSILON = 0.0001d;

        [Fact]
        public void MoreComplexTasks()
        {
            var Results = new double[100];
            var TestObject = new TaskQueue<int>(4, x => { Results[x] = 2 * F(1); return true; });
            for (int x = 0; x < 100; ++x)
            {
                Assert.True(TestObject.Enqueue(x));
            }
            while (!TestObject.IsComplete)
            {
                Thread.Sleep(100);
            }

            Assert.True(Results.All(x => System.Math.Abs(x - 3.14159d) < EPSILON));
        }

        [Fact]
        public void SimpleTasks()
        {
            var Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            var TestObject = new TaskQueue<int>(4, x => { Builder.Append(x); return true; });
            for (int x = 0; x < Temp.Length; ++x)
            {
                Assert.True(TestObject.Enqueue(Temp[x]));
            }
            while (!TestObject.IsComplete)
            {
                Thread.Sleep(100);
            }

            var OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        private double F(int i)
        {
            if (i == 1000)
            {
                return 1;
            }

            return 1 + (i / ((2.0 * i) + 1) * F(i + 1));
        }
    }
}