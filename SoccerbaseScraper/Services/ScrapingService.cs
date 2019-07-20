using ScrapySharp.Extensions;
using ScrapySharp.Network;
using SoccerbaseScraper.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoccerbaseScraper.Services
{
    public class ScrapingService
    {
        private static ScrapingBrowser browser;

        public void Scrape()
        {
            SetSecurityProtocols();
            SetBrowser();
            CreateFile(GetFileName(), GetPlayers());
        }

        private void SetBrowser()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Setting browser");
            Console.ResetColor();
            browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;
        }

        private void CreateFile(string filePath, List<Player> players)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Creating file {0}", filePath);
            Console.ResetColor();

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var player in players)
                {
                    string playerRow = string.Format("{0},{1},{2},{3}", player.FirstName, player.SecondName, player.Position, player.Team);

                    sw.WriteLine(playerRow);
                }
            }
        }

        private string GetFileName()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format("PlayersList_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));
        }

        private void SetSecurityProtocols()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Setting security protocols");
            Console.ResetColor();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;
        }

        private List<Player> GetPlayers()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Scraping players");
            Console.ResetColor();

            List<Player> players = new List<Player>();

            string root = "http://www.soccerbase.com/";

            for (int i = 2; i < 5; i++)
            {
                WebPage page = browser.NavigateToPage(new Uri(string.Format("{0}/tournaments/tournament.sd?comp_id={1}", root, i)));

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

            return players;
        }

        private List<Player> GetPlayers(string link)
        {
            List<Player> players = new List<Player>();

            WebPage PageResult = browser.NavigateToPage(new Uri(link));

            var team = PageResult.Html.CssSelect(".last").First().InnerText.Trim('\r', '\n');

            var playerList = PageResult.Html.CssSelect(".infoList").First();

            foreach (var playerItem in playerList.SelectNodes("li"))
            {
                var names = playerItem.SelectNodes("b/a").First().InnerText.Split(' ');

                Player player = new Player(names, team, playerItem.InnerText.Substring(playerItem.InnerText.IndexOf("(") + 1, 1));
                players.Add(player);
                Console.WriteLine("Acquired {0}", player.ToString());
            }

            return players;
        }

    }
}