using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationManagementApp.ServicesApp
{
    public class HttpServicesReponse
    {
        static readonly HttpClient client = new HttpClient();
        public HttpServicesReponse()
        {

        }
        public async Task<string> GetResponse(string dir, string contentJson = "")
        {
            HttpResponseMessage response = await client.GetAsync(dir);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        public async Task<string> PostResponse(string dir, string contentJson = "")
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(dir),
                Method = HttpMethod.Post,
                Content = new StringContent(contentJson, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
      
    }
}
