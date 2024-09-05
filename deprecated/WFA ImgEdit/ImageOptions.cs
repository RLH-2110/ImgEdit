using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ImageEditCore;

namespace WFA_ImgEdit
{
    public partial class ImageOptions : Form
    {

        private form_main parrent;
        private Settings settings;
        bool suppressUpdates;
        bool presetChanged;
        Bitmap warning;
        int oldMediapresetIndex = 0;

        ushort optimalRootEntries;
        byte[] RootEntrySectors;
        enum RootEntrySectorsIndex
        {
            fat12,fat16
        }

        // change to these: https://en.wikipedia.org/wiki/Design_of_the_FAT_file_system#BPB20_OFS_0Ah

        List <MediaFormat> Presets = new List<MediaFormat>();

        public string setName;


        public ImageOptions(form_main parrent)
        {
            InitializeComponent();

            //////////////////////////
            /// initalize  Variables ///
            //////////////////////////
            
            this.parrent = parrent;
            settings = parrent.settings;

            suppressUpdates = true;

            warning = WFA_ImgEdit.Properties.Resources.warning;
            warning.MakeTransparent(Color.White);

            optimalRootEntries = 224;

            RootEntrySectors = new byte[] {14, 32}; // 14 sectors for fat12. 32 sectors for fat16

            presetChanged = false;

            ///////////////////////
            /// initalize Presets ///
            //////////////////////

            Presets = Core.loadPresets(Program.console);

            foreach (MediaFormat preset in Presets)
            {
                lbox_mediaFormat_mediaPreset.Items.Add(preset.name);
            }



            //////////////////////////
            /// initalize Components ///
            //////////////////////////

            cbx_mediaFormat_kilo.Checked = Core.kilo;

            if (settings.FAT16)
            {
                rbtn_fileFormat_FAT16.Checked = true;
            }
            else
            {
                rbtn_fileFormat_FAT12.Checked = true;
            }

            nup_fileFormat_fatCopies.Value = settings.NumberFATcopies;
            nup_fileFormat_sectorsPerCluster.Value = settings.SectorsPerCluster;
            nup_fileFormat_numRootEntries.Value = settings.NumberRootEntries;
            
            // initalize volume label textbox
            foreach(byte b in settings.VolumeLabel)
            {
                tbx_fileFormat_volumeName.Text += (char)b;
            }
            {

                int lastNonWhiteSpace = tbx_fileFormat_volumeName.Text.Length - 1;
                // remove trainling spaces
                for (int i = lastNonWhiteSpace; i > 0; i--)
                {
                    if (tbx_fileFormat_volumeName.Text[i] != ' ')
                    {
                        lastNonWhiteSpace = i;
                        break;
                    }
                }
                tbx_fileFormat_volumeName.Text = tbx_fileFormat_volumeName.Text.Substring(0, lastNonWhiteSpace + 1);
            }




            if (settings.CustiomVolumeSerialNumber)
            {
                cbx_fileFormat_customSerialNumber.Checked = true;
                tbx_fileFormat_customSerialNumber.Text = settings.VolumeSerialNumber.ToString("X8");
            }


            if (settings.DiskTypeUI < lbox_mediaFormat_mediaPreset.Items.Count) // if its a valid index
            {
                lbox_mediaFormat_mediaPreset.SelectedIndex = settings.DiskTypeUI;
                oldMediapresetIndex = lbox_mediaFormat_mediaPreset.SelectedIndex;
            }
            
            nup_mediaFormat_sides.Value = settings.Sides;
            nup_mediaFormat_sectors.Value = settings.SectorsPerTrack;
            nup_mediaFormat_tracks.Value = settings.TracksPerSide;
            nup_mediaFormat_bytesPerSector.Value = settings.BytesPerSector;
            nup_mediaFormat_mediaDescriptor.Value = settings.MediaDescriptor;

            suppressUpdates = false;

            calulateSize();




        }

      














