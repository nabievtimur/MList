using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;
using MList.Storage;
using MList.Storage.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormEmployee : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerEmployee :
            CustomizeInputFormContainer
        {
            Employee employee;
            public CustomizeInputFormContainerEmployee(Employee employee) :
                base(employee.id == -1 ? "Добавить" : "Изменить")
            {
                this.employee = employee;
            }
            public override bool check(ref List<Tuple<Label, TextBox>> lItems)
            {
                return true;
            }
            public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
            {
                {
                    Label label = new Label();
                    label.Text = "Фамилия";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.lastName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Имя";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.firstName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Отчество";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.middleName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.employee.id == -1)
                {
                    try
                    {
                        Employee.Add(new Employee {
                            id = 0,
                            firstName = lItems[1].Item2.Text,
                            lastName = lItems[0].Item2.Text,
                            middleName = lItems[2].Item2.Text } );
                    }
                    catch(QueryExeption)
                    {
                        MessageBox.Show(
                            "Добавления в базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    try
                    {
                        Employee.Update(new Employee {
                            id = this.employee.id,
                            firstName = lItems[1].Item2.Text,
                            lastName = lItems[0].Item2.Text,
                            middleName = lItems[2].Item2.Text } );
                    }
                    catch (QueryExeption e)
                    {
                        MessageBox.Show(
                            "Обновления базы данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }

                return DialogResult.OK;
            }
        }

        private List<Tuple<Employee, int>> items;

        public TableFormEmployee()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("lastName", "Фамилия");
            this.dataGridView1.Columns.Add("firstName", "Имя");
            this.dataGridView1.Columns.Add("middleName", "Отчество");

            this.Text = "Сотрудники";
            this.items = new List<Tuple<Employee, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerEmployee(
                    new Employee
                    {
                        id = -1,
                        firstName = "",
                        middleName = "",
                        lastName = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Employee, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerEmployee(
                            new Employee
                            {
                                id = item.Item1.id,
                                firstName = item.Item1.firstName,
                                middleName = item.Item1.middleName,
                                lastName = item.Item1.lastName
                            }));
                }
            }
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                foreach (Tuple<Employee, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Employee.Delete(item.Item1);
                        }
                        catch (QueryExeption e)
                        {
                            MessageBox.Show(
                                "Удаление элемента не удалось",
                                "Ошибка",
                                MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }
        protected override void updateGrid()
        {
            List<Employee> list = new List<Employee>();
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0;

            try
            {
                list = this.textBox1.Text.Length > 0 ?
                    Employee.Get(this.textBox1.Text) : Employee.Get();
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }

            foreach (Employee employee in list)
            {
                this.items.Add(new Tuple<Employee, int>(employee, i));
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = employee.lastName;
                this.dataGridView1.Rows[i].Cells[1].Value = employee.firstName;
                this.dataGridView1.Rows[i].Cells[2].Value = employee.middleName;
                i++;
            }
        }
    }
}
