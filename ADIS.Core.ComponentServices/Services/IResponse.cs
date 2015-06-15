using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IResponse
    {
        void AddHeader(string name, string value);
        Stream OutputStream { get; }
        void AddCookie(System.Net.Cookie cookie);
    }
}
