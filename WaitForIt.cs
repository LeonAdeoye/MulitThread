namespace MulitThread
{
    // As well as protecting access to shared state , Monitors can also provide a way to discover when the shared state has changed.
    // The Monitor class provides a Wait method that operates in conjunction with the Pulse or PulseAll method.
    // A thread that wants to wait for some shared state to change can call Wait,  which block until some other thread calls Pulse/PulseAll.
    // You must already hold the lock on the object you pass to Wait, Pulse, or PulseAll.
    // Calling these methods without possessing the lock will result in a SynchronizationLockExcpetion.

    internal class WaitForIt
    {
        private bool _canGo;
        // _lock is an object whose only job is to be the object for which we acquire a lock in order to protect access to canGo.
        private object _lock = new();

        public void WaitUntilReady()
        {
            // Method must acquire lock before doing anything.
            lock(_lock) // Otherwise SynchronizationLockExcpetion thrown 
            {
                // Sits in a loop until _canGo is true. Each time it goes around the loop, it calls Wait which has three effects:
                //      (1) It relinquishes the lock that it just acquired with the lock statement outside the loop. Otherwise the the thread that calls GoNow will not be able to acquite _lock and never be able to set the _canGo Field to true.
                //      (2) It makes the thread calling WaitUntilReady BLOCK until some other thread calls either Pulse or PulseAll for _lock object.
                //      (3) When Wait returns (after Pulse or PulseAll called by another thread), it reacquires the lock only to release it at the end of the lock block. You cannot release what you don't acquire.
                while(!_canGo)
                {
                    Monitor.Wait(_lock);
                } // After the wait gets to proceed, the loop will check _cango again, and this time it will be true and the loop will finish.
                // The code will then leave the lock block, releasing the lock on _lock, enabling the next waiting thread (if there are any) to do the same thing.
                // So all waiting threads will become unblocked one after another.
            }
        }

        // If multiple threads are waiting, they won't start running at once, becasue Monitor.Wait reacquires the lock before running.
        // It relinquishes the lock only temporarily whileit waits - its insists we hold the lock before calling it, and we wil be holding the lock when it returns.
        // This forces all the waiting multiple threads to take turns.
        public void GoNow()
        {
            // Since the canGo field will be used/shared by multiple threads we need synchronization.
            // Method must acquire lock to make sure it's safe to modify the shared state, the _canGo field.
            lock (_lock) // Otherwise SynchronizationLockExcpetion thrown
            {
                _canGo = true; // Set to true when another thread does whatever we're waiting for. The thread will call GoNow to indicate this.
                Monitor.PulseAll(_lock); // Wake up all threads currently waiting on _lock object as soon as we release the lock.
            } // Releases the lock as the flow of execution leaves the lock block which means that any threads waiting inside WaitUntilReady are no longer blocked waiting for the pulse.
        }

        public static void Main()
        {
            WaitForIt waiter = new();

            // The Thread(ThreadStart) constructor can only be used when the signature of your SomeMethod method matches the ThreadStart delegate.
            // Conversely, Thread(ParameterizedThreadStart) requires SomeMethod to match the ParameterizedThreadStart delegate. The signatures are below:
            // public delegate void ThreadStart()
            // public delegate void ParameterizedThreadStart(Object obj)

            // Concretely, this means that you should use ThreadStart when your method does not take any parameters, and ParameterizedThreadStart
            // when it takes a single Object parameter.Threads created with the former should be started by calling Start(),
            // whilst threads created with the latter should have their argument specified through Start(Object).

            // Finally, you can call the Thread constructors without specifying the ThreadStart or ParameterizedThreadStart delegate.
            // In this case, the compiler will match your method to the constructor overload based on its signature, performing the cast implicitly.

            // var threadA = new Thread(ExecuteA);   // implicit cast to ThreadStart. Assuming ExecuteA is a method with no parameters
            // threadA.Start();

            // var threadB = new Thread(ExecuteB);   // implicit cast to ParameterizedThreadStart. Assuming ExecuteB is a method with a parameter.
            // threadB.Start("abc");

            // The difference between new Thread(SomeMethod) and new Thread(new ThreadStart(SomeMethod)) is purely syntactical:
            // The C# compiler generates the same code for these; the former version is an abbreviation of the latter.

            ThreadStart tWork = delegate
            {
                Console.WriteLine("Thread running...");
                Thread.Sleep(1000);
                Console.WriteLine("Notifying...");
                waiter.GoNow();
                Console.WriteLine("Notified...");
                Thread.Sleep(1000);
                Console.WriteLine("Thread Exiting...");
            };

            Thread t = new(tWork);
            Console.WriteLine("\nStarting new thread.");
            t.Start();
            Console.WriteLine("Waiting for the thread to get going.");
            waiter.WaitUntilReady();
            Console.WriteLine("Wait is over.");
        }
    }
}
