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
        public static async void Main()
        {
            BlockingCollection<int> blockingCollection = new();
            Consumer consumer = new(blockingCollection, 10);
            Producer producer = new(blockingCollection, 10);
            Task consumerTask = Task.Run(() => consumer.Run());
            Task producerTask = Task.Run(() => producer.Run());
            await Task.WhenAll(new List<Task> { consumerTask, producerTask });
            Console.WriteLine("The consumer and producer are done with blocking collection.");
        }
    }
}