        private bool handleUnsavedChanges(object sender, EventArgs e, int index)
        {
            if (suppressUpdates) return false;

            suppressUpdates = true;
            this.SuspendLayout();

            if (presetChanged && Core.presetUnsavedChangesStatus > 0)
            {
                DialogResult res;

                if (!Presets[index].buildin)
                    res = Core.output(Core.getString("unsavedchanges_yes_no"), Core.getString("unsavedchanges_title"), MessageBoxButtons.YesNo, MessageBoxIcon.Information, Program.console); // also do unsaved 
                else
                    res = Core.output(Core.getString("unsavedchanges_ok_cancel"), Core.getString("unsavedchanges_title"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information, Program.console); // also do unsaved 

                if (res == DialogResult.Yes)
                {
                    // use index
                    int tmpIndex = lbox_mediaFormat_mediaPreset.SelectedIndex;
                    lbox_mediaFormat_mediaPreset.SelectedIndex = index;

                    // save
                    btn_mediaFormat_savePreset_Click(sender, e); 

                    // restore index
                    lbox_mediaFormat_mediaPreset.SelectedIndex = tmpIndex;
                }

                if (res == DialogResult.Cancel)
                {
                    lbox_mediaFormat_mediaPreset.SelectedIndex = index; // restore the old index
                    // update the values
                    {
                        nup_mediaFormat_mediaDescriptor.Value = settings.MediaDescriptor;
                        nup_mediaFormat_bytesPerSector.Value = settings.BytesPerSector;
                        nup_mediaFormat_sides.Value = settings.Sides;
                        nup_mediaFormat_tracks.Value = settings.TracksPerSide;
                        nup_mediaFormat_sectors.Value = settings.SectorsPerTrack;
                    }

                    this.ResumeLayout();
                    suppressUpdates = false;

                    return false;
                }
            }
            

            this.ResumeLayout();
            suppressUpdates = false;
            return true;
        }

        private void handleUnsavedChanges(object sender, EventArgs e)
        {
            handleUnsavedChanges(sender, e, lbox_mediaFormat_mediaPreset.SelectedIndex);
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (tbx_fileFormat_volumeName.Text.Length == 0) tbx_fileFormat_volumeName.Text = "NO NAME";

            handleUnsavedChanges(sender,e);

            parrent.settings = settings;    // apply the settings
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            handleUnsavedChanges(sender, e);
            this.Close();
        }







        private void rbtn_fileFormat_FAT12_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fileFormat_FAT12.Checked)
            {
                settings.FAT16 = false;
            }

            // root direcroty is 14 sectors in fat12
            optimalRootEntries = (ushort)((RootEntrySectors[(byte)RootEntrySectorsIndex.fat12] * nup_mediaFormat_bytesPerSector.Value) / 32); // In the FAT12 file system, the root directory size is fixed at 14 sectors, and each sector is 512 bytes. Therefore, 14 sectors * 512 bytes/sector = 7,168 bytes.  //224;
            nup_fileFormat_numRootEntries.Value = optimalRootEntries;

            checkFormatWarning();

            calulateSize(); // in case the filesystem limitations are important
        }

