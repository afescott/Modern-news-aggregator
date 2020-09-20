using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolitiQualityAlpha.Logic.FilterLogic.Identical_word_similarity
{
    public class IdenticalWordSimilarity
    {
        /* Two parameters, one for the article and the articles to be compared with */
        public IdenticalWordSimilarity(HtmlRecord recordX, List<HtmlRecord> recordsY)
        {
            this.RecordX = recordX;
            this.RecordsY = recordsY;

            

        }

         private HtmlRecord RecordX = new HtmlRecord();
        private List<HtmlRecord> RecordsY = new List<HtmlRecord>();

        /*  Calculates the cosine similarity score between an article, and the a list of articles belonging to another publisher */
        public List<VectorEntry> CalculateCosine ()
        {
          
            var entryList = new List<VectorEntry>();
                     
                foreach (var recordY in RecordsY)
                {
                 var entry = new VectorEntry();
                entry.RecordComparedTo = new HtmlRecord();

                entry.Record = RecordX;
                
                var listOfSimilarWords = GatherSimilarLocalVectorObjects(recordY); //Calculation of local vector (see documentation) 

                    var alphabetialisedA = listOfSimilarWords[0].OrderBy(x => x.Key).ToList();

                    var alphabetialisedB = listOfSimilarWords[1].OrderBy(x => x.Key).ToList();

          
                    var globalVector = (double)ConvertToLocalGlobalVector(alphabetialisedA, alphabetialisedB);  //Calculation of global vector (see documentation) 

                    var dotProduct12345 = CalculateDotNet(recordY);
                                  
                    entry.CosineScore = globalVector / dotProduct12345;

              
                    entry.VectorDictionaries = new Dictionary<string, int>[]
               {
    alphabetialisedA.ToDictionary(x => x.Key, x => x.Value) ,alphabetialisedB.ToDictionary(x => x.Key, x => x.Value)
                };


                entry.ArticleType = recordY.NewsPublisher;
                entry.RecordComparedTo = recordY;
                entry.Record = RecordX;

                    entryList.Add(entry);

                }
            

            return entryList;

         }

        /* Code used in preparation for article transfer into the database */
        public VectorEntry CalculateCosineHeader(HtmlRecord articleOrHeaderX, HtmlRecord articleOrHeaderY)   
        {
            var entry = new VectorEntry();

            entry.Record = articleOrHeaderX;

            entry.RecordComparedTo = articleOrHeaderY;

            this.RecordX = articleOrHeaderX;

            var listOfSimilarWords = GatherSimilarLocalVectorObjects(articleOrHeaderY);

            var alphabetialisedA = listOfSimilarWords[0].OrderBy(x => x.Key).ToList();

            var alphabetialisedB = listOfSimilarWords[1].OrderBy(x => x.Key).ToList();


            var globalVector = (double)ConvertToLocalGlobalVector(alphabetialisedA, alphabetialisedB);

            var dotProduct12345 = CalculateDotNet(articleOrHeaderY);
                  

            entry.CosineScore = globalVector / dotProduct12345;

       
            entry.VectorDictionaries = new Dictionary<string, int>[]
       {
    alphabetialisedA.ToDictionary(x => x.Key, x => x.Value) ,alphabetialisedB.ToDictionary(x => x.Key, x => x.Value)
        };


            return entry;



        }

        /*  Calculate local vector: This is the shared words between two articles, produces two dictionaries, one for each article with each value matching to a value in the other articles dictionary The frequency is recorded for both regardless of how many the similar word appears*/
        public Dictionary<string, int>[] GatherSimilarLocalVectorObjects(HtmlRecord recordY)
        {
                   
            var i = 0;
            var sharedWordsAndFrequenciesFromX = new Dictionary<string, int>();

            var sharedWordsAndFrequenciesFromY = new Dictionary<string, int>();

            var wordsAndFrequenciesY2 = new Dictionary<string, int>();

            var sharedWords = RecordX.ArticleWordsAndFrequency.Keys.Where(p => !recordY.ArticleWordsAndFrequency.Keys.Any(l => p == l)).ToList();


            var intCountX = sharedWordsAndFrequenciesFromX.Values.Sum();
            var intCountY = wordsAndFrequenciesY2.Values.Sum();

            foreach (var elementY in recordY.ArticleWordsAndFrequency.Keys)
            {
                if (!elementY.Equals(""))
                {
                    
                    if (RecordX.ArticleWordsAndFrequency.Keys.Contains(elementY))
                    {
                        sharedWordsAndFrequenciesFromY.Add(elementY, recordY.ArticleWordsAndFrequency[elementY]);


                    }
                }

            }

            foreach (var elementX in RecordX.ArticleWordsAndFrequency.Keys)
            {

                if (!elementX.Equals(""))
                { 
                    
                    if (recordY.ArticleWordsAndFrequency.Keys.Contains(elementX))
                    {

                        sharedWordsAndFrequenciesFromX.Add(elementX, RecordX.ArticleWordsAndFrequency[elementX]);
                    }

            }
            }

            return new Dictionary<string, int>[]
               {
    sharedWordsAndFrequenciesFromX,sharedWordsAndFrequenciesFromY
               };


            //identfieid words in same articles. Take this away from existing article words and frequencies 

        }

        /*  Identifies global vector by taking the shared words and frequencies between two articles and multiplying the frequencies of the shared word */
        public int ConvertToLocalGlobalVector(List<KeyValuePair<string, int>> sharedWordsAndFrequenciesX, List<KeyValuePair<string, int>> sharedWordsAndFrequenciesY)
            {



            var uniqueWordsAndFrequenciesX = new Dictionary<string, int>();

                var uniqueWordsAndFrequenciesY = new Dictionary<string, int>();

            var k = 0;
            var globalVectorSum = 0;

            foreach (var elementX in sharedWordsAndFrequenciesX)
            {
                var elementY = (from val in sharedWordsAndFrequenciesY where val.Key == elementX.Key select val.Value).FirstOrDefault();

                var result = elementX.Value * elementY;


                globalVectorSum += result;

            }
        

            return globalVectorSum;

            }


        /*  Dot net calculation used in Chali algorithm, returns a double between 0 and 1, corresponding to the cosine score*/
        public double CalculateDotNet(HtmlRecord recordY)
        {

         
            var alphabetialisedLocalA = RecordX.ArticleWordsAndFrequency.OrderBy(x => x.Key).ToList();

            var alphabetialisedLocalB = recordY.ArticleWordsAndFrequency.OrderBy(x => x.Key).ToList();


            var xList = alphabetialisedLocalA.Select(i => i.Value).ToList();

            var yList = alphabetialisedLocalB.Select(i => i.Value).ToList();


          

            var dotProductX = xList.Zip(xList, (a, b) => a * b).Sum();
            var dotProductY = yList.Zip(yList, (a, b) => a * b).Sum();

               

         
            return   Math.Sqrt(dotProductX * dotProductY);

        }


 
     
        }
    }


