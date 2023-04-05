using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace UWeather
{
    internal static class WebHelper
    {
        /// <summary>
        /// Retrieves the nearest weather station grid coordinate 
        /// to display the most relevant information from the API.
        /// </summary>
        /// <param name="deviceLatitude">Device's Current Latitude</param>
        /// <param name="deviceLongitude">Device's Current Longitude</param>
        /// <returns>Dictionary containing 'gridX' and 'gridY' values respectively.</returns>
        public static async Task<Dictionary<string, string>> GetNearestGridCoordinatesAsync(double deviceLatitude, double deviceLongitude)
        {
            HttpClient client = new();
            Uri endpoint = new($"https://api.weather.gov/points/{deviceLatitude},{deviceLongitude}");
            client.DefaultRequestHeaders.Add("User-Agent", "Anything");

            HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, endpoint)).Result;
            string json = await response.Content.ReadAsStringAsync();
            Points.Rootobject deserializedJson = JsonSerializer.Deserialize<Points.Rootobject>(json,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // @todo 'office' could also be 'cwa' or substr of 'forecastOffice', needs testing.
            Dictionary<string, string> gridCoordinates = new();
            gridCoordinates.Add("office", deserializedJson.properties.gridId); 
            gridCoordinates.Add("gridX", deserializedJson.properties.gridX.ToString());
            gridCoordinates.Add("gridY", deserializedJson.properties.gridY.ToString());
            gridCoordinates.Add("city", deserializedJson.properties.relativeLocation.properties.city);
            return gridCoordinates;
        }

        /// <summary>
        /// Retrieves the API endpoint containing the end-user's local weather information.
        /// </summary>
        /// <param name="office"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <param name="route"></param>
        /// <returns>Localized API Endpoint</returns>
        public static string GetLocalizedAPIEndpoint(string office, int gridX, int gridY, string? route = null)
        {
            string path = $"https://api.weather.gov/gridpoints/{office}/{gridX},{gridY}";
            return route != null ? path + route : path;
        }


        /// <summary>
        /// Retrieves the full forecast information from the API endpoint provided.
        /// </summary>
        /// <param name="endpoint">API Endpoint containing all relevant local forecast information</param>
        /// <returns>Full Local Forecast Information</returns>
        public static async Task<Forecast.Rootobject> GetForecast(Uri endpoint)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "Anything");

            HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, endpoint)).Result;
            string json = await response.Content.ReadAsStringAsync();
            var deserializedJson = JsonSerializer.Deserialize<Forecast.Rootobject>(json,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return deserializedJson;
        }
    }
}