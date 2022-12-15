using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddTickets.xaml
    /// </summary>
    public partial class AddTickets : Page
    {
        Box_Offic ticket;
        bool flagUpdate = false;
        Employees User;
        int CountTickets(int index) // Проверка на вместительность самолёта по этому маршруту
        {
            List<Box_Offic> CountTickets = Base.BE.Box_Offic.Where(x => x.id_flight == index).ToList();
            return CountTickets.Count();
        }
        public void uploadFields()  // метод для заполнения списков
        {
            cbDeparture_point.ItemsSource = Base.BE.Citys.ToList();
            cbDeparture_point.SelectedValuePath = "id_city";
            cbDeparture_point.DisplayMemberPath = "city";

            cbArrival_point.ItemsSource = Base.BE.Citys.ToList();
            cbArrival_point.SelectedValuePath = "id_city";
            cbArrival_point.DisplayMemberPath = "city";

            cbFlight.ItemsSource = getListFlights();
            cbFlight.SelectedValuePath = "id_flight";
            cbFlight.DisplayMemberPath = "listFlights";

            cbPassenger.ItemsSource = Base.BE.Passengers.ToList();
            cbPassenger.SelectedValuePath = "id_passenger";
            cbPassenger.DisplayMemberPath = "FIO";

            cbEmployee.ItemsSource = Base.BE.Employees.ToList();
            cbEmployee.SelectedValuePath = "id_employee";
            cbEmployee.DisplayMemberPath = "FIO";

            lbDiscount.ItemsSource = Base.BE.Discounts.ToList();

            btnClear.ToolTip = "Очистка всех полей";
            btnAdd.ToolTip = "Добавить новый билет в базу";
        }
        
        public AddTickets(Employees User) // конструктор для покупки нового билета
        {
            InitializeComponent();
            uploadFields();
            this.User = User;
            cbEmployee.SelectedValue = User.id_employee; // по умолчанию под каким пользователем вошёл, тот и кассир
        }

        public AddTickets(Box_Offic ticket, Employees User) // конструктор для редактирования данных о купленном билете (с аргументом, который хранит информацию о билете)
        {
            InitializeComponent();
            this.User = User;
            uploadFields();
            flagUpdate = true;
            this.ticket = ticket;
            cbFlight.SelectedValue = ticket.id_flight;
            cbPassenger.SelectedValue = ticket.id_passenger; 
            cbEmployee.SelectedValue = ticket.id_employee;

            List<ApplicationOfDiscounts> ad = Base.BE.ApplicationOfDiscounts.Where(x => x.id_ticket == ticket.id_ticket).ToList();

            foreach (Discounts discount in lbDiscount.Items)
            {
                if (ad.FirstOrDefault(x => x.id_discount == discount.id_discount) != null)
                {
                    lbDiscount.SelectedItems.Add(discount);
                }
            }

            imHeaderTicket.Source = new BitmapImage(new Uri("..\\Resources\\icon_updTicket.png", UriKind.Relative));
            tbHeaderTiket.Text = "Редактирование билета в кассе";
            btnClear.Content = "Отмена";
            btnAdd.Content = "Изменить";
            btnClear.ToolTip = "Возврат всех полей к исходным";
            btnAdd.ToolTip = "Изменить существующий билет";
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e) // кнопка назад
        {
            Frameclass.MainFrame.Navigate(new ListOfTickets(User));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (flagUpdate == true) // если это изменение, то кнопка возвращает все данные к исходным, которые в базе
            {
                Frameclass.MainFrame.Navigate(new AddTickets(ticket, User));
            }
            else // если происходит добавление, то эта кнопка очищает все поля
            {
                uploadFields();
                cbDeparture_point.SelectedItem = null;
                cbArrival_point.SelectedItem = null;
                dpDay.Text = null;
                cbFlight.SelectedItem = null;
                cbPassenger.SelectedItem = null;
                cbEmployee.SelectedItem = null;
                lbDiscount.SelectedItems.Clear();
                imDeparture_point.Visibility = Visibility.Collapsed;
                imArrival_point.Visibility = Visibility.Collapsed;
            }
        }

        private List<Flights> getListFlights() // получение списка рейсов
        {
            List<Flights> listFlight = new List<Flights>();
            List<Flights> flights = Base.BE.Flights.ToList();
            foreach (Flights flight in flights)
            {
                if (flight.Planes.number_of_seats <= CountTickets(flight.id_flight)) // если максимальное количество мест у самолёта превысило количество купленных билетов
                {
                    break;
                }
                listFlight.Add(flight); // если есть свободные места на рейс
            }
            listFlight.GroupBy(x => x.departure_date);
            return listFlight;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateListFlights();
        }

        private void updateListFlights() // метод для поиска рейса по трём параметрам (пункт отправления, пункт прибытия, дата рейса)
        {
            List<Flights> flights = getListFlights();
            if (cbDeparture_point.SelectedItem != null)
            {
                if (cbArrival_point.SelectedItem != null)
                {
                    if (dpDay.SelectedDate != null)
                    {
                        flights = updateListFlightsDeparture(flights);
                        flights = updateListFlightsArrival(flights);
                        flights = updateListFlightsDate(flights);
                    }
                    else
                    {
                        flights = updateListFlightsDeparture(flights);
                        flights = updateListFlightsArrival(flights);
                    }
                }
                else if (dpDay.SelectedDate != null)
                {
                    flights = updateListFlightsDeparture(flights);
                    flights = updateListFlightsDate(flights);
                }
                else
                {
                    flights = updateListFlightsDeparture(flights);
                }
            }
            else
            {
                if (cbArrival_point.SelectedItem != null)
                {
                    if (dpDay.SelectedDate != null)
                    {
                        flights = updateListFlightsArrival(flights);
                        flights = updateListFlightsDate(flights);
                    }
                    else
                    {
                        flights = updateListFlightsArrival(flights);
                    }
                }
                else if (dpDay.SelectedDate != null)
                {
                    flights = updateListFlightsDate(flights);
                }
            }
            cbFlight.ItemsSource = flights; // установка нового списка (отобранный по поиску)
            cbFlight.SelectedValuePath = "id_flight";
            cbFlight.DisplayMemberPath = "listFlights";
        }
        private List<Flights> updateListFlightsDeparture(List<Flights> flights) // метод для редактирования списка по начальному маршруту
        {
            List<Flights> listFlight = new List<Flights>();
            foreach (Flights flight in flights)
            {
                Routes routes = Base.BE.Routes.FirstOrDefault(x => x.id_route == flight.id_route);
                Citys city_departure = Base.BE.Citys.FirstOrDefault(x => x.id_city == routes.id_departure_point);
                if(city_departure.city == ((Airport.Citys)cbDeparture_point.SelectedItem).city)
                {
                    listFlight.Add(flight);
                }
            }
            imDeparture_point.Visibility = Visibility.Visible;
            return listFlight;
        }
        private List<Flights> updateListFlightsArrival(List<Flights> flights) // метод для редактирования списка по конечному маршруту
        {
            List<Flights> listFlight = new List<Flights>();
            foreach (Flights flight in flights)
            {
                Routes routes = Base.BE.Routes.FirstOrDefault(x => x.id_route == flight.id_route);
                Citys city_arrival = Base.BE.Citys.FirstOrDefault(x => x.id_city == routes.id_arrival_point);
                if (city_arrival.city == ((Airport.Citys)cbArrival_point.SelectedItem).city)
                {
                    listFlight.Add(flight);
                }
            }
            imArrival_point.Visibility= Visibility.Visible;
            return listFlight;
        }

        private List<Flights> updateListFlightsDate(List<Flights> flights) // метод для редактирования списка по дате рейса
        {
            List<Flights> listFlight = flights.Where(x => x.departure_date == dpDay.SelectedDate).ToList();
            listFlight.OrderBy(x => x.departure_time);
            return listFlight;
        }

        private void imDeparture_point_MouseDown(object sender, MouseButtonEventArgs e) // метод который выполняет действие при нажатие на крестик (очищает пункт отправления)
        {
            imDeparture_point.Visibility = Visibility.Collapsed;
            cbDeparture_point.SelectedItem = null;
        }

        private void imArrival_point_MouseDown(object sender, MouseButtonEventArgs e) // метод который выполняет действие при нажатие на крестик (очищает пункт прибытия)
        {
            imArrival_point.Visibility = Visibility.Collapsed;
            cbArrival_point.SelectedItem = null;
        }

        private void cbFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetCostTicket();
        }

        private void GetCostTicket() // автоматический подсчёт стоимости билета с применёнными к нему скидками
        {
            if (cbFlight.SelectedItem != null) // если указан рейс
            {
                Flights flight = Base.BE.Flights.FirstOrDefault(x => x.id_flight == ((Airport.Flights)cbFlight.SelectedItem).id_flight);
                double summa = GetSummaDiscounts();
                double price = flight.cost - ((flight.cost / 100) * summa);
                tbPrice.Text = "Стоимость билета: " + price + " руб.";
            }
            else
            {
                tbPrice.Text = "Здесь будет указана стоимость билета";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) // добавление или изменение билета в базе
        {
            try
            {
                if (cbFlight.Text == "")
                {
                    MessageBox.Show("Рейс должен быть выбран!");
                    return;
                }
                if (cbPassenger.Text == "")
                {
                    MessageBox.Show("Покупатель должен быть выбран!");
                    return;
                }
                if (cbEmployee.Text == "")
                {
                    MessageBox.Show("Кассир должен быть выбран!");
                    return;
                }
                if (flagUpdate == false)
                {
                    ticket = new Box_Offic();
                }
                ticket.id_flight = Convert.ToInt32(cbFlight.SelectedValue);
                ticket.id_employee = Convert.ToInt32(cbEmployee.SelectedValue);
                ticket.id_passenger = Convert.ToInt32(cbPassenger.SelectedValue);

                if(flagUpdate == false)
                {
                    ticket.date_of_sale = DateTime.Now;

                }

                if (flagUpdate == false)
                {
                    Base.BE.Box_Offic.Add(ticket);
                }

                List<ApplicationOfDiscounts> applicationOfDiscounts = Base.BE.ApplicationOfDiscounts.Where(x => x.id_ticket == ticket.id_ticket).ToList();

                if (applicationOfDiscounts.Count > 0)
                {
                    foreach (ApplicationOfDiscounts aod in applicationOfDiscounts)
                    {
                        Base.BE.ApplicationOfDiscounts.Remove(aod);
                    }
                }

                foreach (Discounts d in lbDiscount.SelectedItems)
                {
                    ApplicationOfDiscounts AD = new ApplicationOfDiscounts()
                    {
                        id_ticket = ticket.id_ticket,
                        id_discount = d.id_discount
                    };
                    Base.BE.ApplicationOfDiscounts.Add(AD);
                }
                Base.BE.SaveChanges();
                if(flagUpdate == true)
                {
                    MessageBox.Show("Запись успешно изменена");
                }
                else
                {
                    MessageBox.Show("Запись добавлена в базу");
                }
            }
            catch
            {
                if (flagUpdate == true)
                {
                    MessageBox.Show("При изменение данных возникла ошибка");
                }
                else
                {
                    MessageBox.Show("При добавление данных возникла ошибка");
                }
            }
        }

        private double GetSummaDiscounts() // подсчёт суммы установленных скидок
        {
            double summa = 0;
            foreach (Discounts discount in lbDiscount.SelectedItems)
            {
                summa += discount.value;
            }
            return summa;
        }

        private void lbDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Discounts d = new Discounts(); // последний выбранный элемент
            foreach(Discounts discount in lbDiscount.SelectedItems)
            {
                d = discount;
            }
            if(GetSummaDiscounts() > 100) // если сумма скидки превышает 100%, то убираем последнюю выбранную
            {
                MessageBox.Show("Сумма скидок превысила 100%!");
                lbDiscount.SelectedItems.Remove(d);
            }
            GetCostTicket(); // пересчет цены билета
        }
    }
    
}
