using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    internal class SqlQueryProvider<T> : QueryProvider<T>
    {



        public SqlQueryProvider(IExpressionProcessor expressionProcessor, INameResolver nameresolver, Type sourceType)
            : base( expressionProcessor, nameresolver, sourceType)
        {
          
        }

        protected override IEnumerable<T> GetResults(QueryBuilder builder)
        {
            System.Diagnostics.Debug.WriteLine(builder.GetFullQuery());
            return default(IEnumerable<T>);
            /*var tModel = Models[typeof(T)];
            var fullUri = builder.GetFullUri();
            tModel.Filter = fullUri;
            _client.UserName = _connectionContext.Username;
            _client.Password = _connectionContext.Password;
            _client.Headers.Remove("YearContext");
            if (_connectionContext.YearContext != null)
            {
                _client.Headers.Add("YearContext", _connectionContext.YearContext.ToString());
            }
            _client.SetCredentials(_client.UserName, _client.Password);
            // Eleminate an extra request
            _client.AlwaysSendBasicAuthHeader = true;
            try
            {
                var response = _client.Get<List<T>>((IReturn<List<T>>)tModel);
                return response;
            }
            catch (WebServiceException ex)
            {
                throw ex;
            }
            //TODO RETURN SERVICE STACK REQUEST

            // return response;

            //var response = Client.Get(fullUri);
            // var serializer = SerializerFactory.Create<T>();
            //var resultSet = serializer.DeserializeList(response);

            //Contract.Assume(resultSet != null);

            // return resultSet;*/
        }

        protected override IEnumerable GetIntermediateResults(Type type, QueryBuilder builder)
        {

            System.Diagnostics.Debug.WriteLine(builder.GetFullQuery());
            return null;
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
