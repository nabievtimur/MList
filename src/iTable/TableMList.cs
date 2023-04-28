using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableMList : iTable
    {
        public TableMList()
        {
            this.StorageTableName = "Маршрутные листы";
        }
        public override iContainer getAssociatedContainer()
        {
            return new ContainerMList();
        }
        public override iContainer getAssociatedContainer(DataGridViewRow row)
        {
            return new ContainerMList(row);
        }
        public override void gridInit(DataGridView table)
        {
            base.gridInit(table);
            table.Columns.Add("numberMlist", "Номер");
            table.Columns.Add("employeeID", "СотрудникID");
            table.Columns[2].Visible = false;
            table.Columns.Add("employeeFullName", "Сотрудник");
            table.Columns.Add("dateCreate", "Дата создания");
            table.Columns.Add("dateBegin", "Дата начала");
            table.Columns.Add("timeBegin", "Время начала");
            table.Columns.Add("dateEnd", "Дата окончания");
            table.Columns.Add("timeEnd", "Время окончания");
            table.Columns.Add("dateCoach", "Время инструктажа");
            table.Columns.Add("datePassGun", "Дата сдачи оружия");
            table.Columns.Add("timePassGun", "Время сдачи оружия");
            table.Columns.Add("datePrint", "Дата печати");
            table.Columns.Add("notes", "Примечание");
        }
        public override void storageAdd(iContainer container)
        {
            throw new NotImplementedException();
        }
        // TODO Ваня Check
        public void storageAdd(
            ContainerMList container, 
            ContainerEmployee employee, 
            ContainerCollection<ContainerGun> guns,
            ContainerCollection<ContainerCar> cars,
            ContainerCollection<ContainerAddress> addressesDeep,
            ContainerCollection<ContainerAddress> addressesArrive)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteCommand createMlistCommand = SqLite.getInstance().getConnection().CreateCommand();
                createMlistCommand.Transaction = transaction;
                createMlistCommand.CommandText =
                    "INSERT INTO " + this.StorageTableName +
                    " (date_create, date_begin, end_date, coach_date, pass_gun_date, print_date, notes, deep_time, arrive_time, pass_gun_time, num_mlist)" +
                    "VALUES (@date_create, @date_begin, @end_date, @coach_date, @pass_gun_date, @print_date, @notes, @deep_time, @arrive_time, @pass_gun_time, @num_mlist); SELECT last_insert_rowid();";
                createMlistCommand.Parameters.Add(new SqliteParameter("@date_create", container.getDateCreate()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@date_begin", container.getDateBegin()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@end_date", container.getDateEnd()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@coach_date", container.getDateCoach()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@pass_gun_date", container.getDatePassGun()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@print_date", container.getDatePrint()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@notes", container.getNotes()));
                createMlistCommand.Parameters.Add(new SqliteParameter("@num_mlist", container.getNumberMlist()));
                
                object mlistID;
                try
                {
                    mlistID = createMlistCommand.ExecuteScalar();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    transaction.Rollback();
                    throw new QueryExeption("Add new Mlist.");
                }
                
                SqliteCommand mlistEmployeeCommand = SqLite.getInstance().getConnection().CreateCommand();
                mlistEmployeeCommand.Transaction = transaction;
                
                mlistEmployeeCommand.CommandText =
                    "INSERT INTO mlist_employees (mlist_id, employee_id) VALUES (@mlist_id, @employee_id)";
                mlistEmployeeCommand.Parameters.Add(new SqliteParameter("@mlist_id", mlistID));
                mlistEmployeeCommand.Parameters.Add(new SqliteParameter("@employee_id", employee.getId()));
                try
                {
                    if (mlistEmployeeCommand.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        throw new QueryExeption("Add employee to mlist.");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    transaction.Rollback();
                    throw new QueryExeption("Add employee to mlist.");
                }

                foreach (var gun in guns)
                {
                    SqliteCommand mlistGunCommand = SqLite.getInstance().getConnection().CreateCommand();
                    mlistGunCommand.Transaction = transaction;
                    
                    mlistGunCommand.CommandText =
                        "INSERT INTO mlist_gun (mlist_id, gun_id) VALUES (@mlist_id, @gun_id)";
                    mlistGunCommand.Parameters.Add(new SqliteParameter("@mlist_id", mlistID));
                    mlistGunCommand.Parameters.Add(new SqliteParameter("@gun_id", gun.getId()));
                    
                    try
                    {
                        if (mlistGunCommand.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            throw new QueryExeption("Add gun to mlist.");
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        throw new QueryExeption("Add gun to mlist.");
                    }
                }
                
                foreach (var car in cars)
                {
                    SqliteCommand mlistCarCommand = SqLite.getInstance().getConnection().CreateCommand();
                    mlistCarCommand.Transaction = transaction;
                    
                    mlistCarCommand.CommandText =
                        "INSERT INTO mlist_cars (mlist_id, car_id) VALUES (@mlist_id, @car_id)";
                    mlistCarCommand.Parameters.Add(new SqliteParameter("@mlist_id", mlistID));
                    mlistCarCommand.Parameters.Add(new SqliteParameter("@car_id", car.getId()));
                    
                    try
                    {
                        if (mlistCarCommand.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            throw new QueryExeption("Add car to mlist.");
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        throw new QueryExeption("Add car to mlist.");
                    }
                }
                
                foreach (var address in addressesDeep)
                {
                    SqliteCommand mlistAddressCommand = SqLite.getInstance().getConnection().CreateCommand();
                    mlistAddressCommand.Transaction = transaction;
                    
                    mlistAddressCommand.CommandText =
                        "INSERT INTO mlist_deep_address (mlist_id, deep_address_id) VALUES (@mlist_id, @deep_address_id)";
                    mlistAddressCommand.Parameters.Add(new SqliteParameter("@mlist_id", mlistID));
                    mlistAddressCommand.Parameters.Add(new SqliteParameter("@deep_address_id", address.getId()));

                    try
                    {
                        if (mlistAddressCommand.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            throw new QueryExeption("Add deep address to mlist.");
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        throw new QueryExeption("Add deep address to mlist.");
                    }
                }
                
                foreach (var address in addressesArrive)
                {
                    SqliteCommand mlistAddressCommand = SqLite.getInstance().getConnection().CreateCommand();
                    mlistAddressCommand.Transaction = transaction;
                    
                    mlistAddressCommand.CommandText =
                        "INSERT INTO mlist_arrive_address (mlist_id, arrive_address_id) VALUES (@mlist_id, @arrive_address_id)";
                    mlistAddressCommand.Parameters.Add(new SqliteParameter("@mlist_id", mlistID));
                    mlistAddressCommand.Parameters.Add(new SqliteParameter("@arrive_address_id", address.getId()));

                    try
                    {
                        if (mlistAddressCommand.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            throw new QueryExeption("Add arrive address to mlist.");
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        throw new QueryExeption("Add arrive address to mlist.");
                    }
                }
                
                transaction.Commit();
            }
        }
        public override void storageUpdate(iContainer container)
        {
            throw new NotImplementedException();
        }
        public void storageUpdate(
            ContainerMList container,
            ContainerEmployee employee,
            ContainerCollection<ContainerGun> guns,
            ContainerCollection<ContainerCar> cars,
            ContainerCollection<ContainerAddress> addressesDeep,
            ContainerCollection<ContainerAddress> addressesArrive)
        {
            SqLite.exec(
                "UPDATE " + this.StorageTableName + " SET " +
                        "date_create   = @date_create," +
                        "date_begin    = @date_begin," +
                        "end_date      = @end_date," +
                        "coach_date    = @coach_date," +
                        "pass_gun_date = @pass_gun_date," +
                        "print_date    = @print_date," +
                        "notes         = @notes," +
                        "deep_time     = @deep_time," +
                        "arrive_time   = @arrive_time," +
                        "pass_gun_time = @pass_gun_time," +
                        "num_mlist     = @num_mlist" +
                        "where id = @id",
                container.storageFillParameterCollectionWithId,
                "Update MList");
            // TODO Ваня
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerMList(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerMList>(SqLite.execGet(
                "SELECT ml.id, " +
                    "ml.num_mlist, " +
                    "ml.date_create, " +
                    "ml.date_begin, " +
                    "ml.end_date, " +
                    "ml.coach_date, " +
                    "ml.pass_gun_date, " +
                    "ml.pass_gun_time, " +
                    "ml.print_date, " +
                    "ml.notes, " +
                    "ml.arrive_time, " +
                    "ml.deep_time, " +
                    "e.id, " +
                    "e.last_name, " +
                    "e.first_name, " +
                    "e.middle_name " +
                    "FROM mlist AS ml " +
                    "JOIN mlist_employees me ON ml.id = me.mlist_id " +
                    "JOIN employees e ON e.id = me.employee_id",
                dFillerEmpty,
                "Reads MList.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerMList>(SqLite.execGet(
                "SELECT ml.id, " +
                    "ml.num_mlist, " +
                    "ml.date_create, " +
                    "ml.date_begin, " +
                    "ml.end_date, " +
                    "ml.coach_date, " +
                    "ml.pass_gun_date, " +
                    "ml.pass_gun_time, " +
                    "ml.print_date, " +
                    "ml.notes, " +
                    "ml.arrive_time, " +
                    "ml.deep_time, " +
                    "e.id, " +
                    "e.last_name, " +
                    "e.first_name, " +
                    "e.middle_name " +
                    "FROM " + this.StorageTableName + " AS ml " +
                    "JOIN mlist_employees me ON ml.id = me.mlist_id " +
                    "JOIN employees e ON e.id = me.employee_id",
                dFillerEmpty,
                "Reads MList.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            throw new NotImplementedException();
        }
        static public long GetNextNum()
        {
            SqliteDataReader reader = SqLite.execGet(
                "SELECT coalesce(max(m.num_mlist), 0) last_order_num FROM mlist as m;",
                dFillerEmpty,
                "Read max number.");
            long maxNum = 0;
            if (reader.HasRows)
            {
                try
                {
                    reader.Read();
                    maxNum = reader.GetInt64(0);
                    reader.Close();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return maxNum + 1;
        }
    }
}
