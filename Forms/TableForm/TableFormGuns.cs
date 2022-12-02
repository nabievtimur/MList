using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;
using MList.Storage;
using MList.Storage.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormGuns : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerGun :
            CustomizeInputFormContainer
        {
            Gun gun;
            public CustomizeInputFormContainerGun(Gun gun) :
                base(gun.id == -1 ? "Добавить" : "Изменить")
            {
                this.gun = gun;
            }
            public override bool check(ref List<Tuple<Label, TextBox>> lItems)
            {
                return true;
            }
            public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
            {
                {
                    Label label = new Label();
                    label.Text = "Брэнд";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.gun.brand;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Серия";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.gun.series;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Номер";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.gun.number.ToString();
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Патроны";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.gun.ammo;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.gun.id == -1)
                {
                    try
                    {
                        Gun.Add(new Gun {
                            id = 0,
                            brand = lItems[0].Item2.Text,
                            series = lItems[1].Item2.Text,
                            number = long.Parse(lItems[2].Item2.Text),
                            ammo = lItems[3].Item2.Text } );
                    }
                    catch(QueryExeption)
                    {
                        MessageBox.Show(
                            "Добавления в базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                    catch(FormatException)
                    {
                        MessageBox.Show(
                            "Неверный номер приказа.",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    try
                    {
                        Gun.Update(new Gun
                        {
                            id = this.gun.id,
                            brand = lItems[0].Item2.Text,
                            series = lItems[1].Item2.Text,
                            number = long.Parse(lItems[2].Item2.Text),
                            ammo = lItems[3].Item2.Text
                        });
                    }
                    catch (QueryExeption)
                    {
                        MessageBox.Show(
                            "Добавления в базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show(
                            "Неверный номер приказа.",
                            "Ошибка",
                            MessageBoxButtons.OK);
                    }
                }

                return DialogResult.OK;
            }
        }

        private List<Tuple<Gun, int>> items;
        public TableFormGuns()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("brand", "Брэнд");
            this.dataGridView1.Columns.Add("series", "Серия");
            this.dataGridView1.Columns.Add("number", "Номер");
            this.dataGridView1.Columns.Add("ammo", "Патроны");

            this.Text = "Оружие";
            this.items = new List<Tuple<Gun, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerGun(
                    new Gun {
                        id = -1,
                        brand = "",
                        series = "",
                        number = 0,
                        ammo = "" } ) );
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Gun, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerGun(
                            new Gun {
                                id = item.Item1.id,
                                brand = item.Item1.brand,
                                series = item.Item1.series,
                                number = item.Item1.number,
                                ammo = item.Item1.ammo } ) );
                }
            }
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                foreach (Tuple<Gun, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Gun.Delete(item.Item1);
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
            this.dataGridView1.Rows.Clear();
            this.items.Clear();
            int i = 0x00;
            try 
            {
                foreach (Gun gun in Gun.Get())
                {
                    this.items.Add(new Tuple<Gun, int>(gun, i));
                    if (i >= dataGridView1.Rows.Count)
                        this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[i].Cells[0].Value = gun.brand;
                    this.dataGridView1.Rows[i].Cells[1].Value = gun.series;
                    this.dataGridView1.Rows[i].Cells[2].Value = gun.number;
                    this.dataGridView1.Rows[i].Cells[3].Value = gun.ammo;
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
    }
}
