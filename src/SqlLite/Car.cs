using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Car // NOT WORK
    {
        public long id;
        public string brand;
        public string number;
        public List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                    new SqliteParameter("@brand", this.brand),
                    new SqliteParameter("@number", this.number) };
        }
        public List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
        }
        static public List<Car> Get()
        {
            List<Car> cars = new List<Car>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT c.id, c.brand, c.\"number\"  FROM cars c ",
                new List<SqliteParameter>(), 
                "Read cars.");

            while (reader.Read()) // построчно считываем данные
            {
                cars.Add(new Car()
                {
                    id = reader.GetInt64(0),
                    brand = reader.GetString(1),
                    number = reader.GetString(2)
                });
            }

            return cars;
        }
        static public List<Car> Get(string search)
        {
            List<Car> cars = new List<Car>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, brand, number FROM cars as cr WHERE cr.brand LIKE '%@like%' cr.number LIKE '%@like%'",
                new List<SqliteParameter> { new SqliteParameter("@like", search) },
                "Search cars.");
            while (reader.Read()) // построчно считываем данные
            {
                cars.Add(new Car() {
                    id = reader.GetInt64(0),
                    brand = reader.GetString(1),
                    number = reader.GetString(2) } );
            }
            return cars;
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
                "UPDATE cars SET brand = @brand, number = @number where WHERE id = @id",
                car.getByParametrListWithId(),
                "Update address.");
        }
        static public void Delete(Car car)
        {
            SqLite.Delete("cars", car.id);
        }
        public List<Car> GetCurrent(MList mlist)
        {
            List<Car> cars = new List<Car>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT cr.id, cr.brand, cr.number FROM cars AS cr " +
                "JOIN mlist_cars mc ON cr.id = mc.car_id WHERE mc.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
                "Read MList cars");
            while (reader.Read()) // построчно считываем данные
            {
                cars.Add(new Car {
                    id = reader.GetInt64(0),
                    brand = reader.GetString(1),
                    number = reader.GetString(2) } );
            }
            return cars;
        }
    }
}
