using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PolitiQualityAlpha.Logic.Database
{
    public class UserRatingConnection : Connection
    {

        public int[] GetArticleResults (string link)
        {

            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[GetArticleVoteResults]";
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;

                
                cmd.Parameters.AddWithValue("@articleLink", link);

                var result = cmd.ExecuteReader();

                var objectValues = new Object[result.FieldCount];

                if (result.Read())
                {
                    result.GetValues(objectValues);

                    conn.Close();

                   

                    return new int [] { (int) objectValues[0], (int)objectValues[1] };
                }
                conn.Close();

                return new int [2] { 0, 0 } ;

            }

            }


        public int GatherUserArticleData(int userId, string articleLink)
        {

            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[RetrieveUserChoices]";
                                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@articleLink", articleLink);

                var result = cmd.ExecuteReader();

                var objectValues = new Object[result.FieldCount];
                if (result.Read())
                {
                    result.GetValues(objectValues);
                }
                else
                {
                    return 0;
                }

                conn.Close();

                return (int)objectValues[3];

            }
        }


        public bool RecordVote(bool userChoice, string authorName, int userId, string userLink)

        {

            using (var conn = new SqlConnection(connectionString))

            {

                conn.Open();
                string spName = @"dbo.[RecordArticleVote]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;

                if (userChoice)
                {

                    cmd.Parameters.AddWithValue("@userChoice", 1);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@userChoice", 2);

                }

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@articleLink", userLink);
                cmd.Parameters.AddWithValue("@journalistName", authorName);

                var result = cmd.ExecuteReader();
                if (result.Read())
                {


                }
                conn.Close();
                return userChoice;





            }
        }


        public bool RecordCommentVote(PolitiQualityAlpha.Logic.Objects.UI.CommentSection comment, bool userChoice)

        {

            using (var conn = new SqlConnection(connectionString))

            {

                conn.Open();
                string spName = @"dbo.[RecordCommentVote]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;

                if (userChoice)
                {

                    cmd.Parameters.AddWithValue("@userChoice", 1);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@userChoice", 2);

                }

                cmd.Parameters.AddWithValue("@loginId", comment.UserId );
                cmd.Parameters.AddWithValue("@commentId", comment.CommentId);
              

                var result = cmd.ExecuteReader();
                if (result.Read())
                {


                }
                conn.Close();
                return userChoice;





            }
        }

    }
}
