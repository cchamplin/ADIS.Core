using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization
{
    public class OrganizationalUnit
    {
        protected string longName;
        protected string shortName;
        protected string machineName;
        protected Uri url;
        protected IUnitType unitType;
        protected OrganizationalUnit parent;
        protected List<OrganizationalUnit> children;

        protected DateTime changeDate;
        protected DateTime addedDate;
        protected Guid changeID;
        protected Guid addID;
    }
}
