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
    public partial class TableFormGuns : TableFormTemplate
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
        public TableFormGuns()
        {
            InitializeComponent();

            Gun.initTable(this.dataGridView1);

            this.Text = "Оружие";
            this.items = new Dictionary<int, iConteiner>();
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
            return new CustomizeInputForm(
                        new CustomizeInputFormContainerGun(this.items[rowIndex] as Gun));
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Gun.Delete(this.items[row.Index] as Gun);
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
                this.items = Gun.fillTable(
                    this.dataGridView1,
                    this.textBox1.Text.Length > 0 ?
                        Gun.Get(this.textBox1.Text).Cast<iConteiner>().ToList() : Gun.Get().Cast<iConteiner>().ToList());
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
