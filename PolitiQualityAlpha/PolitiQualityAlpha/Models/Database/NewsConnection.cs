using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects;

namespace PolitiQualityAlpha.Logic.Database
{
    public class NewsConnection : Connection
    {
        public void InsertArticleSet(List<CosineArticleSet> cosineArticles)
        {


            foreach (var element in cosineArticles)
            {
                InsertJournalistIntoDatabase(element.ArticleX.Record);
                InsertJournalistIntoDatabase(element.ArticleY.RecordComparedTo);
                InsertJournalistIntoDatabase(element.ArticleX.RecordComparedTo);

                InsertArticleIntoDatabase(element.ArticleX.Record);
                InsertArticleIntoDatabase(element.ArticleY.RecordComparedTo);

                InsertArticleIntoDatabase(element.ArticleX.RecordComparedTo);


                using (var conn = new SqlConnection(connectionString))

                {

                    string spName = @"dbo.[InsertArticleSet]";

                    SqlCommand cmd = new SqlCommand(spName, conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@articleLinkOne", element.ArticleX.Record.Link);
                    cmd.Parameters.AddWithValue("@articleLinkTwo", element.ArticleY.RecordComparedTo.Link);
                    cmd.Parameters.AddWithValue("@articleLinkThree", element.ArticleX.RecordComparedTo.Link);
                    cmd.Parameters.AddWithValue("@articleLinkMostCommonHeader", element.HeaderMostSimilarToArticles.Link);

                    conn.Open();

                    cmd.ExecuteNonQuery();

                }


            }



        }


        private List<CosineArticleSet> JournalistsAndArticles = new List<CosineArticleSet>();

        private void InsertArticleIntoDatabase(HtmlRecord record)
        {

            using (var conn = new SqlConnection(connectionString))

            {
                var date = (DateTime)Convert.ToDateTime(record.Date);

                string spName = @"dbo.[InsertArticle]";

                SqlCommand cmd = new SqlCommand(spName, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@newsPublisher", record.NewsPublisher);
                cmd.Parameters.AddWithValue("@link", record.Link);
                cmd.Parameters.AddWithValue("@journalist", record.Author);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@description", record.Description);
                cmd.Parameters.AddWithValue("@header", record.Header);

                conn.Open();

                cmd.ExecuteNonQuery();


                conn.Close();


            }
        }


        public void InsertJournalistIntoDatabase(HtmlRecord record)

        {
            using (var conn = new SqlConnection(connectionString))

            {

                string spName = @"dbo.[InsertJournalist]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;


                cmd.Parameters.AddWithValue("@newsPublisher", record.NewsPublisher);
                cmd.Parameters.AddWithValue("@journalistName", record.Author);

                conn.Open();

                cmd.ExecuteNonQuery();


                conn.Close();
            }



        }

        public List<CosineRecordSet> GatherArticles()
        {
            var articleSetIds = new List<int[]>();

            using (var conn = new SqlConnection(connectionString))

            {


                string spName = @"dbo.[GetArticleSets]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);

                SqlDataReader myReader;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                conn.Open();

                myReader = cmd.ExecuteReader();


                while (myReader.Read())
                {



                    var objectValues = new Object[myReader.FieldCount];


                    myReader.GetValues(objectValues);

                    articleSetIds.Add(objectValues.Cast<int>().ToArray());

                }


                conn.Close();
            }

            return GatherNewsArticles(articleSetIds);

        }


        private List<CosineRecordSet> GatherNewsArticles(List<int[]> articleIdSets)
        {
            var cosineSets = new List<CosineRecordSet>();
            foreach (var element in articleIdSets)
            {


                var htmlRecordSet = new CosineRecordSet();
                htmlRecordSet.ArticleSetId = element[0];
                
                htmlRecordSet.ArticleSet = new List<HtmlRecord>();
                htmlRecordSet.HeaderMostSimilarToArticles = "";

                for (int i = 1; i < element.Count(); i++)
                {

                    using (var conn = new SqlConnection(connectionString))

                    {

                        if (i != 4)
                        {

                            string spName = @"dbo.[GatherNewsArticles]";


                            SqlCommand cmd = new SqlCommand(spName, conn);

                            SqlDataReader myReader;

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;

                            cmd.Parameters.AddWithValue("@articleID", element[i]);

                            conn.Open();

                            myReader = cmd.ExecuteReader();

                            var objectValues = new Object[myReader.FieldCount];

                            if (myReader.Read())
                            {
                                myReader.GetValues(objectValues);
                            }

                            var returnSet = GetPublisherAndJournalistName((int)objectValues[1], (int)objectValues[2]);

                            var htmlRecord = new HtmlRecord();

                            if ((int)objectValues[0] == 0)
                                htmlRecord.NewsPublisher = PublisherEnum.Unspecified;
                            else
                            {
                                if (returnSet[1].ToLower().Contains("cnn"))
                                    htmlRecord.NewsPublisher = PublisherEnum.CNN;
                                else if (returnSet[1].ToLower().Contains("guardian"))
                                    htmlRecord.NewsPublisher = PublisherEnum.Guardian;
                                else
                                    htmlRecord.NewsPublisher = PublisherEnum.BBC;
                            }

                            htmlRecord.ArticleId = element[i];

                            htmlRecord.Link = (string)objectValues[3];
                            htmlRecord.Description = (string)objectValues[5];
                            htmlRecord.Header = (string)objectValues[6];

                            htmlRecord.DateRecord = (DateTime)objectValues[4];



                            htmlRecord.Author = returnSet[0];


                            htmlRecordSet.ArticleSet.Add(htmlRecord);

                        }
                        else
                        {
                            string spName = @"dbo.[RetrieveTitle]";

                            //define the SqlCommand object
                            SqlCommand cmd = new SqlCommand(spName, conn);

                            SqlDataReader myReader;




                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;

                            cmd.Parameters.AddWithValue("@articleID", element[i]);

                            conn.Open();

                            myReader = cmd.ExecuteReader();

                            var objectValues = new Object[myReader.FieldCount];

                            if (myReader.Read())
                            {
                                myReader.GetValues(objectValues);
                            }




                            htmlRecordSet.HeaderMostSimilarToArticles = (string)objectValues[0];
                        }


                        conn.Close();
                    }





                }



                cosineSets.Add(htmlRecordSet);



            }

            return cosineSets;
        }



        private string[] GetPublisherAndJournalistName(int publisherId, int journalistId)
        {

            using (var conn = new SqlConnection(connectionString))

            {
              string spName = @"dbo.[GetJournalistAndPublisher]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);

                SqlDataReader myReader;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@publisherId", publisherId);
                cmd.Parameters.AddWithValue("@journalistId", journalistId);

                conn.Open();

                var reader = cmd.ExecuteReader();
               
                var objectValues = new Object[reader.FieldCount];

                while (reader.Read())
                {


                    reader.GetValues(objectValues);

                }

                conn.Close();

                return new string[] { (string)objectValues[0], (string)objectValues[1] };

             }

        }




    }
}