using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organization
{
    public interface IUnitType
    {
        string Name { get; protected set; }
        string MachineName { get; protected set; }
    }
}
