using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Employee
    {
        public long id;
        public string firstName;
        public string lastName;
        public string middleName;
        public List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                    new SqliteParameter("@first_name", this.firstName),
                    new SqliteParameter("@last_name", this.lastName),
                    new SqliteParameter("@middle_name", this.middleName) };
        }
        public List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
        }
        static public List<Employee> Get()
        {
            List<Employee> employees = new List<Employee>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM employees",
                new List<SqliteParameter>(), 
                "Search employees.");

            while (reader.Read()) // построчно считываем данные
            {
                employees.Add(new Employee() {
                    id = reader.GetInt64(0),
                    firstName = reader.GetString(1),
                    lastName = reader.GetString(2),
                    middleName = reader.GetString(3) } );
            }

            return employees;
        }
        static public List<Employee> Get(String search) // NOT WORK
        {
            List<Employee> employees = new List<Employee>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM employees " +
                    "WHERE first_name LIKE '%@like%' OR last_name LIKE '%@like%' OR middle_name LIKE '%@like%' " +
                    "ORDER BY id;",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", search)},
                "Search employee.");

            while (reader.Read()) // построчно считываем данные
            {
                employees.Add(new Employee()
                {
                    id = reader.GetInt64(0),
                    firstName = reader.GetString(1),
                    lastName = reader.GetString(2),
                    middleName = reader.GetString(3)
                });
            }

            return employees;
        }
        static public void Add(Employee employee)
        {
            SqLite.exec(
                "INSERT INTO employees (first_name, last_name, middle_name) VALUES (@first_name, @last_name, @middle_name)", 
                employee.getByParametrList(),
                "Add new Employee.");
        }
        static public void Update(Employee employee)
        {
            SqLite.exec(
                "UPDATE employees SET first_name = @first_name, last_name = @last_name, middle_name = @middle_name WHERE id = @id",
                employee.getByParametrListWithId(),
                "Update Employee.");
        }
        static public void Delete(Employee employee)
        {
            SqLite.Delete("employees", employee.id);
        }
    }
}
