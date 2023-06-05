using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
{
    public class ContainerMList : iContainer
    {
        // Поля
        private long numberMlist { get; set; }
        private long employeeID { get; set; }
        private string employeeFullName { get; set; }
        private long dateCreate { get; set; }
        private long dateBegin { get; set; }
        private long dateEnd { get; set; }
        private long dateCoach { get; set; }
        private long datePassGun { get; set; }
        private long datePrint { get; set; }
        private string notes { get; set; }
        // Конструкторы
        public ContainerMList() : base()
        {
            this.numberMlist = 0;
            this.employeeID = -1;
            this.employeeFullName = "";
            this.dateCreate = 0;
            this.dateBegin = 0;
            this.dateEnd = 0;
            this.dateCoach = 0;
            this.datePassGun = 0;
            this.datePrint = 0;
            this.notes = "";
        }
        public ContainerMList(
            long id,
            long numberMlist,
            long employeeID,
            string employeeFullName,
            long dateCreate,
            long dateBegin,
            long dateEnd,
            long dateCoach,
            long datePassGun,
            long datePrint,
            string notes) : base(id)
        {
            this.numberMlist = numberMlist;
            this.employeeID = employeeID;
            this.employeeFullName = employeeFullName;
            this.dateCreate = dateCreate;
            this.dateBegin = dateBegin;
            this.dateEnd = dateEnd;
            this.dateCoach = dateCoach;
            this.datePassGun = datePassGun;
            this.datePrint = datePrint;
            this.notes = notes;
        }
        public ContainerMList(DataGridViewRow row) : base(row)
        {
            try
            {
                this.numberMlist = getLongFromCell(row.Cells[1]);
                this.employeeID = getLongFromCell(row.Cells[2]);
                this.employeeFullName = getStringFromCell(row.Cells[3]);
                this.dateCreate = getDateFromCell(row.Cells[4]);
                this.dateBegin = getDateTimeFromCell(row.Cells[5], row.Cells[6]);
                this.dateEnd = getDateTimeFromCell(row.Cells[7],row.Cells[8]);
                this.dateCoach = getTimeFromCell(row.Cells[9]);
                this.datePassGun = getDateTimeFromCell(row.Cells[10],row.Cells[11]);
                this.datePrint = getDateFromCell(row.Cells[12]);
                this.notes = getStringFromCell(row.Cells[13]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        
        public long getDateCreate() { return this.dateCreate; }
        public long getDateBegin() { return this.dateBegin; }
        public long getDateEnd() { return this.dateEnd; }
        public long getDateCoach() { return this.dateCoach; }
        public long getDatePassGun() { return this.datePassGun; }
        public long getDatePrint() { return this.datePrint; }
        public string getNotes() { return this.notes; }
        public long getNumberMlist() { return this.numberMlist; }
        public long getEmployeeID() { return this.employeeID; }
        public string getEmployeeFullName() { return this.employeeFullName; }

        public override void storageFill(SqliteDataReader reader)
        {
            base.storageFill(reader);
            try
            {
                this.numberMlist = reader.GetInt64(1);
                this.dateCreate = reader.GetInt64(2);
                this.dateBegin = reader.GetInt64(3);
                this.dateEnd = reader.GetInt64(4);
                this.dateCoach = reader.GetInt64(5);
                this.datePassGun = reader.GetInt64(6);
                this.datePrint = reader.GetInt64(7);
                this.notes = reader.GetString(8);
                this.employeeID = reader.GetInt64(9);
                this.employeeFullName = string.Format(
                        "{0} {1} {2}",
                        reader.GetString(10),
                        reader.GetString(11),
                        reader.GetString(12));
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            // TODO Ваня Check
            command.Parameters.Add(new SqliteParameter("@num_mlist", this.numberMlist));
            command.Parameters.Add(new SqliteParameter("@date_create", this.dateCreate));
            command.Parameters.Add(new SqliteParameter("@date_begin", this.dateBegin));
            command.Parameters.Add(new SqliteParameter("@end_date", this.dateEnd));
            command.Parameters.Add(new SqliteParameter("@coach_date", this.dateCoach));
            command.Parameters.Add(new SqliteParameter("@pass_gun_date", this.datePassGun));
            command.Parameters.Add(new SqliteParameter("@print_date", this.datePrint));
            command.Parameters.Add(new SqliteParameter("@notes", this.notes));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.numberMlist;
            row.Cells[2].Value = this.employeeID;
            row.Cells[3].Value = this.employeeFullName;
            row.Cells[4].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateCreate).LocalDateTime.ToString("dd.MM.yyyy");
            row.Cells[5].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateBegin).LocalDateTime.ToString("dd.MM.yyyy");
            row.Cells[6].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateBegin).LocalDateTime.ToString("HH:mm:ss");
            row.Cells[7].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateEnd).LocalDateTime.ToString("dd.MM.yyyy");
            row.Cells[8].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateEnd).LocalDateTime.ToString("HH:mm:ss");
            row.Cells[9].Value = DateTimeOffset.FromUnixTimeSeconds(this.dateCoach).LocalDateTime.ToString("HH:mm:ss");
            row.Cells[10].Value = DateTimeOffset.FromUnixTimeSeconds(this.datePassGun).LocalDateTime.ToString("dd.MM.yyyy");
            row.Cells[11].Value = DateTimeOffset.FromUnixTimeSeconds(this.datePassGun).LocalDateTime.ToString("HH:mm:ss");
            row.Cells[12].Value = DateTimeOffset.FromUnixTimeSeconds(this.datePrint).LocalDateTime.ToString("dd.MM.yyyy");
            row.Cells[13].Value = this.notes;
        }
    }
}
