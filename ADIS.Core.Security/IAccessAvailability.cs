using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Security
{
    public interface IAccessAvailability
    {
        Guid ID { get; protected set; }
        bool Allow { get; protected set; }
        AccessAvailabilityType AvailabilityType { get; protected set; }
        DateTime AvailabilityStart { get; protected set; }
        TimeSpan AvailabilityDuration { get; protected set; }
        bool Commit(User u, AccessAvailabilityType type, DateTime start, TimeSpan duration, bool allow = true);
        bool Commit(User u, AccessAvailabilityType type, int day, DateTime start, TimeSpan duration, bool allow = true);
    }
}
