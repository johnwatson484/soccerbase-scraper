using ScrapySharp.Extensions;
using ScrapySharp.Network;
using SoccerbaseScraper.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerbaseScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlayersList.csv");

            List<Player> players = new List<Player>();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;

            string root = "http://www.soccerbase.com/";

            for (int i = 2; i < 5; i++)
            {
                WebPage page = Browser.NavigateToPage(new Uri(string.Format("http://www.soccerbase.com/tournaments/tournament.sd?comp_id={0}", i)));

                var teams = page.Html.CssSelect(".table").First();

                foreach (var row in teams.SelectNodes("tbody/tr"))
                {
                    foreach (var cell in row.SelectNodes("td[2]"))
                    {
                        var link = cell.SelectNodes("a").First().Attributes["href"].Value;

                        players.AddRange(GetPlayers(string.Format("{0}{1}", root, link)));
                    }
                }
            }
            

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var player in players)
                {
                    string playerRow = String.Format("{0},{1},{2},{3}", player.FirstName, player.SecondName, player.Position, player.Team);

                    sw.WriteLine(playerRow);
                }
            }
        }

        static List<Player> GetPlayers(string link)
        {
            List<Classes.Player> players = new List<Classes.Player>();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;

            WebPage PageResult = Browser.NavigateToPage(new Uri(link));

            var team = PageResult.Html.CssSelect(".last").First().InnerText.Trim('\r', '\n');

            var playerList = PageResult.Html.CssSelect(".infoList").First();

            foreach (var item in playerList.SelectNodes("li"))
            {
                var names = item.SelectNodes("b/a").First().InnerText.Split(' ');

                Player player = new Player();
                player.FirstName = names[0];

                if (names.Length > 1)
                {
                    player.SecondName = names[1];
                }
                if (names.Length > 2)
                {
                    player.SecondName = string.Format("{0} {1}", player.SecondName, names[2]);
                }

                string position = item.InnerText.Substring(item.InnerText.IndexOf("(") + 1, 1);

                switch (position)
                {
                    case "G":
                        player.Position = "GK";
                        break;
                    case "D":
                        player.Position = "DEF";
                        break;
                    case "M":
                        player.Position = "MID";
                        break;
                    case "F":
                        player.Position = "FWD";
                        break;
                    default:
                        player.Position = position;
                        break;
                }

                player.Team = team;

                players.Add(player);
            }

            return players;
        }

    }
}
