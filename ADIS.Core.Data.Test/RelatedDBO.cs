using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Test
{
    [DataTable("Test", "Related")]
    [ObjectLabel("Related")]
    public class RelatedDBO : DataBoundObject<RelatedDBO>
    {
        protected Guid id;
        protected Guid complexID;
        protected string relatedProp;

        [DataMember("ID",true)]
        [PropertyLabel("RelatedID")]
        public Guid ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        [DataMember("COMPLEX_ID")]
        [PropertyLabel("ComplexID")]
        public Guid ComplexID
        {
            get
            {
                return complexID;
            }
            set
            {
                complexID = value;
            }
        }


        [DataMember("RELATED_PROP")]
        [PropertyLabel("Related Property")]
        public String RelatedProperty
        {
            get
            {
                return relatedProp;
            }
            set
            {
                relatedProp = value;
            }
        }

        [PropertyLabel("Simple")]
        [ManyToOne("RELATED_ID")]
        public SimpleDBO Simple { get; set; }

    }
}
