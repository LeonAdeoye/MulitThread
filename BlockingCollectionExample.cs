using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    internal class BlockingCollectionExample
    {
        public static void Main()
        {
            BlockingCollection<int> blockingCollection = new();
            int count = 10;

            Consumer consumer = new(blockingCollection, count);
            Thread consumerThread = new(new ThreadStart(consumer.Run));

            Producer producer = new(blockingCollection, count);
            Thread producerThread = new(new ThreadStart(producer.Run));

            consumerThread.Start();
            producerThread.Start();
        }
    }
}
