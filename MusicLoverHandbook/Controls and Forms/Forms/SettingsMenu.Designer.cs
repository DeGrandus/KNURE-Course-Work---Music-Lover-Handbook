namespace MusicLoverHandbook.View.Forms
{
    partial class SettingsMenu
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
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.currentMusicFilesFolderField = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.currentDataPathField = new System.Windows.Forms.TextBox();
            this.resetMusicFilesFolderPathButton = new System.Windows.Forms.Button();
            this.setNewMusicFilesFolderPathButton = new System.Windows.Forms.Button();
            this.resetDataFilePathButton = new System.Windows.Forms.Button();
            this.mainTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 7;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.746521F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.25348F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.25348F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.746521F));
            this.mainTable.Controls.Add(this.currentMusicFilesFolderField, 0, 5);
            this.mainTable.Controls.Add(this.titleLabel, 0, 0);
            this.mainTable.Controls.Add(this.currentDataPathField, 0, 2);
            this.mainTable.Controls.Add(this.resetMusicFilesFolderPathButton, 2, 6);
            this.mainTable.Controls.Add(this.setNewMusicFilesFolderPathButton, 4, 6);
            this.mainTable.Controls.Add(this.resetDataFilePathButton, 3, 3);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 9;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.61905F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.61905F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.mainTable.Size = new System.Drawing.Size(879, 606);
            this.mainTable.TabIndex = 0;
            // 
            // currentMusicFilesFolderField
            // 
            this.currentMusicFilesFolderField.AutoCompleteCustomSource.AddRange(new string[] {
            "what the fuck",
            "yo how"});
            this.currentMusicFilesFolderField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.currentMusicFilesFolderField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.mainTable.SetColumnSpan(this.currentMusicFilesFolderField, 7);
            this.currentMusicFilesFolderField.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentMusicFilesFolderField.Location = new System.Drawing.Point(30, 412);
            this.currentMusicFilesFolderField.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.currentMusicFilesFolderField.Name = "currentMusicFilesFolderField";
            this.currentMusicFilesFolderField.ReadOnly = true;
            this.currentMusicFilesFolderField.Size = new System.Drawing.Size(819, 31);
            this.currentMusicFilesFolderField.TabIndex = 2;
            // 
            // titleLabel
            // 
            this.mainTable.SetColumnSpan(this.titleLabel, 7);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(879, 50);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Settings";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentDataPathField
            // 
            this.currentDataPathField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.currentDataPathField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.mainTable.SetColumnSpan(this.currentDataPathField, 7);
            this.currentDataPathField.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentDataPathField.Location = new System.Drawing.Point(30, 233);
            this.currentDataPathField.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.currentDataPathField.Name = "currentDataPathField";
            this.currentDataPathField.ReadOnly = true;
            this.currentDataPathField.Size = new System.Drawing.Size(819, 31);
            this.currentDataPathField.TabIndex = 1;
            // 
            // button1resr
            // 
            this.resetMusicFilesFolderPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resetMusicFilesFolderPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetMusicFilesFolderPathButton.Location = new System.Drawing.Point(142, 449);
            this.resetMusicFilesFolderPathButton.Name = "button1resr";
            this.resetMusicFilesFolderPathButton.Size = new System.Drawing.Size(194, 44);
            this.resetMusicFilesFolderPathButton.TabIndex = 3;
            this.resetMusicFilesFolderPathButton.Text = "Reset music folder path";
            this.resetMusicFilesFolderPathButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.setNewMusicFilesFolderPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setNewMusicFilesFolderPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setNewMusicFilesFolderPathButton.Location = new System.Drawing.Point(542, 449);
            this.setNewMusicFilesFolderPathButton.Name = "button2";
            this.setNewMusicFilesFolderPathButton.Size = new System.Drawing.Size(194, 44);
            this.setNewMusicFilesFolderPathButton.TabIndex = 4;
            this.setNewMusicFilesFolderPathButton.Text = "Set new music folder path";
            this.setNewMusicFilesFolderPathButton.UseVisualStyleBackColor = true;
            // 
            // resetDataFilePathButton
            // 
            this.resetDataFilePathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resetDataFilePathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetDataFilePathButton.Location = new System.Drawing.Point(342, 270);
            this.resetDataFilePathButton.Name = "resetDataFilePathButton";
            this.resetDataFilePathButton.Size = new System.Drawing.Size(194, 44);
            this.resetDataFilePathButton.TabIndex = 5;
            this.resetDataFilePathButton.Text = "Reset save file path";
            this.resetDataFilePathButton.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 606);
            this.Controls.Add(this.mainTable);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.mainTable.ResumeLayout(false);
            this.mainTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainTable;
        private Label titleLabel;
        private TextBox currentDataPathField;
        private TextBox currentMusicFilesFolderField;
        private Button resetMusicFilesFolderPathButton;
        private Button setNewMusicFilesFolderPathButton;
        private Button resetDataFilePathButton;
    }
}