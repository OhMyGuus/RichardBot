using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Discord.Api
{

    public class Weather
    {
        public Liveweer[] liveweer { get; set; }
        public Liveweer GetLiveWeer()
        {
            return liveweer.FirstOrDefault();
        }
    }

    public class Liveweer
    {
        public string plaats { get; set; }
        public string temp { get; set; }
        public string gtemp { get; set; }
        public string samenv { get; set; }
        public string lv { get; set; }
        public string windr { get; set; }
        public string windms { get; set; }
        public string winds { get; set; }
        public string windk { get; set; }
        public string windkmh { get; set; }
        public string luchtd { get; set; }
        public string ldmmhg { get; set; }
        public string dauwp { get; set; }
        public string zicht { get; set; }
        public string verw { get; set; }
        public string sup { get; set; }
        public string sunder { get; set; }
        public string image { get; set; }
        public string d0weer { get; set; }
        public string d0tmax { get; set; }
        public string d0tmin { get; set; }
        public string d0windk { get; set; }
        public string d0windknp { get; set; }
        public string d0windms { get; set; }
        public string d0windkmh { get; set; }
        public string d0windr { get; set; }
        public string d0neerslag { get; set; }
        public string d0zon { get; set; }
        public string d1weer { get; set; }
        public string d1tmax { get; set; }
        public string d1tmin { get; set; }
        public string d1windk { get; set; }
        public string d1windknp { get; set; }
        public string d1windms { get; set; }
        public string d1windkmh { get; set; }
        public string d1windr { get; set; }
        public string d1neerslag { get; set; }
        public string d1zon { get; set; }
        public string d2weer { get; set; }
        public string d2tmax { get; set; }
        public string d2tmin { get; set; }
        public string d2windk { get; set; }
        public string d2windknp { get; set; }
        public string d2windms { get; set; }
        public string d2windkmh { get; set; }
        public string d2windr { get; set; }
        public string d2neerslag { get; set; }
        public string d2zon { get; set; }
        public string alarm { get; set; }
    }

}
