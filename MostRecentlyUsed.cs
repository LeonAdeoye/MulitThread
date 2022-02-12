namespace MulitThread
{
    internal class MostRecentlyUsed
    {
        private List<string> _recentlyUsed = new();
        private int _maxItems;
        private object _lock = new();

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

                foreach(string num in _recentlyUsed) Console.WriteLine(num);
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
            Random random = new(Thread.CurrentThread.ManagedThreadId);
            string[] items = { "one", "two", "three", "four", "five", "six", "seven", "eight" };
            
            for (int i = 0; i < 5; ++i)
                mru.UseItem(items[random.Next(items.Length)]);
        }

        public static void Main()
        {
            MostRecentlyUsed mru = new(4);

            List<Thread> threads = (from i in Enumerable.Range(0, 3) select new Thread(() => MostRecentlyUsed.TestMru(mru))).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Console.WriteLine("\nFinal contents are: ");
            foreach (string item in mru.GetItems())
                Console.WriteLine(item);
        }
    }
}
