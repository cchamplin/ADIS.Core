using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Services
{
    public interface IPostedFile
    {
        string ContentType { get; }
        int ContentLength { get; }
        string FileName { get; }
        Stream InputStream { get; }
        void SaveAs(string filename);
    }
}
