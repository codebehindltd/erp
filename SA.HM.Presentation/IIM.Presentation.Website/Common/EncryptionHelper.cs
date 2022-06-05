using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class EncryptionHelper
    {
        #region Members

        private static readonly RijndaelManaged _cryptoProvider;
        //128 bit encyption: DO NOT CHANGE    
        private static readonly byte[] _key = { 18, 19, 8, 24, 36, 22, 4, 22, 17, 5, 11, 9, 13, 15, 06, 23 };
        private static readonly byte[] _iv = { 14, 2, 16, 7, 5, 9, 17, 8, 4, 47, 16, 12, 1, 32, 25, 18 };

        #endregion Members

        #region Constructor

        static EncryptionHelper()
        {
            _cryptoProvider = new RijndaelManaged();
            _cryptoProvider.Mode = CipherMode.CBC;
            _cryptoProvider.Padding = PaddingMode.PKCS7;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Encrypts a given string.
        /// </summary>
        /// <param name="unencryptedString">Unencrypted string</param>
        /// <returns>Returns an encrypted string</returns>
        public string Encrypt(string unencryptedString)
        {
            byte[] bytIn = Encoding.ASCII.GetBytes(unencryptedString);
            byte[] bytOut;

            // Create a MemoryStream
            using (MemoryStream ms = new MemoryStream())
            {
                // Create Crypto Stream that encrypts a stream
                using (CryptoStream cs = new CryptoStream(ms, _cryptoProvider.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
                {
                    // Write content into MemoryStream
                    cs.Write(bytIn, 0, bytIn.Length);
                    cs.FlushFinalBlock();
                }

                bytOut = ms.ToArray();
            }
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>
        /// Decrypts a given string.
        /// </summary>
        /// <param name="encryptedString">Encrypted string</param>
        /// <returns>Returns a decrypted string</returns>
        public string Decrypt(string encryptedString)
        {
            if (!string.IsNullOrWhiteSpace(encryptedString))
            {
                // Convert from Base64 to binary
                byte[] bytIn = Convert.FromBase64String(encryptedString);

                // Create a MemoryStream
                using (MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length))
                {
                    // Create a CryptoStream that decrypts the data
                    using (CryptoStream cs = new CryptoStream(ms, _cryptoProvider.CreateDecryptor(_key, _iv), CryptoStreamMode.Read))
                    {
                        // Read the Crypto Stream
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }

                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="unhashedPassword">The unhashed password.</param>
        /// <returns></returns>
        public byte [] HashPassword(string unhashedPassword)
        {
            return Convert.FromBase64String(Encrypt(unhashedPassword)) ;
        }

        /// <summary>
        /// Hashes the password string.
        /// </summary>
        /// <param name="unhashedPassword">The unhashed password.</param>
        /// <returns></returns>
        public string HashPasswordString(string unhashedPassword)
        {
            return Encrypt(unhashedPassword);
        }

        /// <summary>
        /// Salts the and hash.
        /// </summary>
        /// <param name="rawString">The raw string.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string SaltAndHash(string rawString, string salt)
        {
            return Encrypt(string.Concat(rawString, salt));
        }

        /// <summary>
        /// Validates the password match.
        /// </summary>
        /// <param name="password1">The password1.</param>
        /// <param name="password2">The password2.</param>
        /// <returns></returns>
        public bool ValidatePasswordMatch(string password1, byte[] password2)
        {
            return Convert.ToBase64String(password2).Equals(Encrypt(password1));
        }

        /// <summary>
        /// Generates the password.
        /// </summary>
        /// <returns></returns>
        public string GeneratePassword()
        {
            return RandomPassword.Generate();
        }

        /// <summary>
        /// Generates the specified length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public string Generate(int length)
        {
            return RandomPassword.Generate(length);
        }

        /// <summary>
        /// Generates the specified min length.
        /// </summary>
        /// <param name="minLength">Length of the min.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns></returns>
        public string Generate(int minLength, int maxLength)
        {
            return RandomPassword.Generate(minLength, maxLength);
        }
        #endregion Methods
    }

    /// <summary>
    /// This class can generate random passwords, which do not include ambiguous 
    /// characters, such as I, l, and 1. The generated password will be made of
    /// 7-bit ASCII symbols. Every four characters will include one lower case
    /// character, one upper case character, one number, and one special symbol
    /// (such as '%') in a random order. The password will always start with an
    /// alpha-numeric character; it will not start with a special symbol (we do
    /// this because some back-end systems do not like certain special
    /// characters in the first position).
    /// </summary>
    internal class RandomPassword
    {
        // Define default min and max password lengths.
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 6;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random. It will be no shorter than the minimum default and
        /// no longer than maximum default.
        /// </remarks>
        internal static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="length">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        internal static string Generate(int length)
        {
            return Generate(length, length);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="minLength">
        /// Minimum password length.
        /// </param>
        /// <param name="maxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        internal static string Generate(int minLength,
                                      int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new[]
                                      {
                                          PASSWORD_CHARS_LCASE.ToCharArray(),
                                          PASSWORD_CHARS_UCASE.ToCharArray(),
                                          PASSWORD_CHARS_NUMERIC.ToCharArray(),
                                          PASSWORD_CHARS_SPECIAL.ToCharArray()
                                      };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }
    }

}