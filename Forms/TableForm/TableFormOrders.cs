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
using MList.Forms.CustomizeForms;
using MList.Storage.Table.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormOrders : Form
    {

        public TableFormOrders()
        {
            InitializeComponent();

            new TableOrder().gridInit(this.dataGridView1);
            new TableGun().gridInit(this.dataGridView2);

            this.Text = "Приказы о закреплении оружия";
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

            TableFormOrder form = new TableFormOrder(new ContainerOrder(this.dataGridView1.SelectedRows[0]));
            form.ShowDialog();
            this.updateGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    new TableOrder().storageDelete(new ContainerOrder(row));
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
                    TableGun tableGun = new TableGun();
                    tableGun.gridFill(this.dataGridView2, tableGun.storageGetCurrent(
                        new ContainerOrder(this.dataGridView1.SelectedRows[0]).getId()));
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
