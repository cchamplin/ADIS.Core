using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class JsonWebToken
    {
        protected string type;
        protected string algorithm;

        protected string issuer;
        protected long issuedAtTime;
        protected long expirationTime;
        protected string subject;
        protected string audience;
        protected string notBefore;
        protected string jwtID;

    }
}
