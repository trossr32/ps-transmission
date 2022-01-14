using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    /// <summary>
    /// Torrent information
    /// </summary>
    public class Torrent
    {
        /// <summary>
        /// The torrent's unique Id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("addedDate")]
        public int AddedDate { get; set; }

        [JsonProperty("doneDate")]
        public int DoneDate { get; set; }

        [JsonProperty("downloadDir")]
        public string DownloadDir { get; set; }

        [JsonProperty("downloadedEver")]
        public string DownloadedEver { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("ErrorString")]
        public string ErrorString { get; set; }

        [JsonProperty("files")]
        public TorrentFiles[] Files { get; set; }

        [JsonProperty("fileStats")]
        public TorrentFile[] FileStats { get; set; }

        [JsonProperty("hashString")]
        public string HashString { get; set; }

        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty("MagnetLink")]
        public string MagnetLink { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("percentDone")]
        public double PercentDone { get; set; }

        [JsonProperty("queuePosition")]
        public int QueuePosition { get; set; }

        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        [JsonProperty("torrentFile")]
        public string TorrentFile { get; set; }

        [JsonProperty("uploadedEver")]
        public long UploadedEver { get; set; }
        
        [JsonProperty("activityDate")]
        public int ActivityDate { get; set; }

        [JsonProperty("bandwidthPriority")]
        public int BandwidthPriority { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("corruptEver")]
        public long CorruptEver { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("dateCreated")]
        public int DateCreated { get; set; }

        [JsonProperty("desiredAvailable")]
        public long DesiredAvailable { get; set; }

        [JsonProperty("downloadLimit")]
        public string DownloadLimit { get; set; }

        [JsonProperty("downloadLimited")]
        public string DownloadLimited { get; set; }

        [JsonProperty("editDate")]
        public int EditDate { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("etaIdle")]
        public int EtaIdle { get; set; }

        [JsonProperty("haveUnchecked")]
        public int HaveUnchecked { get; set; }

        [JsonProperty("haveValid")]
        public long HaveValid { get; set; }

        [JsonProperty("honorsSessionLimits")]
        public bool HonorsSessionLimits { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("isStalled")]
        public bool IsStalled { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("leftUntilDone")]
        public long LeftUntilDone { get; set; }

        [JsonProperty("manualAnnounceTime")]
        public int ManualAnnounceTime { get; set; }

        [JsonProperty("maxConnectedPeers")]
        public int MaxConnectedPeers { get; set; }

        [JsonProperty("metadataPercentComplete")]
        public double MetadataPercentComplete { get; set; }

        [JsonProperty("peer-limit")]
        public int PeerLimit { get; set; }

        [JsonProperty("peers")]
        public TorrentPeers[] Peers { get; set; }

        [JsonProperty("peersConnected")]
        public int PeersConnected { get; set; }

        [JsonProperty("peersFrom")]
        public TorrentPeersFrom PeersFrom { get; set; }

        [JsonProperty("peersGettingFromUs")]
        public int PeersGettingFromUs { get; set; }

        [JsonProperty("peersSendingToUs")]
        public int PeersSendingToUs { get; set; }

        [JsonProperty("pieces")]
        public string Pieces { get; set; }

        [JsonProperty("pieceCount")]
        public int PieceCount { get; set; }

        [JsonProperty("PieceSize")]
        public int PieceSize { get; set; }

        [JsonProperty("priorities")]
        public int[] Priorities { get; set; }

        [JsonProperty("rateDownload")]
        public int RateDownload { get; set; }

        [JsonProperty("rateUpload")]
        public int RateUpload { get; set; }

        [JsonProperty("recheckProgress")]
        public double RecheckProgress { get; set; }

        [JsonProperty("secondsDownloading")]
        public int SecondsDownloading { get; set; }

        [JsonProperty("secondsSeeding")]
        public int SecondsSeeding { get; set; }

        [JsonProperty("seedIdleLimit")]
        public int SeedIdleLimit { get; set; }

        [JsonProperty("SeedIdleMode")]
        public int SeedIdleMode { get; set; }

        [JsonProperty("seedRatioLimit")]
        public double SeedRatioLimit { get; set; }

        [JsonProperty("SeedRatioMode")]
        public int SeedRatioMode { get; set; }

        [JsonProperty("SizeWhenDone")]
        public long SizeWhenDone { get; set; }

        [JsonProperty("startDate")]
        public int StartDate { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("trackers")]
        public TorrentTrackers[] Trackers { get; set; }

        [JsonProperty("trackerStats")]
        TorrentTracker[] TrackerStats { get; set; }

        [JsonProperty("uploadLimit")]
        public int UploadLimit { get; set; }

        [JsonProperty("uploadLimited")]
        public bool UploadLimited { get; set; }

        [JsonProperty("uploadRatio")]
        public double UploadRatio { get; set; }

        [JsonProperty("wanted")]
        public bool[] Wanted { get; set; }

        [JsonProperty("webseeds")]
        public string[] WebSeeds { get; set; }

        [JsonProperty("webseedsSendingToUs")]
        public int WebSeedsSendingToUs { get; set; }
    }
}
