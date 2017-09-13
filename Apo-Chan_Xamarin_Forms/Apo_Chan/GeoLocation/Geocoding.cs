﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XLabs.Platform.Services.Geolocation;

namespace Apo_Chan.GeoLocation
{
    public class Geocoding
    {
        public static async Task<string> GetAddressAsync(Position position)
        {
            string address = null;

            var client = new HttpClient();
            string requestUri = string.Format
                (
                    Constants.GoogleReverseGeocodingURI,
                    Constants.GoogleGeocodingAPIKey,
                    position.Latitude,
                    position.Longitude
                );
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = Convert.FromJson(content);

                    if (data.Results.Length > 0)
                    {
                        //getting first accurate addresess
                        address = data.Results[0].FormattedAddress;
                    }
                }
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine("-------------------[Debug] " + e.Message);
            }

            return address;
        }
    }
}
