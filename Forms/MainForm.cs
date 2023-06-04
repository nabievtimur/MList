using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MList.Forms;
using MList.Forms.TableForm;
using MList.Storage;
using MList.Storage.Table;
using MList.Storage.Table.Container;

namespace MList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            new TableMList().gridInit(this.dataGridViewMLists);
            new TableGun().gridInit(this.dataGridViewGuns);
            new TableCar().gridInit(this.dataGridViewCars);
            new TableAddress().gridInit(this.dataGridViewDeepAddresses);
            new TableAddress().gridInit(this.dataGridViewArriveAddresses);
        }
        public void updateGrid()
        {
            TableMList tableMList = new TableMList();
            try
            {
                tableMList.gridFill(this.dataGridViewMLists, this.textBox1.Text.Length > 0 ?
                    tableMList.storageGet(this.textBox1.Text) : tableMList.storageGet());
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
            updateSubGrids();
        }
        public void updateSubGrids()
        {
            if (this.dataGridViewMLists.SelectedRows.Count == 0)
            {
                return;
            }

            try
            {
                ContainerMList containerMList = new ContainerMList(this.dataGridViewMLists.SelectedRows[0]);
                new TableGun().gridFill(this.dataGridViewGuns, new TableGun().storageGetCurrent(containerMList.getId()).downCast());
                new TableCar().gridFill(this.dataGridViewCars, new TableCar().storageGetCurrent(containerMList.getId()).downCast());
                new TableAddress().gridFill(this.dataGridViewArriveAddresses, new TableAddress().storageGetCurrentArrive(containerMList.getId()).downCast());
                new TableAddress().gridFill(this.dataGridViewDeepAddresses, new TableAddress().storageGetCurrentDeep(containerMList.getId()).downCast());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.updateGrid();
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
            TableFormTemplate form = new TableFormTemplate(new TableEmployee());
            form.ShowDialog();
        }
        private void импортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormTemplate form = new TableFormTemplate(new TableGun());
            form.ShowDialog();
        }
        private void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormTemplate form = new TableFormTemplate(new TableAddress());
            form.ShowDialog();
        }
        private void очиститьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormTemplate form = new TableFormTemplate(new TableCar());
            form.ShowDialog();
        }
        private void приказыОЗакрепленияОружияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableFormOrders form = new TableFormOrders();
            form.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TableFormMList form = new TableFormMList();
            form.ShowDialog();
            this.updateGrid();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewMLists.SelectedRows.Count > 0)
            {
                TableFormMList form = new TableFormMList(new ContainerMList(this.dataGridViewMLists.SelectedRows[0]));
                form.ShowDialog();
                this.updateGrid();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewMLists.SelectedRows)
            {
                try
                {
                    new TableMList().storageDelete(row);
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
    }
}
