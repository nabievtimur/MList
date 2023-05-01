using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
{
    public class ContainerGun : iContainer
    {
        private string brand { get; set; }
        private string series { get; set; }
        private long number { get; set; }
        private string ammo { get; set; }

        public ContainerGun() : base()
        {
            this.brand = "";
            this.series = "";
            this.number = 0;
            this.ammo = "";
        }
        public ContainerGun(long id, string brand, string series, long number, string ammo) : base(id)
        {
            this.brand = brand;
            this.series = series;
            this.number = number;
            this.ammo = ammo;
        }
        public ContainerGun(DataGridViewRow row) : base(row)
        {
            try
            {
                this.brand = getStringFromCell(row.Cells[1]);
                this.series = getStringFromCell(row.Cells[2]);
                this.number = getLongFromCell(row.Cells[3]);
                this.ammo = getStringFromCell(row.Cells[3]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        
        public string getBrand() { return this.brand; }
        public string getSeries() { return this.series; }
        public long getNumber() { return this.number; }
        public string getAmmo() { return this.ammo; }

        public override void storageFill(SqliteDataReader reader)
        {
            base.storageFill(reader);
            try
            {
                this.brand = reader.GetString(1);
                this.series = reader.GetString(2);
                this.number = reader.GetInt64(3);
                this.ammo = reader.GetString(4);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@brand", this.brand));
            command.Parameters.Add(new SqliteParameter("@series", this.series));
            command.Parameters.Add(new SqliteParameter("@number", this.number));
            command.Parameters.Add(new SqliteParameter("@ammo", this.ammo));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[1].Value = this.brand;
            row.Cells[2].Value = this.series;
            row.Cells[3].Value = this.number;
            row.Cells[4].Value = this.ammo;
        }

        public override List<String> getFieldsNames()
        {
            return new List<String>() { "Брэнд", "Серия", "Номер", "Патроны" };
        }
        public override List<String> getFieldsValues()
        {
            return new List<String>() { this.brand, this.series, this.number.ToString(), this.ammo };
        }
        public override bool checkItemList(ref List<TextBox> lItems)
        {
            return true; // CHECH 
        }
        public override iContainer updateFromList(List<TextBox> lItems)
        {
            if (lItems.Count != 4)
                throw new ParceException();
            this.brand = lItems[0].Text;
            this.series = lItems[1].Text;
            this.number = long.Parse(lItems[2].Text);
            this.ammo = lItems[3].Text;
            return this;
        }
    }
}
