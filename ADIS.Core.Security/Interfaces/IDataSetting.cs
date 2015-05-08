using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface IDataSetting
    {
        Guid ID { get; }
        string SettingName { get; }
        T GetSettingValue<T>();
        bool Commit(User user, object value);
    }
}
