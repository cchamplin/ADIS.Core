﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public interface ISecurityGroupProvider
    {
        ISecurityGroupBinding Binding { get; }
    }
}
