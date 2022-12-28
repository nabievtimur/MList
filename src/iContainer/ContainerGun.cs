using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    internal class ContainerGun : iConteiner
    {
        public string brand;
        public string series;
        public long number;
        public string ammo;

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
        public ContainerGun(SqliteDataReader reader) : base(reader)
        {
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
        public override void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@brand", this.brand));
            parameterCollection.Add(new SqliteParameter("@series", this.series));
            parameterCollection.Add(new SqliteParameter("@number", this.number));
            parameterCollection.Add(new SqliteParameter("@ammo", this.ammo));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[0].Value = this.brand;
            row.Cells[1].Value = this.series;
            row.Cells[2].Value = this.number;
            row.Cells[3].Value = this.ammo;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
