using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using MList.src;

namespace MList.Forms
{
    public partial class CustomizeInputForm : Form
    {
        private List<Tuple<Label, TextBox, Attr>> lItems;
        private List<String> lResult;
        private const string STRING_NEEDED = "Необходимо";

        public CustomizeInputForm(List<Attr> attributes, string title)
        {
            InitializeComponent();
            this.lItems = new List<Tuple<Label, TextBox, Attr>>();
            foreach (Attr a in attributes)
            {
                Label label = new Label();
                TextBox textBox = new TextBox();
                this.lItems.Add( new Tuple<Label,TextBox,Attr>(label, textBox, a));
            }
            this.lResult = new List<String>();
            this.Text = title;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool f = true;
            foreach (Tuple<Label, TextBox, Attr> item in this.lItems)
            {
                if (item.Item2.Text == STRING_NEEDED)
                {
                    f = false;
                }

                if (!item.Item3.check(item.Item2.Text))
                {
                    item.Item2.ForeColor = Color.Red;
                    f = false;
                    if (item.Item2.Text.Length == 0)
                    {
                        item.Item2.Text = STRING_NEEDED;
                    }
                }
            }
            if (f)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CustomizeInputForm_Load(object sender, EventArgs e)
        {
            int i = 0x00;
            foreach (Tuple<Label, TextBox, Attr> item in this.lItems)
            {
                item.Item1.AutoSize = true;
                item.Item1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                item.Item1.Location = new System.Drawing.Point(23, 73 * i + 12);
                item.Item1.Name = "label" + i.ToString();
                item.Item1.Size = new System.Drawing.Size(30, 17);
                item.Item1.TabIndex = 0;
                item.Item1.Text = item.Item3.getName();
                this.panel1.Controls.Add(item.Item1);

                item.Item2.Location = new System.Drawing.Point(12, 73 * i + 41);
                item.Item2.Name = "textBox" + i.ToString();
                item.Item2.Size = new System.Drawing.Size(450, 20);
                item.Item2.TabIndex = 1;
                item.Item2.TextChanged += new System.EventHandler(this.textBox_TextChanged);
                this.panel1.Controls.Add(item.Item2);

                i++;
            }

            this.button1.Location = new System.Drawing.Point(290, 73 * i + 12);
            this.button2.Location = new System.Drawing.Point(371, 73 * i + 12);

            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Size = new System.Drawing.Size(474, 72 * i + 47);
            this.ClientSize = new System.Drawing.Size(474, 72 * i + 47);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            foreach (Tuple<Label, TextBox, Attr> item in this.lItems)
            {
                if (item.Item2.Text != STRING_NEEDED)
                {
                    item.Item2.ForeColor = Color.Black;
                }
            }
        }
    }
}
