using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiaryKeeper
{
    /*
     * 暗号化処理を行うクラス
     */
    class Cipher
    {
        private string key = "DONTLOOKMYDIARY.";
        private string iv = "ENCRYPTEDPAGES@@";

        /**
         * AESで文字列を暗号化する
         * @param text 暗号化する文字列
         */
        public string Encrypt(string text)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);

            byte[] src = Encoding.Unicode.GetBytes(text);

            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                return Convert.ToBase64String(dest);
            }
        }

        /**
         * AESの文字列を復号化する
         * @param text 復号化する文字列
         */
        public string Decrypt(string text)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);
            byte[] src = null;

            try
            {
                src = System.Convert.FromBase64String(text);
            }
            catch(Exception e)
            {
                // not encrypted text
                return text;
            }

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }
    }
}
