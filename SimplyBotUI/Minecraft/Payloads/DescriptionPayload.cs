using Newtonsoft.Json;

namespace SimplyBotUI.Minecraft.Payloads
{
    internal class DescriptionPayload
    {
        [JsonProperty("text")] public string Text { get; set; }
    }
}