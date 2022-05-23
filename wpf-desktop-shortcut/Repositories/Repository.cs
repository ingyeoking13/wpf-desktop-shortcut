using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using wpf_desktop_shortcut.Models;
using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.Repositories
{
    public interface IRepository
    {
        (ICollection<ShortcutModel> shortcutItems, Auth auth) Load();
        void Save(IEnumerable<ShortcutModel> list, Auth auth);
        ICollection<ShortcutModel> ShortcutItems { get; }
        Auth Auth { get; }
    }

    public class LocalRepository : IRepository
    {
        public (ICollection<ShortcutModel> shortcutItems, Auth auth) Load()
        {
            (ICollection<ShortcutModel> shortcutItems, Auth auth) result = (new List<ShortcutModel>(), new Auth());
            try
            {
                result = Preferences.Instance.Load();
            }
            catch
            {
                result.shortcutItems = Preferences.Instance.LoadV1();
            }
            shortcutItems = result.shortcutItems ?? new List<ShortcutModel>(); 
            auth = result.auth;
            return result;
        }

        public void Save(IEnumerable<ShortcutModel> list, Auth auth)
        {

            Preferences.Instance.Save(list, auth);
            return;
        }

        private ICollection<ShortcutModel> shortcutItems;
        private Auth auth;

        public ICollection<ShortcutModel> ShortcutItems { get { return shortcutItems; } }
        public Auth Auth { get { return auth; } }

    }

    public class RemoteRepository : IRepository
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public (ICollection<ShortcutModel> shortcutItems, Auth auth) Load()
        {
            (ICollection<ShortcutModel> shortcutItems, Auth auth) result = (new List<ShortcutModel>(), new Auth());
            try
            {
                string url = "http://localhost:8000/login?id=yohan";
                HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
                _req.Method = "GET";
                using (HttpWebResponse _res = (HttpWebResponse)_req.GetResponse())
                {
                    HttpStatusCode status = _res.StatusCode;
                    Stream _body = _res.GetResponseStream();
                    JObject jobj = null;
                    using (StreamReader _sr = new StreamReader(_body))
                    {
                        var _str = _sr.ReadToEnd();
                        jobj = JObject.Parse(_str);
                    }

                    if (status == HttpStatusCode.OK)
                    {
                        result.shortcutItems = JsonConvert.DeserializeObject<List<ShortcutModel>>(jobj["PackageJson"].ToString());
                        result.auth.ServerHost = jobj["HostIP"]?.ToString();
                        result.auth.UserName = jobj["UserName"]?.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"{nameof(RemoteRepository)} Load : {e.Message}");
            }
            shortcutItems = result.shortcutItems ?? new List<ShortcutModel>();
            auth = result.auth;
            return result;
        }

        public void Save(IEnumerable<ShortcutModel> list, Auth auth)
        {

            var origin = Preferences.Instance.Load();
            Preferences.Instance.Save(origin.shortcutItems, auth);
            try
            {
                string url = "http://localhost:8000/login?id=yohan";
                HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
                _req.Method = "POST";
            }
            catch (Exception e)
            {
                _logger.Error($"{nameof(RemoteRepository)} POST Save : {e.Message}");
            }
            return;
        }

        private void SaveAuth(Auth auth)
        {
            (ICollection<ShortcutModel> shortcutItems, Auth auth) result = (new List<ShortcutModel>(), new Auth());
            try
            {
                result = Preferences.Instance.Load();
            }
            catch
            {
                result.shortcutItems = Preferences.Instance.LoadV1();
            }
            Preferences.Instance.Save(result.shortcutItems, auth);
            return;
        }

        private ICollection<ShortcutModel> shortcutItems;
        private Auth auth;

        public ICollection<ShortcutModel> ShortcutItems { get { return shortcutItems; } }
        public Auth Auth { get { return auth; } }
        
        public bool UpdateAuth(Auth _auth)
        {
            auth = _auth;
            SaveAuth(auth);
            return true;
        }
    }
}
