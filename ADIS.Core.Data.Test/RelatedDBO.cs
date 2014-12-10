using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Test
{
    [DataTable("Test", "Related", "ID")]
    [ObjectLabel("Related")]
    public class RelatedDBO : DataBoundObject<RelatedDBO>
    {
        protected Guid id;
        protected Guid complexID;
        protected string relatedProp;

        [PropertyColumn("ID")]
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

        [PropertyColumn("COMPLEX_ID")]
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


        [PropertyColumn("RELATED_PROP")]
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
