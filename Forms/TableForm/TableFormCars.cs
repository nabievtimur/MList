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
    public partial class TableFormCars : TableFormTemplate
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
                    catch(QueryExeption)
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

        public TableFormCars()
        {
            InitializeComponent();

            Car.initTable(this.dataGridView1);

            this.Text = "Машины";
            this.items = new Dictionary<int, iConteiner>();
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
            return new CustomizeInputForm(
                        new CustomizeInputFormContainerCar(this.items[rowIndex] as Car));
            throw new InvalidOperationException("Ошибка обработки выбранной строки.");
        }
        protected override void delete()
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                try
                {
                    Car.Delete(this.items[row.Index] as Car);
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
                this.items = Car.fillTable(
                    this.dataGridView1,
                    this.textBox1.Text.Length > 0 ?
                        Car.Get(this.textBox1.Text).Cast<iConteiner>().ToList() : Car.Get().Cast<iConteiner>().ToList());
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
