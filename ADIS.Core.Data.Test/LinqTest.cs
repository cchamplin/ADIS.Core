using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADIS.Core.Data.LINQ;
namespace ADIS.Core.Data.Test
{
    [TestClass]
    public class LinqTest
    {

        [TestMethod]
        public void TestLinqActual()
        {
            var items = Data.GenerateComplexDBOs();

            var subset = (from dbo in items
                          where dbo.Last.StartsWith("Complex")
                          && dbo.Related.RelatedProperty.StartsWith("Related")
                          select dbo).ToList();

            System.Diagnostics.Debug.WriteLine(subset.Count);


            subset = (from dbo in items
                      where dbo.Last.StartsWith("Complex")
                      && dbo.Children.Any(x => x.PropertyC == true)
                      select dbo).ToList();
            System.Diagnostics.Debug.WriteLine(subset.Count);
        }

        [TestMethod]
        public void TestBasicSelect()
        {
            IQueryable<SimpleDBO> dbos = new SqlQueryable<SimpleDBO>();

            List<SimpleDBO> data = (from dbo in dbos
                                    where dbo.ID == Guid.Empty
                                    select dbo).ToList();


        }

        [TestMethod]
        public void TestSelectExpand()
        {
            IQueryable<ComplexDBO> dbos = new SqlQueryable<ComplexDBO>();

            List<ComplexDBO> data = (from dbo in dbos
                                    where dbo.ID == Guid.Empty
                                    select dbo).Expand("Children,Related/Simple").ToList();


        }

        [TestMethod]
        public void TestSelectExpandWhere()
        {
            IQueryable<ComplexDBO> dbos = new SqlQueryable<ComplexDBO>();

            List<ComplexDBO> data = (from dbo in dbos
                                     where dbo.ID == Guid.Empty && dbo.Related.Simple.PropertyA == "Test"
                                     select dbo).Expand("Children,Related/Simple").ToList();


        }

        [TestMethod]
        public void TestBasicWhere()
        {
            IQueryable<ComplexDBO> dbos = new SqlQueryable<ComplexDBO>();

            List<ComplexDBO> data = (from dbo in dbos
                                    where dbo.ID == Guid.Empty && dbo.Related.RelatedProperty == "steve"
                                    select dbo).ToList();


        }

        [TestMethod]
        public void TestBasicSelectProjection()
        {
            IQueryable<ComplexDBO> dbos = new SqlQueryable<ComplexDBO>();

            List<RelatedDBO> data = (from dbo in dbos
                                    where dbo.ID == Guid.Empty
                                    select dbo.Related).ToList();


        }

        [TestMethod]
        public void TestBasicSelectMany()
        {
            IQueryable<ComplexDBO> dbos = new SqlQueryable<ComplexDBO>();

            List<SimpleDBO> data = (from dbo in dbos
                                    from child in dbo.Children
                                    where dbo.ID == Guid.Empty
                                    select child).ToList();


        }
    }
}
