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
            this.components = new System.ComponentModel.Container();
            this.mainLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.createNoteButton = new System.Windows.Forms.Button();
            this.noteContentTable = new System.Windows.Forms.TableLayoutPanel();
            this.contentPanel = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.RenderPanel();
            this.searchBarLayout = new System.Windows.Forms.TableLayoutPanel();
            this.qSPanel = new System.Windows.Forms.Panel();
            this.tableQSCentrize = new System.Windows.Forms.TableLayoutPanel();
            this.qSTextBox = new System.Windows.Forms.TextBox();
            this.qSSwitchLabel = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.BasicSwitchLabel();
            this.toolsTable = new System.Windows.Forms.TableLayoutPanel();
            this.advFilterButton = new System.Windows.Forms.Button();
            this.sortStripButton = new MusicLoverHandbook.Controls_and_Forms.Custom_Controls.StripMenuButton();
            this.cancelFilteringButton = new System.Windows.Forms.Button();
            this.panelLabel = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.undoRedoTable = new System.Windows.Forms.TableLayoutPanel();
            this.redoButton = new System.Windows.Forms.Button();
            this.undoButton = new System.Windows.Forms.Button();
            this.mainLayoutTable.SuspendLayout();
            this.noteContentTable.SuspendLayout();
            this.searchBarLayout.SuspendLayout();
            this.qSPanel.SuspendLayout();
            this.tableQSCentrize.SuspendLayout();
            this.toolsTable.SuspendLayout();
            this.panelLabel.SuspendLayout();
            this.undoRedoTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutTable
            // 
            this.mainLayoutTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(234)))));
            this.mainLayoutTable.ColumnCount = 7;
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.mainLayoutTable.Controls.Add(this.createNoteButton, 2, 4);
            this.mainLayoutTable.Controls.Add(this.noteContentTable, 1, 2);
            this.mainLayoutTable.Controls.Add(this.panelLabel, 0, 0);
            this.mainLayoutTable.Controls.Add(this.saveButton, 2, 1);
            this.mainLayoutTable.Controls.Add(this.loadButton, 4, 1);
            this.mainLayoutTable.Controls.Add(this.undoRedoTable, 3, 1);
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
            this.mainLayoutTable.Size = new System.Drawing.Size(1374, 1132);
            this.mainLayoutTable.TabIndex = 0;
            // 
            // createNoteButton
            // 
            this.mainLayoutTable.SetColumnSpan(this.createNoteButton, 3);
            this.createNoteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createNoteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNoteButton.Location = new System.Drawing.Point(115, 1012);
            this.createNoteButton.Margin = new System.Windows.Forms.Padding(0);
            this.createNoteButton.Name = "createNoteButton";
            this.createNoteButton.Size = new System.Drawing.Size(1144, 100);
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
            this.noteContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.noteContentTable.Size = new System.Drawing.Size(1338, 856);
            this.noteContentTable.TabIndex = 0;
            // 
            // contentPanel
            // 
            this.contentPanel.AutoScroll = true;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 50);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1338, 806);
            this.contentPanel.TabIndex = 0;
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
            this.searchBarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.searchBarLayout.Size = new System.Drawing.Size(1338, 50);
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
            this.qSPanel.Size = new System.Drawing.Size(669, 50);
            this.qSPanel.TabIndex = 0;
            // 
            // tableQSCentrize
            // 
            this.tableQSCentrize.ColumnCount = 1;
            this.tableQSCentrize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableQSCentrize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableQSCentrize.Controls.Add(this.qSTextBox, 0, 1);
            this.tableQSCentrize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableQSCentrize.Location = new System.Drawing.Point(160, 0);
            this.tableQSCentrize.Margin = new System.Windows.Forms.Padding(0);
            this.tableQSCentrize.Name = "tableQSCentrize";
            this.tableQSCentrize.RowCount = 3;
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableQSCentrize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableQSCentrize.Size = new System.Drawing.Size(509, 50);
            this.tableQSCentrize.TabIndex = 2;
            // 
            // qSTextBox
            // 
            this.qSTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qSTextBox.Location = new System.Drawing.Point(3, 5);
            this.qSTextBox.Name = "qSTextBox";
            this.qSTextBox.Size = new System.Drawing.Size(503, 39);
            this.qSTextBox.TabIndex = 3;
            // 
            // qSSwitchLabel
            // 
            this.qSSwitchLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(234)))));
            this.qSSwitchLabel.BasicBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(234)))));
            this.qSSwitchLabel.BasicTooltipText = "";
            this.qSSwitchLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.qSSwitchLabel.Location = new System.Drawing.Point(0, 0);
            this.qSSwitchLabel.Name = "qSSwitchLabel";
            this.qSSwitchLabel.Size = new System.Drawing.Size(160, 50);
            this.qSSwitchLabel.SpecialBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(191)))), ((int)(((byte)(214)))));
            this.qSSwitchLabel.SpecialState = false;
            this.qSSwitchLabel.SpecialTooltipText = "";
            this.qSSwitchLabel.SwitchType = MusicLoverHandbook.Controls_and_Forms.Custom_Controls.BasicSwitchLabel.SwitchMode.DoubleClick;
            this.qSSwitchLabel.TabIndex = 0;
            this.qSSwitchLabel.Text = "Quick search:";
            this.qSSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolsTable
            // 
            this.toolsTable.ColumnCount = 3;
            this.toolsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.toolsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.77778F));
            this.toolsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.toolsTable.Controls.Add(this.advFilterButton, 1, 0);
            this.toolsTable.Controls.Add(this.sortStripButton, 0, 0);
            this.toolsTable.Controls.Add(this.cancelFilteringButton, 2, 0);
            this.toolsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolsTable.Location = new System.Drawing.Point(669, 0);
            this.toolsTable.Margin = new System.Windows.Forms.Padding(0);
            this.toolsTable.Name = "toolsTable";
            this.toolsTable.RowCount = 1;
            this.toolsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.toolsTable.Size = new System.Drawing.Size(669, 50);
            this.toolsTable.TabIndex = 1;
            // 
            // advFilterButton
            // 
            this.advFilterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.advFilterButton.Location = new System.Drawing.Point(140, 3);
            this.advFilterButton.Name = "advFilterButton";
            this.advFilterButton.Size = new System.Drawing.Size(475, 44);
            this.advFilterButton.TabIndex = 1;
            this.advFilterButton.Text = "Advanced Search";
            this.advFilterButton.UseVisualStyleBackColor = true;
            // 
            // sortStripButton
            // 
            this.sortStripButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sortStripButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sortStripButton.Location = new System.Drawing.Point(3, 3);
            this.sortStripButton.Name = "sortStripButton";
            this.sortStripButton.Size = new System.Drawing.Size(131, 44);
            this.sortStripButton.TabIndex = 2;
            this.sortStripButton.Text = "Sort";
            this.sortStripButton.UseVisualStyleBackColor = true;
            // 
            // cancelFilteringButton
            // 
            this.cancelFilteringButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelFilteringButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelFilteringButton.Location = new System.Drawing.Point(621, 3);
            this.cancelFilteringButton.Name = "cancelFilteringButton";
            this.cancelFilteringButton.Size = new System.Drawing.Size(45, 44);
            this.cancelFilteringButton.TabIndex = 3;
            this.cancelFilteringButton.Text = "Cancel";
            this.cancelFilteringButton.UseVisualStyleBackColor = true;
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
            this.panelLabel.Size = new System.Drawing.Size(1374, 50);
            this.panelLabel.TabIndex = 5;
            // 
            // title
            // 
            this.title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(1374, 50);
            this.title.TabIndex = 0;
            this.title.Text = "Music Lover Handbook";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveButton
            // 
            this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Location = new System.Drawing.Point(118, 53);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(174, 44);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // loadButton
            // 
            this.loadButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadButton.Location = new System.Drawing.Point(1082, 53);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(174, 44);
            this.loadButton.TabIndex = 7;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // undoRedoTable
            // 
            this.undoRedoTable.ColumnCount = 5;
            this.undoRedoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.undoRedoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.undoRedoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.undoRedoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.undoRedoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.undoRedoTable.Controls.Add(this.redoButton, 3, 0);
            this.undoRedoTable.Controls.Add(this.undoButton, 1, 0);
            this.undoRedoTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoRedoTable.Location = new System.Drawing.Point(295, 50);
            this.undoRedoTable.Margin = new System.Windows.Forms.Padding(0);
            this.undoRedoTable.Name = "undoRedoTable";
            this.undoRedoTable.RowCount = 1;
            this.undoRedoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.undoRedoTable.Size = new System.Drawing.Size(784, 50);
            this.undoRedoTable.TabIndex = 8;
            // 
            // redoButton
            // 
            this.redoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.redoButton.Location = new System.Drawing.Point(420, 3);
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(44, 44);
            this.redoButton.TabIndex = 1;
            this.redoButton.UseVisualStyleBackColor = true;
            // 
            // undoButton
            // 
            this.undoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.undoButton.Location = new System.Drawing.Point(320, 3);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(44, 44);
            this.undoButton.TabIndex = 0;
            this.undoButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1374, 1132);
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
            this.undoRedoTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainLayoutTable;
        private Button createNoteButton;
        private Panel panelLabel;
        public Label title;
        private TableLayoutPanel noteContentTable;
        private TableLayoutPanel searchBarLayout;
        private Panel qSPanel;
        private TableLayoutPanel tableQSCentrize;
        private TextBox qSTextBox;
        private TableLayoutPanel toolsTable;
        private Button advFilterButton;
        private Custom_Controls.RenderPanel contentPanel;
        private Custom_Controls.BasicSwitchLabel qSSwitchLabel;
        private Custom_Controls.StripMenuButton sortStripButton;
        private Button cancelFilteringButton;
        private Button saveButton;
        private Button loadButton;
        private TableLayoutPanel undoRedoTable;
        private Button undoButton;
        private Button redoButton;
    }
}