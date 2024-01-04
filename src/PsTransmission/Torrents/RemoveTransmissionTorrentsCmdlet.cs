using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Remove torrents.
/// </para>
/// <para type="description">
/// Remove torrents. The All, Completed and Incomplete switch parameters will return all torrents, torrents that
/// have finished downloading or are still downloading (based on download percent).
/// </para>
/// <para type="description">
/// If the DeleteData switch is supplied then any local data will be deleted when the torrent is removed.
/// </para>
/// <para type="description">
/// Calls the 'torrent-remove' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Remove all torrents</para>
///     <code>PS C:\> Remove-TransmissionTorrents -All</code>
///     <remarks>Removes all torrents.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Remove all completed torrents and delete local data</para>
///     <code>PS C:\> Remove-TransmissionTorrents -Completed -DeleteData</code>
///     <remarks>Removes all completed torrents and deletes local data.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Remove torrents by id using the pipeline</para>
///     <code>PS C:\> @(1,2) | Remove-TransmissionTorrents</code>
///     <remarks>Removes all torrents with ids 1 and 2.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Remove, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class RemoveTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Array of torrent ids.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public List<int> TorrentIds { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all torrents will be removed.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter All { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all completed torrents will be removed.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Completed { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all in progress torrents will be removed.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Incomplete { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied any local data will also be deleted when removing the torrent.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter DeleteData { get; set; }

    private List<int> _torrentIds;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = [];
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            if (!All.IsPresent && !Completed.IsPresent && !Incomplete.IsPresent && (TorrentIds ?? []).Count == 0)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: All, Completed, Incomplete, TorrentIds"), null, ErrorCategory.InvalidArgument, null));

            var torrentSvc = new TorrentService();

            (bool success, int torrentCount) response;
            var info = "";

            if (All.IsPresent)
                response = Task.Run(async () => await torrentSvc.RemoveTorrents(null, DeleteData.IsPresent)).Result;
            else if (Completed.IsPresent)
            {
                response = Task.Run(async () => await torrentSvc.RemoveCompletedTorrents(DeleteData.IsPresent)).Result;

                info = "completed ";
            }
            else if (Incomplete.IsPresent)
            {
                response = Task.Run(async () => await torrentSvc.RemoveIncompleteTorrents(DeleteData.IsPresent)).Result;

                info = "incomplete ";
            }
            else
                response = Task.Run(async () => await torrentSvc.RemoveTorrents(_torrentIds, DeleteData.IsPresent)).Result;

            if (!response.success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to remove torrents"), null, ErrorCategory.OperationStopped, null));

            if (response.torrentCount == 0)
                WriteWarning("No torrents found.");
            else
                WriteObject($"{response.torrentCount} {info} torrent{(response.torrentCount > 1 ? "s" : "")} removed.");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to remove torrents with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}