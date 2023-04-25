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
        private long dateCreate { get; set; }
        private long dateBegin { get; set; }
        private long dateEnd { get; set; }
        private long dateCoach { get; set; }
        private long datePassGun { get; set; }
        private long datePrint { get; set; }
        private string notes { get; set; }
        private long employeeID { get; set; }
        private string employeeFullName { get; set; }
        // Конструкторы
        public ContainerMList() : base()
        {
            this.dateCreate = 0;
            this.dateBegin = 0;
            this.dateEnd = 0;
            this.dateCoach = 0;
            this.datePassGun = 0;
            this.timePassGun = 0;
            this.datePrint = 0;
            this.notes = "";
            this.numberMlist = 0;
            this.employeeID = -1;
            this.employeeFullName = "";
        }
        public ContainerMList(
            long id,
            long dateCreate,
            long dateBegin,
            long dateEnd,
            long dateCoach,
            long datePassGun,
            long datePrint,
            string notes,
            long timeDeep,
            long timeArrive,
            long timePassGun,
            long numberMlist,
            long employeeID,
            string employeeFullName) : base(id)
        {
            this.dateCreate = dateCreate;
            this.dateBegin = dateBegin;
            this.timeBegin = timeDeep;
            this.dateEnd = dateEnd;
            this.timeEnd = timeArrive;
            this.dateCoach = dateCoach;
            this.datePassGun = datePassGun;
            this.timePassGun = timePassGun;
            this.datePrint = datePrint;
            this.notes = notes;
            this.numberMlist = numberMlist;
            this.employeeID = employeeID;
            this.employeeFullName = employeeFullName;
        }
        public ContainerMList(DataGridViewRow row) : base(row)
        {
            try
            {
                this.numberMlist = getLongFromCell(row.Cells[1]);
                this.employeeID = getLongFromCell(row.Cells[2]);
                this.employeeFullName = getStringFromCell(row.Cells[3]);
                this.dateCreate = getLongFromCell(row.Cells[4]);
                this.dateBegin = getLongFromCell(row.Cells[5]);
                this.timeBegin = getLongFromCell(row.Cells[6]);
                this.dateEnd = getLongFromCell(row.Cells[7]);
                this.timeEnd = getLongFromCell(row.Cells[8]);
                this.dateCoach = getLongFromCell(row.Cells[9]);
                this.datePassGun = getLongFromCell(row.Cells[10]);
                this.timePassGun = getLongFromCell(row.Cells[11]);
                this.datePrint = getLongFromCell(row.Cells[12]);
                this.notes = getStringFromCell(row.Cells[13]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        
        public long getDateCreate() { return this.dateCreate; }
        public long getDateBegin() { return this.dateBegin; }
        public long getTimeBegin() { return this.timeBegin; }
        public long getDateEnd() { return this.dateEnd; }
        public long getTimeEnd() { return this.timeEnd; }
        public long getDateCoach() { return this.dateCoach; }
        public long getDatePassGun() { return this.datePassGun; }
        public long getTimePassGun() { return this.timePassGun; }
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
                this.dateCreate = reader.GetInt64(2);
                this.dateBegin = reader.GetInt64(3);
                this.dateEnd = reader.GetInt64(4);
                this.dateCoach = reader.GetInt64(5);
                this.datePassGun = reader.GetInt64(6);
                this.datePrint = reader.GetInt64(8);
                this.notes = reader.GetString(9);
                this.timePassGun = reader.GetInt64(7);
                this.numberMlist = reader.GetInt64(1);
                this.employeeID = reader.GetInt64(12);
                this.employeeFullName = string.Format(
                        "{0} {1} {2}",
                        reader.GetString(13),
                        reader.GetString(14),
                        reader.GetString(15));
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@date_create", this.dateCreate));
            command.Parameters.Add(new SqliteParameter("@date_begin", this.dateBegin));
            command.Parameters.Add(new SqliteParameter("@end_date", this.dateEnd));
            command.Parameters.Add(new SqliteParameter("@coach_date", this.dateCoach));
            command.Parameters.Add(new SqliteParameter("@pass_gun_date", this.datePassGun));
            command.Parameters.Add(new SqliteParameter("@print_date", this.datePrint));
            command.Parameters.Add(new SqliteParameter("@notes", this.notes));
            command.Parameters.Add(new SqliteParameter("@deep_time", this.timeDeep));
            command.Parameters.Add(new SqliteParameter("@arrive_time", this.timeArrive));
            command.Parameters.Add(new SqliteParameter("@pass_gun_time", this.timePassGun));
            command.Parameters.Add(new SqliteParameter("@num_mlist", this.numberMlist));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.numberMlist;
            row.Cells[2].Value = this.employeeFullName;
            row.Cells[3].Value = new DateTime(this.dateCreate).Date.ToString();
            row.Cells[4].Value = new DateTime(this.dateBegin).Date.ToString();
            row.Cells[5].Value = new DateTime(this.dateCreate).ToLocalTime().ToString();
            row.Cells[6].Value = new DateTime(this.dateEnd).Date.ToString();
            row.Cells[7].Value = new DateTime(this.dateEnd).ToLocalTime().ToString();
            row.Cells[8].Value = new DateTime(this.dateCoach).ToLocalTime().ToString();
            row.Cells[9].Value = new DateTime(this.datePassGun).Date.ToString();
            row.Cells[10].Value = new DateTime(this.datePassGun).ToLocalTime().ToString();
            row.Cells[11].Value = new DateTime(this.datePrint).Date.ToString();
        }
        public override List<Tuple<Label, TextBox>> getItemList()
        {
            throw new NotImplementedException();
        }
        public override bool checkItemList(ref List<Tuple<Label, TextBox>> items)
        {
            throw new NotImplementedException();
        }
    }
}
