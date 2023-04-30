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

            new TableEmployee().gridInit(this.dataGridViewEmployee);
            new TableGun().gridInit(this.dataGridViewGuns);
            new TableGun().gridInit(this.dataGridViewPickedGuns);
            new TableCar().gridInit(this.dataGridViewCars);
            new TableCar().gridInit(this.dataGridViewPickedCars);
            new TableAddress().gridInit(this.dataGridViewDeepAddresses);
            new TableAddress().gridInit(this.dataGridViewPickedDeepAddresses);
            new TableAddress().gridInit(this.dataGridViewArriveAddresses);
            new TableAddress().gridInit(this.dataGridViewPickedArriveAddresses);

            // TODO номер
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
                new TableEmployee().gridFill(this.dataGridViewEmployee,
                    new TableEmployee().storageGet());
                new TableGun().gridFill(this.dataGridViewGuns,
                    new TableGun().storageGet());
                new TableCar().gridFill(this.dataGridViewCars,
                    new TableCar().storageGet());
                new TableAddress().gridFill(this.dataGridViewDeepAddresses,
                    new TableAddress().storageGet());
                new TableAddress().gridFill(this.dataGridViewArriveAddresses,
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
            new TableGun().gridFill(this.dataGridViewPickedGuns, this.guns.downCast());
            new TableCar().gridFill(this.dataGridViewPickedCars, this.cars.downCast());
            new TableAddress().gridFill(this.dataGridViewPickedDeepAddresses, this.deepAdresses.downCast());
            new TableAddress().gridFill(this.dataGridViewPickedArriveAddresses, this.arriveAdresses.downCast());
        }
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewEmployee.SelectedRows.Count == 0)
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
                            Int32.Parse(this.textBoxMlistNum.Text), // try
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getId(),
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getFullName(),
                            this.datePickerCreate.Value.Ticks,
                            this.datePickerBegin.Value.Ticks + this.timePickerBegin.Value.Ticks,
                            this.datePickerEnd.Value.Ticks + this.timePickerEnd.Value.Ticks,
                            this.timePickerCoach.Value.Ticks,
                            this.datePickerPassGun.Value.Ticks + this.timePickerPassGun.Value.Ticks,
                            this.datePickerPrint.Value.Ticks,
                            this.textBoxDescription.Text),
                        new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]),
                        this.guns,
                        this.cars,
                        this.deepAdresses,
                        this.arriveAdresses );
                }
                else
                {
                    new TableMList().storageUpdate(
                        new ContainerMList(
                            this.containerMList.getId(),
                            Int32.Parse(this.textBoxMlistNum.Text), // try
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getId(),
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getFullName(),
                            this.datePickerCreate.Value.Ticks,
                            this.datePickerBegin.Value.Ticks + this.timePickerBegin.Value.Ticks,
                            this.datePickerEnd.Value.Ticks + this.timePickerEnd.Value.Ticks,
                            this.timePickerCoach.Value.Ticks,
                            this.datePickerPassGun.Value.Ticks + this.timePickerPassGun.Value.Ticks,
                            this.datePickerPrint.Value.Ticks,
                            this.textBoxDescription.Text),
                        new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]),
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
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void buttonGunsAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewGuns.SelectedRows)
            {
                this.guns.Add(new ContainerGun(row));
            }
            this.updateSubGrids();
        }

        private void buttonGunsDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewPickedGuns.SelectedRows)
            {
                this.guns.Remove(new ContainerGun(row));
            }
            this.updateSubGrids();
        }

        private void buttonCarsAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewCars.SelectedRows)
            {
                this.cars.Add(new ContainerCar(row));
            }
            this.updateSubGrids();
        }

        private void buttonCarsDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewPickedCars.SelectedRows)
            {
                this.cars.Remove(new ContainerCar(row));
            }
            this.updateSubGrids();
        }

        private void buttonDeepAddressesAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewDeepAddresses.SelectedRows)
            {
                this.deepAdresses.Add(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void buttonDeepAddressesDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewPickedDeepAddresses.SelectedRows)
            {
                this.deepAdresses.Remove(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void buttonArriveAddressesAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewArriveAddresses.SelectedRows)
            {
                this.arriveAdresses.Add(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }

        private void buttonArriveAddressesDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewPickedArriveAddresses.SelectedRows)
            {
                this.arriveAdresses.Remove(new ContainerAddress(row));
            }
            this.updateSubGrids();
        }
    }
}
