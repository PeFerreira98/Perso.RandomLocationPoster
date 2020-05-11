using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace AutoRandomLocationPoster
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string url = "yourURL";
            long patrolID = 0;

            while (true)
            {
                var coord = GetLatLonRandomValue();

                Console.WriteLine("Latitude -> " + coord.Item1 + "; Longitude -> " + coord.Item2);

                PostRequest(client, patrolID, coord.Item1, coord.Item2, url);

                Thread.Sleep(1000);
            }
        }

        public static (double, double) GetLatLonRandomValue()
        {
            var rand = new Random(DateTime.Now.Second);

            double lat = 41 + rand.Next(1454, 2532) * 0.0001;
            double lon = -8 - rand.Next(5577, 7152) * 0.0001;

            return (lat, lon);
        }

        public static async void PostRequest(HttpClient client, long patrolID, double latitude, double longitude, string url)
        {
            var values = new Dictionary<string, string>
            {
                { "PatrolID", patrolID.ToString()},
                { "ActualLatitude", latitude.ToString() },
                { "ActualLongitude", longitude.ToString() },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
