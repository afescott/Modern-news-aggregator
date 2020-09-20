
using CommentUponLibrary.Articles.Articles;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Enums;
using PolitiQualityAlpha.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PolitiQualityAlpha.Logic.FilterLogic.Identical_word_similarity
{

    /*Implementation of Chali's grouping algorithm*/
    public class GroupingAlgorithm
    {

        public List<FinalCluster> FinalClusters = new List<FinalCluster>();

       private Dictionary<HtmlRecord, int> ArticleFinalClusterCount = new Dictionary<HtmlRecord, int>(); //final clusters with number of records value

        public List<FinalCluster> ArticleFinalClustersVectors = new List<FinalCluster>(); //used to temporarily hold final cluster data

        /* Determine s */
        public void RecordHighThresholdArticleCount(List<HtmlRecord> entries)

        {
            foreach (var entry in entries)
            {
                if (ArticleFinalClusterCount.ContainsKey(entry))
                {
                    ArticleFinalClusterCount[entry] += 1;
                }
                else
                {
                    ArticleFinalClusterCount.Add(entry, 1);

                }
            }

        }

   

        public void AddToFinalCluster(List<VectorEntry> entries, double average, HtmlRecord record, int clusterNo)
        {

            ArticleFinalClustersVectors.Add(new FinalCluster { VectorEntries = entries, AggregatedCosineScoreAverage = average, Article = record, ClusterNumber = clusterNo });
        }

        /* Determine if the same article appears in another cluster, as per Chaili's grouping algorithm*/
        public int CheckTextsOverlap(VectorEntry entry, int strikeCount)
        {

            var records = ArticleFinalClustersVectors.Select(i => i).ToList();

            var matchFound = records.Any(i => i.VectorEntries.Any(ii => ii.RecordComparedTo == entry.RecordComparedTo));

            if (matchFound)
                return strikeCount += 1;

            return strikeCount;


        }

        /*Remove articles from other final clusters, as the correct cluster has been assigned to the vector entry */
        public void RemoveOverlappingTexts(HtmlRecord record)
        {

            foreach (var element in ArticleFinalClustersVectors.Select(i => i.VectorEntries.Select(ii => ii.RecordComparedTo)))
            {

                if (element.Contains(record))
                {
                    ArticleFinalClustersVectors
                        .Select(j => j.VectorEntries.RemoveAll((x) => x.RecordComparedTo == record));


                    ArticleFinalClustersVectors
                        .Select(j => j.VectorEntries).ToList().Select(jj => jj.RemoveAll((x) => x.RecordComparedTo == record));

                }
            }

        }

        /* Determine initial clusters. These compare articles from one publisher, with articles from the other two. 
         Each cluster, represents an article, along with articles from the other two publishers that meet the high threshold */
        public List<HighthresholdResult>[] GetThresholdResults(List<VectorEntry> listsOfCosineResults)
        {

            var thresholdResultsX = new List<HighthresholdResult>();
            var thresholdResultsY = new List<HighthresholdResult>();

            var recordsUnique = listsOfCosineResults.Select(x => x.Record).Distinct().ToList();

            foreach (var record in recordsUnique)
            {
                var selectRecords = listsOfCosineResults.Where(x => x.Record == record).ToList();




                var element1 = new VectorEntry();

                var resultPublisherX = new HighthresholdResult();
                var resultPublisherY = new HighthresholdResult();

                resultPublisherX.VectorEntries = new List<VectorEntry>();
                resultPublisherY.VectorEntries = new List<VectorEntry>();

                var elementsBbc = new List<VectorEntry>();
                var elementsGuardian = new List<VectorEntry>();


          

                resultPublisherY.AggregatedCosineScoreAverage = 0;
                resultPublisherX.AggregatedCosineScoreAverage = 0;

                var publishers = selectRecords.Select(x => x.ArticleType).Distinct();





                var resultsPublisherX = selectRecords.Select(i => i).Where(ii => ii.ArticleType.Equals(publishers.ElementAt(0))).ToList();



                var resultsPublisherY = selectRecords.Select(i => i).Where(ii => ii.ArticleType.Equals(publishers.ElementAt(1))).ToList();






                var resultsLowThresholdPublisherY = GetLowHighThresholdResults(resultsPublisherY);

                var resultsHighThresholdPublisherY = GetHighThresholdResults(resultsPublisherY);

                resultsHighThresholdPublisherY.ForEach(i => i.IsHighThreshold = true);


                resultsHighThresholdPublisherY.AddRange(resultsLowThresholdPublisherY);




                var resultsLowThresholdPublisherX = GetLowHighThresholdResults(resultsPublisherX);


                var resultsHighThresholdPublisherX = GetHighThresholdResults(resultsPublisherX);

                resultsHighThresholdPublisherX.ForEach(i => i.IsHighThreshold = true);

                resultsHighThresholdPublisherX.AddRange(resultsLowThresholdPublisherX);

                RecordHighThresholdArticleCount(resultsHighThresholdPublisherY.Select(i => i.RecordComparedTo).ToList());
                RecordHighThresholdArticleCount(resultsHighThresholdPublisherX.Select(i => i.RecordComparedTo).ToList());



                var publisherXHighThresholdResultBool = resultsLowThresholdPublisherX.Any();
                var publisherXThresholdResultBool = resultsHighThresholdPublisherX.Any();



                if ((resultPublisherX != null) && (publisherXHighThresholdResultBool || publisherXThresholdResultBool))
                {

                    resultPublisherX.Record = resultsHighThresholdPublisherX.Select(i => i.Record).FirstOrDefault();

                    resultPublisherX.HighestValueNeighbour = resultsHighThresholdPublisherX.OrderByDescending(x => x.CosineScore).ElementAt(resultsHighThresholdPublisherX.Count() - 1).RecordComparedTo;  //retrieve highest value neighbour. Used for the leftover texts check

                    if (resultsHighThresholdPublisherX.Count > 0)
                    {

                        resultPublisherX.VectorEntries.AddRange(resultsHighThresholdPublisherX);

                        if (resultsHighThresholdPublisherX.Count() > 1)
                        {

                            resultPublisherX.TotalCosineScore = resultsHighThresholdPublisherX.Select(x => x.CosineScore).Sum();  //in case of many vector entries in a highthresholdresult


                            resultPublisherX.AggregatedCosineScoreAverage = resultPublisherX.TotalCosineScore / resultsHighThresholdPublisherX.Count();

                            thresholdResultsX.Add(resultPublisherX);

                        }
                        else
                        {
                            resultPublisherX.TotalCosineScore = resultsHighThresholdPublisherX.ElementAt(0).CosineScore;              //in case of only one vector entry in a highthresholdresult
                            resultPublisherX.AggregatedCosineScoreAverage = resultsHighThresholdPublisherX.ElementAt(0).CosineScore;
                            thresholdResultsX.Add(resultPublisherX);
                        }



                    }

                }

                var publisherYHighThresholdResultBool = resultsHighThresholdPublisherY.Any();
                var publisherYLowThresholdResultBool = resultsLowThresholdPublisherY.Any();


                if ((resultPublisherY != null) && (publisherYLowThresholdResultBool || publisherYHighThresholdResultBool))
                {

                    resultPublisherY.Record = resultsHighThresholdPublisherY.Select(i => i.Record).FirstOrDefault();

                    resultPublisherY.HighestValueNeighbour = resultsHighThresholdPublisherY.OrderByDescending(x => x.CosineScore).ElementAt(resultsHighThresholdPublisherY.Count() - 1).RecordComparedTo;

                    GroupingAlgorithm algorithm = new GroupingAlgorithm();


                    if (resultsHighThresholdPublisherY.Count > 0)
                    {

                        resultPublisherY.VectorEntries.AddRange(resultsHighThresholdPublisherY); //add vector entries that meet the high threshold. 



                        if (resultsHighThresholdPublisherY.Count() > 1)
                        {


                            resultPublisherY.TotalCosineScore = resultsHighThresholdPublisherY.Select(x => x.CosineScore).Sum();

                            resultPublisherY.AggregatedCosineScoreAverage = resultPublisherY.TotalCosineScore / resultsHighThresholdPublisherY.Count();
                            thresholdResultsY.Add(resultPublisherY);



                        }
                        else
                        {
                            resultPublisherY.TotalCosineScore = resultsHighThresholdPublisherY.ElementAt(0).CosineScore;
                            resultPublisherY.AggregatedCosineScoreAverage = resultsHighThresholdPublisherY.ElementAt(0).CosineScore;
                            thresholdResultsY.Add(resultPublisherY);
                        }
                    }


                }

                    
    

            }
           

            return new List<HighthresholdResult>[] { thresholdResultsX, thresholdResultsY };

        }

        /* Calculates the average score between article (vectorentries) entries in a cluster and an article, by doing a cosine comparison  */
        public KeyValuePair<bool, int> FindHighestScoringCluster(List<FinalCluster> clusters, HtmlRecord record)
        {
           
                
                var calculationClusters = new List<FinalCluster>();
                foreach (var cluster in clusters)
                {
                    var calculationCluster = new FinalCluster();
                    var vecEntries = cluster.VectorEntries.Select(t => t).ToList().Select(tt => tt).ToList();


                    var recordsForCosineProcessing = vecEntries.Select(x => x.RecordComparedTo).ToList();
                    recordsForCosineProcessing.ForEach(x => x.CosineType = TypeOfCosine.Article);


                    IdenticalWordSimilarity identicalBbc = new IdenticalWordSimilarity(record, recordsForCosineProcessing); //cosine algorithm, not modification required, which does not violate OOD

                    var listOfLeftoverClusters = identicalBbc.CalculateCosine();

                    calculationCluster.ClusterNumber = cluster.ClusterNumber;

                    calculationCluster.VectorEntries = cluster.VectorEntries;

                    calculationCluster.AggregatedCosineScoreAverage = listOfLeftoverClusters.Sum(x => x.CosineScore += x.CosineScore) / listOfLeftoverClusters.Count(); 

                    calculationClusters.Add(calculationCluster);
                }

                var HighestScoringCluster = calculationClusters.OrderByDescending(x => x.AggregatedCosineScoreAverage).ElementAt(calculationClusters.Count() - 1);

                if (HighestScoringCluster.AggregatedCosineScoreAverage >= 0.60) //if the average of an article compared with the entries in a final cluster is higher than the high threshold,
                {
                    return new KeyValuePair<bool, int>(true, HighestScoringCluster.ClusterNumber);
                }
            
            

            return new KeyValuePair<bool, int>(false, 0);

        }

        /* Determine the cluster in which the highest value neighbour is, in case it becomes a leftover text*/
        public List<KeyValuePair<HtmlRecord, int>> CalculateClusterNumberOfNeighbours(List<FinalCluster> finalClusters, List<HighthresholdResult> highThresholdResults)
        {
            var entryAndNeighbour = new List<KeyValuePair<HtmlRecord, int>>();

            foreach (var element in highThresholdResults.SelectMany(x => x.VectorEntries))
            {
                var finalClustersVectorEntries = finalClusters.SelectMany(x => x.VectorEntries).Select(xx => xx.RecordComparedTo);
                if (finalClustersVectorEntries.Contains(element.RecordComparedTo))
                {
                    var clusterNumber = finalClusters.Where(x => x.VectorEntries.Select(xx => xx.RecordComparedTo).Contains(element.RecordComparedTo)).First().ClusterNumber;

                    entryAndNeighbour.Add(new KeyValuePair<HtmlRecord, int>(element.RecordComparedTo, clusterNumber));

                }
            }

            return entryAndNeighbour;

        }

        /* Final cluster calculation based around (but not exactly) Chali's pseudo-code (see documentation) */
        public List<HighthresholdResult> CalculateFinalClusters(List<HighthresholdResult> highThresholdResults)
        {
            var leftOverClusters = new List<HighthresholdResult>();

            ArticleFinalClustersVectors = new List<FinalCluster>();

            var totalCosineScoreDescending = highThresholdResults.OrderByDescending(i => i.TotalCosineScore);

            var clustersCopy = totalCosineScoreDescending.ToList(); //temp variable

            var finalClusterCounter = 0;

            foreach (var element in totalCosineScoreDescending)
            {
                var entryCopy = element;

                element.IsFinalCluster = true;
                int strikeCount = 0; //counter to determine how many times an article appears 
              





                foreach (var entry in element.VectorEntries)
                {
                    strikeCount += CheckTextsOverlap(entry, strikeCount); 

                    if (strikeCount == 3 || element.VectorEntries.Count() < 4)
                    {
                        entryCopy.IsFinalCluster = false; //if the condition is met, it cannot be a final cluster

                        leftOverClusters.Add(element);

                        break;
                    }

                  

                }
                if (entryCopy.IsFinalCluster)
                {
                    clustersCopy.Remove(element);

                    finalClusterCounter += 1;

                  

                    foreach (var record in element.VectorEntries.Select(i => i.RecordComparedTo)) 
                    {

                        RemoveOverlappingTexts(record); //remove the same article from the other clusters 


                    }

                    AddToFinalCluster(element.VectorEntries, element.AggregatedCosineScoreAverage, element.Record, finalClusterCounter);



               


                }

            }

            return leftOverClusters;

        }

   

      

        /*  Determine what is done with the leftover texts, as per the final subsection in Chali's grouping algorithm  */
        public List<HtmlRecord> FinaliseFinalClusters(List<HighthresholdResult> leftOverClusters, List<FinalCluster> finalClusters, List<KeyValuePair<HtmlRecord, int>> highestValueEntriesAndCluster)
        {




            var jj = new VectorEntry();

            var mm = new List<HtmlRecord>();

            var leftOverTexts = new List<HtmlRecord>();

                var leftOverVectors = leftOverClusters.SelectMany(x => x.VectorEntries).ToList();



                foreach (var element in leftOverVectors)
                {


                    if (element.VectorDictionaries.Count() == 0)
                    {


                        var keyValueCluster = FindHighestScoringCluster(finalClusters, element.Record);


                        var neighbour = leftOverClusters.Select(x => x).Where(xx => xx.Record == element.Record).FirstOrDefault();

                        var highestValueNeighbourIsInFinalClusters = false;

                        var finalClustersArticles = finalClusters.SelectMany(x => x.VectorEntries.Select(xx => xx.RecordComparedTo));

                        if (finalClustersArticles.Contains(neighbour.HighestValueNeighbour))
                        {
                            highestValueNeighbourIsInFinalClusters = true;
                        }

                        if (keyValueCluster.Key) //if the average of an article compared with the entries in a final cluster is higher than the high threshold, the leftover text can be added
                    {
                            finalClusters.Select(x => x).Where(xx => xx.ClusterNumber == keyValueCluster.Value).Select(x => x.VectorEntries).Select(x => x).First().Add(element);
                        }

                        else if (highestValueNeighbourIsInFinalClusters) //if highest value neighbour is in one of the final clusters 
                        {

                               jj = element;
                            //if the max neighbour is in the list of ck add to the cluster entry.Record , where the cluster contains the record.recordcomparedto of the entry we're adding


                            var entrie12s = highestValueEntriesAndCluster.Select(x => x.Key).ToList();

                            mm = highestValueEntriesAndCluster.Select(x => x.Key).ToList();

                            var valueNumber = highestValueEntriesAndCluster.Where(x => x.Key == element.RecordComparedTo).First().Value;

                            finalClusters.Select(x => x).Where(xx => xx.ClusterNumber == valueNumber).Select(x => x.VectorEntries).Select(x => x).First().Add(element);




                        }
                        else
                        {
                            leftOverTexts.Add(element.RecordComparedTo); //else add it to leftover texts that are useless (but could be useful in the future)
                        }



                    }

                                   }
            
          
            FinalClusters = finalClusters;
            return leftOverTexts;
        }

        public List<VectorEntry> GetHighThresholdResults(List<VectorEntry> entries)
        {
            var thresholdResults = (from val in entries where val.CosineScore > 0.60 select val).ToList();

            return thresholdResults;
        }

        public List<VectorEntry> GetLowHighThresholdResults(List<VectorEntry> entries)
        {
            var thresholdResults = (from val in entries where val.CosineScore > 0.3 && val.CosineScore < 0.5 select val).ToList();

            return thresholdResults;
        }

    }
}
