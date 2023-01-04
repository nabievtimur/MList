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
    public partial class TableFormUniverse : TableFormTemplate
    {
        public class CustomizeInputFormContainerUniverse :
            CustomizeInputFormContainer
        {
            iContainer container;
            public CustomizeInputFormContainerUniverse(iContainer container) :
                base(container.getId() == -1 ? "Добавить" : "Изменить")
            {
                this.container = container;
            }
            public override bool check(ref List<Tuple<Label, TextBox>> lItems)
            {
                return true;
            }
            public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
            {
                this.container.fillItemList(ref lItems);
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.container.getId() == -1)
                {
                    try
                    {
                        container.Add(new Address
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
        public TableFormUniverse(IContainer con)
        {
            InitializeComponent();

            //con.initTable(this.dataGridView1);

            this.Text = "Адреса";
            this.items = new Dictionary<int, iContainer>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            throw new NotImplementedException();
            //return new CustomizeInputForm(
            //    new CustomizeInputFormContainerUniverse(
            //        new Address
            //        {
            //            id = -1,
            //            address = ""
            //        }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            throw new NotImplementedException();
            //int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            //return new CustomizeInputForm(
            //            new CustomizeInputFormContainerAddress(this.items[rowIndex] as Address));
            //throw new InvalidOperationException("Ошибка обработки выбранной строки.");
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
                        Address.Get(this.textBox1.Text).Cast<iContainer>().ToList() : Address.Get().Cast<iContainer>().ToList());
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
}
