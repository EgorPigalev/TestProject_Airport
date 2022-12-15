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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        Пользователь для входа под администратором:
        Логин: EgorAdmin
        Пароль: Admin11!
         */
        public MainWindow()
        {
            InitializeComponent();
            Frameclass.MainFrame = fMain;
            Frameclass.MainFrame.Navigate(new MainPage());
            Base.BE = new BaseDataEntities();
        }
    }
}
