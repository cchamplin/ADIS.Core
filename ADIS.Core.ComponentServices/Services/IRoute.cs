﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IRoute
    {
        string Route { get; }
        Dictionary<string,string> GetComponents(Uri uri);
    }
}
