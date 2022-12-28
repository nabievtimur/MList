using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    internal class ContainerCar : iConteiner
    {
        public string brand;
        public string number;

        public ContainerCar() : base()
        {
            this.brand = "";
            this.number = "";
        }
        public ContainerCar(long id, string brand, string number) : base(id)
        {
            this.brand = brand;
            this.number = number;
        }
        public ContainerCar(DataGridViewRow row) : base(row)
        {
            try
            {
                this.brand = getStringFromCell(row.Cells[1]);
                this.number = getStringFromCell(row.Cells[2]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        public ContainerCar(SqliteDataReader reader) : base(reader)
        {
            try
            {
                this.brand = reader.GetString(1);
                this.number = reader.GetString(2);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@brand", this.brand));
            parameterCollection.Add(new SqliteParameter("@number", this.number));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[0].Value = this.brand;
            row.Cells[1].Value = this.number;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
