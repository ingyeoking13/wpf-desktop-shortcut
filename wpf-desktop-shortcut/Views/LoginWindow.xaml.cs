﻿using System.Windows;

namespace wpf_desktop_shortcut.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
/*
            if (vm.IsModified)
            {
                var result =
                MessageBox.Show("변경 사항이 있습니다. 저장하지 않고 창을 닫으시겠습니까?", "경고", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
*/
        }
    }
}
