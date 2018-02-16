using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFavImagesConsole.cs
{
    public class UserSettings
    {
        public string saveDirectry = "";
        public string accessToken = "";
        public string accessTokenSecret = "";

        static public UserSettings loadSetting()
        {
            string fileName = @"settings.xml";
            if (System.IO.File.Exists(fileName) == true)
            {
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    fileName, new System.Text.UTF8Encoding(false));
                UserSettings us = (UserSettings)serializer.Deserialize(sr);
                sr.Close();
                return us;
            }
            else
            {
                return new UserSettings();
            }

        }

        static public void saveSetting(UserSettings us)
        {
            string fileName = @"settings.xml";
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, us);
            sw.Close();
        }
    }
}
