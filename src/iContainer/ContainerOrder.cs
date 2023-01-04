using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    internal class ContainerOrder : iContainer
    {
        public long number;
        public long employeeID;
        public long date;
        public string employeeFullName;

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
                this.date = getLongFromCell(row.Cells[3]);
                this.employeeFullName = getStringFromCell(row.Cells[4]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        public ContainerOrder(SqliteDataReader reader) : base(reader)
        {
            try
            {
                this.address = reader.GetString(1);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@number", this.number));
            parameterCollection.Add(new SqliteParameter("@employee_id", this.employeeID));
            parameterCollection.Add(new SqliteParameter("@date", this.date));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[0].Value = this.number;
            row.Cells[1].Value = this.date;
            row.Cells[2].Value = this.employeeFullName;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
