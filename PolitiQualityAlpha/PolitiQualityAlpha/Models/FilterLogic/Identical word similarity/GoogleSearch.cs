using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace PolitiQuality.Logic
{
    public class GoogleSearch 
    {

        /*   Google api request and response method */
        public List<GoogleResult> Search(List<string> searchQueries)
        {

            var results = new List<GoogleResult>();
            foreach (var searchPhrase in searchQueries)
            {
                 var cx = "003444117244519549193:t6c4vtktcj3";
                var apiKey = "AIzaSyCbxU8sqWKCN3A3L0oFyHgF0dNDYyr0Z7g";

                var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + searchPhrase);
                                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();
                dynamic jsonData = JsonConvert.DeserializeObject(responseString);
                                          

                foreach (var item in jsonData.items)
                {
                    var resultGoogle = new GoogleResult();
                    resultGoogle.Link = item.link;

                    results.Add(new GoogleResult
                    {
                        Title = item.title,
                        Link = item.link
                                             

                    });

                              
                }
            }

            return results;

        }


   
    }
}
