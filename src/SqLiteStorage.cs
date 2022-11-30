﻿using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;

namespace MList.Storage
{
    public class SqLiteStorage
    {
        private SqliteConnection _connection;
        private static SqLiteStorage instance;

        public enum Status
        {
            OK,
            ERROR,
            ERROR_CONNECTION,
            NO_ROWS
        }

        public struct Address
        {
            public long id;
            public string address;
        }

        public struct Car
        {
            public long id;
            public string brand;
            public string number;
        }

        public struct Gun
        {
            public long id;
            public string brand;
            public string series;
            public long number;
            public string ammo;
        }

        public struct Employee
        {
            public long id;
            public string firstName;
            public string lastName;
            public string middleName;
        }

        public struct Order
        {
            public long id;
            public long number;
            public long employeeID;
            public long date;
            public string employeeFullName;
        }

        public struct MList
        {
            public long id;
            public long dateCreate;
            public long dateBegin;
            public long dateEnd;
            public long dateCoach;
            public long datePassGun;
            public long datePrint;
            public string notes;
            public long timeDeep;
            public long timeArrive;
            public long timePassGun;
            public long numberMlist;
            public long employeeID;
            public string employeeFullName;
        }

        private SqLiteStorage()
        {
        }

        public static SqLiteStorage getInstance()
        {
            if (instance == null)
                instance = new SqLiteStorage();
            return instance;
        }

        public Status InitConnection()
        {
            string dbFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MList\\DataBase");
            System.Diagnostics.Debug.WriteLine(dbFolderPath);
            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
                try
                {
                    Directory.CreateDirectory(dbFolderPath);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }

            string dbFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MList\\DataBase\\mlist.db");

            if (!File.Exists(dbFilePath))
            {
                try
                {
                    this._connection = new SqliteConnection(string.Format(
                        "Data Source={0};Cache=Shared;Mode=ReadWriteCreate;Foreign Keys=True;",
                        dbFilePath));
                    this._connection.Open();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }

                SqliteCommand command = new SqliteCommand(DbCreate.createDbSql, this._connection);
                try
                {
                    command.ExecuteNonQuery();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }

            this._connection = new SqliteConnection(string.Format(
                "Data Source={0};Cache=Shared;Mode=ReadWrite;Foreign Keys=True;",
                dbFilePath));
            try
            {
                this._connection.Open();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            return Status.OK;
        }

        public Status Export(string path)
        {
            return Status.OK;
        }

        public Status Import(string path)
        {
            return Status.OK;
        }

        public Status GetCurrent(Order order, out List<Gun> guns)
        {
            guns = new List<Gun>();
            SqliteCommand command = new SqliteCommand(
                "SELECT gn.id, " +
                "gn.number, " +
                "gn.ammo, " +
                "gn.series, " +
                "gn.brand " +
                "FROM guns AS gn " +
                "JOIN order_gun og ON gn.id = og.gun_id " +
                "WHERE og.order_id = @order_id",
                this._connection);
            SqliteParameter mlistIdParam = new SqliteParameter("@order_id", order.id);
            command.Parameters.Add(mlistIdParam);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Gun gun = new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        series = reader.GetString(3),
                        number = reader.GetInt64(4),
                        ammo = reader.GetString(5)
                    };
                    guns.Add(gun);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }
        public Status GetCurrent(MList mlist, out List<Car> cars, out List<Gun> guns, out List<Address> arriveAddresses,
            out List<Address> deepAddresses)
        {
            cars = new List<Car>();
            guns = new List<Gun>();
            arriveAddresses = new List<Address>();
            deepAddresses = new List<Address>();
            ;
            SqliteCommand command = new SqliteCommand(
                "SELECT cr.id, " +
                "cr.brand, " +
                "cr.number " +
                "FROM cars AS cr " +
                "JOIN mlist_cars mc ON cr.id = mc.car_id " +
                "WHERE mc.mlist_id = @mlist_id ",
                this._connection);

            SqliteParameter mlistIdParam = new SqliteParameter("@mlist_id", mlist.id);
            command.Parameters.Add(mlistIdParam);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Car car = new Car
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        number = reader.GetString(2)
                    };
                    cars.Add(car);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            ;
            command = new SqliteCommand(
                "SELECT gn.id, " +
                "gn.number, " +
                "gn.brand, " +
                "gn.series, " +
                "gn.ammo " +
                "FROM guns AS gn " +
                "JOIN mlist_gun mg ON gn.id = mg.gun_id " +
                "WHERE mg.mlist_id = @mlist_id",
                this._connection);

            mlistIdParam = new SqliteParameter("@mlist_id", mlist.id);
            command.Parameters.Add(mlistIdParam);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Gun gun = new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(2),
                        series = reader.GetString(3),
                        number = reader.GetInt64(1),
                        ammo = reader.GetString(4)
                    };
                    guns.Add(gun);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            command = new SqliteCommand(
                "SELECT ad.id, " +
                "ad.address " +
                "FROM addresses AS ad " +
                "JOIN mlist_arrive_address maa ON ad.id = maa.arrive_address_id " +
                "WHERE maa.mlist_id = @mlist_id",
                this._connection);

