using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface IAuthenticationBinding
    {
        string MachineName { get; }
        string Name { get; }
        string EndPoint { get; }

    }
}
