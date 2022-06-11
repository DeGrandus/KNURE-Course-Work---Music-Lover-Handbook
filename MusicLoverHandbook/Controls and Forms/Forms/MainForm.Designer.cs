namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.createNoteButton = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dragInto = new System.Windows.Forms.Panel();
            this.panelLabel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Controls.Add(this.createNoteButton, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelContent, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dragInto, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.panelLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1541, 980);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // createNoteButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.createNoteButton, 3);
            this.createNoteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createNoteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNoteButton.Location = new System.Drawing.Point(115, 640);
            this.createNoteButton.Margin = new System.Windows.Forms.Padding(0);
            this.createNoteButton.Name = "createNoteButton";
            this.createNoteButton.Size = new System.Drawing.Size(1311, 100);
            this.createNoteButton.TabIndex = 1;
            this.createNoteButton.Text = "CreateNewNote";
            this.createNoteButton.UseVisualStyleBackColor = true;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.panelContent, 5);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(18, 148);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1505, 394);
            this.panelContent.TabIndex = 3;
            // 
            // dragInto
            // 
            this.dragInto.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dragInto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dragInto.Location = new System.Drawing.Point(268, 762);
            this.dragInto.Name = "dragInto";
            this.dragInto.Size = new System.Drawing.Size(1005, 194);
            this.dragInto.TabIndex = 4;
            // 
            // panelLabel
            // 
            this.panelLabel.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel1.SetColumnSpan(this.panelLabel, 7);
            this.panelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLabel.Location = new System.Drawing.Point(0, 0);
            this.panelLabel.Margin = new System.Windows.Forms.Padding(0);
            this.panelLabel.Name = "panelLabel";
            this.panelLabel.Size = new System.Drawing.Size(1541, 50);
            this.panelLabel.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1541, 980);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "z";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button createNoteButton;
        private Panel panelContent;
        private Panel dragInto;
        private Panel panelLabel;
    }
}