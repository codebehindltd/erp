using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace InnBoardSDC.SDCTool
{
    class CryptHelper
    {
        public MD5 md5 = new MD5CryptoServiceProvider();

        /// <summary>
        /// String encryption
        /// </summary>
        /// <param name="txt">String to be encrypted</param>
        /// <returns>Encrypted String</returns>
        public string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //Start encryption
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="str">Plain text (to be encrypted)</param>
        /// <param name="key">Encryption Key byte array</param>
        /// <returns></returns>
        public string AesEncrypt(string str, byte[] key)
        {
            string strRet;
            int keyLen = key.Length;
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            try
            {
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                strRet = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strRet;
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="str">Plain text (to be decrypted)</param>
        /// <param name="key">Ciphertext</param>
        /// <returns></returns>
        public string AesDecrypt(string str, byte[] key)
        {
            string strRet;
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            try
            {
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                strRet = Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strRet;
        }

        
        public Byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//The last bit of the binary code of the number is 1 if it is an odd number
                {
                    strTemp = "0" + strTemp;//When the digit is odd, add 0 in front
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//High priority
            }
            catch
            { return null; }
        }

        /// <summary>
        /// Code conversion hexadecimal (compressed BCD)
        /// </summary>
        /// <param name="strTemp"></param>
        /// <param name="IntLen"></param>
        /// <returns></returns>
        public Byte[] ConvertFrom(string strTemp, int IntLen)
        {
            try
            {
                Byte[] Temp = ConvertFrom(strTemp.Trim());
                Byte[] return_Byte = new Byte[IntLen];
                if (IntLen != 0)
                {
                    if (Temp.Length < IntLen)
                    {
                        for (int i = 0; i < IntLen - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, IntLen - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            { return null; }
        }

        /// <summary>
        /// Base conversion BCD (decompress BCD)
        /// </summary>
        /// <param name="AData"></param>
        /// <returns></returns>
        public string ConvertTo(Byte[] AData)
        {
            try
            {
                StringBuilder sb = new StringBuilder(AData.Length * 2);
                foreach (Byte b in AData)
                {
                    sb.Append(b >> 4);
                    sb.Append(b & 0x0f);
                }
                return sb.ToString();
            }
            catch { return null; }
        }

        public byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

    }
}
