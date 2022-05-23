using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace MList
{
    internal class Authorization
    {
        private const string REGISTRY_PASSWORD_KEY = "HKEY_LOCAL_MACHINE\\SECURITY";
        private const string REGISTRY_PASSWORD_VALUE = "MListAdmin";

        public enum Status
        {
            OK,
            PASSWORD_EXIST,
            PASSWORD_NOT_EXIXT,
            PASSWORD_CORRECT,
            PASSWORD_NOT_CORRECT,
            ACCESS_DENIED
        }
        public static Status passwordExist()
        {
            byte[] sPasswordHash = (byte[])Registry.GetValue(
                REGISTRY_PASSWORD_KEY,
                REGISTRY_PASSWORD_VALUE, 
                null);
            if (sPasswordHash == null)
                return Status.PASSWORD_NOT_EXIXT;
            return Status.PASSWORD_EXIST;
        }

        public static Status passwordCreate(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] pbPasswordHash = sha.ComputeHash(Encoding.Unicode.GetBytes(password));
            Registry.SetValue(
                REGISTRY_PASSWORD_KEY,
                REGISTRY_PASSWORD_VALUE,
                pbPasswordHash); // TODO CHECK
            return Status.OK;
        }

        public static Status login(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] pbPasswordHash = sha.ComputeHash(Encoding.Unicode.GetBytes(password));
            byte[] sPasswordHash = (byte[])Registry.GetValue(
                REGISTRY_PASSWORD_KEY,
                REGISTRY_PASSWORD_VALUE,
                null);
            if (pbPasswordHash == sPasswordHash)
                return Status.PASSWORD_CORRECT;
            return Status.PASSWORD_NOT_CORRECT;
        }
    }
}
