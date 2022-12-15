using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для UpdAvatar.xaml
    /// </summary>
    public partial class UpdAvatar : Window
    {
        bool gallery;
        Employees User;
        public UpdAvatar(Employees User)
        {
            this.User = User;
            gallery = false;
            InitializeComponent();
        }

        bool GetComparisonArray(byte[] array1, byte[] array2) // Сравнение двух картинок
        {
            for(int i = 0; i < array1.Length; i++)
            {
                if(array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void btnUpdNewPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.ShowDialog();
                string path = OFD.FileName;
                System.Drawing.Image SDI = System.Drawing.Image.FromFile(path);
                ImageConverter IC = new ImageConverter();
                byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));
                List<EmployeesPhoto> photos = Base.BE.EmployeesPhoto.Where(x => x.id_employee == User.id_employee).ToList(); // Проверка, есть ли в базе уже данное фото у пользователя
                foreach (EmployeesPhoto photo in photos)
                {
                    if(GetComparisonArray(photo.photo_binary, Barray)) // Если такое фото уже есть, то новое не добавляем, а устанавливаем старое
                    {
                        foreach (EmployeesPhoto photo2 in photos)
                        {
                            photo2.actual = false;
                        }
                        photo.actual = true;
                        Base.BE.SaveChanges();
                        MessageBox.Show("Фото добавлено");
                        Frameclass.MainFrame.Navigate(new PersonalAccountPage(User));
                        this.Close();
                        return;
                    }
                }
                EmployeesPhoto employeesPhoto = new EmployeesPhoto(); // Если такого фото ещё не было, то добавляем новое и устанавливаем его
                employeesPhoto.id_employee = User.id_employee;
                employeesPhoto.photo_binary = Barray;
                employeesPhoto.actual = true;
                List<EmployeesPhoto> employeesPhotos = Base.BE.EmployeesPhoto.Where(x => x.id_employee == User.id_employee).ToList();
                foreach (EmployeesPhoto photo in employeesPhotos)
                {
                    photo.actual = false;
                }
                Base.BE.EmployeesPhoto.Add(employeesPhoto);
                Base.BE.SaveChanges();
                MessageBox.Show("Фото добавлено");
                Frameclass.MainFrame.Navigate(new PersonalAccountPage(User));
                this.Close();
            }
            catch
            {
                MessageBox.Show("При добавлении нового фото возникла ошибка!");
            }
        }

        void showImage(byte[] Barray, System.Windows.Controls.Image img)
        {
            BitmapImage BI = new BitmapImage();  // создаем объект для загрузки изображения
            using (MemoryStream m = new MemoryStream(Barray))  // для считывания байтового потока
            {
                BI.BeginInit();  // начинаем считывание
                BI.StreamSource = m;  // задаем источник потока
                BI.CacheOption = BitmapCacheOption.OnLoad;  // переводим изображение
                BI.EndInit();  // заканчиваем считывание
            }
            img.Source = BI;  // показываем картинку на экране (imUser – имя картиник в разметке)
            img.Stretch = Stretch.Uniform;
        }
        int n;
        List<EmployeesPhoto> employeesPhotos;
        private void btnUpdOldPhoto_Click(object sender, RoutedEventArgs e)
        {
            n = 0;
            spMenu.Visibility = Visibility.Collapsed;
            spGallery.Visibility = Visibility.Visible;
            gallery = true;
            employeesPhotos = Base.BE.EmployeesPhoto.Where(x => x.id_employee == User.id_employee).ToList();
            if (employeesPhotos.Count != 0)
            {
                if(employeesPhotos.Count > 1)
                {
                    showImage(employeesPhotos[n].photo_binary, imgGallery);
                    showImage(employeesPhotos[employeesPhotos.Count-1].photo_binary, imgLeft);
                    showImage(employeesPhotos[n + 1].photo_binary, imgRight);
                }
                else
                {
                    imgLeft.Source = null;
                    imgRight.Source = null;
                    btnBackClick.Visibility = Visibility.Collapsed;
                    btnNextClick.Visibility = Visibility.Collapsed;
                    showImage(employeesPhotos[n].photo_binary, imgGallery);
                }
            }
            else
            {
                spMenu.Visibility = Visibility.Visible;
                spGallery.Visibility = Visibility.Collapsed;
                gallery = false;
                MessageBox.Show("В базе данных у данного пользователя отсутствуют картинки");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(gallery)
            {
                spGallery.Visibility = Visibility.Collapsed;
                spMenu.Visibility = Visibility.Visible;
                gallery = false;
            }
            else
            {
                this.Close();
            }
        }

        private void btnDeleteOld_Click(object sender, RoutedEventArgs e)
        {
            Base.BE.EmployeesPhoto.Remove(employeesPhotos[n]);
            Base.BE.SaveChanges();
            btnUpdOldPhoto_Click(sender, e);
        }

        private void btnBackClick_Click(object sender, RoutedEventArgs e)
        {
            MovingPhoto(false);
        }

        private void btnNextClick_Click(object sender, RoutedEventArgs e)
        {
            MovingPhoto(true);
        }
        private void MovingPhoto(bool direction) // direction - направление (false - лево, true - право)
        {
            if(direction)
            {
                if (n == employeesPhotos.Count - 1) // Если сейчас установлена последняя картинка
                {
                    n = 0;
                }
                else
                {
                    n++;
                }
            }
            else
            {
                if(n == 0) // Если сейчас установлена первая картинка
                {
                    n = employeesPhotos.Count - 1;
                }
                else
                {
                    n--;
                }
            }
            if (employeesPhotos.Count != 0)
            {
                if (n != employeesPhotos.Count - 1 && n != 0) // Если картинка не последняя и не первая
                {
                    showImage(employeesPhotos[n].photo_binary, imgGallery);
                    showImage(employeesPhotos[n - 1].photo_binary, imgLeft);
                    showImage(employeesPhotos[n + 1].photo_binary, imgRight);
                }
                else if (n != 0) // Если картинка последняя
                {
                    showImage(employeesPhotos[n].photo_binary, imgGallery);
                    showImage(employeesPhotos[n - 1].photo_binary, imgLeft);
                    showImage(employeesPhotos[0].photo_binary, imgRight);
                }
                else // Если картинка первая
                {
                    showImage(employeesPhotos[n].photo_binary, imgGallery);
                    showImage(employeesPhotos[employeesPhotos.Count - 1].photo_binary, imgLeft);
                    showImage(employeesPhotos[n + 1].photo_binary, imgRight);
                }
            }
        }

        private void btnAddOld_Click(object sender, RoutedEventArgs e)
        {
            foreach(EmployeesPhoto photo in employeesPhotos)
            {
                photo.actual = false;
            }
            employeesPhotos[n].actual = true;
            Base.BE.SaveChanges();
            this.Close();
        }
    }
}
