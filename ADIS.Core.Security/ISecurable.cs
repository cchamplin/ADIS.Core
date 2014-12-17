using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface ISecurable
    {
        Guid ID;
        object Securable { get; set; }
        Guid SecurableID { get; set; }
        AccessType AccessType { get; set; }
        ISecurableType Type { get; set; }
        List<ISecurable> SubSecurables { get; }
        ISecurable parent { get; }
    }
}
