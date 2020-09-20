using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PolitiQuality.Logic.FilterLogic
{
    /*  Validation class */
    public class Filter
    {
        /* Check for link duplicates  */
        public bool CheckIfLinkExists(List<string> links, string link)

        {

            var linkFound = false;

            if (links.Contains(link))
                return linkFound;



            return true;
        }

        public bool CheckForProperPrefixSuffix (string link)
        {
            var countPrefix = Regex.Matches(link, @"http").Count;

            var countSuffixCom = Regex.Matches(link, @".com").Count;
            var countSuffixCo = Regex.Matches(link, @".co.uk").Count;


            if (countPrefix == 1 && (countSuffixCom == 1 || countSuffixCo == 1))
                return true;

            return false;

        }

        /*  Gets publisher search phrases for google api algorithm */
        public List<string> GetPublisherSearchList (string publisher)
        {

            List<string> list = new List<string>();
       

            list.Add(publisher + " U.k. news");
            list.Add(publisher + " world news");
            list.Add(publisher + "U.S news");
            list.Add(publisher + " sport");
            list.Add(publisher + " Politics");
            list.Add(publisher + " health");
            list.Add(publisher + " top news");

            return list;
        }



        /*  Remove duplicate API return results  */
        public List<GoogleResult> FilterLinks (List<GoogleResult> results, string keyword)

        {

            GoogleSearch search = new GoogleSearch();

            var results123 = results;

            var listToRemove = new List<GoogleResult>();

            results.RemoveAll(i => !i.Link.Contains(keyword));

             var finalVersion = results.Distinct().ToList();

            return finalVersion;

        }

        /* Split string based on optional split */
        public string SplitString (string splitPhraseOne, string splitPhraseTwo, string listElement)
        {

            var link = "";
                if (splitPhraseOne == null)
                {

               link =  listElement.Split(new[] { splitPhraseTwo }, StringSplitOptions.None)[0];
                return link;
            }
                else if (splitPhraseTwo == null)
                {

              link =  listElement.Split(new[] { splitPhraseOne }, StringSplitOptions.None)[1];
                return link;
            }

              var htmlSplit = listElement.Split(new[] { splitPhraseOne }, StringSplitOptions.None)[1];
            

           link= htmlSplit.Split(new[] { splitPhraseTwo }, StringSplitOptions.None)[0];
            

            return link;

        }

        public string AppendLink (string phrase, string link)

        {
            if (!link.Contains("cnn.com"))
            {
                return phrase + link;
            }

            return link;
        }

        /*  Regular expression execution method  */
        public string ExecuteRegex (string regexPattern, string phrase)
        {

            Match match = Regex.Match(phrase, regexPattern);

            return match.Value;
        }
        /* Regular expression string replacement method  */
        public string ReplaceRegex (string phrase, string regexPattern)
        {
           
            return Regex.Replace(phrase, regexPattern, "", RegexOptions.Compiled);

        }


        public bool CheckForDuplicates (List<string> links , string link)
        {
            if (links.Contains(link)) 
                {
                return true;
                
            }

            return false;
        }

        /* Appending of a list */
        public string ConcantenateList ( List <string> list )

        {
            if (list.Count() > 0)
            {

                var distinctList = list.Select(x => x).Distinct().ToArray();


               return  string.Join(", ", distinctList);

            }

            return "";
        }

    
    }
}
