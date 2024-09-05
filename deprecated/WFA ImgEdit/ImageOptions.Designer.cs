namespace WFA_ImgEdit
{
    partial class ImageOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageOptions));
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.grp_fileFormat = new System.Windows.Forms.GroupBox();
            this.pb_fatWarning = new System.Windows.Forms.PictureBox();
            this.pb_rootEntryWarning = new System.Windows.Forms.PictureBox();
            this.lbl_fileFormat_numRootEntries = new System.Windows.Forms.Label();
            this.nup_fileFormat_numRootEntries = new System.Windows.Forms.NumericUpDown();
            this.pb_sectorsPerClusterWarning = new System.Windows.Forms.PictureBox();
            this.lbl_fileFormat_VolumeName = new System.Windows.Forms.Label();
            this.tbx_fileFormat_volumeName = new System.Windows.Forms.TextBox();
            this.lbl_fileFormat_sectorsPerCluster = new System.Windows.Forms.Label();
            this.lbl_fileFormat_fatCopies = new System.Windows.Forms.Label();
            this.cbx_fileFormat_customSerialNumber = new System.Windows.Forms.CheckBox();
            this.tbx_fileFormat_customSerialNumber = new System.Windows.Forms.TextBox();
            this.nup_fileFormat_sectorsPerCluster = new System.Windows.Forms.NumericUpDown();
            this.nup_fileFormat_fatCopies = new System.Windows.Forms.NumericUpDown();
            this.rbtn_fileFormat_FAT16 = new System.Windows.Forms.RadioButton();
            this.rbtn_fileFormat_FAT12 = new System.Windows.Forms.RadioButton();
            this.lbox_mediaFormat_mediaPreset = new System.Windows.Forms.ListBox();
            this.lbl_mediaFormat_MediaDesc = new System.Windows.Forms.Label();
            this.grp_mediaFormat = new System.Windows.Forms.GroupBox();
            this.pb_bytesPerSectorWarning = new System.Windows.Forms.PictureBox();
            this.btn_mediaFormat_renamePreset = new System.Windows.Forms.Button();
            this.btn_mediaFormat_newPreset = new System.Windows.Forms.Button();
            this.cbx_mediaFormat_kilo = new System.Windows.Forms.CheckBox();
            this.lbl_mediaFormat_tracks = new System.Windows.Forms.Label();
            this.nup_mediaFormat_tracks = new System.Windows.Forms.NumericUpDown();
            this.nup_mediaFormat_mediaDescriptor = new System.Windows.Forms.NumericUpDown();
            this.nup_mediaFormat_bytesPerSector = new System.Windows.Forms.NumericUpDown();
            this.nup_mediaFormat_sectors = new System.Windows.Forms.NumericUpDown();
            this.nup_mediaFormat_sides = new System.Windows.Forms.NumericUpDown();
            this.lbl_mediaFormat_size = new System.Windows.Forms.Label();
            this.lbl_mediaFormat_mediaDescriptor = new System.Windows.Forms.Label();
            this.lbl_mediaFormat_bytesPerSector = new System.Windows.Forms.Label();
            this.lbl_mediaFormat_sectors = new System.Windows.Forms.Label();
            this.lbl_mediaFormat_sides = new System.Windows.Forms.Label();
            this.btn_mediaFormat_deletePreset = new System.Windows.Forms.Button();
            this.btn_mediaFormat_savePreset = new System.Windows.Forms.Button();
            this.tt_sizeWarning = new System.Windows.Forms.ToolTip(this.components);
            this.tt_bytesPerSectorWarning = new System.Windows.Forms.ToolTip(this.components);
            this.tt_sectorsPerClusterWarning = new System.Windows.Forms.ToolTip(this.components);
            this.tt_fatWarning = new System.Windows.Forms.ToolTip(this.components);
            this.tt_rootEntryWarning = new System.Windows.Forms.ToolTip(this.components);
            this.grp_fileFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_fatWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rootEntryWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_numRootEntries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_sectorsPerClusterWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_sectorsPerCluster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_fatCopies)).BeginInit();
            this.grp_mediaFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_bytesPerSectorWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_tracks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_mediaDescriptor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_bytesPerSector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_sectors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_sides)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(275, 426);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(75, 23);
            this.btn_apply.TabIndex = 0;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.TextChanged += new System.EventHandler(this.btn_apply_TextChanged);
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.AutoSize = true;
            this.btn_cancel.Location = new System.Drawing.Point(12, 426);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Tag = "exclude";
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // grp_fileFormat
            // 
            this.grp_fileFormat.Controls.Add(this.pb_fatWarning);
            this.grp_fileFormat.Controls.Add(this.pb_rootEntryWarning);
            this.grp_fileFormat.Controls.Add(this.lbl_fileFormat_numRootEntries);
            this.grp_fileFormat.Controls.Add(this.nup_fileFormat_numRootEntries);
            this.grp_fileFormat.Controls.Add(this.pb_sectorsPerClusterWarning);
            this.grp_fileFormat.Controls.Add(this.lbl_fileFormat_VolumeName);
            this.grp_fileFormat.Controls.Add(this.tbx_fileFormat_volumeName);
            this.grp_fileFormat.Controls.Add(this.lbl_fileFormat_sectorsPerCluster);
            this.grp_fileFormat.Controls.Add(this.lbl_fileFormat_fatCopies);
            this.grp_fileFormat.Controls.Add(this.cbx_fileFormat_customSerialNumber);
            this.grp_fileFormat.Controls.Add(this.tbx_fileFormat_customSerialNumber);
            this.grp_fileFormat.Controls.Add(this.nup_fileFormat_sectorsPerCluster);
            this.grp_fileFormat.Controls.Add(this.nup_fileFormat_fatCopies);
            this.grp_fileFormat.Controls.Add(this.rbtn_fileFormat_FAT16);
            this.grp_fileFormat.Controls.Add(this.rbtn_fileFormat_FAT12);
            this.grp_fileFormat.Location = new System.Drawing.Point(12, 12);
            this.grp_fileFormat.Name = "grp_fileFormat";
            this.grp_fileFormat.Size = new System.Drawing.Size(345, 175);
            this.grp_fileFormat.TabIndex = 4;
            this.grp_fileFormat.TabStop = false;
            this.grp_fileFormat.Text = "Image Format";
            // 
            // pb_fatWarning
            // 
            this.pb_fatWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_fatWarning.Location = new System.Drawing.Point(224, 17);
            this.pb_fatWarning.Name = "pb_fatWarning";
            this.pb_fatWarning.Size = new System.Drawing.Size(20, 20);
            this.pb_fatWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_fatWarning.TabIndex = 35;
            this.pb_fatWarning.TabStop = false;
            // 
            // pb_rootEntryWarning
            // 
            this.pb_rootEntryWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_rootEntryWarning.Location = new System.Drawing.Point(224, 95);
            this.pb_rootEntryWarning.Name = "pb_rootEntryWarning";
            this.pb_rootEntryWarning.Size = new System.Drawing.Size(20, 20);
            this.pb_rootEntryWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rootEntryWarning.TabIndex = 34;
            this.pb_rootEntryWarning.TabStop = false;
            // 
            // lbl_fileFormat_numRootEntries
            // 
            this.lbl_fileFormat_numRootEntries.Location = new System.Drawing.Point(4, 99);
            this.lbl_fileFormat_numRootEntries.MinimumSize = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_numRootEntries.Name = "lbl_fileFormat_numRootEntries";
            this.lbl_fileFormat_numRootEntries.Size = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_numRootEntries.TabIndex = 33;
            this.lbl_fileFormat_numRootEntries.Text = "Root Entries:";
            this.lbl_fileFormat_numRootEntries.TextChanged += new System.EventHandler(this.lbl_fileFormat_numRootEntries_TextChanged);
            // 
            // nup_fileFormat_numRootEntries
            // 
            this.nup_fileFormat_numRootEntries.Location = new System.Drawing.Point(116, 95);
            this.nup_fileFormat_numRootEntries.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nup_fileFormat_numRootEntries.Name = "nup_fileFormat_numRootEntries";
            this.nup_fileFormat_numRootEntries.Size = new System.Drawing.Size(103, 20);
            this.nup_fileFormat_numRootEntries.TabIndex = 32;
            this.nup_fileFormat_numRootEntries.Value = new decimal(new int[] {
            224,
            0,
            0,
            0});
            this.nup_fileFormat_numRootEntries.ValueChanged += new System.EventHandler(this.nup_fileFormat_numRootEntries_ValueChanged);
            // 
            // pb_sectorsPerClusterWarning
            // 
            this.pb_sectorsPerClusterWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_sectorsPerClusterWarning.Location = new System.Drawing.Point(224, 69);
            this.pb_sectorsPerClusterWarning.Name = "pb_sectorsPerClusterWarning";
            this.pb_sectorsPerClusterWarning.Size = new System.Drawing.Size(20, 20);
            this.pb_sectorsPerClusterWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_sectorsPerClusterWarning.TabIndex = 29;
            this.pb_sectorsPerClusterWarning.TabStop = false;
            // 
            // lbl_fileFormat_VolumeName
            // 
            this.lbl_fileFormat_VolumeName.AutoSize = true;
            this.lbl_fileFormat_VolumeName.Location = new System.Drawing.Point(4, 126);
            this.lbl_fileFormat_VolumeName.Name = "lbl_fileFormat_VolumeName";
            this.lbl_fileFormat_VolumeName.Size = new System.Drawing.Size(76, 13);
            this.lbl_fileFormat_VolumeName.TabIndex = 9;
            this.lbl_fileFormat_VolumeName.Tag = "exclude";
            this.lbl_fileFormat_VolumeName.Text = "Volume Name:";
            // 
            // tbx_fileFormat_volumeName
            // 
            this.tbx_fileFormat_volumeName.Location = new System.Drawing.Point(147, 123);
            this.tbx_fileFormat_volumeName.MaxLength = 11;
            this.tbx_fileFormat_volumeName.Name = "tbx_fileFormat_volumeName";
            this.tbx_fileFormat_volumeName.Size = new System.Drawing.Size(192, 20);
            this.tbx_fileFormat_volumeName.TabIndex = 8;
            this.tbx_fileFormat_volumeName.TextChanged += new System.EventHandler(this.tbx_fileFormat_volumeName_TextChanged);
            // 
            // lbl_fileFormat_sectorsPerCluster
            // 
            this.lbl_fileFormat_sectorsPerCluster.Location = new System.Drawing.Point(4, 71);
            this.lbl_fileFormat_sectorsPerCluster.MinimumSize = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_sectorsPerCluster.Name = "lbl_fileFormat_sectorsPerCluster";
            this.lbl_fileFormat_sectorsPerCluster.Size = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_sectorsPerCluster.TabIndex = 7;
            this.lbl_fileFormat_sectorsPerCluster.Text = "Sectors per Custer:";
            this.lbl_fileFormat_sectorsPerCluster.TextChanged += new System.EventHandler(this.lbl_fileFormat_sectorsPerCluster_TextChanged);
            // 
            // lbl_fileFormat_fatCopies
            // 
            this.lbl_fileFormat_fatCopies.Location = new System.Drawing.Point(4, 45);
            this.lbl_fileFormat_fatCopies.MinimumSize = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_fatCopies.Name = "lbl_fileFormat_fatCopies";
            this.lbl_fileFormat_fatCopies.Size = new System.Drawing.Size(100, 13);
            this.lbl_fileFormat_fatCopies.TabIndex = 6;
            this.lbl_fileFormat_fatCopies.Text = "FAT Copies:";
            this.lbl_fileFormat_fatCopies.TextChanged += new System.EventHandler(this.lbl_fileFormat_fatCopies_TextChanged);
            // 
            // cbx_fileFormat_customSerialNumber
            // 
            this.cbx_fileFormat_customSerialNumber.Location = new System.Drawing.Point(4, 152);
            this.cbx_fileFormat_customSerialNumber.MinimumSize = new System.Drawing.Size(135, 17);
            this.cbx_fileFormat_customSerialNumber.Name = "cbx_fileFormat_customSerialNumber";
            this.cbx_fileFormat_customSerialNumber.Size = new System.Drawing.Size(135, 17);
            this.cbx_fileFormat_customSerialNumber.TabIndex = 5;
            this.cbx_fileFormat_customSerialNumber.Tag = "exclude";
            this.cbx_fileFormat_customSerialNumber.Text = "Custom Serial Number";
            this.cbx_fileFormat_customSerialNumber.UseVisualStyleBackColor = true;
            this.cbx_fileFormat_customSerialNumber.CheckedChanged += new System.EventHandler(this.cbx_fileFormat_customSerialNumber_CheckedChanged);
            this.cbx_fileFormat_customSerialNumber.TextChanged += new System.EventHandler(this.cbx_fileFormat_customSerialNumber_TextChanged);
            // 
            // tbx_fileFormat_customSerialNumber
            // 
            this.tbx_fileFormat_customSerialNumber.Enabled = false;
            this.tbx_fileFormat_customSerialNumber.Location = new System.Drawing.Point(147, 149);
            this.tbx_fileFormat_customSerialNumber.MaxLength = 8;
            this.tbx_fileFormat_customSerialNumber.Name = "tbx_fileFormat_customSerialNumber";
            this.tbx_fileFormat_customSerialNumber.Size = new System.Drawing.Size(192, 20);
            this.tbx_fileFormat_customSerialNumber.TabIndex = 4;
            this.tbx_fileFormat_customSerialNumber.TextChanged += new System.EventHandler(this.tbx_fileFormat_customSerialNumber_TextChanged);
            // 
            // nup_fileFormat_sectorsPerCluster
            // 
            this.nup_fileFormat_sectorsPerCluster.Location = new System.Drawing.Point(116, 69);
            this.nup_fileFormat_sectorsPerCluster.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nup_fileFormat_sectorsPerCluster.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_fileFormat_sectorsPerCluster.Name = "nup_fileFormat_sectorsPerCluster";
            this.nup_fileFormat_sectorsPerCluster.Size = new System.Drawing.Size(104, 20);
            this.nup_fileFormat_sectorsPerCluster.TabIndex = 3;
            this.nup_fileFormat_sectorsPerCluster.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_fileFormat_sectorsPerCluster.ValueChanged += new System.EventHandler(this.nup_fileFormat_sectorsPerCluster_ValueChanged);
            // 
            // nup_fileFormat_fatCopies
            // 
            this.nup_fileFormat_fatCopies.Location = new System.Drawing.Point(116, 43);
            this.nup_fileFormat_fatCopies.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nup_fileFormat_fatCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_fileFormat_fatCopies.Name = "nup_fileFormat_fatCopies";
            this.nup_fileFormat_fatCopies.Size = new System.Drawing.Size(104, 20);
            this.nup_fileFormat_fatCopies.TabIndex = 2;
            this.nup_fileFormat_fatCopies.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nup_fileFormat_fatCopies.ValueChanged += new System.EventHandler(this.nup_fileFormat_fatCopies_ValueChanged);
            // 
            // rbtn_fileFormat_FAT16
            // 
            this.rbtn_fileFormat_FAT16.AutoSize = true;
            this.rbtn_fileFormat_FAT16.Location = new System.Drawing.Point(70, 20);
            this.rbtn_fileFormat_FAT16.Name = "rbtn_fileFormat_FAT16";
            this.rbtn_fileFormat_FAT16.Size = new System.Drawing.Size(57, 17);
            this.rbtn_fileFormat_FAT16.TabIndex = 1;
            this.rbtn_fileFormat_FAT16.TabStop = true;
            this.rbtn_fileFormat_FAT16.Tag = "exclude";
            this.rbtn_fileFormat_FAT16.Text = "FAT16";
            this.rbtn_fileFormat_FAT16.UseVisualStyleBackColor = true;
            this.rbtn_fileFormat_FAT16.CheckedChanged += new System.EventHandler(this.rbtn_fileFormat_FAT16_CheckedChanged);
            // 
            // rbtn_fileFormat_FAT12
            // 
            this.rbtn_fileFormat_FAT12.AutoSize = true;
            this.rbtn_fileFormat_FAT12.Location = new System.Drawing.Point(7, 20);
            this.rbtn_fileFormat_FAT12.Name = "rbtn_fileFormat_FAT12";
            this.rbtn_fileFormat_FAT12.Size = new System.Drawing.Size(57, 17);
            this.rbtn_fileFormat_FAT12.TabIndex = 0;
            this.rbtn_fileFormat_FAT12.TabStop = true;
            this.rbtn_fileFormat_FAT12.Tag = "exclude";
            this.rbtn_fileFormat_FAT12.Text = "FAT12";
            this.rbtn_fileFormat_FAT12.UseVisualStyleBackColor = true;
            this.rbtn_fileFormat_FAT12.CheckedChanged += new System.EventHandler(this.rbtn_fileFormat_FAT12_CheckedChanged);
            // 
            // lbox_mediaFormat_mediaPreset
            // 
            this.lbox_mediaFormat_mediaPreset.FormattingEnabled = true;
            this.lbox_mediaFormat_mediaPreset.Location = new System.Drawing.Point(6, 32);
            this.lbox_mediaFormat_mediaPreset.Name = "lbox_mediaFormat_mediaPreset";
            this.lbox_mediaFormat_mediaPreset.Size = new System.Drawing.Size(332, 43);
            this.lbox_mediaFormat_mediaPreset.TabIndex = 5;
            this.lbox_mediaFormat_mediaPreset.Tag = "exclude";
            this.lbox_mediaFormat_mediaPreset.SelectedIndexChanged += new System.EventHandler(this.lbox_fileFormat_MediaDesc_SelectedIndexChanged);
            // 
            // lbl_mediaFormat_MediaDesc
            // 
            this.lbl_mediaFormat_MediaDesc.AutoSize = true;
            this.lbl_mediaFormat_MediaDesc.Location = new System.Drawing.Point(3, 16);
            this.lbl_mediaFormat_MediaDesc.Name = "lbl_mediaFormat_MediaDesc";
            this.lbl_mediaFormat_MediaDesc.Size = new System.Drawing.Size(72, 13);
            this.lbl_mediaFormat_MediaDesc.TabIndex = 3;
            this.lbl_mediaFormat_MediaDesc.Tag = "exclude";
            this.lbl_mediaFormat_MediaDesc.Text = "Media Preset:";
            // 
            // grp_mediaFormat
            // 
            this.grp_mediaFormat.Controls.Add(this.pb_bytesPerSectorWarning);
            this.grp_mediaFormat.Controls.Add(this.btn_mediaFormat_renamePreset);
            this.grp_mediaFormat.Controls.Add(this.btn_mediaFormat_newPreset);
            this.grp_mediaFormat.Controls.Add(this.cbx_mediaFormat_kilo);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_tracks);
            this.grp_mediaFormat.Controls.Add(this.nup_mediaFormat_tracks);
            this.grp_mediaFormat.Controls.Add(this.nup_mediaFormat_mediaDescriptor);
            this.grp_mediaFormat.Controls.Add(this.nup_mediaFormat_bytesPerSector);
            this.grp_mediaFormat.Controls.Add(this.nup_mediaFormat_sectors);
            this.grp_mediaFormat.Controls.Add(this.nup_mediaFormat_sides);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_size);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_mediaDescriptor);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_bytesPerSector);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_sectors);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_sides);
            this.grp_mediaFormat.Controls.Add(this.btn_mediaFormat_deletePreset);
            this.grp_mediaFormat.Controls.Add(this.btn_mediaFormat_savePreset);
            this.grp_mediaFormat.Controls.Add(this.lbox_mediaFormat_mediaPreset);
            this.grp_mediaFormat.Controls.Add(this.lbl_mediaFormat_MediaDesc);
            this.grp_mediaFormat.Location = new System.Drawing.Point(12, 187);
            this.grp_mediaFormat.Name = "grp_mediaFormat";
            this.grp_mediaFormat.Size = new System.Drawing.Size(345, 233);
            this.grp_mediaFormat.TabIndex = 5;
            this.grp_mediaFormat.TabStop = false;
            this.grp_mediaFormat.Text = "Media Format";
            // 
            // pb_bytesPerSectorWarning
            // 
            this.pb_bytesPerSectorWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_bytesPerSectorWarning.Location = new System.Drawing.Point(224, 158);
            this.pb_bytesPerSectorWarning.Name = "pb_bytesPerSectorWarning";
            this.pb_bytesPerSectorWarning.Size = new System.Drawing.Size(20, 20);
            this.pb_bytesPerSectorWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_bytesPerSectorWarning.TabIndex = 31;
            this.pb_bytesPerSectorWarning.TabStop = false;
            // 
            // btn_mediaFormat_renamePreset
            // 
            this.btn_mediaFormat_renamePreset.Enabled = false;
            this.btn_mediaFormat_renamePreset.Location = new System.Drawing.Point(250, 184);
            this.btn_mediaFormat_renamePreset.Name = "btn_mediaFormat_renamePreset";
            this.btn_mediaFormat_renamePreset.Size = new System.Drawing.Size(88, 23);
            this.btn_mediaFormat_renamePreset.TabIndex = 27;
            this.btn_mediaFormat_renamePreset.Text = "Rename Preset";
            this.btn_mediaFormat_renamePreset.UseVisualStyleBackColor = true;
            this.btn_mediaFormat_renamePreset.TextChanged += new System.EventHandler(this.btn_mediaFormat_renamePreset_TextChanged);
            this.btn_mediaFormat_renamePreset.Click += new System.EventHandler(this.btn_mediaFormat_renamePreset_Click);
            // 
            // btn_mediaFormat_newPreset
            // 
            this.btn_mediaFormat_newPreset.Location = new System.Drawing.Point(250, 155);
            this.btn_mediaFormat_newPreset.Name = "btn_mediaFormat_newPreset";
            this.btn_mediaFormat_newPreset.Size = new System.Drawing.Size(88, 23);
            this.btn_mediaFormat_newPreset.TabIndex = 26;
            this.btn_mediaFormat_newPreset.Text = "New Preset";
            this.btn_mediaFormat_newPreset.UseVisualStyleBackColor = true;
            this.btn_mediaFormat_newPreset.TextChanged += new System.EventHandler(this.btn_mediaFormat_newPreset_TextChanged);
            this.btn_mediaFormat_newPreset.Click += new System.EventHandler(this.btn_mediaFormat_newPreset_Click);
            // 
            // cbx_mediaFormat_kilo
            // 
            this.cbx_mediaFormat_kilo.AutoSize = true;
            this.cbx_mediaFormat_kilo.Location = new System.Drawing.Point(177, 210);
            this.cbx_mediaFormat_kilo.Name = "cbx_mediaFormat_kilo";
            this.cbx_mediaFormat_kilo.Size = new System.Drawing.Size(43, 17);
            this.cbx_mediaFormat_kilo.TabIndex = 25;
            this.cbx_mediaFormat_kilo.Text = "Kilo";
            this.cbx_mediaFormat_kilo.UseVisualStyleBackColor = true;
            this.cbx_mediaFormat_kilo.CheckedChanged += new System.EventHandler(this.cbx_mediaFormat_kilo_CheckedChanged);
            this.cbx_mediaFormat_kilo.TextChanged += new System.EventHandler(this.cbx_mediaFormat_kilo_TextChanged);
            // 
            // lbl_mediaFormat_tracks
            // 
            this.lbl_mediaFormat_tracks.Location = new System.Drawing.Point(4, 134);
            this.lbl_mediaFormat_tracks.MinimumSize = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_tracks.Name = "lbl_mediaFormat_tracks";
            this.lbl_mediaFormat_tracks.Size = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_tracks.TabIndex = 24;
            this.lbl_mediaFormat_tracks.Text = "Tracks per side:";
            this.lbl_mediaFormat_tracks.TextChanged += new System.EventHandler(this.lbl_mediaFormat_tracks_TextChanged);
            // 
            // nup_mediaFormat_tracks
            // 
            this.nup_mediaFormat_tracks.Location = new System.Drawing.Point(106, 132);
            this.nup_mediaFormat_tracks.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nup_mediaFormat_tracks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_tracks.Name = "nup_mediaFormat_tracks";
            this.nup_mediaFormat_tracks.Size = new System.Drawing.Size(113, 20);
            this.nup_mediaFormat_tracks.TabIndex = 23;
            this.nup_mediaFormat_tracks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_tracks.ValueChanged += new System.EventHandler(this.nup_mediaFormat_tracks_ValueChanged);
            // 
            // nup_mediaFormat_mediaDescriptor
            // 
            this.nup_mediaFormat_mediaDescriptor.Hexadecimal = true;
            this.nup_mediaFormat_mediaDescriptor.Location = new System.Drawing.Point(106, 184);
            this.nup_mediaFormat_mediaDescriptor.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nup_mediaFormat_mediaDescriptor.Name = "nup_mediaFormat_mediaDescriptor";
            this.nup_mediaFormat_mediaDescriptor.Size = new System.Drawing.Size(113, 20);
            this.nup_mediaFormat_mediaDescriptor.TabIndex = 21;
            this.nup_mediaFormat_mediaDescriptor.ValueChanged += new System.EventHandler(this.nup_mediaFormat_mediaDescriptor_ValueChanged);
            // 
            // nup_mediaFormat_bytesPerSector
            // 
            this.nup_mediaFormat_bytesPerSector.Location = new System.Drawing.Point(106, 158);
            this.nup_mediaFormat_bytesPerSector.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nup_mediaFormat_bytesPerSector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_bytesPerSector.Name = "nup_mediaFormat_bytesPerSector";
            this.nup_mediaFormat_bytesPerSector.Size = new System.Drawing.Size(113, 20);
            this.nup_mediaFormat_bytesPerSector.TabIndex = 20;
            this.nup_mediaFormat_bytesPerSector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_bytesPerSector.ValueChanged += new System.EventHandler(this.nup_mediaFormat_bytesPerSector_ValueChanged);
            // 
            // nup_mediaFormat_sectors
            // 
            this.nup_mediaFormat_sectors.Location = new System.Drawing.Point(106, 107);
            this.nup_mediaFormat_sectors.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nup_mediaFormat_sectors.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_sectors.Name = "nup_mediaFormat_sectors";
            this.nup_mediaFormat_sectors.Size = new System.Drawing.Size(113, 20);
            this.nup_mediaFormat_sectors.TabIndex = 19;
            this.nup_mediaFormat_sectors.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_sectors.ValueChanged += new System.EventHandler(this.nup_mediaFormat_sectors_ValueChanged);
            // 
            // nup_mediaFormat_sides
            // 
            this.nup_mediaFormat_sides.Location = new System.Drawing.Point(106, 81);
            this.nup_mediaFormat_sides.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nup_mediaFormat_sides.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_sides.Name = "nup_mediaFormat_sides";
            this.nup_mediaFormat_sides.Size = new System.Drawing.Size(113, 20);
            this.nup_mediaFormat_sides.TabIndex = 18;
            this.nup_mediaFormat_sides.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nup_mediaFormat_sides.ValueChanged += new System.EventHandler(this.nup_mediaFormat_sides_ValueChanged);
            // 
            // lbl_mediaFormat_size
            // 
            this.lbl_mediaFormat_size.AutoSize = true;
            this.lbl_mediaFormat_size.Location = new System.Drawing.Point(4, 211);
            this.lbl_mediaFormat_size.Name = "lbl_mediaFormat_size";
            this.lbl_mediaFormat_size.Size = new System.Drawing.Size(84, 13);
            this.lbl_mediaFormat_size.TabIndex = 16;
            this.lbl_mediaFormat_size.Tag = "exclude";
            this.lbl_mediaFormat_size.Text = "~Formated Size:";
            // 
            // lbl_mediaFormat_mediaDescriptor
            // 
            this.lbl_mediaFormat_mediaDescriptor.Location = new System.Drawing.Point(4, 186);
            this.lbl_mediaFormat_mediaDescriptor.MinimumSize = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_mediaDescriptor.Name = "lbl_mediaFormat_mediaDescriptor";
            this.lbl_mediaFormat_mediaDescriptor.Size = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_mediaDescriptor.TabIndex = 15;
            this.lbl_mediaFormat_mediaDescriptor.Text = "Media Descriptor:";
            this.lbl_mediaFormat_mediaDescriptor.TextChanged += new System.EventHandler(this.lbl_mediaFormat_mediaDescriptor_TextChanged);
            // 
            // lbl_mediaFormat_bytesPerSector
            // 
            this.lbl_mediaFormat_bytesPerSector.Location = new System.Drawing.Point(4, 160);
            this.lbl_mediaFormat_bytesPerSector.MinimumSize = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_bytesPerSector.Name = "lbl_mediaFormat_bytesPerSector";
            this.lbl_mediaFormat_bytesPerSector.Size = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_bytesPerSector.TabIndex = 14;
            this.lbl_mediaFormat_bytesPerSector.Text = "Bytes per Sector:";
            this.lbl_mediaFormat_bytesPerSector.TextChanged += new System.EventHandler(this.lbl_mediaFormat_bytesPerSector_TextChanged);
            // 
            // lbl_mediaFormat_sectors
            // 
            this.lbl_mediaFormat_sectors.Location = new System.Drawing.Point(4, 109);
            this.lbl_mediaFormat_sectors.MinimumSize = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_sectors.Name = "lbl_mediaFormat_sectors";
            this.lbl_mediaFormat_sectors.Size = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_sectors.TabIndex = 13;
            this.lbl_mediaFormat_sectors.Text = "Sectors per track:";
            this.lbl_mediaFormat_sectors.TextChanged += new System.EventHandler(this.lbl_mediaFormat_sectors_TextChanged);
            // 
            // lbl_mediaFormat_sides
            // 
            this.lbl_mediaFormat_sides.Location = new System.Drawing.Point(4, 83);
            this.lbl_mediaFormat_sides.MinimumSize = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_sides.Name = "lbl_mediaFormat_sides";
            this.lbl_mediaFormat_sides.Size = new System.Drawing.Size(92, 13);
            this.lbl_mediaFormat_sides.TabIndex = 12;
            this.lbl_mediaFormat_sides.Text = "Sides and Heads:";
            this.lbl_mediaFormat_sides.TextChanged += new System.EventHandler(this.lbl_mediaFormat_sides_TextChanged);
            // 
            // btn_mediaFormat_deletePreset
            // 
            this.btn_mediaFormat_deletePreset.Enabled = false;
            this.btn_mediaFormat_deletePreset.Location = new System.Drawing.Point(250, 98);
            this.btn_mediaFormat_deletePreset.Name = "btn_mediaFormat_deletePreset";
            this.btn_mediaFormat_deletePreset.Size = new System.Drawing.Size(88, 23);
            this.btn_mediaFormat_deletePreset.TabIndex = 7;
            this.btn_mediaFormat_deletePreset.Text = "Delete Preset";
            this.btn_mediaFormat_deletePreset.UseVisualStyleBackColor = true;
            this.btn_mediaFormat_deletePreset.TextChanged += new System.EventHandler(this.btn_mediaFormat_deletePreset_TextChanged);
            this.btn_mediaFormat_deletePreset.Click += new System.EventHandler(this.btn_mediaFormat_deletePreset_Click);
            // 
            // btn_mediaFormat_savePreset
            // 
            this.btn_mediaFormat_savePreset.Enabled = false;
            this.btn_mediaFormat_savePreset.Location = new System.Drawing.Point(250, 127);
            this.btn_mediaFormat_savePreset.Name = "btn_mediaFormat_savePreset";
            this.btn_mediaFormat_savePreset.Size = new System.Drawing.Size(88, 23);
            this.btn_mediaFormat_savePreset.TabIndex = 6;
            this.btn_mediaFormat_savePreset.Text = "Save Preset";
            this.btn_mediaFormat_savePreset.UseVisualStyleBackColor = true;
            this.btn_mediaFormat_savePreset.TextChanged += new System.EventHandler(this.btn_mediaFormat_savePreset_TextChanged);
            this.btn_mediaFormat_savePreset.Click += new System.EventHandler(this.btn_mediaFormat_savePreset_Click);
            // 
            // tt_sizeWarning
            // 
            this.tt_sizeWarning.AutomaticDelay = 0;
            this.tt_sizeWarning.AutoPopDelay = 10000;
            this.tt_sizeWarning.InitialDelay = 0;
            this.tt_sizeWarning.ReshowDelay = 0;
            // 
            // tt_bytesPerSectorWarning
            // 
            this.tt_bytesPerSectorWarning.AutomaticDelay = 0;
            this.tt_bytesPerSectorWarning.AutoPopDelay = 10000;
            this.tt_bytesPerSectorWarning.InitialDelay = 0;
            this.tt_bytesPerSectorWarning.ReshowDelay = 0;
            // 
            // tt_sectorsPerClusterWarning
            // 
            this.tt_sectorsPerClusterWarning.AutomaticDelay = 0;
            this.tt_sectorsPerClusterWarning.AutoPopDelay = 10000;
            this.tt_sectorsPerClusterWarning.InitialDelay = 0;
            this.tt_sectorsPerClusterWarning.ReshowDelay = 0;
            // 
            // tt_fatWarning
            // 
            this.tt_fatWarning.AutomaticDelay = 0;
            this.tt_fatWarning.AutoPopDelay = 10000;
            this.tt_fatWarning.InitialDelay = 0;
            this.tt_fatWarning.ReshowDelay = 0;
            // 
            // tt_rootEntryWarning
            // 
            this.tt_rootEntryWarning.AutomaticDelay = 0;
            this.tt_rootEntryWarning.AutoPopDelay = 10000;
            this.tt_rootEntryWarning.InitialDelay = 0;
            this.tt_rootEntryWarning.ReshowDelay = 0;
            // 
            // ImageOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 461);
            this.Controls.Add(this.grp_mediaFormat);
            this.Controls.Add(this.grp_fileFormat);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(385, 500);
            this.Name = "ImageOptions";
            this.Text = "Image Options";
            this.Load += new System.EventHandler(this.ImageOptions_Load);
            this.grp_fileFormat.ResumeLayout(false);
            this.grp_fileFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_fatWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rootEntryWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_numRootEntries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_sectorsPerClusterWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_sectorsPerCluster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_fileFormat_fatCopies)).EndInit();
            this.grp_mediaFormat.ResumeLayout(false);
            this.grp_mediaFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_bytesPerSectorWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_tracks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_mediaDescriptor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_bytesPerSector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_sectors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nup_mediaFormat_sides)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.GroupBox grp_fileFormat;
        private System.Windows.Forms.RadioButton rbtn_fileFormat_FAT16;
        private System.Windows.Forms.RadioButton rbtn_fileFormat_FAT12;
        private System.Windows.Forms.Label lbl_mediaFormat_MediaDesc;
        public System.Windows.Forms.ListBox lbox_mediaFormat_mediaPreset;
        private System.Windows.Forms.GroupBox grp_mediaFormat;
        private System.Windows.Forms.Button btn_mediaFormat_savePreset;
        private System.Windows.Forms.Button btn_mediaFormat_deletePreset;
        private System.Windows.Forms.Label lbl_mediaFormat_size;
        private System.Windows.Forms.Label lbl_mediaFormat_mediaDescriptor;
        private System.Windows.Forms.Label lbl_mediaFormat_bytesPerSector;
        private System.Windows.Forms.Label lbl_mediaFormat_sectors;
        private System.Windows.Forms.Label lbl_mediaFormat_sides;
        private System.Windows.Forms.NumericUpDown nup_mediaFormat_mediaDescriptor;
        private System.Windows.Forms.NumericUpDown nup_mediaFormat_bytesPerSector;
        private System.Windows.Forms.NumericUpDown nup_mediaFormat_sectors;
        private System.Windows.Forms.NumericUpDown nup_mediaFormat_sides;
        private System.Windows.Forms.Label lbl_mediaFormat_tracks;
        private System.Windows.Forms.NumericUpDown nup_mediaFormat_tracks;
        private System.Windows.Forms.CheckBox cbx_mediaFormat_kilo;
        private System.Windows.Forms.Button btn_mediaFormat_newPreset;
        private System.Windows.Forms.Button btn_mediaFormat_renamePreset;
        private System.Windows.Forms.CheckBox cbx_fileFormat_customSerialNumber;
        private System.Windows.Forms.TextBox tbx_fileFormat_customSerialNumber;
        private System.Windows.Forms.NumericUpDown nup_fileFormat_sectorsPerCluster;
        private System.Windows.Forms.NumericUpDown nup_fileFormat_fatCopies;
        private System.Windows.Forms.Label lbl_fileFormat_sectorsPerCluster;
        private System.Windows.Forms.Label lbl_fileFormat_fatCopies;
        private System.Windows.Forms.ToolTip tt_sizeWarning;
        private System.Windows.Forms.TextBox tbx_fileFormat_volumeName;
        private System.Windows.Forms.Label lbl_fileFormat_VolumeName;
        private System.Windows.Forms.PictureBox pb_sectorsPerClusterWarning;
        private System.Windows.Forms.PictureBox pb_bytesPerSectorWarning;
        private System.Windows.Forms.ToolTip tt_bytesPerSectorWarning;
        private System.Windows.Forms.ToolTip tt_sectorsPerClusterWarning;
        private System.Windows.Forms.Label lbl_fileFormat_numRootEntries;
        private System.Windows.Forms.NumericUpDown nup_fileFormat_numRootEntries;
        private System.Windows.Forms.PictureBox pb_fatWarning;
        private System.Windows.Forms.PictureBox pb_rootEntryWarning;
        private System.Windows.Forms.ToolTip tt_fatWarning;
        private System.Windows.Forms.ToolTip tt_rootEntryWarning;
    }
}