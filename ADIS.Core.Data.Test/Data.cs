using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Test
{
    public static class Data
    {
        public static List<ComplexDBO> GenerateComplexDBOs()
        {
            List<ComplexDBO> output = new List<ComplexDBO>();
            for (int x = 0; x < 20; x++)
            {
                var item = new ComplexDBO();
                item.ID = Guid.NewGuid();
                item.Name = "ComplexName" + x;
                item.Last = "ComplexLast" + x;
                item.IntID = x;
                item.Children = GenerateSimpleDBOs(item.ID, true,(x % 2 == 0) ? true : false);
                var related = new RelatedDBO();
                related.ID = Guid.NewGuid();
                related.ComplexID = item.ID;
                related.RelatedProperty = (x % 2 == 0) ? "Related" + x : "NotRelated" + x;

                var simple = new SimpleDBO();
                simple.ID = Guid.NewGuid();
                simple.PropertyA = "RelatedSimplePropA" + x;
                simple.PropertyB = x;
                simple.ComplexID = item.ID;
                simple.PropertyC = false;
                related.Simple = simple;


                item.Related = related;
                output.Add(item);
            }
            return output;
        }
        public static List<SimpleDBO> GenerateSimpleDBOs(Guid complexID, bool setPropC = false, bool propC = true)
        {
            List<SimpleDBO> output = new List<SimpleDBO>();
            for (int x = 0; x < 20; x++)
            {
                var item = new SimpleDBO();
                item.ID = Guid.NewGuid();
                item.PropertyA = "SimplePropA" + x;
                item.PropertyB = x;
                item.ComplexID = complexID;
                if (setPropC)
                {
                    item.PropertyC = propC;
                }
                else
                {
                    item.PropertyC = (x % 2 == 0) ? true : false;
                }
                output.Add(item);
            }
            return output;
        }
    }
}
