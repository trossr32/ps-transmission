namespace PsTransmission.Core.Constants;

public static class ErrorMessages
{
    public const string ConfigMissing =
        "No host or auth credentials supplied and no local configuration found. Either supply valid Host, User and Password parameters or store a permanent configuration using the Set-TransmissionConfig command.";
}