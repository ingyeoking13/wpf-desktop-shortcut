using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using wpf_desktop_shortcut.Models;
using wpf_desktop_shortcut.Repositories;
using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<ShortcutModel> OpenCommand{ get; set; }
        public ObservableCollection<ShortcutModel> Shortcuts { get; set; } 
            = new ObservableCollection<ShortcutModel>();
        public void UpdateShortcuts(IEnumerable<ShortcutModel> list)
        {
            Shortcuts.Clear();
            foreach(var item in list)
                Shortcuts.Add(item);
        }

        private IRepository _repo;

        public MainViewModel(IRepository _repo)
        {
            OpenCommand = new RelayCommand<ShortcutModel>(OnOpen);
            this._repo = _repo;
            LoadData();
        }

        private void LoadData()
        {
            foreach (var item in _repo.ShortcutItems)
                Shortcuts.Add(item);
        }


        private void OnOpen(ShortcutModel model)
        {
            try
            {
                switch (model.ExecuteType)
                {
                    case ExecuteTypes.Chrome:
                        Process.Start("chrome", model.FilePath);
                        break;
                    case ExecuteTypes.IE:
                        Process.Start("iexplore", model.FilePath);
                        break;
                    case ExecuteTypes.EXE:
                        Process.Start(model.FilePath);
                        break;
                    case ExecuteTypes.Edge:
                        Process.Start("msedge", model.FilePath);
                        break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "오류", MessageBoxButton.OK);
            }
        }
    }
}
