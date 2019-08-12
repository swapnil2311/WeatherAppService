using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Controllers
{
    public class WeatherServiceController : Controller
    {
        //
        // GET: /WeatherService/
        public ActionResult Index()
        {
            List<string> CityIDList = new List<string>();
            List<OpenweatherInfo.FileBaseOutputClass> CityWiseData;
            try
            {
               
                // Open the text file using a stream reader.
                //File is saved in Folder Content/ InputFolder/ Folder with name CityIdentifiers_Extract.txt

                using (StreamReader sr = new StreamReader(Server.MapPath("~/Content/InputFolder/CityIdentifiers_Extract.txt")))
                {
                    // Read the stream to a string
                    String line = sr.ReadToEnd();
                    line = Regex.Replace(line, @"\s+", string.Empty);

                    //Used ~ char to split the city id in file
                    CityIDList = line.Split('~').ToList();
                }
                if (CityIDList.Count > 0)
                {
                    //Called Open weather webapi and store Data 
                    CityWiseData = GetWeatherData(CityIDList);
                    if (CityWiseData.Count > 0)
                    {
                        //Create city wise date wise file in  Content/ OutputFolder/ Folder
                        CreateOutputFile(CityWiseData);
                        ViewBag.Message = "File Were Created Successfully in OutputFolder";
                    }
                }
                return View();
            }
            catch(Exception e)
            {
                ViewBag.Message = "Some Error Occur In Service";
                return View();
            }
            finally
            {
                CityIDList = null;
                CityWiseData = null;
            }
        }

        //Call Open Weather api by passing CityID List
        public List<OpenweatherInfo.FileBaseOutputClass> GetWeatherData(List<string> CityIDList)
        {
            List<OpenweatherInfo.FileBaseOutputClass> CityWiseData = new List<OpenweatherInfo.FileBaseOutputClass>();
            OpenweatherInfo.BaseOutputClass objBaseOutputClass;
            using (WebClient client = new WebClient())
            {
                foreach (string CityName in CityIDList)
                {
                    String[] CityID = CityName.Split('=');
                    string WebAPI = WebConfigurationManager.AppSettings["OpenWeatherApi"];
                    string AppID = WebConfigurationManager.AppSettings["OpenWeatherApiAppID"];
                    string url = string.Format(WebAPI + "&appid={1}", CityID[0], AppID);
                    var outputjson = client.DownloadString(url);
                    var result = JsonConvert.DeserializeObject<OpenweatherInfo.BaseOutputClass>(outputjson);
                    objBaseOutputClass = result;
                    CityWiseData.Add(new OpenweatherInfo.FileBaseOutputClass { CityID = Convert.ToInt32(CityID[0]), baseOutputClass = objBaseOutputClass });
                }
            }
            return CityWiseData;
        }

        //Create Files in Output Folder
        public void CreateOutputFile(List<OpenweatherInfo.FileBaseOutputClass> CityWiseData)
        {
            foreach (OpenweatherInfo.FileBaseOutputClass objFileBaseOutputClass in CityWiseData)
            {
                var dir = Server.MapPath("~/Content/OutputFolder");
                string FileName = objFileBaseOutputClass.baseOutputClass.name.Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMyyyy");
                string path = Path.Combine(dir, FileName + ".txt");

                if (!System.IO.File.Exists(path))
                {
                    StreamWriter stwriter = System.IO.File.CreateText(path);
                    stwriter.WriteLine("Weather Forecast for City :-" + objFileBaseOutputClass.baseOutputClass.name);
                    stwriter.WriteLine("Weather Forecast Date :- " + DateTime.Now.ToShortDateString());
                    stwriter.WriteLine("City geo location, longitude :- '" + objFileBaseOutputClass.baseOutputClass.coord.lon + "'");
                    stwriter.WriteLine("City geo location, latitude :- '" + objFileBaseOutputClass.baseOutputClass.coord.lat + "'");
                    stwriter.WriteLine("Weather Forecast for the Day :- " + objFileBaseOutputClass.baseOutputClass.weather.FirstOrDefault().main);
                    stwriter.WriteLine("Weather Forecast for the Day Description :- " + objFileBaseOutputClass.baseOutputClass.weather.FirstOrDefault().description);
                    stwriter.WriteLine("Wind Speed :- " + objFileBaseOutputClass.baseOutputClass.wind.speed + " meter/sec ");
                    stwriter.WriteLine("Wind Direction :- " + objFileBaseOutputClass.baseOutputClass.wind.deg + " °");
                    stwriter.WriteLine("Humidity :- " + objFileBaseOutputClass.baseOutputClass.main.humidity + " % ");
                    stwriter.Close();
                }
            }
            
        }

    }
}