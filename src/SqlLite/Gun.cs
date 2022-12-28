using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Gun : iConteiner
    {
        public string brand;
        public string series;
        public long number;
        public string ammo;
        public override List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                    new SqliteParameter("@brand", this.brand),
                    new SqliteParameter("@series", this.series),
                    new SqliteParameter("@number", this.number),
                    new SqliteParameter("@ammo", this.ammo) };
        }
        public override List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
        }
        public override DataGridViewRow fillRow(DataGridViewRow row)
        {
            return row;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            {
                Label label = new Label();
                label.Text = "Брэнд";
                TextBox textBox = new TextBox();
                textBox.Text = this.brand;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }

            {
                Label label = new Label();
                label.Text = "Серия";
                TextBox textBox = new TextBox();
                textBox.Text = this.series;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }

            {
                Label label = new Label();
                label.Text = "Номер";
                TextBox textBox = new TextBox();
                textBox.Text = this.number.ToString();
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }

            {
                Label label = new Label();
                label.Text = "Патроны";
                TextBox textBox = new TextBox();
                textBox.Text = this.ammo;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }
        }
        static private List<Gun> Read(SqliteDataReader reader)
        {
            List<Gun> guns = new List<Gun>();

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    guns.Add(new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        series = reader.GetString(2),
                        number = reader.GetInt64(3),
                        ammo = reader.GetString(4)
                    });
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw new QueryExeption();
            }

            return guns;
        }
        static public List<Gun> Get()
        {
            return Gun.Read(SqLite.execGet(
                "SELECT id, brand, series, number, ammo FROM guns",
                new List<SqliteParameter>(),
                "Read guns."));
        }
        static public List<Gun> Get(string search)
        {
            return Gun.Read(SqLite.execGet(
                "SELECT g.id, g.brand, g.series, g.number, g.ammo FROM guns g " +
                    "WHERE g.brand LIKE @like OR g.series LIKE @like OR g.\"number\" LIKE @like or g.ammo LIKE @like " +
                    "ORDER BY g.brand;",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", "%" + search + "%")},
                "Read guns."));
        }
        static public List<Gun> Get(Gun gun)
        {
            return Gun.Read(SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM guns AS gn" +
                    "JOIN order_gun og ON gn.id = og.gun_id" +
                    "JOIN orders o ON o.id = og.order_id" +
                    "JOIN employees e ON e.id = o.employee_id" +
                    "WHERE e.id = @emp_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@emp_id", gun.id )},
                "Read guns by order."));
        }
        static public List<Gun> GetCurrent(Order order)
        {
            return Gun.Read(SqLite.execGet(
                "SELECT gn.id, gn.number, gn.ammo, gn.series, gn.brand FROM guns AS gn " +
                    "JOIN order_gun og ON gn.id = og.gun_id WHERE og.order_id = @order_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@order_id", order.id)},
                "Read guns by order."));
        }
        static public void Add(Gun gun)
        {
            SqLite.exec(
                "INSERT INTO guns (brand, series, number, ammo) " +
                    "VALUES (@brand, @series, @number, @ammo)",
                gun.getByParametrList(),
                "Add new gun.");
        }
        
        static public void Update(Gun gun)
        {
            SqLite.exec(
                "UPDATE guns SET brand = @brand, series = @series, number = @number, ammo = @ammo WHERE id = @id",
                gun.getByParametrListWithId(),
                "Update gun.");
        }
        static public void Delete(Gun gun)
        {
            SqLite.Delete("guns", gun.id);
        }
        static public List<Gun> GetCurrent(MList mlist)
        {
            return Gun.Read(SqLite.execGet(
                "SELECT gn.id, gn.number, gn.brand, gn.series, gn.ammo FROM guns AS gn " +
                    "JOIN mlist_gun mg ON gn.id = mg.gun_id WHERE mg.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
                "Get guns by mlist."));
        }
        static public void initTable(DataGridView table)
        {
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("series", "Серия");
            table.Columns.Add("number", "Номер");
            table.Columns.Add("ammo", "Патроны");
        }
    }
}
