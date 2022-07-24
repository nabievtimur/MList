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
        }

        public struct MList
        {
            public long id;
            public long dateCreate;
            public long dateBegin;
            public long dateEnd;
            public long dateCoach;
            public long datePassGun;
            public long datePring;
            public string notes;
            public long timeDeep;
            public long timeArrive;
            public long timePassGun;
            public long numberMlist;
        }

        private SqLiteStorage() { }
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
            }

            string dbFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MList\\DataBase\\mlist.db");

            if (!File.Exists(dbFilePath))
            {
                this._connection = new SqliteConnection(
                    string.Format("Data Source={0};Cache=Shared;Mode=ReadWriteCreate;Foreign Keys=True;",
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

            this._connection = new SqliteConnection(
                string.Format("Data Source={0};Cache=Shared;Mode=ReadWrite;Foreign Keys=True;", 
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
            employees.Add(new Employee() { id = 1, firstName = "Первый", lastName = "Численный", middleName = "Большой" });
            employees.Add(new Employee() { id = 2, firstName = "Второй", lastName = "Порядковый", middleName = "Маленький" });
            employees.Add(new Employee() { id = 3, firstName = "Третий", lastName = "Перечислительный", middleName = "Средний" });
            return Status.OK;

            SqliteCommand command = new SqliteCommand(
                "SELECT (id, first_name, last_name, middle_name) FROM employees", 
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }
        public Status Get(out List<Order> orders)
        {
            orders = new List<Order>();
            SqliteCommand command = new SqliteCommand(
                "SELECT (id, number, employee_id, 'date') FROM employees", 
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
                        employeeID = reader.GetInt64(2),
                        date = reader.GetInt64(3),
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
        public Status Get(out List<MList> orders)
        {
            throw new NotImplementedException();
        }
        public Status GetByEmployee(Employee emp, out List<Gun> orders)
        {
            throw new NotImplementedException();
        }
        // MLIST add
        public Status Add(Address adress)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO addresses (address) VALUES (@address)", 
                this._connection);
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
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO cars (brand, number) VALUES (@brand, @number)", 
                this._connection);
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
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO guns (brand, series, number, ammo) VALUES (@brand, @series, @number, @ammo)", 
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
                Console.WriteLine(e.ToString());
                return Status.ERROR;
            }
        }
        public Status Add(Employee employee)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO employees (first_name, last_name, middle_name) VALUES (@first_name, @last_name, @middle_name)", 
                this._connection);

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
        public Status Add(MList mlist)
        {
            throw new NotImplementedException();
        }
        // Mlist update
        public Status Update(Address adress)
        {
            SqliteCommand command = new SqliteCommand(
                "INSERT INTO addresses (address) VALUES (@address)",
                this._connection);
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
        public Status Update(Car car)
        {
            throw new NotImplementedException();
        }
        public Status Update(Gun gun)
        {
            throw new NotImplementedException();
        }
        public Status Update(Employee employee)
        {
            throw new NotImplementedException();
        }
        public Status Update(Order order)
        {
            throw new NotImplementedException();
        }
        public Status Update(MList mlist)
        {
            throw new NotImplementedException();
        }
        // delete Mlist
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
        public Status Delete(MList mlist)
        {
            throw new NotImplementedException();
        }
    }
}