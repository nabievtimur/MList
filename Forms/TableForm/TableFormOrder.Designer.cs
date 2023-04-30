namespace MList.Forms.TableForm
{
    partial class TableFormOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonGunsAdd = new System.Windows.Forms.Button();
            this.buttonGunsDelete = new System.Windows.Forms.Button();
            this.dataGridViewPickedGuns = new System.Windows.Forms.DataGridView();
            this.dataGridViewGuns = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxOrderNum = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.datePickerCreate = new System.Windows.Forms.DateTimePicker();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dataGridViewEmployee = new System.Windows.Forms.DataGridView();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox10.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPickedGuns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuns)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEmployee)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.panel1);
            this.groupBox10.Controls.Add(this.dataGridViewGuns);
            this.groupBox10.Controls.Add(this.dataGridViewPickedGuns);
            this.groupBox10.Location = new System.Drawing.Point(12, 12);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(500, 450);
            this.groupBox10.TabIndex = 65;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Оружие";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonGunsAdd);
            this.panel1.Controls.Add(this.buttonGunsDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 211);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(180, 5, 25, 5);
            this.panel1.Size = new System.Drawing.Size(494, 41);
            this.panel1.TabIndex = 35;
            // 
            // buttonGunsAdd
            // 
            this.buttonGunsAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonGunsAdd.Location = new System.Drawing.Point(180, 5);
            this.buttonGunsAdd.Name = "buttonGunsAdd";
            this.buttonGunsAdd.Size = new System.Drawing.Size(140, 31);
            this.buttonGunsAdd.TabIndex = 31;
            this.buttonGunsAdd.Text = "Добавить";
            this.buttonGunsAdd.UseVisualStyleBackColor = true;
            this.buttonGunsAdd.Click += new System.EventHandler(this.buttonGunsAdd_Click);
            // 
            // buttonGunsDelete
            // 
            this.buttonGunsDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonGunsDelete.Location = new System.Drawing.Point(329, 5);
            this.buttonGunsDelete.Name = "buttonGunsDelete";
            this.buttonGunsDelete.Size = new System.Drawing.Size(140, 31);
            this.buttonGunsDelete.TabIndex = 32;
            this.buttonGunsDelete.Text = "Удалить";
            this.buttonGunsDelete.UseVisualStyleBackColor = true;
            this.buttonGunsDelete.Click += new System.EventHandler(this.buttonGunsDelete_Click);
            // 
            // dataGridViewPickedGuns
            // 
            this.dataGridViewPickedGuns.AllowUserToAddRows = false;
            this.dataGridViewPickedGuns.AllowUserToDeleteRows = false;
            this.dataGridViewPickedGuns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPickedGuns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPickedGuns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewPickedGuns.Location = new System.Drawing.Point(3, 252);
            this.dataGridViewPickedGuns.Name = "dataGridViewPickedGuns";
            this.dataGridViewPickedGuns.ReadOnly = true;
            this.dataGridViewPickedGuns.RowHeadersVisible = false;
            this.dataGridViewPickedGuns.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewPickedGuns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPickedGuns.Size = new System.Drawing.Size(494, 195);
            this.dataGridViewPickedGuns.TabIndex = 34;
            // 
            // dataGridViewGuns
            // 
            this.dataGridViewGuns.AllowUserToAddRows = false;
            this.dataGridViewGuns.AllowUserToDeleteRows = false;
            this.dataGridViewGuns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGuns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGuns.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewGuns.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewGuns.Name = "dataGridViewGuns";
            this.dataGridViewGuns.ReadOnly = true;
            this.dataGridViewGuns.RowHeadersVisible = false;
            this.dataGridViewGuns.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewGuns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGuns.Size = new System.Drawing.Size(494, 195);
            this.dataGridViewGuns.TabIndex = 33;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxOrderNum);
            this.groupBox5.Location = new System.Drawing.Point(518, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(211, 45);
            this.groupBox5.TabIndex = 66;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Номер Приказа";
            // 
            // textBoxOrderNum
            // 
            this.textBoxOrderNum.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxOrderNum.Location = new System.Drawing.Point(3, 16);
            this.textBoxOrderNum.Name = "textBoxOrderNum";
            this.textBoxOrderNum.Size = new System.Drawing.Size(205, 20);
            this.textBoxOrderNum.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.datePickerCreate);
            this.groupBox6.Location = new System.Drawing.Point(735, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(211, 45);
            this.groupBox6.TabIndex = 67;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Дата создания";
            // 
            // datePickerCreate
            // 
            this.datePickerCreate.Dock = System.Windows.Forms.DockStyle.Top;
            this.datePickerCreate.Location = new System.Drawing.Point(3, 16);
            this.datePickerCreate.Margin = new System.Windows.Forms.Padding(4);
            this.datePickerCreate.Name = "datePickerCreate";
            this.datePickerCreate.Size = new System.Drawing.Size(205, 20);
            this.datePickerCreate.TabIndex = 19;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dataGridViewEmployee);
            this.groupBox8.Location = new System.Drawing.Point(518, 63);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(428, 399);
            this.groupBox8.TabIndex = 68;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Сотрудник";
            // 
            // dataGridViewEmployee
            // 
            this.dataGridViewEmployee.AllowUserToAddRows = false;
            this.dataGridViewEmployee.AllowUserToDeleteRows = false;
            this.dataGridViewEmployee.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEmployee.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewEmployee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEmployee.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewEmployee.MultiSelect = false;
            this.dataGridViewEmployee.Name = "dataGridViewEmployee";
            this.dataGridViewEmployee.ReadOnly = true;
            this.dataGridViewEmployee.RowHeadersVisible = false;
            this.dataGridViewEmployee.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewEmployee.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEmployee.Size = new System.Drawing.Size(422, 380);
            this.dataGridViewEmployee.TabIndex = 30;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(640, 468);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(140, 28);
            this.buttonApply.TabIndex = 69;
            this.buttonApply.Text = "Ок";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(786, 468);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(140, 28);
            this.buttonCancel.TabIndex = 70;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TableFormOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 506);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox10);
            this.Name = "TableFormOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TableFormOrder";
            this.Load += new System.EventHandler(this.TableFormOrder_Load);
            this.groupBox10.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPickedGuns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuns)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEmployee)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonGunsAdd;
        private System.Windows.Forms.Button buttonGunsDelete;
        protected System.Windows.Forms.DataGridView dataGridViewGuns;
        protected System.Windows.Forms.DataGridView dataGridViewPickedGuns;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxOrderNum;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DateTimePicker datePickerCreate;
        private System.Windows.Forms.GroupBox groupBox8;
        protected System.Windows.Forms.DataGridView dataGridViewEmployee;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
    }
}