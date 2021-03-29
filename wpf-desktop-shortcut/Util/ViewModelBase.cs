using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpf_desktop_shortcut.Util
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string props =null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(props));
        }
    }
}
