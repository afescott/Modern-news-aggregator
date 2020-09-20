
using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data.Entity.Design.PluralizationServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using PolitiQualityAlpha.Logic.Objects;
using PolitiQualityAlpha.Logic.FilterLogic.Identical_word_similarity;
using PolitiQualityAlpha.Logic.Enums;

namespace PolitiQuality.Logic.FilterLogic
{
    /*  Each method in this class represents the pre-requirement for the cosine smilarity algorithm */
    public class ArticleText
    {

        private Filter filter = new Filter();

        private string ArticleContents = "";
   
        private void RemoveSpecialCharacters()
        {
            ArticleContents = filter.ReplaceRegex(ArticleContents, "[^a-zA-Z_ ]+");
        }

        private string RemoveStopWords()
        {

            ArticleContents = ArticleContents.ToLower();

            StopWord stopWord = new StopWord();

            StringBuilder input = new StringBuilder(ArticleContents);

            var splitList = ArticleContents.Split(' ').ToList();

            var splitList1 = new List<string>();

            foreach (var word in splitList)
            {
                var checkIfStopWord = stopWord.StopWords.Any(i => i.Equals(word));

                if (!checkIfStopWord)
                {
                    splitList1.Add(word);
                }

            
             
            }
            var jj = string.Join(" ", splitList1);
            ArticleContents = string.Join(" ", splitList1);

            return string.Join(" ", splitList1);
        }


        public string ConvertToSingular()
        {

            CultureInfo ci = new CultureInfo("en-us");

            var j = PluralizationService.CreateService(ci);

            var articleSingularized = "";

            foreach (var element in ArticleContents.Split(' '))
            {
                var singular = j.Singularize(element);

                articleSingularized += singular + " ";
            }

            return articleSingularized;

        }

  
        public Dictionary<string,int> TrackUniqueWords()
        {

          var  wordPairs = new Dictionary<string, int>();
                                         
    
              

               
                foreach (var word in ArticleContents.Split(' '))
                {
                    
                    if (!wordPairs.ContainsKey(word))
                    {
                      
                        wordPairs.Add(word, 1);
                    }
                    else
                    {
                        wordPairs[word] = wordPairs[word] + 1;

                    }
                


               
            }

            return wordPairs;
        }

        public List<HtmlRecord> PerformAnalyitics(List<HtmlRecord> records , string articleType)
        {


            var recordsFinal = new List<HtmlRecord>();
            foreach (var element in records)
            {
                if (records.FirstOrDefault().CosineType == TypeOfCosine.Description)
                {
                    ArticleContents = element.Description;
                }
                else if (records.FirstOrDefault().CosineType == TypeOfCosine.Title)
                {

                    ArticleContents = element.Header;
                } else {


                    ArticleContents = element.Article;
                }

                RemoveSpecialCharacters();

                RemoveStopWords();

                if (articleType != "")
                {

                    var publisher = (PublisherEnum)System.Enum.Parse(typeof(PublisherEnum), articleType);

                    element.NewsPublisher = publisher;
                }

                 ArticleContents =ConvertToSingular(); 

                element.Article = ArticleContents;

                element.ArticleWordsAndFrequency = TrackUniqueWords();

                element.ArticleUniqueWords = new List<string>();


                foreach (var item in element.ArticleWordsAndFrequency.Where(x => x.Value == 1))
                {
                    element.ArticleUniqueWords.Add(item.Key);

                }

                recordsFinal.Add(element);
            }

            return recordsFinal;

        }


        public HtmlRecord PerformAnalyiticsHeaderTitle(HtmlRecord headerOrTitleRecord, TypeOfCosine typeOfCosine)
        {

            var record = headerOrTitleRecord;

            if (typeOfCosine == TypeOfCosine.Title)
            {
                ArticleContents = headerOrTitleRecord.Header;
            } else
            {
                ArticleContents = headerOrTitleRecord.Description;

            }
            RemoveSpecialCharacters();

            RemoveStopWords();

          
            ArticleContents = ConvertToSingular(); // not sure about this, might need the original article for the user

            record.Article = ArticleContents;

            record.ArticleWordsAndFrequency = TrackUniqueWords();

            record.ArticleUniqueWords = new List<string>();


            foreach (var item in record.ArticleWordsAndFrequency.Where(x => x.Value == 1))
            {
                record.ArticleUniqueWords.Add(item.Key);

            }



            return record;

        }

            public List<VectorEntry> PerformCosineAnalyitics(List<HtmlRecord> listToCompare, List<HtmlRecord> listToCompareWithX, List<HtmlRecord> listToCompareWithY)
        {
            
            var returnList = new List<List<VectorEntry>>();
            
                foreach (var element in listToCompare)
                {

                    listToCompareWithX.ForEach(y => y.CosineType = TypeOfCosine.Article);
                    listToCompareWithY.ForEach(y => y.CosineType = TypeOfCosine.Article);

                    IdenticalWordSimilarity identicalBbc = new IdenticalWordSimilarity(element, listToCompareWithX);

                    var listCnn = identicalBbc.CalculateCosine();

                    IdenticalWordSimilarity identicalGuardian = new IdenticalWordSimilarity(element, listToCompareWithY);


                    listCnn.AddRange(identicalGuardian.CalculateCosine());

                    returnList.Add(listCnn);


                
            }
          
            return returnList.SelectMany(x => x).ToList();
        }




    }


}
