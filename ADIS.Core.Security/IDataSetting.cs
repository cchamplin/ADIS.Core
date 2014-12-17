using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface IDataSetting
    {
        Guid ID { get; protected set; }
        string SettingName { get; protected set; }
        T GetSettingValue<T>();
        bool Commit(User user, object value);
    }
}
