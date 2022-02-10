
using MulitThread;

MostRecentlyUsed mru = new MostRecentlyUsed(4);

List<Thread> threads = (from i in Enumerable.Range(0, 3) select new Thread(() => MostRecentlyUsed.TestMru(mru))).ToList();

threads.ForEach(t => t.Start());
threads.ForEach(t => t.Join());

foreach(string item in mru.GetItems())
    Console.WriteLine(item);
