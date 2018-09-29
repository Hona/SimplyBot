using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimplyBotUI.Data
{
    internal class TempusDataAccess
    {
        private HttpClient _httpClient;
        public TempusDataAccess()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("https://tempus.xyz/api")};
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetDemoRecord(string map)
        {
            var response = await _httpClient.GetAsync("/maps/name/"+map);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return "";
        }

    }
}
