using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PolitiQualityAlpha.Logic.Database
{
    public class CommentConnection : Connection
    {
        public void SendComment(int userId, string comment, int articlesetId)
        {


            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[InsertComment]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;


                cmd.Parameters.AddWithValue("@articleSetId", articlesetId);
                cmd.Parameters.AddWithValue("@comment", comment);
                cmd.Parameters.AddWithValue("@LoginId", userId);

                var result = cmd.ExecuteReader();


                if (result.Read())
                {

                }

                conn.Close();


            }


        }

        public List<PolitiQualityAlpha.Logic.Objects.UI.CommentSection> GetComments(int articlesetId)
        {
            var commentList = new List<PolitiQualityAlpha.Logic.Objects.UI.CommentSection>();

            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[GetComments]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;


                cmd.Parameters.AddWithValue("@articleSetId", articlesetId);


                var result = cmd.ExecuteReader();



                while (result.Read())
                {
                    var objectValues = new Object[result.FieldCount];


                    result.GetValues(objectValues);

                    var comment = new PolitiQualityAlpha.Logic.Objects.UI.CommentSection { CommentId = (int)objectValues[0], CommentDate = (DateTime)objectValues[1], CommentText = (string)objectValues[2], UserId = (int)objectValues[3], ArticleSetId = (int)objectValues[4] };


                    commentList.Add(GetCommentUserVotes(comment));
                  
                }

                conn.Close();


            }

            return commentList;

        }

        private PolitiQualityAlpha.Logic.Objects.UI.CommentSection GetCommentUserVotes(PolitiQualityAlpha.Logic.Objects.UI.CommentSection comment)
        {
            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[GetCommentUserVotes]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;


                cmd.Parameters.AddWithValue("@commentId", comment.CommentId);
                cmd.Parameters.AddWithValue("@loginId", comment.UserId);



                var result = cmd.ExecuteReader();



                if (result.Read())
                {

                    var objectValues = new Object[result.FieldCount];


                    result.GetValues(objectValues);

                    comment.CommentUpVotes = (int)objectValues[0];
                    comment.CommentDownVotes = (int)objectValues[1];
                    comment.Username = (string)objectValues[2];

                  var userVote =  GetDets(comment.UserId);

                    if (userVote == 1)
                    {
                        comment.UpVoteCommentBackgroundColourToggle = true;
                        
                    }
                    if (userVote == 2)
                    {

                        comment.DownVoteCommentBackgroundColourToggle = true;

                    }


                }

                conn.Close();

                return comment;
            }

        }


        private int GetDets(int loginId)
        {
            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                string spName = @"dbo.[GetLoginUserCommentChoice]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;



                cmd.Parameters.AddWithValue("@loginId", loginId);



                var result = cmd.ExecuteReader();



                if (result.Read())
                {

                    var objectValues = new Object[result.FieldCount];


                    result.GetValues(objectValues);
                    var userChoice = (int)objectValues[0];

                    if (userChoice == 1)
                    {
                        conn.Close();
                        return 1;
                    }
                    if (userChoice == 2)
                    {
                        conn.Close();
                        return 2;

                    }

                }

                conn.Close();
                return 0;
            }



        }
        }
}
