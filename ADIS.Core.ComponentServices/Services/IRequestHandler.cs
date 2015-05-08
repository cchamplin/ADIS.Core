using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IRequestHandler
    {
        object Handle(IRequest request, IRoute route);
    }
}
