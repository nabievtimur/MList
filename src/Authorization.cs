using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Windows.Forms;

namespace MList
{
    internal class Authorization
    {
        private const string REGISTRY_PASSWORD_KEY = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\MList";
        private const string REGISTRY_PASSWORD_VALUE = "Admin";

        public enum Status
        {
            OK,
            ERROR,
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

        public static Status passwordCreate()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                InputBox inputBox = new InputBox("Создание пароля администратора.", "Введите пароль администратора.");
                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    SHA256 sha = SHA256.Create();
                    byte[] pbPasswordHash = sha.ComputeHash(Encoding.Unicode.GetBytes(inputBox.getResult()));
                    Registry.SetValue(
                        REGISTRY_PASSWORD_KEY,
                        REGISTRY_PASSWORD_VALUE,
                        pbPasswordHash);
                    MessageBox.Show(
                        "Пароль успешно задан.",
                        "Успех",
                        MessageBoxButtons.OK);
                    return Status.OK;
                }
                else
                {
                    MessageBox.Show(
                        "Создание пароля завершилось ошибкой.",
                        "Ошибка аунтетификации",
                        MessageBoxButtons.OK);
                    return Status.ERROR;
                }
            }
            MessageBox.Show(
                "Пароль администратора не задан, для задания пароля запустите приложение с правами администратора.",
                "Ошибка аунтетификации",
                MessageBoxButtons.OK);
            return Status.ACCESS_DENIED;
        }

        public static Status login(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] pbPasswordHash = sha.ComputeHash(Encoding.Unicode.GetBytes(password));
            byte[] pbEthalonPasswordHash = (byte[])Registry.GetValue(
                REGISTRY_PASSWORD_KEY,
                REGISTRY_PASSWORD_VALUE,
                null);
            bool correct = true;
            for(int i = 0; i < pbPasswordHash.Length; i++)
            {
                if (pbPasswordHash[i] != pbEthalonPasswordHash[i])
                    correct = false;
            }
            if (correct)
                return Status.PASSWORD_CORRECT;
            return Status.PASSWORD_NOT_CORRECT;
        }
    }
}
