using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace Webcrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlSite();
        }

        static void CrawlSite()
        {
            string mainSite = "http://www.espn.com/nba/statistics";

            //We first crawl the site
            HtmlWeb site = new HtmlWeb();
            HtmlDocument htmlDocument = site.Load(@mainSite);

            //Now we find all the components of an specific form
            HtmlNodeCollection leaderBoards_top = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer']");
            HtmlNodeCollection leaderBoards_bottom = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer mod-no-header']");
        }

    }
}
