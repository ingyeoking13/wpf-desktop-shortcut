using System.Collections.Generic;
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
            shortcutItems = result.shortcutItems;
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
        public (ICollection<ShortcutModel> shortcutItems, Auth auth) Load()
        {
            (ICollection<ShortcutModel> shortcutItems, Auth auth) result = (new List<ShortcutModel>(), new Auth());
            //try
            //{
            //    result = Preferences.Instance.Load();
            //}
            //catch
            //{
            //    result.shortcutItems = Preferences.Instance.LoadV1();
            //}
            //shortcutItems = result.shortcutItems;
            //auth = result.auth;
            return result;
        }

        public void Save(IEnumerable<ShortcutModel> list, Auth auth)
        {

            //Preferences.Instance.Save(list, auth);
            return;
        }
        private ICollection<ShortcutModel> shortcutItems;
        private Auth auth;

        public ICollection<ShortcutModel> ShortcutItems { get { return shortcutItems; } }
        public Auth Auth { get { return auth; } }
    }
}
