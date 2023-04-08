using System;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

// @debug
// using System.Diagnostics;

namespace UWeather
{
    // @todo Change Console Writelines to Message Boxes (or equivalent) for user debugging.
    internal static class DeviceInformation
    {
        /// <summary>
        /// Gets the access status of the end-user's Geolocation Access selection after prompted.
        /// </summary>
        /// <returns>Geolocation Access Status based on end-user selection.</returns>
        public static GeolocationAccessStatus GetGeolocationAccessStatus()
        {
            try
            {
                return Task.Run(async () => { return await Geolocator.RequestAccessAsync(); }).Result;
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.Flatten().InnerExceptions)
                    Console.WriteLine($"{e.GetType().FullName} {e.Message}");
            }

            return GeolocationAccessStatus.Unspecified;
        }

        /// <summary>
        /// Retrieves the current device's geocoordinate information (For use in the weather.gov API).
        /// </summary>
        /// <returns>Current Device's Geocoordinate</returns>
        public static Geocoordinate GetDeviceGeocoordinate()
        {

            if (GetGeolocationAccessStatus().Equals(GeolocationAccessStatus.Allowed))
                return Task.Run(async () => { return await new Geolocator().GetGeopositionAsync(); }).Result.Coordinate;
            else
            {
                // Return a fallback consentless position if location access is denied
                return Task.Run(async () => 
                {
                    var geolocator = new Geolocator();
                    geolocator.AllowFallbackToConsentlessPositions();
                    return await geolocator.GetGeopositionAsync();
                }).Result.Coordinate;
            }
        }
    }
}
