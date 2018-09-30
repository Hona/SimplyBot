using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimplyBotUI.Models.Tempus.Responses;

namespace SimplyBotUI.Data
{
    public class TempusDataAccess
    {
        public List<string> MapList { get; set; }
        private static HttpWebRequest CreateWebRequest(string path)
        {
            return (HttpWebRequest) WebRequest.Create("https://tempus.xyz/api" + path);
        }

        private static HttpWebRequest BuildWebRequest(string relativePath)
        {
            var httpWebRequest = CreateWebRequest(relativePath);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            return httpWebRequest;
        }


        private static async Task<T> GetResponse<T>(WebRequest request)
        {
            var response = (HttpWebResponse) await request.GetResponseAsync();
            var stream = response.GetResponseStream();
            var buffer = new byte[1024 * 32];
            if (stream != null) await stream.ReadAsync(buffer, 0, buffer.Length);
            object stringValue = Encoding.UTF8.GetString(buffer).Trim('\0', ' ');

            // If T is a string, don't deserialise
            return typeof(T) == typeof(string) ? (T) stringValue: JsonConvert.DeserializeObject<T>((string)stringValue);
        }
        private static async Task<T> GetResponse<T>(string request)
        {
            var response = (HttpWebResponse)await BuildWebRequest(request).GetResponseAsync();
            var stream = response.GetResponseStream();
            var buffer = new byte[1024 * 128];
            if (stream != null) await stream.ReadAsync(buffer, 0, buffer.Length);
            object stringValue = Encoding.UTF8.GetString(buffer).Trim('\0', ' ');

            // If T is a string, don't deserialise
            return typeof(T) == typeof(string) ? (T)stringValue : JsonConvert.DeserializeObject<T>((string)stringValue);
        }
        public async Task<MapFullOverviewModel> GetFullMapOverView(string map)
        {
            var response  = await GetResponse<MapFullOverviewModel>($"/maps/name/{ParseMapName(map)}/fullOverview");
            return response;
        }
        public async Task<string> GetMapList()
        {
            var response = await GetResponse<string>("/maps/detailedList");
            return response;
        }


        private string ParseMapName(string map)
        {
            return map;
            // TODO: Change this
            map = map.ToLower();
            if (MapList.Contains(map))
            {
                return map;
            }

            foreach (var mapName in MapList)
            {
                var mapParts = mapName.Split('_');
                if (mapParts.Any(mapPart => map == mapPart))
                {
                    return mapName;
                }
            }

            throw new Exception("Map not found");
        }

        public async Task UpdateMapList()
        {
            var response = await GetMapList();
            //MapList = response;

        }
        


    }
}
