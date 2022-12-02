using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Order
    {
        public long id;
        public long number;
        public long employeeID;
        public long date;
        public string employeeFullName;
        public List<Tuple<String, object>> getByParametrList()
        {
            return new List<Tuple<String, object>> {
                    new Tuple<String, object>("@number", this.number),
                    new Tuple<String, object>("@employee_id", this.employeeID),
                    new Tuple<String, object>("@date", this.date) };
        }
        public List<Tuple<String, object>> getByParametrListWithId()
        {
            List<Tuple<String, object>> l = getByParametrList();
            l.Add(new Tuple<String, object>("@id", this.id));
            return l;
        }
        static public List<Order> Get()
        {
            List<Order> orders = new List<Order>();

            SqliteDataReader reader = SqLite.execGet(
                "SELECT od.id, od.number, od.date, e.id, e.last_name, e.first_name, e.middle_name " +
                    "FROM orders AS od JOIN orders_employees oe ON od.id = oe.order_id " +
                    "JOIN employees e ON e.id = oe.employee_id", 
                null, 
                "Read orders.");

            while (reader.Read()) // построчно считываем данные
            {
                orders.Add(new Order() {
                    id = reader.GetInt64(0),
                    number = reader.GetInt64(1),
                    date = reader.GetInt64(2),
                    employeeID = reader.GetInt64(3),
                    employeeFullName = string.Format(
                            "{0} {1} {2}",
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6) ) } );
            }

            return orders;
        }
        static public long GetNextOrderNum()
        {
            SqliteDataReader reader = SqLite.execGet("SELECT max(number) FROM orders", null, "Read max number.");
            long maxOrderNum = 0;
            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    maxOrderNum = reader.GetInt64(0);
                }
            }
            return maxOrderNum + 1;
        }
        static public void Add(Order order, List<Gun> guns)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteCommand createOrderCommand = SqLite.getInstance().getConnection().CreateCommand();
                createOrderCommand.CommandText =
                    "INSERT INTO orders (number, employee_id, date)" +
                    "VALUES (@number, @employee_id, @date);" +
                    "SELECT last_insert_rowid();";

                createOrderCommand.Parameters.Add(new SqliteParameter("@number", order.number));
                createOrderCommand.Parameters.Add(new SqliteParameter("@employee_id", order.employeeID));
                createOrderCommand.Parameters.Add(new SqliteParameter("@date", order.date));
                object orderID;
                try
                {
                    orderID = createOrderCommand.ExecuteScalar();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    transaction.Rollback();
                    throw new QueryExeption("Add new order.");
                }

                foreach (var gun in guns)
                {
                    SqliteCommand orderGunCommand = SqLite.getInstance().getConnection().CreateCommand();

                    orderGunCommand.CommandText =
                        "INSERT INTO order_gun (order_id, gun_id)" +
                        "VALUES (@order_id, @gun_id)";
                    orderGunCommand.Parameters.Add(new SqliteParameter("@order_id", orderID));
                    orderGunCommand.Parameters.Add(new SqliteParameter("@gun_id", gun.id));
                    try
                    {
                        if (orderGunCommand.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            throw new QueryExeption("Add guns to order.");
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        throw new QueryExeption("Add guns to order.");
                    }
                }
                transaction.Commit();
            }

        }
        static public void Update(Order order)
        {
            SqLite.exec(
                "UPDATE orders SET number = @number, employee_id = @employee_id, date = @date where id = @id",
                order.getByParametrListWithId(),
                "Update Order");
        }
        static public void Delete(Order order)
        {
            SqLite.Delete("orders", order.id);
        }
    }
}
