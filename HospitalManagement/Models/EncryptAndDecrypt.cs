﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.UI;

namespace HospitalManagement.Models
{
    public class EncryptAndDecrypt
    {
        public static string EncryptedKey = "b14ca5898a4e4133bbce2ea2315a1916";
        public static string EncryptData(string textData, string Encryptionkey)
        {
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                //set the mode for operation of the algorithm
                objrij.Mode = CipherMode.CBC;
                //set the padding mode used in the algorithm.
                objrij.Padding = PaddingMode.PKCS7;
                //set the size, in bits, for the secret key.
                objrij.KeySize = 0x80;
                //set the block size in bits for the cryptographic operation.
                objrij.BlockSize = 0x80;
                //set the symmetric key that is used for encryption & decryption.
                byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
                //set the initialization vector (IV) for the symmetric algorithm
                byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                int len = passBytes.Length;
                if (len > EncryptionkeyBytes.Length)
                {
                    len = EncryptionkeyBytes.Length;
                }
                Array.Copy(passBytes, EncryptionkeyBytes, len);
                objrij.Key = EncryptionkeyBytes;
                objrij.IV = EncryptionkeyBytes;
                //Creates symmetric AES object with the current key and initialization vector IV.
                ICryptoTransform objtransform = objrij.CreateEncryptor();
                byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
                //Final transform the test string.
                return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string DecryptData(string EncryptedText, string Encryptionkey)
        {
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.CBC;
                objrij.Padding = PaddingMode.PKCS7;
                objrij.KeySize = 0x80;
                objrij.BlockSize = 0x80;
                byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
                byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
                byte[] EncryptionkeyBytes = new byte[0x10];
                int len = passBytes.Length;
                if (len > EncryptionkeyBytes.Length)
                {
                    len = EncryptionkeyBytes.Length;
                }
                Array.Copy(passBytes, EncryptionkeyBytes, len);
                objrij.Key = EncryptionkeyBytes;
                objrij.IV = EncryptionkeyBytes;
                byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
                return Encoding.UTF8.GetString(TextByte);
                //it will return readable string
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}