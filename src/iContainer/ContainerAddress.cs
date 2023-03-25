using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
{
    public class ContainerAddress : iContainer
    {
        private string address { get; set; }

        public ContainerAddress() : base()
        {
            this.address = "";
        }
        public ContainerAddress(long id, string address) : base(id)
        {
            this.address = address;
        }
        public ContainerAddress(DataGridViewRow row) : base(row)
        {
            try
            {
                this.address = getStringFromCell(row.Cells[1]);
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
                this.address = reader.GetString(1);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@address", this.address));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.address;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
