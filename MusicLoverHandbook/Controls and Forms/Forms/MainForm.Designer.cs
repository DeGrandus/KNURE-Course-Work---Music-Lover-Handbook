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
            this.mainLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.createNoteButton = new System.Windows.Forms.Button();
            this.noteContentTable = new System.Windows.Forms.TableLayoutPanel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.searchBarLayout = new System.Windows.Forms.TableLayoutPanel();
            this.qSPanel = new System.Windows.Forms.Panel();
            this.tableQSCentrize = new System.Windows.Forms.TableLayoutPanel();
            this.qSTextBox = new System.Windows.Forms.TextBox();
            this.qSSwitchLabel = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.BasicSwitchLabel();
            this.toolsTable = new System.Windows.Forms.TableLayoutPanel();
            this.sortButtonStrip = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.StripMenuButton();
            this.advSearchButton = new System.Windows.Forms.Button();
            this.panelLabel = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.mainLayoutTable.SuspendLayout();
            this.noteContentTable.SuspendLayout();
            this.searchBarLayout.SuspendLayout();
            this.qSPanel.SuspendLayout();
            this.tableQSCentrize.SuspendLayout();
            this.toolsTable.SuspendLayout();
            this.panelLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutTable
            // 
            this.mainLayoutTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(234)))));
            this.mainLayoutTable.ColumnCount = 7;
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.mainLayoutTable.Controls.Add(this.createNoteButton, 2, 4);
            this.mainLayoutTable.Controls.Add(this.noteContentTable, 1, 2);
            this.mainLayoutTable.Controls.Add(this.panelLabel, 0, 0);
            this.mainLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutTable.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutTable.Name = "mainLayoutTable";
            this.mainLayoutTable.RowCount = 6;
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayoutTable.Size = new System.Drawing.Size(1631, 1123);
            this.mainLayoutTable.TabIndex = 0;
            // 
            // createNoteButton
            // 
            this.mainLayoutTable.SetColumnSpan(this.createNoteButton, 3);
            this.createNoteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createNoteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNoteButton.Location = new System.Drawing.Point(115, 1003);
            this.createNoteButton.Margin = new System.Windows.Forms.Padding(0);
            this.createNoteButton.Name = "createNoteButton";
            this.createNoteButton.Size = new System.Drawing.Size(1401, 100);
            this.createNoteButton.TabIndex = 1;
            this.createNoteButton.Text = "CreateNewNote";
            this.createNoteButton.UseVisualStyleBackColor = true;
            // 
            // noteContentTable
            // 
            this.noteContentTable.ColumnCount = 1;
            this.mainLayoutTable.SetColumnSpan(this.noteContentTable, 5);
            this.noteContentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.noteContentTable.Controls.Add(this.contentPanel, 0, 1);
            this.noteContentTable.Controls.Add(this.searchBarLayout, 0, 0);
            this.noteContentTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteContentTable.Location = new System.Drawing.Point(18, 103);
            this.noteContentTable.Name = "noteContentTable";
            this.noteContentTable.RowCount = 2;
            this.noteContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.noteContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.noteContentTable.Size = new System.Drawing.Size(1595, 847);
            this.noteContentTable.TabIndex = 0;
            // 
            // contentPanel
            // 
            this.noteContentTable.SetColumnSpan(this.contentPanel, 4);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 53);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1595, 794);
            this.contentPanel.TabIndex = 3;
            // 
            // searchBarLayout
            // 
            this.searchBarLayout.ColumnCount = 2;
            this.searchBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.searchBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.searchBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.searchBarLayout.Controls.Add(this.qSPanel, 0, 0);
            this.searchBarLayout.Controls.Add(this.toolsTable, 1, 0);
            this.searchBarLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBarLayout.Location = new System.Drawing.Point(0, 0);
            this.searchBarLayout.Margin = new System.Windows.Forms.Padding(0);
            this.searchBarLayout.Name = "searchBarLayout";
            this.searchBarLayout.RowCount = 1;
            this.searchBarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchBarLayout.Size = new System.Drawing.Size(1595, 50);
            this.searchBarLayout.TabIndex = 4;
            // 
            // qSPanel
            // 
            this.qSPanel.Controls.Add(this.tableQSCentrize);
            this.qSPanel.Controls.Add(this.qSSwitchLabel);
            this.qSPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qSPanel.Location = new System.Drawing.Point(0, 0);
            this.qSPanel.Margin = new System.Windows.Forms.Padding(0);
            this.qSPanel.Name = "qSPanel";
            this.qSPanel.Size = new System.Drawing.Size(797, 50);
            this.qSPanel.TabIndex = 0;
            // 
            // tableQSCentrize
            // 
            this.tableQSCentrize.ColumnCount = 1;
            this.tableQSCentrize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableQSCentrize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableQSCentrize.Controls.Add(this.qSTextBox, 0, 1);
            this.tableQSCentrize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableQSCentrize.Location = new System.Drawing.Point(171, 0);
            this.tableQSCentrize.Margin = new System.Windows.Forms.Padding(0);
            this.tableQSCentrize.Name = "tableQSCentrize";
            this.tableQSCentrize.RowCount = 3;
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableQSCentrize.Size = new System.Drawing.Size(626, 50);
            this.tableQSCentrize.TabIndex = 2;
            // 
            // qSTextBox
            // 
            this.qSTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qSTextBox.Location = new System.Drawing.Point(3, 5);
            this.qSTextBox.Name = "qSTextBox";
            this.qSTextBox.Size = new System.Drawing.Size(620, 39);
            this.qSTextBox.TabIndex = 3;
            // 
            // qSSwitchLabel
            // 
            this.qSSwitchLabel.BasicBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(234)))));
            this.qSSwitchLabel.BasicTooltipText = "";
            this.qSSwitchLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.qSSwitchLabel.Location = new System.Drawing.Point(0, 0);
            this.qSSwitchLabel.Name = "qSSwitchLabel";
            this.qSSwitchLabel.Size = new System.Drawing.Size(171, 50);
            this.qSSwitchLabel.SpecialBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(191)))), ((int)(((byte)(214)))));
            this.qSSwitchLabel.SpecialTooltipText = "";
            this.qSSwitchLabel.TabIndex = 1;
            this.qSSwitchLabel.Text = "Quick Search:";
            this.qSSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolsTable
            // 
            this.toolsTable.ColumnCount = 2;
            this.toolsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.16792F));
            this.toolsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.83208F));
            this.toolsTable.Controls.Add(this.sortButtonStrip, 0, 0);
            this.toolsTable.Controls.Add(this.advSearchButton, 1, 0);
            this.toolsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolsTable.Location = new System.Drawing.Point(797, 0);
            this.toolsTable.Margin = new System.Windows.Forms.Padding(0);
            this.toolsTable.Name = "toolsTable";
            this.toolsTable.RowCount = 1;
            this.toolsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.toolsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.toolsTable.Size = new System.Drawing.Size(798, 50);
            this.toolsTable.TabIndex = 1;
            // 
            // sortButtonStrip
            // 
            this.sortButtonStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sortButtonStrip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sortButtonStrip.Location = new System.Drawing.Point(0, 0);
            this.sortButtonStrip.Margin = new System.Windows.Forms.Padding(0);
            this.sortButtonStrip.Name = "sortButtonStrip";
            this.sortButtonStrip.Size = new System.Drawing.Size(136, 50);
            this.sortButtonStrip.TabIndex = 0;
            this.sortButtonStrip.Text = "Sorting";
            this.sortButtonStrip.UseVisualStyleBackColor = true;
            // 
            // advSearchButton
            // 
            this.advSearchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.advSearchButton.Location = new System.Drawing.Point(136, 0);
            this.advSearchButton.Margin = new System.Windows.Forms.Padding(0);
            this.advSearchButton.Name = "advSearchButton";
            this.advSearchButton.Size = new System.Drawing.Size(662, 50);
            this.advSearchButton.TabIndex = 1;
            this.advSearchButton.Text = "Advanced Search";
            this.advSearchButton.UseVisualStyleBackColor = true;
            // 
            // panelLabel
            // 
            this.panelLabel.BackColor = System.Drawing.Color.Beige;
            this.mainLayoutTable.SetColumnSpan(this.panelLabel, 7);
            this.panelLabel.Controls.Add(this.title);
            this.panelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLabel.Location = new System.Drawing.Point(0, 0);
            this.panelLabel.Margin = new System.Windows.Forms.Padding(0);
            this.panelLabel.Name = "panelLabel";
            this.panelLabel.Size = new System.Drawing.Size(1631, 50);
            this.panelLabel.TabIndex = 5;
            // 
            // title
            // 
            this.title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(1631, 50);
            this.title.TabIndex = 0;
            this.title.Text = "Music Lover Handbook";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1631, 1123);
            this.Controls.Add(this.mainLayoutTable);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "z";
            this.mainLayoutTable.ResumeLayout(false);
            this.noteContentTable.ResumeLayout(false);
            this.searchBarLayout.ResumeLayout(false);
            this.qSPanel.ResumeLayout(false);
            this.tableQSCentrize.ResumeLayout(false);
            this.tableQSCentrize.PerformLayout();
            this.toolsTable.ResumeLayout(false);
            this.panelLabel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainLayoutTable;
        private Button createNoteButton;
        private Panel panelLabel;
        public Label title;
        public Panel contentPanel;
        private TableLayoutPanel noteContentTable;
        private TableLayoutPanel searchBarLayout;
        private Panel qSPanel;
        private TableLayoutPanel tableQSCentrize;
        private TextBox qSTextBox;
        private Custom_Controls.BasicSwitchLabel qSSwitchLabel;
        private TableLayoutPanel toolsTable;
        private Custom_Controls.StripMenuButton sortButtonStrip;
        private Button advSearchButton;
    }
}