using MahApps.Metro.Controls;
using WpfMvvmApp.ViewMadels;
using WpfMvvmApp.ViewModels;

namespace WpfMvvmApp
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
