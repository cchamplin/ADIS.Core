using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface IAccessAvailability
    {
        Guid ID { get; }
        bool Allow { get; }
        AccessAvailabilityType AvailabilityType { get;  }
        DateTime AvailabilityStart { get;  }
        TimeSpan AvailabilityDuration { get; }
        bool Commit(User u, AccessAvailabilityType type, DateTime start, TimeSpan duration, bool allow = true);
        bool Commit(User u, AccessAvailabilityType type, int day, DateTime start, TimeSpan duration, bool allow = true);
    }
}
