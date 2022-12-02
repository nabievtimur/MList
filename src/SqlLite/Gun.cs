using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Gun
    {
        public long id;
        public string brand;
        public string series;
        public long number;
        public string ammo;
        public List<Tuple<String, object>> getByParametrList()
        {
            return new List<Tuple<String, object>> {
                    new Tuple<String, object>("@brand", this.brand),
                    new Tuple<String, object>("@series", this.series),
                    new Tuple<String, object>("@numder", this.number),
                    new Tuple<String, object>("@ammo", this.ammo) };
        }
        public List<Tuple<String, object>> getByParametrListWithId()
        {
            List<Tuple<String, object>> l = getByParametrList();
            l.Add(new Tuple<String, object>("@id", this.id));
            return l;
        }
        static public List<Gun> Get()
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, brand, series, number, ammo FROM guns",
                null,
                "Read guns.");

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

            return guns;
        }
        public List<Gun> Get(string search)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT g.id, g.brand, g.series, g.number, g.ammo FROM guns g " +
                    "WHERE g.brand LIKE '%@like%' OR g.series LIKE '%@like%' OR g.\"number\" LIKE '%@like%' or g.ammo LIKE '%@like%' " +
                    "ORDER BY g.brand;",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@like", search)},
                "Read guns.");

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

            return guns;
        }
        public List<Gun> Get(Employee employee)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM guns AS gn" +
                    "JOIN order_gun og ON gn.id = og.gun_id" +
                    "JOIN orders o ON o.id = og.order_id" +
                    "JOIN employees e ON e.id = o.employee_id" +
                    "WHERE e.id = @emp_id",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@emp_id", employee.id )},
                "Read guns by order.");

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

            return guns;
        }
        public List<Gun> GetCurrent(Order order)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT gn.id, gn.number, gn.ammo, gn.series, gn.brand FROM guns AS gn " +
                    "JOIN order_gun og ON gn.id = og.gun_id WHERE og.order_id = @order_id",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@order_id", order.id)},
                "Read guns by order.");

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

            return guns;
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
        static public void Delete(Gun address)
        {
            SqLite.Delete("addresses", address.id);
        }
        public List<Gun> GetCurrent(MList mlist)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT gn.id, gn.number, gn.brand, gn.series, gn.ammo FROM guns AS gn " +
                    "JOIN mlist_gun mg ON gn.id = mg.gun_id WHERE mg.mlist_id = @mlist_id",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@mlist_id", mlist.id) },
                "Get guns by mlist.");

            while (reader.Read()) // построчно считываем данные
            {
                guns.Add(new Gun {
                    id = reader.GetInt64(0),
                    brand = reader.GetString(2),
                    series = reader.GetString(3),
                    number = reader.GetInt64(1),
                    ammo = reader.GetString(4) } );
            }

            return guns;
        }
    }
}
