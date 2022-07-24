using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;
using MList.Storage;

namespace MList.Forms.TableForm
{
    public partial class TableFormEmployee : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerEmployee :
            CustomizeInputFormContainer
        {
            SqLiteStorage.Employee employee;
            public CustomizeInputFormContainerEmployee(SqLiteStorage.Employee employee) :
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
                    label.Text = "Имя";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.firstName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Фамилия";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.middleName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Отчество";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.employee.lastName;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.employee.id == -1)
                {
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Add(
                        new SqLiteStorage.Employee
                        {
                            id = 0,
                            firstName = lItems[0].Item2.Text,
                            middleName = lItems[1].Item2.Text,
                            lastName = lItems[2].Item2.Text
                        }))
                    {
                        MessageBox.Show(
                            "Ошибка",
                            "Добавления в базу данных",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Update(
                        new SqLiteStorage.Employee
                        {
                            id = 0,
                            firstName = lItems[0].Item2.Text,
                            middleName = lItems[1].Item2.Text,
                            lastName = lItems[2].Item2.Text
                        }))
                    {
                        MessageBox.Show(
                            "Ошибка",
                            "Обновления базы данных",
                            MessageBoxButtons.OK);
                    }
                }

                return DialogResult.OK;
            }
        }

        private List<Tuple<SqLiteStorage.Employee, int>> items;

        public TableFormEmployee()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("middleName", "Фамилия");
            this.dataGridView1.Columns.Add("firstName", "Имя");
            this.dataGridView1.Columns.Add("lastName", "Отчество");

            this.Text = "Сотрудники";
            this.items = new List<Tuple<SqLiteStorage.Employee, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerEmployee(
                    new SqLiteStorage.Employee
                    {
                        id = -1,
                        firstName = "",
                        middleName = "",
                        lastName = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            //this.dataGridView1.
            return new CustomizeInputForm(
                new CustomizeInputFormContainerEmployee(
                    new SqLiteStorage.Employee
                    {
                        id = -1,
                        firstName = "",
                        middleName = "",
                        lastName = ""
                    }));
        }
        protected override void delete()
        {

        }
        protected override void updateGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormAddress::updateGrid");
            List<SqLiteStorage.Employee> list = new List<SqLiteStorage.Employee>();
            SqLiteStorage.Status status = SqLiteStorage.Status.OK;
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(out list)))
            {
                if (status != SqLiteStorage.Status.NO_ROWS)
                {
                    MessageBox.Show(
                    "Ошибка",
                    "Чтение из базы данных",
                    MessageBoxButtons.OK);
                }
            }

            this.items.Clear();
            int i = 0;
            foreach (SqLiteStorage.Employee employee in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.Employee, int>(employee, i));
                System.Diagnostics.Debug.WriteLine(
                    employee.firstName + " " + 
                    employee.middleName + " " + 
                    employee.lastName);
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = employee.firstName;
                this.dataGridView1.Rows[i].Cells[1].Value = employee.middleName;
                this.dataGridView1.Rows[i].Cells[2].Value = employee.lastName;
                i++;
            }
        }
    }
}
