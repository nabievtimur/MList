using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MList.Forms.CustomizeForms;
using MList.Storage.Container;
using MList.Storage;

namespace MList.Forms.TableForm
{
    public partial class TableFormCars : MList.Forms.TableFormTemplate
    {
        public class CustomizeInputFormContainerCar :
            CustomizeInputFormContainer
        {
            Car car;
            public CustomizeInputFormContainerCar(Car car) :
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
                    try
                    {
                        Car.Add(new Car {
                            id = -1,
                            brand = lItems[0].Item2.Text,
                            number = lItems[1].Item2.Text } );
                    }
                    catch(QueryExeption e)
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
                        Car.Update(new Car
                        {
                            id = this.car.id,
                            brand = lItems[0].Item2.Text,
                            number = lItems[1].Item2.Text
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

        private List<Tuple<Car, int>> items;
        public TableFormCars()
        {
            InitializeComponent();

            this.dataGridView1.Columns.Add("brand", "Брэнд");
            this.dataGridView1.Columns.Add("number", "Номер");

            this.Text = "Машины";
            this.items = new List<Tuple<Car, int>>();
        }
        protected override CustomizeInputForm getAddForm()
        {
            return new CustomizeInputForm(
                new CustomizeInputFormContainerCar(
                    new Car
                    {
                        id = -1,
                        brand = "",
                        number = ""
                    }));
        }

        protected override CustomizeInputForm getUpdateForm()
        {
            int rowIndex = this.dataGridView1.SelectedRows[0].Index;
            foreach (Tuple<Car, int> item in this.items)
            {
                if (item.Item2 == rowIndex)
                {
                    return new CustomizeInputForm(
                        new CustomizeInputFormContainerCar(
                            new Car
                            {
                                id = item.Item1.id,
                                brand = item.Item1.brand,
                                number = item.Item1.number
                            }));
                }
            }
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                foreach (Tuple<Car, int> item in this.items)
                {
                    if (item.Item2 == row.Index)
                    {
                        try
                        {
                            Car.Delete(item.Item1);
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
                foreach (Car car in Car.Get())
                {
                    this.items.Add(new Tuple<Car, int>(car, i));
                    if (i >= dataGridView1.Rows.Count)
                        this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[i].Cells[0].Value = car.brand;
                    this.dataGridView1.Rows[i].Cells[1].Value = car.number;
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
