using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;

namespace ZFramework.Utils.Encrypt
{
    public class EncryptUtil
    {
        public static string hashKey = "as4werghhw5sszdf";
        public static string Encrypt(string input) {                        

            byte[] data = UTF8Encoding.UTF8.GetBytes(input);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {

                byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));

                using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key,Mode = CipherMode.ECB ,Padding = PaddingMode.PKCS7}) {
                    ICryptoTransform tr = trip.CreateEncryptor();
                    byte[] result = tr.TransformFinalBlock(data,0,data.Length);
                    return Convert.ToBase64String(result,0,result.Length);
                }
            }
        }
        public static string Decrypt(string input)
        {

            byte[] data = Convert.FromBase64String(input);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {

                byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));

                using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform tr = trip.CreateDecryptor();
                    byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(result);
                }
            }
        }
    }
}