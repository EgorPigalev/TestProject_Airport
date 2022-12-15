using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для UpdLoginAndPassword.xaml
    /// </summary>
    public partial class UpdLoginAndPassword : Window
    {
        Employees User;
        public UpdLoginAndPassword(Employees User)
        {
            InitializeComponent();
            this.User = User;
            tbLogin.Text = User.login;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void imVisibleOldPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePasswordOld();
        }

        private void HidePasswordOld()
        {
            imVisibleOldPassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_visible.png", UriKind.Relative));
            tbOldPasswordVisible.Visibility = Visibility.Collapsed;
            pbOldPassword.Visibility = Visibility.Visible;
            pbOldPassword.Focus();
        }


        private void ShowPasswordOld()
        {
            imVisibleOldPassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_not_visible.png", UriKind.Relative));
            tbOldPasswordVisible.Visibility = Visibility.Visible;
            pbOldPassword.Visibility = Visibility.Collapsed;
            tbOldPasswordVisible.Text = pbOldPassword.Password;
        }
        private void imVisibleOldPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPasswordOld();
        }

        private void imVisibleNewPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePasswordNew();
        }

        private void HidePasswordNew()
        {
            imVisibleNewPassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_visible.png", UriKind.Relative));
            tbNewPasswordVisible.Visibility = Visibility.Collapsed;
            pbNewPassword.Visibility = Visibility.Visible;
            pbNewPassword.Focus();
        }


        private void ShowPasswordNew()
        {
            imVisibleNewPassword.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_not_visible.png", UriKind.Relative));
            tbNewPasswordVisible.Visibility = Visibility.Visible;
            pbNewPassword.Visibility = Visibility.Collapsed;
            tbNewPasswordVisible.Text = pbNewPassword.Password;
        }

        private void imVisibleNewPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPasswordNew();
        }

        private void imVisibleNewPasswordRepeated_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePasswordNewRepeated();
        }
        private void HidePasswordNewRepeated()
        {
            imVisibleNewPasswordRepeated.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_visible.png", UriKind.Relative));
            tbNewPasswordRepeatedVisible.Visibility = Visibility.Collapsed;
            pbNewPasswordRepeated.Visibility = Visibility.Visible;
            pbNewPasswordRepeated.Focus();
        }


        private void ShowPasswordNewRepeated()
        {
            imVisibleNewPasswordRepeated.Source = new BitmapImage(new Uri("..\\Resources\\icon_password_not_visible.png", UriKind.Relative));
            tbNewPasswordRepeatedVisible.Visibility = Visibility.Visible;
            pbNewPasswordRepeated.Visibility = Visibility.Collapsed;
            tbNewPasswordRepeatedVisible.Text = pbNewPasswordRepeated.Password;
        }
        private void imVisibleNewPasswordRepeated_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPasswordNewRepeated();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int p = pbOldPassword.Password.GetHashCode();
            Employees employee = Base.BE.Employees.FirstOrDefault(x => x.login == User.login && x.password == p);
            if (employee == null)
            {
                MessageBox.Show("Старый пароль введён не верно!");
            }
            else
            {
                if(pbNewPassword.Password.ToString() != "")
                {
                    Regex regexCapitalLatinCharacter = new Regex("(?=.*[A-Z])"); // Регулярное выражение для проверки наличия 1 заглавного латинского символа
                    if (regexCapitalLatinCharacter.IsMatch(pbNewPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Новый пароль должен содержать не менее 1 заглавного латинского символа");
                        return;
                    }
                    Regex regexAtLeastCharacters = new Regex("(?=.*[a-z].*[a-z].*[a-z])"); // Регулярное выражение для проверки наличия 3 строчных латинских символов
                    if (regexAtLeastCharacters.IsMatch(pbNewPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Новый пароль должен содержать не менее 3 строчных латинских символов");
                        return;
                    }
                    Regex regexAtLeastDigits = new Regex("(?=.*[0-9].*[0-9])"); // Регулярное выражение для проверки наличия 2 цифр
                    if (regexAtLeastDigits.IsMatch(pbNewPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Новый пароль должен содержать не менее 2 цифр");
                        return;
                    }
                    Regex regexSpecialСharacter = new Regex("(?=.*[!@#$&*])"); // Регулярное выражение для проверки наличия 1 спец. символа
                    if (regexSpecialСharacter.IsMatch(pbNewPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Новый пароль должен содержать не менее 1 спец. символа");
                        return;
                    }
                    Regex regexLength = new Regex(".{8,}"); // Регулярное выражение для проверки длины пароля
                    if (regexSpecialСharacter.IsMatch(pbNewPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Общая длина нового пароля должна быть не менее 8 символов");
                        return;
                    }
                    if (pbNewPassword.Password.ToString() != pbNewPasswordRepeated.Password.ToString())
                    {
                        MessageBox.Show("Пароль не подтверждён!");
                        return;
                    }
                    User.password = pbNewPassword.Password.GetHashCode();
                }
                User.login = tbLogin.Text;
                Base.BE.SaveChanges();
                this.Close();
                MessageBox.Show("Данные успешно обнавлены");
            }
        }
    }
}
