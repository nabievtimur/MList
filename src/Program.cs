using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MList.Storage;
using MList.Storage.Table.Container;

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
            
            try
            {
                SqLite.getInstance();
            }
            catch(Exception)
            {
                MessageBox.Show(
                    "Ошибка подключения к базе данных обратитесь к разработчикам.",
                    "Ошибка БД",
                    MessageBoxButtons.OK);
            }
            
            Application.Run(new MainForm());
            
            
            // ContainerMList mlist = new ContainerMList(
            //     id: 1L,
            //     numberMlist: 100L,
            //     employeeID: 200L,
            //     employeeFullName: "John Doe",
            //     dateCreate: 20230601L,
            //     dateBegin: 20230602L,
            //     dateEnd: 20230603L,
            //     dateCoach: 20230604L,
            //     datePassGun: 20230605L,
            //     datePrint: 20230606L,
            //     notes: "Test Notes");
            //
            // List<ContainerAddress> deepAddresses = new List<ContainerAddress> {
            //     new ContainerAddress(id: 1L, address: "Deep Address 1"),
            //     new ContainerAddress(id: 2L, address: "Deep Address 2")
            // };
            //
            // List<ContainerAddress> arriveAddresses = new List<ContainerAddress> {
            //     new ContainerAddress(id: 3L, address: "Arrive Address 1"),
            //     new ContainerAddress(id: 4L, address: "Arrive Address 2")
            // };
            //
            // List<ContainerCar> cars = new List<ContainerCar> {
            //     new ContainerCar(id: 1L, brand: "Brand1", number: "Number1"),
            //     new ContainerCar(id: 2L, brand: "Brand2", number: "Number2")
            // };
            //
            // List<ContainerGun> guns = new List<ContainerGun> {
            //     new ContainerGun(id: 1L, brand: "Brand1", series: "Series1", number: 100L, ammo: "Ammo1"),
            //     new ContainerGun(id: 2L, brand: "Brand2", series: "Series2", number: 200L, ammo: "Ammo2")
            // };
            //
            // Printer.MsWord.print(mlist, deepAddresses, arriveAddresses, cars, guns);
        }
    }
}
