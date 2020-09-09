using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Flurl;
using PsTransmission.Core.Components;
using Transmission.Base;

namespace Transmission.System
{
    [Cmdlet(VerbsLifecycle.Invoke, "TransmissionWeb", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class InvokeTransmissionWebCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="InvokeTransmissionWebCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="InvokeTransmissionWebCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            string url = new Url(TransmissionContext.Credentials.Host).Root;

            try
            {
                Process.Start(url);

                WriteObject("Web UI launched");
            }
            catch (Exception e)
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");

                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to launch web UI. {e.Message}", e), null, ErrorCategory.OperationStopped, null));
                }
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="InvokeTransmissionWebCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
