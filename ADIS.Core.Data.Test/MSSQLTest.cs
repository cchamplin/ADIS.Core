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
        public void TestQueryObject()
        {
            var sql = new MSSQL();
            var query = sql.QueryObject<SimpleDBO>(new SimpleDBO());

            var condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(),SimpleDBO.Properties["PropertyA"] as DataBoundProperty);
            condition.SetRight("merf");

            query.Conditions.AddCondition(condition);

            var ors = new ConditionSet();
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), SimpleDBO.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.Empty);
            ors.AddCondition(new OrCondition(condition));
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), SimpleDBO.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.NewGuid());
            ors.AddCondition(new OrCondition(condition));

            query.Conditions.AddCondition(ors);

            query.Execute();
        }

        [TestMethod]
        public void TestQueryObject2()
        {
            var sql = new MSSQL();
            var query = sql.QueryObject<SimpleDBO>(new SimpleDBO());

            var join = query.InnerJoin("test", "testerson");
            var condition = new PropertyCondition();
            condition.SetLiteralLeft(join.GetTableAlias(),"Something");
            condition.SetPropertyRight(query.GetTableAlias(), SimpleDBO.Properties["ID"] as DataBoundProperty);
            join.Conditions.AddCondition(condition);

            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), SimpleDBO.Properties["PropertyA"] as DataBoundProperty);
            condition.SetRight("merf");

            query.Conditions.AddCondition(condition);

            var ors = new ConditionSet();
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), SimpleDBO.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.Empty);
            ors.AddCondition(new OrCondition(condition));
            condition = new PropertyCondition();
            condition.SetPropertyLeft(query.GetTableAlias(), SimpleDBO.Properties["ID"] as DataBoundProperty);
            condition.SetRight(Guid.NewGuid());
            ors.AddCondition(new OrCondition(condition));

            query.Conditions.AddCondition(ors);

            query.Execute();
        }
    }
}
