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
        public static void print(
            ContainerMList mlist, 
            List<ContainerAddress> deepAddresses,
            List<ContainerAddress> arriveAddresses,
            List<ContainerCar> cars,
            List<ContainerGun> guns)
        {
            long mlistNum = mlist.getNumberMlist();

            DateTime dateTimeBegin = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateBegin()).LocalDateTime;
            DateTime dateTimeEnd = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateEnd()).LocalDateTime;
            DateTime dateTimeCoach = DateTimeOffset.FromUnixTimeSeconds(mlist.getDateCoach()).LocalDateTime;
            DateTime dateTimePassGun = DateTimeOffset.FromUnixTimeSeconds(mlist.getDatePassGun()).LocalDateTime;
            
            string dateBegin = dateTimeBegin.ToString("dd-MM-yyyy");
            string timeBegin = dateTimeBegin.ToString("hh:mm:ss");
            string dateEnd = dateTimeEnd.ToString("dd-MM-yyyy");
            string timeEnd = dateTimeEnd.ToString("hh:mm:ss");
            string dateCoach = dateTimeCoach.ToString("dd-MM-yyyy");
            string timeCoach = dateTimeCoach.ToString("hh:mm:ss");
            string datePassGun = dateTimePassGun.ToString("dd-MM-yyyy");
            string timePassGun = dateTimePassGun.ToString("hh:mm:ss");

            ContainerCar[] carsArr = cars.ToArray();
            ContainerGun[] gunsArr = guns.ToArray();
            
            ContainerAddress[] deepAddressesArr = deepAddresses.ToArray();
            ContainerAddress[] arriveAddressesArr = arriveAddresses.ToArray();
            
            List<TemplateEngine.Docx.IContentItem> contents = new List<IContentItem>();
            contents.Add(new FieldContent("Mlist_Num", mlist.getNumberMlist().ToString()));
            contents.Add(new FieldContent("##ATIME##", timeBegin));
            contents.Add(new FieldContent("##ADATE##", dateBegin));
            contents.Add(new FieldContent("##DTIME##", timeEnd));
            contents.Add(new FieldContent("##DDATE##", dateEnd));
            contents.Add(new FieldContent("##CDATE##", $"{dateCoach} в {timeCoach}"));
            
            
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
            
            contents.Add(new FieldContent("##ODATE##", datePassGun));
            contents.Add(new FieldContent("##OTIME##", timePassGun));
            var valuesToFill = new Content(contents.ToArray());
            
            File.Delete(@"F:\c_sharp_projects\mlist_word\Output.docx");
            File.Copy(@"F:\c_sharp_projects\mlist_word\Template2.docx", @"F:\c_sharp_projects\mlist_word\Output.docx");
            using (var outputDocument = new TemplateProcessor(@"F:\c_sharp_projects\mlist_word\Output.docx")
                       .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            openWord(@"F:\c_sharp_projects\mlist_word\Output.docx");
        }

        // private static void openWord(string filepath)
        // {
        //     Word.ApplicationClass WordApp = new Word.ApplicationClass();
        //
        //     object fileName = filepath;
        //     object readOnly = false;
        //     object isVisible = true;
        //     object missing = System.Reflection.Missing.Value;
        //     WordApp.Visible = true;
        //     Word.Document aDoc = WordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing,
        //         ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
        //         ref isVisible);
        //     aDoc.Activate();
        // }
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