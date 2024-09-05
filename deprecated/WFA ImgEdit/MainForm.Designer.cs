
namespace WFA_ImgEdit
{
    partial class form_main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_main));
            this.btn_createImage = new System.Windows.Forms.Button();
            this.btn_selFiles = new System.Windows.Forms.Button();
            this.btn_selOutputFile = new System.Windows.Forms.Button();
            this.ofd_selectInput = new System.Windows.Forms.OpenFileDialog();
            this.sfd_selectOutput = new System.Windows.Forms.SaveFileDialog();
            this.tbx_outputFile = new System.Windows.Forms.TextBox();
            this.lbl_outputFile = new System.Windows.Forms.Label();
            this.tre_fileViewer = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ImgOptions = new System.Windows.Forms.Button();
            this.fbd_selectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_selFolder = new System.Windows.Forms.Button();
            this.btn_removeElement = new System.Windows.Forms.Button();
            this.lbl_freeSpace = new System.Windows.Forms.Label();
            this.lbl_usedSpace = new System.Windows.Forms.Label();
            this.lbl_availabeSpace = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_createImage
            // 
            this.btn_createImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_createImage.Location = new System.Drawing.Point(132, 433);
            this.btn_createImage.Name = "btn_createImage";
            this.btn_createImage.Size = new System.Drawing.Size(135, 23);
            this.btn_createImage.TabIndex = 9;
            this.btn_createImage.Text = "Create Image";
            this.btn_createImage.UseVisualStyleBackColor = true;
            this.btn_createImage.Click += new System.EventHandler(this.btm_createImage_Click);
            // 
            // btn_selFiles
            // 
            this.btn_selFiles.AutoSize = true;
            this.btn_selFiles.Location = new System.Drawing.Point(23, 27);
            this.btn_selFiles.Name = "btn_selFiles";
            this.btn_selFiles.Size = new System.Drawing.Size(81, 23);
            this.btn_selFiles.TabIndex = 2;
            this.btn_selFiles.Text = "Add files";
            this.btn_selFiles.UseVisualStyleBackColor = true;
            this.btn_selFiles.Click += new System.EventHandler(this.btn_selInputFile_Click);
            this.btn_selFiles.Resize += new System.EventHandler(this.btn_selFiles_Resize);
            // 
            // btn_selOutputFile
            // 
            this.btn_selOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_selOutputFile.AutoSize = true;
            this.btn_selOutputFile.Location = new System.Drawing.Point(338, 404);
            this.btn_selOutputFile.Name = "btn_selOutputFile";
            this.btn_selOutputFile.Size = new System.Drawing.Size(26, 23);
            this.btn_selOutputFile.TabIndex = 4;
            this.btn_selOutputFile.Text = "...";
            this.btn_selOutputFile.UseVisualStyleBackColor = true;
            this.btn_selOutputFile.Click += new System.EventHandler(this.btn_selOutputFile_Click);
            // 
            // ofd_selectInput
            // 
            this.ofd_selectInput.Multiselect = true;
            this.ofd_selectInput.Title = "Select File";
            this.ofd_selectInput.FileOk += new System.ComponentModel.CancelEventHandler(this.ofd_selectInput_FileOk);
            // 
            // sfd_selectOutput
            // 
            this.sfd_selectOutput.DefaultExt = "img";
            this.sfd_selectOutput.FileName = "image.img";
            this.sfd_selectOutput.Filter = "Image file|*.img|All Files|*.*";
            this.sfd_selectOutput.OverwritePrompt = false;
            this.sfd_selectOutput.Title = "Select output file";
            this.sfd_selectOutput.FileOk += new System.ComponentModel.CancelEventHandler(this.sfd_selectOutput_FileOk);
            // 
            // tbx_outputFile
            // 
            this.tbx_outputFile.AllowDrop = true;
            this.tbx_outputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx_outputFile.Location = new System.Drawing.Point(87, 407);
            this.tbx_outputFile.Name = "tbx_outputFile";
            this.tbx_outputFile.Size = new System.Drawing.Size(245, 20);
            this.tbx_outputFile.TabIndex = 3;
            this.tbx_outputFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbx_outputFile_DragDrop);
            this.tbx_outputFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbx_outputFile_DragEnter);
            // 
            // lbl_outputFile
            // 
            this.lbl_outputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_outputFile.Location = new System.Drawing.Point(20, 410);
            this.lbl_outputFile.MinimumSize = new System.Drawing.Size(64, 0);
            this.lbl_outputFile.Name = "lbl_outputFile";
            this.lbl_outputFile.Size = new System.Drawing.Size(64, 13);
            this.lbl_outputFile.TabIndex = 0;
            this.lbl_outputFile.Text = "Output File:";
            this.lbl_outputFile.TextChanged += new System.EventHandler(this.lbl_outputFile_TextChanged);
            // 
            // tre_fileViewer
            // 
            this.tre_fileViewer.AllowDrop = true;
            this.tre_fileViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tre_fileViewer.Location = new System.Drawing.Point(22, 56);
            this.tre_fileViewer.Name = "tre_fileViewer";
            this.tre_fileViewer.Size = new System.Drawing.Size(342, 332);
            this.tre_fileViewer.TabIndex = 10;
            this.tre_fileViewer.DragDrop += new System.Windows.Forms.DragEventHandler(this.tre_fileViewer_DragDrop);
            this.tre_fileViewer.DragEnter += new System.Windows.Forms.DragEventHandler(this.tre_fileViewer_DragEnter);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(384, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStip";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageOptionsToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // imageOptionsToolStripMenuItem
            // 
            this.imageOptionsToolStripMenuItem.Name = "imageOptionsToolStripMenuItem";
            this.imageOptionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.imageOptionsToolStripMenuItem.Text = "Image Options";
            this.imageOptionsToolStripMenuItem.Click += new System.EventHandler(this.imageOptionsToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.languageToolStripMenuItem.Text = "Language";
            this.languageToolStripMenuItem.Click += new System.EventHandler(this.languageToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btn_ImgOptions
            // 
            this.btn_ImgOptions.AutoSize = true;
            this.btn_ImgOptions.Location = new System.Drawing.Point(283, 27);
            this.btn_ImgOptions.Name = "btn_ImgOptions";
            this.btn_ImgOptions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_ImgOptions.Size = new System.Drawing.Size(81, 23);
            this.btn_ImgOptions.TabIndex = 12;
            this.btn_ImgOptions.Text = "Options";
            this.btn_ImgOptions.UseVisualStyleBackColor = true;
            this.btn_ImgOptions.Click += new System.EventHandler(this.btn_ImgOptions_Click);
            this.btn_ImgOptions.Resize += new System.EventHandler(this.btn_ImgOptions_Resize);
            // 
            // fbd_selectFolder
            // 
            this.fbd_selectFolder.ShowNewFolderButton = false;
            // 
            // btn_selFolder
            // 
            this.btn_selFolder.AutoSize = true;
            this.btn_selFolder.Location = new System.Drawing.Point(109, 27);
            this.btn_selFolder.Name = "btn_selFolder";
            this.btn_selFolder.Size = new System.Drawing.Size(81, 23);
            this.btn_selFolder.TabIndex = 13;
            this.btn_selFolder.Text = "Add folder";
            this.btn_selFolder.UseVisualStyleBackColor = true;
            this.btn_selFolder.Click += new System.EventHandler(this.btn_selFolder_Click);
            this.btn_selFolder.Resize += new System.EventHandler(this.btn_selFolder_Resize);
            // 
            // btn_removeElement
            // 
            this.btn_removeElement.AutoSize = true;
            this.btn_removeElement.Location = new System.Drawing.Point(196, 27);
            this.btn_removeElement.Name = "btn_removeElement";
            this.btn_removeElement.Size = new System.Drawing.Size(81, 23);
            this.btn_removeElement.TabIndex = 14;
            this.btn_removeElement.Text = "Remove";
            this.btn_removeElement.UseVisualStyleBackColor = true;
            this.btn_removeElement.Click += new System.EventHandler(this.btn_removeElement_Click);
            this.btn_removeElement.Resize += new System.EventHandler(this.btn_removeElement_Resize);
            // 
            // lbl_freeSpace
            // 
            this.lbl_freeSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_freeSpace.AutoEllipsis = true;
            this.lbl_freeSpace.Location = new System.Drawing.Point(136, 391);
            this.lbl_freeSpace.MinimumSize = new System.Drawing.Size(110, 0);
            this.lbl_freeSpace.Name = "lbl_freeSpace";
            this.lbl_freeSpace.Size = new System.Drawing.Size(110, 13);
            this.lbl_freeSpace.TabIndex = 15;
            this.lbl_freeSpace.Text = "Free: 000 bytes";
            this.lbl_freeSpace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_usedSpace
            // 
            this.lbl_usedSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_usedSpace.AutoEllipsis = true;
            this.lbl_usedSpace.Location = new System.Drawing.Point(20, 391);
            this.lbl_usedSpace.MinimumSize = new System.Drawing.Size(110, 0);
            this.lbl_usedSpace.Name = "lbl_usedSpace";
            this.lbl_usedSpace.Size = new System.Drawing.Size(110, 13);
            this.lbl_usedSpace.TabIndex = 16;
            this.lbl_usedSpace.Text = "Used: 000 bytes";
            this.lbl_usedSpace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_availabeSpace
            // 
            this.lbl_availabeSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_availabeSpace.AutoEllipsis = true;
            this.lbl_availabeSpace.Location = new System.Drawing.Point(254, 391);
            this.lbl_availabeSpace.MinimumSize = new System.Drawing.Size(110, 0);
            this.lbl_availabeSpace.Name = "lbl_availabeSpace";
            this.lbl_availabeSpace.Size = new System.Drawing.Size(110, 13);
            this.lbl_availabeSpace.TabIndex = 17;
            this.lbl_availabeSpace.Text = "Disk: 000 bytes";
            this.lbl_availabeSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // form_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.lbl_availabeSpace);
            this.Controls.Add(this.lbl_usedSpace);
            this.Controls.Add(this.lbl_freeSpace);
            this.Controls.Add(this.btn_removeElement);
            this.Controls.Add(this.btn_selFolder);
            this.Controls.Add(this.btn_ImgOptions);
            this.Controls.Add(this.tre_fileViewer);
            this.Controls.Add(this.btn_selOutputFile);
            this.Controls.Add(this.btn_selFiles);
            this.Controls.Add(this.lbl_outputFile);
            this.Controls.Add(this.tbx_outputFile);
            this.Controls.Add(this.btn_createImage);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "form_main";
            this.Text = "ImageEdit";
            this.Load += new System.EventHandler(this.form_main_Load);
            this.Resize += new System.EventHandler(this.form_main_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_createImage;
        private System.Windows.Forms.Button btn_selFiles;
        private System.Windows.Forms.Button btn_selOutputFile;
        private System.Windows.Forms.OpenFileDialog ofd_selectInput;
        private System.Windows.Forms.SaveFileDialog sfd_selectOutput;
        private System.Windows.Forms.TextBox tbx_outputFile;
        private System.Windows.Forms.Label lbl_outputFile;
        private System.Windows.Forms.TreeView tre_fileViewer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btn_ImgOptions;
        private System.Windows.Forms.FolderBrowserDialog fbd_selectFolder;
        private System.Windows.Forms.Button btn_selFolder;
        private System.Windows.Forms.Button btn_removeElement;
        private System.Windows.Forms.Label lbl_freeSpace;
        private System.Windows.Forms.Label lbl_usedSpace;
        private System.Windows.Forms.Label lbl_availabeSpace;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
    }
}

