using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Storage;
using MList.Forms.CustomizeForms;

namespace MList.Forms.TableForm
{
    public partial class TableFormAddresses : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerAddress : 
            CustomizeInputFormContainer
        {
            SqLiteStorage.Address addr;
            public CustomizeInputFormContainerAddress(SqLiteStorage.Address arrd) : 
                base(arrd.id == -1 ? "Добавить" : "Изменить")
            {
                this.addr = arrd;
            }
            public override bool check(ref List<Tuple<Label, TextBox>> lItems)
            {
                return true;
            }
            public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
            {
                Label label = new Label();
                label.Text = "Адрес";
                TextBox textBox = new TextBox();
                textBox.Text = this.addr.address;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.addr.id == -1)
                {
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Add(
                        new SqLiteStorage.Address
                        {
                            id = 0,
                            address = lItems[0].Item2.Text
                        }))
                    {
                        MessageBox.Show(
                            "Ошибка",
                            "Добавления в базу данных",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Update(
                        new SqLiteStorage.Address
                        {
                            id = this.addr.id,
                            address = lItems[0].Item2.Text
                        } ))
                    {
                        MessageBox.Show(
                            "Ошибка",
                            "Обновления базы данных",
                            MessageBoxButtons.OK);
                    }
                }

                return DialogResult.OK;
            }
        }

        private List<Tuple<SqLiteStorage.Address, int>> items;
        public TableFormAddresses()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("address", "Адрес");

            this.Text = "Адреса";
            this.items = new List<Tuple<SqLiteStorage.Address, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerAddress(
                    new SqLiteStorage.Address
                    {
                        id = -1,
                        address = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<SqLiteStorage.Address, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerAddress(
                            new SqLiteStorage.Address
                            {
                                id = item.Item1.id,
                                address = item.Item1.address
                            }));
                }
            }
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete() 
        { 
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                foreach (Tuple<SqLiteStorage.Address, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Delete(item.Item1))
                        {
                            MessageBox.Show(
                                "Удаление элемента не удалось",
                                "Ошибка",
                                MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }
        protected override void updateGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormAddress::updateGrid");
            List<SqLiteStorage.Address> list = new List<SqLiteStorage.Address>();
            SqLiteStorage.Status status = SqLiteStorage.Status.OK;
            if (SqLiteStorage.Status.OK != (status = SqLiteStorage.getInstance().Get(out list)))
            {
                if (status != SqLiteStorage.Status.NO_ROWS)
                {
                    MessageBox.Show(
                    "Ошибка",
                    "Чтение из базы данных",
                    MessageBoxButtons.OK);
                }
            }

            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.Address addr in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.Address, int>(addr, i));
                System.Diagnostics.Debug.WriteLine(addr.address);
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = addr.address;
                i++;
            }
        }
    }
}
