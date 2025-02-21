
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace TrudoseAdminPortalAPI.Helpers
{
    public class CryptLogic
    {
        private static string _encryptionKey = "ACQUEONT";
        private static string secretKey = "0123456789abcdef0123456789abcdef";
        public static string Encrypt(string plainText)
        {
            // Check arguments. 
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] encrypted;

            // Encrypt the string to an array of bytes. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all .data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the Converted Base String of encrypted bytes from the memory stream. 
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            string plaintext;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    CryptoStream csDecrypt = null;
                    StreamReader srDecrypt = null;
                    try
                    {
                        csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                        using (srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                    finally
                    {
                        srDecrypt?.Dispose();
                        csDecrypt?.Dispose();
                    }
                }
            }

            return plaintext;
        }

        public static string ECBDecrypt(string cipherText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, null);

                byte[] encryptedBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedBytes = new byte[encryptedBytes.Length];

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (MemoryStream msDecrypted = new MemoryStream())
                {
                    csDecrypt.CopyTo(msDecrypted);
                    decryptedBytes = msDecrypted.ToArray();
                }

                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedText;
            }
        }

        public static string ECBEncrypt(string plaintext)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, null);

                // Convert the plaintext to bytes
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    // Get the encrypted bytes
                    byte[] encryptedBytes = msEncrypt.ToArray();

                    // Convert to a base64-encoded string
                    string encryptedText = Convert.ToBase64String(encryptedBytes);
                    return encryptedText;
                }
            }
        }


        public static string EncodeObjectTo64<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return CryptLogic.EncodeTo64(json);
        }

        public static T DecodeObjectTo64<T>(string str)
        {
            string json = CryptLogic.DecodeFrom64(str);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static T AESDecryptionData<T>(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");
            var encrypted = Convert.FromBase64String(cipherText);
            return JsonConvert.DeserializeObject<T>(DecryptStringFromBytes(encrypted, keybytes, iv));
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new AesCryptoServiceProvider())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        CryptoStream csDecrypt = null;
                        StreamReader srDecrypt = null;
                        try
                        {
                            csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                            using (srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                        finally
                        {
                            srDecrypt?.Dispose();
                            csDecrypt?.Dispose();
                        }

                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
    }
}
