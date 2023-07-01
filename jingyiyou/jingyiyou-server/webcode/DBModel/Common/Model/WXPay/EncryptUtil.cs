using Org.BouncyCastle.Crypto;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ncc2019.Common.Model
{
    public class EncryptUtil
    {

        // 利用CryptoStream进行加密
        public static byte[] Encrypt(byte[] message, BufferedAsymmetricBlockCipher cipher)
        {
            using (var buffer = new MemoryStream())
            {
                using (var transform = new BufferedCipherTransform(cipher))
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                using (var messageStream = new MemoryStream(message))
                    messageStream.CopyTo(stream);
                return buffer.ToArray();
            }
        }
    }

}