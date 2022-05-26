using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using wpf_desktop_shortcut.Models;

namespace wpf_desktop_shortcut.Business.Login
{
    public partial class LoginWindow : Window
    {
        public Auth Auth { get; set; } 
        public LoginWindow(Auth _auth)
        {
            this.Auth = _auth;
            InitializeComponent();
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RequestRemoteInfo(this.HostServer.Text, this.UserName.Text);
            } 
            catch (WebException ex)
            {
                string message = String.Empty;
                if (ex.Response != null)
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        message = reader.ReadToEnd();
                    }
                else message = ex.Message;

                MessageBox.Show(message, "실패", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "실패", MessageBoxButton.OK);
                return;
            }
            this.DialogResult = true;
            MessageBox.Show("성공적으로 로그인 되었습니다", "성공", MessageBoxButton.OK);
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void RequestRemoteInfo(string _serverhost, string _username)
        {
            string url = $"http://{_serverhost}/login?id={_username}";
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
            _req.Method = "GET";
            using (HttpWebResponse _res = (HttpWebResponse)_req.GetResponse())
            {
                HttpStatusCode status = _res.StatusCode;
                Stream _body = _res.GetResponseStream();
                JObject jobj = null;
                using (StreamReader _sr = new StreamReader(_body))
                {
                    var _str = _sr.ReadToEnd();
                    jobj = JObject.Parse(_str);
                }

                if (status == HttpStatusCode.OK)
                {
                    Auth.ServerHost = _serverhost;
                    Auth.UserName = jobj["UserName"]?.ToString();
                    Auth.LikeName = jobj["PackageJson"].ToString();
                }
                else
                {
                    throw new Exception(jobj.ToString());
                }
            }
        }
    }
}
