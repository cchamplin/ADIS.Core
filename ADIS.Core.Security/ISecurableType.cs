﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface ISecurableType
    {
        string MachineName { get; protected set; }
        string Name { get; protected set; }
    }
}
