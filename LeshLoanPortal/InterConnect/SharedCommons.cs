using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InterConnect
{
    public static class SharedCommons
    {
        public static List<string> AllowedImageExtensions = new List<string>(new string[] { ".JPG", ".JPEG", ".PNG", ".PDF" });

        public static bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static string GenerateUniqueId(string LeadString)
        {
            return LeadString + "-" + DateTime.Now.Ticks.ToString();
        }

        public static string GenerateUniqueCode(string LeadString)
        {

            int counter = 0;
            if (counter < 10000)
                return LeadString + "-" + counter++.ToString().PadLeft(4, '0');
            else
                return LeadString + "-" + counter++.ToString();

        }
        public static string GenerateMD5Hash(string input)
        {
            string Salt = "LoanLesh";
            input = Salt + input;
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GenerateUserPassword(string userId)
        {
            string Password = GenerateMD5Hash(userId);
            return Password;
        }
        public static string GeneratePassword()
        {
            string Password = GenerateMD5Hash("T3rr1613");
            return Password;
        }

        public static string GeneratePin()
        {
            string Password = GenerateMD5Hash("13254");
            return Password;
        }

        public static bool IsNumeric(string Amount)
        {
            try
            {
                int amount = int.Parse(Amount.Split('.')[0]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsNumericAndAboveZero(string Amount)
        {
            try
            {

                Int64 amount = Int64.Parse(Amount.Split('.')[0]);
                if (amount <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValidBoolean(string p)
        {
            if (String.IsNullOrEmpty(p))
            {
                return false;
            }
            else if (p.ToUpper() == "TRUE" || p.ToUpper() == "FALSE")
            {
                return true;
            }

            return false;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                if (email == "N/A") { return true; }
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetBoolFromStringDefaultsToFalse(string text)
        {
            try
            {
                return Convert.ToBoolean(text);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int GetIntFromStringDefaultsToFalse(string text)
        {
            try
            {
                return Convert.ToInt32(text);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string GetDigitalSignature(string dataToSign, string PathToPrivateKey)
        {
            // path to your private key
            string pathToPrivateKey = PathToPrivateKey;

            // Password of your private key 
            string Password = "Tingate710";

            // load the private key
            X509Certificate2 cert = new X509Certificate2(pathToPrivateKey, Password, X509KeyStorageFlags.UserKeySet);
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;

            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(dataToSign);
            byte[] hash = sha1.ComputeHash(data);

            // Sign the hash
            byte[] digitalSignatureBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            string strDigitalSignature = Convert.ToBase64String(digitalSignatureBytes);
            return strDigitalSignature;
        }

        public static bool VerifyDigitalSignature(string data, string digitalSignature, string pathToPublicKey)
        {
            return true;
        }

        public static string DecryptString(string encryptedText, string Key)
        {
            return Encryption.encrypt.DecryptString(encryptedText, Key);
        }

        public static string EncryptString(string plainText, string Key)
        {
            return Encryption.encrypt.EncryptString(plainText, Key);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
