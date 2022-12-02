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
        public List<Tuple<String, object>> getByParametrList()
        {
            return new List<Tuple<String, object>> {
                    new Tuple<String, object>("@first_name", this.firstName),
                    new Tuple<String, object>("@last_name", this.lastName),
                    new Tuple<String, object>("@middle_name", this.middleName) };
        }
        public List<Tuple<String, object>> getByParametrListWithId()
        {
            List<Tuple<String, object>> l = getByParametrList();
            l.Add(new Tuple<String, object>("@id", this.id));
            return l;
        }
        static public List<Employee> Get()
        {
            List<Employee> employees = new List<Employee>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM employees", null, "Search employees.");

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
                employee.getByParametrList(),
                "Update Employee.");
        }
        static public void Delete(Employee employee)
        {
            SqLite.Delete("employees", employee.id);
        }
    }
}
