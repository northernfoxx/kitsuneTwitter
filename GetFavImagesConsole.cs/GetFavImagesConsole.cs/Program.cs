using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CoreTweet;

namespace GetFavImagesConsole.cs
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0) {
                Console.Write("no args");
                Console.ReadKey();
                return;
            }

            if (args[0] == "get" && args[1] == "all")
            {
                Console.WriteLine("start Downloading");
                downloadMedias();
            }

            if (args.Length > 0) { SystemSetting.setting(args); }


            Console.Write("oh euro!!");
            Console.ReadKey();
        }

        static void downloadMedias()
        {
            string myLibPath = @"histry.txt";
            Status[] downliadList = getMediaTweetStatuses();
            long[] mylib = loadDownloadedIds(myLibPath);

            if (mylib == null) {
                long[] tmp = { 0 };
                mylib = tmp;
            }

            UserSettings us = UserSettings.loadSetting();
            if( us.saveDirectry == "") { us.saveDirectry = @"img\"; } 
            if( System.IO.Directory.Exists (us.saveDirectry) == false) { Directory.CreateDirectory(us.saveDirectry); }

            System.Net.WebClient wc = new System.Net.WebClient();
            StreamWriter sw = new StreamWriter(path: myLibPath, append: true);

            foreach (Status status in downliadList)
            {
                if (Array.IndexOf(mylib, status.Id) > -1 ){ continue; }
                string userId = status.User.ScreenName;
                string userName = status.User.Name;
                string tweetId = status.ToString();
                int imgCount = -1;
                foreach (MediaEntity entity in status.ExtendedEntities.Media)
                {
                    imgCount++;
                    System.Console.WriteLine(tweetId + " " + entity.MediaUrl);
                    var extension = System.IO.Path.GetExtension(entity.MediaUrl);
                    string fileName = us.saveDirectry + userId + "_" + tweetId;
                    if (imgCount > 1) fileName += "_" + imgCount.ToString("#");
                    fileName += extension;
                    wc.DownloadFile(entity.MediaUrl, fileName);
                    wc.Dispose();
                }
                sw.WriteLine(DateTime.Now.ToString() + "," + status.Id.ToString());
            }
            sw.Close();
        }

        static Status[] getMediaTweetStatuses()
        {
            UserSettings us = new UserSettings();
            us = UserSettings.loadSetting();

            CoreTweet.Tokens tokens = CoreTweet.Tokens.Create(Variables.consumerKey, Variables.consumerSecret, us.accessToken, us.accessTokenSecret);
            CoreTweet.Core.ListedResponse<Status> list = tokens.Favorites.List(count: 500);
            List<Status> favMediaTweets = new List<Status>();
            foreach (Status stts in list)
            {
                if (stts.Entities.Media != null)
                {
                    favMediaTweets.Add(stts);
                }
            }
            return favMediaTweets.ToArray();
        }

        static long[] loadDownloadedIds(string filename)
        {
            if (File.Exists(filename) == false) { return null; }
            StreamReader sr = new StreamReader(filename);

            List<long> idList = new List<long>();

            do
            {
                try
                {
                    string str = sr.ReadLine();
                    long id = long.Parse(str.Split(',')[1]);
                    idList.Add(id);
                }
                catch { }
            } while (!sr.EndOfStream);

            sr.Close();
            return idList.ToArray();
        }
    }

    class TweetList
    {
        DateTime datetime;
        long id;
    }

}
