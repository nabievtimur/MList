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

namespace MList.Forms.TableForm
{
    public partial class TableFormOrder : Form
    {
        private SqLiteStorage.Order order;
        private List<Tuple<SqLiteStorage.Employee, int>> itemsEmployee;
        private List<Tuple<SqLiteStorage.Gun, int>> itemsGun;
        private List<SqLiteStorage.Gun> itemsPickedGun;
        public TableFormOrder() 
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("middleName", "Фамилия");
            this.dataGridView1.Columns.Add("firstName", "Имя");
            this.dataGridView1.Columns.Add("lastName", "Отчество");

            this.dataGridView2.Columns.Add("brand", "Брэнд");
            this.dataGridView2.Columns.Add("series", "Серия");
            this.dataGridView2.Columns.Add("number", "Номер");
            this.dataGridView2.Columns.Add("ammo", "Патроны");

            this.dataGridView3.Columns.Add("brand", "Брэнд");
            this.dataGridView3.Columns.Add("series", "Серия");
            this.dataGridView3.Columns.Add("number", "Номер");
            this.dataGridView3.Columns.Add("ammo", "Патроны");

            this.itemsEmployee = new List<Tuple<SqLiteStorage.Employee, int>>();
            this.itemsGun = new List<Tuple<SqLiteStorage.Gun, int>>();
            this.itemsPickedGun = new List<SqLiteStorage.Gun>();
            this.order = new SqLiteStorage.Order { 
                id = -1
            };

            this.Text = "Добавить";
        }

        public TableFormOrder(SqLiteStorage.Order order) :
            this()
        {
            SqLiteStorage.Status status= SqLiteStorage.Status.OK;
            this.textBox1.Text = order.number.ToString();
            this.dateTimePicker1.Value = new DateTime(order.date);
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(
                out this.itemsPickedGun)))
            {
                if (status != SqLiteStorage.Status.NO_ROWS)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
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

        private void UpdateEmloyeeGrid() {
            List<SqLiteStorage.Employee> list = new List<SqLiteStorage.Employee>();
            SqLiteStorage.Status status = SqLiteStorage.Status.OK;
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(out list))) 
            {
                if (status != SqLiteStorage.Status.NO_ROWS) 
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            this.dataGridView1.Rows.Clear();
            this.itemsEmployee.Clear();
            int i = 0;
            foreach (SqLiteStorage.Employee employee in list) 
            {
                this.itemsEmployee.Add(new Tuple<SqLiteStorage.Employee, int>(employee, i));
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = employee.firstName;
                this.dataGridView1.Rows[i].Cells[1].Value = employee.middleName;
                this.dataGridView1.Rows[i].Cells[2].Value = employee.lastName;
                i++;
            }
        }

        private void UpdateGunGrid()
        {
            List<SqLiteStorage.Gun> list = new List<SqLiteStorage.Gun>();
            SqLiteStorage.Status status = SqLiteStorage.Status.OK;
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(out list)))
            {
                if (status != SqLiteStorage.Status.NO_ROWS)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            this.dataGridView2.Rows.Clear();
            this.itemsGun.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.Gun gun in list)
            {
                this.itemsGun.Add(new Tuple<SqLiteStorage.Gun, int>(gun, i));
                if (i >= dataGridView2.Rows.Count)
                    this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[i].Cells[0].Value = gun.brand;
                this.dataGridView2.Rows[i].Cells[1].Value = gun.series;
                this.dataGridView2.Rows[i].Cells[2].Value = gun.number;
                this.dataGridView2.Rows[i].Cells[3].Value = gun.ammo;
                i++;
            }
        }

        private void UpdatePickedGunGrid()
        {
            this.dataGridView3.Rows.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.Gun gun in this.itemsPickedGun)
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
            foreach (Tuple<SqLiteStorage.Gun, int> item in this.itemsGun)
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
    }
}
