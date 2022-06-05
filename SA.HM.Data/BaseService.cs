using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.IO;
using HotelManagement.Entity.HMCommon;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;

namespace HotelManagement.Data
{
    public abstract class BaseService
    {
        protected Database dbSmartAspects;
        protected DbConnection cnSmartAspects;
        string encryptedConnectionString = string.Empty;
        string decryptedConnectionString = string.Empty;
   
        public BaseService()
        {
            try
            {
                if (!CheckConnection())
                {
                    encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InnboardConnectionString"].ConnectionString;
                    decryptedConnectionString = Cryptography.Decrypt(encryptedConnectionString);                   
                }
                else
                {
                    encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InnboardConnectionString"].ConnectionString;
                    //// //-----Hotel 71 Main Server Connction String.............
                    //encryptedConnectionString = "KL8eRzEp/QV6avsN7muLDl0GSIiqjRDehA7BUXYp1tbe70+7QfepAngm4Sv8nIwbKoolFmLJTykmAJvwylQgDsDC0iwaW4DG4U20Jv58fCQX8oYonCvLMuuCxbRuClHr";
                    decryptedConnectionString = Cryptography.Decrypt(encryptedConnectionString);
                }

                dbSmartAspects = new SqlDatabase(decryptedConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckConnection()
        {
            bool connectionStatus = false;

            try
            {
                encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InnboardConnectionString"].ConnectionString;
                // //-----Hotel 71 Hotel 71 Main Server Connction String.............
                //encryptedConnectionString = "KL8eRzEp/QV6avsN7muLDl0GSIiqjRDehA7BUXYp1tbe70+7QfepAngm4Sv8nIwbKoolFmLJTykmAJvwylQgDsDC0iwaW4DG4U20Jv58fCQX8oYonCvLMuuCxbRuClHr";
                decryptedConnectionString =  Cryptography.Decrypt(encryptedConnectionString);

                using (SqlDatabase connection = new SqlDatabase(decryptedConnectionString))
                {
                    try
                    {
                        cnSmartAspects = connection.CreateConnection();
                        cnSmartAspects.Open();

                        connectionStatus = true;
                    }
                    catch (SqlException ex)
                    {
                        connectionStatus = false;
                        throw ex;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnSmartAspects.Close();
                cnSmartAspects.Dispose();                
            }

            return connectionStatus;
        }
    }

    public static class Cryptography
    {
        private const string ENCRYPTION_KEY = "PSVJQRk9QTEpNVU1DWUZCRVFGV1VVT0ZOV1RRU1NaWQ=";
        private const string INITILIZATION_VECTOR = "YWlFLVEZZUFNaWlhPQ01ZT0lLWU5HTFJQVFNCRUJZVA=";
        public static String Encrypt(String Input)
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
            aes.IV = Convert.FromBase64String(INITILIZATION_VECTOR);

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }
        public static String Decrypt(String Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
            aes.IV = Convert.FromBase64String(INITILIZATION_VECTOR);

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }
    }
}

