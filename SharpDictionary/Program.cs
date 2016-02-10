using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using LinqToTwitter;
using System.Configuration;
using System.Timers;

namespace SharpDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Looped!");
                var myTimer = new System.Timers.Timer(60 * 60 * 1000); //one hour in milliseconds
                myTimer.Elapsed += new ElapsedEventHandler(everyHour);
                System.Threading.Thread.Sleep(10000);
            }                
        }

        private static void dateStuff()
        {
            string line = System.IO.File.ReadAllText(@"date.dat");
            if (line.Substring(0, 9) != DateTime.Now.Date.ToString().Substring(0, 9))
            {
                readLines();
                System.IO.File.WriteAllText(@"date.dat", DateTime.Now.Date.ToString(), Encoding.UTF8);
            }
            
        }

        private static void everyHour(object src, ElapsedEventArgs e)
        {
            dateStuff();
        }

        private static void readLines()
        {
            string[] lines = System.IO.File.ReadAllLines(@"englishLanguage.dat");
            makeTwitterCall(lines);
        }

        private static void makeTwitterCall(string[] lines)
        {
            var service = new TwitterService("bOMj58v2TPT4Jr0Pob256D2Jo", "6yU8qWsWnWyhzO0wBs3fm8njpGvA1BGxSapC9kee5134hTrXpp");
            service.AuthenticateWith("4896141748-yoyZLt4wu5RWBZn6uiTP5H3f90jb9Uy2JsWdcK9", "GzvkjWZOvKNaLcF2G0vlsY3nnY8S43YT0o9w8WDt5ewNC");
            TwitterStatus result = service.SendTweet(new SendTweetOptions
            {
                Status = "The word of the day is '" + lines.First() + "'." + Environment.NewLine + "#sharpdictionary #everyword #" + lines.First()
            });
            var newLines = lines.Where(val => val != lines.First()).ToArray();
            System.IO.File.WriteAllText(@"englishLanguage.dat", string.Empty);
            System.IO.File.WriteAllLines(@"englishLanguage.dat", newLines, Encoding.UTF8);
        }

    }
}