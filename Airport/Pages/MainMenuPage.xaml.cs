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
    /// Логика взаимодействия для MainMenuAdminPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        Employees User; // Логин пользователя который вошёл в систему
        public MainMenuPage(Employees User)
        {
            InitializeComponent();
            this.User = User;
            tbRoleUser.Text = tbRoleUser.Text + " " + User.Roles.role;
            tbFIOUser.Text = User.FIO;
            if(User.Roles.role == "Пользователь")
            {
                btnSeeUsers.Visibility = Visibility.Collapsed;
                btnSeeTickets.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSeeUsers.Visibility = Visibility.Visible;
                btnSeeTickets.Visibility = Visibility.Visible;
            }
        }

        private void btnSeeUsers_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new SeeUsers(User));
        }

        private void btnExitMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new MainPage());
        }

        private void btnSeeTickets_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new ListOfTickets(User));
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new PersonalAccountPage(User));
        }
    }
}
