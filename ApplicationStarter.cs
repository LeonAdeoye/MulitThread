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

            Console.WriteLine("Waiting for verification...");
            foreach (IServiceVerifier serviceVerifier in _serviceVerifiers)
            {
                new Thread(new ThreadStart(serviceVerifier.Verify)).Start();
            }

            _latch.Wait();
            Console.WriteLine("Completed verification.");
        }
    }
}
