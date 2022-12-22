using System;
using System.Collections.Generic;
using System.Windows.Forms;

using MList.Forms.TableForm;
using MList.Storage;

namespace MList
{
    public partial class MainForm : Form
    {
        private Dictionary<int, Storage.Container.MList> items; 
        public MainForm()
        {
            InitializeComponent();

            this.items = new Dictionary<int, Storage.Container.MList>();

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
        public void updateGrid()
        {
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            try
            {
                foreach (Storage.Container.MList mlist in Storage.Container.MList.Get())
                {
                    this.items.Add(i, mlist);
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
            catch(QueryExeption)
            {
                MessageBox.Show(
                    "Чтение из базы данных",
                    "Ошибка",
                    MessageBoxButtons.OK);
            }
        }
        public void updateSubGrids()
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            Storage.Container.MList mlist = this.items[this.dataGridView1.SelectedRows[0].Index];

            {
                this.dataGridView2.Rows.Clear();
                int i = 0x00;
                try
                {
                    foreach (Storage.Container.Gun gun in Storage.Container.Gun.GetCurrent(mlist))
                    {
                        this.dataGridView2.Rows.Add();
                        this.dataGridView2.Rows[i].Cells[0].Value = gun.brand;
                        this.dataGridView2.Rows[i].Cells[1].Value = gun.series;
                        this.dataGridView2.Rows[i].Cells[2].Value = gun.number;
                        this.dataGridView2.Rows[i].Cells[3].Value = gun.ammo;
                        i++;
                    }
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            {
                this.dataGridView3.Rows.Clear();
                int i = 0x00;
                try
                {
                    foreach (Storage.Container.Car car in Storage.Container.Car.GetCurrent(mlist))
                    {
                        this.dataGridView3.Rows.Add();
                        this.dataGridView3.Rows[i].Cells[0].Value = car.brand;
                        this.dataGridView3.Rows[i].Cells[1].Value = car.number;
                        i++;
                    }
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            {
                this.dataGridView4.Rows.Clear();
                int i = 0x00;
                try
                {
                    foreach (Storage.Container.Address addr in Storage.Container.Address.GetCurrentArrive(mlist))
                    {
                        this.dataGridView4.Rows.Add();
                        this.dataGridView4.Rows[i].Cells[0].Value = addr.address;
                        i++;
                    }
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }

            {
                this.dataGridView5.Rows.Clear();
                int i = 0x00;
                try 
                {
                    foreach (Storage.Container.Address addr in Storage.Container.Address.GetCurrentDeep(mlist))
                    {
                        this.dataGridView5.Rows.Add();
                        this.dataGridView5.Rows[i].Cells[0].Value = addr.address;
                        i++;
                    }
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.updateGrid();
            this.updateSubGrids();
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
        private void button1_Click(object sender, EventArgs e)
        {
            TableFormMList form = new TableFormMList();
            form.ShowDialog();
            this.updateGrid();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            TableFormMList form = new TableFormMList();
            form.ShowDialog();
            this.updateGrid();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Storage.Container.MList.Delete(this.items[row.Index]);
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