            mlistIdParam = new SqliteParameter("@mlist_id", mlist.id);
            command.Parameters.Add(mlistIdParam);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Address address = new Address
                    {
                        id = reader.GetInt64(0),
                        address = reader.GetString(1)
                    };
                    arriveAddresses.Add(address);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            ;
            command = new SqliteCommand(
                "SELECT ad.id, " +
                "ad.address " +
                "FROM addresses AS ad " +
                "JOIN mlist_deep_address mda ON ad.id = mda.deep_address_id " +
                "WHERE mda.mlist_id = @mlist_id",
                this._connection);

            mlistIdParam = new SqliteParameter("@mlist_id", mlist.id);
            command.Parameters.Add(mlistIdParam);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Address address = new Address
                    {
                        id = reader.GetInt64(0),
                        address = reader.GetString(1)
                    };
                    deepAddresses.Add(address);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            return Status.OK;
        }

        public Status Get(out List<MList> mlists)
        {
            mlists = new List<MList>();
            SqliteCommand command = new SqliteCommand(
                "SELECT ml.id, " +
                "ml.num_mlist, " +
                "ml.date_create, " +
                "ml.date_begin, " +
                "ml.date_end, " +
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
                "JOIN employees e ON e.id = me.employee_id;",
                this._connection);
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    MList mList = new MList
                    {
                        id = reader.GetInt64(0),
                        dateCreate = reader.GetInt64(2),
                        dateBegin = reader.GetInt64(3),
                        dateEnd = reader.GetInt64(4),
                        dateCoach = reader.GetInt64(5),
                        datePassGun = reader.GetInt64(6),
                        datePrint = reader.GetInt64(8),
                        notes = reader.GetString(9),
                        timeDeep = reader.GetInt64(11),
                        timeArrive = reader.GetInt64(10),
                        timePassGun = reader.GetInt64(7),
                        numberMlist = reader.GetInt64(1),
                        employeeID = reader.GetInt64(12),
                        employeeFullName = string.Format(
                            "{0} {1} {2}",
                            reader.GetString(13),
                            reader.GetString(14),
                            reader.GetString(15)
                        )
                    };
                    mlists.Add(mList);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Address> adresses)
        {
            adresses = new List<Address>();
            SqliteCommand command = new SqliteCommand(
                "SELECT id, address " +
                "FROM addresses",
                _connection);
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    Address address = new Address
                    {
                        id = reader.GetInt64(0),
                        address = reader.GetString(1)
                    };
                    adresses.Add(address);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }

