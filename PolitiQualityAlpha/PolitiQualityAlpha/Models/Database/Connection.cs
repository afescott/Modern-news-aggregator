using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PolitiQualityAlpha.Logic.Database
{


   public class Connection
    {
              
        protected const string connectionString =
     @"Data Source=politiquality.database.windows.net;Initial Catalog=PolitiQuality;User ID=s4908683;Password=#afafaf";

        protected SqlCommand Con = new SqlCommand();



        protected  void Connect()
        {
            

     

        }

    }
}
