using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

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

        private SqLiteStorage()
        {
        }

        public static SqLiteStorage getInstance()
        {
            if (instance == null)
                instance = new SqLiteStorage();
            return instance;
        }

        public Status initConnection()
        {
            // получить путь /*Users*/Public/MList/BD
            // проверить есть ли бд
            // создать или открыть
            // 
            return Status.OK;
        }

        public Status export(string path)
        {
            return Status.OK;
        }

        public Status import(string path)
        {
            return Status.OK;
        }

        public Status get(out List<Address> adresses)
        {
            adresses = new List<Address>();

            string sqlExpression = "SELECT (id, address) FROM addresses";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
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
            }

            return Status.OK;
        }

        public Status get(out List<Car> cars)
        {
            cars = new List<Car>();
            string sqlExpression = "SELECT (id, brand, number) FROM cars";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
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
            }

            return Status.OK;
        }

        public Status get(out List<Gun> guns)
        {
            guns = new List<Gun>();
            string sqlExpression = "SELECT (id, brand, series, number, ammo) FROM guns";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        Gun gun = new Gun
                        {
                            id = reader.GetInt64(0),
                            brand = reader.GetString(1),
                            series = reader.GetString(2),
                            number =  reader.GetInt64(3),
                            ammo = reader.GetString(4)
                        };
                        guns.Add(gun);
                    }
                }
            }
            return Status.OK;
        }

        public Status get(out List<Employee> employees)
        {
            employees = new List<Employee>();
            
            string sqlExpression = "SELECT (id, first_name, last_name, middle_name) FROM employees";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        Employee employee = new Employee()
                        {
                            id = reader.GetInt64(0),
                            firstName = reader.GetString(1),
                            lastName = reader.GetString(2),
                            middleName =  reader.GetString(3),
                        };
                        employees.Add(employee);
                    }
                }
            }
            return Status.OK;
        }

        public Status get(out List<Order> orders)
        {
            orders = new List<Order>();
            string sqlExpression = "SELECT (id, number, employee_id, 'date') FROM employees";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        Order order = new Order()
                        {
                            id = reader.GetInt64(0),
                            number = reader.GetInt64(1),
                            employeeID = reader.GetInt64(2),
                            date =  reader.GetInt64(3),
                        };
                        orders.Add(order);
                    }
                }
            }
            return Status.OK;
        }

        // MLIST add

        public Status add(Address adress)
        {
            string sqlExpression = "INSERT INTO addresses (address) VALUES (@address)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            SqliteParameter addressParam = new SqliteParameter("@address", adress.address);
            command.Parameters.Add(addressParam);
            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            return Status.OK;
        }

        public Status add(Car car)
        {
            string sqlExpression = "INSERT INTO cars (brand, number) VALUES (@brand, @number)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            SqliteParameter brandParam = new SqliteParameter("@brand", car.brand);
            command.Parameters.Add(brandParam);
            SqliteParameter numberParam = new SqliteParameter("@number", car.number);
            command.Parameters.Add(numberParam);
            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            return Status.OK;
        }

        public Status add(Gun gun)
        {
            string sqlExpression = "INSERT INTO guns (brand, series, number, ammo) VALUES (@brand, @series, @number, @ammo)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter brandParam = new SqliteParameter("@brand", gun.brand);
            command.Parameters.Add(brandParam);
            
            SqliteParameter seriesParam = new SqliteParameter("@series", gun.series);
            command.Parameters.Add(seriesParam);
            
            SqliteParameter numberParam = new SqliteParameter("@number", gun.number);
            command.Parameters.Add(numberParam);
            
            SqliteParameter ammoParam = new SqliteParameter("@ammo", gun.ammo);
            command.Parameters.Add(ammoParam);
            
            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status add(Employee employee)
        {
            string sqlExpression = "INSERT INTO employees (first_name, last_name, middle_name) VALUES (@first_name, @last_name, @middle_name)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter firstNameParam = new SqliteParameter("@first_name", employee.firstName);
            command.Parameters.Add(firstNameParam);
            
            SqliteParameter lastNameParam = new SqliteParameter("@last_name", employee.lastName);
            command.Parameters.Add(lastNameParam);
            
            SqliteParameter middleNameParam = new SqliteParameter("@middle_name", employee.middleName);
            command.Parameters.Add(middleNameParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status add(Order order)
        {
            string sqlExpression = "INSERT INTO orders (number, employee_id, date) VALUES (@number, @employee_id, @date)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter numberParam = new SqliteParameter("@number", order.number);
            command.Parameters.Add(numberParam);
            
            SqliteParameter employeeIdParam = new SqliteParameter("@employee_id", order.employeeID);
            command.Parameters.Add(employeeIdParam);
            
            SqliteParameter dateParam = new SqliteParameter("@date", order.date);
            command.Parameters.Add(dateParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        // add Mlist

        public Status delete(Address address)
        {
            string sqlExpression = "DELETE FROM addresses WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter idParam = new SqliteParameter("@id", address.id);
            command.Parameters.Add(idParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status delete(Car car)
        {
            string sqlExpression = "DELETE FROM cars WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter idParam = new SqliteParameter("@id", car.id);
            command.Parameters.Add(idParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status delete(Gun gun)
        {
            string sqlExpression = "DELETE FROM guns WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter idParam = new SqliteParameter("@id", gun.id);
            command.Parameters.Add(idParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status delete(Employee employee)
        {
            string sqlExpression = "DELETE FROM employees WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter idParam = new SqliteParameter("@id", employee.id);
            command.Parameters.Add(idParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        public Status delete(Order order)
        {
            string sqlExpression = "DELETE FROM orders WHERE id = @id)";

            SqliteCommand command = new SqliteCommand(sqlExpression, this._connection);
            
            SqliteParameter idParam = new SqliteParameter("@id", order.id);
            command.Parameters.Add(idParam);

            int number = command.ExecuteNonQuery();
            if (number == 0)
            {
                return Status.ERROR;
            }
            
            return Status.OK;
        }

        //delete MList
    }
}