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

        private Repository _repo;

        public MainViewModel(Repository repo)
        {
            OpenCommand = new RelayCommand<ShortcutModel>(OnOpen);
            _repo = repo;
            
            LoadData();
        }

        private void LoadData()
        {
            List<ShortcutModel> list = new List<ShortcutModel>();
            _repo.Load(list);
            foreach(var item in list)
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
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "오류", MessageBoxButton.OK);
            }
        }
    }
}
