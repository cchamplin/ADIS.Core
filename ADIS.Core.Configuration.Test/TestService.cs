using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration.Test
{
    public class TestService : ITestService
    {
        public void DoTest(string value)
        {
            System.Diagnostics.Debug.WriteLine("Doing Test: " + value);
        }
    }
}
