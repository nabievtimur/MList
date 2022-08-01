using System;
using System.Collections.Generic;
using System.Windows.Forms;

using MList.Forms.TableForm;
using MList.Storage;

namespace MList
{
    public partial class MainForm : Form
    {
        private List<Tuple<SqLiteStorage.MList, int>> items;
        public MainForm()
        {
            InitializeComponent();

            this.items = new List<Tuple<SqLiteStorage.MList, int>>();

            this.dataGridView1.Columns.Add("number", "Номер");
            this.dataGridView1.Columns.Add("employee", "Сотрудник");
            this.dataGridView1.Columns.Add("createDate", "Дата создания");
            this.dataGridView1.Columns.Add("createStart", "Дата начала");
            this.dataGridView1.Columns.Add("startTime", "Время начала");
            this.dataGridView1.Columns.Add("endDate", "Дата окончания");
            this.dataGridView1.Columns.Add("endTime", "Время окончания");
            this.dataGridView1.Columns.Add("instractionTime", "Время инструктажа");
            this.dataGridView1.Columns.Add("returnGunDate", "Дата сдачи оружия");
            this.dataGridView1.Columns.Add("returnGunTime", "Время сдачи оружия");
            this.dataGridView1.Columns.Add("printDate", "Дата печати");
            this.dataGridView1.Columns.Add("description", "Примечание");

            this.dataGridView2.Columns.Add("brand", "Брэнд");
            this.dataGridView2.Columns.Add("series", "Серия");
            this.dataGridView2.Columns.Add("number", "Номер");
            this.dataGridView2.Columns.Add("ammo", "Патроны");

            this.dataGridView3.Columns.Add("brand", "Брэнд");
            this.dataGridView3.Columns.Add("number", "Номер");


            this.dataGridView4.Columns.Add("address", "Адрес убытия");

            this.dataGridView5.Columns.Add("address", "Адрес прибытия");
        }

        public void updateDataGrid1()
        {
            System.Diagnostics.Debug.WriteLine("enter MainForm::updateGrid");
            List<SqLiteStorage.MList> list = new List<SqLiteStorage.MList>();
            SqLiteStorage.Status status = SqLiteStorage.Status.OK;
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(out list)))
            {
                if (status != SqLiteStorage.Status.NO_ROWS)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.MList mlist in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.MList, int>(mlist, i));
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = mlist.numberMlist;
                this.dataGridView1.Rows[i].Cells[1].Value = mlist.employeeFullName;
                this.dataGridView1.Rows[i].Cells[2].Value = new DateTime(mlist.dateCreate).Date.ToString();
                this.dataGridView1.Rows[i].Cells[3].Value = new DateTime(mlist.dateBegin).Date.ToString();
                this.dataGridView1.Rows[i].Cells[4].Value = new DateTime(mlist.dateCreate).ToLocalTime().ToString();
                this.dataGridView1.Rows[i].Cells[5].Value = new DateTime(mlist.dateEnd).Date.ToString();
                this.dataGridView1.Rows[i].Cells[6].Value = new DateTime(mlist.dateEnd).ToLocalTime().ToString();
                this.dataGridView1.Rows[i].Cells[7].Value = new DateTime(mlist.dateCoach).ToLocalTime().ToString();
                this.dataGridView1.Rows[i].Cells[8].Value = new DateTime(mlist.datePassGun).Date.ToString();
                this.dataGridView1.Rows[i].Cells[9].Value = new DateTime(mlist.datePassGun).ToLocalTime().ToString();
                this.dataGridView1.Rows[i].Cells[10].Value = new DateTime(mlist.datePrint).Date.ToString();
                this.dataGridView1.Rows[i].Cells[11].Value = mlist.notes;
                i++;
            }
        }
        public void updateDataGrid2()
        {

        }
        public void updateDataGrid3()
        {

        }
        public void updateDataGrid4()
        {

        }
        public void updateDataGrid5()
        {

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
