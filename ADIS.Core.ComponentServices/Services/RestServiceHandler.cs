using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public class RestServiceHandler : IRequestHandler
    {
        public object Handle(IRequest request, IRoute route)
        {
            switch (request.Method.ToUpper())
            {
                case "GET":
                    return Get();
                case "DELETE":
                    return Delete(Guid.Empty);
                case "POST":
                    return Create(null);
                case "PUT":
                    return Update(null);
            }
            throw new NotImplementedException();
        }

        public virtual object Get()
        {
            throw new NotImplementedException();
        }
        public virtual object Get(Guid guid)
        {
            throw new NotImplementedException();
        }
        public virtual object Delete(Guid guid)
        {
            throw new NotImplementedException();
        }
        public virtual object Update(object entity)
        {
            throw new NotImplementedException();
        }
        public virtual object Create(object entity)
        {
            throw new NotImplementedException();
        }

    }
}
