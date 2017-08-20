using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Webcrawler.CustomObjects;

using HtmlAgilityPack;

using System.IO;
using System.Configuration;

namespace Webcrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlSite();

            Console.ReadLine();
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

            Console.WriteLine("1. Crawling website: " + mainSite);
            
            //We first crawl the site
            HtmlWeb site = new HtmlWeb();
            HtmlDocument htmlDocument = site.Load(@mainSite);

            //Now we find all the components of an specific form
            HtmlNodeCollection leaderBoards_01 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer']"); //We need only the first two.
            HtmlNodeCollection leaderBoards_02 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer mod-no-header']"); //We will use all of them

            Console.WriteLine("Crawled!\n");
            Console.WriteLine("2. Constructing objects.");

            //We get the following:

            //1. Offensive leaders and defensive leaders.
            listOfPlayers_offensiveLeaders = createList_01(leaderBoards_01[0]);
            listOfPlayers_defensiveLeaders = createList_01(leaderBoards_01[1]);

            //2. Assists, Field goal, blocks, steals
            listOfPlayers_assists = createList_01(leaderBoards_02[0]);
            listOfPlayers_fieldGoal = createList_01(leaderBoards_02[1]);
            listOfPlayers_blocks = createList_01(leaderBoards_02[2]);
            listOfPlayers_steals = createList_01(leaderBoards_02[03]);

            //Now we save to file.

            Console.WriteLine("Constructed!\n");
            Console.WriteLine("3.Saving to files.");

            if( SaveToFile(listOfPlayers_offensiveLeaders,"Offensive Leaders") &&
                SaveToFile(listOfPlayers_defensiveLeaders, "Defensive Leaders") &&
                SaveToFile(listOfPlayers_assists, "Assists") &&
                SaveToFile(listOfPlayers_blocks, "Blocks") &&
                SaveToFile(listOfPlayers_fieldGoal, "Field goal") &&
                SaveToFile(listOfPlayers_steals, "Steals")
              )
            {
                Console.WriteLine("Saved! Please check " + ConfigurationSettings.AppSettings["File.location"]);
            }else
            {
                Console.WriteLine("There was an error saving to the file. Please check and try again.");
            }
        }

        public static List<Player> createList_01(HtmlNode node)
        {
            List<Player> result = new List<Player>();

            HtmlNode node_div = node.SelectNodes("div[@class='mod-content']").FirstOrDefault();
            HtmlNode node_table = node_div.SelectNodes("table").FirstOrDefault();

            for (int i = 1; i < node_table.SelectNodes("tr").Count() - 1; i++) //We skip the first and last row
            {
                HtmlNode row = node_table.SelectNodes("tr")[i];

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

                result.Add(player);
            }

            return result;
        }


        public static bool SaveToFile(List<Player> listOfPlayers, string title)
        {

            string fileName = ConfigurationSettings.AppSettings["File.location"];

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true)) //we use 'using' because it automatically flushes and closes the stream; also calls the IDisposable.Dispose of the stream object.
                {
                    file.WriteLine("---------------------");
                    file.WriteLine(title);
                    file.WriteLine("");

                    for (int i = 0; i < listOfPlayers.Count; i++)
                    {
                        int counter = i + 1;
                        file.WriteLine(counter + ", " + listOfPlayers[i].name + ", " + listOfPlayers[i].points + ", " + listOfPlayers[i].link);
                    }

                    file.WriteLine("---------------------");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
            
        }
    }
}
