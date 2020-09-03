using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
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
