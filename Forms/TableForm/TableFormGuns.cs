using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;
using MList.Storage;

namespace MList.Forms.TableForm
{
    public partial class TableFormGuns : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerGun :
            CustomizeInputFormContainer
        {
            SqLiteStorage.Gun gun;
            public CustomizeInputFormContainerGun(SqLiteStorage.Gun gun) :
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
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Add(
                        new SqLiteStorage.Gun
                        {
                            id = 0,
                            brand = lItems[0].Item2.Text,
                            series = lItems[1].Item2.Text,
                            number = long.Parse(lItems[2].Item2.Text),
                            ammo = lItems[3].Item2.Text
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
                        new SqLiteStorage.Gun
                        {
                            id = this.gun.id,
                            brand = lItems[0].Item2.Text,
                            series = lItems[1].Item2.Text,
                            number = long.Parse(lItems[2].Item2.Text),
                            ammo = lItems[3].Item2.Text
                        }))
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

        private List<Tuple<SqLiteStorage.Gun, int>> items;
        public TableFormGuns()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("brand", "Брэнд");
            this.dataGridView1.Columns.Add("series", "Серия");
            this.dataGridView1.Columns.Add("number", "Номер");
            this.dataGridView1.Columns.Add("ammo", "Патроны");

            this.Text = "Оружие";
            this.items = new List<Tuple<SqLiteStorage.Gun, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerGun(
                    new SqLiteStorage.Gun
                    {
                        id = -1,
                        brand = "",
                        series = "",
                        number = 0,
                        ammo = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            //this.dataGridView1.
            return new CustomizeInputForm(
                new CustomizeInputFormContainerGun(
                    new SqLiteStorage.Gun
                    {
                        id = -1,
                        brand = "",
                        series = "",
                        number = 0,
                        ammo = ""
                    }));
        }
        protected override void delete()
        {

        }
        protected override void updateGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormAddress::updateGrid");
            List<SqLiteStorage.Gun> list = new List<SqLiteStorage.Gun>();
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

            this.items.Clear();
            int i = 0x00;
            foreach (SqLiteStorage.Gun gun in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.Gun, int>(gun, i));
                System.Diagnostics.Debug.WriteLine(
                    gun.brand + " " + 
                    gun.series + " " + 
                    gun.number + " " + 
                    gun.ammo);
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = gun.brand;
                this.dataGridView1.Rows[i].Cells[1].Value = gun.series;
                this.dataGridView1.Rows[i].Cells[2].Value = gun.number;
                this.dataGridView1.Rows[i].Cells[3].Value = gun.ammo;
                i++;
            }
        }
    }
}
