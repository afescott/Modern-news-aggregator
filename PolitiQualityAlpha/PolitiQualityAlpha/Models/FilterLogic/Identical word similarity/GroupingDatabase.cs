using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolitiQualityAlpha.Logic.Objects;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Enums;
using PolitiQuality.Logic.FilterLogic;
using System.Security.Cryptography;

namespace PolitiQualityAlpha.Logic.FilterLogic.Identical_word_similarity
{
   public class DatabaseArticleGroup
    {

        /* Simple method to combine the final clusters based on publisher, i.e. (BBC news + Guardian) + (bbc news + CNN)  */
        public List<FinalCluster[]> CombineClusters (List<FinalCluster> finalClusterX, List<FinalCluster> finalClusterY)
        {
            var combinationOfClusters =  new List<FinalCluster[]>();

         

            foreach (var elementX in finalClusterX)
            {
                foreach (var elementY in finalClusterY)
                {
                    if (elementX.Article == elementY.Article)
                    {
                        var combination = new FinalCluster[2];
                        combination[0] = elementX;
                        combination[1] = elementY;
                        combinationOfClusters.Add(combination);

                    


                    }

                }


                

            }

            return combinationOfClusters;

        }


        /*  Identifies information relevant for database process such as most similar header, and the description, would could be useful in the future*/
        public List<List<CosineArticleSet>> PrepareForDatabase ( List<FinalCluster[]> clusters)
        {
           
            var cosineArraySets = new List<List<CosineArticleSet>>();


         


            foreach (var cluster in clusters)
            {

                
                var recordsComparedToX = new List<HtmlRecord>();
                var recordsComparedToY = new List<HtmlRecord>();


                if (cluster[0].VectorEntries.Any() && cluster[1].VectorEntries.Any())
                 {
                    var cosineSets = new List<CosineArticleSet>();
                   
                    foreach (var elementX in cluster[0].VectorEntries)

                    {
                        
                        var j = 0;
                        foreach (var elementY in cluster[1].VectorEntries)
                        {
                            var cosineSet = new CosineArticleSet();
                            
                            cosineSet.ArticleX = elementX;

                            cosineSet.ArticleY = elementY;
                                                       

                            var identicalObjectArticle = new IdenticalWordSimilarity(elementX.RecordComparedTo, (new List<HtmlRecord>() { elementY.RecordComparedTo })); //as resultsHeader is the same

                            var resultArticle = identicalObjectArticle.CalculateCosine().First();

                            cosineSet.ArticleZ = resultArticle;

                            cosineSet.CosineArticleScoreOtherArticles = resultArticle.CosineScore;

                            

                            var descriptionCosineX = elementX.RecordComparedTo;

                            descriptionCosineX.CosineType = TypeOfCosine.Description;

                            var descriptionCosineY = elementX.RecordComparedTo;

                            descriptionCosineY.CosineType = TypeOfCosine.Description;

                            var descriptionCosineZ = elementX.Record;

                            descriptionCosineZ.CosineType = TypeOfCosine.Description;




                            var headerCosineX = elementX.RecordComparedTo;

                            headerCosineX.CosineType = TypeOfCosine.Title;

                            var headerCosineY = elementX.RecordComparedTo;

                            headerCosineY.CosineType = TypeOfCosine.Title;

                            var headerCosineZ = elementY.Record;

                            headerCosineZ.CosineType = TypeOfCosine.Title;

                            var textObject = new ArticleText();

                            var resultDescriptionX = textObject.PerformAnalyiticsHeaderTitle(headerCosineX, TypeOfCosine.Title);
                            var resultDescriptionY = textObject.PerformAnalyiticsHeaderTitle(headerCosineY, TypeOfCosine.Title);
                            var resultDescriptionZ = textObject.PerformAnalyiticsHeaderTitle(headerCosineZ, TypeOfCosine.Title);

                            var resultHeaderX = textObject.PerformAnalyiticsHeaderTitle(descriptionCosineX, TypeOfCosine.Description);
                            var resultHeaderY = textObject.PerformAnalyiticsHeaderTitle(descriptionCosineY, TypeOfCosine.Description);
                            var resultHeaderZ = textObject.PerformAnalyiticsHeaderTitle(descriptionCosineZ, TypeOfCosine.Description);

                           

                            var headerCosineXY = identicalObjectArticle.CalculateCosineHeader(resultHeaderX, resultHeaderY);
                            var headerCosineYZ = identicalObjectArticle.CalculateCosineHeader(resultHeaderY, resultHeaderZ);
                            var headerCosineXZ = identicalObjectArticle.CalculateCosineHeader(resultHeaderX, resultHeaderZ);

                            var headerList = new List<VectorEntry> { headerCosineXY, headerCosineYZ, headerCosineXZ };

                            cosineSet.HeaderMostSimilarToArticles = headerList.OrderByDescending(x => x.CosineScore).First().Record; //Determine highest scoring header
                           

                            cosineSet.CosineTitleScoreAggregated = headerCosineXY.CosineScore + headerCosineYZ.CosineScore + headerCosineXZ.CosineScore;


                            cosineSets.Add(cosineSet);



                        }

                        cosineArraySets.Add(cosineSets) ;
                    }

    
                }

                
            }
                          

            return cosineArraySets;

        }

