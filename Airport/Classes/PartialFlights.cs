using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public partial class Flights
    {
        public string listFlights
        {
            get
            {
                Routes routes = Base.BE.Routes.FirstOrDefault(x => x.id_route == id_route);
                Citys city_departure_point = Base.BE.Citys.FirstOrDefault(x => x.id_city == routes.id_departure_point);
                Citys city_arrival_point = Base.BE.Citys.FirstOrDefault(x => x.id_city == routes.id_arrival_point);
                return departure_date.ToString("dd.MM.yyyy") + " " + departure_time.ToString("hh\\:mm") + " " + city_departure_point.city + " - " + city_arrival_point.city;
            }
        }
    }
}
