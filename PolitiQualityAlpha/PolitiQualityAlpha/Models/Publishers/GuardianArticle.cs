using HtmlAgilityPack;
using PolitiQuality.Interfaces;
using PolitiQuality.Logic;
using PolitiQuality.Logic.FilterLogic;
using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace CommentUponLibrary.Articles.Articles
{
    /*  Gathering of Guardian articles and their contents*/
    public class GuardianArticle : Publisher
    {
        public async Task<List<HtmlRecord>> ComputeGuardianLinks(List<GoogleResult> filteredGuardianLinks)
        {
            var recordsGuardian = new List<HtmlRecord>();
           
                var guardianArticle = new GuardianArticle();
                var listOfNodes = new List<string>();
              

                foreach (var guardianLink in filteredGuardianLinks)
                {
                    var conn = await GetHtmlDoc(guardianLink.Link);

                    listOfNodes.AddRange(GatherRawHtml(conn));
                }
       
                var guardianNodesFiltered = listOfNodes.Distinct();


                foreach (var element in guardianNodesFiltered)
                {
                    var filter = new Filter();
                   
                    var description = filter.SplitString(@"""js-headline-text"">", @"</span></span>", element);


                    var link = filter.SplitString(@"href=""", @"""", element);

                    var record = new HtmlRecord { Link = link, Description = description };

                    record = await guardianArticle.GatherGuardianArticleProperties(record);
                        recordsGuardian.Add(record); //check tommoz
                    }
            return recordsGuardian;
        }

        public  List<string> GatherRawHtml(HtmlDocument conn)
        {
            var filter = new Filter();

            var listNodesString = new List<string>();
            
                       
            foreach (HtmlAgilityPack.HtmlNode node in conn.DocumentNode.Descendants("h3"))
            {
                if (node.InnerHtml.Contains("article") && node.InnerHtml.Contains("fc-item__link"))
                {
                    var result =  filter.CheckIfLinkExists(listNodesString, node.InnerHtml);

                    var resultCount = filter.CheckForProperPrefixSuffix(node.InnerHtml);

                    if (result && resultCount)

                        listNodesString.Add(node.InnerHtml);

                }
            }

            return listNodesString;
        }
        public async Task<HtmlRecord> GatherGuardianArticleProperties(HtmlRecord record)
        {

            var filter = new Filter();

            var doc = await GetHtmlDoc(record.Link);

            var articleList = doc.DocumentNode.Descendants("p").Where(i => i.ParentNode.OuterHtml.Contains("articleBody")).Select(i => i.InnerText);
 

            record.Article = filter.ConcantenateList(articleList.ToList());


            if (record.Article.Equals("") || record.Article.Equals(" "))
            {

                articleList = doc.DocumentNode.Descendants("p").Where(i => i.OuterHtml.Contains("articleBody")).Select(i => i.InnerText);

                record.Article = filter.ConcantenateList(articleList.ToList());

            }

            var authorList = doc.DocumentNode.Descendants("meta").Where(i => i.OuterHtml.Contains(@"meta name=""author""")).Select(i=> i.OuterHtml).ToList();
               
            var appendedAuthorList = authorList.Select(i => i.Split(new[] { @"content=" }, StringSplitOptions.None)[1]).ToList();

            var authors = appendedAuthorList.Select(i => i).AsEnumerable().Select(i => filter.ReplaceRegex(i, @"[^a-zA-Z0-9_. ]+"));
          
            record.Author = String.Join(", ", authors.ToArray());
            
            record.Date 
                = doc.DocumentNode.Descendants("meta")
                .Where(i => i.OuterHtml.Contains("published_time")).Select(i => i.Attributes["content"].Value).FirstOrDefault();

            bool descriptionFound = false;

            foreach (HtmlAgilityPack.HtmlNode node in doc.DocumentNode.DescendantsAndSelf("html"))
            {
                var text = node.InnerText.Trim();

                string[] titleCrop = text.Split(new[] { "\n" }, StringSplitOptions.None);

                var excessCut = titleCrop[0].Split(new[] { " |" }, StringSplitOptions.None);


                record.Header = excessCut[0];


            }

            foreach (HtmlAgilityPack.HtmlNode node in doc.DocumentNode.Descendants("meta"))
            {
                foreach (HtmlAgilityPack.HtmlAttribute attribute in node.Attributes)
                {

                    if (descriptionFound)
                    {
                        record.Description = attribute.Value;

                        return record;
                    }
                    if (attribute.Name.Contains("name") && attribute.Value.Contains("description"))
                    {
                        descriptionFound = true;

                    }

                }
            }
            return record;
        }

  
    }
}
