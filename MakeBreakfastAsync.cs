namespace MulitThread
{
    class Egg  {}

    class Bacon {}

    class Toast {}

    internal class MakeBreakfastAsync
    {
        private static void ApplyButter() => Console.WriteLine("Spreading butter on the toast.");

        private static async Task<Toast> MakeToastWithButterAsync(int quantity)
        {
            Console.WriteLine($"Putting {quantity} slices of bread in the toaster.");
            Console.WriteLine($"Toasting bread.");
            await Task.Delay(500);
            for (int slices = 0; slices < quantity; ++slices)
            {
                ApplyButter();
                Console.WriteLine($"Putting the slice of bread on a plate.");
            }
            return new();
        }

        // This is not an Async method, and does not return a Task. It is a CPU-bound operation. However, you can run it asynchronously using Task.Run().
        private static long ThinkAboutLife(long times)
        {
            long totalThoughts = 0;

            for (long thoughtCounts = 0; thoughtCounts < times; ++thoughtCounts)
                totalThoughts++;

            return totalThoughts;
        }

        private static async Task<Egg> FryEggsAsync(int quantity)
        {
            Console.WriteLine($"Cracking open {quantity} eggs into the pan.");
            Console.WriteLine($"Cooking the eggs.");
            await Task.Delay(500);
            for (int eggCount = 0; eggCount < quantity; ++eggCount)
            {
                Console.WriteLine($"Putting the egg on a plate.");
            }
            return new();
        }

        private static async Task<Bacon> FryBaconAsync(int quantity)
        {
            Console.WriteLine($"Putting {quantity} slices of bacon in the pan.");
            Console.WriteLine($"Cooking the first side of bacon.");
            await Task.Delay(500);
            for (int slices = 0; slices < quantity; ++slices)
            {
                Console.WriteLine($"Flipping the slice of bacon.");
            }
            Console.WriteLine($"Cooking the second side of bacon.");
            await Task.Delay(400);
            for (int slices = 0; slices < quantity; ++slices)
            {
                Console.WriteLine($"Putting the slice of bacon on a plate.");
            }
            return new();
        }

        public static async Task Main()
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            // Use Task.Run to wrap an asynchronous bubble around a method call that is synchronous, making the method call asynchronous. 
            var thinkingTask = Task.Run(() => ThinkAboutLife(100000000));
            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(4);
            var toastTask = MakeToastWithButterAsync(3);

            // if you want to wait for all tasks to complete you can use WhenAll.
            // await Task.WhenAll(eggsTask, baconTask, toastTask);

            var breakfastTasks = new List<Task>() { eggsTask, baconTask, toastTask, thinkingTask};
            while(breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if(finishedTask == eggsTask)
                {
                    Console.WriteLine("Eggs are ready!");
                }
                else if(finishedTask == baconTask)
                {
                    Console.WriteLine("Bacon is ready!");
                }
                else if(finishedTask == toastTask)
                {
                    Console.WriteLine("Toast is ready");
                }
                else if(finishedTask == thinkingTask)
                {
                    Console.WriteLine($"while cooking I had {thinkingTask.Result} thoughts!");
                }
                breakfastTasks.Remove(finishedTask);
            }

            stopWatch.Stop();
            Console.WriteLine($"All done after {stopWatch.ElapsedMilliseconds} ms. Breakfast is ready!!!\n");
        }
    }
}