        private void rbtn_fileFormat_FAT16_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fileFormat_FAT16.Checked)
            {
                settings.FAT16 = true;
            }


            // root direcroty is 32 sectors in fat16
            optimalRootEntries = (ushort)((RootEntrySectors[(byte)RootEntrySectorsIndex.fat16] * nup_mediaFormat_bytesPerSector.Value) / 32); // In the FAT12 file system, the root directory size is fixed at 14 sectors, and each sector is 512 bytes. Therefore, 14 sectors * 512 bytes/sector = 7,168 bytes.  //224;
            nup_fileFormat_numRootEntries.Value = optimalRootEntries;

            checkFormatWarning();

            calulateSize(); // in case the filesystem limitations are important
        }


        public void checkFormatWarning()
        {
            tt_fatWarning.RemoveAll();

            if (rbtn_fileFormat_FAT12.Checked)
            {
                uint sectors = (uint)(nup_mediaFormat_sides.Value * nup_mediaFormat_tracks.Value * nup_mediaFormat_sectors.Value);
                if (sectors > 4096)
                {
                    pb_fatWarning.Image = warning;
                    tt_fatWarning.SetToolTip(pb_fatWarning,Core.getStringFormated("wrong_fat", "FAT16", "FAT12"));
                }
                else
                {
                    pb_fatWarning.Image = null;
                    tt_fatWarning.RemoveAll();
                }
            }
            else
            {

                uint sectors = (uint)(nup_mediaFormat_sides.Value * nup_mediaFormat_tracks.Value * nup_mediaFormat_sectors.Value);
                if (sectors <= 4096)
                {
                    pb_fatWarning.Image = warning;
                    tt_fatWarning.SetToolTip(pb_fatWarning, Core.getStringFormated("wrong_fat", "FAT12", "FAT16"));
                }
                else
                {
                    //    pb_fatWarning.Image = null;
                    //    tt_fatWarning.RemoveAll();
                    pb_fatWarning.Image = warning;
                    tt_fatWarning.SetToolTip(pb_fatWarning, Core.getString("fat16_notimplemented"));
                }
            }

            if (optimalRootEntries != nup_fileFormat_numRootEntries.Value)
            {
                pb_rootEntryWarning.Image = warning;
                tt_rootEntryWarning.SetToolTip(pb_rootEntryWarning, Core.getStringFormated("rootentries_warningtt",optimalRootEntries));
            }
            else
            {
                pb_rootEntryWarning.Image = null;
                tt_rootEntryWarning.RemoveAll();
            }

        }





        private void lbox_fileFormat_MediaDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressUpdates) return;

            if(!handleUnsavedChanges(sender,e,oldMediapresetIndex)) return; // about if we had to cancel
            oldMediapresetIndex = lbox_mediaFormat_mediaPreset.SelectedIndex;

            settings.DiskTypeUI = lbox_mediaFormat_mediaPreset.SelectedIndex;                                      // save the selection the user has made     (UI only (needed since some mediaDesciptors are shared between formats)
            if (lbox_mediaFormat_mediaPreset.SelectedIndex >= Presets.Count)
            {
                Core.output(Core.getStringFormated("configurationfile_notfound", lbox_mediaFormat_mediaPreset.SelectedItem), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, Program.console);
                return;
            }

            suppressUpdates = true;
            // apply changes
            {
                // save the Media Desciptor the user has selected
                settings.MediaDescriptor = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].mediaDescriptor;
                nup_mediaFormat_mediaDescriptor.Value = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].mediaDescriptor;

                // save bytes per sector
                settings.BytesPerSector = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].bytesPerSector;
                nup_mediaFormat_bytesPerSector.Value = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].bytesPerSector;

                // save number sectors
                settings.NumberOfSectors = (uint)(Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sides * Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].tracksPerSide * Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sectorsPerTrack);

                nup_mediaFormat_sides.Value = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sides;
                nup_mediaFormat_tracks.Value = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].tracksPerSide;
                nup_mediaFormat_sectors.Value = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sectorsPerTrack;
            }
            suppressUpdates = false;
            calulateSize();

            presetChanged = false;
            btn_mediaFormat_savePreset.Text = Core.getString("savepreset");

            // update buttons
            {
                if (Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].buildin)
                {
                    btn_mediaFormat_deletePreset.Enabled = false;
                    btn_mediaFormat_renamePreset.Enabled = false;
                    btn_mediaFormat_savePreset.Enabled = false;
                }
                else
                {
                    btn_mediaFormat_deletePreset.Enabled = true;
                    btn_mediaFormat_renamePreset.Enabled = true;
                    btn_mediaFormat_savePreset.Enabled = true;
                }
            }

            checkFormatWarning();
            
        }















        private void nup_mediaFormat_bytesPerSector_ValueChanged(object sender, EventArgs e)
        {
            // check if its a supported value
            switch ((ushort)nup_mediaFormat_bytesPerSector.Value)
            {
                case 512:
                case 1024:
                case 2048:
                case 4096:
                    pb_bytesPerSectorWarning.Image = null;
                    tt_bytesPerSectorWarning.RemoveAll();
                    break;
                default:
                    pb_bytesPerSectorWarning.Image = warning;
                    tt_bytesPerSectorWarning.SetToolTip(pb_bytesPerSectorWarning,Core.getString("bytes_per_sector_warningtt"));
                    break;
            }

            
            optimalRootEntries = (ushort)((RootEntrySectors[Core.boolAsIndex(rbtn_fileFormat_FAT16.Checked)] * nup_mediaFormat_bytesPerSector.Value) / 32); // get the right value dependend on the fat
            nup_fileFormat_numRootEntries.Value = optimalRootEntries;

            presetChange();
        }

        private void nup_mediaFormat_sides_ValueChanged(object sender, EventArgs e)
        {
            settings.Sides = (byte)nup_mediaFormat_sides.Value;
            presetChange();

        }

        private void nup_mediaFormat_sectors_ValueChanged(object sender, EventArgs e)
        {
            settings.SectorsPerTrack = (byte)nup_mediaFormat_sectors.Value;
            presetChange();
        }

        private void nup_mediaFormat_tracks_ValueChanged(object sender, EventArgs e)
        {
            settings.TracksPerSide = (ushort)nup_mediaFormat_tracks.Value;
            presetChange();
        }

        private void nup_mediaFormat_mediaDescriptor_ValueChanged(object sender, EventArgs e)
        {
            presetChange();
        }

        private void presetChange()
        {
            if (Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].buildin == false || Core.presetUnsavedChangesStatus == Core.UnsavedChangesStatus.allPresetWarning) // do not count changed to buldin, unless we have the setting that makes us do that
            {
                if (!presetChanged) // check if we need to change the text, if not, we can save time by not searching for the text.
                {
                    btn_mediaFormat_savePreset.Text = Core.getString("savepreset_changed");
                    presetChanged = true;
                }
            }
            calulateSize();
        }

        private bool calulateSize()
        {
            if (suppressUpdates) return false;


            uint sectors = (uint)(nup_mediaFormat_sides.Value * nup_mediaFormat_tracks.Value * nup_mediaFormat_sectors.Value);
            settings.NumberOfSectors = sectors;
            settings.MediaDescriptor = (byte)nup_mediaFormat_mediaDescriptor.Value;
            settings.BytesPerSector = (ushort)nup_mediaFormat_bytesPerSector.Value;

            // display the size
            ImageGenerator.DiskRequirements diskReq = ImageGenerator.contentFitsInsideDisk(new List<FileAndDir>(), settings, Program.console);

            

            ulong size = 0;

            if (diskReq.availableDiskSize > diskReq.neededDiskSize) // if the number is positive
                size = diskReq.availableDiskSize - diskReq.neededDiskSize;

            Core.DisplayInUnits_return textInserts = Core.DisplayInUnits(size, cbx_mediaFormat_kilo.Checked);
            lbl_mediaFormat_size.Text = Core.getStringFormated("formatedsize", textInserts.number, textInserts.text);

            checkFormatWarning();

            return true;
        }


        private void cbx_mediaFormat_kilo_CheckedChanged(object sender, EventArgs e)
        {
            calulateSize();
        }


        ulong getFATOverhead(uint clusters)
        {
            ulong overhead = 512; // boot sector is always this size

            uint fatSize = (uint)(nup_fileFormat_fatCopies.Value * clusters);

            overhead += fatSize;

            return overhead;
        }









        private void btn_mediaFormat_newPreset_Click(object sender, EventArgs e)
        {
            setName = "New Preset";

            Rename_Preset rename_PresetDialoge = new Rename_Preset(this);
            rename_PresetDialoge.ShowDialog();

            lbox_mediaFormat_mediaPreset.Items.Add(setName);
            Presets.Add(new MediaFormat((ushort)nup_mediaFormat_bytesPerSector.Value, (ushort)nup_mediaFormat_tracks.Value, (byte)nup_mediaFormat_sectors.Value, (byte)nup_mediaFormat_sides.Value, (byte)nup_mediaFormat_mediaDescriptor.Value,false, setName));
            lbox_mediaFormat_mediaPreset.SelectedIndex = lbox_mediaFormat_mediaPreset.Items.Count - 1;

            // save the new preset (just click the button for saving automatically, lol)
            btn_mediaFormat_savePreset_Click(sender,e);
            
        }

        private void btn_mediaFormat_savePreset_Click(object sender, EventArgs e)
        {
           
            // update internal preset variable
            {
                MediaFormat MF = Presets.ElementAt(lbox_mediaFormat_mediaPreset.SelectedIndex);

                if (MF.buildin) return; // we cant overwrite buildin stuff

                MF.bytesPerSector = (ushort) nup_mediaFormat_bytesPerSector.Value;
                MF.tracksPerSide = (ushort)nup_mediaFormat_tracks.Value;
                MF.sides = (byte)nup_mediaFormat_sides.Value;
                MF.mediaDescriptor = (byte)nup_mediaFormat_mediaDescriptor.Value;
                MF.sectorsPerTrack = (byte)nup_mediaFormat_sectors.Value;

                Presets.RemoveAt(lbox_mediaFormat_mediaPreset.SelectedIndex);
                Presets.Insert(lbox_mediaFormat_mediaPreset.SelectedIndex, MF);
            }

            const byte sizeTillName = 10;
            byte[] data = new byte[sizeTillName + Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].name.Length];

            data[0] = 1;    // major version of this file format
            data[1] = 0;    // minor version of this file format

            // save Bytes per Sector in little endian
            data[2] = (byte)Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].bytesPerSector;
            data[3] = (byte)(Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].bytesPerSector >> 8);
            
            // tracksPerSide in little endian
            data[4] = (byte)Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].tracksPerSide;
            data[5] = (byte)(Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].tracksPerSide >> 8);
            

            data[6] = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sectorsPerTrack;
            data[7] = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].sides;
            data[8] = Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].mediaDescriptor;

            data[9] = (byte)Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].name.Length;

            // save name
            for (int i = sizeTillName; i < data.Length && i - sizeTillName < data[9]; i++)
            {
                data[i] = (byte)Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].name[i - sizeTillName];
            }


            try
            {

                if (!Directory.Exists(@".\Presets\"))
                {
                    Directory.CreateDirectory(@".\Presets\");
                }

                if (Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].path == null)
                {
                    string path = @".\Presets\"+string.Join("_", Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].name.Split(Path.GetInvalidFileNameChars()));
                    string tmp = path;
                    int i = 0;

                    while (File.Exists(tmp + ".cfg"))
                    {
                        tmp = path + i.ToString();
                        i++;
                    }
                    path = tmp + ".cfg";

                    File.WriteAllBytes(path, data);

                    // update path, in case the user wants to overwrite the file
                    MediaFormat MF = Presets.ElementAt(lbox_mediaFormat_mediaPreset.SelectedIndex);
                    MF.path = path;
                    Presets.RemoveAt(lbox_mediaFormat_mediaPreset.SelectedIndex);
                    Presets.Insert(lbox_mediaFormat_mediaPreset.SelectedIndex, MF);

                }
                else // we already have a path
                {
                    File.WriteAllBytes(Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].path, data);
                }
            }
            catch (IOException err)
            {
                Core.output(Core.getStringFormated("configurationfile_errorsave", err.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, Program.console);
            }
        }

        private void btn_mediaFormat_deletePreset_Click(object sender, EventArgs e)
        {
            presetChanged = false;

            if (Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].path != null)
            {
                try
                {
                    File.Delete(Presets[lbox_mediaFormat_mediaPreset.SelectedIndex].path);
                }
                catch (IOException err)
                {
                    Core.output(Core.getStringFormated("configurationfile_errordelete", err.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, Program.console);
                    return;
                }
            }

            int index = lbox_mediaFormat_mediaPreset.SelectedIndex;

            suppressUpdates = true;
            Presets.RemoveAt(lbox_mediaFormat_mediaPreset.SelectedIndex);
            lbox_mediaFormat_mediaPreset.Items.RemoveAt(lbox_mediaFormat_mediaPreset.SelectedIndex);
            suppressUpdates = false;

            if (index >= lbox_mediaFormat_mediaPreset.Items.Count)
                index--;
            lbox_mediaFormat_mediaPreset.SelectedIndex = index;

            oldMediapresetIndex = lbox_mediaFormat_mediaPreset.SelectedIndex;
        }

        private void btn_mediaFormat_renamePreset_Click(object sender, EventArgs e)
        {
            int index = lbox_mediaFormat_mediaPreset.SelectedIndex;

            setName = lbox_mediaFormat_mediaPreset.Items[index].ToString();

            Rename_Preset rename_PresetDialoge = new Rename_Preset(this);
            rename_PresetDialoge.ShowDialog();

            suppressUpdates = true;
            lbox_mediaFormat_mediaPreset.Items.RemoveAt(index); // remove the visual item and replace it with the real item
            lbox_mediaFormat_mediaPreset.Items.Insert(index, setName);
            suppressUpdates = false;

            lbox_mediaFormat_mediaPreset.SelectedIndex = index;

        }




        private void cbx_fileFormat_customSerialNumber_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_fileFormat_customSerialNumber.Checked) {
                if (tbx_fileFormat_customSerialNumber.Text.Equals(""))
                {
                    Random rnd = new Random(); // used for serial number
                    byte[] volumeSerialNumber = new byte[4];
                    rnd.NextBytes(volumeSerialNumber);
                    foreach (byte b in volumeSerialNumber)
                    {
                        tbx_fileFormat_customSerialNumber.Text += b.ToString("X2");
                    }
                }
            }
            else
            {
                tbx_fileFormat_customSerialNumber.Text = "";
            }

            tbx_fileFormat_customSerialNumber.Enabled = cbx_fileFormat_customSerialNumber.Checked;
            settings.CustiomVolumeSerialNumber = cbx_fileFormat_customSerialNumber.Checked;

        }

        private void nup_fileFormat_fatCopies_ValueChanged(object sender, EventArgs e)
        {
            settings.NumberFATcopies = (byte) nup_fileFormat_fatCopies.Value;
            calulateSize(); // we calulate the formated size, so when the format changes, this must update
        }

        private void nup_fileFormat_sectorsPerCluster_ValueChanged(object sender, EventArgs e)
        {
            // check if its a supported value
            switch ((byte)nup_fileFormat_sectorsPerCluster.Value) {
                case 1:
                case 2:
                case 4:
                case 8:
                case 16:
                case 32:
                case 128:
                    pb_sectorsPerClusterWarning.Image = null;
                    tt_sectorsPerClusterWarning.RemoveAll();
                    break;
                default:
                    pb_sectorsPerClusterWarning.Image = warning;
                    tt_sectorsPerClusterWarning.SetToolTip(pb_sectorsPerClusterWarning,Core.getString("sectors_per_cluster_warningtt"));
                    break;
            }

            settings.SectorsPerCluster = (byte) nup_fileFormat_fatCopies.Value;
            calulateSize(); // we calulate the formated size, so when the format changes, this must update
        }



        private void tbx_fileFormat_customSerialNumber_TextChanged(object sender, EventArgs e)
        {
            if (suppressUpdates) return;


            suppressUpdates = true;

            for (byte i = 0; i < tbx_fileFormat_customSerialNumber.Text.Length; i++)
            {
                // if char is [0-9A-F]
                if ((tbx_fileFormat_customSerialNumber.Text[i] >= '0' && tbx_fileFormat_customSerialNumber.Text[i] <= '9') || (tbx_fileFormat_customSerialNumber.Text[i] >= 'A' && tbx_fileFormat_customSerialNumber.Text[i] <= 'F'))
                { }
                else
                {
                    if ((i + 1) < tbx_fileFormat_customSerialNumber.Text.Length)
                    {
                        tbx_fileFormat_customSerialNumber.Text = tbx_fileFormat_customSerialNumber.Text.Substring(0, i) + tbx_fileFormat_customSerialNumber.Text.Substring(i + 1);
                        tbx_fileFormat_customSerialNumber.Select(i, 0);
                    }
                    else
                    {
                        tbx_fileFormat_customSerialNumber.Text = tbx_fileFormat_customSerialNumber.Text.Substring(0, i);
                        tbx_fileFormat_customSerialNumber.Select(i, 0);
                    }
                }
            }

            if (tbx_fileFormat_customSerialNumber.Text.Length == 8)
            {
                // try to save the value in the settings
                int.TryParse(tbx_fileFormat_customSerialNumber.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out settings.VolumeSerialNumber);
            }

            suppressUpdates = false;
        }

        private void tbx_fileFormat_volumeName_TextChanged(object sender, EventArgs e)
        {
            int index = tbx_fileFormat_volumeName.SelectionStart;
            if (suppressUpdates) return;
            tbx_fileFormat_volumeName.Text = tbx_fileFormat_volumeName.Text.ToUpper();

            byte[] data = new byte[11];
            for (int i= 0; i < data.Length; i++)
            {
                if (i >= tbx_fileFormat_volumeName.Text.Length)
                {
                    data[i] = (byte)' ';
                }
                else
                {
                    data[i] = (byte)tbx_fileFormat_volumeName.Text[i];
                }
               
            }
            settings.VolumeLabel = data;
            tbx_fileFormat_volumeName.SelectionStart = index;
        }

        private void nup_fileFormat_numRootEntries_ValueChanged(object sender, EventArgs e)
        {
            if (suppressUpdates) return;

            settings.NumberRootEntries = (ushort)nup_fileFormat_numRootEntries.Value;
            checkFormatWarning();
        }

        private void ImageOptions_Load(object sender, EventArgs e)
        {
            loadLanguage();
        }




        // resize functions

        const int lblDefaultHeight = 13;
        private void lblResizeSteps(int sizeDifference)
        {
            this.Size = new Size(this.Size.Width + sizeDifference, this.Size.Height);                              // rezize window
            this.MinimumSize = new Size(this.MinimumSize.Width + sizeDifference, this.MinimumSize.Height);         // resize minumum size of window

            btn_apply.Location = new Point(btn_apply.Location.X + sizeDifference, btn_apply.Location.Y);

            // resize checkbox text area
            cbx_fileFormat_customSerialNumber.Size = new Size(cbx_fileFormat_customSerialNumber.Size.Width + sizeDifference,       cbx_fileFormat_customSerialNumber.Size.Height);
            cbx_fileFormat_customSerialNumber.MinimumSize = new Size(cbx_fileFormat_customSerialNumber.Size.Width,         cbx_fileFormat_customSerialNumber.Size.Height);

            // resize listbox

            lbox_mediaFormat_mediaPreset.Size = new Size(lbox_mediaFormat_mediaPreset.Size.Width + sizeDifference, lbox_mediaFormat_mediaPreset.Height);

            // for the elements of each container
            foreach (GroupBox group in this.Controls.OfType<GroupBox>())
            {
                // resize group
                group.Size = new Size(group.Size.Width + sizeDifference, group.Size.Height);

                // for all controlls that dont have a tag
                foreach (Control control in group.Controls)
                {
                    if (control.Tag != null) continue;

                    // resize all labels that dont have a tag
                    if (control is Label)
                    {
                        control.MinimumSize = new Size(control.MinimumSize.Width + sizeDifference, lblDefaultHeight);
                        control.Size = new Size(control.MinimumSize.Width, lblDefaultHeight);
                        continue;
                    }
                    else
                    {
                        // Move all other elements that dont have a tag to the right

                        control.Location = new Point(control.Location.X + sizeDifference, control.Location.Y);
                        continue;
                    }

                }

            }


        }

        const int cbxSize = 20;

        private void ctl_resize(Control control)
        {

            int newSize = TextRenderer.MeasureText(control.Text, control.Font).Width;

            if (control is CheckBox) newSize += cbxSize;

            if (newSize <= control.MinimumSize.Width)
            {
                control.Size = new Size(control.MinimumSize.Width, lblDefaultHeight);
            }
            else
            {
                // we need to resize everything, so it keeps looking okayish

                int sizeDifference = newSize - control.MinimumSize.Width;

                // reposition elements
                lblResizeSteps(sizeDifference);
            }
        }





        private void btnResize(int sizeDifference)
        {
            this.Size = new Size(this.Size.Width + sizeDifference, this.Size.Height);                              // rezize window
            this.MinimumSize = new Size(this.MinimumSize.Width + sizeDifference, this.MinimumSize.Height);         // resize minumum size of window

            btn_apply.Location = new Point(btn_apply.Location.X + sizeDifference, btn_apply.Location.Y);

            // resize listbox
            lbox_mediaFormat_mediaPreset.Size = new Size(lbox_mediaFormat_mediaPreset.Size.Width + sizeDifference, lbox_mediaFormat_mediaPreset.Height);

            // resize groups
            grp_fileFormat.Size = new Size(grp_fileFormat.Size.Width + sizeDifference, grp_fileFormat.Size.Height);
            grp_mediaFormat.Size = new Size(grp_mediaFormat.Size.Width + sizeDifference, grp_mediaFormat.Size.Height);
            

            foreach (Control control in grp_mediaFormat.Controls)
            {

                // resize all buttons
                if (control is Button)
                {
                    control.Size = new Size(control.Size.Width + sizeDifference, control.Size.Height);
                    continue;
                }

            }


        }



        private void lbl_fileFormat_fatCopies_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_fileFormat_fatCopies);
        }

        private void lbl_fileFormat_numRootEntries_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_fileFormat_numRootEntries);
        }

        private void lbl_mediaFormat_mediaDescriptor_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_mediaFormat_mediaDescriptor);
        }

        private void lbl_mediaFormat_bytesPerSector_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_mediaFormat_bytesPerSector);
        }

        private void lbl_mediaFormat_tracks_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_mediaFormat_tracks);
        }

        private void lbl_mediaFormat_sectors_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_mediaFormat_sectors);
        }

        private void lbl_mediaFormat_sides_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_mediaFormat_sides);
        }

        private void lbl_fileFormat_sectorsPerCluster_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(lbl_fileFormat_sectorsPerCluster);
        }

        // cbx 1

        private void cbx_fileFormat_customSerialNumber_TextChanged(object sender, EventArgs e)
        {
            ctl_resize(cbx_fileFormat_customSerialNumber);
        }

        // btns

        private const int btnSize = 10;

        private void btn_mediaFormat_deletePreset_TextChanged(object sender, EventArgs e)
        {
            btnResize(btn_mediaFormat_deletePreset);
        }

        private void btn_mediaFormat_savePreset_TextChanged(object sender, EventArgs e)
        {
            btnResize(btn_mediaFormat_savePreset);
        }

        private void btn_mediaFormat_newPreset_TextChanged(object sender, EventArgs e)
        {
            btnResize(btn_mediaFormat_newPreset);
        }

        private void btn_mediaFormat_renamePreset_TextChanged(object sender, EventArgs e)
        {
            btnResize(btn_mediaFormat_renamePreset);
        }

        void btnResize(Button button)
        {
            int TextSize = TextRenderer.MeasureText(button.Text, button.Font).Width;
            if (TextSize > button.Size.Width - btnSize)
            {
                int diff = TextSize - button.Size.Width + btnSize;
                btnResize(diff);
            }
        }

        private void btn_apply_TextChanged(object sender, EventArgs e)
        {
            int TextSize = TextRenderer.MeasureText(btn_apply.Text, btn_apply.Font).Width;
            if (TextSize > btn_apply.Size.Width - btnSize) 
            {
                int diff = TextSize - btn_apply.Size.Width + btnSize;
                btnResize(diff);
                btn_apply.Location = new Point(btn_apply.Location.X - diff, btn_apply.Location.Y);
                btn_apply.Size = new Size(TextSize+ btnSize, btn_apply.Size.Height);
                
            }
        }

        // cbx 2
        private void cbx_mediaFormat_kilo_TextChanged(object sender, EventArgs e)
        {
            int anchor = nup_mediaFormat_mediaDescriptor.Location.X + nup_mediaFormat_mediaDescriptor.Size.Width;
            int newSize = TextRenderer.MeasureText(cbx_mediaFormat_kilo.Text, cbx_mediaFormat_kilo.Font).Width;
            cbx_mediaFormat_kilo.Location = new Point(anchor - newSize - cbxSize + 5, cbx_mediaFormat_kilo.Location.Y); // +5 is a small adjustment to move it more to the right and to make it look like its in line with the anchor
        }



        private void loadLanguage()
        {
            grp_fileFormat.Text = Core.getString("imageformat");
            grp_mediaFormat.Text = Core.getString("mediaformat");

            lbl_fileFormat_fatCopies.Text = Core.getString("number_fatcopies");
            lbl_fileFormat_sectorsPerCluster.Text = Core.getString("sectors_per_cluster");
            lbl_fileFormat_numRootEntries.Text = Core.getString("rootentries");
            lbl_fileFormat_VolumeName.Text = Core.getString("volumename");
            cbx_fileFormat_customSerialNumber.Text = Core.getString("custom_serialnumber");

            lbl_mediaFormat_sides.Text = Core.getString("sides_and_heads");
            lbl_mediaFormat_sectors.Text = Core.getString("sectors_per_track");
            lbl_mediaFormat_tracks.Text = Core.getString("tracks_per_side");
            lbl_mediaFormat_bytesPerSector.Text = Core.getString("bytes_per_sector");
            lbl_mediaFormat_mediaDescriptor.Text = Core.getString("mediadescriptor");

            cbx_mediaFormat_kilo.Text = Core.getString("kilo");

            lbl_mediaFormat_MediaDesc.Text = Core.getString("media_preset");

            btn_apply.Text = Core.getString("apply");
            btn_cancel.Text = Core.getString("cancel");

            btn_mediaFormat_deletePreset.Text = Core.getString("deletepreset");
            btn_mediaFormat_savePreset.Text = Core.getString("savepreset");
            btn_mediaFormat_newPreset.Text = Core.getString("newpreset");
            btn_mediaFormat_renamePreset.Text = Core.getString("renamepreset");
        }

    }
}
