using HtmlAgilityPack;
using PolitiQuality.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolitiQuality.Interfaces
{

      /*   Publishers including unspecified if user has unselected the publisher   */
    public abstract class Publisher
    {

            public async Task<HtmlDocument> GetHtmlDoc(string link)
    {

        HtmlWeb web1 = new HtmlAgilityPack.HtmlWeb();
        HtmlDocument conn = await web1.LoadFromWebAsync(link);

        return conn;
    }




}
}
