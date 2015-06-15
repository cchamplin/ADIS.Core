using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IRawRequestHandler
    {
        void Handle(IRequest request, IRoute route, IResponse response);
    }
}
