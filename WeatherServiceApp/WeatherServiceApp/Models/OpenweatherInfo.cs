using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherServiceApp.Models
{
    public class OpenweatherInfo
    {
        //Classes to deserialize the output of the Openweather api

        //coord class 
        public class coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        //Weather Class 
        public class weather
        {
            public int id {get; set;}
            public string main {get; set;}
            public string description { get; set; }

        }

        //Main Class
        public class main
        {
            public double temp {get; set;}
            public double pressure {get; set;}
            public double humidity  {get; set;}
            public double temp_min {get; set;}
            public double temp_max { get; set; }
        }

        //wind class
        public class wind
        {
            public double speed {get; set;}
            public double deg {get; set;}
        }

        //sys class
        public class sys
        {
           public string type {get; set;}
           public int id { get; set;}
           public string  message {get; set;}
           public string country {get; set; }
           public string sunrise {get; set;}
           public string sunset { get; set; } 
        }


        //Base Class for API output
        public class BaseOutputClass
        {
            public string name { get; set; }
            public int id { get; set; }
            public sys sys { get; set; }
            public double dt { get; set; }
            public wind wind { get; set; }
            public main main { get; set; }
            public List<weather> weather { get; set; }
            public coord coord { get; set; }
        }
        public class FileBaseOutputClass
        {
            public int CityID { get; set; }
            public BaseOutputClass baseOutputClass { get; set; }
        }
    }
}