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
    public partial class TableFormOrders : Form
    {
        private Dictionary<int, Order> items;
        public TableFormOrders()
        {
            InitializeComponent();

            Order.initTable(this.dataGridView1);
            Gun.initTable(this.dataGridView2);

            this.Text = "Приказы о закреплении оружия";
            this.items = new Dictionary<int, Order>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TableFormOrder form = new TableFormOrder();
            form.ShowDialog();
            this.updateGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show(
                        "Не выбрано ни одной строки",
                        "Ошибка",
                        MessageBoxButtons.OK);
                return;
            }

            TableFormOrder form = new TableFormOrder(this.items[this.dataGridView1.SelectedRows[0].Index]);
            form.ShowDialog();
            this.updateGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Order.Delete(this.items[row.Index]);
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Удаление элемента не удалось",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }
            this.updateGrid();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.updateGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.updateSubGrid();
        }

        protected void updateGrid()
        {
            List<Order> list = new List<Order>();
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;

            try
            {
                list = this.textBox1.Text.Length > 0 ?
                    Order.Get(this.textBox1.Text) : Order.Get();
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }

            foreach (Order order in list)
            {
                this.items.Add(i, order);
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = order.number;
                this.dataGridView1.Rows[i].Cells[1].Value = new DateTime(order.date).ToString();
                this.dataGridView1.Rows[i].Cells[2].Value = order.employeeFullName;
                i++;
            }
            this.updateSubGrid();
        }

        protected void updateSubGrid()
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    Gun.fillTable(
                        this.dataGridView2,
                        Gun.GetCurrent(this.items[this.dataGridView1.SelectedRows[0].Index]).Cast<iConteiner>().ToList());
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

        private void TableFormOrders_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
    }
}
