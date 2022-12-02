using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MList
{
    public partial class InputBox : Form
    {
        private string result;
        public string getResult() { return this.result; }
        private InputBox()
        {
            InitializeComponent();
            this.result = "";
        }

        public InputBox(string title, string text)
        {
            InitializeComponent();
            this.Text = title;
            this.label1.Text = text;
            this.result = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length != 0)
            {
                this.result = this.textBox1.Text;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
