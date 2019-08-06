using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace WebApplication.Tests.DAL
{
    public class TimeClockDataBaseTest
    {
        [TestClass]
        public abstract class DatabaseTest
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
                    return Config.GetConnectionString("Test");
                }
            }
            [TestInitialize]
            public virtual void Setup()
            {

                transaction = new TransactionScope();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"delete from Roles
                                    delete from UserLogin
                                    delete from Jobs
                                    delete from Locations
                                    delete from Tasks
                                    delete from userJob";

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
}