        /* Simple method to combine the final clusters based on publisher, i.e. (BBC news + Guardian) + (bbc news + CNN)  */
        public List<CosineArticleSet> DetermineFinalValues(List<List<CosineArticleSet>> cosineSets)
        {
            var list = new List<CosineArticleSet>();
            var elementTemp = new CosineArticleSet();
            var i = 0;
            foreach (var element in cosineSets.Select(x => x))
            {
              

                var xOrder = element.OrderByDescending(x => x.ArticleX.CosineScore).First(); //order in descending, based on all 3 of the cosine values in each cosine set. If the description and header 
                var YOrder = element.OrderByDescending(x => x.ArticleY.CosineScore).First();
                var ZOrder = element.OrderByDescending(x => x.ArticleZ.CosineScore).First();

           


                if ((xOrder.ArticleX.Record == YOrder.ArticleY.Record) && (xOrder.ArticleX.RecordComparedTo == YOrder.ArticleZ.Record) && (xOrder.ArticleY.RecordComparedTo == YOrder.ArticleZ.RecordComparedTo))
                {
                  
                    list.Add(new CosineArticleSet() { ArticleX = xOrder.ArticleX, ArticleY = YOrder.ArticleY, ArticleZ = ZOrder.ArticleZ, HeaderMostSimilarToArticles = xOrder.HeaderMostSimilarToArticles });

                } else if ((xOrder.ArticleX.Record == YOrder.ArticleY.Record) && (xOrder.ArticleX.RecordComparedTo == YOrder.ArticleZ.Record) && (ZOrder.CosineArticleScoreOtherArticles > 0.5) || ZOrder.CosineTitleScoreAggregated > 0.5)
                    {
                    list.Add(new CosineArticleSet() { ArticleX = xOrder.ArticleX, ArticleY = YOrder.ArticleY, ArticleZ = ZOrder.ArticleZ, HeaderMostSimilarToArticles = xOrder.HeaderMostSimilarToArticles });

                } else if 
                    ((xOrder.ArticleX.RecordComparedTo == YOrder.ArticleZ.Record) && (xOrder.ArticleY.RecordComparedTo == YOrder.ArticleZ.RecordComparedTo) && ((xOrder.CosineArticleScoreOtherArticles > 0.25) || xOrder.CosineTitleScoreAggregated > 0.25))
                    {
                    list.Add(new CosineArticleSet() { ArticleX = xOrder.ArticleX, ArticleY = YOrder.ArticleY, ArticleZ = ZOrder.ArticleZ, HeaderMostSimilarToArticles = xOrder.HeaderMostSimilarToArticles });

                }
                else if ((xOrder.ArticleX.Record == YOrder.ArticleY.Record) && (xOrder.ArticleY.RecordComparedTo == YOrder.ArticleZ.RecordComparedTo) && (YOrder.CosineArticleScoreOtherArticles > 0.25) || YOrder.CosineTitleScoreAggregated > 0.25)

                {

                    list.Add(new CosineArticleSet() { ArticleX = xOrder.ArticleX, ArticleY = YOrder.ArticleY, ArticleZ = ZOrder.ArticleZ, HeaderMostSimilarToArticles = xOrder.HeaderMostSimilarToArticles });
                }

                       
            }

            return list;
        }


    }
}
