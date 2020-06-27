using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementApp.ServicesApp
{
    public class HttpServicesReponse
    {
        static readonly HttpClient client = new HttpClient();
        private readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public async Task<string> GetResponse(string dir, string contentJson = "")
        {
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("Jwt")))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Jwt").Trim('"'));
            }
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
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("Jwt")))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Jwt").Trim('"'));
            }
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
        public async Task<string> PutResponse(string dir, string contentJson = "")
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(dir),
                Method = HttpMethod.Put,
                Content = new StringContent(contentJson, Encoding.UTF8, "application/json")
            };
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("Jwt")))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Jwt").Trim('"'));
            }
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
        public async Task<string> DeleteResponse(string dir, string contentJson = "")
        {
            HttpResponseMessage response = await client.DeleteAsync(dir);
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("Jwt")))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Jwt").Trim('"'));
            }
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
