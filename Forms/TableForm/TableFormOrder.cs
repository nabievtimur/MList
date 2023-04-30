using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MList.Storage;
using MList.Storage.Table;
using MList.Storage.Table.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormOrder : Form
    {
        private ContainerCollection<ContainerGun> guns;
        private ContainerOrder order;

        public TableFormOrder() 
        {
            InitializeComponent();

            this.Text = "Добавить";
            this.guns = new ContainerCollection<ContainerGun>();
            this.order = new ContainerOrder();

            this.textBoxOrderNum.Text = TableOrder.GetNextOrderNum().ToString();
            new TableEmployee().gridInit(this.dataGridViewEmployee);
            new TableGun().gridInit(this.dataGridViewGuns);
            new TableGun().gridInit(this.dataGridViewPickedGuns);
        }

        public TableFormOrder(ContainerOrder order) :
            this()
        {
            this.Text = "Изменить";

            this.order = order;

            this.textBoxOrderNum.Text = order.getNumber().ToString();
            this.datePickerCreate.Value = new DateTime(order.getDate());

            try
            {
                this.guns = new TableGun().storageGetCurrent(order.getId());
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                this.DialogResult = DialogResult.Cancel;
            }

            this.UpdatePickedGunGrid();

            this.dataGridViewPickedGuns.ClearSelection();
            foreach (DataGridViewRow row in this.dataGridViewEmployee.Rows)
            {
                if (new ContainerEmployee(row).getId() == this.order.getId())
                {
                    row.Selected = true;
                }
            }
        }

        private void TableFormOrder_Load(object sender, EventArgs e) 
        {
            this.UpdateEmloyeeGrid();
            this.UpdateGunGrid();
        }

        private void UpdateEmloyeeGrid() 
        {
            try
            {
                new TableEmployee().gridFill(this.dataGridViewEmployee,
                    new TableEmployee().storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }

        private void UpdateGunGrid()
        {
            try
            {
                new TableGun().gridFill(this.dataGridViewGuns,
                    new TableGun().storageGet());
            }
            catch (QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }

        private void UpdatePickedGunGrid()
        {
            new TableGun().gridFill(this.dataGridViewPickedGuns, this.guns.downCast());
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewEmployee.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Нет ни одного сотрудника.",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                this.DialogResult = DialogResult.Cancel;
            }

            if (this.dataGridViewPickedGuns.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Не выбрано оружие",
                    "Добавления в базу данных",
                    MessageBoxButtons.OK);
                return;
            }

            try
            {
                if (this.order.getId() == -1)
                {
                    new TableOrder().storageAdd(
                        new ContainerOrder(
                            0,
                            int.Parse(this.textBoxOrderNum.Text),
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getId(),
                            this.datePickerCreate.Value.Ticks,
                            ""),
                        this.guns);
                }
                else
                {
                    new TableOrder().storageUpdate(
                        new ContainerOrder(
                            this.order.getId(),
                            int.Parse(this.textBoxOrderNum.Text),
                            new ContainerEmployee(this.dataGridViewEmployee.SelectedRows[0]).getId(),
                            this.datePickerCreate.Value.Ticks,
                            ""),
                        this.guns);
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
            this.UpdatePickedGunGrid();
        }

        private void buttonGunsDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewPickedGuns.SelectedRows)
            {
                this.guns.Remove(new ContainerGun(row));
            }
            this.UpdatePickedGunGrid();
        }
    }
}
