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
    public partial class TableFormAddresses : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerAddress : 
            CustomizeInputFormContainer
        {
            Storage.Container.Address address ;
            public CustomizeInputFormContainerAddress(Storage.Container.Address arrd) : 
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
                        Storage.Container.Address.Add(new Storage.Container.Address
                        {
                            id = 0,
                            address = lItems[0].Item2.Text
                        });
                    }
                    catch (QueryExeption e)
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
                        Storage.Container.Address.Update(new Storage.Container.Address
                        {
                            id = 0,
                            address = lItems[0].Item2.Text
                        });
                    }
                    catch (QueryExeption e)
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

        private List<Tuple<Storage.Container.Address, int>> items;
        public TableFormAddresses()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("address", "Адрес");

            this.Text = "Адреса";
            this.items = new List<Tuple<Storage.Container.Address, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerAddress(
                    new Storage.Container.Address
                    {
                        id = -1,
                        address = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Storage.Container.Address, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerAddress(
                            new Storage.Container.Address
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
                foreach (Tuple<Storage.Container.Address, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Storage.Container.Address.Delete(item.Item1);
                        }
                        catch(QueryExeption e)
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
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            try
            {
                foreach (Storage.Container.Address addr in Storage.Container.Address.Get())
                {
                    this.items.Add(new Tuple<Storage.Container.Address, int>(addr, i));
                    this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[i++].Cells[0].Value = addr.address;
                    i++;
                }
            }
            catch(QueryExeption e)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }

            
            
        }
    }
}
