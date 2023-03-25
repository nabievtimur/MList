using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms.VisualStyles;
using MList.Storage;
using Word = Microsoft.Office.Interop.Word;
using TemplateEngine.Docx;

namespace MList.Printer
{
    public class MsWord
    {
        [Serializable]
        public class WordDocumentParserException : Exception
        {
            public WordDocumentParserException()
            {
            }

            public WordDocumentParserException(string message)
                : base(message)
            {
            }
        }

        static void print(
            Storage.Container.MList mlist,
            string[] employees,
            string[] deepAddresses,
            string[] arriveAddresses,
            string[] cars,
            string[] guns,
            long[] deepDateUnix,
            long[] arriveDateUnix)
        {
            if (mlist.numberMlist <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Номер маршрутного листа не может быть меньше или равным 0");
                throw new WordDocumentParserException("Номер маршрутного листа не может быть меньше или равным 0");
            }

            if (mlist.dateBegin == 0)
            {
                System.Diagnostics.Debug.WriteLine("Дата начала маршрутного листа не может быть пустой");
                throw new WordDocumentParserException("Дата начала маршрутного листа не может быть пустой");
            }

            if (mlist.dateEnd == 0)
            {
                System.Diagnostics.Debug.WriteLine("Дата окончания маршрутного листа не может быть пустой");
                throw new WordDocumentParserException("Дата окончания маршрутного листа не может быть пустой");
            }


            DateTime beginDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mlist.dateBegin);
            string beginDate = beginDateTime.ToString("dd:MM:yyyy");
            string beginTime = beginDateTime.ToString("HH:mm");

            DateTime endDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mlist.dateEnd);
            string endDate = endDateTime.ToString("dd:MM:yyyy");
            string endTime = endDateTime.ToString("HH:mm");

            DateTime passGunDateTime =
                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mlist.datePassGun);
            string passGunDate = passGunDateTime.ToString("dd:MM:yyyy");
            string passGunTime = passGunDateTime.ToString("HH:mm");

            DateTime coachDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mlist.dateCoach);
            string coachDT = coachDateTime.ToString("dd:MM:yyyy HH:mm");


            List<TemplateEngine.Docx.IContentItem> contents = new List<IContentItem>();
            contents.Add(new FieldContent("Mlist_Num", mlist.numberMlist.ToString()));
            contents.Add(new FieldContent("##ATIME##", beginTime));
            contents.Add(new FieldContent("##ADATE##", beginDate));
            contents.Add(new FieldContent("##DTIME##", endTime));
            contents.Add(new FieldContent("##DDATE##", endDate));
            contents.Add(new FieldContent("##CDATE##", coachDT));
            contents.Add(new FieldContent("##ODATE##", passGunDate));
            contents.Add(new FieldContent("##OTIME##", passGunTime));



            TemplateEngine.Docx.TableContent groupTable = new TableContent("##GROUP##");
            for (var i = 0; i < employees.Length; i++)
            {
                string groupTitle = "";
                if (i == 0)
                {
                    groupTitle = "Состав группы:";
                }

                groupTable.AddRow(
                    new FieldContent("##GROUP_TITLE##", groupTitle),
                    new FieldContent("##PNAME##", employees[i]));
            }
            contents.Add(groupTable);
            
            TemplateEngine.Docx.TableContent gunsTable = new TableContent("##GUNS##");
            for (var i = 0; i < guns.Length; i++)
            {
                string gunsTitle = "";
                if (i == 0)
                {
                    gunsTitle = "Вооружение:";
                }

                gunsTable.AddRow(
                    new FieldContent("##GUNS_TITLE##", gunsTitle),
                    new FieldContent("##GNAME##",guns[i]));
            }
            contents.Add(gunsTable);
            
            TemplateEngine.Docx.TableContent carsTable = new TableContent("##CARS##");
            for (var i = 0; i < cars.Length; i++)
            {
                string autoTitle = "";
                if (i == 0)
                {
                    autoTitle = "Автомашина:";
                }

                carsTable.AddRow(
                    new FieldContent("##CARS_TITLE##", autoTitle),
                    new FieldContent("##ANAME##",cars[i]));
            }
            contents.Add(carsTable);
            
            TemplateEngine.Docx.TableContent addrTable = new TableContent("Addresses");

            if (deepAddresses.Length != arriveAddresses.Length && arriveAddresses.Length != deepDateUnix.Length && deepDateUnix.Length != arriveDateUnix.Length)
            {
                System.Diagnostics.Debug.WriteLine("количество адресов убытия и прибытия не совпадает со временем убытия и прибытия");
                throw new WordDocumentParserException("количество адресов убытия и прибытия не совпадает со временем убытия и прибытия");
            }
            
            for (var i = 0; i < deepAddresses.Length; i++)
            {
                DateTime deepDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(deepDateUnix[i]);
                string deepTime = deepDateTime.ToString("HH:mm");

                DateTime arriveDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(arriveDateUnix[i]);
                string arriveTime = arriveDateTime.ToString("HH:mm");

                addrTable.AddRow(
                    new FieldContent("Deep_Address", deepAddresses[i]),
                    new FieldContent("Deep_Time", deepTime),
                    new FieldContent("Arrive_Address", arriveAddresses[i]),
                    new FieldContent("Arrive_Time", arriveTime));
            }

            string templateFilePath = @"F:\c_sharp_projects\mlist_word\Output.docx";
            string newDoc = @"F:\c_sharp_projects\mlist_word\" + mlist.numberMlist.ToString() + ". " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ".docx";
            
            File.Delete(newDoc);
            File.Copy(templateFilePath, newDoc);

            var valuesToFill = new Content(contents.ToArray());
            
            using (var outputDocument = new TemplateProcessor(newDoc)
                       .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            openDocument(newDoc);
        }

        private static void openDocument(string filePath)
        {
            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine("File " + "'" + filePath + "' " + "not found");
                throw new WordDocumentParserException("File " + "'" + filePath + "' " + "not found");
            }

            Word.ApplicationClass WordApp = new Word.ApplicationClass();

            object fileName = filePath;
            object readOnly = false;
            object isVisible = true;
            object missing = System.Reflection.Missing.Value;
            WordApp.Visible = true;
            Word.Document aDoc = WordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref isVisible);
            aDoc.Activate();
            aDoc.ActiveWindow.Activate();
        }
    }
}