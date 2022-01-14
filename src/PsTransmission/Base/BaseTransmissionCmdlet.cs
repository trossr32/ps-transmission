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
        /// Attempt to validate credentials are available in the session or stored permanently.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (TransmissionContext.HasCredentials)
                return;

            try
            {
                TransmissionContext.Credentials = Task.Run(async () => await AuthService.GetConfig()).Result;
            }
            catch (Exception e)
            {
                var credsGetError = "Failed to retrieve credentials. If you have upgraded from v1.0.8 or lower then this is likely caused by the device id package dependency that is used to encrypt your credentials being upgraded. Please remove your existing credentials with Remove-TransmissionCredentials and then set them again with Set-TransmissionCredentials.";

                ThrowTerminatingError(new ErrorRecord(new Exception(credsGetError, e), null, ErrorCategory.AuthenticationError, null)
                {
                    CategoryInfo = { Reason = credsGetError },
                    ErrorDetails = new ErrorDetails(credsGetError)
                });
            }

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
