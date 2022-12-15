using Airport.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
using static System.Net.WebRequestMethods;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для ListOfTickets.xaml
    /// </summary>
    public partial class ListOfTickets : Page
    {
        PageChange pc = new PageChange();
        List<Box_Offic> Box_OfficFilter = new List<Box_Offic>();
        Employees User;
        public ListOfTickets(Employees User)
        {
            InitializeComponent();
            this.User = User;
            lvListTickets.ItemsSource = Base.BE.Box_Offic.ToList();

            List<Box_Offic> BT = Base.BE.Box_Offic.ToList();

            List<Employees> employees = new List<Employees>(); // Список пользователей, которые совершали продажи

            cmbEmployee.Items.Add("Все кассиры");
            for (int i = 0; i < BT.Count; i++)
            {
                employees.Add(BT[i].Employees);
            }
            employees = employees.Distinct().ToList();
            foreach(Employees employee in employees)
            {
                cmbEmployee.Items.Add(employee.FIO);
            }
            cmbEmployee.SelectedIndex = 0;
            cbNameField.SelectedIndex = 0;
            cbSortingDirection.SelectedIndex = 0;

            Box_OfficFilter = Base.BE.Box_Offic.ToList();
            pc.CountPage = Base.BE.Box_Offic.ToList().Count;
            pc.VisibleButton[0] = "Hidden";
            pc.VisibleButton[1] = "Hidden";
            DataContext = pc;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new MainMenuPage(User));
        }

        private void tbDiscounts_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            List<ApplicationOfDiscounts> AOD = Base.BE.ApplicationOfDiscounts.Where(x => x.id_ticket == index).ToList();
            string str = "";
            foreach (ApplicationOfDiscounts aod in AOD)
            {
                str += "\n" + aod.Discounts.description + " - " + aod.Discounts.value + " %";
            }
            if(str.Length > 0)
            {
                tb.Text = "Применённые скидки: " + str;
            }
        }

        private void tbCost_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            double summDiscounts = 0; // Сумма всех скидок
            List<ApplicationOfDiscounts> AOD = Base.BE.ApplicationOfDiscounts.Where(x => x.id_ticket == index).ToList();
            foreach (ApplicationOfDiscounts aod in AOD)
            {
                summDiscounts += aod.Discounts.value;
            }
            Box_Offic box_Offic = Base.BE.Box_Offic.FirstOrDefault(x => x.id_ticket == index);
            Flights flights = Base.BE.Flights.FirstOrDefault(x => x.id_flight == box_Offic.id_flight);
            double cost = flights.cost - (flights.cost / 100 * summDiscounts);
            tb.Text = cost + " руб.";
        }

        private void tbDeparturePoint_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            Citys city = Base.BE.Citys.FirstOrDefault(x => x.id_city == index);
            tb.Text = city.city;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new AddTickets(User));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            Box_Offic ticket = Base.BE.Box_Offic.FirstOrDefault(x => x.id_ticket == index);
            Base.BE.Box_Offic.Remove(ticket);         
            Base.BE.SaveChanges();
            Frameclass.MainFrame.Navigate(new ListOfTickets(User));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) 
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            Box_Offic ticket = Base.BE.Box_Offic.FirstOrDefault(x => x.id_ticket == index);
            Frameclass.MainFrame.Navigate(new AddTickets(ticket, User));
        }

        private int GetCountDiscount(int index) // Подсчет колличества скидок применённых к билету
        {
            List<ApplicationOfDiscounts> applicationOfDiscounts = Base.BE.ApplicationOfDiscounts.Where(x => x.id_ticket == index).ToList();
            return applicationOfDiscounts.Count();
        }

        private int GetIdEmployee(string FIO) // Поиск id по FIO
        {
            List<Employees> employees = Base.BE.Employees.ToList();
            foreach(Employees employee in employees)
            {
                if(employee.FIO == FIO)
                {
                    return employee.id_employee;
                }
            }
            return 0;
        }

        void Filter()  // Метод для одновременной фильтрации, поиска и сортировки
        {
            List<Box_Offic> box_Offics = new List<Box_Offic>();

            string employee = cmbEmployee.SelectedValue.ToString();
            int index = GetIdEmployee(employee);

            // поиск значений, удовлетворяющих условия фильтра
            if (index != 0)
            {
                box_Offics = Base.BE.Box_Offic.Where(x => x.Employees.id_employee == index).ToList();
            }
            else  // если выбран пункт "Все кассиры", то сбрасываем фильтрацию
            {
                box_Offics = Base.BE.Box_Offic.ToList();
            }

            // поиск совпадений по ФИО пасажира
            if (!string.IsNullOrWhiteSpace(tbPassenger.Text))  // если строка не пустая и если она не состоит из пробелов
            {
                box_Offics = box_Offics.Where(x => x.Passengers.FIO.ToLower().Contains(tbPassenger.Text.ToLower())).ToList();
            }

            // выбор элементов только со скидками
            if (cbStock.IsChecked == true)
            {
                box_Offics = box_Offics.Where(x => GetCountDiscount(x.id_ticket) != 0).ToList();
            }

            // сортировка
            switch (cbNameField.SelectedIndex)
            {
                case 1:
                    {
                        if(cbSortingDirection.SelectedIndex == 0)
                        {
                            box_Offics.Sort((x, y) => x.Passengers.FIO.CompareTo(y.Passengers.FIO));
                        }
                        else
                        {
                            box_Offics.Sort((x, y) => x.Passengers.FIO.CompareTo(y.Passengers.FIO));
                            box_Offics.Reverse();
                        }
                    }
                    break;
                case 2:
                    {
                        if (cbSortingDirection.SelectedIndex == 0)
                        {
                            box_Offics.Sort((x, y) => x.date_of_sale.CompareTo(y.date_of_sale));
                        }
                        else
                        {
                            box_Offics.Sort((x, y) => x.date_of_sale.CompareTo(y.date_of_sale));
                            box_Offics.Reverse();
                        }
                    }
                    break;
                case 3:
                    {
                        if (cbSortingDirection.SelectedIndex == 0)
                        {
                            box_Offics.Sort((x, y) => x.DepartureDate.CompareTo(y.DepartureDate));
                        }
                        else
                        {
                            box_Offics.Sort((x, y) => x.DepartureDate.CompareTo(y.DepartureDate));
                            box_Offics.Reverse();
                        }
                    }
                    break;
            }

            lvListTickets.ItemsSource = box_Offics;
            Box_OfficFilter = box_Offics;

            string str = txtPageCount.Text; // Меняем пагинацию
            txtPageCount.Text = "";
            txtPageCount.Text = str;

            if (box_Offics.Count == 0)
            {
                MessageBox.Show("В базе данных отсутсвуют записи удовлетворяющие условиям!");
            }
        }

        private void cmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tbPassenger_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbStock_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(txtPageCount.Text);
            }
            catch
            {
                pc.CountPage = Box_OfficFilter.Count;
            }
            pc.Countlist = Box_OfficFilter.Count;
            lvListTickets.ItemsSource = Box_OfficFilter.Skip(0).Take(pc.CountPage).ToList();
            pc.CurrentPage = 1;
        }

        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            lvListTickets.ItemsSource = Box_OfficFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
        }

        private void txtPageCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void txtNextFirst_MouseDown(object sender, MouseButtonEventArgs e) // Переход к первой странице
        {
            pc.CurrentPage = 1;
            lvListTickets.ItemsSource = Box_OfficFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
        }

        private void txtNextLast_MouseDown(object sender, MouseButtonEventArgs e) // Переход к последней странице
        {
            pc.CurrentPage = pc.CountPages;
            lvListTickets.ItemsSource = Box_OfficFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
        }
    }
}
