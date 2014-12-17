﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SecurityGroup
    {
        protected Guid ID;
        protected string name;
        protected ISecurityGroupBinding binding;
        protected SecurityGroup inherits;
        protected List<ISecurable> secured;
    }
}