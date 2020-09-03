using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Components;
using PsTransmission.Core.Services;

namespace Transmission.Base
{
    public abstract class BaseTransmissionCmdlet : Cmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (TransmissionContext.HasCredentials)
                return;

            TransmissionContext.Credentials = Task.Run(async () => await AuthService.GetConfig()).Result;

            if (TransmissionContext.HasCredentials)
                return;

            var error = "No credentials set, Set-TransmissionCredentials must be run to save credentials before running any cmdlets.";

            ThrowTerminatingError(new ErrorRecord(new Exception(error), null, ErrorCategory.AuthenticationError, null)
            {
                CategoryInfo = {Reason = error},
                ErrorDetails = new ErrorDetails(error)
            });
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
