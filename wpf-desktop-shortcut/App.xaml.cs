using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using wpf_desktop_shortcut.Views;
using wpf_desktop_shortcut.Repositories;
using Winforms = System.Windows.Forms;
using wpf_desktop_shortcut.Models;
using PeanutButter.TinyEventAggregator;

namespace wpf_desktop_shortcut
{
    public partial class App : Application
    {
        Winforms.NotifyIcon noti;
        MainWindow _mainWindow;
        SettingWindow _settingWindow;
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static EventAggregator EA { get; } = new EventAggregator();
        public static IRepository Repo { get; set; } = new LocalRepository();

        public App()
        {

            var domain = AppDomain.CurrentDomain;
            domain.UnhandledException += (s, e) =>
            {
                var exception = e.ExceptionObject as Exception;
                _logger.Error($"Unhandled Exception occured.. {exception.Message}");
                _logger.Error($"{exception.StackTrace}");
                _logger.Fatal($"Is Terminating : {e.IsTerminating}");
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (noti == null)
            {
                SetNoti();
            }
            LoadData();
            ShowMainWindow();
        }


        private void SetNoti()
        {
            noti = new Winforms.NotifyIcon();
            var stream = GetResourceStream(new Uri("pack://application:,,,/wpf-desktop-shortcut;component/Resources/doge.png")).Stream;
            var bitmap = new Bitmap(stream);
            noti.Icon = Icon.FromHandle(bitmap.GetHicon());
            noti.Visible = true;

            noti.MouseDown += Noti_MouseDown;
        }

        private void Noti_MouseDown(object sender, Winforms.MouseEventArgs e)
        {
            if (e.Button == Winforms.MouseButtons.Left)
            {
                ShowMainWindow();
            }
            else if (e.Button == Winforms.MouseButtons.Right)
            {
                ContextMenu menu = this.FindResource("_ContextMenu") as ContextMenu;
                menu.IsOpen = true;
            } 
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void SettingMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_settingWindow == null)
            {
                _settingWindow = new SettingWindow();
                _settingWindow.ShowDialog();
                _settingWindow = null;
            }
        }
        private void ShowMainWindow()
        {
            if (_mainWindow == null)
                _mainWindow = new MainWindow();
            
            _mainWindow.Show();
        }

        public void LoadData()
        {
            var _loadedItem = Repo.Load();
            Auth _auth = _loadedItem.auth;
            if (_auth.UserName != "로컬사용자" && _auth.UserName != "")
            {
                Repo = new RemoteRepository();
                ((RemoteRepository)Repo).UpdateAuth(_auth);
                try
                {
                    Repo.Load();
                } catch (Exception e)
                {
                    MessageBox.Show(e.Message, "실패", MessageBoxButton.OK);
                    Repo = new LocalRepository();
                    Repo.Load();
                    _auth.UserName = "로컬사용자";
                }
            } 
            else
            {
                _auth.UserName = "로컬사용자"; 
            }
        }
    }
}
