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
            Producer producer = new(blockingCollection, count);

            // Create threads using Thread and ThreadStart:
            //new Thread(new ThreadStart(consumer.Run)).Start();
            //new Thread(new ThreadStart(producer.Run)).Start();

            // Create parent-child thread using Task Parallel Library:
            Task parentTask = Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() => consumer.Run(), TaskCreationOptions.AttachedToParent);
                Task.Factory.StartNew(() => producer.Run(), TaskCreationOptions.AttachedToParent)
                    .ContinueWith(t => Console.WriteLine("The consumer and producer are done with blocking collection."));
            });

            parentTask.Wait();

        }
    }
}
