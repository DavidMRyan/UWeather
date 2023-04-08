using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;

// @temp
using System.Threading.Tasks;
using Windows.Storage;

// @debug
// using System.Diagnostics;

namespace UWeather
{
    public sealed partial class MainPage : Page
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        /// <summary>
        /// Entry Point of the Application.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            // @debug -> Remove in production release.
            //AllocConsole();

            Task.Run(async () => { await ApplicationData.Current.ClearAsync(); });

            // Set window size on page load
            ApplicationView.PreferredLaunchViewSize = new Size(600, 900);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            InitializeUI();
            ToggleSwitch CelsiusToggleSwitch = (ToggleSwitch)FindName("CelsiusToggleSwitch");
            CelsiusToggleSwitch.Toggled += CelsiusToggleSwitch_Toggled;
        }

        /// <summary>
        /// Fills all UI component values with correct & relevant information.
        /// </summary>
        private void InitializeUI()
        {
            // Initialize variables to store relevant information
            Geocoordinate deviceCoordinate = DeviceInformation.GetDeviceGeocoordinate();
            var gridInfo = WebHelper.GetNearestGridCoordinatesAsync(deviceCoordinate.Point.Position.Latitude,
                deviceCoordinate.Point.Position.Longitude).Result;

            string endpoint = WebHelper.GetLocalizedAPIEndpoint(gridInfo["office"],
                Convert.ToInt32(gridInfo["gridX"]), Convert.ToInt32(gridInfo["gridY"]), "/forecast");

            // @debug -> Show Current Endpoint
            // Console.WriteLine($"Endpoint: {endpoint}");

            Forecast.Rootobject forecast = WebHelper.GetForecast(new Uri(endpoint)).GetAwaiter().GetResult();

            // Find all UI components we need and Initialize Variables for them
            TextBlock CityTextBlock             = (TextBlock)FindName("CityTextBlock");
            TextBlock TemperatureTextBlock      = (TextBlock)FindName("TemperatureTextBlock");
            TextBlock ShortForecastTextBlock    = (TextBlock)FindName("ShortForecastTextBlock");
            TextBlock HighTemperatureTextBlock  = (TextBlock)FindName("HighTemperatureTextBlock");
            TextBlock LowTemperatureTextBlock   = (TextBlock)FindName("LowTemperatureTextBlock");
            TextBlock DetailedForecastTextBlock = (TextBlock)FindName("DetailedForecastTextBlock");

            // @todo Find where the daily High and Low temps are stored in the API.
            // Set UI values with the Correct Information
            CityTextBlock.Text                  = gridInfo["city"];
            TemperatureTextBlock.Text           = $"{forecast.properties.periods[0].temperature}\u00B0";
            ShortForecastTextBlock.Text         = forecast.properties.periods[0].shortForecast;
            HighTemperatureTextBlock.Text       = $"H: {0}\u00B0";
            LowTemperatureTextBlock.Text        = $"L: {0}\u00B0";
            DetailedForecastTextBlock.Text      = forecast.properties.periods[0].detailedForecast;
        }

        // @todo Add Celsius conversions for the detailed forecast, right now it only converts the main temperature.
        /// <summary>
        /// When the temperature toggle switch is toggled on or off, convert the temperature to Celsius or Fahrenheit respectively.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CelsiusToggleSwitch_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ToggleSwitch CelsiusToggleSwitch = (ToggleSwitch)FindName("CelsiusToggleSwitch");
            TextBlock TemperatureTextBlock = (TextBlock)FindName("TemperatureTextBlock");

            // Remove the degree unicode character from the string using a regular expressison and convert to an integer.
            int cleanedTemperature = Convert.ToInt32(new Regex(@"[^\d]+").Replace(TemperatureTextBlock.Text, ""));

            // Convert from Fahrenheit -> Celsius
            if (!CelsiusToggleSwitch.IsOn)
                TemperatureTextBlock.Text = $"{(cleanedTemperature - 32) * 5 / 9}\u00B0";
            // Convert from Celsius -> Fahrenheit
            else TemperatureTextBlock.Text = $"{(cleanedTemperature * 9 / 5) + 32}\u00B0";
        }
    }
}