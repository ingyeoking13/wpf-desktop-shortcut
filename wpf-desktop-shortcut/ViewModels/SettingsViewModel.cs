using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using wpf_desktop_shortcut.Business.Login;
using wpf_desktop_shortcut.Models;
using wpf_desktop_shortcut.Repositories;
using wpf_desktop_shortcut.Util;
using wpf_desktop_shortcut.Util.Events;

namespace wpf_desktop_shortcut.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IRepository _repo;
        public bool IsModified { get; set; }
        public CurrentAuth CurrentAuth { get; set; } = new CurrentAuth();
        public ObservableCollection<ShortcutModel> Shortcuts { get; set; }
        public LoginWindow _loginWindow = null;
        public ICommand AddShortcutComand { get; }
        public ICommand SaveCommand { get; }
        public ICommand RegistImgCommand { get; }
        public ICommand RegistFileCommand { get; }
        public ICommand DeleteShortcutCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public Array ExecuteTypes
        {
            get { return Enum.GetValues(typeof(ExecuteTypes)); }
        }

        public SettingsViewModel(IRepository _repo)
        {
            AddShortcutComand = new RelayCommand<object>(OnAddShortCut);
            SaveCommand = new RelayCommand<object>(OnSave);
            RegistImgCommand = new RelayCommand<ShortcutModel>(OnRegistImgCommand);
            RegistFileCommand = new RelayCommand<ShortcutModel>(OnRegistFilePathCommand);
            DeleteShortcutCommand = new RelayCommand<ShortcutModel>(OnDelteShortcutCommand);
            
            LoginCommand = new RelayCommand<object>(OnLoginCommand);
            LogoutCommand = new RelayCommand<object>(OnLogoutCommand);

            Shortcuts = new ObservableCollection<ShortcutModel>();
            this._repo = _repo;
            LoadAppdata();
        }

        private void LoadAppdata()
        {
            Shortcuts = new ObservableCollection<ShortcutModel>();
            foreach (var item in _repo.ShortcutItems)
                Shortcuts.Add(item);
            CurrentAuth.UserName = _repo.Auth.UserName;
        }

        private void OnAddShortCut(object obj)
        {
            Shortcuts.Add(new ShortcutModel());
            IsModified = true;
        }

        private void OnSave(object obj)
        {
            try
            {
                _repo.Save(Shortcuts, _repo.Auth);
                IsModified = false;

                (App.Current.MainWindow.DataContext as MainViewModel)
                    .UpdateShortcuts(Shortcuts);

                MessageBox.Show(
                    "저장에 성공하였습니다",
                    "저장완료",
                    MessageBoxButtons.OK);
            } catch
            {

            }
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

        private void OnLoginCommand(object obj)
        {
            if (_loginWindow != null) return;

            _loginWindow = new LoginWindow(_repo.Auth);
            _loginWindow.ShowDialog();

            if (_loginWindow.DialogResult == true)
            {
                var remoteRepo = new RemoteRepository();
                _repo = remoteRepo;
                remoteRepo.UpdateAuth(_loginWindow.Auth);
                _repo.Load();
                App.Repo = _repo;

                CurrentAuth.UserName = _repo.Auth.UserName;

                Shortcuts.Clear();
                foreach (var item in _repo.ShortcutItems)
                    Shortcuts.Add(item);

                App.EA.GetEvent<ItemChanged>().Publish(new List<ShortcutModel>(App.Repo.ShortcutItems));
            }

            _loginWindow = null;
        }

        private void OnLogoutCommand(object obj)
        {
            DialogResult _result= MessageBox.Show( "로그아웃 하시겠습니까?", "로그아웃", MessageBoxButtons.OKCancel);
            if (_result != DialogResult.OK) return;
            try
            {
                ((RemoteRepository)App.Repo).UpdateAuth(new Auth());
           }
            catch
            {

            }
            finally
            {
                App.Repo = new LocalRepository();
                App.Repo.Load();
                _repo = App.Repo;
                Shortcuts.Clear();
                foreach (var item in App.Repo.ShortcutItems)
                    Shortcuts.Add(item);
                CurrentAuth.UserName = "로컬사용자";
                App.EA.GetEvent<ItemChanged>().Publish(new List<ShortcutModel>(App.Repo.ShortcutItems));
            }
        }
    }

    public class CurrentAuth : ViewModelBase
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value;  OnPropertyChanged();  }
        }
    }
}