            return Status.OK;
        }

        public Status Get(out List<Car> cars)
        {
            cars = new List<Car>();
            SqliteCommand command = new SqliteCommand(
                "SELECT id, brand, number " +
                "FROM cars",
                _connection);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    Car car = new Car
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        number = reader.GetString(2)
                    };
                    cars.Add(car);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Gun> guns)
        {
            guns = new List<Gun>();
            SqliteCommand command = new SqliteCommand(
                "SELECT id, brand, series, number, ammo " +
                "FROM guns",
                _connection);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    Gun gun = new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        series = reader.GetString(2),
                        number = reader.GetInt64(3),
                        ammo = reader.GetString(4)
                    };
                    guns.Add(gun);
                }


                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Employee> employees)
        {
            employees = new List<Employee>();
            SqliteCommand command = new SqliteCommand(
                "SELECT id, first_name, last_name, middle_name FROM employees",
                _connection);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    Employee employee = new Employee()
                    {
                        id = reader.GetInt64(0),
                        firstName = reader.GetString(1),
                        lastName = reader.GetString(2),
                        middleName = reader.GetString(3),
                    };
                    employees.Add(employee);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Order> orders)
        {
            orders = new List<Order>();
            SqliteCommand command = new SqliteCommand(
                "SELECT od.id, " +
                "od.number, " +
                "od.date, " +
                "e.id, " +
                "e.last_name, " +
                "e.first_name, " +
                "e.middle_name " +
                "FROM orders AS od " +
                "JOIN orders_employees oe ON od.id = oe.order_id " +
                "JOIN employees e ON e.id = oe.employee_id",
                _connection);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Status.NO_ROWS;
                }

                while (reader.Read()) // построчно считываем данные
                {
                    Order order = new Order()
                    {
                        id = reader.GetInt64(0),
                        number = reader.GetInt64(1),
                        date = reader.GetInt64(2),
                        employeeID = reader.GetInt64(3),
                        employeeFullName = string.Format(
                            "{0} {1} {2}",
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6)
                        )
                    };
                    orders.Add(order);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Gun> guns, Employee employee)
        {
            guns = new List<Gun>();
            SqliteCommand command = new SqliteCommand(
                "select gn.id," +
                "gn.brand," +
                "gn.series," +
                "gn.number," +
                "gn.ammo" +
                "from guns as gn" +
                "join order_gun og on gn.id = og.gun_id" +
                "join orders o on o.id = og.order_id" +
                "join employees e on e.id = o.employee_id" +
                "where e.id = @emp_id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@emp_id", employee.id));
            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Gun gun = new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        series = reader.GetString(3),
                        number = reader.GetInt64(4),
                        ammo = reader.GetString(5)
                    };
                    guns.Add(gun);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Gun> guns, Order order)
        {
            guns = new List<Gun>();
            SqliteCommand command = new SqliteCommand(
                "select gn.id," +
                "gn.brand," +
                "gn.series," +
                "gn.number," +
                "gn.ammo" +
                "from guns as gn" +
                "join order_gun og on gn.id = og.gun_id" +
                "join orders o on o.id = og.order_id" +
                "where o.id = @ord_id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@ord_id", order.id));
            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    Gun gun = new Gun
                    {
                        id = reader.GetInt64(0),
                        brand = reader.GetString(1),
                        series = reader.GetString(3),
                        number = reader.GetInt64(4),
                        ammo = reader.GetString(5)
                    };
                    guns.Add(gun);
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out long orderRecommendNumber)
        {
            SqliteCommand command = new SqliteCommand(
                "select max(number) from orders;",
                this._connection);
            try
            {
                long orN = 0;
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        orN = reader.GetInt64(0);
                    }
                }
                orderRecommendNumber = orN+1;
                return Status.OK;
            }
            catch (Exception e)
            {
                orderRecommendNumber = 0;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Gun> guns, string like)
        {
            guns = new List<Gun>();
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand getGun = this._connection.CreateCommand();
                getGun.CommandText = 
                    "SELECT " +
                    "g.id," +
                    "g.brand," +
                    "g.series," +
                    "g.number," +
                    "g.ammo" +
                    "FROM guns g " +
                    "WHERE " +
                    "g.brand LIKE '%@like%' OR " +
                    "g.series LIKE '%@like%' OR " +
                    "g.\"number\" LIKE '%@like%' or " +
                    "g.ammo LIKE '%@like%' " +
                    "ORDER BY g.brand;";
                getGun.Parameters.Add(new SqliteParameter("@like", like));
                try
                {
                    SqliteDataReader reader = getGun.ExecuteReader();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Gun gun = new Gun
                        {
                            id = reader.GetInt64(0),
                            brand = reader.GetString(1),
                            series = reader.GetString(3),
                            number = reader.GetInt64(4),
                            ammo = reader.GetString(5)
                        };
                        guns.Add(gun);
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }
        
        public Status Get(out List<Address> adresses, string like)
        {
            adresses = new List<Address>();
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand getAddress = this._connection.CreateCommand();
                getAddress.CommandText = 
                    "SELECT " +
                    "id, " +
                    "address " +
                    "FROM addresses as ad" +
                    "WHERE " +
                    "ad.address LIKE '%@like%';";
                getAddress.Parameters.Add(new SqliteParameter("@like", like));
                try
                {
                    SqliteDataReader reader = getAddress.ExecuteReader();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Address address = new Address()
                        {
                            id = reader.GetInt64(0),
                            address = reader.GetString(1),
                        };
                        adresses.Add(address);
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }
        
        public Status Get(out List<Car> cars, string like)
        {
            cars = new List<Car>();
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand getCars = this._connection.CreateCommand();
                getCars.CommandText = 
                    "SELECT " +
                    "id, " +
                    "brand, " +
                    "number " +
                    "FROM cars as cr" +
                    "WHERE " + 
                    "cr.brand LIKE '%@like%'" +
                    "cr.number LIKE '%@like%';";
                getCars.Parameters.Add(new SqliteParameter("@like", like));
                try
                {
                    SqliteDataReader reader = getCars.ExecuteReader();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Car car = new Car()
                        {
                            id = reader.GetInt64(0),
                            brand = reader.GetString(1),
                            number = reader.GetString(2)
                        };
                        cars.Add(car);
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Add(Address adress)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand addAddress = this._connection.CreateCommand();
                addAddress.CommandText =  
                    "INSERT INTO addresses (address) " +
                    "VALUES (@address)";
                addAddress.Parameters.Add(new SqliteParameter("@address", adress.address));
                try
                {
                    if (addAddress.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Add(Car car)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO cars (brand, number)" +
                "VALUES (@brand, @number)",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@brand", car.brand));
            command.Parameters.Add(new SqliteParameter("@number", car.number));

            try
            {
                if (command.ExecuteNonQuery() == 0)
                {
                    return Status.ERROR;
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Gun gun)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand addGun = this._connection.CreateCommand();
                addGun.CommandText =
                    "INSERT INTO guns (brand, series, number, ammo) " +
                    "VALUES (@brand, @series, @number, @ammo)";
                addGun.Parameters.Add(new SqliteParameter("@brand", gun.brand));
                addGun.Parameters.Add(new SqliteParameter("@series", gun.series));
                addGun.Parameters.Add(new SqliteParameter("@number", gun.number));
                addGun.Parameters.Add(new SqliteParameter("@ammo", gun.ammo));
                try
                {
                    if (addGun.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Add(Employee employee)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO employees (first_name, last_name, middle_name) " +
                "VALUES (@first_name, @last_name, @middle_name)",
                this._connection);

            command.Parameters.Add(new SqliteParameter("@first_name", employee.firstName));
            command.Parameters.Add(new SqliteParameter("@last_name", employee.lastName));
            command.Parameters.Add(new SqliteParameter("@middle_name", employee.middleName));

            try
            {
                if (command.ExecuteNonQuery() == 0)
                {
                    return Status.ERROR;
                }

                return Status.OK;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Order order, List<Gun> guns)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand createOrderCommand = this._connection.CreateCommand();
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
                    return Status.ERROR;
                }
                
                foreach (var gun in guns)
                {
                    SqliteCommand orderGunCommand = this._connection.CreateCommand();
                        
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
                            return Status.ERROR;
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                }
                transaction.Commit();
                return Status.OK;
            }
            
        }

        public Status Add(MList mlist)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand addMList = this._connection.CreateCommand();
                addMList.CommandText =
                    "insert into mlist (date_create, date_begin, end_date, coach_date, pass_gun_date," +
                    " print_date, notes, deep_time, arrive_time, pass_gun_time, num_mlist)" +
                    "VALUES (@date_create, @date_begin, @end_date, @coach_date, @pass_gun_date, @print_date," +
                    " @notes, @deep_time, @arrive_time, @pass_gun_time, @num_mlist);";
                addMList.Parameters.Add(new SqliteParameter("@date_create", mlist.dateCreate));
                addMList.Parameters.Add(new SqliteParameter("@date_begin", mlist.dateBegin));
                addMList.Parameters.Add(new SqliteParameter("@end_date", mlist.dateEnd));
                addMList.Parameters.Add(new SqliteParameter("@coach_date", mlist.dateCoach));
                addMList.Parameters.Add(new SqliteParameter("@pass_gun_date", mlist.datePassGun));
                addMList.Parameters.Add(new SqliteParameter("@print_date", mlist.datePrint));
                addMList.Parameters.Add(new SqliteParameter("@notes", mlist.notes));
                addMList.Parameters.Add(new SqliteParameter("@deep_time", mlist.timeDeep));
                addMList.Parameters.Add(new SqliteParameter("@arrive_time", mlist.timeArrive));
                addMList.Parameters.Add(new SqliteParameter("@pass_gun_time", mlist.timePassGun));
                addMList.Parameters.Add(new SqliteParameter("@num_mlist", mlist.numberMlist));
                try
                {
                    if (addMList.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        // Mlist update
        public Status Update(Address adress)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateAddress = this._connection.CreateCommand();
                updateAddress.CommandText =  
                    "update addresses set address = @address where id = @id;";
                updateAddress.Parameters.Add(new SqliteParameter("@address", adress.address));
                updateAddress.Parameters.Add(new SqliteParameter("@id", adress.id));
                
                try
                {
                    if (updateAddress.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Update(Car car)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateCar = this._connection.CreateCommand();
                updateCar.CommandText =  
                    "update cars set brand = @brand, number = @number where  id = @id;";
                updateCar.Parameters.Add(new SqliteParameter("@brand", car.brand));
                updateCar.Parameters.Add(new SqliteParameter("@number", car.number));
                updateCar.Parameters.Add(new SqliteParameter("@id", car.id));
                try
                {
                    if (updateCar.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Update(Gun gun)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateGun = this._connection.CreateCommand();
                updateGun.CommandText =  
                    "update guns set brand = @brand, series = @series, number = @number, ammo = @ammo where  id = @id;";
                updateGun.Parameters.Add(new SqliteParameter("@brand", gun.brand));
                updateGun.Parameters.Add(new SqliteParameter("@number", gun.number));
                updateGun.Parameters.Add(new SqliteParameter("@ammo", gun.ammo));
                updateGun.Parameters.Add(new SqliteParameter("@series", gun.series));
                updateGun.Parameters.Add(new SqliteParameter("@id", gun.id));
                try
                {
                    if (updateGun.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Update(Employee employee)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateEmployee = this._connection.CreateCommand();
                updateEmployee.CommandText =  
                    "update employees " +
                    "set first_name = @first_name, last_name = @last_name, middle_name = @middle_name where id = @id;";
                updateEmployee.Parameters.Add(new SqliteParameter("@first_name", employee.firstName));
                updateEmployee.Parameters.Add(new SqliteParameter("@last_name", employee.lastName));
                updateEmployee.Parameters.Add(new SqliteParameter("@middle_name", employee.middleName));
                updateEmployee.Parameters.Add(new SqliteParameter("@id", employee.id));
                try
                {
                    if (updateEmployee.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Update(Order order)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateOrder = this._connection.CreateCommand();
                updateOrder.CommandText =  
                    "update orders " +
                    "set number = @number, employee_id = @employee_id, date = @date where id = @id;";
                updateOrder.Parameters.Add(new SqliteParameter("@number", order.number));
                updateOrder.Parameters.Add(new SqliteParameter("@employee_id", order.employeeID));
                updateOrder.Parameters.Add(new SqliteParameter("@date", order.date));
                updateOrder.Parameters.Add(new SqliteParameter("@id", order.id));
                try
                {
                    if (updateOrder.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Update(MList mlist)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateMlist = this._connection.CreateCommand();
                updateMlist.CommandText =  
                    "update mlist" +
                    "set date_create   = @date_create," +
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
                    "where id = @id;";
                updateMlist.Parameters.Add(new SqliteParameter("@date_create", mlist.dateCreate));
                updateMlist.Parameters.Add(new SqliteParameter("@date_begin", mlist.dateBegin));
                updateMlist.Parameters.Add(new SqliteParameter("@end_date", mlist.dateEnd));
                updateMlist.Parameters.Add(new SqliteParameter("@coach_date", mlist.dateCoach));
                updateMlist.Parameters.Add(new SqliteParameter("@pass_gun_date", mlist.datePassGun));
                updateMlist.Parameters.Add(new SqliteParameter("@print_date", mlist.datePrint));
                updateMlist.Parameters.Add(new SqliteParameter("@notes", mlist.notes));
                updateMlist.Parameters.Add(new SqliteParameter("@deep_time", mlist.timeDeep));
                updateMlist.Parameters.Add(new SqliteParameter("@arrive_time", mlist.timeArrive));
                updateMlist.Parameters.Add(new SqliteParameter("@pass_gun_time", mlist.timePassGun));
                updateMlist.Parameters.Add(new SqliteParameter("@num_mlist", mlist.numberMlist));
                updateMlist.Parameters.Add(new SqliteParameter("@id", mlist.id));
                try
                {
                    if (updateMlist.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        // delete Mlist
        public Status Delete(string DataBase, long id)
        {
            using (var transaction = this._connection.BeginTransaction())
            {
                SqliteCommand updateOrder = this._connection.CreateCommand();
                updateOrder.CommandText =  
                    "DELETE FROM " + DataBase + " WHERE id = @id";
                updateOrder.Parameters.Add(new SqliteParameter("@id", id));
                try
                {
                    if (updateOrder.ExecuteNonQuery() == 0)
                    {
                        transaction.Rollback();
                        return Status.ERROR;
                    }
                    transaction.Commit();
                    return Status.OK;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
        }

        public Status Delete(Address address)
        {
            return Delete("addresses", address.id);
        }

        public Status Delete(Car car)
        {
            return Delete("cars", car.id);
        }

        public Status Delete(Gun gun)
        {
            return Delete("guns", gun.id);
        }

        public Status Delete(Employee employee)
        {
            return Delete("employees", employee.id);
        }

        public Status Delete(Order order)
        {
            return Delete("orders", order.id);
        }

        public Status Delete(MList mlist)
        {
            return Delete("mlist", mlist.id);
        }
    }
}