using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFavImagesConsole.cs
{
    public static class SystemSetting
    {
        public static void setting(string[] args)
        {
            if (args.Length < 2) { Console.WriteLine("invalid argument(s)"); return; }

            if (args[0] == "config") { configure(args); }
        }

        private static void configure( string[] args)
        {
            if (args[1] == "oauth")
            {
                var session = CoreTweet.OAuth.Authorize(Variables.consumerKey, Variables.consumerSecret);
                var url = session.AuthorizeUri;
                Console.WriteLine(url.ToString());
                System.Diagnostics.Process.Start(url.ToString());
                Console.Write("Enter the PIN:");
                string pin = Console.ReadLine();

                try
                {
                    var tokens = CoreTweet.OAuth.GetTokens(session, pin);

                    Console.WriteLine("accessToken:" + tokens.AccessToken.ToString());
                    Console.WriteLine("accessTokenSecret:" + tokens.AccessTokenSecret.ToString());

                    UserSettings us = new UserSettings();
                    us = UserSettings.loadSetting();
                    us.accessToken = tokens.AccessToken.ToString();
                    us.accessTokenSecret = tokens.AccessTokenSecret.ToString();
                    UserSettings.saveSetting(us);

                    Console.WriteLine("Authorization has been completed");
                }
                catch { Console.WriteLine("Error"); }

                // tokens.Statuses.Update(new Dictionary<string, object>() { { "status", "わたしはえっち画像を保存しました。  " + DateTime.UtcNow.ToString() } });
            }
        }
    }



   static class CommandList
    {
        const string set =
            "  set directry --- set directry to store images and videos";

        const string config =
            "  config oauth --- configure OAuth setting";
    }
}
