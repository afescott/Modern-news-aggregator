using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PolitiQualityAlpha.Logic.Objects.UI;

namespace PolitiQualityAlpha.Logic.Database
{
    public class UserConnection : Connection
    {
      

        public List<bool> GetPublisherChoices (int loginId)
        {
            var userPreferences = new List<bool>();
            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[GetUserPublisherPreferences]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@loginId", loginId);

                var result = cmd.ExecuteReader();
                var objectValues = new Object[result.FieldCount];

                if (result.Read())
                {

                    result.GetValues(objectValues);

                    userPreferences.AddRange(objectValues.Cast<bool>().ToList());
                    
                } else
                {

                    userPreferences.AddRange(new List<bool> { true, true, true });


                }


                conn.Close();


                return userPreferences;





            }

        }

    

        public void UpdateUserPreferences(int loginId, UserFilterChoices choices )
        {
            

            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[UpdateUserPreference]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@loginId", loginId);

                cmd.Parameters.AddWithValue("@visibleHiddenCnn", choices.CnnPreference);
                cmd.Parameters.AddWithValue("@visibleHiddenBbc", choices.BbcPreference);
                cmd.Parameters.AddWithValue("@visibleHiddenGuardian", choices.GuardianPreference);

                var result = cmd.ExecuteReader();


                if (result.Read())
                {

                }

                    conn.Close();


            }
        }



                public int Authenticate(string username, string password)
        {

            using (var conn = new SqlConnection(connectionString))

            {
                var userId = 0;
                conn.Open();
                string spName = @"dbo.[Authenticate]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                

                var result = cmd.ExecuteReader();
               

                if (result.Read())
                {

                    var objectValues = new Object[result.FieldCount];


                    result.GetValues(objectValues);

                    userId = (int) objectValues[0];
                }


                conn.Close();


                return (userId);





            }

           
        }


        public bool CreateUser(string username, string password)
        {

            using (var conn = new SqlConnection(connectionString))

            {

                string spName = @"dbo.[CreateUser]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                conn.Open();

                //var asfsa = cmd.ExecuteNonQuery();


                var dr = cmd.ExecuteReader();

                if (dr.RecordsAffected < 1)
                {
                    conn.Close();
                    return false;

                }

                conn.Close();

            }
            
            return true;
        }

   
    }
}
