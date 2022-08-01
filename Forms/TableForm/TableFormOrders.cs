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
    public partial class TableFormOrders : Form
    {
        private List<Tuple<SqLiteStorage.Order, int>> items;
        public TableFormOrders()
        {
            InitializeComponent();

            this.items = new List<Tuple<SqLiteStorage.Order, int>>();

            this.Text = "Приказы о закреплении оружия";
            this.dataGridView1.Columns.Add("number", "Номер");
            this.dataGridView1.Columns.Add("date", "дата");
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
                foreach (Tuple<SqLiteStorage.Order, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Delete(item.Item1))
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
            System.Diagnostics.Debug.WriteLine("enter TableFormOrders::updateGrid");
            List<SqLiteStorage.Order> list = new List<SqLiteStorage.Order>();
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
            this.items.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.Order order in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.Order, int>(order, i));
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = order.number;
                this.dataGridView1.Rows[i].Cells[1].Value = order.date;
                this.dataGridView1.Rows[i].Cells[2].Value = order.employeeFullName;
                i++;
            }
        }

        protected void updateSubGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormOrders::updateSubGrid");
            this.dataGridView2.Rows.Clear();

            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<SqLiteStorage.Order, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    List<SqLiteStorage.Gun> list = new List<SqLiteStorage.Gun>();
                    SqLiteStorage.Status status = SqLiteStorage.Status.OK;
                    if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(
                        out list, 
                        item.Item1)))
                    {
                        if (status != SqLiteStorage.Status.NO_ROWS)
                        {
                            MessageBox.Show(
                                "Чтение из базы данных",
                                "Ошибка",
                                MessageBoxButtons.OK);
                        }
                    }

                    int i = 0x00;
                    foreach (SqLiteStorage.Gun gun in list)
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
            }            
        }
    }
}
