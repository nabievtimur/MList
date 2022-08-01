using System;
using System.Collections.Generic;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;

namespace MList.Forms
{
    public partial class TableFormTemplate : Form
    {
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

        public TableFormTemplate()
        {
            InitializeComponent();
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
        protected virtual void delete() { }
        protected virtual void updateGrid() { }
        private void TableFormTemplate_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
    }
}
