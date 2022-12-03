using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Storage;
using MList.Storage.Container;
using MList.Forms.CustomizeForms;

namespace MList.Forms.TableForm
{
    public partial class TableFormAddresses : TableFormTemplate
    {
        public class CustomizeInputFormContainerAddress : 
            CustomizeInputFormContainer
        {
            Address address ;
            public CustomizeInputFormContainerAddress(Address arrd) : 
                base(arrd.id == -1 ? "Добавить" : "Изменить")
            {
                this.address = arrd;
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
                textBox.Text = this.address.address;
                lItems.Add(new Tuple<Label, TextBox>(label, textBox));
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.address.id == -1)
                {
                    try
                    {
                        Address.Add(new Address
                        {
                            id = 0,
                            address = lItems[0].Item2.Text
                        });
                    }
                    catch (QueryExeption)
                    {
                        MessageBox.Show(
                            "Добавления в базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    try
                    {
                        Address.Update(new Address
                        {
                            id = this.address.id,
                            address = lItems[0].Item2.Text
                        });
                    }
                    catch (QueryExeption)
                    {
                        MessageBox.Show(
                            "Обновления базы данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }

                return DialogResult.OK;
            }
        }

        private List<Tuple<Address, int>> items;
        public TableFormAddresses()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("address", "Адрес");

            this.Text = "Адреса";
            this.items = new List<Tuple<Address, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerAddress(
                    new Address
                    {
                        id = -1,
                        address = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Address, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerAddress(
                            new Address
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
                foreach (Tuple<Address, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Address.Delete(item.Item1);
                        }
                        catch(QueryExeption)
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
            List<Address> list = new List<Address>();
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;

            try
            {
                list = this.textBox1.Text.Length > 0 ?
                    Address.Get(this.textBox1.Text) : Address.Get();
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }

            foreach (Address addr in list)
            {
                this.items.Add(new Tuple<Address, int>(addr, i));
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = addr.address;
                i++;
            }
        }
    }
}
