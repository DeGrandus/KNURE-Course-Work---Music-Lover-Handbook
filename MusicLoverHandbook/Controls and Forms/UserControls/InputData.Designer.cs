namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    partial class InputData
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.boxName = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.SmartComboBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.renameSection = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.renameInput = new System.Windows.Forms.TextBox();
            this.renameCheck = new System.Windows.Forms.CheckBox();
            this.noteTypeLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.renameSection.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.boxName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.descriptionBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.renameSection, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.noteTypeLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1164, 346);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // boxName
            // 
            this.boxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.boxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxName.FormattingEnabled = true;
            this.boxName.Location = new System.Drawing.Point(3, 156);
            this.boxName.Name = "boxName";
            this.boxName.Size = new System.Drawing.Size(576, 33);
            this.boxName.Status = MusicLoverHandbook.Models.Enums.InputStatus.EMPTY_FIELD;
            this.boxName.TabIndex = 0;
            // 
            // descriptionBox
            // 
            this.descriptionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionBox.Location = new System.Drawing.Point(585, 10);
            this.descriptionBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.descriptionBox.Multiline = true;
            this.descriptionBox.Name = "descriptionBox";
            this.tableLayoutPanel1.SetRowSpan(this.descriptionBox, 3);
            this.descriptionBox.Size = new System.Drawing.Size(576, 326);
            this.descriptionBox.TabIndex = 1;
            // 
            // renameSection
            // 
            this.renameSection.Controls.Add(this.tableLayoutPanel2);
            this.renameSection.Controls.Add(this.renameCheck);
            this.renameSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameSection.Location = new System.Drawing.Point(3, 195);
            this.renameSection.Name = "renameSection";
            this.renameSection.Size = new System.Drawing.Size(576, 148);
            this.renameSection.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.renameInput, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(475, 148);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // renameInput
            // 
            this.renameInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameInput.Location = new System.Drawing.Point(3, 58);
            this.renameInput.Name = "renameInput";
            this.renameInput.Size = new System.Drawing.Size(469, 31);
            this.renameInput.TabIndex = 0;
            // 
            // renameCheck
            // 
            this.renameCheck.AutoSize = true;
            this.renameCheck.Dock = System.Windows.Forms.DockStyle.Right;
            this.renameCheck.Location = new System.Drawing.Point(475, 0);
            this.renameCheck.Name = "renameCheck";
            this.renameCheck.Size = new System.Drawing.Size(101, 148);
            this.renameCheck.TabIndex = 3;
            this.renameCheck.Text = "Rename";
            this.renameCheck.UseVisualStyleBackColor = true;
            // 
            // noteTypeLabel
            // 
            this.noteTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.noteTypeLabel.Name = "noteTypeLabel";
            this.noteTypeLabel.Size = new System.Drawing.Size(576, 153);
            this.noteTypeLabel.TabIndex = 3;
            this.noteTypeLabel.Text = "{notetype}";
            this.noteTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "InputData";
            this.Size = new System.Drawing.Size(1164, 346);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.renameSection.ResumeLayout(false);
            this.renameSection.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Custom_Controls.SmartComboBox boxName;
        private TextBox descriptionBox;
        private Panel renameSection;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox renameInput;
        private CheckBox renameCheck;
        private Label noteTypeLabel;
    }
}
