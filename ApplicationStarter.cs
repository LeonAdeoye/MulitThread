using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    internal class ApplicationStarter
    {
        public static void VerifyServices()
        {
            CountdownEvent _latch = new(3);
            List<IServiceVerifier> _serviceVerifiers = new();
            _serviceVerifiers.Add(new WorkbenchServiceVerifierImpl("logging-servivce", _latch));
            _serviceVerifiers.Add(new WorkbenchServiceVerifierImpl("configuration-servivce", _latch));
            _serviceVerifiers.Add(new WorkbenchServiceVerifierImpl("security-servivce", _latch));

            Console.WriteLine("\nWaiting for verification...");

            // Creating threads using ThreadStart.
            foreach (IServiceVerifier serviceVerifier in _serviceVerifiers)
            {
                if(serviceVerifier.GetName().Equals("logging-service"))
                    new Thread(new ThreadStart(serviceVerifier.Verify)).Start();
            }

            // Creating threads using Task Parallel Library (TPL).
            foreach (IServiceVerifier serviceVerifier in _serviceVerifiers)
            {
                if (!serviceVerifier.GetName().Equals("logging-service"))
                    Task.Factory.StartNew(() => serviceVerifier.Verify()); // Use lambda-based argument to pass any number of parameters.
                    //Task.Factory.StartNew(serviceVerifier.Verify); This works as well.
            }


            _latch.Wait();
            Console.WriteLine("Completed verification.");
        }
    }
}
