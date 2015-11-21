namespace Ignite.Api
{
    using Newtonsoft.Json;

    public class ErrorJsonResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("validationmessages")]
        public string[] ValidationMessages { get; set; }

        [JsonProperty("developerException")]
        public string DeveloperException { get; set; }
    }
}