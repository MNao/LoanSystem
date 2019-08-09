using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LeshLoanMngt.Entities
{
    public static class SharedCommons
    {
        public static string GenerateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            string Salt = "LoanLesh";
            input = Salt + input;

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

        public static string GeneratePassword(string Salt)
        {
            string Password = GenerateMD5Hash(Salt + "T3rr1613");
            return Password;
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

        public static string GenearetHMACSha256Hash(string key, string dataToSign)
        {
            ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(dataToSign);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string base64string = Convert.ToBase64String(hashmessage);
                string HmacHexString = ByteArrayToString(hashmessage);
                return HmacHexString;
            }
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }


        //public static string DecryptString(string encryptedText, string Key)
        //{
        //    return Encryption.encrypt.DecryptString(encryptedText, Key);
        //}

        //public static string EncryptString(string plainText, string Key)
        //{
        //    return Encryption.encrypt.EncryptString(plainText, Key);
        //}

        public static string CheckForNulls(object obj, string nullableProperties)
        {
            string[] nullables = nullableProperties.Split('|');
            List<string> propertiesThatAreAllowedToBeNull = new List<string>();
            propertiesThatAreAllowedToBeNull.AddRange(nullables);

            FieldInfo[] oldFields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (FieldInfo objProperties in oldFields)
            {
                string propertyName = objProperties.Name;
                string propertyValue = objProperties.GetValue(obj) as string;
                if (propertiesThatAreAllowedToBeNull.Contains(propertyName))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(propertyValue))
                {
                    return "PLEASE SUPPLY A VALUE IN FIELD [" + propertyName + "]";
                }
            }

            return Globals.SUCCESS_STATUS_TEXT;
        }

        public static bool IsNumeric(string Amount)
        {
            try
            {
                Amount = Amount.Replace(",", string.Empty);
                int amount = int.Parse(Amount.Split('.')[0]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string SanitizeNumericInput(string Amount)
        {
            try
            {
                if (string.IsNullOrEmpty(Amount))
                {
                    Amount = "0";
                }
                Amount = Amount.Replace(",", string.Empty);
                Amount = Amount.Split('.')[0];

                return Amount;
            }
            catch (Exception)
            {
                return Amount;
            }
        }

        public static bool IsNumericAndAboveZero(string Amount)
        {
            try
            {
                Amount = Amount.Replace(",", string.Empty);
                int amount = int.Parse(Amount.Split('.')[0]);
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



        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }

}

