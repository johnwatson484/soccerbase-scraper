using ScrapySharp.Extensions;
using ScrapySharp.Network;
using SoccerbaseScraper.Classes;
using SoccerbaseScraper.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoccerbaseScraper
{
    class Program
    {        
        static void Main(string[] args)
        {
            ScrapingService scrapingService = new ScrapingService();
            scrapingService.Scrape();
        }
    }
}
