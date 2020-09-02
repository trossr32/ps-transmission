using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;

namespace TransmissionManager.Config
{
    [Cmdlet(VerbsCommon.Set, "TransmissionConfig", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class SetTransmissionConfigCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("H")]
        public string Host { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("U")]
        public string User { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("P")]
        public string Password { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// The majority of the word cloud drawing occurs here.
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                var config = new TransmissionCredentials
                {
                    Host = !string.IsNullOrWhiteSpace(Host) ? Host : null,
                    User = !string.IsNullOrWhiteSpace(User) ? User : null,
                    Password = !string.IsNullOrWhiteSpace(Password) ? Password : null
                };

                Task.Run(async () => await AuthService.SetConfig(config));
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to create a local configuration file, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
