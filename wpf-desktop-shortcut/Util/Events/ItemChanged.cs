using PeanutButter.TinyEventAggregator;
using System;
using System.Collections.Generic;
using wpf_desktop_shortcut.Models;

namespace wpf_desktop_shortcut.Util.Events
{
    public class ItemChanged : EventBase<List<ShortcutModel>>
    {
    }
}
