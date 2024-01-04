using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Move a torrent.
/// </para>
/// <para type="description">
/// Move a torrent.
/// </para>
/// <para type="description">
/// Calls the 'torrent-set-location' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Set torrent location</para>
///     <code>PS C:\> Set-TransmissionTorrentsLocation -TorrentIds @(1) -Location "some_path"</code>
///     <remarks>Sets the torrent's location.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Set torrent location from pipeline, move from previous location and return results as a bool</para>
///     <code>PS C:\> @(1) | Set-TransmissionTorrentsLocation -Location "some_path" -Move -AsBool</code>
///     <remarks>Sets the torrent's location and moves from previous location.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Set, "TransmissionTorrentsLocation", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class SetTransmissionTorrentsLocationCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Array of torrent ids.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public List<int> TorrentIds { get; set; }

    /// <summary>
    /// <para type="description">
    /// New torrent location.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true)]
    public string Location { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied move torrent from previous location, otherwise search "location" for files.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Move { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the response will be a boolean, otherwise a success message or a terminating error.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter AsBool { get; set; }

    private List<int> _torrentIds;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = [];
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            // validate a torrent id has been supplied
            if ((TorrentIds ?? []).Count == 0)
                ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

            var torrentSvc = new TorrentService();
                
            var success = Task.Run(async () => await torrentSvc.SetTorrentsLocation(_torrentIds, Location, Move.IsPresent)).Result;

            if (AsBool.IsPresent)
            {
                WriteObject(success);

                return;
            }

            if (!success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Set torrent(s) location failed"), null, ErrorCategory.OperationStopped, null));

            WriteObject("Set torrent(s) location successful");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to set torrent(s) location with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}