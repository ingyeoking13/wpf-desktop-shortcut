using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using wpf_desktop_shortcut.Models;

namespace wpf_desktop_shortcut.Util
{
    public class Preferences 
    {
        private static Lazy<Preferences> _instance = new Lazy<Preferences>(()=>new Preferences());
        public static Preferences Instance { get { return _instance.Value; } }

        private string DirPath = "BangbaeShortcut";
        private string FilePath = "BangbaeShortcut.json";
        private Preferences()
        {
        }

        /// <summary>
        /// Load JSON data
        /// </summary>
        public (List<ShortcutModel> shortcutItems, Auth auth) Load()
        {
            string filePath = _GetFilePath();
            (List<ShortcutModel> shortcutItems, Auth auth) result = (new List<ShortcutModel>(), new Auth());
            using(FileStream stream = File.Open(filePath, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    var readResult = streamReader.ReadToEnd();
                    JObject jObject = JObject.Parse(readResult);
                    string version = jObject["version"].ToString();
                    result.auth = JsonConvert.DeserializeObject<Auth>(jObject["auth"].ToString());
                    result.shortcutItems = JsonConvert.DeserializeObject<List<ShortcutModel>>(jObject["shortcutItems"].ToString());
                }
            }
            return result;
        }


        public List<ShortcutModel> LoadV1()
        {
            string filePath = _GetFilePath();
            List<ShortcutModel> result = new List<ShortcutModel>();
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    var readResult = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<List<ShortcutModel>>(readResult);
                }
            }
            return result ?? new List<ShortcutModel>();
        }

        /// <summary>
        /// Save JSON
        /// </summary>
        public bool Save(IEnumerable<ShortcutModel> shortcutItems, Auth auth)
        {
            object data = new { auth = auth, shortcutItems = shortcutItems, version = "2" };
            try
            {
                string filePath = _GetFilePath();
                bool fileExist = File.Exists(filePath);
                if (fileExist == true)
                    File.Delete(filePath);

                using (FileStream stream = File.Create(filePath))
                {
                    using (StreamWriter streamWriter = new StreamWriter(stream))
                    {
                        var str = JsonConvert.SerializeObject(data);
                        streamWriter.Write(str);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private string _GetFilePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dirPath = Path.Combine(docFolder, DirPath);

            bool dirExist = Directory.Exists(dirPath);
            if (dirExist == false)
                Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, FilePath);
            bool fileExist = File.Exists(filePath);
            if (fileExist == false)
            {
                var tempStream = File.Create(filePath);
                tempStream.Close();
            }

            return filePath;
        }
    }
}
