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

namespace MList.Forms
{
    public partial class TableFormTemplate : Form
    {
        iTable table;
        private class CustomizeInputFormContainerEmpty :
            CustomizeInputFormContainer
        {
            public CustomizeInputFormContainerEmpty() : base("empty") {}
            public override bool check(ref List<Tuple<Label, TextBox>> lItems)
            {
                throw new NotImplementedException();
            }

            public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
            {
                throw new NotImplementedException();
            }

            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                throw new NotImplementedException();
            }
        }

        protected TableFormTemplate()
        {
            InitializeComponent();
        }

        public TableFormTemplate(iTable table) : this()
        {
            this.table = table;

            this.table.gridInit(this.dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.getAddForm().ShowDialog();
            this.updateGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                        "Не выбрано ни одной строки",
                        "Ошибка",
                        MessageBoxButtons.OK);
                return;
            }
            this.getUpdateForm().ShowDialog();
            this.updateGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                        "Не выбрано ни одной строки",
                        "Ошибка",
                        MessageBoxButtons.OK);
                return;
            }
            this.delete();
            this.updateGrid();
        }

        // virtual
        protected virtual CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerEmpty());
        }

        protected virtual CustomizeInputForm getUpdateForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerEmpty());
        }
        protected void delete() 
        {

            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    this.table.storageDelete(row);
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
