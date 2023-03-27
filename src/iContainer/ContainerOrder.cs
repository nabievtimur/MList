using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
{
    public class ContainerOrder : iContainer
    {
        private long number { get; set; }
        private long employeeID { get; set; }
        private long date { get; set; }
        private string employeeFullName { get; set; }
        public long getNumber() { return this.number; }
        public long getEmployeeID() { return this.employeeID; }
        public long getDate() { return this.date; }

        public ContainerOrder() : base()
        {
            this.number = 0;
            this.employeeID = -1;
            this.date = 0;
            this.employeeFullName = "";
        }
        public ContainerOrder(
            long id,
            long number,
            long employeeID,
            long date,
            string employeeFullName) : base(id)
        {
            this.number = number;
            this.employeeID = employeeID;
            this.date = date;
            this.employeeFullName = employeeFullName;
        }
        public ContainerOrder(DataGridViewRow row) : base(row)
        {
            try
            {
                this.number = getLongFromCell(row.Cells[1]);
                this.employeeID = getLongFromCell(row.Cells[2]);
                this.date = getDateFromCell(row.Cells[3]);
                this.employeeFullName = getStringFromCell(row.Cells[4]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        public override void storageFill(SqliteDataReader reader)
        {
            base.storageFill(reader);
            try
            {
                this.number = reader.GetInt64(1);
                this.date = reader.GetInt64(2);
                this.employeeID = reader.GetInt64(3);
                this.employeeFullName = string.Format(
                    "{0} {1} {2}",
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6));
        }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@number", this.number));
            command.Parameters.Add(new SqliteParameter("@employee_id", this.employeeID));
            command.Parameters.Add(new SqliteParameter("@date", this.date));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.number;
            row.Cells[2].Value = this.employeeID;
            row.Cells[3].Value = new DateTime(this.date).ToString();
            row.Cells[4].Value = this.employeeFullName;
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
