using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using MList.Storage;
using MList.Storage.Container;
using MList.Forms.CustomizeForms;

namespace MList.Forms.TableForm
{
    public partial class TableFormEmployee : TableFormTemplate
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

        public TableFormEmployee()
        {
            InitializeComponent();

            Employee.initTable(this.dataGridView1);

            this.Text = "Сотрудники";
            this.items = new Dictionary<int, iConteiner>();
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
            return new CustomizeInputForm(
                        new CustomizeInputFormContainerEmployee(this.items[rowIndex] as Employee));
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Employee.Delete(this.items[row.Index] as Employee);
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Удаление элемента не удалось",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }
        }
        protected override void updateGrid()
        {
            try
            {
                this.items = Employee.fillTable(
                    this.dataGridView1,
                    this.textBox1.Text.Length > 0 ?
                        Employee.Get(this.textBox1.Text).Cast<iConteiner>().ToList() : Employee.Get().Cast<iConteiner>().ToList());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }
    }
}
