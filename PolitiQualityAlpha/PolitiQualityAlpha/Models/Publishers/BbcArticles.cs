using HtmlAgilityPack;
using PolitiQuality.Interfaces;
using PolitiQuality.Logic;
using PolitiQuality.Logic.FilterLogic;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentUponLibrary.Articles.Articles
{

    /*  Gathering of bbc articles and their contents*/
    public class BbcArticle : Publisher
    {
   

        public async Task <List<HtmlRecord>> ComputeBbcLinks(List<GoogleResult> filteredBbcLinks)
        {
            var bbcLinks = new List<string>();
            var listOfBbcRecords = new List<string>();
            var listOfLinksBbc = new List<string>();
            var conn = new HtmlDocument();

            var filterLogic = new Filter();
          
            var bbcRecord = new HtmlRecord();
            var bbcRecords = new List<HtmlRecord>();

            foreach (var element in filteredBbcLinks)
            { 

                if (!element.Link.Contains("live"))
                {
                    conn = await GetHtmlDoc(element.Link);

                    listOfLinksBbc.AddRange(GatherRawHtml(conn)); //get articles here
                }
            }
            var newLink = "";

            bool isOver12 = false;

            var distinctList = listOfLinksBbc.Select(x => x).Distinct().ToList();

            foreach (var link in distinctList) //should be distinct
            {
                if (isOver12)
                    break;                         
                    newLink = link;
                  
                    if (link.Contains("><span class="))
                        newLink = filterLogic.SplitString(null, "\"><span class=", link);

                    if (!newLink.Contains("live/") || !newLink.Contains("news/av"))
                    {                  
                    var record = new HtmlRecord { Link = newLink };

                        record = GetArticleContents(conn);      

                    if (record.Article != null)
                        {
                            bool result = record.Article.Any(x => !char.IsLetter(x));

                            if (result)

                            {
                                bbcRecords.Add(record);
                            }
                        }
                }

            }
            return bbcRecords;
        }

              
        public HtmlRecord GetArticleContents (HtmlDocument conn)
        {
            HtmlRecord record = new HtmlRecord();
                  
                record.Header = conn.DocumentNode.Descendants("title").FirstOrDefault().InnerText;

                           record.Author = conn.DocumentNode.Descendants("meta").Where(x => x.OuterHtml.Contains("article:author")).Select(xx => xx.Attributes["content"].Value).FirstOrDefault();

                var dateList = new List<string>();

                    dateList = conn.DocumentNode.SelectNodes("//div[@class='date date--v2']").Select(i => i.InnerText).ToList();

                    var dateItems = dateList.ElementAt(dateList.Count() - 1).Split(' ');

                    var month = (int)Enum.Parse(typeof(Month), dateItems.ElementAt(1).ToLower());

                    record.Date = dateItems.ElementAt(2) + "/" + month + "/" + dateItems.ElementAt(0) ;

                if (!record.Header.ToLower().Contains("quiz"))
                {

                    bool has = conn.DocumentNode.Descendants("p").Any(i => i.OuterHtml.Contains("story-body__introduction"));
                    if (has)
                        record.Description = conn.DocumentNode.Descendants("p").Where(i => i.OuterHtml.Contains("story-body__introduction")).FirstOrDefault().InnerText;

                    var articlesNodes = conn.DocumentNode.Descendants("p").Where(i => i.ParentNode.InnerHtml.Contains("media-with-caption") || i.ParentNode.InnerHtml.Contains("full-width lead")).ToList();

                    var innerHtmls = articlesNodes.Select(i => i.InnerText);

                    record.Article = String.Join(", ", innerHtmls.ToArray());

                    if (record.Article == " " || (record.Article == ""))
                    {
                        var articleList = conn.DocumentNode.Descendants("p").Where(i => i.ParentNode.InnerHtml.Contains("story-body__inner") || i.ParentNode.InnerHtml.Contains("articleBody")).ToList();

                        innerHtmls = articlesNodes.Select(i => i.InnerText);

                        record.Article = String.Join(", ", innerHtmls.ToArray());
                    }

                    if (record.Article == " " || (record.Article == ""))
                    {
                        var articleList = conn.DocumentNode.Descendants("p").Where(i => i.ParentNode.InnerHtml.Contains("story-body")).ToList();

                        innerHtmls = articlesNodes.Select(i => i.InnerText);

                        record.Article = String.Join(", ", innerHtmls.ToArray());
                    }

                    if (record.Article == " " || (record.Article == ""))
                    {

                        var article = "";
                        foreach (var element in conn.DocumentNode.SelectNodes("//div[@id='story-body']/p"))
                        {

                            if (element.InnerText != null)
                            {
                                article += " " + element.InnerText;

                            }
                        }

                        record.Article = article;
                    }

                }
            return record;

        }


        public List<string> GatherRawHtml(HtmlDocument conn)
        {

            var listOfLinks = new List<string>();
            var homepageLinks = conn.DocumentNode.Descendants("a")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("gs-c-promo-heading")).ToList();



            var filter = new Filter();
            foreach (var element in homepageLinks)
            {

                var link = filter.SplitString(@"href=""", "\"><h3", element.OuterHtml);
                link = "http://www.bbc.co.uk" + link;

                var result = filter.CheckIfLinkExists(listOfLinks, link);

                var resultCount = filter.CheckForProperPrefixSuffix(link);

                if (result && resultCount)
                listOfLinks.Add(link);


            }

            homepageLinks.ForEach(i => i.InnerText.Split(new[] { @"href=""" }, StringSplitOptions.None));
               




            return listOfLinks;


        }


          
    }
}
