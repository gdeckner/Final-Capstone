using System;
using System.Collections.Generic;
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



        //---------------------FOR DEMO ONLY------------------------------
    }
}
