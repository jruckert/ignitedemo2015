namespace Ignite.Api
{
    using Newtonsoft.Json;

    public class PagedJsonResult
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("errors")]
        public ErrorJsonResult Errors { get; set; }
    }
}
