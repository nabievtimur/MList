using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MList.Storage;

namespace MList
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Authorization.Status.PASSWORD_NOT_EXIXT == Authorization.passwordExist())
            {
                if (Authorization.Status.OK != Authorization.passwordCreate())
                {
                    return;
                }
            }

            var st = SqLiteStorage.getInstance();
            SqLiteStorage.Address add = new SqLiteStorage.Address();
            add.id=5;
            Application.Run(new MainForm());
        }
    }
}
