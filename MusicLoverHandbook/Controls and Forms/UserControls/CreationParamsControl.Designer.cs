namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    partial class CreationParamsControl
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
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.boxName = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.SmartComboBox();
            this.renameSection = new System.Windows.Forms.Panel();
            this.centrizeRenameTable = new System.Windows.Forms.TableLayoutPanel();
            this.renameInput = new System.Windows.Forms.TextBox();
            this.renameCheck = new System.Windows.Forms.CheckBox();
            this.noteTypeLabel = new System.Windows.Forms.Label();
            this.tipLabel = new System.Windows.Forms.Label();
            this.descriptionPanel = new System.Windows.Forms.Panel();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.mainTable.SuspendLayout();
            this.renameSection.SuspendLayout();
            this.centrizeRenameTable.SuspendLayout();
            this.descriptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.Controls.Add(this.boxName, 0, 2);
            this.mainTable.Controls.Add(this.renameSection, 0, 3);
            this.mainTable.Controls.Add(this.noteTypeLabel, 0, 0);
            this.mainTable.Controls.Add(this.tipLabel, 0, 1);
            this.mainTable.Controls.Add(this.descriptionPanel, 1, 0);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 4;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.Size = new System.Drawing.Size(1164, 346);
            this.mainTable.TabIndex = 0;
            // 
            // boxName
            // 
            this.boxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.boxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxName.FormattingEnabled = true;
            this.boxName.InputType = MusicLoverHandbook.Models.Enums.NoteType.Author;
            this.boxName.Location = new System.Drawing.Point(3, 167);
            this.boxName.Name = "boxName";
            this.boxName.NoteParent = null;
            this.boxName.NotesContainer = null;
            this.boxName.RestrictedType = typeof(object);
            this.boxName.Size = new System.Drawing.Size(576, 33);
            this.boxName.Status = MusicLoverHandbook.Models.Enums.InputStatus.EMPTY_FIELD;
            this.boxName.TabIndex = 0;
            // 
            // renameSection
            // 
            this.renameSection.Controls.Add(this.centrizeRenameTable);
            this.renameSection.Controls.Add(this.renameCheck);
            this.renameSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameSection.Location = new System.Drawing.Point(3, 206);
            this.renameSection.Name = "renameSection";
            this.renameSection.Size = new System.Drawing.Size(576, 137);
            this.renameSection.TabIndex = 2;
            // 
            // centrizeRenameTable
            // 
            this.centrizeRenameTable.ColumnCount = 1;
            this.centrizeRenameTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.centrizeRenameTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.centrizeRenameTable.Controls.Add(this.renameInput, 0, 1);
            this.centrizeRenameTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centrizeRenameTable.Location = new System.Drawing.Point(0, 0);
            this.centrizeRenameTable.Name = "centrizeRenameTable";
            this.centrizeRenameTable.RowCount = 3;
            this.centrizeRenameTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.centrizeRenameTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.centrizeRenameTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.centrizeRenameTable.Size = new System.Drawing.Size(475, 137);
            this.centrizeRenameTable.TabIndex = 4;
            // 
            // renameInput
            // 
            this.renameInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameInput.Location = new System.Drawing.Point(3, 53);
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
            this.renameCheck.Size = new System.Drawing.Size(101, 137);
            this.renameCheck.TabIndex = 3;
            this.renameCheck.Text = "Rename";
            this.renameCheck.UseVisualStyleBackColor = true;
            // 
            // noteTypeLabel
            // 
            this.noteTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.noteTypeLabel.Name = "noteTypeLabel";
            this.noteTypeLabel.Size = new System.Drawing.Size(576, 143);
            this.noteTypeLabel.TabIndex = 3;
            this.noteTypeLabel.Text = "{notetype}";
            this.noteTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tipLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tipLabel.Location = new System.Drawing.Point(3, 143);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(576, 21);
            this.tipLabel.TabIndex = 4;
            this.tipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // descriptionPanel
            // 
            this.descriptionPanel.Controls.Add(this.descriptionBox);
            this.descriptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionPanel.Location = new System.Drawing.Point(585, 10);
            this.descriptionPanel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.descriptionPanel.Name = "descriptionPanel";
            this.mainTable.SetRowSpan(this.descriptionPanel, 4);
            this.descriptionPanel.Size = new System.Drawing.Size(576, 326);
            this.descriptionPanel.TabIndex = 5;
            // 
            // descriptionBox
            // 
            this.descriptionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionBox.Location = new System.Drawing.Point(0, 0);
            this.descriptionBox.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionBox.Multiline = true;
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Size = new System.Drawing.Size(576, 326);
            this.descriptionBox.TabIndex = 2;
            // 
            // CreationParamsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTable);
            this.Name = "CreationParamsControl";
            this.Size = new System.Drawing.Size(1164, 346);
            this.mainTable.ResumeLayout(false);
            this.mainTable.PerformLayout();
            this.renameSection.ResumeLayout(false);
            this.renameSection.PerformLayout();
            this.centrizeRenameTable.ResumeLayout(false);
            this.centrizeRenameTable.PerformLayout();
            this.descriptionPanel.ResumeLayout(false);
            this.descriptionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainTable;
        private Custom_Controls.SmartComboBox boxName;
        private Panel renameSection;
        private TableLayoutPanel centrizeRenameTable;
        private TextBox renameInput;
        private CheckBox renameCheck;
        private Label noteTypeLabel;
        private Label tipLabel;
        private Panel descriptionPanel;
        private TextBox descriptionBox;
    }
}
