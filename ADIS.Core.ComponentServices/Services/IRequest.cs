using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IRequest
    {
        string Method { get; }
        string Authorization { get; }
        string RawUrl { get; }
        Uri Url { get; }
        bool IsSecure { get; }
        string RemoteIP { get; }
        Dictionary<string, System.Net.Cookie> Cookies { get; }
        Dictionary<string, IPostedFile> Files { get; }
        Dictionary<string, string> QueryParameters { get; }
        Dictionary<string, string> Form { get; }
        string Data { get; }
        //Dictionary<string, object> Items { get; }
    }
}
