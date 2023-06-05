using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MList.Storage.Table.Container;
using Word = Microsoft.Office.Interop.Word;
using TemplateEngine.Docx;

namespace MList.Printer
{
    public class MsWord
    {
        [Serializable]
        public class ConnectionExeption : Exception
        {
            public ConnectionExeption() { }
            public ConnectionExeption(string message)
                : base(message) { }
        }
        private static string templateWordFilePath = Path.Combine(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "MList\\WordTemplate"), "Template.docx");
        public static void print(
            ContainerMList mlist, 
            List<ContainerAddress> deepAddresses,
            List<ContainerAddress> arriveAddresses,
            List<ContainerCar> cars,
            List<ContainerGun> guns)
        {   
            DateTime timeNow = DateTime.Now.ToLocalTime();
            DateTime dateTimeBegin = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateBegin()).LocalDateTime;
            DateTime dateTimeEnd = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateEnd()).LocalDateTime;
            DateTime dateTimeCoach = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateCoach()).LocalDateTime;
            DateTime dateTimePassGun = DateTimeOffset.FromUnixTimeSeconds(mlist.getDatePassGun()).LocalDateTime;

            ContainerCar[] carsArr = cars.ToArray();
            ContainerGun[] gunsArr = guns.ToArray();
            
            ContainerAddress[] deepAddressesArr = deepAddresses.ToArray();
            ContainerAddress[] arriveAddressesArr = arriveAddresses.ToArray();
            
            List<TemplateEngine.Docx.IContentItem> contents = new List<IContentItem>();
            contents.Add(new FieldContent("Mlist_Num", mlist.getNumberMlist().ToString()));
            contents.Add(new FieldContent("##ATIME##", dateTimeBegin.ToString("HH:mm:ss")));
            contents.Add(new FieldContent("##ADATE##", dateTimeBegin.ToString("dd-MM-yyyy")));
            contents.Add(new FieldContent("##DTIME##", dateTimeEnd.ToString("HH:mm:ss")));
            contents.Add(new FieldContent("##DDATE##", dateTimeEnd.ToString("dd-MM-yyyy")));
            contents.Add(new FieldContent("##CDATE##", $"{dateTimeCoach.ToString("dd-MM-yyyy")} в {dateTimeCoach.ToString("dd-MM-yyyy")}"));
            
            
            string groupTitle = "Состав группы:";
            TemplateEngine.Docx.TableContent groupTable = new TableContent("##GROUP##");
            groupTable.AddRow(
                new FieldContent("##GROUP_TITLE##", groupTitle),
                new FieldContent("##PNAME##",mlist.getEmployeeFullName()));
            contents.Add(groupTable);
            
            TemplateEngine.Docx.TableContent gunsTable = new TableContent("##GUNS##");
            for (var i = 0; i < gunsArr.Length; i++)
            {
                string gunsTitle = "";
                if (i == 0)
                {
                    gunsTitle = "Вооружение:";
                }

                gunsTable.AddRow(
                    new FieldContent("##GUNS_TITLE##", gunsTitle),
                    new FieldContent("##GNAME##",$"{gunsArr[i].getSeries()} {gunsArr[i].getBrand()} {gunsArr[i].getAmmo()}"));
            }
            contents.Add(gunsTable);
            
            TemplateEngine.Docx.TableContent autoTable = new TableContent("##CARS##");
            for (var i = 0; i < carsArr.Length; i++)
            {
                string autoTitle = "";
                if (i == 0)
                {
                    autoTitle = "Автомашина:";
                }

                autoTable.AddRow(
                    new FieldContent("##CARS_TITLE##", autoTitle),
                    new FieldContent("##ANAME##",$"{carsArr[i].getBrand()} {carsArr[i].getNumber()}"));
            }
            contents.Add(autoTable);
            
            TemplateEngine.Docx.TableContent addrTable = new TableContent("Addresses");
            for (var i = 0; i < deepAddressesArr.Length; i++)
            {
                string addrTitle = "";
                if (i == 0)
                {
                    addrTitle = "Адреса:";
                }

                addrTable.AddRow(
                    new FieldContent("Deep_Address", deepAddressesArr[i].getAddress()),
                    new FieldContent("Deep_Time", ""),
                    new FieldContent("Arrive_Address", arriveAddressesArr[i].getAddress()),
                    new FieldContent("Arrive_Time", ""));
            }
            contents.Add(addrTable);
            
            contents.Add(new FieldContent("##ODATE##", dateTimePassGun.ToString("dd-MM-yyyy")));
            contents.Add(new FieldContent("##OTIME##", dateTimePassGun.ToString("hh:mm:ss")));
            var valuesToFill = new Content(contents.ToArray());

            string newFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Маршрутные листы");
            if (!Directory.Exists(newFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(newFolderPath);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
            }
            
            string newFilePath = Path.Combine(newFolderPath, $"{mlist.getNumberMlist().ToString()}. {timeNow.ToString("yyyy-MM-dd hh_mm_ss")}.docx");
            
            File.Copy(templateWordFilePath, newFilePath);
            using (var outputDocument = new TemplateProcessor(newFilePath)
                       .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            openWord(newFilePath);
        }
        
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void openWord(string filepath)
        {
            Word.Application WordApp = new Word.Application();

            object fileName = filepath;
            object readOnly = false;
            object isVisible = true;
            object missing = System.Reflection.Missing.Value;
            WordApp.Visible = true;
            Word.Document aDoc = WordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref isVisible);
            aDoc.Activate();

            SetForegroundWindow(new IntPtr(WordApp.ActiveWindow.Hwnd));
        }
    }
}