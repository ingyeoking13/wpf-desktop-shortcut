using System.Collections.Generic;
using wpf_desktop_shortcut.Models;
using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.Repositories
{
    public class Repository
    {
        public void Load(ICollection<ShortcutModel> list)
        {
            foreach (var item in Preferences.Instance.Load())
                list.Add(item);
            return;
        }

        public void Save(IEnumerable<ShortcutModel> list)
        {

            Preferences.Instance.Save(list);
            return;
        }
    }
}
