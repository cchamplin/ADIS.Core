﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public interface IAuthenticationProvider
    {
        IAuthenticationBinding Binding { get; }
        object AuthenticateUser(User user, string password);
        object AuthenticateUser(User user, object[] requestFields);
        List<AuthenticationRequestField> ValidRequestFields();
    }
}