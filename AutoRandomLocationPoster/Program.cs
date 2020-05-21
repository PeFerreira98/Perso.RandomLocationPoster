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
            string url = "http://local.drive.pt:10122/drive.webapi.mobile/api/patrol/SubmitLocationToDirectionCalculation";
            long patrolID = 5;
            int patrolRange = 10;

            while (true)
            {
                long patrol = GetRandomPatrol(patrolID, patrolRange);

                var coord = GetLatLonRandomValue();

                Console.WriteLine("Latitude -> " + coord.Item1 + "; Longitude -> " + coord.Item2);

                PostRequest(client, patrol, coord.Item1, coord.Item2, url);

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

        public static long GetRandomPatrol(long patrolID, int patrolIDRange)
        {
            var rand = new Random(DateTime.Now.Second);
            return rand.Next((int)patrolID, (int)(patrolID+(long)patrolIDRange));
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
