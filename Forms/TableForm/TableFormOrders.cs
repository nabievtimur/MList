using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using MList.Storage;
using MList.Storage.Table;
using MList.Storage.Table.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormOrders : Form
    {
        TableOrder tableOrder;
        TableGun tableGun;
        public TableFormOrders()
        {
            InitializeComponent();

            this.tableOrder = new TableOrder();
            this.tableOrder.gridInit(this.dataGridView1);
            this.tableGun = new TableGun();
            this.tableGun.gridInit(this.dataGridView2);

            this.Text = "Приказы о закреплении оружия";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == new TableFormOrder().ShowDialog())
            {
                this.updateGrid();
            }
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

            if (DialogResult.OK == new TableFormOrder(new ContainerOrder(this.dataGridView1.SelectedRows[0])).ShowDialog())
            {
                this.updateGrid();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    this.tableOrder.storageDelete(row);
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Удаление элемента не удалось",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
                this.updateGrid();
            }
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
            TableOrder tableOrder = new TableOrder();
            try
            {
                tableOrder.gridFill(this.dataGridView1, this.textBox1.Text.Length > 0 ?
                    tableOrder.storageGet(this.textBox1.Text) : tableOrder.storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }

            this.updateSubGrid();
        }

        protected void updateSubGrid()
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    this.tableGun.gridFill(this.dataGridView2, this.tableGun.storageGetCurrent(
                        new ContainerOrder(this.dataGridView1.SelectedRows[0]).getId()).downCast());
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                            "Чтение из базы данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                }
            }
            else
            {
                this.dataGridView2.Rows.Clear();
            }
        }

        private void TableFormOrders_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
    }
}
