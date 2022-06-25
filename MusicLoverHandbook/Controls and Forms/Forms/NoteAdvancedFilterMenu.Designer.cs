namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    partial class NoteAdvancedFilterMenu
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
            this.mainContentTable = new System.Windows.Forms.TableLayoutPanel();
            this.filteringTable = new System.Windows.Forms.TableLayoutPanel();
            this.byDescInput = new System.Windows.Forms.TextBox();
            this.byNameInput = new System.Windows.Forms.TextBox();
            this.searchNameLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.searchDescLabel = new System.Windows.Forms.Label();
            this.noteTypeSelectFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.titleContPanel = new System.Windows.Forms.Panel();
            this.previewFilteredPanel = new System.Windows.Forms.Panel();
            this.applyButtonsTable = new System.Windows.Forms.TableLayoutPanel();
            this.applyFilterButton = new System.Windows.Forms.Button();
            this.applyFilterRoughButton = new System.Windows.Forms.Button();
            this.advFiltersFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.mainContentTable.SuspendLayout();
            this.filteringTable.SuspendLayout();
            this.applyButtonsTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainContentTable
            // 
            this.mainContentTable.ColumnCount = 1;
            this.mainContentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContentTable.Controls.Add(this.filteringTable, 0, 0);
            this.mainContentTable.Controls.Add(this.applyButtonsTable, 0, 2);
            this.mainContentTable.Controls.Add(this.advFiltersFlow, 0, 1);
            this.mainContentTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContentTable.Location = new System.Drawing.Point(0, 0);
            this.mainContentTable.Margin = new System.Windows.Forms.Padding(4);
            this.mainContentTable.Name = "mainContentTable";
            this.mainContentTable.RowCount = 3;
            this.mainContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainContentTable.Size = new System.Drawing.Size(1336, 787);
            this.mainContentTable.TabIndex = 0;
            // 
            // filteringTable
            // 
            this.filteringTable.ColumnCount = 3;
            this.filteringTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filteringTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.filteringTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.filteringTable.Controls.Add(this.byDescInput, 1, 3);
            this.filteringTable.Controls.Add(this.byNameInput, 1, 2);
            this.filteringTable.Controls.Add(this.searchNameLabel, 0, 2);
            this.filteringTable.Controls.Add(this.titleLabel, 0, 0);
            this.filteringTable.Controls.Add(this.searchDescLabel, 0, 3);
            this.filteringTable.Controls.Add(this.noteTypeSelectFlow, 0, 4);
            this.filteringTable.Controls.Add(this.titleContPanel, 2, 0);
            this.filteringTable.Controls.Add(this.previewFilteredPanel, 2, 1);
            this.filteringTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filteringTable.Location = new System.Drawing.Point(0, 0);
            this.filteringTable.Margin = new System.Windows.Forms.Padding(0);
            this.filteringTable.Name = "filteringTable";
            this.filteringTable.RowCount = 4;
            this.filteringTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.filteringTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.filteringTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.filteringTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.filteringTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.filteringTable.Size = new System.Drawing.Size(1336, 368);
            this.filteringTable.TabIndex = 1;
            // 
            // byDescInput
            // 
            this.byDescInput.Location = new System.Drawing.Point(173, 135);
            this.byDescInput.Margin = new System.Windows.Forms.Padding(0);
            this.byDescInput.Name = "byDescInput";
            this.byDescInput.Size = new System.Drawing.Size(581, 39);
            this.byDescInput.TabIndex = 2;
            // 
            // byNameInput
            // 
            this.byNameInput.Location = new System.Drawing.Point(173, 96);
            this.byNameInput.Margin = new System.Windows.Forms.Padding(0);
            this.byNameInput.Name = "byNameInput";
            this.byNameInput.Size = new System.Drawing.Size(581, 39);
            this.byNameInput.TabIndex = 0;
            // 
            // searchNameLabel
            // 
            this.searchNameLabel.AutoSize = true;
            this.searchNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchNameLabel.Location = new System.Drawing.Point(0, 96);
            this.searchNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.searchNameLabel.Name = "searchNameLabel";
            this.searchNameLabel.Size = new System.Drawing.Size(173, 39);
            this.searchNameLabel.TabIndex = 1;
            this.searchNameLabel.Text = "By Name:";
            this.searchNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.filteringTable.SetColumnSpan(this.titleLabel, 2);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(928, 32);
            this.titleLabel.TabIndex = 5;
            this.titleLabel.Text = "Advanced Filter";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchDescLabel
            // 
            this.searchDescLabel.AutoSize = true;
            this.searchDescLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchDescLabel.Location = new System.Drawing.Point(0, 135);
            this.searchDescLabel.Margin = new System.Windows.Forms.Padding(0);
            this.searchDescLabel.Name = "searchDescLabel";
            this.searchDescLabel.Size = new System.Drawing.Size(173, 39);
            this.searchDescLabel.TabIndex = 3;
            this.searchDescLabel.Text = "By Description:";
            // 
            // noteTypeSelectFlow
            // 
            this.noteTypeSelectFlow.AutoScroll = true;
            this.filteringTable.SetColumnSpan(this.noteTypeSelectFlow, 2);
            this.noteTypeSelectFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteTypeSelectFlow.Location = new System.Drawing.Point(3, 177);
            this.noteTypeSelectFlow.Name = "noteTypeSelectFlow";
            this.noteTypeSelectFlow.Size = new System.Drawing.Size(922, 188);
            this.noteTypeSelectFlow.TabIndex = 7;
            // 
            // titleContPanel
            // 
            this.titleContPanel.AutoSize = true;
            this.titleContPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleContPanel.Location = new System.Drawing.Point(928, 0);
            this.titleContPanel.Margin = new System.Windows.Forms.Padding(0);
            this.titleContPanel.Name = "titleContPanel";
            this.titleContPanel.Size = new System.Drawing.Size(408, 32);
            this.titleContPanel.TabIndex = 8;
            // 
            // previewFilteredPanel
            // 
            this.previewFilteredPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewFilteredPanel.Location = new System.Drawing.Point(931, 35);
            this.previewFilteredPanel.Name = "previewFilteredPanel";
            this.filteringTable.SetRowSpan(this.previewFilteredPanel, 4);
            this.previewFilteredPanel.Size = new System.Drawing.Size(402, 330);
            this.previewFilteredPanel.TabIndex = 9;
            // 
            // applyButtonsTable
            // 
            this.applyButtonsTable.ColumnCount = 2;
            this.applyButtonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.applyButtonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.applyButtonsTable.Controls.Add(this.applyFilterButton, 0, 0);
            this.applyButtonsTable.Controls.Add(this.applyFilterRoughButton, 1, 0);
            this.applyButtonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyButtonsTable.Location = new System.Drawing.Point(3, 739);
            this.applyButtonsTable.Name = "applyButtonsTable";
            this.applyButtonsTable.RowCount = 1;
            this.applyButtonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.applyButtonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.applyButtonsTable.Size = new System.Drawing.Size(1330, 45);
            this.applyButtonsTable.TabIndex = 2;
            // 
            // applyFilterButton
            // 
            this.applyFilterButton.BackColor = System.Drawing.Color.PaleGreen;
            this.applyFilterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.applyFilterButton.Location = new System.Drawing.Point(0, 0);
            this.applyFilterButton.Margin = new System.Windows.Forms.Padding(0);
            this.applyFilterButton.Name = "applyFilterButton";
            this.applyFilterButton.Size = new System.Drawing.Size(665, 45);
            this.applyFilterButton.TabIndex = 0;
            this.applyFilterButton.Text = "Apply filtering";
            this.applyFilterButton.UseVisualStyleBackColor = false;
            // 
            // applyFilterRoughButton
            // 
            this.applyFilterRoughButton.BackColor = System.Drawing.Color.LemonChiffon;
            this.applyFilterRoughButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyFilterRoughButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.applyFilterRoughButton.Location = new System.Drawing.Point(665, 0);
            this.applyFilterRoughButton.Margin = new System.Windows.Forms.Padding(0);
            this.applyFilterRoughButton.Name = "applyFilterRoughButton";
            this.applyFilterRoughButton.Size = new System.Drawing.Size(665, 45);
            this.applyFilterRoughButton.TabIndex = 1;
            this.applyFilterRoughButton.Text = "Apply rough filtering";
            this.applyFilterRoughButton.UseVisualStyleBackColor = false;
            // 
            // advFiltersFlow
            // 
            this.advFiltersFlow.AutoScroll = true;
            this.advFiltersFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advFiltersFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.advFiltersFlow.Location = new System.Drawing.Point(3, 371);
            this.advFiltersFlow.Name = "advFiltersFlow";
            this.advFiltersFlow.Size = new System.Drawing.Size(1330, 362);
            this.advFiltersFlow.TabIndex = 3;
            // 
            // NoteAdvancedFilterMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1336, 787);
            this.Controls.Add(this.mainContentTable);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NoteAdvancedFilterMenu";
            this.Text = "NoteAdvancedFilterMenu";
            this.mainContentTable.ResumeLayout(false);
            this.filteringTable.ResumeLayout(false);
            this.filteringTable.PerformLayout();
            this.applyButtonsTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainContentTable;
        private TableLayoutPanel filteringTable;
        private TextBox byNameInput;
        private Label searchNameLabel;
        private TextBox byDescInput;
        private Label searchDescLabel;
        private FlowLayoutPanel noteTypeSelectFlow;
        private Panel titleContPanel;
        private Panel previewFilteredPanel;
        private TableLayoutPanel applyButtonsTable;
        private Button applyFilterButton;
        private Button applyFilterRoughButton;
        public Label titleLabel;
        private FlowLayoutPanel advFiltersFlow;
    }
}