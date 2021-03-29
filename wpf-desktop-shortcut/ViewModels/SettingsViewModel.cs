using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_desktop_shortcut.Models;
using wpf_desktop_shortcut.Repositories;
using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private Repository _repo;
        public bool IsModified { get; set; }
        public ObservableCollection<ShortcutModel> Shortcuts { get; set; }
        public ICommand AddShortcutComand { get; }
        public ICommand SaveCommand { get; }
        public ICommand RegistImgCommand { get; }
        public ICommand RegistFileCommand { get; }
        public ICommand DeleteShortcutCommand { get; }
        
        public Array ExecuteTypes
        {
            get { return Enum.GetValues(typeof(ExecuteTypes)); }
        }


        public SettingsViewModel(Repository repo)
        {
            AddShortcutComand = new RelayCommand<object>(OnAddShortCut);
            SaveCommand = new RelayCommand<object>(OnSave);
            RegistImgCommand = new RelayCommand<ShortcutModel>(OnRegistImgCommand);
            RegistFileCommand = new RelayCommand<ShortcutModel>(OnRegistFilePathCommand);
            DeleteShortcutCommand = new RelayCommand<ShortcutModel>(OnDelteShortcutCommand);
            Shortcuts = new ObservableCollection<ShortcutModel>();
            _repo = repo;
            LoadAppdata();
        }


        private void LoadAppdata()
        {
            Shortcuts = new ObservableCollection<ShortcutModel>();
            List<ShortcutModel> list = new List<ShortcutModel>();

            _repo.Load(list);

            foreach (var item in list)
                Shortcuts.Add(item);
        }

        private void OnAddShortCut(object obj)
        {
            Shortcuts.Add(new ShortcutModel());
            IsModified = true;
        }

        private void OnSave(object obj)
        {
            _repo.Save(Shortcuts);
            IsModified = false;
            
            (App.Current.MainWindow.DataContext as MainViewModel)
                .UpdateShortcuts(Shortcuts);

            MessageBox.Show(
                "저장에 성공하였습니다", 
                "저장완료", 
                MessageBoxButtons.OK);
        }

        private void OnRegistImgCommand(ShortcutModel model)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "img file (*.jpg, *.png)|*.jpg;*.png|icon file (*.ico)|*.ico";
            ofd.Title = "아이콘에 등록할 이미지를 선택해주세요.";
            var result = ofd.ShowDialog();

            if (result != DialogResult.OK)
                return;

            string base64Str = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var bitmap = new Bitmap(ofd.FileName))
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    base64Str = Convert.ToBase64String(ms.GetBuffer());
                }
            }

            model.IconBase64 = base64Str;
        }
        
        private void OnRegistFilePathCommand(ShortcutModel model)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "all files (*.*)|*.*";
            ofd.Title = "등록할 파일을 선택해주세요.";

            var result = ofd.ShowDialog();

            if (result != DialogResult.OK)
                return;

            model.FilePath = ofd.FileName;
        }
        private void OnDelteShortcutCommand(ShortcutModel obj)
        {
            Shortcuts.Remove(obj);
            IsModified = true;
        }
    }
}
