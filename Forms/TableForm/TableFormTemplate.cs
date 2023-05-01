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

namespace MList.Forms
{
    public partial class TableFormTemplate : Form
    {
        iTable table;
        protected TableFormTemplate()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyDownEvent);
        }
        public TableFormTemplate(iTable table) : this()
        {
            this.table = table;
            this.Text = table.getVisibleTableName();

            this.table.gridInit(this.dataGridView1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            add();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            update();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            update();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            delete();
        }
        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                delete();
            }
        }
        private void add()
        {
            new CustomizeInputForm(
                this.table.getAssociatedContainer().getFieldsNames(),
                this.table.getAssociatedContainer().checkItemList,
                (ref List<TextBox> lItems) =>
                {
                    try
                    {
                        this.table.storageAdd(this.table.getAssociatedContainer().updateFromList(lItems));
                    }
                    catch (ParceException)
                    {
                        MessageBox.Show(
                        "Неверные входные данные.",
                        "Ошибка",
                        MessageBoxButtons.OK);
                        return DialogResult.OK;
                    }
                    catch (QueryExeption)
                    {
                        MessageBox.Show(
                        "Не удалось добавить элемент в базу данных.",
                        "Ошибка",
                        MessageBoxButtons.OK);
                        return DialogResult.OK;
                    }
                    return DialogResult.Cancel;
                }).ShowDialog();
            this.updateGrid();
        }
        private void update()
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                        "Не выбрано ни одной строки",
                        "Ошибка",
                        MessageBoxButtons.OK);
                return;
            }

            iContainer container = this.table.getAssociatedContainer(this.dataGridView1.SelectedRows[0]);

            new CustomizeInputForm(
                container.getFieldsNames(),
                container.getFieldsValues(),
                this.table.getAssociatedContainer().checkItemList,
                (ref List<TextBox> lItems) =>
                {
                    try
                    {
                        this.table.storageUpdate(container.updateFromList(lItems));
                    }
                    catch (ParceException)
                    {
                        MessageBox.Show(
                        "Неверные входные данные.",
                        "Ошибка",
                        MessageBoxButtons.OK);
                        return DialogResult.OK;
                    }
                    catch (QueryExeption)
                    {
                        MessageBox.Show(
                        "Не удалось обновить элемент в базу данных.",
                        "Ошибка",
                        MessageBoxButtons.OK);
                        return DialogResult.OK;
                    }
                    return DialogResult.Cancel;
                } ).ShowDialog();

            this.updateGrid();
        }
        private void delete()
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                        "Не выбрано ни одной строки",
                        "Ошибка",
                        MessageBoxButtons.OK);
                return;
            }

            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    this.table.storageDelete(this.table.getAssociatedContainer(row));
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
        protected void updateGrid() 
        {
            try
            {
                this.table.gridFill(this.dataGridView1, this.textBox1.Text.Length > 0 ?
                    this.table.storageGet(this.textBox1.Text) : this.table.storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }
        private void TableFormTemplate_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.updateGrid();
        }
    }
}
