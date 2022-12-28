using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Order : iConteiner
    {
        public long number;
        public long employeeID;
        public long date;
        public string employeeFullName;
        public override DataGridViewRow fillRow(DataGridViewRow row)
        {
            return row;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
        static private List<Order> Read(SqliteDataReader reader)
        {
            List<Order> orders = new List<Order>();

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    orders.Add(new Order {
                        id = reader.GetInt64(0),
                        number = reader.GetInt64(1),
                        date = reader.GetInt64(2),
                        employeeID = reader.GetInt64(3),
                        employeeFullName = string.Format(
                            "{0} {1} {2}",
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6)) } );
                }
            }
            catch (Exception)
            {
                throw new QueryExeption();
            }

            return orders;
        }
        static public List<Order> Get()
        {
            return Order.Read(SqLite.execGet(
                "SELECT od.id, od.number, od.date, e.id, e.last_name, e.first_name, e.middle_name " +
                    "FROM orders AS od JOIN employees e ON od.employee_id = e.id ",
                new List<SqliteParameter>(),
                "Read orders."));
        }
        static public List<Order> Get(String search) // CHECK
        {
            return Order.Read(SqLite.execGet(
                "SELECT od.id, od.number, od.date, e.id, e.last_name, e.first_name, e.middle_name " +
                    "FROM orders AS od JOIN employees e ON od.employee_id = e.id " +
                    "WHERE od.number LIKE @like OR od.date LIKE @like OR e.first_name LIKE @like OR " +
                    "e.last_name LIKE @like OR e.middle_name LIKE @like ",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", "%" + search + "%") },
                "Search Orders."));
        }
        static public long GetNextOrderNum()
        {
            SqliteDataReader reader = SqLite.execGet(
                "SELECT coalesce(max(o.number), 0) last_order_num FROM orders as o;", 
                new List<SqliteParameter>(), 
                "Read max number.");
            long maxOrderNum = 0;
            if (reader.HasRows)
            {
                try
                {
                    reader.Read();
                    maxOrderNum = reader.GetInt64(0);
                    reader.Close();
                }
                catch (Exception) 
                {
                    return 0;
                }
            }
            return maxOrderNum + 1;
        }
        static public void Add(Order order, List<Gun> guns) // think about it
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteCommand createOrderCommand = SqLite.getInstance().getConnection().CreateCommand();
                createOrderCommand.Transaction = transaction;
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
                    orderGunCommand.Transaction = transaction;

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
        static public void initTable(DataGridView table)
        {
            table.Columns.Add("number", "Номер");
            table.Columns.Add("date", "Дата");
            table.Columns.Add("employeeFullName", "Сотрудник");
        }
    }
}
