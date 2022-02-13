using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitThread
{
    internal class WorkbenchServiceVerifierImpl : IServiceVerifier
    {
        readonly private String _name;
        readonly private CountdownEvent _latch;
        public WorkbenchServiceVerifierImpl(String name, CountdownEvent latch)
        {
            _name = name;
            _latch = latch;
        }

        public void Verify()
        {
            try
            {
                Console.WriteLine($"Verifying service: {_name}");
                Thread.Sleep(200);
                _latch.Signal();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }

        string IServiceVerifier.GetName()
        {
            return _name;
        }
    }
}
