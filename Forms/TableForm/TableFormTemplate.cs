using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MList.src;

namespace MList.Forms
{
    public partial class TableFormTemplate : Form
    {
        protected List<Attr> attrs;
        public TableFormTemplate()
        {
            InitializeComponent();

            this.attrs = new List<Attr>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomizeInputForm form = new CustomizeInputForm(this.attrs, "Добавить");
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CustomizeInputForm form = new CustomizeInputForm(this.attrs, "Изменить");
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected DataGridView getGrid() { return this.dataGridView1; } 

        // virtual
        protected void add(List<string> result) { }
        protected void change(List<string> result) { }
        protected void delete() { }
        protected void updateGrid() { }
        private void TableFormTemplate_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
    }
}
