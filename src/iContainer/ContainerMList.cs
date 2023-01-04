using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    internal class ContainerMList : iContainer
    {
        // Поля
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
        // Конструкторы
        public ContainerMList() : base()
        {
            this.dateCreate = 0;
            this.dateBegin = 0;
            this.dateEnd = 0;
            this.dateCoach = 0;
            this.datePassGun = 0;
            this.datePrint = 0;
            this.notes = "";
            this.timeDeep = 0;
            this.timeArrive = 0;
            this.timePassGun = 0;
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
            this.dateEnd = dateEnd;
            this.dateCoach = dateCoach;
            this.datePassGun = datePassGun;
            this.datePrint = datePrint;
            this.notes = notes;
            this.timeDeep = timeDeep;
            this.timeArrive = timeArrive;
            this.timePassGun = timePassGun;
            this.numberMlist = numberMlist;
            this.employeeID = employeeID;
            this.employeeFullName = employeeFullName;
        }
        public ContainerMList(DataGridViewRow row) : base(row)
        {
            try
            {
                this.dateCreate = getLongFromCell(row.Cells[1]);
                this.dateBegin = getLongFromCell(row.Cells[2]);
                this.dateEnd = getLongFromCell(row.Cells[3]);
                this.dateCoach = getLongFromCell(row.Cells[4]);
                this.datePassGun = getLongFromCell(row.Cells[5]);
                this.datePrint = getLongFromCell(row.Cells[6]);
                this.notes = getStringFromCell(row.Cells[7]);
                this.timeDeep = getLongFromCell(row.Cells[8]);
                this.timeArrive = getLongFromCell(row.Cells[9]);
                this.timePassGun = getLongFromCell(row.Cells[10]);
                this.numberMlist = getLongFromCell(row.Cells[11]);
                this.employeeID = getLongFromCell(row.Cells[12]);
                this.employeeFullName = getStringFromCell(row.Cells[13]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        public ContainerMList(SqliteDataReader reader) : base(reader)
        {
            try
            {
                this.dateCreate = reader.GetInt64(2);
                this.dateBegin = reader.GetInt64(3);
                this.dateEnd = reader.GetInt64(4);
                this.dateCoach = reader.GetInt64(5);
                this.datePassGun = reader.GetInt64(6);
                this.datePrint = reader.GetInt64(8);
                this.notes = reader.GetString(9);
                this.timeDeep = reader.GetInt64(11);
                this.timeArrive = reader.GetInt64(10);
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
        public override void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@date_create", this.dateCreate));
            parameterCollection.Add(new SqliteParameter("@date_begin", this.dateBegin));
            parameterCollection.Add(new SqliteParameter("@end_date", this.dateEnd));
            parameterCollection.Add(new SqliteParameter("@coach_date", this.dateCoach));
            parameterCollection.Add(new SqliteParameter("@pass_gun_date", this.datePassGun));
            parameterCollection.Add(new SqliteParameter("@print_date", this.datePrint));
            parameterCollection.Add(new SqliteParameter("@notes", this.notes));
            parameterCollection.Add(new SqliteParameter("@deep_time", this.timeDeep));
            parameterCollection.Add(new SqliteParameter("@arrive_time", this.timeArrive));
            parameterCollection.Add(new SqliteParameter("@pass_gun_time", this.timePassGun));
            parameterCollection.Add(new SqliteParameter("@num_mlist", this.numberMlist));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[0].Value = this.numberMlist;
            row.Cells[1].Value = this.employeeFullName;
            row.Cells[2].Value = new DateTime(this.dateCreate).Date.ToString();
            row.Cells[3].Value = new DateTime(this.dateBegin).Date.ToString();
            row.Cells[4].Value = new DateTime(this.dateCreate).ToLocalTime().ToString();
            row.Cells[5].Value = new DateTime(this.dateEnd).Date.ToString();
            row.Cells[6].Value = new DateTime(this.dateEnd).ToLocalTime().ToString();
            row.Cells[7].Value = new DateTime(this.dateCoach).ToLocalTime().ToString();
            row.Cells[8].Value = new DateTime(this.datePassGun).Date.ToString();
            row.Cells[9].Value = new DateTime(this.datePassGun).ToLocalTime().ToString();
            row.Cells[10].Value = new DateTime(this.datePrint).Date.ToString();
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
