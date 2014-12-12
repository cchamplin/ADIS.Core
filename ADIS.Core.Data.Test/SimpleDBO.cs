using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data;
namespace ADIS.Core.Data.Test
{
    [DataTable("Test","Sample")]
    [ObjectLabel("Sample")]
    public class SimpleDBO : DataBoundObject<SimpleDBO>
    {
        protected Guid id;
        protected Guid complexID;
        protected string propertyA;
        protected int propertyB;
        protected bool propertyC;

        [DataMember("ID", true)]
        [PropertyLabel("SampleID")]
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


        [DataMember("PROPERTY_A")]
        [PropertyLabel("Property A")]
        public String PropertyA
        {
            get
            {
                return propertyA;
            }
            set
            {
                propertyA = value;
            }
        }

        [DataMember("PROPERTY_B")]
        [PropertyLabel("Property B")]
        public int PropertyB
        {
            get
            {
                return propertyB;
            }
            set
            {
                propertyB = value;
            }
        }

        [DataMember("PROPERTY_C")]
        [PropertyLabel("Property C")]
        public bool PropertyC
        {
            get
            {
                return propertyC;
            }
            set
            {
                propertyC = value;
            }
        }

    }
}
