using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Forms.CustomizeForms
{
    public abstract partial class CustomizeInputFormContainer
    {
        private string sOperationName;
        public CustomizeInputFormContainer()
        {
            this.sOperationName = "Name";
        }
        public CustomizeInputFormContainer(string sOperationName)
        {
            this.sOperationName = sOperationName;
        }
        public string getOperationName() { return sOperationName; }

        // Создаём список полей
        public abstract void fillItemList(ref List<Tuple<Label, TextBox>> lItems);
        // Проверяем заполенение полей, отмечаем неверные заполнения
        public abstract bool check(ref List<Tuple<Label, TextBox>> lItems);
        // Производим операцию, попадаем сюда только после check
        public abstract DialogResult operation(List<Tuple<Label, TextBox>> lItems);
    }
}
