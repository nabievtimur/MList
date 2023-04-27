using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MList.Storage;
using MList.Storage.Table;
using MList.Storage.Table.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormMList : Form
    {
        ContainerMList containerMList;
        ContainerCollection<ContainerGun> guns;
        ContainerCollection<ContainerCar> cars;
        ContainerCollection<ContainerAddress> deepAdresses;
        ContainerCollection<ContainerAddress> arriveAdresses;

        public TableFormMList()
        {
            InitializeComponent();

            this.containerMList = new ContainerMList();
            this.guns = new ContainerCollection<ContainerGun>();
            this.cars = new ContainerCollection<ContainerCar>();
            this.deepAdresses = new ContainerCollection<ContainerAddress>();
            this.arriveAdresses = new ContainerCollection<ContainerAddress>();

            new TableEmployee().gridInit(this.dataGridView1);
            new TableGun().gridInit(this.dataGridView2);
            new TableGun().gridInit(this.dataGridView3);
            new TableCar().gridInit(this.dataGridView4);
            new TableCar().gridInit(this.dataGridView5);
            new TableAddress().gridInit(this.dataGridView6);
            new TableAddress().gridInit(this.dataGridView7);
            new TableAddress().gridInit(this.dataGridView8);
            new TableAddress().gridInit(this.dataGridView9);
        }
        public TableFormMList(ContainerMList containerMList):
            this()
        {
            this.containerMList = containerMList;

            // TODO UPDATE

        }
        private void TableFormMList_Load(object sender, EventArgs e)
        {
            updateConstGrids();
        }

        private void updateConstGrids()
        {
            try
            {
                new TableEmployee().gridFill(this.dataGridView1,
                    new TableEmployee().storageGet());
                new TableGun().gridFill(this.dataGridView2,
                    new TableGun().storageGet());
                new TableCar().gridFill(this.dataGridView4,
                    new TableCar().storageGet());
                new TableAddress().gridFill(this.dataGridView6,
                    new TableAddress().storageGet());
                new TableAddress().gridFill(this.dataGridView8,
                    new TableAddress().storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }
        private void updateSubGrids()
        {
            new TableGun().gridFill(this.dataGridView3, this.guns.downCast());
            new TableCar().gridFill(this.dataGridView5, this.cars.downCast());
            new TableAddress().gridFill(this.dataGridView7, this.deepAdresses.downCast());
            new TableAddress().gridFill(this.dataGridView9, this.arriveAdresses.downCast());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView2.SelectedRows)
            {
                this.guns.Add(new ContainerGun(row));
            }
            this.updateSubGrids();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView3.SelectedRows)
            {
                this.guns.Remove(new ContainerGun(row));
            }
            this.updateSubGrids();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView4.SelectedRows)
            {
                this.cars.Add(new ContainerCar(row));
            }
            this.updateSubGrids();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView5.SelectedRows)
            {
                this.cars.Remove(new ContainerCar(row));
            }
            this.updateSubGrids();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView6.SelectedRows)
            {
                this.deepAdresses.Add(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView7.SelectedRows)
            {
                this.deepAdresses.Remove(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView8.SelectedRows)
            {
                this.arriveAdresses.Add(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView9.SelectedRows)
            {
                this.arriveAdresses.Remove(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного сотрудника.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            if (this.guns.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного оружия.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            if (this.cars.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одной машины.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            if (this.deepAdresses.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного адреса убытия.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            if (this.arriveAdresses.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного адреса прибытия.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            try
            {
                if (this.containerMList.getId() == -1)
                {
                    new TableMList().storageAdd(
                        new ContainerMList(
                            -1,
                            this.dateTimePicker1.Value.Ticks,
                            this.dateTimePicker2.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker6.Value.Ticks,
                            this.dateTimePicker7.Value.Ticks,
                            this.dateTimePicker9.Value.Ticks,
                            this.textBox1.Text.ToString(),
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            new ContainerEmployee(this.dataGridView1.SelectedRows[0]).getId(),
                            this.dateTimePicker1.Value.Ticks,
                            ""),
                        new ContainerEmployee(this.dataGridView1.SelectedRows[0]),
                        this.guns,
                        this.cars,
                        this.deepAdresses,
                        this.arriveAdresses ) ;
                }
                else
                {
                    new TableMList().storageUpdate(
                        new ContainerMList(
                            this.containerMList.getId(),
                            this.dateTimePicker1.Value.Ticks,
                            this.dateTimePicker2.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker6.Value.Ticks,
                            this.dateTimePicker7.Value.Ticks,
                            this.dateTimePicker9.Value.Ticks,
                            this.textBox1.Text.ToString(),
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            this.dateTimePicker4.Value.Ticks,
                            new ContainerEmployee(this.dataGridView1.SelectedRows[0]).getId(),
                            this.dateTimePicker1.Value.Ticks,
                            ""),
                        new ContainerEmployee(this.dataGridView1.SelectedRows[0]),
                        this.guns,
                        this.cars,
                        this.deepAdresses,
                        this.arriveAdresses);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Неверный номер приказа.",
                    MessageBoxButtons.OK);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Не заполнен номер приказа.",
                    MessageBoxButtons.OK);
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
