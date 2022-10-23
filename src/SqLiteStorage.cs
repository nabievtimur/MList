using System;
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

//         public Status GetDataFoMlistByEmployee(
//             Employee employee,
//             out List<Car> cars,
//             out List<Gun> guns,
//             out List<Address> addresses
//         )
//         {
//             cars = new List<Car>();
//             guns = new List<Gun>();
//             addresses = new List<Address>();
//             
//             string sqlExpression = @"
// select cr.id, cr.brand, cr.number
// from cars as cr;
// ";
//             SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
//
//             try
//             {
//                 SqliteDataReader reader = command.ExecuteReader();
//
//                 while (reader.Read()) // построчно считываем данные
//                 {
//                     Car car = new Car
//                     {
//                         id = reader.GetInt64(0),
//                         brand = reader.GetString(1),
//                         number = reader.GetString(2)
//                     };
//                     cars.Add(car);
//                 }
//             }
//             catch (Exception e)
//             {
//                 System.Diagnostics.Debug.WriteLine(e.ToString());
//                 return Status.ERROR;
//             }
//             
//             sqlExpression = @"
// select gn.id, gn.number, gn.brand, gn.series, gn.ammo
// from guns as gn
// ";
//             command = new SqliteCommand(sqlExpression, this._connection);
//             
//             try
//             {
//                 SqliteDataReader reader = command.ExecuteReader();
//
//                 while (reader.Read()) // построчно считываем данные
//                 {
//                     Gun gun = new Gun
//                     {
//                         id = reader.GetInt64(0),
//                         brand = reader.GetString(2),
//                         series = reader.GetString(3),
//                         number = reader.GetInt64(1),
//                         ammo = reader.GetString(4)
//                     };
//                     guns.Add(gun);
//                 }
//             }
//             catch (Exception e)
//             {
//                 System.Diagnostics.Debug.WriteLine(e.ToString());
//                 return Status.ERROR;
//             }
//             
//             sqlExpression = @"
// select ad.id,
//        ad.address
// from addresses as ad
// ";
//
//             command = new SqliteCommand(sqlExpression, this._connection);
//
//             try
//             {
//                 SqliteDataReader reader = command.ExecuteReader();
//
//                 while (reader.Read()) // построчно считываем данные
//                 {
//                     Address address = new Address
//                     {
//                         id = reader.GetInt64(0),
//                         address = reader.GetString(1)
//                     };
//                     addresses.Add(address);
//                 }
//             }
//             catch (Exception e)
//             {
//                 System.Diagnostics.Debug.WriteLine(e.ToString());
//                 return Status.ERROR;
//             }
//         }

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

        // MLIST add
        public Status Add(Address adress)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO addresses (address) " +
                "VALUES (@address)",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@address", adress.address));

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
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO guns (brand, series, number, ammo) " +
                "VALUES (@brand, @series, @number, @ammo)",
                this._connection);

            command.Parameters.Add(new SqliteParameter("@brand", gun.brand));
            command.Parameters.Add(new SqliteParameter("@series", gun.series));
            command.Parameters.Add(new SqliteParameter("@number", gun.number));
            command.Parameters.Add(new SqliteParameter("@ammo", gun.ammo));

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

        public Status Add(Order order)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO orders number, employee_id, date" +
                "VALUES @number, @employee_id, @date",
                this._connection);

            command.Parameters.Add(new SqliteParameter("@number", order.number));
            command.Parameters.Add(new SqliteParameter("@employee_id", order.employeeID));
            command.Parameters.Add(new SqliteParameter("@date", order.date));

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

        public Status Add(MList mlist)
        {
            SqliteCommand command = new SqliteCommand(
                "insert into mlist (date_create, date_begin, end_date, coach_date, pass_gun_date," +
                " print_date, notes, deep_time, arrive_time, pass_gun_time, num_mlist)" +
                "VALUES (@date_create, @date_begin, @end_date, @coach_date, @pass_gun_date, @print_date," +
                " @notes, @deep_time, @arrive_time, @pass_gun_time, @num_mlist);",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@date_create", mlist.dateCreate));
            command.Parameters.Add(new SqliteParameter("@date_begin", mlist.dateBegin));
            command.Parameters.Add(new SqliteParameter("@end_date", mlist.dateEnd));
            command.Parameters.Add(new SqliteParameter("@coach_date", mlist.dateCoach));
            command.Parameters.Add(new SqliteParameter("@pass_gun_date", mlist.datePassGun));
            command.Parameters.Add(new SqliteParameter("@print_date", mlist.datePrint));
            command.Parameters.Add(new SqliteParameter("@notes", mlist.notes));
            command.Parameters.Add(new SqliteParameter("@deep_time", mlist.timeDeep));
            command.Parameters.Add(new SqliteParameter("@arrive_time", mlist.timeArrive));
            command.Parameters.Add(new SqliteParameter("@pass_gun_time", mlist.timePassGun));
            command.Parameters.Add(new SqliteParameter("@num_mlist", mlist.numberMlist));
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

        // Mlist update
        public Status Update(Address adress)
        {
            SqliteCommand command = new SqliteCommand(
                "update addresses set address = @address where id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@address", adress.address));
            command.Parameters.Add(new SqliteParameter("@id", adress.id));

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

        public Status Update(Car car)
        {
            SqliteCommand command = new SqliteCommand(
                "update cars set brand = @brand, number = @number where  id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@brand", car.brand));
            command.Parameters.Add(new SqliteParameter("@number", car.number));
            command.Parameters.Add(new SqliteParameter("@id", car.id));

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

        public Status Update(Gun gun)
        {
            SqliteCommand command = new SqliteCommand(
                "update guns set brand = @brand, series = @series, number = @number, ammo = @ammo where  id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@brand", gun.brand));
            command.Parameters.Add(new SqliteParameter("@number", gun.number));
            command.Parameters.Add(new SqliteParameter("@ammo", gun.ammo));
            command.Parameters.Add(new SqliteParameter("@series", gun.series));
            command.Parameters.Add(new SqliteParameter("@id", gun.id));

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

        public Status Update(Employee employee)
        {
            SqliteCommand command = new SqliteCommand(
                "update employees " +
                "set first_name = @first_name, last_name = @last_name, middle_name = @middle_name where id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@first_name", employee.firstName));
            command.Parameters.Add(new SqliteParameter("@last_name", employee.lastName));
            command.Parameters.Add(new SqliteParameter("@middle_name", employee.middleName));
            command.Parameters.Add(new SqliteParameter("@id", employee.id));

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

        public Status Update(Order order)
        {
            SqliteCommand command = new SqliteCommand(
                "update employees " +
                "set number = @number, employee_id = @employee_id, date = @date where id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@number", order.number));
            command.Parameters.Add(new SqliteParameter("@employee_id", order.employeeID));
            command.Parameters.Add(new SqliteParameter("@date", order.date));
            command.Parameters.Add(new SqliteParameter("@id", order.id));

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

        public Status Update(MList mlist)
        {
            SqliteCommand command = new SqliteCommand(
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
                "where id = @id;",
                this._connection);
            command.Parameters.Add(new SqliteParameter("@date_create", mlist.dateCreate));
            command.Parameters.Add(new SqliteParameter("@date_begin", mlist.dateBegin));
            command.Parameters.Add(new SqliteParameter("@end_date", mlist.dateEnd));
            command.Parameters.Add(new SqliteParameter("@coach_date", mlist.dateCoach));
            command.Parameters.Add(new SqliteParameter("@pass_gun_date", mlist.datePassGun));
            command.Parameters.Add(new SqliteParameter("@print_date", mlist.datePrint));
            command.Parameters.Add(new SqliteParameter("@notes", mlist.notes));
            command.Parameters.Add(new SqliteParameter("@deep_time", mlist.timeDeep));
            command.Parameters.Add(new SqliteParameter("@arrive_time", mlist.timeArrive));
            command.Parameters.Add(new SqliteParameter("@pass_gun_time", mlist.timePassGun));
            command.Parameters.Add(new SqliteParameter("@num_mlist", mlist.numberMlist));
            command.Parameters.Add(new SqliteParameter("@id", mlist.id));

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

        // delete Mlist
        public Status Delete(string DataBase, long id)
        {
            string sqlExpression = "DELETE FROM " + DataBase + " WHERE id = @id";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", id);
            command.Parameters.Add(idParam);

            try
            {
                int number = command.ExecuteNonQuery();
                if (number == 0)
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