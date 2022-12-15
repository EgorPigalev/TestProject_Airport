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
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Lifetime;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            cbGender.ItemsSource = Base.BE.Gender.ToList();
            cbGender.SelectedValuePath = "id_gender";
            cbGender.DisplayMemberPath = "gender";
            cbGender.SelectedIndex = 0;
        }

        private void TBNextRegistration_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new AuthorizationPage());
        }

        bool GetProverkaParol()
        {
            Employees employees = Base.BE.Employees.FirstOrDefault(x => x.login == tbLogin.Text);
            if (employees != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbSurname.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле фамилия должно быть заполнено!");
                    return;
                }
                if (tbName.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле имя должно быть заполнено!");
                    return;
                }
                if (tbPatronomic.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле Отчество должно быть заполнено!");
                    return;
                }
                if (cbGender.Text == "")
                {
                    MessageBox.Show("Поле пол должно быть заполнено!");
                    return;
                }
                if (dpBrithday.Text == "")
                {
                    MessageBox.Show("Поле дата рождения должно быть заполнено!");
                    return;
                }
                if (tbLogin.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле логин должно быть заполнено!");
                    return;
                }
                Regex regexCapitalLatinCharacter = new Regex("(?=.*[A-Z])"); // Регулярное выражение для проверки наличия 1 заглавного латинского символа
                if (regexCapitalLatinCharacter.IsMatch(pbPassword.Password.ToString()) == false)
                {
                    MessageBox.Show("Пароль должен содержать не менее 1 заглавного латинского символа");
                    return;
                }
                Regex regexAtLeastCharacters = new Regex("(?=.*[a-z].*[a-z].*[a-z])"); // Регулярное выражение для проверки наличия 3 строчных латинских символов
                if (regexAtLeastCharacters.IsMatch(pbPassword.Password.ToString()) == false)
                {
                    MessageBox.Show("Пароль должен содержать не менее 3 строчных латинских символов");
                    return;
                }
                Regex regexAtLeastDigits = new Regex("(?=.*[0-9].*[0-9])"); // Регулярное выражение для проверки наличия 2 цифр
                if (regexAtLeastDigits.IsMatch(pbPassword.Password.ToString()) == false)
                {
                    MessageBox.Show("Пароль должен содержать не менее 2 цифр");
                    return;
                }
                Regex regexSpecialСharacter = new Regex("(?=.*[!@#$&*])"); // Регулярное выражение для проверки наличия 1 спец. символа
                if (regexSpecialСharacter.IsMatch(pbPassword.Password.ToString()) == false)
                {
                    MessageBox.Show("Пароль должен содержать не менее 1 спец. символа");
                    return;
                }
                Regex regexLength = new Regex(".{8,}"); // Регулярное выражение для проверки длины пароля
                if (regexSpecialСharacter.IsMatch(pbPassword.Password.ToString()) == false)
                {
                    MessageBox.Show("Общая длина пароля должна быть не менее 8 символов");
                    return;
                }
                if (pbPassword.Password.ToString() != pbPasswordRepeated.Password.ToString())
                {
                    MessageBox.Show("Пароль не подтверждён!");
                    return;
                }
                if (GetProverkaParol() == true)
                {
                    MessageBox.Show("Пользователь с таким логиным уже зарегистрирован!");
                    return;
                }
                Passport_deta passport_Deta = new Passport_deta()
                {

                };
                Base.BE.Passport_deta.Add(passport_Deta);
                Base.BE.SaveChanges();
                Employees employee = new Employees()
                {
                    id_employee = passport_Deta.id_passport_deta,
                    surname = tbSurname.Text,
                    name = tbName.Text,
                    patronomic = tbPatronomic.Text,
                    id_gender = cbGender.SelectedIndex + 1,
                    date_of_birth = Convert.ToDateTime(dpBrithday.SelectedDate),
                    phone = tbPhone.Text,
                    login = tbLogin.Text,
                    password = pbPassword.Password.GetHashCode(),
                    id_role = 2
                };
                Base.BE.Employees.Add(employee);
                Base.BE.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались");
                Frameclass.MainFrame.Navigate(new AuthorizationPage());
            }
            catch
            {
                MessageBox.Show("При регистрации пользователя возникла ошибка!");
            }
        }
        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == "(") || (e.Text == ")") || (e.Text == "+") || (e.Text == "-")))
            {
                e.Handled = true;
            }
        }

        private void imVisiblePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void imVisiblePassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void HidePassword()
        {
            imVisiblePassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_visible.png", UriKind.Relative));
            pbPasswordVisible.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Visible;
            pbPassword.Focus();
        }


        private void ShowPassword()
        {
            imVisiblePassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_not_visible.png", UriKind.Relative));
            pbPasswordVisible.Visibility = Visibility.Visible;
            pbPassword.Visibility = Visibility.Collapsed;
            pbPasswordVisible.Text = pbPassword.Password;
        }

        private void imVisiblePasswordRepeated_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePasswordRepeated();
        }

        private void imVisiblePasswordRepeated_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPasswordRepeated();
        }

        private void HidePasswordRepeated()
        {
            imVisiblePasswordRepeated.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_visible.png", UriKind.Relative));
            pbPasswordRepeatedVisible.Visibility = Visibility.Collapsed;
            pbPasswordRepeated.Visibility = Visibility.Visible;
            pbPasswordRepeated.Focus();
        }


        private void ShowPasswordRepeated()
        {
            imVisiblePasswordRepeated.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_not_visible.png", UriKind.Relative));
            pbPasswordRepeatedVisible.Visibility = Visibility.Visible;
            pbPasswordRepeated.Visibility = Visibility.Collapsed;
            pbPasswordRepeatedVisible.Text = pbPasswordRepeated.Password;
        }
    }
}
