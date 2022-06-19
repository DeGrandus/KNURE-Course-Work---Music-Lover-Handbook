namespace MusicLoverHandbook.View.Forms
{
    partial class AddNoteMenu
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
            this.title = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.tableInputs = new System.Windows.Forms.TableLayoutPanel();
            this.inputSongFile = new MusicLoverHandbook.Controls_and_Forms.UserControls.InputData();
            this.inputDisc = new MusicLoverHandbook.Controls_and_Forms.UserControls.InputData();
            this.inputAuthor = new MusicLoverHandbook.Controls_and_Forms.UserControls.InputData();
            this.dragDropPanel = new System.Windows.Forms.Panel();
            this.dragDropText = new System.Windows.Forms.Label();
            this.inputSong = new MusicLoverHandbook.Controls_and_Forms.UserControls.InputData();
            this.tableSwap = new System.Windows.Forms.TableLayoutPanel();
            this.discInAuthorLabel = new System.Windows.Forms.Label();
            this.authorInDiscLabel = new System.Windows.Forms.Label();
            this.title.SuspendLayout();
            this.tableInputs.SuspendLayout();
            this.dragDropPanel.SuspendLayout();
            this.tableSwap.SuspendLayout();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Controls.Add(this.titleLabel);
            this.title.Dock = System.Windows.Forms.DockStyle.Top;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(969, 88);
            this.title.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(969, 88);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Create new note";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // createButton
            // 
            this.createButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.createButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.createButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createButton.Location = new System.Drawing.Point(0, 1144);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(969, 88);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "Create/Update Note";
            this.createButton.UseVisualStyleBackColor = false;
            // 
            // tableInputs
            // 
            this.tableInputs.BackColor = System.Drawing.SystemColors.Control;
            this.tableInputs.ColumnCount = 1;
            this.tableInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableInputs.Controls.Add(this.inputSongFile, 0, 3);
            this.tableInputs.Controls.Add(this.inputDisc, 0, 1);
            this.tableInputs.Controls.Add(this.inputAuthor, 0, 0);
            this.tableInputs.Controls.Add(this.dragDropPanel, 0, 4);
            this.tableInputs.Controls.Add(this.inputSong, 0, 2);
            this.tableInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableInputs.Location = new System.Drawing.Point(0, 136);
            this.tableInputs.Name = "tableInputs";
            this.tableInputs.RowCount = 6;
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableInputs.Size = new System.Drawing.Size(969, 1008);
            this.tableInputs.TabIndex = 2;
            // 
            // inputSongFile
            // 
            this.inputSongFile.AutoFill = true;
            this.inputSongFile.BackColor = System.Drawing.SystemColors.Control;
            this.inputSongFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputSongFile.InputType = MusicLoverHandbook.Models.Enums.InputType.SongFile;
            this.inputSongFile.Location = new System.Drawing.Point(5, 596);
            this.inputSongFile.Margin = new System.Windows.Forms.Padding(5);
            this.inputSongFile.Name = "inputSongFile";
            this.inputSongFile.Size = new System.Drawing.Size(959, 187);
            this.inputSongFile.TabIndex = 3;
            // 
            // inputDisc
            // 
            this.inputDisc.AutoFill = true;
            this.inputDisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputDisc.InputType = MusicLoverHandbook.Models.Enums.InputType.Disc;
            this.inputDisc.Location = new System.Drawing.Point(5, 202);
            this.inputDisc.Margin = new System.Windows.Forms.Padding(5);
            this.inputDisc.Name = "inputDisc";
            this.inputDisc.Size = new System.Drawing.Size(959, 187);
            this.inputDisc.TabIndex = 1;
            // 
            // inputAuthor
            // 
            this.inputAuthor.AutoFill = true;
            this.inputAuthor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputAuthor.InputType = MusicLoverHandbook.Models.Enums.InputType.Author;
            this.inputAuthor.Location = new System.Drawing.Point(5, 5);
            this.inputAuthor.Margin = new System.Windows.Forms.Padding(5);
            this.inputAuthor.Name = "inputAuthor";
            this.inputAuthor.Size = new System.Drawing.Size(959, 187);
            this.inputAuthor.TabIndex = 0;
            // 
            // dragDropPanel
            // 
            this.dragDropPanel.Controls.Add(this.dragDropText);
            this.dragDropPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dragDropPanel.Location = new System.Drawing.Point(0, 788);
            this.dragDropPanel.Margin = new System.Windows.Forms.Padding(0);
            this.dragDropPanel.Name = "dragDropPanel";
            this.dragDropPanel.Padding = new System.Windows.Forms.Padding(14);
            this.dragDropPanel.Size = new System.Drawing.Size(969, 197);
            this.dragDropPanel.TabIndex = 4;
            // 
            // dragDropText
            // 
            this.dragDropText.BackColor = System.Drawing.SystemColors.Control;
            this.dragDropText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dragDropText.Location = new System.Drawing.Point(14, 14);
            this.dragDropText.Margin = new System.Windows.Forms.Padding(3);
            this.dragDropText.Name = "dragDropText";
            this.dragDropText.Size = new System.Drawing.Size(941, 169);
            this.dragDropText.TabIndex = 0;
            this.dragDropText.Text = "Drop .mp3 file";
            this.dragDropText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputSong
            // 
            this.inputSong.AutoFill = true;
            this.inputSong.BackColor = System.Drawing.SystemColors.Control;
            this.inputSong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputSong.InputType = MusicLoverHandbook.Models.Enums.InputType.SongName;
            this.inputSong.Location = new System.Drawing.Point(5, 399);
            this.inputSong.Margin = new System.Windows.Forms.Padding(5);
            this.inputSong.Name = "inputSong";
            this.inputSong.Size = new System.Drawing.Size(959, 187);
            this.inputSong.TabIndex = 5;
            // 
            // tableSwap
            // 
            this.tableSwap.ColumnCount = 2;
            this.tableSwap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSwap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSwap.Controls.Add(this.discInAuthorLabel, 0, 0);
            this.tableSwap.Controls.Add(this.authorInDiscLabel, 1, 0);
            this.tableSwap.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableSwap.Location = new System.Drawing.Point(0, 88);
            this.tableSwap.Name = "tableSwap";
            this.tableSwap.RowCount = 1;
            this.tableSwap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSwap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSwap.Size = new System.Drawing.Size(969, 48);
            this.tableSwap.TabIndex = 1;
            // 
            // discInAuthorLabel
            // 
            this.discInAuthorLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.discInAuthorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discInAuthorLabel.Location = new System.Drawing.Point(3, 0);
            this.discInAuthorLabel.Name = "discInAuthorLabel";
            this.discInAuthorLabel.Size = new System.Drawing.Size(478, 48);
            this.discInAuthorLabel.TabIndex = 0;
            this.discInAuthorLabel.Text = "Create Disc In Author";
            this.discInAuthorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authorInDiscLabel
            // 
            this.authorInDiscLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.authorInDiscLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authorInDiscLabel.Location = new System.Drawing.Point(487, 0);
            this.authorInDiscLabel.Name = "authorInDiscLabel";
            this.authorInDiscLabel.Size = new System.Drawing.Size(479, 48);
            this.authorInDiscLabel.TabIndex = 1;
            this.authorInDiscLabel.Text = "Create Author in Disc";
            this.authorInDiscLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddNoteMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 1232);
            this.Controls.Add(this.tableInputs);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.tableSwap);
            this.Controls.Add(this.title);
            this.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AddNoteMenu";
            this.Text = "AddNoteMenu";
            this.title.ResumeLayout(false);
            this.tableInputs.ResumeLayout(false);
            this.dragDropPanel.ResumeLayout(false);
            this.tableSwap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel title;
        private Label titleLabel;
        private Button createButton;
        private TableLayoutPanel tableInputs;
        private Controls_and_Forms.UserControls.InputData inputAuthor;
        private Controls_and_Forms.UserControls.InputData inputSongFile;
        private Controls_and_Forms.UserControls.InputData inputDisc;
        private Panel dragDropPanel;
        private Label dragDropText;
        private Controls_and_Forms.UserControls.InputData inputSong;
        private TableLayoutPanel tableSwap;
        private Label discInAuthorLabel;
        private Label authorInDiscLabel;
    }
}