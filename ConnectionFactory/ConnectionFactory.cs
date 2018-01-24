using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Utilities.ConnectionFactory
{
    public class ConnectionFactory
    {
        private IConfiguration Configuration { get; }

        public ConnectionFactory(IConfiguration config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected string ConnectionString => Configuration.GetConnectionString("Default");

        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
