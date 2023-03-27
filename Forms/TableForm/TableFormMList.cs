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

            updateConstGrids();
        }
        public TableFormMList(ContainerMList containerMList):
            this()
        {


        }
        private void TableFormMList_Load(object sender, EventArgs e)
        {

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
    }
}
