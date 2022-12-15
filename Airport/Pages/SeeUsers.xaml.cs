using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для SeeUsers.xaml
    /// </summary>
    public partial class SeeUsers : Page
    {
        Employees User;
        public SeeUsers(Employees User)
        {
            InitializeComponent();
            this.User = User;
            cbGender.ItemsSource = Base.BE.Gender.ToList();
            cbGender.DisplayMemberPath = "gender";
            cbGender.SelectedIndex = 0;
            dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Roles.role != "Администратор");

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new MainMenuPage(User));
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(tbLogin.Text != "")
            {
                if(tbSurname.Text != "")
                {
                    dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.surname == tbSurname.Text && x.login == tbLogin.Text && x.Roles.role != "Администратор");
                }
                else
                {
                    dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.login == tbLogin.Text && x.Roles.role != "Администратор");
                }
            }
            else
            {
                if (tbSurname.Text != "")
                {
                    dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.surname == tbSurname.Text && x.Roles.role != "Администратор");
                }
                else
                {
                    dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Roles.role != "Администратор");
                    return;
                }
            }
            tbInitialState.Visibility = Visibility.Visible;
        }

        private void tbInitialState_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbLogin.Text = "";
            tbSurname.Text = "";
            cbGender.SelectedIndex = 0;
            dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Roles.role != "Администратор");
            tbInitialState.Visibility = Visibility.Collapsed;
        }

        private void btnFiltering_Click(object sender, RoutedEventArgs e)
        {
            dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Gender.gender == cbGender.Text && x.Roles.role != "Администратор");
            tbInitialState.Visibility = Visibility.Visible;
        }

        private void btnSortAsc_Click(object sender, RoutedEventArgs e)
        {
            dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Gender.gender == cbGender.Text && x.Roles.role != "Администратор").OrderBy(x => x.surname);
            tbInitialState.Visibility = Visibility.Visible;
        }

        private void btnSortDesc_Click(object sender, RoutedEventArgs e)
        {
            dgUsers.ItemsSource = Base.BE.Employees.ToList().Where(x => x.Gender.gender == cbGender.Text && x.Roles.role != "Администратор").OrderByDescending(x => x.surname);
            tbInitialState.Visibility = Visibility.Visible;
        }
    }
}
