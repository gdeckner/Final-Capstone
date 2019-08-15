using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.DAL
{
    public class DEMO_SqlDAL : IDEMO_DAL
    {
        //---------------------FOR DEMO ONLY------------------------------

        private readonly string connectionString;

        public DEMO_SqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
            
        }
        public void DemoReset()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"EXEC sys.sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
                                    EXEC sys.sp_msforeachtable 'Delete From ?'
                                    DBCC CHECKIDENT('userLogin',Reseed,0)
                                    DBCC CHECKIDENT('Hours',Reseed,0)
                                    DBCC CHECKIDENT('Locations',Reseed,0)
                                    DBCC CHECKIDENT('Log',Reseed,0)
                                    DBCC CHECKIDENT('Tasks',Reseed,0)
                                    DBCC CHECKIDENT('Jobs',Reseed,0)

                                    EXEC sys.sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL' 
                                  



";
                connection.Open();
                cmd.ExecuteNonQuery();
                string currentDirectory = Directory.GetCurrentDirectory();
                string path = System.IO.Path.Combine(currentDirectory, "DemoScript.sql");
                string script = File.ReadAllText(path);

                cmd.CommandText = script;
                cmd.ExecuteNonQuery();
            }

        }



        //---------------------FOR DEMO ONLY------------------------------
    }
}
