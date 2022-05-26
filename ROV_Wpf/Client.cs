using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ROV_Wpf
{
    public class Client
    {
        HttpClient http_client = new();
        Uri GetUri(string path) => new Uri("http://raspberrypi.local:8000/" + path);
        public Client()
        {
            http_client.Timeout = TimeSpan.FromSeconds(5);
            var a = http_client.GetAsync(GetUri("")).Result;
        }

        public Telemetry get_data()
        {
            string a = http_client.GetAsync(GetUri("data")).Result.Content.ReadAsStringAsync().Result;

            try
            {
                Telemetry m = JsonConvert.DeserializeObject<Telemetry>(a);
                return m;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void engine(EngineCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            var a = http_client.PostAsync(GetUri("engines"),
                new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            return;
        }
    }
}
