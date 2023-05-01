using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MList.Forms
{
    public partial class CustomizeInputForm : Form
    {
        public delegate bool dCheck(ref List<TextBox> lItems);
        public delegate DialogResult dOperation(ref List<TextBox> lItems);

        private dCheck check;
        private dOperation operation;
        private List<TextBox> lItems;
        public CustomizeInputForm()
        {
            InitializeComponent();
            this.lItems = new List<TextBox>();
        }
        public CustomizeInputForm(
            List<String> lItems, 
            dCheck check, 
            dOperation operation) : 
            this()
        {
            this.Text = "Добавить";

            this.tableLayoutPanel2.RowCount = lItems.Count;
            foreach (var it in lItems)
            {
                TextBox textBox = new TextBox();
                textBox.Dock = DockStyle.Fill;
                textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
                this.lItems.Add(textBox);

                GroupBox box = new GroupBox();
                box.Dock = DockStyle.Fill;
                box.Height = 45;
                box.Text = it;
                box.Controls.Add(textBox);

                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                this.tableLayoutPanel2.Controls.Add(box);
            }

            this.check = check;
            this.operation = operation;
        }
        public CustomizeInputForm(
            List<String> lItems,
            List<String> lValues,
            dCheck check,
            dOperation operation) :
            this(lItems, check, operation)
        {
            this.Text = "Изменить";
            if (lItems.Count != lValues.Count)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < lItems.Count; i++)
            {
                this.lItems[i].Text = lValues[i];
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool correct = true;
            foreach (var it in this.lItems)
            {
                if (it.Text.Length == 0)
                {
                    it.BackColor = Color.Red;
                    correct = false;
                }
            }

            if (correct && this.check(ref this.lItems))
            {
                this.DialogResult = this.operation(ref this.lItems);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void CustomizeInputForm_Load(object sender, EventArgs e)
        {
            this.Height = 117 + this.lItems.Count * 45;
            foreach (RowStyle it in this.tableLayoutPanel2.RowStyles)
            {
                //it.SizeType = SizeType.Absolute;
                //it.Height = 45;
            }

            //int i = 0x00;
            //foreach (Tuple<Label, TextBox> item in this.lItems)
            //{
            //    item.Item1.AutoSize = true;
            //    item.Item1.Font = new System.Drawing.Font(
            //        "Microsoft Sans Serif", 
            //        10F, 
            //        System.Drawing.FontStyle.Regular, 
            //        System.Drawing.GraphicsUnit.Point, 
            //        ((byte)(204)));
            //    item.Item1.Location = new System.Drawing.Point(23, 73 * i + 12);
            //    item.Item1.Name = "label" + i.ToString();
            //    item.Item1.Size = new System.Drawing.Size(30, 17);
            //    item.Item1.TabIndex = 0;
            //    this.panel1.Controls.Add(item.Item1);
            //
            //    item.Item2.Location = new System.Drawing.Point(12, 73 * i + 41);
            //    item.Item2.Name = "textBox" + i.ToString();
            //    item.Item2.Size = new System.Drawing.Size(450, 20);
            //    item.Item2.TabIndex = 1;
            //    item.Item2.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            //    this.panel1.Controls.Add(item.Item2);
            //
            //    i++;
            //}
            //
            //this.button1.Location = new System.Drawing.Point(290, 73 * i + 12);
            //this.button1.Enabled = false;
            //this.button2.Location = new System.Drawing.Point(371, 73 * i + 12);
            //
            //this.panel1.Location = new System.Drawing.Point(0, 0);
            //this.panel1.Size = new System.Drawing.Size(474, 72 * i + 47);
            //this.ClientSize = new System.Drawing.Size(474, 72 * i + 47);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            foreach (var it in this.lItems)
            {
                if ((it == sender) && (it.Text.Length != 0))
                {
                    it.BackColor = SystemColors.Window;
                }
            }
        }
    }
}
