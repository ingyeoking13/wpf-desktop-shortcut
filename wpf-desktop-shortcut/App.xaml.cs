using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using wpf_desktop_shortcut.Views;
using Winforms = System.Windows.Forms;

namespace wpf_desktop_shortcut
{
    public partial class App : Application
    {
        Winforms.NotifyIcon noti;
        MainWindow _mainWindow;
        SettingWindow _settingWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (noti == null)
            {
                SetNoti();
            }
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
                _mainWindow.Show();
            }
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
                _mainWindow.Show();
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
    }
}
