using EmployeesManagementApp.Models.Wrappers;
using EmployeesManagementApp.ViewModels;
using MahApps.Metro.Controls;

namespace EmployeesManagementApp.Views
{
    /// <summary>
    /// Interaction logic for AddEditEmployeeView.xaml
    /// </summary>
    public partial class AddEditEmployeeView : MetroWindow
    {
        public AddEditEmployeeView(EmployeeWrapper employee = null)
        {
            InitializeComponent();
            DataContext = new AddEditEmployeeViewModel(employee);
        }
    }
}
