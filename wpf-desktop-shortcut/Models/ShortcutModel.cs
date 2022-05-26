using wpf_desktop_shortcut.Util;

namespace wpf_desktop_shortcut.Models
{
    public enum ExecuteTypes
    {
        IE=0,
        Chrome,
        EXE,
        Edge,
    }

    public class ShortcutModel : ViewModelBase
    {
        private string _iconName;
        public string IconName 
        { 
            get{ return _iconName; }
            set{ 
                _iconName = value;
                if (value == "null") _iconName = null;
                OnPropertyChanged(); 
            }
        }
        private string _iconBase64;
        public string IconBase64 {
            get { return _iconBase64; }
            set 
            { 
                _iconBase64 = value;
                if (value == "null") _iconBase64 = null;
                OnPropertyChanged();
            }
        }

        private string _filePath;
        public string FilePath 
        {
            get { return _filePath; } 
            set { 
                _filePath = value; 
                if (value == "null") _filePath = null;
                OnPropertyChanged(); 
            }
            
        }
        private ExecuteTypes _executeType;
        public ExecuteTypes ExecuteType 
        { 
            get { return _executeType;  } 
            set { _executeType = value; OnPropertyChanged(); } 
        }
    }
}
