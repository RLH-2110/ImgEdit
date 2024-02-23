using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageEditCore;

namespace WFA_ImgEdit
{

    

    public partial class form_main : Form
    {

        List<FileAndDir> files;

        public form_main()
        {
            InitializeComponent();
            files = new List<FileAndDir>();
        }

        // settings
        public Settings settings;

        private void btn_selFolder_Click(object sender, EventArgs e)
        {
            if (fbd_selectFolder.ShowDialog() == DialogResult.OK)
            {
                FileAndDir folder = Core.createFAD(fbd_selectFolder.SelectedPath,settings,Program.console);
                if (!files.Contains(folder))
                {
                    addNodes(new FileAndDir[] {folder},null);
                }
            }
        }

        

        private void btn_selInputFile_Click(object sender, EventArgs e)
        {
            ofd_selectInput.ShowDialog();
        }

        private void btn_selOutputFile_Click(object sender, EventArgs e)
        {
            sfd_selectOutput.ShowDialog();
        }









    private void btn_removeElement_Click(object sender, EventArgs e)
        {
            if (tre_fileViewer.SelectedNode == null) return;

            
            FileAndDir[] result = FindAndRemoveElement(files.ToArray());

            if (!result.Equals(Core.nullFADArray))
            {
                files = new List<FileAndDir>(result);

                tre_fileViewer.Nodes.Remove(tre_fileViewer.SelectedNode);

                calcSizes();
                update_lbl_FreeSpace_Position();
            }

        }

        private FileAndDir[] FindAndRemoveElement(FileAndDir[] fdirs)
        {
            ulong removedFileSize;
            bool updateParrentNode;

            return FindAndRemoveElement(fdirs, out removedFileSize, out updateParrentNode);
        }

        private FileAndDir[] FindAndRemoveElement(FileAndDir[] fdirs, out ulong removedFileSize, out bool updateParrentNode)
        {
            for (uint i = 0; i < fdirs.Length; i++) // for every fdir
            {
                if (fdirs[i].ID.Equals(tre_fileViewer.SelectedNode)) // if we found the node we are looking for
                {
                    removedFileSize = fdirs[i].fileSize;
                    updateParrentNode = true;
                    return Core.removeElementFromFdir(fdirs, i);

                }
                else // if this node is not what we where looking for
                {
                    if (fdirs[i].children == null) // if we have no children, go to the next iteration of the for loop
                        continue;

   
                    FileAndDir[] result = FindAndRemoveElement(fdirs[i].children, out removedFileSize, out updateParrentNode);

                    if (result.Equals(Core.nullFADArray)) // if we  did not get a result, go to the next iteration of the for loop
                        continue;

                    fdirs[i].fileSize -= removedFileSize;
                    fdirs[i].children = result; // apply the changes that where made to the children

                    if (updateParrentNode)
                        updateTreeNode(fdirs[i].ID, fdirs[i],tre_fileViewer.Nodes);

                    return fdirs;


                }
            }


            // nothing was found
            removedFileSize = 0;
            updateParrentNode = false;
            return Core.nullFADArray;
        }


        private TreeNodeCollection updateTreeNode(TreeNode toUpdate, FileAndDir fdir, TreeNodeCollection search)
        {

            foreach (TreeNode node in search)
            {
                if (node.Equals(toUpdate))
                {
                    node.Text = Core.getNodeName(fdir.path, fdir.fileSize); // modify TreeNodeCollection 
                    fdir.ID = node;

                    return search;
                }
                else
                {
                    if (node.Nodes.Count == 0)
                        continue;

                    TreeNodeCollection result = updateTreeNode(toUpdate, fdir, node.Nodes);
                    if (result != null)// if we managed to update a node
                    {
                        TreeNode backup = tre_fileViewer.SelectedNode;

                        setTreeNodeCollection(node.Nodes,result); // carry the changes

                        tre_fileViewer.SelectedNode = backup;
                        return search;
                    }
                }
            }

            return null;
        }

        private void setTreeNodeCollection(TreeNodeCollection desination, TreeNodeCollection source) //!! MAKE A  BACKUO OF THE SELECTED NODE FIRST !!
        {

            List<TreeNode> cloneSource = new List<TreeNode>();
            foreach (TreeNode node in source)
                cloneSource.Add(node);


           

            desination.Clear();
            foreach (TreeNode node in cloneSource)
            {
                desination.Add(node);
            }

            
        }




