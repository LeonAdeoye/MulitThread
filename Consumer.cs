using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MulitThread
{
    internal class Consumer
    {
        private readonly BlockingCollection<int> _blockingCollection;
        private int _count;

        public Consumer(BlockingCollection<int> blockingCollection, int count)
        {
            _blockingCollection = blockingCollection;
            _count = count;
        }

        public void Run()
        {
            int currentCount = 0;
            while (currentCount < _count)
            {
                int sleepTime = new Random().Next(1, _count) * 10;
                Thread.Sleep(sleepTime);
                int i = _blockingCollection.Take();
                Console.WriteLine($"Removed: {i} after {sleepTime} from blocking collection.");
                ++currentCount;
            }
        }
    }
}
