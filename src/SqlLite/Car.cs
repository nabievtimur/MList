using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Car : iConteiner
    {
        public string brand;
        public string number;
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
                label.Text = "Номер";
                TextBox textBox = new TextBox();
                textBox.Text = this.number;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }
        }
        static private List<Car> Read(SqliteDataReader reader)
        {
            List<Car> cars = new List<Car>();

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    cars.Add(new Car()
                    {
                        id = reader.GetInt64(0),
                    });
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw new QueryExeption();
            }
            return cars;
        }
        static public List<Car> Get()
        {
            return Car.Read(SqLite.execGet(
                "SELECT c.id, c.brand, c.\"number\"  FROM cars c ",
                new List<SqliteParameter>(),
                "Read cars."));
        }
        static public List<Car> Get(string search)
        {
            return Car.Read(SqLite.execGet(
                "SELECT id, brand, number FROM cars as cr WHERE cr.brand LIKE @like OR cr.number LIKE @like",
                new List<SqliteParameter> { new SqliteParameter("@like", "%" + search + "%") },
                "Search cars."));
        }
        static public void Add(Car car)
        {
            SqLite.exec(
                "INSERT INTO cars (brand, number) VALUES (@brand, @number)",
                car.getByParametrList(),
                "Add new adress.");
        }
        static public void Update(Car car)
        {
            SqLite.exec(
                "UPDATE cars SET brand = @brand, number = @number WHERE id = @id",
                car.getByParametrListWithId(),
                "Update address.");
        }
        static public void Delete(Car car)
        {
            SqLite.Delete("cars", car.id);
        }
        static public List<Car> GetCurrent(MList mlist)
        {
            return Car.Read(SqLite.execGet(
                "SELECT cr.id, cr.brand, cr.number FROM cars AS cr " +
                    "JOIN mlist_cars mc ON cr.id = mc.car_id WHERE mc.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
                "Read MList cars"));
        }
        static public void initTable(DataGridView table)
        {
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("number", "Номер");
        }
    }
}
