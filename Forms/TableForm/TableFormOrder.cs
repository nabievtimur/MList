using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MList.Storage;
using MList.Storage.Table;
using MList.Storage.Table.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormOrder : Form
    {
        private ContainerCollection<ContainerGun> guns;

        public TableFormOrder() 
        {
            InitializeComponent();

            new TableEmployee().gridInit(this.dataGridView1);
            new TableGun().gridInit(this.dataGridView2);
            new TableGun().gridInit(this.dataGridView3);
            this.guns = new ContainerCollection<ContainerGun>();
            
            this.Text = "Добавить";
        }

        public TableFormOrder(ContainerOrder order) :
            this()
        {
            this.textBox1.Text = order.getNumber().ToString();
            this.dateTimePicker1.Value = new DateTime(order.getDate());

            try
            {
                new TableGun().gridFill(this.dataGridView1,
                    new TableGun().storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
            this.UpdatePickedGunGrid();

            //this.dataGridView1.SelectedRows.Clear();
            //foreach (var item in this.itemsEmployee)
            //{
            //    if (item.Item1.id == order.employeeID)
            //    {
            //        this.dataGridView1.Rows[item.Item2].Selected = true;
            //    }
            //}

            this.Text = "Изменить";
        }

        private void TableFormOrder_Load(object sender, EventArgs e) 
        {
            this.UpdateEmloyeeGrid();
            this.UpdateGunGrid();
        }

        private void UpdateEmloyeeGrid() 
        {
            try
            {
                new TableEmployee().gridFill(this.dataGridView1,
                    new TableEmployee().storageGet());
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
            try
            {
                new TableGun().gridFill(this.dataGridView2,
                    new TableGun().storageGet());
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
            new TableGun().gridFill(this.dataGridView3, this.guns.downCast());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView2.SelectedRows.Count == 0)
            {
                return;
            }
            foreach (DataGridViewRow row in this.dataGridView2.SelectedRows)
            {
                this.guns.Add(new ContainerGun(row));
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
                this.guns.Remove(new ContainerGun(row));
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
                new TableOrder().storageAdd(
                    new ContainerOrder(
                        0,
                        int.Parse(this.textBox1.Text),
                        new ContainerEmployee(this.dataGridView1.SelectedRows[0]).getId(),
                        this.dateTimePicker1.Value.Ticks,
                        ""),
                    this.guns);
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
