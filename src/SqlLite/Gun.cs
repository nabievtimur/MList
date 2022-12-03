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
        public List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                    new SqliteParameter("@brand", this.brand),
                    new SqliteParameter("@series", this.series),
                    new SqliteParameter("@number", this.number),
                    new SqliteParameter("@ammo", this.ammo) };
        }
        public List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
        }
        static public List<Gun> Get()
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, brand, series, number, ammo FROM guns",
                new List<SqliteParameter>(),
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
        static public List<Gun> Get(string search)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT g.id, g.brand, g.series, g.number, g.ammo FROM guns g " +
                    "WHERE g.brand LIKE '%@like%' OR g.series LIKE '%@like%' OR g.\"number\" LIKE '%@like%' or g.ammo LIKE '%@like%' " +
                    "ORDER BY g.brand;",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", search)},
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
        public List<Gun> Get(Gun gun)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM guns AS gn" +
                    "JOIN order_gun og ON gn.id = og.gun_id" +
                    "JOIN orders o ON o.id = og.order_id" +
                    "JOIN employees e ON e.id = o.employee_id" +
                    "WHERE e.id = @emp_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@emp_id", gun.id )},
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
                new List<SqliteParameter> {
                    new SqliteParameter("@order_id", order.id)},
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
        static public void Delete(Gun gun)
        {
            SqLite.Delete("guns", gun.id);
        }
        public List<Gun> GetCurrent(MList mlist)
        {
            List<Gun> guns = new List<Gun>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT gn.id, gn.number, gn.brand, gn.series, gn.ammo FROM guns AS gn " +
                    "JOIN mlist_gun mg ON gn.id = mg.gun_id WHERE mg.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
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
