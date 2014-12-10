using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Test
{
    [DataTable("Test", "ComplexDBO", "ID")]
    [ObjectLabel("Complex")]
    public class ComplexDBO : DataBoundObject<ComplexDBO>
    {

        [PropertyColumn("ID")]
        [PropertyLabel("ComplexID")]
        public Guid ID { get; set; }


        [PropertyColumn("FIRST_NAME")]
        [PropertyLabel("FirstName")]
        public string Name { get; set; }


        [PropertyColumn("LAST_NAME")]
        [PropertyLabel("LastName")]
        public string Last { get; set; }

        [PropertyLabel("Children")]
        [OneToMany("COMPLEX_ID")]
        public List<SimpleDBO> Children { get; set; }

        [PropertyLabel("Related")]
        [ManyToOne("COMPLEX_ID")]
        public RelatedDBO Related { get; set; }
    }
}
