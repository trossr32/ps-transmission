using System.Collections.Generic;
using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    public class AddTorrentsResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AddTorrentSuccess> Successes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AddTorrentFailure> Failures { get; set; }
    }

    public class AddTorrentSuccess
    {
        public string SubmittedData { get; set; }
        public string SubmittedDataType { get; set; }

        public CreatedTorrent TransmissionResponse { get; set; }
    }

    public class AddTorrentFailure
    {
        public string SubmittedData { get; set; }
        public string SubmittedDataType { get; set; }

        public string Error { get; set; }
    }
}
