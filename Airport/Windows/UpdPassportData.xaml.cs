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
    /// Логика взаимодействия для UpdPassportData.xaml
    /// </summary>
    public partial class UpdPassportData : Window
    {
        Employees User;
        public UpdPassportData(Employees User)
        {
            InitializeComponent();
            this.User = User;
            tbSeria.Text = Convert.ToString(User.Passport_deta.series);
            tbNomer.Text = Convert.ToString(User.Passport_deta.nomer);
            dpDateIssue.Text = Convert.ToString(User.Passport_deta.date_issue);
            tbDivisionCode.Text = User.Passport_deta.division_code;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(tbSeria.Text == "")
            {
                User.Passport_deta.series = null;
            }
            else
            {
                User.Passport_deta.series = Convert.ToInt32(tbSeria.Text);
            }
            if (tbNomer.Text == "")
            {
                User.Passport_deta.nomer = null;
            }
            else
            {
                User.Passport_deta.nomer = Convert.ToInt32(tbSeria.Text);
            }
            if (dpDateIssue.Text == "")
            {
                User.Passport_deta.date_issue = null;
            }
            else
            {
                User.Passport_deta.date_issue = Convert.ToDateTime(dpDateIssue.SelectedDate);
            }
            if (tbDivisionCode.Text == "")
            {
                User.Passport_deta.division_code = null;
            }
            else
            {
                User.Passport_deta.division_code = tbDivisionCode.Text;
            }
            Base.BE.SaveChanges();
            this.Close();
        }

        private void tbSeriaAndNomer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void tbDivisionCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == "-")))
            {
                e.Handled = true;
            }
        }
    }
}
