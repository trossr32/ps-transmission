using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    public class TorrentPeersFrom
    {
        [JsonProperty("fromDht")]
        public int FromDht { get; set; }

        [JsonProperty("fromIncoming")]
        public int FromIncoming { get; set; }

        [JsonProperty("fromLpd")]
        public int FromLpd { get; set; }

        [JsonProperty("fromLtep")]
        public int FromLtep { get; set; }

        [JsonProperty("fromPex")]
        public int FromPex { get; set; }

        [JsonProperty("fromTracker")]
        public int FromTracker { get; set; }
    }
}
