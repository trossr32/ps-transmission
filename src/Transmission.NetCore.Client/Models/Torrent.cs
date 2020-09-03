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

        [JsonProperty("bandwidthPriority")]
        public int BandwidthPriority { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("corruptEver")]
        public int CorruptEver { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("dateCreated")]
        public int DateCreated { get; set; }

        [JsonProperty("desiredAvailable")]
        public long DesiredAvailable { get; set; }

        [JsonProperty("doneDate")]
        public int DoneDate { get; set; }

        [JsonProperty("downloadDir")]
        public string DownloadDir { get; set; }

        [JsonProperty("downloadedEver")]
        public string DownloadedEver { get; set; }

        [JsonProperty("downloadLimit")]
        public string DownloadLimit { get; set; }

        [JsonProperty("downloadLimited")]
        public string DownloadLimited { get; set; }

        [JsonProperty("editDate")]
        public int EditDate { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("ErrorString")]
        public string ErrorString { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("etaIdle")]
        public int EtaIdle { get; set; }

        [JsonProperty("files")]
        public TorrentFiles[] Files { get; set; }

        [JsonProperty("fileStats")]
        public TorrentFile[] FileStats { get; set; }

        [JsonProperty("hashString")]
        public string HashString { get; set; }

        [JsonProperty("haveUnchecked")]
        public int HaveUnchecked { get; set; }

        [JsonProperty("haveValid")]
        public long HaveValid { get; set; }

        [JsonProperty("honorsSessionLimits")]
        public bool HonorsSessionLimits { get; set; }

        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("isStalled")]
        public bool IsStalled { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("leftUntilDone")]
        public long LeftUntilDone { get; set; }

        [JsonProperty("MagnetLink")]
        public string MagnetLink { get; set; }

        [JsonProperty("manualAnnounceTime")]
        public int ManualAnnounceTime { get; set; }

        [JsonProperty("maxConnectedPeers")]
        public int MaxConnectedPeers { get; set; }

        [JsonProperty("metadataPercentComplete")]
        public double MetadataPercentComplete { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("peer-limit")]
        public int PeerLimit { get; set; }

        [JsonProperty("peers")]
        public TorrentPeers[] Peers { get; set; }

        [JsonProperty("peersConnected")]
        public int PeersConnected { get; set; }

        [JsonProperty("peersFrom")]
        public TorrentPeersFrom PeersFrom { get; set; }

        [JsonProperty("peersSendingToUs")]
        public int PeersSendingToUs { get; set; }

        [JsonProperty("percentDone")]
        public double PercentDone { get; set; }

        [JsonProperty("pieces")]
        public string Pieces { get; set; }

        [JsonProperty("pieceCount")]
        public int PieceCount { get; set; }

        [JsonProperty("PieceSize")]
        public int PieceSize { get; set; }

        [JsonProperty("priorities")]
        public int[] Priorities { get; set; }

        [JsonProperty("queuePosition")]
        public int QueuePosition { get; set; }

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

        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        [JsonProperty("torrentFile")]
        public string TorrentFile { get; set; }

        [JsonProperty("uploadedEver")]
        public long UploadedEver { get; set; }

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

    public class TorrentFiles
    {
        [JsonProperty("bytesCompleted")]
        public double BytesCompleted { get; set; }

        [JsonProperty("length")]
        public double Length { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TorrentFile
    {
        [JsonProperty("bytesCompleted")]
        public double BytesCompleted { get; set; }

        [JsonProperty("wanted")]
        public bool Wanted { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }
    }

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

    public class TorrentTrackers
    {
        [JsonProperty("announce")]
        public string Announce { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("scrape")]
        public string Scrape { get; set; }

        [JsonProperty("tier")]
        public int Tier { get; set; }
    }

    public class TorrentTracker
    {

        [JsonProperty("announce")]
        public string Announce { get; set; }

        [JsonProperty("announceState")]
        public int AnnounceState { get; set; }

        [JsonProperty("downloadCount")]
        public int DownloadCount { get; set; }

        [JsonProperty("hasAnnounced")]
        public bool HasAnnounced { get; set; }

        [JsonProperty("hasScraped")]
        public bool HasScraped { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("isBackup")]
        public bool IsBackup { get; set; }

        [JsonProperty("lastAnnouncePeerCount")]
        public int LastAnnouncePeerCount { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lastAnnounceResult")]
        public string LastAnnounceResult { get; set; }

        [JsonProperty("lastAnnounceSucceeded")]
        public bool LastAnnounceSucceeded { get; set; }

        [JsonProperty("lastAnnounceStartTime")]
        public int LastAnnounceStartTime { get; set; }

        [JsonProperty("lastScrapeResult")]
        public string LastScrapeResult { get; set; }

        [JsonProperty("lastAnnounceTimedOut")]
        public bool LastAnnounceTimedOut { get; set; }

        [JsonProperty("lastAnnounceTime")]
        public int LastAnnounceTime { get; set; }

        [JsonProperty("lastScrapeSucceeded")]
        public bool LastScrapeSucceeded { get; set; }

        [JsonProperty("lastScrapeStartTime")]
        public int LastScrapeStartTime { get; set; }

        [JsonProperty("lastScrapeTimedOut")]
        public bool LastScrapeTimedOut { get; set; }

        [JsonProperty("lastScrapeTime")]
        public int LastScrapeTime { get; set; }

        [JsonProperty("scrape")]
        public string Scrape { get; set; }

        [JsonProperty("tier")]
        public int Tier { get; set; }

        [JsonProperty("leecherCount")]
        public int LeecherCount { get; set; }

        [JsonProperty("nextAnnounceTime")]
        public int NextAnnounceTime { get; set; }

        [JsonProperty("nextScrapeTime")]
        public int NextScrapeTime { get; set; }

        [JsonProperty("scrapeState")]
        public int ScrapeState { get; set; }

        [JsonProperty("seederCount")]
        public int SeederCount { get; set; }
    }

    //TODO: Separate "remove" and "active" torrents in "torrentsGet"
    /// <summary>
    /// Contains arrays of torrents and removed torrents
    /// </summary>
    public class Torrents
    {
        /// <summary>
        /// Array of torrents
        /// </summary>
        [JsonProperty("torrents")]
        public Torrent[] TorrentList { get; set; }

        /// <summary>
        /// Array of torrent-id numbers of recently-removed torrents
        /// </summary>
        [JsonProperty("removed")]
        public Torrent[] Removed { get; set; }
    }
}
