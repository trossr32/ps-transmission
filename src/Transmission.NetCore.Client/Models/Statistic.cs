using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    public class Statistic
    {
        [JsonProperty("activeTorrentCount")]
        public int ActiveTorrentCount { get; set; }

        [JsonProperty("downloadSpeed")]
        public int DownloadSpeed { get; set; }

        [JsonProperty("pausedTorrentCount")]
        public int PausedTorrentCount { get; set; }

        [JsonProperty("torrentCount")]
        public int TorrentCount { get; set; }

        [JsonProperty("uploadSpeed")]
        public int UploadSpeed { get; set; }

        [JsonProperty("cumulative-stats")]
        public CommonStatistic CumulativeStats { get; set; }

        [JsonProperty("current-stats")]
        public CommonStatistic CurrentStats { get; set; }
    }

    public class CommonStatistic
    {
        [JsonProperty("uploadedBytes")]
        public double UploadedBytes { get; set; }

        [JsonProperty("downloadedBytes")]
        public double DownloadedBytes { get; set; }

        [JsonProperty("filesAdded")]
        public int FilesAdded { get; set; }

        [JsonProperty("SessionCount")]
        public int SessionCount { get; set; }

        [JsonProperty("SecondsActive")]
        public int SecondsActive { get; set; }
    }
}
