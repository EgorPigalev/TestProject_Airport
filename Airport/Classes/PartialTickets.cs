using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Airport
{
    public partial class Box_Offic
    {

        public string DepartureDate
        {
            get
            {
                return Flights.departure_date.ToString("dd MMMM yyyy");
            }
        }
        public string DepartureTime
        {
            get
            {
                return Flights.departure_time.ToString("hh\\:mm");
            }
        }
        public string DateSale
        {
            get
            {
                return date_of_sale.ToString("dd MMMM yyyy HH:mm");
            }
        }
        public SolidColorBrush TimeColor
        {
            get
            {
                TimeSpan timeMorning = TimeSpan.FromHours(7);
                TimeSpan timeEvening = TimeSpan.FromHours(21);
                if (Flights.departure_time > timeMorning && Flights.departure_time < timeEvening)
                {
                    SolidColorBrush NightFlights = new SolidColorBrush(Color.FromRgb(199, 240, 254));
                    return NightFlights;
                }
                else
                {
                    SolidColorBrush DayFlights = new SolidColorBrush(Color.FromRgb(83, 168, 225));
                    return DayFlights;
                }
            }
        }
    }
}
