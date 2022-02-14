using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    class Egg  {}

    class Bacon {}

    class Toast {}

    internal class MakeBreakfastAsync
    {
        static async Task<Toast> MakeToastWithButterAsync(int quantity)
        {
            Console.WriteLine($"Putting {quantity} slices of bread in the toaster.");
            Console.WriteLine($"Toasting bread.");
            await Task.Delay(500);
            for (int slices = 0; slices < quantity; ++slices)
            {
                Console.WriteLine($"Putting toasted slice of bread on plate.");
            }
            return new();
        }

        static async Task<Egg> FryEggsAsync(int quantity)
        {
            Console.WriteLine($"Cracking {quantity} eggs into the pan.");
            Console.WriteLine($"Cooking eggs.");
            await Task.Delay(500);
            for (int eggCount = 0; eggCount < quantity; ++eggCount)
            {
                Console.WriteLine($"Putting egg on plate.");
            }
            return new();
        }

        static async Task<Bacon> FryBaconAsync(int quantity)
        {
            Console.WriteLine($"Putting {quantity} slices of bacon in the pan.");
            Console.WriteLine($"Cooking first side of bacon.");
            await Task.Delay(500);
            for (int slices = 0; slices < quantity; ++slices)
            {
                Console.WriteLine($"Flipping slice of bacon.");
            }
            Console.WriteLine($"Cooking second side of bacon.");
            await Task.Delay(400);
            for (int slices = 0; slices < quantity; ++slices)
            {
                Console.WriteLine($"Putting slice of bacon on plate.");
            }
            return new();
        }

        public static async Task Main()
        {
            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(4);
            var toastTask = MakeToastWithButterAsync(3);
            var breakfastTasks = new List<Task>() { eggsTask, baconTask, toastTask };

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
                breakfastTasks.Remove(finishedTask);
            }

            Console.WriteLine("Breakfast is ready!!!\n");
        }

        private static void applyButter() => Console.WriteLine("Putting butter on toast.");


    }
}
