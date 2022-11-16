using Calcolatrice;
using Weather;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace Calcolatrice
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        ObservableCollection<List> WeathersWS = new ObservableCollection<List>();
        ObservableCollection<City> CityWS= new ObservableCollection<City>();

        public MainPage()
        {
            this.InitializeComponent();

            if (isInternetConnected())
            {
                DataServiceAsync("https://api.openweathermap.org/data/2.5/forecast?lat=44.34&lon=10.99&appid=d41c51bd00b41f49ec175f3582337b6c");
                ListViewWeatherWS.DataContext = WeathersWS;
                CityName.DataContext = CityWS;
            }

        }

        private void ListViewPersone_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selItem = e.ClickedItem;
            Frame p = Window.Current.Content as Frame;
            //p.Navigate(typeof(DetailPage), selItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Console.WriteLine("MainPage: Appare per la prima volta questa schermata");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Console.WriteLine("User clicked on list");
        }

        private async void DataServiceAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            string dato = await response.Content.ReadAsStringAsync();

            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(dato)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Rootobject));
                Rootobject WeatherWSObj = serializer.ReadObject(ms) as Rootobject;
                List[] WeatherList = WeatherWSObj.list;
                foreach (List item in WeatherList)
                {
                    WeathersWS.Add(item);
                } 
                //WeathersWS.Add(WeatherWSObj);
                CityWS.Add(WeatherWSObj.city);
            }
        }

        public static bool isInternetConnected()
        {
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (InternetConnectionProfile == null) ? false : InternetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }
        public void onClick(Object Sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DayInfo));
        }

        public class Rootobject
        {
            public string cod { get; set; }
            public int message { get; set; }
            public int cnt { get; set; }
            public List[] list { get; set; }
            public City city { get; set; }
        }

        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
            public int population { get; set; }
            public int timezone { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Coord
        {
            public float lat { get; set; }
            public float lon { get; set; }
        }

        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public Weather[] weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public int visibility { get; set; }
            public float pop { get; set; }
            public Rain rain { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
        }

        public class Main
        {
            public float temp { get; set; }
            public float feels_like { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
            public int pressure { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
            public int humidity { get; set; }
            public float temp_kf { get; set; }

            public double tempC { get { return Math.Round(temp - 273.15, 1); } }
            public double tempMinC { get { return Math.Round(temp_min - 273.15, 1); } }


        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Wind
        {
            public float speed { get; set; }
            public int deg { get; set; }
            public float gust { get; set; }
        }

        public class Rain
        {
            public float _3h { get; set; }
        }

        public class Sys
        {
            public string pod { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }

            public string weatherIcon { get { return "Assets/" + main + ".png"; } }

        }

        private void ListViewWeatherWS_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
