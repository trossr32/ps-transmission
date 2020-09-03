using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Components;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;

namespace TransmissionManager.Credentials
{
    [Cmdlet(VerbsCommon.Set, "TransmissionCredentials", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class SetTransmissionCredentialsCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("H")]
        public string Host { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("U")]
        public string User { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("P")]
        public string Password { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter StorePermanent { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var config = new TransmissionCredentials
                {
                    Host = Host,
                    User = User,
                    Password = Password
                };

                TransmissionContext.Credentials = config;

                if (StorePermanent.IsPresent)
                    Task.Run(async () => await AuthService.SetConfig(config));
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to set credentials, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
