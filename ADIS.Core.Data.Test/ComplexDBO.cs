using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Test
{
    [DataTable("Test", "ComplexDBO")]
    [ObjectLabel("Complex")]
    public class ComplexDBO : DataBoundObject<ComplexDBO>
    {

        [DataMember("ID",true)]
        [PropertyLabel("ComplexID")]
        public Guid ID { get; set; }


        [DataMember("FIRST_NAME")]
        [PropertyLabel("FirstName")]
        public string Name { get; set; }

        [DataMember("INT_ID")]
        [PropertyLabel("IntID")]
        public int IntID { get; set; }

        [DataMember("LAST_NAME")]
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
