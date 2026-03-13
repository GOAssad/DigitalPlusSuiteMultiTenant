using System;
using System.Security.Cryptography;

namespace Acceso.Clases.Datos.Generales
{
    /// <summary>
    /// Verifica passwords hasheados por ASP.NET Core Identity V3 (PBKDF2).
    /// Compatible con .NET Framework 4.8.
    /// </summary>
    public static class IdentityPasswordVerifier
    {
        /// <summary>
        /// Verifica un password contra un hash de ASP.NET Core Identity.
        /// </summary>
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
                return false;

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
                return false;

            byte formatMarker = decodedHashedPassword[0];

            if (formatMarker == 0x01)
                return VerifyV3(decodedHashedPassword, providedPassword);

            // V2 (format marker 0x00) - legacy, unlikely but handle
            if (formatMarker == 0x00)
                return VerifyV2(decodedHashedPassword, providedPassword);

            return false;
        }

        private static bool VerifyV3(byte[] decodedHashedPassword, string password)
        {
            // Format: marker(1) + prf(4) + iterCount(4) + saltLen(4) + salt(saltLen) + subkey
            if (decodedHashedPassword.Length < 13)
                return false;

            int prf = ReadNetworkByteOrder(decodedHashedPassword, 1);
            int iterCount = ReadNetworkByteOrder(decodedHashedPassword, 5);
            int saltLength = ReadNetworkByteOrder(decodedHashedPassword, 9);

            if (saltLength < 0 || decodedHashedPassword.Length < 13 + saltLength)
                return false;

            byte[] salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, saltLength);

            int subkeyLength = decodedHashedPassword.Length - 13 - saltLength;
            byte[] expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 13 + saltLength, expectedSubkey, 0, subkeyLength);

            // Map PRF enum to HashAlgorithmName
            HashAlgorithmName algorithm;
            switch (prf)
            {
                case 0: algorithm = HashAlgorithmName.SHA1; break;
                case 1: algorithm = HashAlgorithmName.SHA256; break;
                case 2: algorithm = HashAlgorithmName.SHA512; break;
                default: return false;
            }

            byte[] actualSubkey;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterCount, algorithm))
            {
                actualSubkey = pbkdf2.GetBytes(subkeyLength);
            }

            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }

        private static bool VerifyV2(byte[] decodedHashedPassword, string password)
        {
            // V2: PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations
            if (decodedHashedPassword.Length != 49) // 1 + 16 + 32
                return false;

            byte[] salt = new byte[16];
            Buffer.BlockCopy(decodedHashedPassword, 1, salt, 0, 16);

            byte[] expectedSubkey = new byte[32];
            Buffer.BlockCopy(decodedHashedPassword, 17, expectedSubkey, 0, 32);

            byte[] actualSubkey;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA1))
            {
                actualSubkey = pbkdf2.GetBytes(32);
            }

            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }

        private static int ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((int)buffer[offset] << 24)
                | ((int)buffer[offset + 1] << 16)
                | ((int)buffer[offset + 2] << 8)
                | (int)buffer[offset + 3];
        }

        // Polyfill para .NET Framework 4.8 que no tiene CryptographicOperations
        private static class CryptographicOperations
        {
            public static bool FixedTimeEquals(byte[] left, byte[] right)
            {
                if (left.Length != right.Length)
                    return false;

                int result = 0;
                for (int i = 0; i < left.Length; i++)
                    result |= left[i] ^ right[i];

                return result == 0;
            }
        }
    }
}
