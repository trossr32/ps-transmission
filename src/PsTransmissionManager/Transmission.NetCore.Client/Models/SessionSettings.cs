﻿using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    public class SessionSettings : ArgumentsBase
    {
        /// <summary>
        /// Max global download speed (KBps)
        /// </summary>
        [JsonProperty("alt-speed-down")]
        public int? AlternativeSpeedDown { get; set; }

        /// <summary>
        /// True means use the alt speeds
        /// </summary>
        [JsonProperty("alt-speed-enabled")]
        public bool? AlternativeSpeedEnabled { get; set; }

        /// <summary>
        /// When to turn on alt speeds (units: minutes after midnight)
        /// </summary>
        [JsonProperty("alt-speed-time-begin")]
        public int? AlternativeSpeedTimeBegin { get; set; }

        /// <summary>
        /// True means the scheduled on/off times are used
        /// </summary>
        [JsonProperty("alt-speed-time-enabled")]
        public bool? AlternativeSpeedTimeEnabled { get; set; }

        /// <summary>
        /// When to turn off alt speeds
        /// </summary>
        [JsonProperty("alt-speed-time-end")]
        public int? AlternativeSpeedTimeEnd { get; set; }

        /// <summary>
        /// What day(s) to turn on alt speeds
        /// </summary>
        [JsonProperty("alt-speed-time-day")]
        public int? AlternativeSpeedTimeDay { get; set; }

        /// <summary>
        /// Max global upload speed (KBps)
        /// </summary>
        [JsonProperty("alt-speed-up")]
        public int? AlternativeSpeedUp { get; set; }

        /// <summary>
        /// Location of the blocklist to use for "blocklist-update"
        /// </summary>
        [JsonProperty("blocklist-url")]
        public string BlockListUrl { get; set; }

        /// <summary>
        /// True means enabled
        /// </summary>
        [JsonProperty("blocklist-enabled")]
        public bool? BlockListEnabled { get; set; }

        /// <summary>
        /// Maximum size of the disk cache (MB)
        /// </summary>
        [JsonProperty("cache-size-mb")]
        public int? CacheSizeMb { get; set; }

        /// <summary>
        /// Default path to download torrents
        /// </summary>
        [JsonProperty("download-dir")]
        public string DownloadDirectory { get; set; }

        /// <summary>
        /// Max number of torrents to download at once (see download-queue-enabled)
        /// </summary>
        [JsonProperty("download-queue-size")]
        public int? DownloadQueueSize { get; set; }

        /// <summary>
        /// If true, limit how many torrents can be downloaded at once
        /// </summary>
        [JsonProperty("download-queue-enabled")]
        public bool? DownloadQueueEnabled { get; set; }

        /// <summary>
        /// True means allow dht in public torrents
        /// </summary>
        [JsonProperty("dht-enabled")]
        public bool? DhtEnabled { get; set; }

        /// <summary>
        /// "required", "preferred", "tolerated"
        /// </summary>
        [JsonProperty("encryption")]
        public string Encryption { get; set; }

        /// <summary>
        /// Torrents we're seeding will be stopped if they're idle for this long
        /// </summary>
        [JsonProperty("idle-seeding-limit")]
        public int? IdleSeedingLimit { get; set; }

        /// <summary>
        /// True if the seeding inactivity limit is honored by default
        /// </summary>
        [JsonProperty("idle-seeding-limit-enabled")]
        public bool? IdleSeedingLimitEnabled { get; set; }

        /// <summary>
        /// Path for incomplete torrents, when enabled
        /// </summary>
        [JsonProperty("incomplete-dir")]
        public string IncompleteDirectory { get; set; }

        /// <summary>
        /// True means keep torrents in incomplete-dir until done
        /// </summary>
        [JsonProperty("incomplete-dir-enabled")]
        public bool? IncompleteDirectoryEnabled { get; set; }

        /// <summary>
        /// True means allow Local Peer Discovery in public torrents
        /// </summary>
        [JsonProperty("lpd-enabled")]
        public bool? LpdEnabled { get; set; }

        /// <summary>
        /// Maximum global number of peers
        /// </summary>
        [JsonProperty("peer-limit-global")]
        public int? PeerLimitGlobal { get; set; }

        /// <summary>
        /// Maximum global number of peers
        /// </summary>
        [JsonProperty("peer-limit-per-torrent")]
        public int? PeerLimitPerTorrent { get; set; }

        /// <summary>
        /// True means allow pex in public torrents
        /// </summary>
        [JsonProperty("pex-enabled")]
        public bool? PexEnabled { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        [JsonProperty("peer-port")]
        public int? PeerPort { get; set; }

        /// <summary>
        /// True means pick a random peer port on launch
        /// </summary>
        [JsonProperty("peer-port-random-on-start")]
        public bool? PeerPortRandomOnStart { get; set; }

        /// <summary>
        /// true means enabled
        /// </summary>
        [JsonProperty("port-forwarding-enabled")]
        public bool? PortForwardingEnabled { get; set; }

        /// <summary>
        /// Whether or not to consider idle torrents as stalled
        /// </summary>
        [JsonProperty("queue-stalled-enabled")]
        public bool? QueueStalledEnabled { get; set; }

        /// <summary>
        /// Torrents that are idle for N minuets aren't counted toward seed-queue-size or download-queue-size
        /// </summary>
        [JsonProperty("queue-stalled-minutes")]
        public int? QueueStalledMinutes { get; set; }

        /// <summary>
        /// True means append ".part" to incomplete files
        /// </summary>
        [JsonProperty("rename-partial-files")]
        public bool? RenamePartialFiles { get; set; }

        /// <summary>
        /// Filename of the script to run
        /// </summary>
        [JsonProperty("script-torrent-done-filename")]
        public string ScriptTorrentDoneFilename { get; set; }

        /// <summary>
        /// Whether or not to call the "done" script
        /// </summary>
        [JsonProperty("script-torrent-done-enabled")]
        public bool? ScriptTorrentDoneEnabled { get; set; }

        /// <summary>
        /// The default seed ratio for torrents to use
        /// </summary>
        [JsonProperty("seedRatioLimit")]
        public double? SeedRatioLimit { get; set; }

        /// <summary>
        /// True if seedRatioLimit is honored by default
        /// </summary>
        [JsonProperty("seedRatioLimited")]
        public bool? SeedRatioLimited { get; set; }

        /// <summary>
        /// Max number of torrents to uploaded at once (see seed-queue-enabled)
        /// </summary>
        [JsonProperty("seed-queue-size")]
        public int? SeedQueueSize { get; set; }

        /// <summary>
        /// If true, limit how many torrents can be uploaded at once
        /// </summary>
        [JsonProperty("seed-queue-enabled")]
        public bool? SeedQueueEnabled { get; set; }

        /// <summary>
        /// Max global download speed (KBps)
        /// </summary>
        [JsonProperty("speed-limit-down")]
        public int? SpeedLimitDown { get; set; }

        /// <summary>
        /// True means enabled
        /// </summary>
        [JsonProperty("speed-limit-down-enabled")]
        public bool? SpeedLimitDownEnabled { get; set; }

        /// <summary>
        ///  max global upload speed (KBps)
        /// </summary>
        [JsonProperty("speed-limit-up")]
        public int? SpeedLimitUp { get; set; }

        /// <summary>
        /// True means enabled
        /// </summary>
        [JsonProperty("speed-limit-up-enabled")]
        public bool? SpeedLimitUpEnabled { get; set; }

        /// <summary>
        /// True means added torrents will be started right away
        /// </summary>
        [JsonProperty("start-added-torrents")]
        public bool? StartAddedTorrents { get; set; }

        /// <summary>
        /// True means the .torrent file of added torrents will be deleted
        /// </summary>
        [JsonProperty("trash-original-torrent-files")]
        public bool? TrashOriginalTorrentFiles { get; set; }

        [JsonProperty("units")]
        public Units Units { get; set; }

        /// <summary>
        /// True means allow utp
        /// </summary>
        [JsonProperty("utp-enabled")]
        public bool? UtpEnabled { get; set; }
    }
}
