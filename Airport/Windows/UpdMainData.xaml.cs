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
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для UpdMainData.xaml
    /// </summary>
    public partial class UpdMainData : Window
    {
        Employees User;
        public UpdMainData(Employees User)
        {
            InitializeComponent();
            this.User = User;
            tbSurname.Text = User.surname;
            tbName.Text = User.name;
            tbPatronomic.Text = User.patronomic;
            cbGender.ItemsSource = Base.BE.Gender.ToList();
            cbGender.SelectedValuePath = "id_gender";
            cbGender.DisplayMemberPath = "gender";
            cbGender.SelectedValue = User.id_gender;
            dpBrithday.Text = User.date_of_birth.ToString();
            tbPhone.Text = User.phone;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == "(") || (e.Text == ")") || (e.Text == "+") || (e.Text == "-")))
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            User.surname = tbSurname.Text;
            User.name = tbName.Text;
            User.patronomic = tbPatronomic.Text;
            User.id_gender = (int)cbGender.SelectedValue;
            User.date_of_birth = Convert.ToDateTime(dpBrithday.SelectedDate);
            User.phone = tbPhone.Text;
            Base.BE.SaveChanges();
            this.Close();
        }
    }
}
