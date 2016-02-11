using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using LinqToTwitter;
using System.Configuration;
using System.Timers;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Services;

namespace SharpDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SharpDictionary - Checking....";
            Console.SetWindowSize(25, 5);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
            // Icons from http://ic8.link/453 and http://ic8.link/437 //
            while (true)
            {
                dateStuff();
                System.Threading.Thread.Sleep(60000); // Wait for an hour
                Console.WriteLine("Checked date " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
            }
        }

        private static void dateStuff()
        {
            string line = System.IO.File.ReadAllText(@"date.dat");
            if (line.Substring(0, 9) != DateTime.Now.Date.ToString().Substring(0, 9))
            {
                readLines();
                System.IO.File.WriteAllText(@"date.dat", DateTime.Now.Date.ToString(), Encoding.UTF8);
                Console.WriteLine("It's a new day!" + Environment.NewLine + "Posted to Twitter.");
            } else
            {
                Console.WriteLine("It's still today!");
            }

        }

        public static Google.Apis.Urlshortener.v1.Data.Url publicUrl = new Google.Apis.Urlshortener.v1.Data.Url();
        public static string response { get; set; }
        private static void googleStuff(string url)
        {
            UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCetMcIe3VOy0skeuJbWSZZ5M-BKyTkUk4",
                ApplicationName = "SharpDictionary",
            });
            var m = new Google.Apis.Urlshortener.v1.Data.Url();
            m.LongUrl = url;
            publicUrl = m;
            response = service.Url.Insert(m).Execute().Id;
        }

        private static void readLines()
        {
            string[] lines = System.IO.File.ReadAllLines(@"englishLanguage.dat");
            makeTwitterCall(lines);
        }

        private static void makeTwitterCall(string[] lines)
        {
            string searchQuery = "http://www.google.com/search?q=" + "define " + lines.First();
            googleStuff(searchQuery);
            var service = new TwitterService("bOMj58v2TPT4Jr0Pob256D2Jo", "6yU8qWsWnWyhzO0wBs3fm8njpGvA1BGxSapC9kee5134hTrXpp");
            service.AuthenticateWith("4896141748-yoyZLt4wu5RWBZn6uiTP5H3f90jb9Uy2JsWdcK9", "GzvkjWZOvKNaLcF2G0vlsY3nnY8S43YT0o9w8WDt5ewNC");
            TwitterStatus result = service.SendTweet(new SendTweetOptions
            {
                Status = "The word of the day is '" + lines.First() + "'." + Environment.NewLine + "#sharpdictionary #everyword #" + lines.First() + " " + "#" + (lines.Length-1).ToString() + "togo" + Environment.NewLine + response
            });
            var newLines = lines.Where(val => val != lines.First()).ToArray();
            System.IO.File.WriteAllText(@"englishLanguage.dat", string.Empty);
            System.IO.File.WriteAllLines(@"englishLanguage.dat", newLines, Encoding.UTF8);
        }

    }
}