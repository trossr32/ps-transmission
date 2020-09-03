using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DeviceId;

namespace PsTransmission.Core.Services
{
    public static class EncryptionService
    {
        private static readonly byte[] Salt = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Encrypts the input using the salt defined in this service and an encryption key derived from the host machine's processor and machine ids.
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static async Task<string> Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using Aes crypto = Aes.Create();
            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey(), Salt);

            crypto.Key = pdb.GetBytes(32);
            crypto.IV = pdb.GetBytes(16);

            await using MemoryStream ms = new MemoryStream();
            await using (CryptoStream cs = new CryptoStream(ms, crypto.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Decrypts the input using the salt defined in this service and an encryption key derived from the host machine's processor and machine ids.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static async Task<string> Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");

            byte[] cipherBytes = Convert.FromBase64String(cipherText.Trim());

            using Aes crypto = Aes.Create();
            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey(), Salt);

            crypto.Key = pdb.GetBytes(32);
            crypto.IV = pdb.GetBytes(16);

            await using MemoryStream ms = new MemoryStream();
            await using (CryptoStream cs = new CryptoStream(ms, crypto.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
            }

            return Encoding.Unicode.GetString(ms.ToArray());
        }

        /// <summary>
        /// Build an encryption key unique to the host machine
        /// </summary>
        /// <returns></returns>
        private static string EncryptionKey()
        {
            return new DeviceIdBuilder()
                .AddProcessorId()
                .AddMachineName()
                .ToString();
        }
    }
}
