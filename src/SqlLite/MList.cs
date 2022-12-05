using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class MList
    {
        public long id;
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
        public List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                new SqliteParameter("@date_create", this.dateCreate),
                new SqliteParameter("@date_begin", this.dateBegin),
                new SqliteParameter("@end_date", this.dateEnd),
                new SqliteParameter("@coach_date", this.dateCoach),
                new SqliteParameter("@pass_gun_date", this.datePassGun),
                new SqliteParameter("@print_date", this.datePrint),
                new SqliteParameter("@notes", this.notes),
                new SqliteParameter("@deep_time", this.timeDeep),
                new SqliteParameter("@arrive_time", this.timeArrive),
                new SqliteParameter("@pass_gun_time", this.timePassGun),
                new SqliteParameter("@num_mlist", this.numberMlist) };
        }
        public List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
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
    }
}
