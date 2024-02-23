using System;
using System.IO;
using ImageEditCore;
using System.Windows.Forms;
using System.Collections.Generic;

namespace imgEdit
{

    class Program
    {
        // global variables for the entire progamm
        public const string VERSION_NUMBER = "0.6";
        public const bool console = true;

        public static List<FileAndDir> files;
        public static Settings settings;

        public static System.Windows.Forms.TreeView tre_fileViewer; // never displayed as a form, but we use it so we can copy pasta the code from the non-cmd version

        static int Main(string[] args)
        {
            string workAround = Core.VERSION_NUMBER;
            if (VERSION_NUMBER != workAround)
            {
                /*int coreMajorVer;
                int thisMajorVer;
                if (int.TryParse(Core.VERSION_NUMBER.Split('.')[0],out coreMajorVer))
                    if (int.TryParse(VERSION_NUMBER.Split('.')[0], out thisMajorVer))
                        if (thisMajorVer != coreMajorVer)*/

                            Core.output(Core.getString("version_nomatch"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, console);
            }


            Core.Main(console);

            files = new List<FileAndDir>();
            tre_fileViewer = new System.Windows.Forms.TreeView();

            string[] inputs = null;
            string output = null;
            bool confirm = false;
            byte overwriteStatus = 0; // 0 = ask user 1 = overwrite 2 = no overwrite

            // default settings
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
            }

            // arguments
            {

                if (args.Length == 0) {
                    HelpOutput();
                    return 0;
                }

                for (int i = 0; i < args.Length; i++)
                {
                    string argument = args[i].ToLower();

                    if (argument.Length > 0)
                        if (argument[0] == '\\' || argument[0] == '/' || argument[0] == '-') argument = argument.Remove(0, 1); // remove lading \, / or -


                    switch (argument)
                    {

                        default:
                        case "?":
                        case "help":
                            HelpOutput();
                            return 0;

                        case "e":
                        case "err":

                            if (i + 1 < argument.Length)
                            {
                                i++;
                                int err = 0;
                                if (int.TryParse(argument, out err))
                                    Console.WriteLine(buidinTranslation.builinDefaultLanguage.Find(x => x.Key.Equals(err)).Value);
                                else
                                    Console.WriteLine(Core.getString("cmdargument_errorcode_notfound"));
                            }

                            Console.WriteLine(Core.getString("presskey"));
                            Console.ReadKey();
                            return 0;
                        case "o":
                        case "output":
                            if (args.Length > i + 1)
                            {
                                i++;


                                if (args[i].Length > 0)
                                    if (args[i][0] == '\\' || args[i][0] == '/' || args[i][0] == '-')  // if lading \, / or -
                                    {
                                        Console.WriteLine(Core.getStringFormated("cmdargument_tofew_args", "output"));
                                        i--;
                                        break;
                                    }


                                output = args[i];
                            }
                            else
                            {
                                Console.WriteLine(Core.getStringFormated("cmdargument_tofew_args", "output"));
                            }
                            break;

                        case "i":
                        case "input":
                            if (args.Length > i + 1)
                            {
                                while ((args.Length > i + 1)) {
                                    i++;

                                    if (args[i][0] == '\\' || args[i][0] == '/' || args[i][0] == '-'){  // if lading \, / or -
                                        i--;
                                        break;
                                    }

                                    if (!File.Exists(args[i]) && !Directory.Exists(args[i])) // does not exist
                                    {
                                        args[i] = @"\." + args[i];
                                        if (!File.Exists(args[i]) && !Directory.Exists(args[i])) // does still not exist
                                            continue;
                                    }

                                    if (inputs == null)
                                        inputs = new string[] { args[i] }; // create first element
                                    else
                                    {
                                        {   // realloc
                                            string[] inputs2 = inputs;
                                            inputs = new string[inputs2.Length + 1];
                                            for (int j = 0; j < inputs2.Length; j++)
                                                inputs[j] = inputs2[j];
                                        }

                                        inputs[inputs.Length - 1] = args[i]; // add the new element
                                    }

                                }
                                
                            }
                            else
                            {
                                Console.WriteLine(Core.getStringFormated("cmdargument_tofew_args", "input"));
                            }
                            break;
                        case "c":
                        case "confirm":
                            confirm = true;
                            break;

                        case "w":
                        case "write":
                            overwriteStatus = 1;
                            break;
                        case "nw":
                        case "nowrite":
                            overwriteStatus = 2;
                            break;

                    }
                }
            }



            ///
            ///     prepare stuff to generate image
            ///

            if (inputs != null && output != null)
            {
                if (confirm)
                {
                    string inputFiles = "";
                    foreach (string str in inputs)
                    {
                        if (File.Exists(str))
                            inputFiles += str.ToLower() + ",\n";
                        else if (Directory.Exists(str))
                            inputFiles += str.ToUpper() + ",\n";

                    }
                    

                    Core.output(Core.getStringFormated("outputfile_cmd", output), "", MessageBoxButtons.OK, MessageBoxIcon.None, console);
                    Core.output(Core.getStringFormated("inputfile_cmd", inputFiles), "", MessageBoxButtons.OK, MessageBoxIcon.None, console);

                    if (File.Exists(output) || Directory.Exists(output))
                    {
                        string str = "overwritemsg_cmd_" + overwriteStatus;
                        Core.output(Core.getStringFormated("overwritemsg_cmd", output, Core.getString(str)), "", MessageBoxButtons.OK, MessageBoxIcon.None, console);
                    }

                    DialogResult result = Core.output("\n"+Core.getString("exec_cmd"), "", MessageBoxButtons.YesNo, MessageBoxIcon.None, console);

                    if (result == DialogResult.No)
                        return 0;

                }
            }


            ///
            ///                 add files and folders
            ///

            foreach(string path in inputs)
            {
                if (File.Exists(path))
                {
                    fileAdd(path);
                }else if (Directory.Exists(path))
                {
                    directoryAdd(path);
                }
            }

            ///
            ///                 Prepare to Generate image
            ///

            if (calcSizes().free < 0)
            {
                Core.output(Core.getString("databiggerdisk"), Core.getString("warningmessagebox_title"), MessageBoxButtons.OK, MessageBoxIcon.Warning, console);
                Console.WriteLine(Core.getString("presskey"));
                Console.ReadKey();
                return -1;
            }

            // check if output file has invalid chars
            if (output.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 || output.Length <= 3)
            {
                Core.output(Core.getString("outputfile_invalid"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, console);
                Console.WriteLine(Core.getString("presskey"));
                Console.ReadKey();
                return -1;
            }


            // check if output file already exists
            if (File.Exists(output) || Directory.Exists(output))
            {
                string fileName = output.Split('\\')[output.Split('\\').Length - 1];
                switch (overwriteStatus) {

                    default:
                    case 0:
                    DialogResult result = Core.output(Core.getStringFormated("overwritemsg", fileName), Core.getString("overwritemsg_title"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, console);

                    if (result != DialogResult.Yes)
                    {
                        return -1;
                    }
                break;

                    case 1:
                        Core.output(Core.getStringFormated("overwritemsg_info_1", fileName), "", MessageBoxButtons.OK, MessageBoxIcon.None, console);
                        break;
                    case 2:
                        Core.output(Core.getStringFormated("overwritemsg_info_2", fileName), "", MessageBoxButtons.OK, MessageBoxIcon.None, console);
                        Console.WriteLine(Core.getString("presskey"));
                        Console.ReadKey();
                        return 1;
                }
            }


            // call the image creator, and remove the output file path if it was successfull
            if (ImageGenerator.main(files, output, settings, console))
            {
                Console.WriteLine(Core.getString("presskey"));
                Console.ReadKey();
                return 0;
            }
            Console.WriteLine(Core.getString("presskey"));
            Console.ReadKey();
            return -1;



        }




















        public struct storageSpace
        {
            public long free, used, disk;
            public storageSpace(long free, long used, long disk)
            {
                this.free = free;
                this.used = used;
                this.disk = disk;
            }
        }
        public static storageSpace calcSizes()
        {
            ImageGenerator.DiskRequirements diskReq = ImageGenerator.contentFitsInsideDisk(files, settings, Program.console);


            return new storageSpace((long)(diskReq.availableDiskSize - diskReq.neededDiskSize), (long)diskReq.neededDiskSize, (long)diskReq.availableDiskSize);
        }






        private static void fileAdd(string file)
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
                Core.output(Core.getStringFormated("ioexcept_sizecalc", err.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, console);
            }

            FileAndDir fdir = new FileAndDir(false, null, file, fileSize);
            if (!files.Contains(fdir))
            {
                addNodes(new FileAndDir[] { fdir }, null);
            }
        }




        private static void directoryAdd(string directory)
        {

            FileAndDir fdir = Core.createFAD(directory,settings,console);
            if (!files.Contains(fdir))
            {
                addNodes(new FileAndDir[] { fdir }, null);
            }
            
        }


        private static void addNodes(FileAndDir[] paths, TreeNode parrent)
        {
            //foreach (FileAndDir file in files)
            for (int i = 0; i < paths.Length; i++)
            {
                FileAndDir file = paths[i];

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
                paths[i] = file;   // save the new file

                if (parrent == null) // only when the recursion collapsed.
                {
                    files.Add(file); // add the new file to the global file database
                }
                
            }
        }









        private static void HelpOutput()
        {
            Console.WriteLine(Core.getString("cmdcommands") + '\n' +
                              Core.getStringFormated("cmdargument", "help", Core.getString("cmdargument_help")) + '\n' +
                              Core.getStringFormated("cmdargument2", "err", Core.getString("cmdparam_errorcode"), Core.getString("cmdargument_errorcode")) + '\n' +
                              Core.getStringFormated("cmdargument2", "input", Core.getString("cmdparam_input"), Core.getString("cmdargument_input")) + '\n' +
                              Core.getStringFormated("cmdargument2", "output", Core.getString("cmdparam_output"), Core.getString("cmdargument_output")) + '\n' +
                              Core.getStringFormated("cmdargument", "confirm", Core.getString("cmdargument_confirm")) + '\n'+
                              Core.getStringFormated("cmdargument", "write", Core.getString("cmdargument_write")) + '\n' +
                              Core.getStringFormated("cmdargument", "nowrite", Core.getString("cmdargument_nowrite")) + '\n');

            Console.WriteLine(Core.getString("presskey"));
            Console.ReadKey();
            return;
        }
    }
}
