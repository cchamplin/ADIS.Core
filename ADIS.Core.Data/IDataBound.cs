using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public interface IDataBound
    {
        string TableName { get; }
        string Schema { get; }
        DataBoundProperty PrimaryKey { get; }
        Dictionary<string, DataProperty> Properties { get; }
        List<DataProperty> PropertyList { get; }
    }
}
