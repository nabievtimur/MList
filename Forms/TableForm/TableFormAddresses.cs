using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

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
        public TableFormAddresses()
        {
            InitializeComponent();

            Address.initTable(this.dataGridView1);

            this.Text = "Адреса";
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
            return new CustomizeInputForm(
                        new CustomizeInputFormContainerAddress(this.items[rowIndex] as Address));
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete() 
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Address.Delete(this.items[row.Index] as Address);
                }
                catch (QueryExeption)
                {
                    MessageBox.Show(
                        "Удаление элемента не удалось",
                        "Ошибка",
                        MessageBoxButtons.OK);
                }
            }
        }
        protected override void updateGrid()
        {
            try
            {
                this.items = Address.fillTable(
                    this.dataGridView1,
                    this.textBox1.Text.Length > 0 ?
                        Address.Get(this.textBox1.Text).Cast<iConteiner>().ToList() : Address.Get().Cast<iConteiner>().ToList());
            }
            catch(QueryExeption)
            {
                MessageBox.Show(
                        "Чтение из базы данных",
                        "Ошибка",
                        MessageBoxButtons.OK);
            }
        }
    }
}
