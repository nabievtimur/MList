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

            // public Cars()
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
            string dbFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MList\\DataBase");
            Console.WriteLine(dbFolderPath);
            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
                try
                {
                    Directory.CreateDirectory(dbFolderPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }

            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MList\\DataBase\\mlist.db");

            if (!File.Exists(dbFilePath))
            {
                try
                {
                    this._connection = new SqliteConnection(string.Format("Data Source={0};Cache=Shared;Mode=ReadWriteCreate;Foreign Keys=True;",
                        dbFilePath));
                    this._connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
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
                    Console.WriteLine(e.ToString());
                    return Status.ERROR;
                }
            }
            this._connection = new SqliteConnection(string.Format("Data Source={0};Cache=Shared;Mode=ReadWrite;Foreign Keys=True;",
                dbFilePath));
            try
            {
                this._connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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

            string sqlExpression = @"
select gn.id,
       gn.number,
       gn.ammo,
       gn.series,
       gn.brand
from guns as gn
         join order_gun og on gn.id = og.gun_id
where og.order_id = @order_id
";
            
            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
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
                Console.WriteLine(e.ToString());
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


            string sqlExpression = @"
select cr.id, cr.brand, cr.number
from cars as cr
         join mlist_cars mc on cr.id = mc.car_id
where mc.mlist_id = @mlist_id
";
            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }

            sqlExpression = @"
select gn.id, gn.number, gn.brand, gn.series, gn.ammo
from guns as gn
         join mlist_gun mg on gn.id = mg.gun_id
where mg.mlist_id = @mlist_id
";
            command = new SqliteCommand(sqlExpression, this._connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }

            sqlExpression = @"
select ad.id,
       ad.address
from addresses as ad
         join mlist_arrive_address maa on ad.id = maa.arrive_address_id
where maa.mlist_id = @mlist_id
";
            
            command = new SqliteCommand(sqlExpression, this._connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }

            sqlExpression = @"
select ad.id,
       ad.address
from addresses as ad
         join mlist_deep_address mda on ad.id = mda.deep_address_id
where mda.mlist_id = @mlist_id
";
            command = new SqliteCommand(sqlExpression, this._connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status Get(out List<MList> mlists)
        {
            mlists = new List<MList>();

            // string sqlExpression = "SELECT (ml.id, ml.num_mlist, ml.date_create, ml.date_begin, ml.date_end, ml.coach_date, ml.pass_gun_date, ml.pass_gun_time, ml.print_date, ml.notes, ml.arrive_time, ml.deep_time) FROM mlist as ml";
            string sqlExpression = @"
select ml.id,
       ml.num_mlist,
       ml.date_create,
       ml.date_begin,
       ml.date_end,
       ml.coach_date,
       ml.pass_gun_date,
       ml.pass_gun_time,
       ml.print_date,
       ml.notes,
       ml.arrive_time,
       ml.deep_time,
       e.id,
       e.last_name,
       e.first_name,
       e.middle_name
from mlist as ml
         JOIN mlist_employees me on ml.id = me.mlist_id
         JOIN employees e on e.id = me.employee_id;
";
            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Address> adresses)
        {
            adresses = new List<Address>();

            string sqlExpression = "SELECT (id, address) FROM addresses";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }

            return Status.OK;
        }

        public Status Get(out List<Car> cars)
        {
            cars = new List<Car>();
            string sqlExpression = "SELECT (id, brand, number) FROM cars";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Gun> guns)
        {
            guns = new List<Gun>();
            string sqlExpression = "SELECT (id, brand, series, number, ammo) FROM guns";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Employee> employees)
        {
            employees = new List<Employee>();

            string sqlExpression = "SELECT (id, first_name, last_name, middle_name) FROM employees";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Get(out List<Order> orders)
        {
            orders = new List<Order>();
            string sqlExpression = @"
select od.id,
       od.number,
       od.date,
       e.id,
       e.last_name,
       e.first_name,
       e.middle_name
from orders as od
         join orders_employees oe on od.id = oe.order_id
         join employees e on e.id = oe.employee_id
         ";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        // MLIST add

        public Status Add(Address adress)
        {
            string sqlExpression = "INSERT INTO addresses (address) VALUES (@address)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            SqliteParameter addressParam = new SqliteParameter("@address", adress.address);
            command.Parameters.Add(addressParam);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Car car)
        {
            string sqlExpression = "INSERT INTO cars (brand, number) VALUES (@brand, @number)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            SqliteParameter brandParam = new SqliteParameter("@brand", car.brand);
            command.Parameters.Add(brandParam);
            SqliteParameter numberParam = new SqliteParameter("@number", car.number);
            command.Parameters.Add(numberParam);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Gun gun)
        {
            string sqlExpression =
                "INSERT INTO guns (brand, series, number, ammo) VALUES (@brand, @series, @number, @ammo)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter brandParam = new SqliteParameter("@brand", gun.brand);
            command.Parameters.Add(brandParam);

            SqliteParameter seriesParam = new SqliteParameter("@series", gun.series);
            command.Parameters.Add(seriesParam);

            SqliteParameter numberParam = new SqliteParameter("@number", gun.number);
            command.Parameters.Add(numberParam);

            SqliteParameter ammoParam = new SqliteParameter("@ammo", gun.ammo);
            command.Parameters.Add(ammoParam);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Employee employee)
        {
            string sqlExpression =
                "INSERT INTO employees (first_name, last_name, middle_name) VALUES (@first_name, @last_name, @middle_name)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter firstNameParam = new SqliteParameter("@first_name", employee.firstName);
            command.Parameters.Add(firstNameParam);

            SqliteParameter lastNameParam = new SqliteParameter("@last_name", employee.lastName);
            command.Parameters.Add(lastNameParam);

            SqliteParameter middleNameParam = new SqliteParameter("@middle_name", employee.middleName);
            command.Parameters.Add(middleNameParam);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Add(Order order)
        {
            string sqlExpression =
                "INSERT INTO orders (number, employee_id, date) VALUES (@number, @employee_id, @date)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter numberParam = new SqliteParameter("@number", order.number);
            command.Parameters.Add(numberParam);

            SqliteParameter employeeIdParam = new SqliteParameter("@employee_id", order.employeeID);
            command.Parameters.Add(employeeIdParam);

            SqliteParameter dateParam = new SqliteParameter("@date", order.date);
            command.Parameters.Add(dateParam);

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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }
        
        public Status Add(long orderID, long employeeID, long gunID)
        {
            string sqlExpression =
                "insert into order_gun (order_id, gun_id) VALUES (@order_id, @gun_id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            command.Parameters.Add(new SqliteParameter("@order_id", orderID));
            command.Parameters.Add(new SqliteParameter("@gun_id", gunID));

            try
            {
                if (command.ExecuteNonQuery() == 0) return Status.ERROR;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }

            sqlExpression = "insert into orders_employees (order_id, employee_id) VALUES (@order_id, @employee_id)";
            command = new SqliteCommand(sqlExpression, this._connection);
            
            command.Parameters.Add(new SqliteParameter("@order_id", orderID));
            command.Parameters.Add(new SqliteParameter("@employee_id", employeeID));
            
            try
            {
                if (command.ExecuteNonQuery() == 0) return Status.ERROR;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        // add Mlist

        public Status Delete(Address address)
        {
            string sqlExpression = "DELETE FROM addresses WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", address.id);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Delete(Car car)
        {
            string sqlExpression = "DELETE FROM cars WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", car.id);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Delete(Gun gun)
        {
            string sqlExpression = "DELETE FROM guns WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", gun.id);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Delete(Employee employee)
        {
            string sqlExpression = "DELETE FROM employees WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", employee.id);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        public Status Delete(Order order)
        {
            string sqlExpression = @"DELETE FROM orders WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);

            SqliteParameter idParam = new SqliteParameter("@id", order.id);
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }

        //delete MList
    }
}