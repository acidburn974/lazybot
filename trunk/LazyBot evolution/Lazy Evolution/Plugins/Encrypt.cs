/*
This file is part of LazyBot.

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LazyEvo.Plugins
{
    internal class Encryptor
    {
        internal static List<string> text = createKeys();

        private static List<string> createKeys()
        {
            var res = new List<string>();
            res.Add("lazy_evo");
            res.Add("hsf_klfa");
            res.Add("ios_dnsd");
            res.Add("asi_yqnx");
            res.Add("pig_sdkl");
            res.Add("dgi_ckja");
            res.Add("soa_adnc");
            res.Add("wod_cmcs");
            res.Add("kds_dsns");
            res.Add("vnq_isxm");

            res.Add("evolution");
            res.Add("xalipxasm");
            res.Add("feipofwls");
            res.Add("asdmvnkds");
            res.Add("ewmchysow");
            res.Add("czxjcnlck");
            res.Add("oqwieqocc");
            res.Add("auxcnnmxs");
            res.Add("skcguhsvz");
            res.Add("zzjcsumdh");

            res.Add("@1B2c3D4e5F6g7H8");
            res.Add("!sf2SDl9j2@d8GdC");
            res.Add("D@kd9KMl87asdxE3");
            res.Add("d&@D9lg5n6F1!iB8");
            res.Add("18wegaSd@saf!fjf");
            res.Add("9fENs7D!cfnCsdu2");
            res.Add("casD7Sn@!nc@mc2S");
            res.Add("Snc@mc!B26dFm1N1");
            res.Add("cCbjsY2@Ksb72D68");
            res.Add("c2!mNdO8D52d97Gc");

            return res;
        }

        internal static string Encrypt(string plainText,
                                       string passPhrase,
                                       string saltValue,
                                       string hashAlgorithm,
                                       int passwordIterations,
                                       string initVector,
                                       int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize/8);

            var symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream,
                                                encryptor,
                                                CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }


        internal static string Decrypt(string cipherText,
                                       string passPhrase,
                                       string saltValue,
                                       string hashAlgorithm,
                                       int passwordIterations,
                                       string initVector,
                                       int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);


            var password = new PasswordDeriveBytes(
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations);


            byte[] keyBytes = password.GetBytes(keySize/8);

            var symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                keyBytes,
                initVectorBytes);

            var memoryStream = new MemoryStream(cipherTextBytes);

            var cryptoStream = new CryptoStream(memoryStream,
                                                decryptor,
                                                CryptoStreamMode.Read);

            var plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();


            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);
  
            return plainText;
        }

        internal static string Encrypt(string input)
        {
            int a, b, c;
            var r = new Random();
            a = r.Next(9);
            b = r.Next(9);
            c = r.Next(9);
            return "" + b + Encrypt(input, text[a], text[b + 10], "SHA1", 2, text[c + 20], 256) + a + c;
        }

        internal static string Decrypt(string input)
        {
            int a, b, c;
            try
            {
                a = Int32.Parse(input.Substring(input.Length - 2, 1));
                b = Int32.Parse(input.Substring(0, 1));
                c = Int32.Parse(input.Substring(input.Length - 1, 1));

                return Decrypt(input.Substring(1, input.Length - 3), text[a], text[b + 10], "SHA1", 2, text[c + 20], 256);
            }
            catch (Exception e)
            {
                throw new Exception("Could not decrypt relogging information");
            }
            //return Encryptor.Decrypt(input, "lazy_evo", "evolution", "SHA1", 2, "@1B2c3D4e5F6g7H8", 256);
        }
    }
}