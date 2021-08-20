using SoccerbaseScraper.Services;

namespace SoccerbaseScraper
{
    static class Program
    {        
        static void Main(string[] args)
        {
            ScrapingService scrapingService = new ScrapingService();
            scrapingService.Scrape();
        }
    }
}
