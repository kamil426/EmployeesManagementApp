using EmployeesManagementApp.ViewModels;
using MahApps.Metro.Controls;

namespace EmployeesManagementApp.Views
{
    /// <summary>
    /// Interaction logic for ConfigConnectionWithDBView.xaml
    /// </summary>
    public partial class ConfigConnectionWithDBView : MetroWindow
    {
        public ConfigConnectionWithDBView(bool openAppSettingsFromButton = false)
        {
            InitializeComponent();
            DataContext = new ConfigConnectionWithDbViewModel(openAppSettingsFromButton);
        }
    }
}
