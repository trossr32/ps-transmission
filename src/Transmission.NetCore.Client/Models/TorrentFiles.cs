using Newtonsoft.Json;

namespace Transmission.NetCore.Client.Models;

public abstract class TorrentFilesBase
{
    [JsonProperty("bytesCompleted")]
    public double BytesCompleted { get; set; }
}

public class TorrentFiles : TorrentFilesBase
{
    [JsonProperty("length")]
    public double Length { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class TorrentFile : TorrentFilesBase
{
    [JsonProperty("wanted")]
    public bool Wanted { get; set; }

    [JsonProperty("priority")]
    public int Priority { get; set; }
}