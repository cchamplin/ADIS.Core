using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data;
namespace ADIS.Core.Data.Test
{
    [DataTable("Test","Sample","ID")]
    [ObjectLabel("Sample")]
    public class SimpleDBO : DataBoundObject<SimpleDBO>
    {
        protected Guid id;
        protected Guid complexID;
        protected string propertyA;
        protected int propertyB;

        [PropertyColumn("ID")]
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


        [PropertyColumn("PROPERTY_A")]
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

        [PropertyColumn("PROPERTY_B")]
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

    }
}
