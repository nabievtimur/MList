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
    public partial class TableFormCars : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerCar :
            CustomizeInputFormContainer
        {
            SqLiteStorage.Car car;
            public CustomizeInputFormContainerCar(SqLiteStorage.Car car) :
                base(car.id == -1 ? "Добавить" : "Изменить")
            {
                this.car = car;
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
                    textBox.Text = this.car.brand;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }

                {
                    Label label = new Label();
                    label.Text = "Номер";
                    TextBox textBox = new TextBox();
                    textBox.Text = this.car.number;
                    lItems.Add(new Tuple<Label, TextBox>(label, textBox));
                }
            }
            public override DialogResult operation(List<Tuple<Label, TextBox>> lItems)
            {
                if (this.car.id == -1)
                {
                    if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Add(
                        new SqLiteStorage.Car
                        {
                            id = -1,
                            brand = lItems[0].Item2.Text,
                            number = lItems[1].Item2.Text
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
                        new SqLiteStorage.Car
                        {
                            id = this.car.id,
                            brand = lItems[0].Item2.Text,
                            number = lItems[1].Item2.Text
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

        private List<Tuple<SqLiteStorage.Car, int>> items;
        public TableFormCars()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("brand", "Брэнд");
            this.dataGridView1.Columns.Add("number", "Номер");

            this.Text = "Машины";
            this.items = new List<Tuple<SqLiteStorage.Car, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerCar(
                    new SqLiteStorage.Car
                    {
                        id = -1,
                        brand = "",
                        number = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            //this.dataGridView1.
            return new CustomizeInputForm(
                new CustomizeInputFormContainerCar(
                    new SqLiteStorage.Car
                    {
                        id = -1,
                        brand = "",
                        number = ""
                    }));
        }
        protected override void delete()
        {

        }
        protected override void updateGrid()
        {
            System.Diagnostics.Debug.WriteLine("enter TableFormCar::updateGrid");
            List<SqLiteStorage.Car> list = new List<SqLiteStorage.Car>();
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
            foreach (SqLiteStorage.Car car in list)
            {
                this.items.Add(new Tuple<SqLiteStorage.Car, int>(car, i));
                System.Diagnostics.Debug.WriteLine(car.brand + " " + car.number);
                if (i >= dataGridView1.Rows.Count)
                    this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = car.brand;
                this.dataGridView1.Rows[i].Cells[1].Value = car.number;
                i++;
            }
        }
    }
}
