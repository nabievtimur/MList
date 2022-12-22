using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MList.Storage;
using MList.Storage.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormOrder : Form
    {
        private Order order;
        private List<Tuple<Employee, int>> itemsEmployee;
        private List<Tuple<Gun, int>> itemsGun;
        private List<Gun> itemsPickedGun;
        public TableFormOrder() 
        {
            InitializeComponent();

            Employee.initTable(this.dataGridView1);
            Gun.initTable(this.dataGridView2);
            Gun.initTable(this.dataGridView3);

            this.itemsEmployee = new List<Tuple<Employee, int>>();
            this.itemsGun = new List<Tuple<Gun, int>>();
            this.itemsPickedGun = new List<Gun>();
            this.order = new Order { 
                id = -1 };
            try
            {
                this.textBox1.Text = Order.GetNextOrderNum().ToString();
            }
            catch (QueryExeption) { };
            
            this.Text = "Добавить";
        }

        public TableFormOrder(Order order) :
            this()
        {
            this.textBox1.Text = order.number.ToString();
            this.dateTimePicker1.Value = new DateTime(order.date);
            try
            {
                this.itemsPickedGun = Gun.Get();
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
            this.UpdatePickedGunGrid();

            this.dataGridView1.SelectedRows.Clear();
            foreach (var item in this.itemsEmployee)
            {
                if (item.Item1.id == order.employeeID)
                {
                    this.dataGridView1.Rows[item.Item2].Selected = true;
                }
            }

            this.Text = "Изменить";
        }

        private void TableFormOrder_Load(object sender, EventArgs e) 
        {
            this.UpdateEmloyeeGrid();
            this.UpdateGunGrid();
        }

        private void UpdateEmloyeeGrid() 
        {
            this.dataGridView1.Rows.Clear();
            this.itemsEmployee.Clear();
            int i = 0;
            try
            {
                foreach (Employee employee in Employee.Get())
                {
                    this.itemsEmployee.Add(new Tuple<Employee, int>(employee, i));
                    if (i >= dataGridView1.Rows.Count)
                        this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[i].Cells[0].Value = employee.lastName;
                    this.dataGridView1.Rows[i].Cells[1].Value = employee.firstName;
                    this.dataGridView1.Rows[i].Cells[2].Value = employee.middleName;
                    i++;
                }
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }

        private void UpdateGunGrid()
        {
            this.dataGridView2.Rows.Clear();
            this.itemsGun.Clear();
            int i = 0x00;
            try
            {
                foreach (Gun gun in Gun.Get())
                {
                    this.itemsGun.Add(new Tuple<Gun, int>(gun, i));
                    this.dataGridView2.Rows.Add();
                    this.dataGridView2.Rows[i].Cells[0].Value = gun.brand;
                    this.dataGridView2.Rows[i].Cells[1].Value = gun.series;
                    this.dataGridView2.Rows[i].Cells[2].Value = gun.number;
                    this.dataGridView2.Rows[i].Cells[3].Value = gun.ammo;
                    i++;
                }
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }

        private void UpdatePickedGunGrid()
        {
            this.dataGridView3.Rows.Clear();
            int i = 0x00;
            foreach (Gun gun in this.itemsPickedGun)
            {
                if (i >= dataGridView3.Rows.Count)
                    this.dataGridView3.Rows.Add();
                this.dataGridView3.Rows[i].Cells[0].Value = gun.brand;
                this.dataGridView3.Rows[i].Cells[1].Value = gun.series;
                this.dataGridView3.Rows[i].Cells[2].Value = gun.number;
                this.dataGridView3.Rows[i].Cells[3].Value = gun.ammo;
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView2.SelectedRows.Count == 0)
            {
                return;
            }
            foreach (Tuple<Gun, int> item in this.itemsGun)
            {
                foreach (DataGridViewRow row in this.dataGridView2.SelectedRows)
                {
                    if (item.Item2 == row.Index)
                    {
                        this.itemsPickedGun.Add(item.Item1);
                    }
                }
            }
            this.UpdatePickedGunGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView3.SelectedRows.Count == 0)
            {
                return;
            }
            foreach (DataGridViewRow row in this.dataGridView3.SelectedRows)
            {
                this.itemsPickedGun.RemoveAt(row.Index);
            }
            this.UpdatePickedGunGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного сотрудника.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                this.DialogResult = DialogResult.Cancel;
            }

            int index = this.dataGridView1.SelectedRows[0].Index;
            if (index == -1)
            {
                MessageBox.Show(
                    "Не выбран сотрудник",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            if (this.dataGridView3.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Не выбрано оружие",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            try
            {
                Order.Add(
                    new Order {
                        id = 0,
                        number = int.Parse(this.textBox1.Text),
                        employeeID = this.itemsEmployee[index].Item1.id,
                        date = this.dateTimePicker1.Value.Ticks,
                        employeeFullName = "" },
                    this.itemsPickedGun);
            }
            catch(FormatException)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Неверный номер приказа.",
                    MessageBoxButtons.OK);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Не заполнен номер приказа.",
                    MessageBoxButtons.OK);
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
