using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    /// <summary>
    /// Rename torrent result information
    /// </summary>
	public class RenamedTorrent
    {
        /// <summary>
        /// The torrent's unique Id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// File path.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
