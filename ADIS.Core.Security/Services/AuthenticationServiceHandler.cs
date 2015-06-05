using ADIS.Core.ComponentServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class AuthenticationServiceHandler : IRequestHandler
    {
        public object Handle(IRequest request, IRoute route)
        {
            if (request.Method == "POST")
            {
            }
            throw new Exception("Invalid request type");
        }
    }
}
