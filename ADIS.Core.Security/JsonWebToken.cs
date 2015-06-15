using FastSerialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class JsonWebToken : ISerializable
    {
        Dictionary<string, string> claims;
        public JsonWebToken(string issuer, string subject, DateTime expires, Dictionary<string,string> claims) : this(issuer,subject,expires)
        {

            this.claims = claims;


        }
        public JsonWebToken(string issuer, string subject, DateTime expires)
        {
            this.type = "JWT";
            this.algorithm = "HS256";
            var expiresStamp = (long)(expires - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            var issuedStamp = (long)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;

            this.issuedAtTime = issuedStamp;
            this.expirationTime = expiresStamp;
            this.issuer = issuer;

        }
        protected string type;
        protected string algorithm;

        protected string issuer;
        protected long issuedAtTime;
        protected long expirationTime;
        protected string subject;
        protected string audience;
        protected string notBefore;
        protected string jwtID;

        private class Header
        {
            public string typ;
            public string alg;

        }
        


        public string GetData()
        {
            var cs = ComponentServices.ComponentServices.Fetch("Text");
           var serializer = cs.Resolve<ISerializer>();
           var headerPart = serializer.Serialize(new Header() { typ = this.type, alg = this.algorithm });
           var encodedHeader = ComponentServices.Text.Base64.encode(headerPart);
           return encodedHeader;
        }
    }
}
