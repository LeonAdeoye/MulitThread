using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    internal class Producer
    {
        private readonly BlockingCollection<int> _blockingCollection;
        private readonly int _count;

        public Producer(BlockingCollection<int> blockingCollection, int count)
        {
            _count = count;
            _blockingCollection = blockingCollection;
        }
        
        public void Run()
        {
            for(int i = 0; i < _count; ++i)
            {
                int sleepTime = new Random().Next(1, _count) * 10;
                Thread.Sleep(sleepTime);
                _blockingCollection.Add(i);
                Console.WriteLine($"Added: {i} after {sleepTime} to blocking collection.");

            }
        }
    }
}
