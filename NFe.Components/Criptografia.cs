using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace NFe.Components
{
    public static class Criptografia
    {
        private static string _chave = "unimake_uninfe";

        public static string criptografaSenha(string senhaCripto)
        {
            try
            {
                return criptografaSenha(senhaCripto, _chave);
            }
            catch(Exception ex)
            {
                return "String errada. " + ex.Message;
            }

        }

        public static string descriptografaSenha(string senhaDescripto)
        {
            try
            {
                return descriptografaSenha(senhaDescripto, _chave);
            }
            catch(Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        public static string criptografaSenha(string senhaCripto, string chave)
        {
            try
            {
                TripleDESCryptoServiceProvider objcriptografaSenha = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objcriptoMd5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = chave;

                byteHash = objcriptoMd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objcriptoMd5 = null;
                objcriptografaSenha.Key = byteHash;
                objcriptografaSenha.Mode = CipherMode.ECB;

                byteBuff = ASCIIEncoding.ASCII.GetBytes(senhaCripto);
                return Convert.ToBase64String(objcriptografaSenha.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch(Exception ex)
            {
                return "Digite os valores Corretamente." + ex.Message;
            }
        }

        public static string descriptografaSenha(string strCriptografada, string chave)
        {
            try
            {
                TripleDESCryptoServiceProvider objdescriptografaSenha = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objcriptoMd5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = chave;

                byteHash = objcriptoMd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objcriptoMd5 = null;
                objdescriptografaSenha.Key = byteHash;
                objdescriptografaSenha.Mode = CipherMode.ECB;

                byteBuff = Convert.FromBase64String(strCriptografada);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objdescriptografaSenha.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objdescriptografaSenha = null;

                return strDecrypted;
            }
            catch(Exception ex)
            {
                return "Digite os valores Corretamente." + ex.Message;
            }
        }

        public static bool compararStrings(string num01, string num02)
        {
            bool stringValor;
            if(num01.Equals(num02))
            {
                stringValor = true;
            }
            else
            {
                stringValor = false;
            }
            return stringValor;
        }

        /// <summary>
        /// Encripta uma string em SHA1 e assina com RSA
        /// </summary>
        /// <param name="value">string a ser criptografada</param>
        /// <returns></returns>
        public static string SignWithRSASHA1(string value)
        {
            string result = "";
            //Converta a cadeia de caracteres ASCII para bytes. 
            byte[] bytes = Encoding.ASCII.GetBytes(value);

            //Gere o HASH (array de bytes) utilizando SHA1. 
            byte[] hashData = new SHA1Managed().ComputeHash(bytes);

            //Assine o HASH (array de bytes) utilizando RSA-SHA1.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] signature = rsa.SignHash(hashData, CryptoConfig.MapNameToOID("SHA1"));

            //converte para uma string base 64 e ...
            result = Convert.ToBase64String(signature);

            //... retorna
            return result;
        }
    }
}