using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableOrder : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("number", "Номер");
            table.Columns.Add("date", "Дата");
            table.Columns.Add("employeeFullName", "Сотрудник");
        }
        public override void storageAdd(iContainer container)
        {
            throw new NotImplementedException();
        }
        public void storageAdd(ContainerOrder order, ContainerCollection<ContainerGun> guns)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteCommand createOrderCommand = SqLite.getInstance().getConnection().CreateCommand();
                createOrderCommand.Transaction = transaction;
                createOrderCommand.CommandText =
                    "INSERT INTO orders (number, employee_id, date)" +
                    "VALUES (@number, @employee_id, @date);" +
                    "SELECT last_insert_rowid();";

                createOrderCommand.Parameters.Add(new SqliteParameter("@number", order.getNumber()));
                createOrderCommand.Parameters.Add(new SqliteParameter("@employee_id", order.getEmployeeID()));
                createOrderCommand.Parameters.Add(new SqliteParameter("@date", order.getDate()));
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
                    orderGunCommand.Parameters.Add(new SqliteParameter("@gun_id", gun.getId()));
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
        public override void storageUpdate(iContainer container)
        {
            throw new NotImplementedException();
        }
        public override void storageDelete(DataGridViewRow row)
        {
            this.storageDelete(new ContainerOrder(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT od.id, od.number, od.date, e.id, e.last_name, e.first_name, e.middle_name " +
                    "FROM orders AS od JOIN employees e ON od.employee_id = e.id ",
                dFillerEmpty,
                "Read orders.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT od.id, od.number, od.date, e.id, e.last_name, e.first_name, e.middle_name " +
                    "FROM orders AS od JOIN employees e ON od.employee_id = e.id " +
                    "WHERE od.number LIKE @like OR od.date LIKE @like OR e.first_name LIKE @like OR " +
                    "e.last_name LIKE @like OR e.middle_name LIKE @like ",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Search Orders.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            throw new NotImplementedException();
        }
        static public long GetNextOrderNum()
        {
            SqliteDataReader reader = SqLite.execGet(
                "SELECT coalesce(max(o.number), 0) last_order_num FROM orders as o;",
                dFillerEmpty,
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
    }
}
