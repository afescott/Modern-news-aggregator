using PolitiQuality.Interfaces;
using PolitiQuality.Logic;
using PolitiQuality.Logic.FilterLogic;
using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CommentUponLibrary.Articles.Articles
{
    /*  Gathering of Cnn articles and their contents*/
    public class CnnArticle : Publisher
    {
        public async Task<List<HtmlRecord>> ComputeCnnLinks(List<GoogleResult> filteredCnnLinks)
        {
            var cnnBaseLinks = new List<string>();
        
            var recordsCnn = new List<HtmlRecord>();

            var recordCnn = new HtmlRecord();

            var filterLogic = new Filter();

            foreach (var element in filteredCnnLinks)
            {
              
                if (!(new[] { "blogs.cnn", "lite", "cnnpressroom", "2020", "2017" }.Any(x => element.Link.Contains(x))))
                {

                    var conn = await GetHtmlDoc(element.Link);

                    cnnBaseLinks.AddRange(GatherHomepageLinks(conn));
                }
            }
            var cnnDistinctLinks = cnnBaseLinks.Distinct();

            foreach (var link in cnnDistinctLinks)
            {

                var list = recordsCnn.Select(i => i.Link).ToList();
                var duplicate = filterLogic.CheckForDuplicates(list, link);

                if (!link.ToLower().Contains("gallery") || !(link.Contains("live-news")))
                {
                    var conn = await GetHtmlDoc(link);
                    var link1 = link;


                    recordCnn.Link = link;
                    recordCnn = RetrieveCnnArticle(conn); 



                    recordCnn.Date = filterLogic.ExecuteRegex(@"\d{4}/\d{2}/\d{2}", link);

                    IFormatProvider culture = new CultureInfo("en-US", true);

                    bool result = recordCnn.Date.Any(x => !char.IsLetter(x));

                    if (result)
                    {

                        DateTime dateVal = DateTime.ParseExact(recordCnn.Date, "yyyy/MM/dd", culture);

                        dateVal = dateVal.Date;

                        if ((dateVal.Date - DateTime.Now).TotalDays < 6)
                        {
                            if (result)
                       
                            {                            
                                recordCnn.Author = filterLogic.ExecuteRegex(@"=""[A-Za-z]+\s[A-Za-z]+", recordCnn.Author);
                                recordCnn.Author = Regex.Replace(recordCnn.Author, @"[^0-9a-zA-Z]+", "");

                                if (recordCnn.Author != null)
                                {
                                    var listAuthor = Regex.Split(recordCnn.Author, @"(?<!^)(?=[A-Z])").ToList();
                                    recordCnn.Author = String.Join(", ", listAuthor.ToArray());
                                }
                            }
                        }
                        recordCnn.Link = link;

                        recordsCnn.Add(recordCnn);

                    }
                }
            }             
            return recordsCnn;                   
    }

     

        public  List<string> GatherHomepageLinks(HtmlAgilityPack.HtmlDocument doc)
        {

            List<string> linkList = new List<string>();

            var filter = new Filter();
          
            var fullDate = DateTime.Today;

            var cropFirstHalf = fullDate.ToString().Split('{');
            var cropSecondHalf = cropFirstHalf[0].Split(' ');

            var date = cropSecondHalf[0].ToString();

            var dateSplit = date.Split('/');

            var attributesList = doc.DocumentNode.Descendants("a").Select(s => s.Attributes)
            .Select(ss => ss).Select(sss => sss).ToList();

            foreach (var attribute in attributesList)
            {
                var value = attribute.Select(s => s.Value).ElementAt(0);

                if (value.Contains("index.html"))
                {
                    var result = filter.CheckIfLinkExists(linkList, value);
                                     

                    if (result)
                    {
                        var link = filter.AppendLink("http://www.cnn.com", value);
                        
                        var resultCount = filter.CheckForProperPrefixSuffix(link);

                        if (resultCount)
                            linkList.Add(link);

                    }
                }

            }

      
            return linkList;
        }

        public HtmlRecord RetrieveCnnArticle (HtmlAgilityPack.HtmlDocument doc)
        {
            var record = new HtmlRecord();
                var filter = new Filter();   

                record.Header = doc.DocumentNode.Descendants("title").FirstOrDefault().InnerText;
                record.Description = doc.DocumentNode.Descendants("class").Where(i => i.OuterHtml.Contains("og:description")).Select(i => i.InnerText).FirstOrDefault();

                record.Description = doc.DocumentNode.Descendants("meta").Where(i => i.OuterHtml.Contains("og:description")).Select(i => i.InnerText).FirstOrDefault();

                record.Description = doc.DocumentNode.Descendants("div").Where(i => i.OuterHtml.Contains("description")).Select(j => j.InnerText).FirstOrDefault();

                var article = new List<string>();

                var article2 = new List<string>();

                if (doc.DocumentNode.SelectSingleNode("//div[@class='zn-body__paragraph speakable']") != null)
                {
                    article = doc.DocumentNode.SelectNodes("//div[@class='zn-body__paragraph speakable']").Select(i => i.InnerText).ToList();

                    article2 = doc.DocumentNode.SelectNodes("//div[@class='zn-body__paragraph']").Select(i => i.InnerText).ToList();
                }
                else
                {
                    var articleRemanants = doc.DocumentNode.SelectNodes("//div[@class='Chrome__content']")
    .Where(x => x.OuterHtml.Contains("Paragraph__component")).ToList();
                    article = articleRemanants.Select(x => x.Descendants("div").Where(xx => xx.OuterHtml.Contains("Paragraph__component"))).Select(x => x).FirstOrDefault().Select(xx => xx.InnerText).ToList();


                    article = article.Select(x => x).Where(x => x.Count() < 300).ToList();

                }
                    record.Article = record.Description + filter.ConcantenateList(article) + filter.ConcantenateList(article2);


                record.Author = doc.DocumentNode.Descendants("meta").Where(i => i.Attributes.Contains("content")).Select(j => j.OuterHtml).Where(k => k.Contains("author")).FirstOrDefault();

            return record;


                }

    }
}
