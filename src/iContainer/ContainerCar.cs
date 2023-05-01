using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
{
    public class ContainerCar : iContainer
    {
        private string brand { get; set; }
        private string number { get; set; }

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
        
        public string getBrand() { return this.brand; }
        public string getNumber() { return this.number; }
        
        public override void storageFill(SqliteDataReader reader)
        {
            base.storageFill(reader);
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
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@brand", this.brand));
            command.Parameters.Add(new SqliteParameter("@number", this.number));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.brand;
            row.Cells[2].Value = this.number;
        }
        public override List<String> getFieldsNames()
        {
            return new List<String>() { "Брэнд", "Номер" };
        }
        public override List<String> getFieldsValues()
        {
            return new List<String>() { this.brand, this.number };
        }
        public override bool checkItemList(ref List<TextBox> lItems)
        {
            return true;
        }
        public override iContainer updateFromList(List<TextBox> lItems)
        {
            if (lItems.Count != 2)
                throw new ParceException();
            this.brand = lItems[0].Text;
            this.number = lItems[1].Text;
            return this;
        }
    }
}
