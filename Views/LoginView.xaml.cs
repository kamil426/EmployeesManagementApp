using EmployeesManagementApp.ViewModels;
using MahApps.Metro.Controls;

namespace EmployeesManagementApp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView(bool openLoginWindowFromButton = false)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(openLoginWindowFromButton);
        }
    }
}
