using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public class Employee : iConteiner
    {
        public string firstName;
        public string lastName;
        public string middleName;
        public override DataGridViewRow fillRow(DataGridViewRow row)
        {
            return row;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            {
                Label label = new Label();
                label.Text = "Фамилия";
                TextBox textBox = new TextBox();
                textBox.Text = this.lastName;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }

            {
                Label label = new Label();
                label.Text = "Имя";
                TextBox textBox = new TextBox();
                textBox.Text = this.firstName;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }

            {
                Label label = new Label();
                label.Text = "Отчество";
                TextBox textBox = new TextBox();
                textBox.Text = this.middleName;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }
        }
        static private List<Employee> Read(SqliteDataReader reader)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    employees.Add(new Employee()
                    {
                        id = reader.GetInt64(0),
                    });
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw new QueryExeption();
            }

            return employees;
        }
        static public List<Employee> Get()
        {
            return Employee.Read(SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM employees",
                new List<SqliteParameter>(),
                "Search employees."));
        }

        static public List<Employee> Get(String search)
        {
            return Employee.Read(SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM employees " +
                    "WHERE first_name LIKE @like OR last_name LIKE @like OR middle_name LIKE @like " +
                    "ORDER BY id;",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", "%" + search + "%") },
                "Search employee."));
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
        static public void initTable(DataGridView table)
        {
            table.Columns.Add("lastName", "Фамилия");
            table.Columns.Add("firstName", "Имя");
            table.Columns.Add("middleName", "Отчество");
        }
    }
}