        private void btm_createImage_Click(object sender, EventArgs e)
        {
            if (calcSizes() < 0)
            {
                Core.output(Core.getString("databiggerdisk"), Core.getString("warningmessagebox_title"), MessageBoxButtons.OK, MessageBoxIcon.Warning, Program.console);
                return;
            }

            // check if output file has invalid chars
            if (tbx_outputFile.Text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 || tbx_outputFile.Text.Length <= 3)
            {
                Core.output(Core.getString("outputfile_invalid"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Program.console);
                return;
            }


            // check if output file already exists
            if (File.Exists(tbx_outputFile.Text) || Directory.Exists(tbx_outputFile.Text))
            {
                string fileName = tbx_outputFile.Text.Split('\\')[tbx_outputFile.Text.Split('\\').Length - 1];
                DialogResult result = Core.output(Core.getStringFormated("overwritemsg",fileName),Core.getString("overwritemsg_title"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Program.console);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            // call the image creator, and remove the output file path if it was successfull
            if (ImageGenerator.main(files, tbx_outputFile.Text, settings,Program.console))
            {
                tbx_outputFile.Text = "";
            }
        }


        private void sfd_selectOutput_FileOk(object sender, CancelEventArgs e)
        {
            tbx_outputFile.Text = sfd_selectOutput.FileName;
        }



        private void ofd_selectInput_FileOk(object sender, CancelEventArgs e)
        {

            foreach (string file in ofd_selectInput.FileNames)
            {
                fileAdd(file);
            }
        }

        private void fileAdd(string file)
        {
            uint bytesPerCluser = (uint)settings.BytesPerSector * settings.SectorsPerCluster;
            ulong fileSize = 0;

            try
            {
                // get filesize on disk
                fileSize = (uint)Math.Ceiling(new FileInfo(file).Length / (double)bytesPerCluser) * bytesPerCluser;
            }
            catch (IOException err)
            {
                Core.output(Core.getStringFormated("ioexcept_sizecalc", err.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,Program.console);
            }

            FileAndDir fdir = new FileAndDir(false, null, file, fileSize);
            if (!files.Contains(fdir))
            {
                //files.Add(addNodes(new FileAndDir[] { fdir }, null)[0]);
                addNodes(new FileAndDir[] { fdir }, null);
            }
        }

        private void addNodes(FileAndDir[] files, TreeNode parrent)
        {
            //foreach (FileAndDir file in files)
            for (int i = 0; i < files.Length; i++)
            {
                FileAndDir file = files[i];

                string fileName = file.path.Split('\\')[file.path.Split('\\').Length - 1];
                TreeNode newNode = new TreeNode(Core.getStringFormated("filelist", fileName, Core.DisplayInUnits(file.fileSize, Core.kilo).number, Core.DisplayInUnits(file.fileSize, Core.kilo).text));
                
                if (parrent != null)
                    parrent.Nodes.Add(newNode);
                else
                    tre_fileViewer.Nodes.Add(newNode);
                


                if (file.children != null)
                {
                    addNodes(file.children, newNode);
                }

                file.ID = newNode; // save the node in the file, so we can find the file via the node
                files[i] = file;   // save the new file

                if (parrent == null) // only when the recursion collapsed.
                {
                    this.files.Add(file); // add the new file to the global file database
                    calcSizes();
                    update_lbl_FreeSpace_Position();
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.output(    Core.getStringFormated("about_infotext", Program.VERSION_NUMBER)      , Core.getString("about_infotext_title"), MessageBoxButtons.OK,MessageBoxIcon.None, Program.console);
        }

        private void btn_ImgOptions_Click(object sender, EventArgs e)
        {
            ImageOptions options = new ImageOptions(this);
            options.ShowDialog();
            calcSizes();
        }

        private void form_main_Load(object sender, EventArgs e)
        {
            settings.BytesPerSector = 0x0200; // 512
            settings.CustiomVolumeSerialNumber = false;
            settings.FAT16 = false;
            settings.MediaDescriptor = 0xF0; settings.DiskTypeUI = 2;
            settings.NumberFATcopies = 0x2;
            settings.SectorsPerCluster = 1;
            settings.VolumeLabel = new byte[] { 0x4E, 0X4F, 0X20, 0X4E, 0X41, 0X4D, 0X45, 0x20, 0x20, 0x20, 0x20 }; // 11 byte string "NO NAME    "
            settings.NumberOfSectors = 2880;

            settings.NumberRootEntries = 224;

            settings.DiskTypeUI = 1;
            settings.Sides = 2;
            settings.SectorsPerTrack = 18;
            settings.TracksPerSide = 80;

            calcSizes();
            update_lbl_FreeSpace_Position();



            // load language:
            loadLanguage();
        }

        private void form_main_Resize(object sender, EventArgs e)
        {
            update_lbl_FreeSpace_Position();
        }

        private void update_lbl_FreeSpace_Position()
        {
            lbl_freeSpace.Location = new Point(this.Size.Width / 2 - lbl_freeSpace.Size.Width / 2, lbl_freeSpace.Location.Y);    // position in the middle of the window
        }



        long calcSizes()
        {
            ImageGenerator.DiskRequirements diskReq = ImageGenerator.contentFitsInsideDisk(files, settings,Program.console);

            Core.DisplayInUnits_return textInserts;

            textInserts = Core.DisplayInUnits(diskReq.neededDiskSize, Core.kilo);
            lbl_usedSpace.Text = Core.getStringFormated("used", textInserts.number, textInserts.text);

            textInserts = Core.DisplayInUnits(diskReq.availableDiskSize, Core.kilo);
            lbl_availabeSpace.Text = Core.getStringFormated("disk", textInserts.number, textInserts.text);

            long free = (long)(diskReq.availableDiskSize - diskReq.neededDiskSize);
            if (free < 0)
                lbl_freeSpace.ForeColor = Color.DarkRed;
            else
                lbl_freeSpace.ForeColor = Color.Black;

            textInserts = Core.DisplayInUnits(free, Core.kilo);
            lbl_freeSpace.Text = Core.getStringFormated("free", textInserts.number, textInserts.text);

            return free;
        }














        // resize handleling

        private void commonResizeSteps(int sizeDifference)
        {
            this.Size = new Size(this.Size.Width + sizeDifference, this.Size.Height);                              // rezize window
            this.MinimumSize = new Size(this.MinimumSize.Width + sizeDifference, this.MinimumSize.Height);       // resize minumum size of window

            tre_fileViewer.Size = new Size(tre_fileViewer.Size.Width + sizeDifference, tre_fileViewer.Size.Height);// resize treeViewer, its scaled with the window


            tbx_outputFile.Size = new Size(tbx_outputFile.Size.Width + sizeDifference, tbx_outputFile.Height);   // scale outputFile textfield with window
            btn_selOutputFile.Location = new Point(btn_selOutputFile.Location.X + sizeDifference, tbx_outputFile.Location.Y); // reposition the selection buttion

            btn_createImage.Location = new Point(this.Size.Width / 2 - btn_createImage.Size.Width / 2, btn_createImage.Location.Y);    // position ind the middle of the window

        }

        // buttons

        private const int standardTopBtnSize = 81;

        private void btn_selFiles_Resize(object sender, EventArgs e)
        {
            btn_selFolder.Location = new Point(btn_selFiles.Location.X + btn_selFiles.Size.Width + Core.buttionDistance, btn_selFolder.Location.Y);
            btn_removeElement.Location = new Point(btn_selFolder.Location.X + btn_selFolder.Width + Core.buttionDistance, btn_removeElement.Location.Y);
            btn_ImgOptions.Location = new Point(btn_removeElement.Location.X + btn_removeElement.Width + Core.buttionDistance, btn_ImgOptions.Location.Y);
            commonResizeSteps(btn_selFiles.Size.Width - standardTopBtnSize);
        }

        private void btn_selFolder_Resize(object sender, EventArgs e)
        {
            btn_removeElement.Location = new Point(btn_selFolder.Location.X + btn_selFolder.Width + Core.buttionDistance, btn_removeElement.Location.Y);
            btn_ImgOptions.Location = new Point(btn_removeElement.Location.X + btn_removeElement.Width + Core.buttionDistance, btn_ImgOptions.Location.Y);
            commonResizeSteps(btn_selFolder.Size.Width - standardTopBtnSize);

        }

        private void btn_removeElement_Resize(object sender, EventArgs e)
        {
            btn_ImgOptions.Location = new Point(btn_removeElement.Location.X + btn_removeElement.Width + Core.buttionDistance, btn_ImgOptions.Location.Y);
            commonResizeSteps(btn_removeElement.Size.Width - standardTopBtnSize);
        }

        private void btn_ImgOptions_Resize(object sender, EventArgs e)
        {
            commonResizeSteps(btn_ImgOptions.Size.Width - standardTopBtnSize);
        }



        // labels


        public const int lblDefaultHeight = 13;

        private void lbl_outputFile_TextChanged(object sender, EventArgs e)
        {
            lbl_resize(lbl_outputFile, LblID.outputfile);
        }


        enum LblID
        {
            outputfile,
        }

        private void lbl_resize(Label label, LblID lblID)
        {

            int newSize = TextRenderer.MeasureText(label.Text, label.Font).Width;


            if (newSize <= label.MinimumSize.Width)
            {
                label.Size = new Size(label.MinimumSize.Width, lblDefaultHeight);
            }
            else
            {
                // we need to resize everything, so it keeps looking okayish

                int sizeDifference = newSize - label.MinimumSize.Width;

                label.MinimumSize = new Size(newSize, lblDefaultHeight);
                label.Size = new Size(newSize, lblDefaultHeight);


                // reposition elements
                commonResizeSteps(sizeDifference);

                switch (lblID)
                {
                    case LblID.outputfile:
                        tbx_outputFile.Location = new Point(tbx_outputFile.Location.X + sizeDifference, tbx_outputFile.Location.Y);
                        tbx_outputFile.Size = new Size(tbx_outputFile.Size.Width - sizeDifference, tbx_outputFile.Size.Height);

                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
        }




        //
        // menu strip
        //

        private void imageOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_ImgOptions_Click(sender, e); // do the same as the buttion
        }



        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            languageSelect languageSelect = new languageSelect();
            languageSelect.ShowDialog();
        }


        private void loadLanguage()
        {
            btn_selFiles.Text = Core.getString("addfiles");
            btn_selFolder.Text = Core.getString("addfolder");
            btn_removeElement.Text = Core.getString("remove");
            btn_ImgOptions.Text = Core.getString("imageoptions");
            btn_createImage.Text = Core.getString("createimage");
            btn_selOutputFile.Text = Core.getString("...");

            lbl_outputFile.Text = Core.getString("outputfile");


            menuStrip.Items[(int)menuStipItems.options].Text = Core.getString("options");
            menuStrip.Items[(int)menuStipItems.help].Text = Core.getString("help");

            menuStipL2_changeName(menuStipItems.help, (uint)menuStipItems_help.about, Core.getString("about"));

            menuStipL2_changeName(menuStipItems.options, (uint)menuStipItems_options.imageOptions, Core.getString("imageoptions_menuestrip"));
            menuStipL2_changeName(menuStipItems.options, (uint)menuStipItems_options.language, Core.getString("language"));


            ofd_selectInput.Title = Core.getString("selectinput");
            sfd_selectOutput.Title = Core.getString("selectoutputfile");
        }

        private void menuStipL2_changeName(menuStipItems layer1, uint layer2, string text)
        {
            if ((uint)layer1 >= menuStrip.Items.Count) return;

            ToolStripMenuItem helpMenuItem = (ToolStripMenuItem)menuStrip.Items[(int)layer1];

            if (layer2 >= helpMenuItem.DropDownItems.Count) return;

            ToolStripItem aboutMenuItem = helpMenuItem.DropDownItems[(int)layer2];
            aboutMenuItem.Text = text;
        }

        enum menuStipItems
        {
            options,
            help,
        };

        enum menuStipItems_help
        {
            about,
        };

        enum menuStipItems_options
        {
            imageOptions,
            language,
        };

        private void tre_fileViewer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tre_fileViewer_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.FileDrop, false);
            if (data is string[]) {
                string[] files = (string[])data;
                foreach (string file in files)
                {
                    if (File.Exists(file)) // add file
                    {
                        fileAdd(file);
                    }


                    else if (Directory.Exists(file)) // add folder
                    {
                        FileAndDir folder = Core.createFAD(file,settings, Program.console);
                        if (!this.files.Contains(folder))
                        {
                            addNodes(new FileAndDir[] { folder }, null);
                        }
                    }
                }
                    
                   
            }
            else
            {
                Core.output(Core.getStringFormated("unexpectedinternal", 100), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, Program.console);
            }
        }

        private void tbx_outputFile_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.FileDrop, false);
            if (data is string[])
            {
                string[] files = (string[])data;
                foreach (string file in files)
                {
                    if (File.Exists(file)) // so we dont accidentaly overwrite a folder
                    {
                        tbx_outputFile.Text = file;
                    }
                    

                }


            }
            else
            {
                Core.output(Core.getStringFormated("unexpectedinternal", 101), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, Program.console);
            }
        }

        private void tbx_outputFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
    }



}


