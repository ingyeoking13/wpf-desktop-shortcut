using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.Models
{
    public class Auth : ViewModelBase
    {
        private string _serverHost;
        public string ServerHost { 
            get { return _serverHost; }  
            set {
                _serverHost = value;
                OnPropertyChanged();
            }
        }
        private string _userName;
        public string UserName { 
            get { return _userName;  }
            set
            {
                _userName = value;
                OnPropertyChanged();
            } 
        }
        private string _likeNmae;
        public string LikeName { 
            get { return _likeNmae; } 
            set
            {
                _likeNmae = value;
                OnPropertyChanged();
            }
        }
        private string _device;
        public string Device {
            get { return _device; } 
            set
            {
                _device = value;
                OnPropertyChanged();
            } 
        }

        public Auth(string serverHost = "", string userName = "로컬사용자", string likeNmae = "", string device = "")
        {
            _serverHost = serverHost;
            _userName = userName;
            _likeNmae = likeNmae;
            _device = device;
        }
    }
}
