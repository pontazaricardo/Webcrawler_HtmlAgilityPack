using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Webcrawler.CustomObjects;

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

            List<Player> listOfPlayers_offensiveLeaders = new List<Player>();
            List<Player> listOfPlayers_defensiveLeaders = new List<Player>();
            List<Player> listOfPlayers_assists = new List<Player>();
            List<Player> listOfPlayers_blocks = new List<Player>();
            List<Player> listOfPlayers_fieldGoal = new List<Player>();
            List<Player> listOfPlayers_steals = new List<Player>();

            //We first crawl the site
            HtmlWeb site = new HtmlWeb();
            HtmlDocument htmlDocument = site.Load(@mainSite);

            //Now we find all the components of an specific form
            HtmlNodeCollection leaderBoards_01 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer']"); //We need only the first two.
            HtmlNodeCollection leaderBoards_02 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer mod-no-header']"); //We will use all of them

            //We get the following:
            //1. Offensive leaders
            HtmlNode node_offensiveLeaders = leaderBoards_01[0];
            HtmlNode node_offensiveLeaders_div = node_offensiveLeaders.SelectNodes("div[@class='mod-content']").FirstOrDefault();
            HtmlNode node_offensiveLeaders_table = node_offensiveLeaders_div.SelectNodes("table").FirstOrDefault();

            for (int i = 1; i < node_offensiveLeaders_table.SelectNodes("tr").Count(); i++) //We skip the first row
            {
                HtmlNode row = node_offensiveLeaders_table.SelectNodes("tr")[i];

                HtmlNode cell_name = null;
                HtmlNode cell_points = null;

                if (i == 1)
                {
                    cell_name = row.SelectNodes("td|th")[1];
                    cell_points = row.SelectNodes("td|th")[2];
                }
                else
                {
                    cell_name = row.SelectNodes("td|th")[0];
                    cell_points = row.SelectNodes("td|th")[1];
                }

                HtmlNode cell_name_aTag = cell_name.SelectNodes("a").FirstOrDefault();

                string link = cell_name_aTag.Attributes["href"].Value.ToString();
                string name = cell_name_aTag.InnerText;
                float points = float.Parse(cell_points.InnerText);

                Player player = new Player();
                player.name = name;
                player.link = link;
                player.points = points;

                listOfPlayers_offensiveLeaders.Add(player);

            }



        }

    }
}
