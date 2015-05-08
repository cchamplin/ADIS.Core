using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IServiceRouter
    {
        IRoute Add(string route, IRequestHandler handler);
        IRoute Add(string route, IRequestHandler handler, RequestMethod method);
    }
}
