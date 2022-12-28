using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class MList : iConteiner
    {
        public long dateCreate;
        public long dateBegin;
        public long dateEnd;
        public long dateCoach;
        public long datePassGun;
        public long datePrint;
        public string notes;
        public long timeDeep;
        public long timeArrive;
        public long timePassGun;
        public long numberMlist;
        public long employeeID;
        public string employeeFullName;
        public override DataGridViewRow fillRow(DataGridViewRow row)
        {

            return row;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
        static public List<MList> Get()
        {
            List<MList> mlists = new List<MList>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT ml.id, " +
                    "ml.num_mlist, " +
                    "ml.date_create, " +
                    "ml.date_begin, " +
                    "ml.end_date, " +
                    "ml.coach_date, " +
                    "ml.pass_gun_date, " +
                    "ml.pass_gun_time, " +
                    "ml.print_date, " +
                    "ml.notes, " +
                    "ml.arrive_time, " +
                    "ml.deep_time, " +
                    "e.id, " +
                    "e.last_name, " +
                    "e.first_name, " +
                    "e.middle_name " +
                    "FROM mlist AS ml " +
                    "JOIN mlist_employees me ON ml.id = me.mlist_id " +
                    "JOIN employees e ON e.id = me.employee_id",
                new List<SqliteParameter>(),
                "Reads MList.");
            while (reader.Read()) // построчно считываем данные
            {
                mlists.Add(new MList {
                    id = reader.GetInt64(0),
                    dateCreate = reader.GetInt64(2),
                    dateBegin = reader.GetInt64(3),
                    dateEnd = reader.GetInt64(4),
                    dateCoach = reader.GetInt64(5),
                    datePassGun = reader.GetInt64(6),
                    datePrint = reader.GetInt64(8),
                    notes = reader.GetString(9),
                    timeDeep = reader.GetInt64(11),
                    timeArrive = reader.GetInt64(10),
                    timePassGun = reader.GetInt64(7),
                    numberMlist = reader.GetInt64(1),
                    employeeID = reader.GetInt64(12),
                    employeeFullName = string.Format(
                        "{0} {1} {2}",
                        reader.GetString(13),
                        reader.GetString(14),
                        reader.GetString(15)) } );
            }
            reader.Close();

            return mlists;
        }
        public void Add(MList mlist)
        {
            SqLite.exec(
                "INSERT INTO mlist (date_create, date_begin, end_date, coach_date, pass_gun_date, print_date, notes, deep_time, arrive_time, pass_gun_time, num_mlist)" +
                "VALUES (@date_create, @date_begin, @end_date, @coach_date, @pass_gun_date, @print_date, @notes, @deep_time, @arrive_time, @pass_gun_time, @num_mlist)",
                mlist.getByParametrList(),
                "Add new MLlist");
        }
        public void Update(MList mlist)
        {
            SqLite.exec(
                "UPDATE mlist SET " +
                        "date_create   = @date_create," +
                        "date_begin    = @date_begin," +
                        "end_date      = @end_date," +
                        "coach_date    = @coach_date," +
                        "pass_gun_date = @pass_gun_date," +
                        "print_date    = @print_date," +
                        "notes         = @notes," +
                        "deep_time     = @deep_time," +
                        "arrive_time   = @arrive_time," +
                        "pass_gun_time = @pass_gun_time," +
                        "num_mlist     = @num_mlist" +
                        "where id = @id",
                mlist.getByParametrListWithId(),
                "Update MList");
        }
        static public void Delete(MList mlist)
        {
            SqLite.Delete("mlist", mlist.id);
        }
        static public void initTable(DataGridView table)
        {
            table.Columns.Add("number", "Номер");
            table.Columns.Add("employee", "Сотрудник");
            table.Columns.Add("createDate", "Дата создания");
            table.Columns.Add("createStart", "Дата начала");
            table.Columns.Add("startTime", "Время начала");
            table.Columns.Add("endDate", "Дата окончания");
            table.Columns.Add("endTime", "Время окончания");
            table.Columns.Add("instractionTime", "Время инструктажа");
            table.Columns.Add("returnGunDate", "Дата сдачи оружия");
            table.Columns.Add("returnGunTime", "Время сдачи оружия");
            table.Columns.Add("printDate", "Дата печати");
            table.Columns.Add("description", "Примечание");
        }
    }
}
