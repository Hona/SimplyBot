using Newtonsoft.Json;

namespace SimplyBotUI.Minecraft
{
    internal class Player
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "id")] public string Id { get; set; }
    }
}