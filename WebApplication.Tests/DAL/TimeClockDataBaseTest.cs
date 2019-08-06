using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace WebApplication.Tests.DAL
{

    [TestClass]
    public abstract class TimeClockDataBaseTest
    {
        private IConfigurationRoot config;
        private TransactionScope transaction;
        protected IConfigurationRoot Config
        {
            get
            {
                if (config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    config = builder.Build();
                }
                return config;
            }
        }
        protected string ConnectionString
        {
            get
            {
                return Config.GetConnectionString("Database");
            }
        }
        [TestInitialize]
        public virtual void Setup()
        {

            transaction = new TransactionScope();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"delete from userJob
                                    delete from Tasks
                                    delete from Locations
                                    delete from Jobs
                                    delete from UserLogin
                                    delete from Roles";

                conn.Open();
                cmd.ExecuteNonQuery();

            }


        }
        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }
    }

}
