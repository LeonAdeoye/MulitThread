using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    public class MostRecentlyUsed
    {
        private List<string> _recentlyUsed = new List<string>();
        private int _maxItems;
        private object _lock = new object();

        public MostRecentlyUsed(int maxItems) => _maxItems = maxItems;

        public void UseItem(string item)
        {
            lock (_lock)
            {
                int itemIndex = _recentlyUsed.IndexOf(item);

                if (itemIndex > 0)
                    _recentlyUsed.RemoveAt(itemIndex);

                if (itemIndex != 0)
                {
                    _recentlyUsed.Insert(0, item);

                    if (_recentlyUsed.Count > _maxItems)
                        _recentlyUsed.RemoveAt(_recentlyUsed.Count - 1);
                }

                //foreach(string num in _recentlyUsed) Console.WriteLine(num);
            }
        }

        public IEnumerable<string> GetItems()
        {
            lock(_lock)
            {
                return _recentlyUsed;
            }
        }

        public static void TestMru(MostRecentlyUsed mru)
        {
            Random random = new Random(Thread.CurrentThread.ManagedThreadId);
            string[] items = { "one", "two", "three", "four", "five", "six", "seven", "eight" };
            
            for (int i = 0; i < 10000; ++i)
                mru.UseItem(items[random.Next(items.Length)]);
        }
    }
}
