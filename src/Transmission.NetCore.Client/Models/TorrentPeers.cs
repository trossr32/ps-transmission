using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    public class TorrentPeers
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("clientIsChoked")]
        public bool ClientIsChoked { get; set; }

        [JsonProperty("clientIsInterested")]
        public bool ClientIsInterested { get; set; }

        [JsonProperty("flagStr")]
        public string FlagStr { get; set; }

        [JsonProperty("isDownloadingFrom")]
        public bool IsDownloadingFrom { get; set; }

        [JsonProperty("isEncrypted")]
        public bool IsEncrypted { get; set; }

        [JsonProperty("isUploadingTo")]
        public bool IsUploadingTo { get; set; }

        [JsonProperty("isUTP")]
        public bool IsUtp { get; set; }

        [JsonProperty("peerIsChoked")]
        public bool PeerIsChoked { get; set; }

        [JsonProperty("peerIsInterested")]
        public bool PeerIsInterested { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("progress")]
        public double Progress { get; set; }

        [JsonProperty("rateToClient")]
        public int RateToClient { get; set; }

        [JsonProperty("rateToPeer")]
        public int RateToPeer { get; set; }
    }
}
