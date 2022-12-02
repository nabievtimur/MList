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
    public partial class TableFormOrders : Form
    {
        private List<Tuple<Order, int>> items;
        public TableFormOrders()
        {
            InitializeComponent();

            this.items = new List<Tuple<Order, int>>();

            this.Text = "Приказы о закреплении оружия";
            this.dataGridView1.Columns.Add("number", "Номер");
            this.dataGridView1.Columns.Add("date", "Дата");
            this.dataGridView1.Columns.Add("employeeFullName", "Сотрудник");

            this.dataGridView2.Columns.Add("brand", "Брэнд");
            this.dataGridView2.Columns.Add("series", "Серия");
            this.dataGridView2.Columns.Add("number", "Номер");
            this.dataGridView2.Columns.Add("ammo", "Патроны");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TableFormOrder form = new TableFormOrder();
            form.ShowDialog();
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
            foreach(var item in this.items)
            {
                if (item.Item2 == this.dataGridView1.SelectedRows[0].Index)
                {
                    TableFormOrder form = new TableFormOrder(item.Item1);
                    form.ShowDialog();
                }
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                foreach (Tuple<Order, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Order.Delete(item.Item1);
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
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.updateSubGrid();
        }

        protected void updateGrid()
        {
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            try
            {
                foreach (Order order in Order.Get())
                {
                    this.items.Add(new Tuple<Order, int>(order, i));
                    if (i >= dataGridView1.Rows.Count)
                        this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[i].Cells[0].Value = order.number;
                    this.dataGridView1.Rows[i].Cells[1].Value = order.date;
                    this.dataGridView1.Rows[i].Cells[2].Value = order.employeeFullName;
                    i++;
                }
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
        }

        protected void updateSubGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormOrders::updateSubGrid");
            this.dataGridView2.Rows.Clear();

            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Order, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    int i = 0x00;
                    try
                    {
                        foreach (Gun gun in Gun.Get())
                        {
                            if (i >= dataGridView1.Rows.Count)
                                this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[i].Cells[0].Value = gun.brand;
                            this.dataGridView1.Rows[i].Cells[1].Value = gun.series;
                            this.dataGridView1.Rows[i].Cells[2].Value = gun.number;
                            this.dataGridView1.Rows[i].Cells[3].Value = gun.ammo;
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
            }            
        }
    }
}
