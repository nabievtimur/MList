using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MList.Forms.TableForm;

namespace MList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void авторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox("Аунтентификация", "Введите пароль");
            if (DialogResult.OK == inputBox.ShowDialog())
            {
                if (Authorization.Status.PASSWORD_CORRECT == Authorization.login(inputBox.getResult())) // TODO password request
                {
                    this.базаДанныхToolStripMenuItem.Enabled = true;
                }
                else
                {
                    MessageBox.Show(
                        "Неверный пароль",
                        "Ошибка аунтетификации",
                        MessageBoxButtons.OK);
                }
            }
        }

        private void задатьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authorization.passwordCreate();
        }

        private void авторизацияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox("Аунтентификация", "Введите пароль");
            if (DialogResult.OK == inputBox.ShowDialog())
            {
                if (Authorization.Status.PASSWORD_CORRECT == Authorization.login(inputBox.getResult())) // TODO password request
                {
                    this.базаДанныхToolStripMenuItem.Enabled = true;
                }
                else
                {
                    MessageBox.Show(
                        "Неверный пароль",
                        "Ошибка аунтетификации",
                        MessageBoxButtons.OK);
                }
            }
        }

        private void сменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authorization.passwordCreate();
        }

        private void открытьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormEmployee form = new TableFormEmployee();
            form.ShowDialog();
        }

        private void импортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormGuns form = new TableFormGuns();
            form.ShowDialog();
        }

        private void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormAddresses form = new TableFormAddresses();
            form.ShowDialog();
        }

        private void очиститьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormCars form = new TableFormCars();
            form.ShowDialog();
        }

        private void приказыОЗакрепленияОружияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormOrders form = new TableFormOrders();
            form.ShowDialog();
        }
    }
}
