using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models
{
    /// <summary>
	/// Information of added torrent
	/// </summary>
	public class CreatedTorrent
    {
        /// <summary>
        /// Torrent ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Torrent name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Torrent Hash
        /// </summary>
        [JsonProperty("hashString")]
        public string HashString { get; set; }
    }
}
