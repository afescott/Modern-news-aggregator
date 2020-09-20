using CommentUponLibrary.Articles.Articles;
using HtmlAgilityPack;
using PolitiQuality.Logic;
using PolitiQuality.Logic.FilterLogic;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.FilterLogic.Identical_word_similarity;
using PolitiQualityAlpha.Logic.Objects;
using System.Collections.Generic;
using System.Linq;

namespace PolitiQualityAlpha.Logic.Models
{
    public class FindArticleInfoModel
    {




        /* Model to communicate with the article similarity sprint side */

        public async void CalculateNewsArticles()
        {
            

            var filter = new Filter();
            

            GoogleSearch Search = new GoogleSearch();
            var cnnArticle = new CnnArticle();
            var bbcArticle = new BbcArticle();
            var guardianArticle = new GuardianArticle();


            var list = new List<GoogleResult>();

            
            var guardianList = filter.GetPublisherSearchList("Guardian");
            var cnnList = filter.GetPublisherSearchList("CNN");
            var sunList = filter.GetPublisherSearchList("Sun");
            var bbcList = filter.GetPublisherSearchList("BBC");

            System.Collections.ArrayList lists = new System.Collections.ArrayList();

            lists.Add(guardianList);
            lists.Add(cnnList);
            lists.Add(bbcList);
            lists.Add(sunList);

            var data = new List<string>[] { guardianList, cnnList, bbcList, sunList };





            var guardianListResults = Search.Search(data[0]);
            var cnnListResults = Search.Search(data[1]);
            var bbcListResults = Search.Search(data[2]);
            var sunListResults = Search.Search(data[3]);


            var filteredGuardianLinks = filter.FilterLinks(guardianListResults, "theguardian.com");
            var filteredCnnLinks = filter.FilterLinks(cnnListResults, "cnn.com");
            var filteredBbcLinks = filter.FilterLinks(bbcListResults, "bbc.co.uk");
            var filteredSunLinks = filter.FilterLinks(sunListResults, "sun.co.uk");





            var n = 0;


            var listOfNodesBbc = new List<HtmlNode>();

   
            var bbcLinks = new List<string>();



          

            var bbcRecords = await bbcArticle.ComputeBbcLinks(filteredBbcLinks);
            bbcRecords = bbcRecords.Where(s => !string.IsNullOrWhiteSpace(s.Article)).Distinct().ToList();

            var cnnRecords = await cnnArticle.ComputeCnnLinks(filteredCnnLinks);
            cnnRecords = cnnRecords.Where(s => !string.IsNullOrWhiteSpace(s.Article)).Distinct().ToList();
            var guardRecords = await guardianArticle.ComputeGuardianLinks(filteredGuardianLinks);
      

            guardRecords = guardRecords.Where(s => !string.IsNullOrWhiteSpace(s.Article)).Distinct().ToList();




           

            var listerGuardian = new List<string>();
            var listerBbc = new List<string>();

            ArticleText text = new ArticleText();

            var bbcRecordsFinal = text.PerformAnalyitics(bbcRecords, "BBC");

            var cnnRecordsFinal = text.PerformAnalyitics(cnnRecords, "CNN");

            var guardianRecordsFinal = text.PerformAnalyitics(guardRecords, "Guardian");



            var listsOfArticlesAndComparisonsBbc      = text.PerformCosineAnalyitics(bbcRecordsFinal, guardianRecordsFinal, cnnRecordsFinal);

            var listsOfArticlesAndComparisonsGuardian     = text.PerformCosineAnalyitics(guardianRecordsFinal, bbcRecordsFinal, cnnRecordsFinal);

            var listsOfArticlesAndComparisonsCnn       = text.PerformCosineAnalyitics(cnnRecordsFinal, bbcRecordsFinal, guardianRecordsFinal);


            var broken = listsOfArticlesAndComparisonsGuardian.Select(x => x).Where(xx => xx.CosineScore > 0.8).ToList();


      

            GroupingAlgorithm algorithm = new GroupingAlgorithm();


            var cnnBbcClusters = new List<VectorEntry>();
            var cnnGuardianClusters = new List<VectorEntry>();

            var cnnListFinal = listsOfArticlesAndComparisonsCnn;

            var lowThresholdListGuardian = new List<HighthresholdResult>();
            var highThresholdListGuardian = new List<HighthresholdResult>();


            var lowThresholdListBbc = new List<HighthresholdResult>();
            var highThresholdListBbc = new List<HighthresholdResult>();

            var thresholdListBbc = new List<HighthresholdResult>();

            var resultsCnn = algorithm.GetThresholdResults(listsOfArticlesAndComparisonsCnn);





            var resultsBbc = algorithm.GetThresholdResults(listsOfArticlesAndComparisonsBbc);


            var resultsGuardian = algorithm.GetThresholdResults(listsOfArticlesAndComparisonsGuardian);
                       




             var leftOverBbcGuardian = algorithm.CalculateFinalClusters(resultsBbc[0]);

            var finalClustersBbcGuard = algorithm.ArticleFinalClustersVectors;

            var leftOverBbcCnn = algorithm.CalculateFinalClusters(resultsBbc[1]);

            var finalClustersBbcCnn = algorithm.ArticleFinalClustersVectors;


            var leftOverCnnBbc = algorithm.CalculateFinalClusters(resultsCnn[0]);

            var finalClustersCnnBbc = algorithm.ArticleFinalClustersVectors;

            var leftOverCnnGuard = algorithm.CalculateFinalClusters(resultsCnn[1]);

            var finalClustersCnnGuard = algorithm.ArticleFinalClustersVectors;


            var leftOverGuardCnn = algorithm.CalculateFinalClusters(resultsGuardian[0]);

            var finalClustersGuardCnn = algorithm.ArticleFinalClustersVectors;

            var leftOverGuardBbc = algorithm.CalculateFinalClusters(resultsGuardian[1]);

            var finalClustersGuardBbc = algorithm.ArticleFinalClustersVectors;

               
            var entryAndClusterBbcCnn = algorithm.CalculateClusterNumberOfNeighbours(finalClustersBbcCnn, resultsBbc[1].ToList());
            var entryAndClusterBbcGuard = algorithm.CalculateClusterNumberOfNeighbours(finalClustersBbcGuard, resultsBbc[0].ToList());

            var entryAndClusterGuardBbc = algorithm.CalculateClusterNumberOfNeighbours(finalClustersGuardBbc, resultsGuardian[1].ToList());
            var entryAndClusterGuardCnn = algorithm.CalculateClusterNumberOfNeighbours(finalClustersGuardCnn, resultsGuardian[0].ToList());

            var entryAndClusterCnnBbc = algorithm.CalculateClusterNumberOfNeighbours(finalClustersCnnBbc, resultsCnn[0].ToList());
            var entryAndClusterCnnGuard = algorithm.CalculateClusterNumberOfNeighbours(finalClustersCnnGuard, resultsCnn[1].ToList());


            var leftOverBbcCnnTexts = algorithm.FinaliseFinalClusters(leftOverBbcCnn, finalClustersBbcCnn, entryAndClusterBbcCnn);

            var finalClustersBbcCnnFinal = algorithm.FinalClusters;


            var leftOverBbcGuardTexts = algorithm.FinaliseFinalClusters(leftOverBbcGuardian, finalClustersBbcGuard, entryAndClusterBbcGuard);

            var finalClustersBbcGuardFinal = algorithm.FinalClusters;


            var leftoverCnnBbcTexts = algorithm.FinaliseFinalClusters(leftOverCnnBbc, finalClustersCnnBbc, entryAndClusterCnnBbc);

            var finalClustersCnnBbcFinal = algorithm.FinalClusters;


            var leftoverCnnGuardTexts = algorithm.FinaliseFinalClusters(leftOverCnnGuard, finalClustersCnnGuard, entryAndClusterCnnGuard);

            var finalClustersCnnGuardFinal = algorithm.FinalClusters;


            var leftOverGuardBbcTexts = algorithm.FinaliseFinalClusters(leftOverGuardBbc, finalClustersGuardBbc, entryAndClusterGuardBbc);

            var finalClustersGuardBbcFinal = algorithm.FinalClusters;


            var leftOverGuardCnnTexts = algorithm.FinaliseFinalClusters(leftOverGuardCnn, finalClustersGuardCnn, entryAndClusterGuardCnn);

            var finalClustersGuardCnnFinal = algorithm.FinalClusters;


            var grouping = new DatabaseArticleGroup();


         var bbcResults =   grouping.CombineClusters(finalClustersBbcCnnFinal, finalClustersBbcGuardFinal);

           var cnnResults =  grouping.CombineClusters(finalClustersCnnBbcFinal, finalClustersCnnGuardFinal);

            var guardianResults = grouping.CombineClusters(finalClustersGuardBbcFinal, finalClustersGuardCnnFinal);

            List<FinalCluster[]> combinationOfClusters = new List<FinalCluster[]>();


           var cosineSets = grouping.PrepareForDatabase(cnnResults);


           var articles = grouping.DetermineFinalValues(cosineSets);
            

            NewsConnection connection = new NewsConnection(); 

            connection.InsertArticleSet(articles); //insertion of article sets into database
                
            





        }



        

        }

    }
    

