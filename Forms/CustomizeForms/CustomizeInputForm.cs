using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;

namespace MList.Forms
{
    public partial class CustomizeInputForm : Form
    {
        private CustomizeInputFormContainer cContainer;
        private List<Tuple<Label, TextBox>> lItems;
        private const string STRING_NEEDED = "Необходимо";

        public CustomizeInputForm(CustomizeInputFormContainer container)
        {
            InitializeComponent();
            this.cContainer = container;
            this.lItems = new List<Tuple<Label, TextBox>>();
            container.fillItemList(ref this.lItems);
            this.Text = container.getOperationName();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cContainer.check(ref this.lItems))
            {
                this.DialogResult = this.cContainer.operation(this.lItems);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void CustomizeInputForm_Load(object sender, EventArgs e)
        {
            int i = 0x00;
            foreach (Tuple<Label, TextBox> item in this.lItems)
            {
                item.Item1.AutoSize = true;
                item.Item1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                item.Item1.Location = new System.Drawing.Point(23, 73 * i + 12);
                item.Item1.Name = "label" + i.ToString();
                item.Item1.Size = new System.Drawing.Size(30, 17);
                item.Item1.TabIndex = 0;
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
            this.button1.Enabled = false;
            this.button2.Location = new System.Drawing.Point(371, 73 * i + 12);

            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Size = new System.Drawing.Size(474, 72 * i + 47);
            this.ClientSize = new System.Drawing.Size(474, 72 * i + 47);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            bool edited = true;
            foreach (Tuple<Label, TextBox> item in this.lItems)
            {
                if (item.Item2.Text.Length == 0)
                    edited = false;
            }
            if (edited)
                this.button1.Enabled = true;
            else
                this.button2.Enabled = false;
        }
    }
}
