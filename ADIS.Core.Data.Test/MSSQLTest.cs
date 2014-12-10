using System;
using ADIS.Core.Data.Providers;
using ADIS.Core.Data.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADIS.Core.Data.Test
{
    [TestClass]
    public class MSSQLTest
    {
        [TestMethod]
        public void TestFetchObject()
        {
            var sql = new MSSQL();
            var SimpleDBO = sql.FetchObject<SimpleDBO>(new SimpleDBO(), Guid.Empty);
        }

        [TestMethod]
        public void TestConditionals()
        {
            SimpleDBO dbo = new SimpleDBO();
            var context = new SelectFragmentContext();
            var condition = new PropertyCondition();
            condition.SetPropertyLeft("t0", dbo.Properties["PropertyA"] as DataBoundProperty);
            condition.SetRight("merf");
            var sql = condition.ToSQL(context);
            System.Diagnostics.Debug.WriteLine(sql);

            var ors = new Or();
            condition = new PropertyCondition();
            condition.SetPropertyLeft("t0", dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.Empty);
            ors.Condition(condition);
            condition = new PropertyCondition();
            condition.SetPropertyLeft("t0", dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.NewGuid());
            ors.Condition(condition);
            sql = ors.ToSQL(context);
            System.Diagnostics.Debug.WriteLine(sql);

            ors = new Or();
            condition = new PropertyCondition("t0", dbo.Properties["ID"] as DataBoundProperty, Guid.Empty);
            ors.Condition(condition);
            condition = new PropertyCondition("t0", dbo.Properties["ID"] as DataBoundProperty, Guid.NewGuid());
            ors.Condition(condition);
            sql = ors.ToSQL(context);
            System.Diagnostics.Debug.WriteLine(sql);
        }

        [TestMethod]
        public void TestQueryObject()
        {
            SimpleDBO dbo = new SimpleDBO();
            var sql = new MSSQL();
            var query = sql.QueryObject<SimpleDBO>(new SimpleDBO());

            var condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["PropertyA"] as DataBoundProperty);
            condition.SetRight("merf");

            query.Conditions.AddCondition(condition);

            var ors = new Or();
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.Empty);
            ors.Condition(condition);
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.NewGuid());
            ors.Condition(condition);

            query.Conditions.AddCondition(ors);

            query.Execute();
        }

        [TestMethod]
        public void TestQueryObject2()
        {
            SimpleDBO dbo = new SimpleDBO();
            var sql = new MSSQL();
            var query = sql.QueryObject<SimpleDBO>(new SimpleDBO());

            var join = query.InnerJoin("test", "testerson");
            var condition = new PropertyCondition();
            condition.SetLiteralLeft(join.GetTableAlias(),"Something");
            condition.SetPropertyRight(query.GetTableAlias(), dbo.Properties["ID"] as DataBoundProperty);
            join.Conditions.AddCondition(condition);

            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["PropertyA"] as DataBoundProperty);
            condition.SetRight("merf");

            query.Conditions.AddCondition(condition);

            var ors = new Or();
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.Empty);
            ors.AddCondition(condition);
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), dbo.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.NewGuid());
            ors.AddCondition(condition);

            query.Conditions.AddCondition(ors);

            query.Execute();
        }
    }
}
