using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Substructio.Core
{
    public static class AESEncryption
    {

        private static byte[] _key;
        private static byte[] _IV = { };

        private static UTF8Encoding _encoder;
        private static RijndaelManaged _rm;
        static AESEncryption()
        {
            _key = Convert.FromBase64String("secret+string+hooray");
            _rm = new RijndaelManaged();
            _encoder = new UTF8Encoding();
        }

        public static string Encrypt(string unencrypted)
        {
            var rm = new RijndaelManaged();
            rm.GenerateIV();
            var IV = rm.IV;
            var cryptogram = IV.Concat(Encrypt(_encoder.GetBytes(unencrypted), IV));
            return Convert.ToBase64String(cryptogram.ToArray());
        }

        public static string Decrypt(string encrypted)
        {
            var cryptogram = Convert.FromBase64String(encrypted);
            var vector = cryptogram.Take(16).ToArray();
            var buffer = cryptogram.Skip(16).ToArray();
            return _encoder.GetString(Decrypt(buffer, vector));
        }

        public static byte[] Encrypt(byte[] unencrypted, byte[] vector)
        {
            var encryptor = _rm.CreateEncryptor(_key, vector);
            return Transform(unencrypted, encryptor);
        }

        public static byte[] Decrypt(byte[] encrypted, byte[] vector)
        {
            var decryptor = _rm.CreateDecryptor(_key, vector);
            return Transform(encrypted, decryptor);
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
