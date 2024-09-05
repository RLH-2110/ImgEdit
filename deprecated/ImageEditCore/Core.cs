using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageEditCore
{

    public struct MediaFormat
    {
        public ushort bytesPerSector;
        public ushort tracksPerSide;
        public byte sectorsPerTrack;
        public byte sides;
        public byte mediaDescriptor;
        public bool buildin;
        public string path;
        public string name;
        public MediaFormat(ushort bytesPerSector, ushort tracksPerSide, byte sectorsPerTrack, byte sides, byte mediaDescriptor, bool buildin, string name)
        {
            this.bytesPerSector = bytesPerSector;
            this.tracksPerSide = tracksPerSide;
            this.sectorsPerTrack = sectorsPerTrack;
            this.sides = sides;
            this.mediaDescriptor = mediaDescriptor;
            this.buildin = buildin;
            path = null;
            this.name = name;
        }

    }
    public struct Settings
    {
        public bool FAT16;
        public ushort BytesPerSector;
        public byte SectorsPerCluster;
        public byte NumberFATcopies;
        public byte MediaDescriptor;
        public byte[] VolumeLabel;
        public uint NumberOfSectors;

        public bool CustiomVolumeSerialNumber;
        public int VolumeSerialNumber;

        public int DiskTypeUI;
        public byte Sides;
        public byte SectorsPerTrack;
        public ushort TracksPerSide;

        public ushort NumberRootEntries;
    }


    public struct FileAndDir
    {
        public bool isDir;
        public FileAndDir[] children;
        public string path;
        public TreeNode ID; // this is to link the treeview to this stucture, we save the node (which is uniqe) for this file, and when we deleate the node, we can look though here to find the node and deleate this too.
        public ulong fileSize;
        public FileAndDir(bool isDir, FileAndDir[] children, string path, ulong fileSize)
        {
            this.isDir = isDir;
            this.children = children;
            this.path = path;
            this.fileSize = fileSize;
            ID = null;
        }
        public FileAndDir(bool isDir, FileAndDir[] children, string path, ulong fileSize, TreeNode ID)
        {
            this.isDir = isDir;
            this.children = children;
            this.path = path;
            this.fileSize = fileSize;
            this.ID = ID;
        }
        public FileAndDir(bool console) // this is for the case when I want to create a file and dir that has invalid data, that I can easily check against
        {
            isDir = false;
            children = null;
            path = null;
            ID = null;
            fileSize = 0;
        }
    }

    public static class Core
    {
        // global variables for the entire progamm
        public const string VERSION_NUMBER = "0.6";

        public static string[,] Sizes = { { "Byte", "KiB", "MiB", "GiB" }, { "Byte", "KB", "MB", "GB" } };
        public static string language = "en-gb";
        public const int buttionDistance = 5;
        public static string configFile = ".\\" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".cfg"; // the config file should have the same name as the application
        public static UnsavedChangesStatus presetUnsavedChangesStatus = UnsavedChangesStatus.ownPresetWarning;
        public static bool kilo = false;
        public static string consoleVerPath = @".\imged.exe";

        public static FileAndDir[] nullFADArray;

        // language stuff
        public static string languageFolder = ".\\langs\\";
        public const string languageFileExt = ".lang";
        public const string languageBuildIn = "buildin";
        public static string languageSource = languageBuildIn;


        enum Configurations
        {
            language,
            presetUnsavedChangesWarning,
            kilo,
            console,
            languagePath,
        }
        private static readonly int configurationsCount = Enum.GetNames(typeof(Configurations)).Length;

        public enum UnsavedChangesStatus
        {
            noWarning,
            ownPresetWarning,
            allPresetWarning,
        }


        public struct DisplayInUnits_return
        {
            public float number;
            public string text;
            public DisplayInUnits_return(float number, string text)
            {
                this.number = number;
                this.text = text;
            }
        }
        public static DisplayInUnits_return DisplayInUnits(decimal value, bool kilo)
        {

            byte i = 0;
            bool neg = value < 0 ? true : false; // set if value is negative
            if (neg) value = 0 - value; // make it positive (for the algorigm, we invert it back later) 

            ushort divider = 1024;
            if (kilo) divider = 1000;

            while (value > 999)
            {
                i++;
                if (i >= Sizes.GetLength(1))
                {
                    i--;
                    break;
                }
                value /= divider;
            }


            if (neg) value = 0 - value; // make it negative again, if it was negative at the start

            return new DisplayInUnits_return((float)Math.Round(value, 2), Sizes[boolAsIndex(kilo), i]);
        }
        public static byte boolAsIndex(bool val)
        {
            if (val)
                return 1;
            return 0;
        }


        public static string getString(string Key)
        {
            string result;

            if (!languageSource.Equals(languageBuildIn)) // if we dont use buildin
            {
                result = externalLanguage.Find(x => x.Key.Equals(Key)).Value;
                if (result == null)
                {
                    // use buildin if the language does not translate the string
                    result = buidinTranslation.builinDefaultLanguage.Find(x => x.Key.Equals(Key)).Value;
                    if (result == null) result = Key;
                }
            }
            else
            {
                result = buidinTranslation.builinDefaultLanguage.Find(x => x.Key.Equals(Key)).Value;
                if (result == null) result = Key;
            }



            return result;
        }

        public static string getStringFormated(string Key, params object[] input)
        {
            uint expectedParameters = (uint)input.Length;

            string result = getString(Key);

            bool open = false;
            string content = "";

            // check if the result does accsess out of bounds using string.format
            for (int i = 0; i < result.Length; i++) // for every char
            {
                switch (result[i])
                {
                    case '{':   // new braket was opened
                        if (open)
                        {
                            // we already have a open braket
                            result = "Text format error in " + Key;
                            return result;
                        }
                        else
                        {
                            // save that we have a new open braked
                            open = true;
                        }
                        break;


                    case '}':
                        open = false; // we close the open braket

                        uint textIndex; // string.format id, like the 1 in {1}

                        if (uint.TryParse(content, out textIndex)) // parse it to uint if possible
                        {


                            if (textIndex >= expectedParameters) //if it excedes the argument count!
                            {
                                result = "Text format error in " + Key;
                                return result;
                            }
                            // if it is in the argument count, do nothing

                        }
                        else // it cant be parsed into an uint
                        {
                            result = "Text format error in " + Key;
                            return result;
                        }

                        content = ""; // reset content to nothing
                        break;

                    default: // any other char
                        if (open)
                            content += result[i]; // note it down, if we are in an open braket
                        break;

                }

            }

            if (open) // if the last braked was never closed
            {
                result = "Text format error in " + Key;
                return result;
            }

            return string.Format(result, input);
        }

        public static void getLanguageSource(bool console)
        {


            try
            {

                if (!Directory.Exists(languageFolder)) // if the language folder does not exist, create it.
                    Directory.CreateDirectory(languageFolder);


                if (File.Exists(languageFolder + language + languageFileExt)) // if we find a language file that is an exact match
                {
                    languageSource = language + languageFileExt;

                    // load ze file into externalLanguage
                    loadLangIntoExtlang(console);


                }

                else
                {
                    string[] files = Directory.GetFiles(languageFolder); // search all files
                    foreach (string fileR in files)
                    {
                        string file = fileR.Substring(languageFolder.Length); // filter out the folder path

                        if (file.Length != 5 + languageFileExt.Length) continue; // if the file does not follow the nameing format, then skip it.
                        if (file[2] != '-') continue; // if the file does not follow the nameing format, then skip it.

                        if (file.Substring(0, 2).Equals(language.Substring(0, 2)))  // did we find a match for the language?     (codes are language-region, both are 2 characters)
                        {
                            languageSource = file.Substring(0, 2);

                            // load ze file into externalLanguage
                            loadLangIntoExtlang(console);
                        }
                    }


                    if (languageSource == null) // if we did not find a match.
                    {
                        languageSource = languageBuildIn;
                    }
                }
            }
            catch (IOException)
            {
                languageSource = languageBuildIn;
            }
        }


        private static void loadLangIntoExtlang(bool console)
        {
            string[] lines;
            try
            {
                lines = readAllLines(languageFolder + languageSource, 25 * 1024 * 1024,console);
                if (lines == null) return;
            }
            catch (IOException)
            {
                output("Error while reading language file! falling back to buildin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                return;
            }

            foreach (string lineRaw in lines)
            {

                // ingore empty lines, or lines with comments
                if (lineRaw.Length == 0) continue;
                switch (lineRaw[0])
                {
                    case '#':
                    case '\n':
                    case '\r':
                        continue;
                }

                string line = lineRaw.Split('#')[0]; // remove comments)
                line = line.Trim();

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == ':')
                    {
                        if (i + 1 >= line.Length) break; // invalid line

                        string key = line.Substring(0, i).ToLower();
                        string value = line.Substring(i + 1, line.Length - (i + 1));

                        value = value.Replace("\\n", Environment.NewLine);
                        value = value.Replace("\\t", "\t");

                        externalLanguage.Add(new KeyValuePair<string, string>(key, value));
                        break;
                    }
                }
            }
        }

        static List<KeyValuePair<string, string>> externalLanguage = new List<KeyValuePair<string, string>>();

        public static string getNodeName(string path, ulong fileSize) // returns the name of a node giving a file path and filesize
        {
            string fileName = path.Split('\\')[path.Split('\\').Length - 1];
            return getStringFormated("filelist", fileName, Core.DisplayInUnits(fileSize, Core.kilo).number, Core.DisplayInUnits(fileSize, kilo).text);
        }
        static public FileAndDir[] removeElementFromFdir(FileAndDir[] fdir, uint index) // returns an FileAndDir[] that does not contain the element at index.
        {
            if (index >= fdir.Length) return fdir;  // no element to remove

            for (uint i = index + 1; i < fdir.Length; i++)  // move each element one back;
                fdir[i - 1] = fdir[i];

            FileAndDir[] newFdir = new FileAndDir[fdir.Length - 1]; // create a smaller fdir
            for (int i = 0; i < newFdir.Length; i++)    // copy everything over, except the last element
                newFdir[i] = fdir[i];

            return newFdir; // return the new fdir array, without the removed element
        }


        public static string[] readAllLines(string path, ulong filesizeLimit,bool console)
        {
            if (!File.Exists(path)) return null;
            if ((ulong)new FileInfo(path).Length > filesizeLimit)
            {
                DisplayInUnits_return display = DisplayInUnits(filesizeLimit, kilo);
                output(getStringFormated("filesizelimitexceeded", path, display.number, display.text), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                return null;
            }
            return File.ReadAllLines(path); 
        }

        public static byte[] readAllBytes(string path, ulong filesizeLimit, bool console)
        {
            if (!File.Exists(path)) return null;
            if ((ulong)new FileInfo(path).Length > filesizeLimit)
            {
                DisplayInUnits_return display = DisplayInUnits(filesizeLimit, kilo);
                output(getStringFormated("filesizelimitexceeded", path, display.number, display.text), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, console);
                return null;
            }
            return File.ReadAllBytes(path);
        }

        public static DialogResult output(string text, string title, MessageBoxButtons btns,MessageBoxIcon icon, bool console)
        {
            if (console)
            {
                Console.WriteLine(text);

                // get DialogeResult from user
                ConsoleKeyInfo key;
                switch (btns)
                {
                    case MessageBoxButtons.AbortRetryIgnore:
                        Console.Write(getString("abortretryignore"));
                        do
                        {
                            key = Console.ReadKey();
                        } while (key.Key != ConsoleKey.A && key.Key != ConsoleKey.R && key.Key != ConsoleKey.I);

                        Console.Write("\n");
                        switch (key.Key)
                        {
                            case ConsoleKey.A: return DialogResult.Abort;
                            case ConsoleKey.R: return DialogResult.Retry;
                            case ConsoleKey.I: return DialogResult.Ignore;
                            default: return DialogResult.Abort;
                        }

                    default:
                    case MessageBoxButtons.OK: return DialogResult.OK;

                    case MessageBoxButtons.OKCancel:
                        Console.Write(getString("okcancel"));
                        do
                        {
                            key = Console.ReadKey();
                        } while (key.Key != ConsoleKey.O && key.Key != ConsoleKey.C);

                        Console.Write("\n");
                        switch (key.Key)
                        {
                            case ConsoleKey.O: return DialogResult.OK;
                            case ConsoleKey.C: return DialogResult.Cancel;
                            default: return DialogResult.Cancel;
                        }
                    case MessageBoxButtons.RetryCancel:
                        Console.Write(getString("retrycancel"));
                        do
                        {
                            key = Console.ReadKey();
                        } while (key.Key != ConsoleKey.R && key.Key != ConsoleKey.C);

                        Console.Write("\n");
                        switch (key.Key)
                        {
                            case ConsoleKey.R: return DialogResult.Retry;
                            case ConsoleKey.C: return DialogResult.Cancel;
                            default: return DialogResult.Cancel;
                        }
                    case MessageBoxButtons.YesNo:
                        Console.Write(getString("yesno"));
                        do
                        {
                            key = Console.ReadKey();
                        } while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N);

                        Console.Write("\n");
                        switch (key.Key)
                        {
                            case ConsoleKey.Y: return DialogResult.Yes;
                            case ConsoleKey.N: return DialogResult.No;
                            default: return DialogResult.No;
                        }
                    case MessageBoxButtons.YesNoCancel:
                        Console.Write(getString("yesnocancel"));
                        do
                        {
                            key = Console.ReadKey();
                        } while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N && key.Key != ConsoleKey.C);

                        Console.Write("\n");
                        switch (key.Key)
                        {
                            case ConsoleKey.Y: return DialogResult.Yes;
                            case ConsoleKey.N: return DialogResult.No;
                            case ConsoleKey.C: return DialogResult.Cancel;
                            default: return DialogResult.Cancel;
                        }

                }
            }
            else
            {
                return MessageBox.Show(text, title, btns, icon);
            }
        }


        const byte presetMajorVer = 1;
        const byte presetMinorVer = 0;
        private static MediaFormat readMediaFromFile(string path, bool console)
        {
            MediaFormat mediaFromat = new MediaFormat();

            byte[] data;

            try
            {
                data = readAllBytes(path,10240,console);
            }
            catch (IOException)
            {
                return mediaFromat; // return an invalid media format (it will be checked against)
            }

            if (data[0] > presetMajorVer) // check for version (first byte is mayor version. second byte is minor version, minor versions are backwards compatible. so 1.99 can be read by 1.0 but 2.0 can NOT be read by 1.0
            {
                output(getStringFormated("configurationfile_notsupported", presetMajorVer + "." + presetMinorVer, data[0] + "." + data[1]), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning, console);
                return mediaFromat; // return an invalid media format (it will be checked against)
            }

            if (data.Length < 11)   // data should be at least 11 bytes
            {
                return mediaFromat; // return an invalid media format (it will be checked against)
            }
            if (data[9] == 0)   // name can not be size 0
            {
                return mediaFromat; // return an invalid media format (it will be checked against)
            }

            mediaFromat.bytesPerSector = (ushort)(data[2] + (data[3] << 8));
            mediaFromat.tracksPerSide = (ushort)(data[4] + (data[5] << 8));
            mediaFromat.sectorsPerTrack = data[6];
            mediaFromat.sides = data[7];
            mediaFromat.mediaDescriptor = data[8];
            mediaFromat.name = "";
            for (int i = 0xA; i < 0xA + data[9]; i++)
            {
                if (i >= data.Length)
                {
                    DialogResult result = output(getStringFormated("configurationfile_corrupted", path), getString("warningmessagebox_title"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, console);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (IOException)
                        {
                            output(getString("configurationfile_corrupted_unabletodelete"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, console);
                        }
                    }
                    return new MediaFormat(); // return invalid media format that will be checked against
                }
                mediaFromat.name += (char)data[i];
            }

            // fixing invalid data.
            if (mediaFromat.bytesPerSector == 0) mediaFromat.bytesPerSector = 1;
            if (mediaFromat.tracksPerSide == 0) mediaFromat.tracksPerSide = 1;
            if (mediaFromat.sectorsPerTrack == 0) mediaFromat.sectorsPerTrack = 1;
            if (mediaFromat.sides == 0) mediaFromat.sides = 1;

            return mediaFromat;
        }

        public static List<MediaFormat> loadPresets(bool console)
        {
            List<MediaFormat> Presets = new List<MediaFormat>();    // saves return value

            // add buildin presets
            Presets.Add(new MediaFormat(512, 80, 36, 2, 0xF0, true, "2.88 MiB - 3.5-inch, 2-sided, 80 tacks @ 36 sectors 512 BPS"));
            Presets.Add(new MediaFormat(512, 80, 18, 2, 0xF0, true, "1.44 MiB - 3.5-inch, 2-sided, 80 tacks @ 18 sectors 512 BPS"));
            Presets.Add(new MediaFormat(512, 80, 15, 2, 0xF9, true, "1.2 MiB - 5.25-inch, 2-sided, 80 tacks @ 15 sectors 512 BPS"));

            // add user created presets
            try
            {
                if (!Directory.Exists(@".\Presets")) // check if directory does not exists
                    return Presets;     

                foreach (string file in Directory.GetFiles(@".\Presets")) // for all files
                {
                    // check if we have the right file extension
                    string fileExt = file.Substring(file.Length - 4).ToUpper();
                    if (!fileExt.Equals(".CFG")) continue;

                    MediaFormat MF = readMediaFromFile(file, console);  // read the data, but don't add it yet
                    if (MF.sides == 0) // invalid data, an error occured
                    {
                        continue;
                    }
                    MF.path = file;     // save the file path (needed to apply changes to this file)
                    Presets.Add(MF);    

                }
                

            }
            catch (IOException)
            {
                Core.output(getString("unable_load_preset"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, console);
            }

            return Presets;
        }


        public static FileAndDir createFAD(string path, Settings settings, bool console)
        {
            uint bytesPerCluser = (uint)settings.BytesPerSector * settings.SectorsPerCluster;

            FileAndDir result = new FileAndDir();

            FileAndDir[] children = null;
            bool isDir = false;
            ulong fileSize = 0;

            try
            {

                if (Directory.Exists(path))
                {
                    isDir = true;

                    List<string> paths = new List<string>();
                    foreach (string p in Directory.GetFiles(path))
                        paths.Add(p);
                    foreach (string p in Directory.GetDirectories(path))
                        paths.Add(p);

                    children = createChildren(paths.ToArray(), settings, console);

                    result = new FileAndDir(isDir, children, path, fileSize);
                    fileSize = ImageGenerator.getFileSizeOnDisk(new FileAndDir[] { result }, bytesPerCluser, console) * bytesPerCluser;
                    result.fileSize = fileSize;

                }
                else if (File.Exists(path))
                {
                    // get filesize on disk
                    fileSize = (uint)Math.Ceiling(new FileInfo(path).Length / (double)bytesPerCluser) * bytesPerCluser;
                    result = new FileAndDir(isDir, children, path, fileSize);

                }
            }
            catch (IOException e)
            {
                Core.output(Core.getStringFormated("ioexcept_file", path, e.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand, console);
            }
            return result;

        }
        private static FileAndDir[] createChildren(string[] children, Settings settings, bool console)
        {
            FileAndDir[] result = new FileAndDir[children.Length];

            for (int i = 0; i < children.Length; i++)
            {
                result[i] = createFAD(children[i], settings, console);
            }

            return result;
        }














        public static void Main(bool console)
        {

            nullFADArray = new FileAndDir[] { new FileAndDir(console) };   // we can not use NULL for some functions, so we use this to represent null

            language = System.Globalization.CultureInfo.CurrentCulture.Name.ToLower();
            
            ///
            /// config file stuff
            ///

            if (File.Exists(configFile))
            {

                bool[] configurationsFound = new bool[configurationsCount]; // remember what configurations we have found

                // set all to false
                for (int i = 0; i < configurationsCount; i++)
                    configurationsFound[i] = false;

                try
                {
                    string[] configLines = readAllLines(configFile, 500 * 1024,console); // read all lines if the file is not bigger than 500 KiB
                    if (configLines == null) goto configurationEnd; // skip the code that relies on config file to be a string

                    // find the configurations
                    foreach (string lineUnfiltered in configLines)
                    {
                        //prepare line
                        if (lineUnfiltered.Length == 0) continue; // empty file
                        if (lineUnfiltered[0] == '#' || lineUnfiltered[0] == ' ') continue; // skip lines starting with a comment or a newline
                        string line = lineUnfiltered.Split('#')[0]; // remove comments
                        line = line.Trim();
                        line = line.ToLower();


                        if (line.StartsWith("language:".ToLower()))
                        {
                            language = line.Substring("language:".Length);
                            configurationsFound[(int)Configurations.language] = true;
                            continue;
                        }
                        if (line.StartsWith("presetUnsavedChangesWarningLevel:".ToLower()))
                        {

                            byte value = (byte)(line[line.Length - 1] - 0x30); // get the second to last char (should be the last digit of the number (and hopefully the only digit))

                            if (value > 2) // we only have 3 (0-2) warning levels 
                            {
                                value = 1; // set value to the default warning level
                                output(getStringFormated("invalidvalue", "presetUnsavedChangesWarningLevel"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                            }
                            presetUnsavedChangesStatus = (UnsavedChangesStatus)value;
                            configurationsFound[(int)Configurations.presetUnsavedChangesWarning] = true;
                            continue;
                        }
                        if (line.StartsWith("kilobyte:".ToLower()))
                        {

                            string value = line.Substring("kilobyte:".Length);

                            try
                            {
                                kilo = bool.Parse(value);
                            }
                            catch (FormatException)
                            {
                                output(getStringFormated("invalidvalue", "kilobyte"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                            }

                            configurationsFound[(int)Configurations.kilo] = true;
                            continue;
                        }
                        if (line.StartsWith("consoleversion:".ToLower()))
                        {
                            string value = line.Substring("consoleversion:".Length);

                            consoleVerPath = value;

                            configurationsFound[(int)Configurations.console] = true;
                            continue;
                        }
                        if (line.StartsWith("languageFiles:".ToLower()))
                        {
                            string value = line.Substring("languageFiles:".Length);

                            if (Directory.Exists(value)) 
                                languageFolder = value;
                            else if (Directory.Exists(@".\" + value))
                                languageFolder = @".\" + value;
                            else
                                output(getStringFormated("invalidvalue", "languageFiles"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning, console);

                            configurationsFound[(int)Configurations.languagePath] = true;
                            continue;
                        }
                    }


                    // add missing configurations
                    {
                        if (!configurationsFound[(int)Configurations.language])
                        {
                            try
                            {
                                File.AppendAllText(configFile, "\nlanguage:" + language);
                            }
                            catch (IOException)
                            {
                                output(getString("\nconfigfile_writeerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                            }
                        }
                        if (!configurationsFound[(int)Configurations.presetUnsavedChangesWarning])
                        {
                            try
                            {
                                File.AppendAllText(configFile, "\npresetUnsavedChangesWarningLevel:1");
                            }
                            catch (IOException)
                            {
                                output(getString("configfile_writeerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                            }
                        }
                        if (!configurationsFound[(int)Configurations.kilo])
                        {
                            try
                            {
                                File.AppendAllText(configFile, "\nkilobyte:false");
                            }
                            catch (IOException)
                            {
                                output(getString("configfile_writeerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                            }
                        }
                        if (!configurationsFound[(int)Configurations.console])
                        {
                            try
                            {
                                File.AppendAllText(configFile, "\nconsoleversion:.\\imged.exe");
                            }
                            catch (IOException)
                            {
                                output(getString("configfile_writeerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning, console);
                            }
                        }
                        if (!configurationsFound[(int)Configurations.languagePath])
                        {
                            try
                            {
                                File.AppendAllText(configFile, "\nlanguageFiles:"+languageFolder);
                            }
                            catch (IOException)
                            {
                                output(getString("configfile_writeerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning, console);
                            }
                        }
                    }

                configurationEnd:; // progamm goes to this goto if the configuration file was to big and the sting was set to null

                }
                catch (IOException)
                {
                    output(getString("configfile_readerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                }
            }
            else
            {
                try
                {
                    File.AppendAllText(configFile,
                        "language:" + language +
                        "\npresetUnsavedChangesWarningLevel:1" +
                        "\nkilobyte:false");
                }
                catch (IOException)
                {
                    output(getString("configfile_createerror"), getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                }
            }


            /// conig file stuff over


            getLanguageSource(console);

            // translation stuff
            {
                Sizes[0, 0] = getString("bytes");
                Sizes[0, 1] = getString("kib");
                Sizes[0, 2] = getString("mib");
                Sizes[0, 3] = getString("gib");

                Sizes[1, 0] = getString("bytes");
                Sizes[1, 1] = getString("kb");
                Sizes[1, 2] = getString("mb");
                Sizes[1, 3] = getString("gb");
            }

        }




    }


    public static class buidinTranslation
    {

        // these errors should never happen
        public static List<KeyValuePair<int, string>> errorCodeDescriptons = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(100,"file: MainForm.cs\nFunction: private void tre_fileViewer_DragDrop(object sender, DragEventArgs e)\nDescription: e.Data.GetData(DataFormats.FileDrop, false) is expected to return a string[] Object, but if this error occured, then it did not return an string[]"),
            new KeyValuePair<int, string>(101,"file: MainForm.cs\nFunction: private void tbx_outputFile_DragDrop(object sender, DragEventArgs e)\nDescription: e.Data.GetData(DataFormats.FileDrop, false) is expected to return a string[] Object, but if this error occured, then it did not return an string[]"),

            new KeyValuePair<int, string>(301,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The end of the Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(310,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The start of the . Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(300,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The start of the Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(311,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The end of the . Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(320,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The start of the .. Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(321,"file: Core.cs\nFunction: static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, WFA_ImgEdit.FileAndDir dir, byte[] FAT, bool fat16,uint parrentCluster, bool console)\nDescription: The end of the .. Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),


            new KeyValuePair<int, string>(400,"file: Core.cs\nFunction: static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)\nDescription: The start of the Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(401,"file: Core.cs\nFunction: static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)\nDescription: The end of the Directory entry, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(410,"file: Core.cs\nFunction: static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)\nDescription: The end of the file, if it would be added to the filesystem, would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(420,"file: Core.cs\nFunction: static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)\nDescription: The start of the current cluser of the data we want to write would be out of bounds (the disk is too small)"),
            new KeyValuePair<int, string>(421,"file: Core.cs\nFunction: static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)\nDescription: The end of the current cluser of the data we want to write would be out of bounds (the disk is too small)"),


        };

        // MUST BE IN LOWER CASE!
        public static List<KeyValuePair<string, string>> builinDefaultLanguage = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("abortretryignore","[A]bort [R]etry [I]gnore"),
            new KeyValuePair<string, string>("okcancel","[O]K [C]ancel"),
            new KeyValuePair<string, string>("retrycancel","[R]etry [C]ancel"),
            new KeyValuePair<string, string>("yesno","[Y]es [N]o"),
            new KeyValuePair<string, string>("yesnocancel","[Y]es [N]o [C]ancel"),

            new KeyValuePair<string, string>("addfiles","Add files"),
            new KeyValuePair<string, string>("addfolder","Add folder"),
            new KeyValuePair<string, string>("remove","Remove"),
            new KeyValuePair<string, string>("outputfile","Output file: "),
            new KeyValuePair<string, string>("createimage","Create image"),
            new KeyValuePair<string, string>("...","..."),
            new KeyValuePair<string, string>("language","Language"),

            new KeyValuePair<string, string>("help","Help"),
            new KeyValuePair<string, string>("options","Options"),
            new KeyValuePair<string, string>("imageoptions_menuestrip","Image options"),
            new KeyValuePair<string, string>("imageoptions","Options"),
            new KeyValuePair<string, string>("about","About ImageEdit"),

            new KeyValuePair<string, string>("used","Used: {0} {1}"),
            new KeyValuePair<string, string>("free","Free: {0} {1}"),
            new KeyValuePair<string, string>("disk","Disk: {0} {1}"),

            new KeyValuePair<string, string>("about_infotext","Programmed by: Roland Hartung\n\nVersion: {0}\n\n\nImage Credits:\n\nFloppy Icon: commons.wikimedia.org/wiki/File:Save-icon-floppy-disk-transparent-with-circle.png\n\nWarning Icon: https://commons.wikimedia.org/wiki/File:Atencion2.png"),
            new KeyValuePair<string, string>("about_infotext_title","About ImgEdit"),

            new KeyValuePair<string, string>("selectinput","Select File(s)"),
            new KeyValuePair<string, string>("selectoutputfile","Set output file"),



            new KeyValuePair<string, string>("bytes","Bytes"),
            new KeyValuePair<string, string>("kib","KiB"),
            new KeyValuePair<string, string>("mib","MiB"),
            new KeyValuePair<string, string>("gib","GiB"),

            new KeyValuePair<string, string>("kb","KB"),
            new KeyValuePair<string, string>("mb","MB"),
            new KeyValuePair<string, string>("gb","GB"),



            new KeyValuePair<string, string>("imageformat","Image Format"),
            new KeyValuePair<string, string>("number_fatcopies","FAT Copies:"),
            new KeyValuePair<string, string>("sectors_per_cluster","Sectors Per Cluster:"),
            new KeyValuePair<string, string>("rootentries","Root Entries:"),
            new KeyValuePair<string, string>("volumename","Volume Name"),
            new KeyValuePair<string, string>("custom_serialnumber","Custom Serial Number"),

            new KeyValuePair<string, string>("mediaformat","Media Format"),
            new KeyValuePair<string, string>("sides_and_heads","Sides and Heads:"),
            new KeyValuePair<string, string>("sectors_per_track","Sectors per track:"),
            new KeyValuePair<string, string>("tracks_per_side","Tracks per side"),
            new KeyValuePair<string, string>("bytes_per_sector","Bytes per sector"),
            new KeyValuePair<string, string>("mediadescriptor","Media Descriptor"),

            new KeyValuePair<string, string>("media_preset","Media Preset:"),

            new KeyValuePair<string, string>("formatedsize","Formated Size: {0} {1}"),

            new KeyValuePair<string, string>("kilo","Kilo"),

            new KeyValuePair<string, string>("apply","Apply"),
            new KeyValuePair<string, string>("cancel","Cancel"),


            new KeyValuePair<string, string>("fat16_notimplemented","Warning: FAT16 does not seem to work currently, use with caution."),
            new KeyValuePair<string, string>("sectors_per_cluster_warningtt","Supported values are: 1, 2, 4, 8, 16, 32 or 128"),
            new KeyValuePair<string, string>("bytes_per_sector_warningtt","Supported values are: 512, 1024, 2048 or 4096. (512 is recommended)"),
            new KeyValuePair<string, string>("rootentries_warningtt","Warning: Root entries are not the correct size, it should be: {0}"),
            new KeyValuePair<string, string>("wrong_fat","Warning: Media might be recognized as a {0} Media Instead of a {1} Media!"),

            new KeyValuePair<string, string>("deletepreset","Delete Preset"),
            new KeyValuePair<string, string>("savepreset","Save Preset"),
            new KeyValuePair<string, string>("savepreset_changed","Save Preset*"),
            new KeyValuePair<string, string>("newpreset","New Preset"),
            new KeyValuePair<string, string>("renamepreset","Rename Preset"),




            new KeyValuePair<string, string>("errormessagebox_tile","Error"),
            new KeyValuePair<string, string>("warningmessagebox_title","Warning"),

            new KeyValuePair<string, string>("unable_load_preset","Unable to load presets."),
            new KeyValuePair<string, string>("configurationfile_notsupported","Configuration File not supported! current version: {0} File version: {1}!"),
            new KeyValuePair<string, string>("configurationfile_corrupted","Configuration file {0} is corrupted!\nShould it be removed?"),
            new KeyValuePair<string, string>("configurationfile_corrupted_unabletodelete","Error: can not delete the file!"),
            new KeyValuePair<string, string>("configurationfile_notfound","Could not find the Data for {0}"),
            new KeyValuePair<string, string>("configurationfile_errorsave"," Error while Saving Preset:\n\n{0}"),
            new KeyValuePair<string, string>("configurationfile_errordelete","Error: can not delete preset:\n\n{0}"),


            new KeyValuePair<string, string>("fatbigerthanfat","Critical Internal error: FAT is bigger than the sectors per FAT!"),
            new KeyValuePair<string, string>("fatsmall","Critical error: FAT is less than 2 entries!"),
            new KeyValuePair<string, string>("invalidfilename","Invalid filename!"),
            new KeyValuePair<string, string>("writeat_reservedcluser","Error: attempted to write in a reserved cluster! Due to implementation, cluster 2 will be corrupted"),
            new KeyValuePair<string, string>("filenotexist","File {0} does not exist!"),
            new KeyValuePair<string, string>("filenameerror","Filename error: {0}"),
            new KeyValuePair<string, string>("directory_doesnotexist","Folder {0} does not exist!"),
            new KeyValuePair<string, string>("filenotfound_sizecalc","Warning: File not found during size calculation!\nFile: {0}"),
            new KeyValuePair<string, string>("ioexcept_delfile","IO Exception occurred while trying to delete {0}\n\n{1}"),
            new KeyValuePair<string, string>("ioexcept_file","IO error for file: {0}!\n\nmore info: {1}"),
            new KeyValuePair<string, string>("rootdirnotmultiple","Root Directory is not a multiple of {0} this might cause errors."),

            new KeyValuePair<string, string>("missingstorage","Error: more storage is needed than is available!\nNeeded storage: \t{0} {1}\nAvailabe storage: \t{2} {3}\nMissing storage: {4} {5}"),
            new KeyValuePair<string, string>("codeinvalidsize","code has invalid size, it must be 448 bytes and it's: {0} bytes"),
            new KeyValuePair<string, string>("toomanyroot","Too many files in the root directory!\nRoot directory can only hold {0} files, but {1} files were trieed to be added!"),
            new KeyValuePair<string, string>("fatwriteerror","Error: could not load {0}th FAT at adress {1}.\nFAT has size: {2}"),
            new KeyValuePair<string, string>("imgwriteerror","IO error While writing the image file.\n\n{0}"),
            new KeyValuePair<string, string>("outofbounds","Error {0}! Adress {1} is out of bounds!"),
            new KeyValuePair<string, string>("filetoobig","File {0} is too big! ( {1} bytes. Maximum size is {2} )"),

            new KeyValuePair<string, string>("filelist","{0} - {1} {2}"),

            new KeyValuePair<string, string>("filecreatedsuccsess","Image Successfully  Created!"),
            new KeyValuePair<string, string>("filecreatedsuccsess_title","Success"),

            new KeyValuePair<string, string>("unexpected_behavior","ERROR: Unexpected behavior!"),
            new KeyValuePair<string, string>("outputfile_invalid","Output path is invalid!"),

            new KeyValuePair<string, string>("ioexcept_sizecalc","ERROR:Unexpected IO Exception during Size Calculation!\nThis may create further errors down the line.\n\n{0}"),

            new KeyValuePair<string, string>("overwritemsg","{0} already exists!\nDo you want to overwrite it?"),
            new KeyValuePair<string, string>("overwritemsg_title","Overwrite file"),

            new KeyValuePair<string, string>("filesizelimit","File {0} exceeds the filesize limit of {1}!"),

            new KeyValuePair<string, string>("selectlanguage","Select Language:"),
            new KeyValuePair<string, string>("languageselection_windowtitle","Language Selection"),
            new KeyValuePair<string, string>("restartprog","Restart Required"),
            new KeyValuePair<string, string>("restartprog_title","Overwrite file"),

            new KeyValuePair<string, string>("databiggerdisk","Disk is not big enough to fit all your data.\nPlease consider going inside the image options to change the disk format."),
            new KeyValuePair<string, string>("disk_toosmall","The disk is too small to fit all the data! You should never see this error, if you do, contact the developer!"),

            new KeyValuePair<string, string>("renamepreset_title","Rename Preset"),
            new KeyValuePair<string, string>("presetname","Preset name:"),

            new KeyValuePair<string, string>("outofbounds_fat","Error: FAT cluster {0} is beyond the File Allocation Table!"),

            new KeyValuePair<string, string>("filesizelimitexceeded","Filesize limit of {1} {2} for {0} was exceeded"),
            new KeyValuePair<string, string>("invalidvalue","invalid value for {0}!"),
            new KeyValuePair<string, string>("configfile_writeerror","Config file can't be updated!"),
            new KeyValuePair<string, string>("configfile_createerror","Config file can't be created!"),
            new KeyValuePair<string, string>("configfile_readerror","Config file can't be read!"),

            new KeyValuePair<string, string>("unsavedchanges_title","Unsaved Changes"),
            new KeyValuePair<string, string>("unsavedchanges_yes_no","Do you want to save the Preset before continuing?"),
            new KeyValuePair<string, string>("unsavedchanges_ok_cancel","Unsaved changes with a buildin preset.\nYou may abort and save the changes as a new preset."),

            new KeyValuePair<string, string>("unexpectedinternal","Unexpected Internal error {0}!"),

            new KeyValuePair<string, string>("version_nomatch","ImageEditCore.DLL Version does not match the progamms version!"),




            new KeyValuePair<string, string>("cmdcommands","Commands:"),

            new KeyValuePair<string, string>("cmdargument","{0}\t\t - {1}"),
            new KeyValuePair<string, string>("cmdargument2","{0} {1}\t\t - {2}"),

            new KeyValuePair<string, string>("cmdargument_help","Shows this help text"),
            new KeyValuePair<string, string>("cmdargument_errorcode","Explains the specified error code."),
            new KeyValuePair<string, string>("cmdargument_output","specifies the output file"),
            new KeyValuePair<string, string>("cmdargument_input","specifies input file(s)"),
            new KeyValuePair<string, string>("cmdargument_confirm","list the effects of the command, before executing it."),

            new KeyValuePair<string, string>("cmdparam_errorcode","<error code>"),
            new KeyValuePair<string, string>("cmdparam_output","<output path>"),
            new KeyValuePair<string, string>("cmdparam_input","<input files>"),

            new KeyValuePair<string, string>("cmdargument_errorcode_notfound","This error code does not exist!"),
            new KeyValuePair<string, string>("cmdargument_tofew_args","command {0} needs an argument!"),
            new KeyValuePair<string, string>("presskey","Press any key to continue"),
            new KeyValuePair<string, string>("outputfile_cmd","output file: {0}"),
            new KeyValuePair<string, string>("inputfile_cmd","input file(s): {0}"),
            new KeyValuePair<string, string>("exec_cmd","execute command?"),

            new KeyValuePair<string, string>("overwritemsg_cmd","{0} already exist, {1}"),
            new KeyValuePair<string, string>("overwritemsg_cmd_0","and you will be asked if you want to overwrite it."),
            new KeyValuePair<string, string>("overwritemsg_cmd_1","and will be automatically overwritten."),
            new KeyValuePair<string, string>("overwritemsg_cmd_2","and the command will abort."),

            new KeyValuePair<string, string>("overwritemsg_info_1","file {0} already exists, it will be overwritten!"),
            new KeyValuePair<string, string>("overwritemsg_info_2","file {0} already exists, progamm aborts!"),

            new KeyValuePair<string, string>("de-de","German (Germany)"),
            new KeyValuePair<string, string>("en-gb","English (United Kingdom)"),



        };
    }

    public static class ImageGenerator
    {

        // http://www.maverick-os.dk/FileSystemFormats/FAT16_FileSystem.html#:~:text=The%20FAT16%20file%20system%20uses,volume%20(volume%20%3D%20partition).

        public const uint DirectoryEntrySize = 32;
        struct diskInfo
        {
            public ushort bytesPerSector;
            public byte sectorsPerCluster;
            public uint sectorCount;
            public uint clusterCount;


        };

        struct importantRegions
        {
            public uint FATRegionStart;
            public uint RootDirectoryRegionStart;
            public uint DataRegionStart;

        };


        struct Dates
        {
            public byte creationMillisecond;
            public ushort creationTime;
            public ushort creationDate;
            public ushort lastAccessDate;
            public ushort lastWriteTime;
            public ushort lastWriteDate;



            static public ushort convertTime(DateTime time)
            {
                return (ushort)((ushort)time.Second + ((ushort)time.Minute << 5) + ((ushort)time.Hour << 11));
            }
            static public ushort convertDate(DateTime time)
            {
                byte yearsFrom1980 = (byte)(time.Year - 1980);
                return (ushort)((ushort)time.Day + ((ushort)time.Month << 5) + ((ushort)yearsFrom1980 << 9));
            }


            static public ushort convertTime(byte hour, byte minute, byte second)
            {
                return (ushort)((ushort)second + ((ushort)minute << 5) + ((ushort)hour << 11));
            }
            static public ushort convertDate(ushort year, byte month, byte day)
            {
                byte yearsFrom1980 = (byte)(year - 1980);
                return (ushort)((ushort)day + ((ushort)month << 5) + ((ushort)yearsFrom1980 << 9));
            }
            static public ushort convertDate(byte year, byte month, byte day)
            {
                return (ushort)((ushort)day + ((ushort)month << 5) + ((ushort)year << 9));
            }
        }

        enum directoryEntryOffsets
        {
            filename = 0x00,
            extenstion = 0x08,
            attributes = 0x0B,
            reserved = 0x0C,
            creationMils = 0x0D,
            creationTime = 0x0E,
            creationDate = 0x10,
            lastAccsesDate = 0x12,
            ignoreInFAT12 = 0x14,
            lastWriteTime = 0x16,
            lastWriteDate = 0x18,
            firstLogicalCluster = 0x1A,
            fileSize = 0x1C


        }

        enum biosEntryOffsets
        {
            code = 0x0000,
            osName = 0x0003,
            bytesperSector = 0x000B,
            sectorsPerCluster = 0x000D,
            eeservedSectors = 0x000E,
            numberOfFATcopies = 0x0010,
            numberOfPossibleRootEntries = 0x0011,
            smallNumberOfSectors = 0x0013,
            mediaDescriptor = 0x0015,
            sectorsPerFAT = 0x0016,
            sectorsPerTrack = 0x0018,
            numberOfHeads = 0x001A,
            hiddenSectors = 0x001C,
            largeNumberOfSectors = 0x0020,
            driveNumber = 0x0024,
            reserved = 0x0025,
            extendedBootSignature = 0x0026,
            volumeSerialNumber = 0x0027,
            volumeLabel = 0x002B,
            dileSystemType = 0x0036,
            bootstrapCode = 0x003E,
            bootSectorSignature = 0x01FE
        }

        static diskInfo DiskInfo;
        static importantRegions ImportantRegions;

        static byte[] data;


        static private bool CheckIfExists(FileAndDir[] children, bool console)
        {
            bool ret = true;
            foreach (FileAndDir file in children)
            {
                if (!File.Exists(file.path) && !Directory.Exists(file.path))
                {
                    Core.output(Core.getStringFormated("filenotexist", file.path), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }
                if (file.children != null) ret = CheckIfExists(file.children, console);
                if (!ret) return false;
            }
            return true;
        }
        public static bool main(List<FileAndDir> inputFiles, string outputFile, Settings settings, bool console)
        {


            if (File.Exists(outputFile))
            {
                // loop for is the user wants to try again
                bool breakLoop = false;
                while (!breakLoop)
                {
                    try
                    {
                        File.Delete(outputFile);
                        breakLoop = true;
                    }
                    catch (IOException e)
                    {
                        DialogResult result = Core.output(Core.getStringFormated("ioexcept_delfile", outputFile, e.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand,console);
                        if (result == DialogResult.Cancel) return false;
                    }
                }
            }

            // check if all input files exist
            {
                bool ret = true;
                ret = CheckIfExists(inputFiles.ToArray(),console);
                if (!ret) return false;
            }


            bool fat16 = settings.FAT16;  // true for fat 16, false for fat 12

            // important info
            ushort bytesPerSector = settings.BytesPerSector;
            uint numberOfSectors = settings.NumberOfSectors;
            byte sectorsPerCluster = settings.SectorsPerCluster;             // supported values: 1, 2, 4, 8, 16, 32     (for floppy it should be 1)
            byte MediaDescriptor = settings.MediaDescriptor;
            byte FatCopies = settings.NumberFATcopies;                       // how man FATs are there?
            byte[] volumeLabel = settings.VolumeLabel;//{ 0x4E, 0X4F, 0X20, 0X4E, 0X41, 0X4D, 0X45, 0x20, 0x20, 0x20, 0x20 }; // 11 byte string

            DiskInfo.clusterCount = (numberOfSectors / sectorsPerCluster);

            DiskInfo.bytesPerSector = bytesPerSector;
            DiskInfo.sectorsPerCluster = sectorsPerCluster;
            DiskInfo.sectorCount = numberOfSectors;

            ushort reservedSectors = (ushort)Math.Ceiling((double)512 / bytesPerSector);//512 = bootsector size.  // boot sector is always reserved, so min value is 1. (how many sectors till the first fat sector)
            ushort numRootEntries = settings.NumberRootEntries;

            double clustersPerFAT;
            if (fat16)
            {
                clustersPerFAT = (double)Math.Ceiling((double)((DiskInfo.clusterCount * 2) / (double)bytesPerSector)); // amount of clusters times 2 (because we adress them as words) divided by the bytes we have in a sector
                if (clustersPerFAT < 1) clustersPerFAT = 1;
                if ((ushort)clustersPerFAT < clustersPerFAT) clustersPerFAT = (ushort)clustersPerFAT + 1; // if we use a fraction of a sector, count it as a whole sector
            }
            else
            {
                //sectorsPerFAT = (DiskInfo.clusterCount * 1.5) / bytesPerSector; // amount of clusters times 1.5 (because we adress them with 1.5 bytes) divided by the bytes we have in a sector
                clustersPerFAT = (double)Math.Ceiling((DiskInfo.clusterCount * 1.5) / bytesPerSector); // amount of clusters times 1.5 (because we adress them with 1.5 bytes) divided by the bytes we have in a sector
                if (clustersPerFAT < 1) clustersPerFAT = 1;
                if ((ushort)clustersPerFAT < clustersPerFAT) clustersPerFAT = (ushort)clustersPerFAT + 1; // if we use a fraction of a sector, count it as a whole sector
            }

            const byte rootDirectoryClusters = 14;
            uint FileSystemClusters = reservedSectors + (uint)(FatCopies * clustersPerFAT) + rootDirectoryClusters; // seems to leave 1 free cluster at the end.


            ImportantRegions.FATRegionStart = (uint)(reservedSectors * bytesPerSector);
            ImportantRegions.RootDirectoryRegionStart = (FatCopies * (uint)clustersPerFAT + reservedSectors) * bytesPerSector;
            ImportantRegions.DataRegionStart = ImportantRegions.RootDirectoryRegionStart + (numRootEntries * (uint)DirectoryEntrySize);

            if (ImportantRegions.DataRegionStart % bytesPerSector != 0) // if not a multiple of bytesPerSector, then make it a multiple of bytesPerSector
            {
                DialogResult result = Core.output(Core.getStringFormated("rootdirnotmultiple", bytesPerSector), Core.getString("warningmessagebox_title"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,console);
                if (result == DialogResult.Cancel) return false;

                ImportantRegions.DataRegionStart = ((uint)(ImportantRegions.DataRegionStart / bytesPerSector) + 1) * bytesPerSector;
            }






            data = new byte[bytesPerSector * numberOfSectors];


            // check if the data can even fit on the disk
            DiskRequirements diskReq = contentFitsInsideDisk(inputFiles, settings, console);
            if (!diskReq.dataFitsOnDisk)
            {
                Core.output(Core.getStringFormated("missingstorage",
                    Core.DisplayInUnits(diskReq.neededDiskSize, Core.kilo).number, Core.DisplayInUnits(diskReq.neededDiskSize, Core.kilo).text,
                    Core.DisplayInUnits(diskReq.availableDiskSize, Core.kilo).number, Core.DisplayInUnits(diskReq.availableDiskSize, Core.kilo).text,
                    Core.DisplayInUnits((long)(diskReq.neededDiskSize - diskReq.availableDiskSize), Core.kilo).number, Core.DisplayInUnits((long)(diskReq.neededDiskSize - diskReq.availableDiskSize), Core.kilo).text
                    ), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                return false;
            }





            {
                // *********** //
                //  Boot Sector  //
                // *********** //


                byte[] volumeSerialNumber = new byte[4];

                if (settings.CustiomVolumeSerialNumber)
                {
                    volumeSerialNumber[0] = (byte)(settings.VolumeSerialNumber >> (8 * 0));
                    volumeSerialNumber[1] = (byte)(settings.VolumeSerialNumber >> (8 * 1));
                    volumeSerialNumber[2] = (byte)(settings.VolumeSerialNumber >> (8 * 2));
                    volumeSerialNumber[3] = (byte)(settings.VolumeSerialNumber >> (8 * 3));
                }
                else
                {
                    Random rnd = new Random(); // used for serial number
                    rnd.NextBytes(volumeSerialNumber);
                }




                // ****************************************************

                // ALL DATA (EXPECT STRINGS AND CODE) IS LITTLE ENDIAN!

                // ****************************************************

                writeInData(0x0000, new byte[] { 0xEB, 0x3C, 0x90 });    //      //      //      //      // jump to code             // asmebly jump instucton and a nop. the middle byte is the jump target (relative) 
                writeInData(0x0003, new byte[] { 0x52, 0x48, 0x20, 0x49, 0x4D, 0x47, 0x20, 0x31 });      // os name                  // string "RH IMG 1"   (must be 8 bytes)


                // bios Parameter Block

                writeInData(0x000B, new byte[] { (byte)bytesPerSector, (byte)(bytesPerSector >> 8) });   // bytes Per Sector
                writeInData(0x000D, new byte[] { sectorsPerCluster });   //      //      //      //      // sectors Per Cluster
                writeInData(0x000E, new byte[] { (byte)reservedSectors, (byte)(reservedSectors >> 8) }); // reserved Sectors         // boot sector is always reserved, so min value is 1. (how many sectors till the first fat sector)
                writeInData(0x0010, new byte[] { FatCopies });   //      //      //      //      //      // number Of FAT copies     // backups to prevent data loss (2 is recommended)
                writeInData(0x0011, new byte[] { (byte)numRootEntries, (byte)(numRootEntries >> 8) });   // num possible root entys  // 224 entries  // guide says 512 are recomended, but the images I found use 224

                if (numberOfSectors > 0xFFFF)
                    writeInData(0x0013, new byte[] { 0x00, 0x00 });         //      //      //      //   // SmallNumberOfSectors     // zero when we are over the limit, we use Large Number Of Sectors instead
                else
                    writeInData(0x0013, new byte[] { (byte)numberOfSectors, (byte)(numberOfSectors >> 8) }); // SmallNumberOfSectors // Used when volume size is less 0xffff sectors big


                writeInData(0x0015, new byte[] { MediaDescriptor });     //      //      //      //      // MediaDescriptor
                writeInData(0x0016, new byte[] { (byte)clustersPerFAT, (byte)((ushort)clustersPerFAT >> 8) });// Sectors per FAT

                writeInData(0x0018, new byte[] { settings.SectorsPerTrack, 0x00 });         //      //      // Sectors Per Track
                writeInData(0x001A, new byte[] { settings.Sides, 0x00 });        //      //      //      // Number Of Heads          // set to the amount of sides at he moment

                writeInData(0x001C, new byte[] { 0x00, 0x00, 0x00, 0x00 });      //      //      //      // Hidden Sectors           // 0

                // Large Number Of Sectors
                if (numberOfSectors > 0xFFFF)
                    writeInData(0x0020, new byte[] { (byte)clustersPerFAT, (byte)((ushort)clustersPerFAT >> 8), (byte)((ushort)clustersPerFAT >> 16), (byte)((ushort)clustersPerFAT >> 24) });
                else
                    writeInData(0x0020, new byte[] { 0x00, 0x00, 0x00, 0x00 });    // set to zero if the  small number sectors is usuable

                // Ext. BIOS Parameter Block // contains info only for the fat16 file system

                writeInData(0x0024, new byte[] { 0x00 });        //      //      //      //      //      // Drive Number
                writeInData(0x0025, new byte[] { 0x00 });        //      //      //      //      //      // Reserved                 // It was original used to store the cylinder on which the boot sector is located. But Windows NT uses this byte to store two flags.

                writeInData(0x0026, new byte[] { 0x29 });        //      //      //      //      //      // Extended Boot Signature  // 29h means the next 3 bytes are availabe
                writeInData(0x0027, volumeSerialNumber);         //      //      //      //      //      // Volume Serial Number
                writeInData(0x002B, volumeLabel);        //      //      //      //      //      //      // Volume Label

                byte[] fileSystemType = { 0x46, 0X41, 0X54, 0X31, 0X32, 0x20, 0x20, 0x20 }; // 8 byte string (fat12)
                if (fat16)
                {
                    fileSystemType[4] = 0x36; // '6' (makes the string fat16 instead of fat12)
                    writeInData(0x0036, fileSystemType);
                }
                else
                {
                    writeInData(0x0036, fileSystemType);
                }




                byte[] code = { 0xFA,0x33,0xC0,0x8E,0xD0,0xBC,0x00,0x7C,0x16,0x07,0xBB,0x78,0x00,0x36,0xC5,0x37,
0x1E,0x56,0x16,0x53,0xBF,0x3E,0x7C,0xB9,0x0B,0x00,0xFC,0xF3,0xA4,0x06,0x1F,0xC6,
0x45,0xFE,0x0F,0x8B,0x0E,0x18,0x7C,0x88,0x4D,0xF9,0x89,0x47,0x02,0xC7,0x07,0x3E,
0x7C,0xFB,0xCD,0x13,0x72,0x79,0x33,0xC0,0x39,0x06,0x13,0x7C,0x74,0x08,0x8B,0x0E,
0x13,0x7C,0x89,0x0E,0x20,0x7C,0xA0,0x10,0x7C,0xF7,0x26,0x16,0x7C,0x03,0x06,0x1C,
0x7C,0x13,0x16,0x1E,0x7C,0x03,0x06,0x0E,0x7C,0x83,0xD2,0x00,0xA3,0x50,0x7C,0x89,
0x16,0x52,0x7C,0xA3,0x49,0x7C,0x89,0x16,0x4B,0x7C,0xB8,0x20,0x00,0xF7,0x26,0x11,
0x7C,0x8B,0x1E,0x0B,0x7C,0x03,0xC3,0x48,0xF7,0xF3,0x01,0x06,0x49,0x7C,0x83,0x16,
0x4B,0x7C,0x00,0xBB,0x00,0x05,0x8B,0x16,0x52,0x7C,0xA1,0x50,0x7C,0xE8,0x92,0x00,
0x72,0x1D,0xB0,0x01,0xE8,0xAC,0x00,0x72,0x16,0x8B,0xFB,0xB9,0x0B,0x00,0xBE,0xE6,
0x7D,0xF3,0xA6,0x75,0x0A,0x8D,0x7F,0x20,0xB9,0x0B,0x00,0xF3,0xA6,0x74,0x18,0xBE,
0x9E,0x7D,0xE8,0x5F,0x00,0x33,0xC0,0xCD,0x16,0x5E,0x1F,0x8F,0x04,0x8F,0x44,0x02,
0xCD,0x19,0x58,0x58,0x58,0xEB,0xE8,0x8B,0x47,0x1A,0x48,0x48,0x8A,0x1E,0x0D,0x7C,
0x32,0xFF,0xF7,0xE3,0x03,0x06,0x49,0x7C,0x13,0x16,0x4B,0x7C,0xBB,0x00,0x07,0xB9,
0x03,0x00,0x50,0x52,0x51,0xE8,0x3A,0x00,0x72,0xD8,0xB0,0x01,0xE8,0x54,0x00,0x59,
0x5A,0x58,0x72,0xBB,0x05,0x01,0x00,0x83,0xD2,0x00,0x03,0x1E,0x0B,0x7C,0xE2,0xE2,
0x8A,0x2E,0x15,0x7C,0x8A,0x16,0x24,0x7C,0x8B,0x1E,0x49,0x7C,0xA1,0x4B,0x7C,0xEA,
0x00,0x00,0x70,0x00,0xAC,0x0A,0xC0,0x74,0x29,0xB4,0x0E,0xBB,0x07,0x00,0xCD,0x10,
0xEB,0xF2,0x3B,0x16,0x18,0x7C,0x73,0x19,0xF7,0x36,0x18,0x7C,0xFE,0xC2,0x88,0x16,
0x4F,0x7C,0x33,0xD2,0xF7,0x36,0x1A,0x7C,0x88,0x16,0x25,0x7C,0xA3,0x4D,0x7C,0xF8,
0xC3,0xF9,0xC3,0xB4,0x02,0x8B,0x16,0x4D,0x7C,0xB1,0x06,0xD2,0xE6,0x0A,0x36,0x4F,
0x7C,0x8B,0xCA,0x86,0xE9,0x8A,0x16,0x24,0x7C,0x8A,0x36,0x25,0x7C,0xCD,0x13,0xC3,
0x0D,0x0A,0x4E,0x6F,0x6E,0x2D,0x53,0x79,0x73,0x74,0x65,0x6D,0x20,0x64,0x69,0x73,
0x6B,0x20,0x6F,0x72,0x20,0x64,0x69,0x73,0x6B,0x20,0x65,0x72,0x72,0x6F,0x72,0x0D,
0x0A,0x52,0x65,0x70,0x6C,0x61,0x63,0x65,0x20,0x61,0x6E,0x64,0x20,0x70,0x72,0x65,
0x73,0x73,0x20,0x61,0x6E,0x79,0x20,0x6B,0x65,0x79,0x20,0x77,0x68,0x65,0x6E,0x20,
0x72,0x65,0x61,0x64,0x79,0x0D,0x0A,0x00,0x49,0x4F,0x20,0x20,0x20,0x20,0x20,0x20,
0x53,0x59,0x53,0x4D,0x53,0x44,0x4F,0x53,0x20,0x20,0x20,0x53,0x59,0x53,0x00,0x00, };  // bootstrap code I found in the first dos floppy


                if (code.Length != 448)
                {
                    //Console.WriteLine("code has invalid size, it must be 448 and its: " + code.Length);
                    //Console.WriteLine("press any key to continue");
                    //Console.ReadKey();
                    Core.output(Core.getStringFormated("codeinvalidsize", code.Length), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }

                writeInData(0x003E, code);

                writeInData(0x01FE, new byte[] { 0x55, 0xAA });     // signature (magic to detect corruption, should not change)



                // ********************* //
                //  File Allocation Table  //
                // ********************* //


                byte[] fileAllocationTable;

                {               // generate a file allocation table with the appropiate size
                    double FATsize;
                    if (fat16)
                    {
                        FATsize = (numberOfSectors / sectorsPerCluster - FileSystemClusters) * 2; // 2 bytes per cluster
                        FATsize += 2; // offset the 2 reserved entries, that dont seem to point to anything really.
                    }
                    else
                    {
                        FATsize = (numberOfSectors / sectorsPerCluster - FileSystemClusters) * 1.5; // 1.5 bytes per cluster
                        if ((ushort)FATsize < FATsize) FATsize = (ushort)FATsize + 2; // if we use a fraction, count it as a whole and add 1, so the bytes are in 3 bytes clusters
                        FATsize += 3; // offset the 2 reserved entries, that dont seem to point to anything really.
                    }

                    fileAllocationTable = new byte[(ushort)FATsize];
                }

                // check for errors
                if (fileAllocationTable.Length > bytesPerSector * sectorsPerCluster * clustersPerFAT)
                {
                    Core.output(Core.getString("fatbigerthanfat"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }
                /*if (getSectorWithByteOffest((uint)fileAllocationTable.Length, true) < sectorsPerFAT)
                {
                    Console.WriteLine("Info: fat is a sector smaller than the sectors per FAT");
                }*/


                for (uint i = 0; i < fileAllocationTable.Length; i++)   // set everything to be a free cluster
                {
                    fileAllocationTable[i] = 0x00; // free cluster 
                }


                if (fileAllocationTable.Length < 2) // if fat too small,       exit progamm
                {
                    //Console.WriteLine("CRITICAL ERROR: FAT WAY TOO SMALL!");
                    Core.output(Core.getString("fatsmall"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }

                // set up reserved entrys


                writeInFat(0, fileAllocationTable, (ushort)(0xff00 + MediaDescriptor), fat16);  // first entry must be ffxx where xx is the Media Descriptor
                writeInFat(1, fileAllocationTable, 0xffff, fat16);                              // second entry must be end of file

                if (fat16)  // 2nd entry is reserverd in fat16
                {
                    if (!writeInFat(2, fileAllocationTable, 0xffff, fat16)) { Core.output(Core.getStringFormated("outofbounds_fat", 2), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return false; }
                }
                // root directory

                // set up the volume name
                byte[] directoryEntry = new byte[DirectoryEntrySize];   // data for a directory Entry is saved here

                for (uint i = 0; i < volumeLabel.Length; i++)
                {
                    directoryEntry[i] = volumeLabel[i];
                }

                directoryEntry[(uint)directoryEntryOffsets.attributes] = 0b00001000;  // flag for VolumeLabel

                writeInData(ImportantRegions.RootDirectoryRegionStart, directoryEntry);




                if (inputFiles.Count > numRootEntries)
                {
                    Core.output(Core.getStringFormated("toomanyroot", numRootEntries, inputFiles.Count), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }












                // write files
                ushort currentCluster = getNextFreeCluster(fileAllocationTable, 0, fat16);

                uint dirEntryIndex = 1;
                foreach (FileAndDir file in inputFiles)
                {
                    if (currentCluster == 0)
                    {
                        Core.output(Core.getString("disk_toosmall"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                        return false;
                    }


                    if (file.isDir)
                    {
                        if (!addDirectoryWithChildren(0, file, dirEntryIndex, currentCluster, fileAllocationTable, fat16, ImportantRegions.RootDirectoryRegionStart,console)) return false; // write dir, but abort if an error occured
                        currentCluster = getNextFreeCluster(fileAllocationTable, currentCluster, fat16);

                        dirEntryIndex++;
                    }
                    else
                     {
                        if (!writeFile(dirEntryIndex, ImportantRegions.RootDirectoryRegionStart, currentCluster, file.path, fileAllocationTable, fat16,console)) return false; // write the file, but abort if error
                        currentCluster = getNextFreeCluster(fileAllocationTable, currentCluster, fat16);

                        dirEntryIndex++;
                    }
                }








                // write the FATs to the image
                {
                    uint adr = 0x200;
                    for (uint i = 0; i < FatCopies; i++)
                    {
                        if (!writeInData(adr, fileAllocationTable))
                        {
                            Core.output(Core.getStringFormated("fatwriteerror", i + 1, adr, fileAllocationTable.Length), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                            return false;
                        }
                        adr += (uint)(bytesPerSector * clustersPerFAT);
                    }
                }

                // this loop is in case of an IO Expection, it gives the user the option to retry.
                bool breakLoop = false;
                while (!breakLoop)
                {
                    try
                    {
                        FileStream fs = File.Create(outputFile);
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        breakLoop = true;
                    }
                    catch (IOException e)
                    {
                        DialogResult result = Core.output(Core.getStringFormated("imgwriteerror", e.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand,console);
                        if (result == DialogResult.Cancel) return false;
                    }
                }

            }


            Core.output(Core.getString("filecreatedsuccsess"), Core.getString("filecreatedsuccsess_title"), MessageBoxButtons.OK, MessageBoxIcon.Information,console);
            return true;
        }


















        static bool addDirectoryWithChildren(ushort parrent, FileAndDir directory, uint dirEntryIndex, ushort currentCluster, byte[] fileAllocationTable, bool fat16, uint parentDirectoryAdress,bool console)
        {

            uint directoryAdress = writeDirectory(dirEntryIndex, parentDirectoryAdress, (ushort)currentCluster, directory, fileAllocationTable, fat16, parrent, console);
            if (directoryAdress == 0) return false;

            ushort freeCluster = getNextFreeCluster(fileAllocationTable, currentCluster, fat16);

            uint dirEntryIndexOwn = 2; // start at 2 so we keep the . and .. entry

            foreach (FileAndDir fdir in directory.children)
            {
                if (freeCluster == 0)
                {
                    Core.output(Core.getString("disk_toosmall"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }

                if (fdir.isDir)
                {
                    if (!addDirectoryWithChildren((ushort)currentCluster, fdir, dirEntryIndexOwn, freeCluster, fileAllocationTable, fat16, directoryAdress,console)) return false;
                }
                else
                    if (!writeFile(dirEntryIndexOwn, directoryAdress, freeCluster, fdir.path, fileAllocationTable, fat16,console)) return false; // write the file, but abort if error                   


                dirEntryIndexOwn++;
                freeCluster = getNextFreeCluster(fileAllocationTable, freeCluster, fat16);
            }


            return true;
        }

        static byte[] convertFileNameExt(string path,bool console)
        {
            byte[] filenameAndExtension = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };    // 11 space

            // if the path ends with \ then look up the string before \
            byte tmpOffset = 1;
            if (path[path.Length - 1] == '\\')
            {
                tmpOffset = 2;

                if (path.Length < 2) { Core.output(Core.getStringFormated("filenameerror", path), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return filenameAndExtension; }
            }


            // get the raw filename
            string filename = path.ToUpper().Split('\\')[path.Split('\\').Length - tmpOffset];
            char[] byteName = new char[filename.Length + 1];
            int byteIndex = 0;

            // filter out bad charaters
            for (int i = 0; i < filename.Length; i++)
            {
                byteName[byteIndex] = filename[i];
                switch (byteName[byteIndex])
                {

                    case ' ':
                        byteName[byteIndex] = '\0';
                        continue;


                    default:
                        byteIndex++;
                        break;
                }
            }

            filename = new string(byteName);
            filename = filename.Split('\0')[0]; // remove any /0

            string[] nameAndExt = filename.Split('.');
            //if (nameAndExt[0].Length == 0) { Console.WriteLine("invalid filename!"); return filenameAndExtension; }
            if (nameAndExt[0].Length == 0) { Core.output(Core.getString("invalidfilename"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return filenameAndExtension; }



            for (byte i = 0, j = 0; i < 8 && j < nameAndExt[0].Length; i++, j++)
                filenameAndExtension[i] = (byte)nameAndExt[0][i];   // copy 8 chars from the filename


            if (nameAndExt.Length == 1) return filenameAndExtension;


            for (byte i = 0, j = 0; i < 3 && j < nameAndExt[nameAndExt.Length - 1].Length; i++, j++)
                filenameAndExtension[i + 8] = (byte)nameAndExt[nameAndExt.Length - 1][j];   // copy 3 chars from the extention

            return filenameAndExtension;
        }


        static uint writeDirectory(uint index, uint dirEntryStartAdress, ushort cluster, FileAndDir dir, byte[] FAT, bool fat16, uint parrentCluster,bool console) //return 0 = error
        {


            // check if folder exists
            if (!Directory.Exists(dir.path))
            {
                Core.output(Core.getStringFormated("directory_doesnotexist", dir.path), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                return 0; // 0 = error
            }


            // get dates
            Dates dates;
            dates.creationMillisecond = (byte)(Directory.GetCreationTime(dir.path).Millisecond / 10); if (dates.creationMillisecond > 199) dates.creationMillisecond -= 199;
            dates.creationTime = Dates.convertTime(Directory.GetCreationTime(dir.path));
            dates.lastAccessDate = Dates.convertTime(Directory.GetLastAccessTime(dir.path));
            dates.lastWriteTime = Dates.convertTime(Directory.GetLastWriteTime(dir.path));

            dates.creationDate = Dates.convertDate(Directory.GetCreationTime(dir.path));
            dates.lastWriteDate = Dates.convertDate(Directory.GetLastAccessTime(dir.path));



            // get the filename
            byte[] filename = convertFileNameExt(dir.path,console);

            // save stuff


            byte[] directoryEntry = new byte[DirectoryEntrySize];  // data for a directory Entry is temporary saved here




            for (uint j = 0; j < 11; j++)          // copy filename
                directoryEntry[(uint)directoryEntryOffsets.filename + j] = filename[j];


            for (uint j = 0; j < 2; j++)           // set startung cluster
                directoryEntry[(uint)directoryEntryOffsets.firstLogicalCluster + j] = (byte)(cluster >> (byte)(j * 8));


            for (uint j = 0; j < 4; j++)          // set file size
                directoryEntry[(uint)directoryEntryOffsets.fileSize + j] = 0;


            directoryEntry[(uint)directoryEntryOffsets.attributes] = (byte)0b00010000; // this is a directory



            // get dates
            directoryEntry[(uint)directoryEntryOffsets.creationMils] = dates.creationMillisecond;

            for (uint j = 0; j < 2; j++)           // set creationTime
                directoryEntry[(uint)directoryEntryOffsets.creationTime + j] = (byte)(dates.creationTime >> (byte)(j * 8));

            for (uint j = 0; j < 2; j++)           // set creationDate
                directoryEntry[(uint)directoryEntryOffsets.creationDate + j] = (byte)(dates.creationDate >> (byte)(j * 8));

            for (uint j = 0; j < 2; j++)           // setlastWriteTime
                directoryEntry[(uint)directoryEntryOffsets.lastWriteTime + j] = (byte)(dates.lastWriteTime >> (byte)(j * 8));

            for (uint j = 0; j < 2; j++)           // set lastWriteDate
                directoryEntry[(uint)directoryEntryOffsets.lastWriteDate + j] = (byte)(dates.lastWriteDate >> (byte)(j * 8));




            // write dir entry                                                                                                        
            if (dirEntryStartAdress + index * DirectoryEntrySize >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "300", "0x" + (dirEntryStartAdress + index).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; } // return 0 = error
            if (dirEntryStartAdress + index * DirectoryEntrySize + DirectoryEntrySize - 1 >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "301", "0x" + (dirEntryStartAdress + index + DirectoryEntrySize - 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; }
            writeInData(dirEntryStartAdress + index * DirectoryEntrySize, directoryEntry);


            uint newDirectoryAdress = getAdressFromCluster(cluster);

            // write the dir entries for . and ..
            {
                directoryEntry = new byte[DirectoryEntrySize];  // data for a directory Entry is temporary saved here
                directoryEntry[(uint)directoryEntryOffsets.filename] = (byte)'.';
                for (int i = 1; i < 11; i++) // starts at 1, since we have already written '.'
                    directoryEntry[(uint)directoryEntryOffsets.filename + i] = (byte)' ';


                directoryEntry[(uint)directoryEntryOffsets.attributes] = (byte)0b00010000; // this is a directory

                for (uint j = 0; j < 2; j++)           // set startung cluster
                    directoryEntry[(uint)directoryEntryOffsets.firstLogicalCluster + j] = (byte)(cluster >> (byte)(j * 8));

                // write dir entry
                if (newDirectoryAdress + 0 * DirectoryEntrySize >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "310", "0x" + (newDirectoryAdress + 0).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; } // return 0 = error
                if (newDirectoryAdress + 0 * DirectoryEntrySize + DirectoryEntrySize - 1 >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "311", "0x" + (newDirectoryAdress + 0 + DirectoryEntrySize - 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; }
                writeInData(newDirectoryAdress + 0 * DirectoryEntrySize, directoryEntry);


                ///////////////

                directoryEntry = new byte[DirectoryEntrySize];  // data for a directory Entry is temporary saved here
                directoryEntry[(uint)directoryEntryOffsets.filename] = (byte)'.';
                directoryEntry[(uint)directoryEntryOffsets.filename + 1] = (byte)'.';
                for (int i = 2; i < 11; i++) // starts at 2, since we have already written '..'
                    directoryEntry[(uint)directoryEntryOffsets.filename + i] = (byte)' ';

                directoryEntry[(uint)directoryEntryOffsets.attributes] = (byte)0b00010000; // this is a directory

                for (uint j = 0; j < 2; j++)           // set startung cluster
                    directoryEntry[(uint)directoryEntryOffsets.firstLogicalCluster + j] = (byte)(parrentCluster >> (byte)(j * 8));

                // write dir entry
                if (newDirectoryAdress + 1 * DirectoryEntrySize >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "320", "0x" + (newDirectoryAdress + 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; } // return 0 = error
                if (newDirectoryAdress + 1 * DirectoryEntrySize + DirectoryEntrySize - 1 >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "321", "0x" + (newDirectoryAdress + 1 + DirectoryEntrySize - 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; }
                writeInData(newDirectoryAdress + 1 * DirectoryEntrySize, directoryEntry);
            }



            ushort currentCluster = cluster;

            if ((dir.children.Length + 2) * DirectoryEntrySize > DiskInfo.bytesPerSector * DiskInfo.sectorsPerCluster) // if we need more than just one cluster
            {
                uint neededClusters = (uint)Math.Ceiling((dir.children.Length + 2) * (double)DirectoryEntrySize / (DiskInfo.bytesPerSector * DiskInfo.sectorsPerCluster));
                neededClusters--; // do not count the last cluster

                while (neededClusters > 0)
                {
                    if (getAdressFromCluster((ushort)(currentCluster + 1)) > data.Length)
                    {
                        Core.output(Core.getString("disk_toosmall"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                        return 0;
                    }
                    if (!writeInFat(currentCluster, FAT, ++currentCluster, fat16)) // set current cluter to file end
                    { Core.output(Core.getStringFormated("outofbounds_fat", currentCluster - 1), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; }
                    neededClusters--;
                }

            }

            if (!writeInFat(currentCluster, FAT, 0xffff, fat16)) // set current cluter to file end
            { Core.output(Core.getStringFormated("outofbounds_fat", currentCluster), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); return 0; }





            return newDirectoryAdress;
        }
        static bool writeFile(uint index, uint dirEntryStartAdress, ushort cluster, string path, byte[] FAT, bool fat16, bool console)
        {

            bool breakloop = false;

            while (!breakloop)
            {
                // check if file exists
                if (!File.Exists(path))
                {
                    Core.output(Core.getStringFormated("filenotexist", path), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                    return false;
                }

                try
                {
                    FileStream readfile = new FileStream(path, FileMode.Open);

                    //Console.WriteLine("Info: Input File: {0} lenght: {1} bytes.", readfile.Name, readfile.Length);

                    // get dates
                    Dates dates;
                    dates.creationMillisecond = (byte)(File.GetCreationTime(path).Millisecond / 10); if (dates.creationMillisecond > 199) dates.creationMillisecond -= 199;
                    dates.creationTime = Dates.convertTime(File.GetCreationTime(path));
                    dates.lastAccessDate = Dates.convertTime(File.GetLastAccessTime(path));
                    dates.lastWriteTime = Dates.convertTime(File.GetLastWriteTime(path));

                    dates.creationDate = Dates.convertDate(File.GetCreationTime(path));
                    dates.lastWriteDate = Dates.convertDate(File.GetLastAccessTime(path));



                    // get the filename
                    byte[] filename = convertFileNameExt(path,console);

                    // check if file is too big
                    if (readfile.Length > uint.MaxValue)
                    {
                        Core.output(Core.getStringFormated("filetoobig ", readfile.Name, readfile.Length, uint.MaxValue), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                        readfile.Close();
                        return false;
                    }




                    // save stuff


                    byte[] directoryEntry = new byte[DirectoryEntrySize];  // data for a directory Entry is temporary saved here


                    if (readfile.Length == 0) cluster = 0; // did not find this in dokumentation, i found it out myself by testing. to veryfy, save an empty text file to a floppy and check the cluster in a hex editor


                    for (uint j = 0; j < 11; j++)          // copy filename
                        directoryEntry[(uint)directoryEntryOffsets.filename + j] = filename[j];


                    for (uint j = 0; j < 2; j++)           // set starting cluster
                        directoryEntry[(uint)directoryEntryOffsets.firstLogicalCluster + j] = (byte)(cluster >> (byte)(j * 8));


                    for (uint j = 0; j < 4; j++)          // set file size
                        directoryEntry[(uint)directoryEntryOffsets.fileSize + j] = (byte)(readfile.Length >> (byte)(j * 8));


                    directoryEntry[(uint)directoryEntryOffsets.attributes] = (byte)File.GetAttributes(path);


                    // write dates
                    directoryEntry[(uint)directoryEntryOffsets.creationMils] = dates.creationMillisecond;

                    for (uint j = 0; j < 2; j++)           // set startung cluster
                        directoryEntry[(uint)directoryEntryOffsets.creationTime + j] = (byte)(dates.creationTime >> (byte)(j * 8));

                    for (uint j = 0; j < 2; j++)           // set startung cluster
                        directoryEntry[(uint)directoryEntryOffsets.creationDate + j] = (byte)(dates.creationDate >> (byte)(j * 8));

                    for (uint j = 0; j < 2; j++)           // set startung cluster
                        directoryEntry[(uint)directoryEntryOffsets.lastWriteTime + j] = (byte)(dates.lastWriteTime >> (byte)(j * 8));

                    for (uint j = 0; j < 2; j++)           // set startung cluster
                        directoryEntry[(uint)directoryEntryOffsets.lastWriteDate + j] = (byte)(dates.lastWriteDate >> (byte)(j * 8));




                    // write dir entry
                    if (dirEntryStartAdress + index * DirectoryEntrySize >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "400", "0x" + (dirEntryStartAdress + index).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); readfile.Close(); return false; }
                    if (dirEntryStartAdress + index * DirectoryEntrySize + DirectoryEntrySize - 1 >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "401", "0x" + (dirEntryStartAdress + index + DirectoryEntrySize - 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); ; readfile.Close(); return false; }
                    writeInData(dirEntryStartAdress + index * DirectoryEntrySize, directoryEntry);

                    if (readfile.Length == 0)
                    {
                        readfile.Close();
                        breakloop = true;
                        continue;
                        
                    }

                    ushort currentCluster = cluster;
                    bool fileEnd = false;
                    int i;

                    if (getDataRegionOffset(currentCluster,console) + (uint)readfile.Length >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "410", "0x" + getDataRegionOffset(currentCluster + (uint)readfile.Length,console).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); ; readfile.Close(); return false; }

                    do
                    {

                        // check if the entire cluster is in bounds
                        if (getDataRegionOffset(currentCluster,console) >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "420", "0x" + getDataRegionOffset(currentCluster,console).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); readfile.Close(); return false; }
                        if (getDataRegionOffset(currentCluster,console) + DiskInfo.bytesPerSector - 1 >= DiskInfo.sectorCount * DiskInfo.bytesPerSector) { Core.output(Core.getStringFormated("outofbounds", "421", "0x" + (getDataRegionOffset(currentCluster + (uint)1, console) - 1).ToString("x")), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); readfile.Close(); return false; }

                        for (i = 0; i < DiskInfo.bytesPerSector; i++)
                        {

                            // write data

                            //ushort tmpCluser = getNextFreeCluster(FAT, currentCluster);

                            writeInData((uint)(getDataRegionOffset(currentCluster,console) + i), new byte[] { (byte)readfile.ReadByte() });
                            //if (readfile.Position == readfile.Length - 1) { fileEnd = true; break; }
                            if (readfile.Position == readfile.Length) { fileEnd = true; break; }

                        }


                        // write in fat
                        if (i == DiskInfo.bytesPerSector)
                        {
                            if (!writeInFat(currentCluster, FAT, ++currentCluster, fat16)) { Core.output(Core.getStringFormated("outofbounds_fat", currentCluster - 1), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); readfile.Close(); return false; }
                        }

                    } while (!fileEnd);

                    if (!writeInFat(currentCluster, FAT, 0xffff, fat16))
                    { // set current cluter to file end
                        Core.output(Core.getStringFormated("outofbounds_fat", currentCluster), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console); readfile.Close(); return false;
                    }


                    readfile.Close();
                    breakloop = true;
                }
                catch (IOException e)
                {
                    DialogResult result = Core.output(Core.getStringFormated("ioexcept_file", path, e.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand,console);
                    if (result == DialogResult.Cancel) breakloop = true;
                }
            }
            return true;
        }

        static bool writeInData(uint offset, byte[] value)
        {
            if (data.Length <= offset) return false;                // if the start of the value is out of bounds, return false
            if (data.Length <= offset + value.Length) return false; // if the end of the value is out of bounds, return false

            for (uint i = 0; i < value.Length; i++)  // for every byte in value
            {
                data[offset + i] = value[i]; // copy it
            }

            return true;
        }


        static bool writeInFat(uint offset, byte[] FAT, ushort value, bool fat16)
        {
            uint i = fat16 ? offset * 2 : (uint)(offset * 1.5); // if fat 16, offset used 2 bytes, if fat 12 fat used 1.5 bytes

            if (FAT.Length <= i + 1)
                return false; // out of bounds

            if (fat16)
            {
                FAT[i] = (byte)value;             // load the lower byte
                FAT[i + 1] = (byte)(value >> 8);  // load the upper byte
            }
            else
            {
                if (offset * 1.5 - (uint)(offset * 1.5) > 0.25) // we begin with a half byte
                {
                    FAT[i] += (byte)((value << 4) & 0xF0); // load the lower byte   (and it with 0xF0 making it only use the lowwer half)
                    FAT[i + 1] = (byte)(value >> 4); // load the upper byte

                }
                else  // we begin with a full byte
                {
                    FAT[i] = (byte)value;                     // load the lower byte
                    FAT[i + 1] = (byte)((value >> 8) & 0x0F); // load the upper byte   ( shift the first half byte out and and it with 0x0F making it only use the upper half)
                }
            }


            return true;
        }


        static uint getDataRegionOffset(uint FATcluster, bool console)
        {
            if (FATcluster <= 1)
            {
                Core.output(Core.getString("writeat_reservedcluser"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
                return ImportantRegions.DataRegionStart;
            }


            return ImportantRegions.DataRegionStart + (FATcluster - 2) * DiskInfo.bytesPerSector;
        }


        static uint getSectorWithByteOffest(uint index) // rounds down the sector
        {
            return index / DiskInfo.bytesPerSector;
        }

        static ushort getNextFreeCluster(byte[] FAT, ushort startSearchAt, bool fat16)
        {
            uint i = fat16 ? (uint)startSearchAt * 2 : (uint)((uint)startSearchAt * 1.5);   // convert the number system the caller uses to the actuall numbers

            while (true)
            {
                if (i + 1 >= FAT.Length)
                {   // if we cant find any free clusters from the start id, return 0 (meaning error)
                    return 0;
                }

                // if we find a free cluster return it

                if (fat16)
                {
                    if (FAT[i] == 0 && FAT[i + 1] == 0)
                    {
                        return (ushort)(i / 2);   // convert back to the number system the caller uses
                    }
                }
                else
                {
                    if (i / 1.5 - (uint)(i / 1.5) > 0.25) // we are in a half byte
                    {
                        if ((FAT[i] & 0xf0) == 0 && FAT[i + 1] == 0) // if fat[i] logical and operation 0xf0 (the left half of the byte) is zero and ...
                        {
                            return (ushort)((i / 1.5) + 1);  // convert back to the number system the caller uses   (+1 so we round up and now down)
                        }


                    }
                    else  // we are in a full byte
                    {
                        if (FAT[i] == 0 && (FAT[i + 1] & 0x0f) == 0) // ... and if fat[i+1] logical and operation 0x0f (the right half of the byte) is zero
                        {
                            return (ushort)(i / 1.5);  // convert back to the number system the caller uses
                        }

                    }
                }

                i++;
            }

        }

        static uint getAdressFromCluster(ushort cluster)
        {
            return (uint)(ImportantRegions.DataRegionStart + ((cluster - 2) * DiskInfo.sectorsPerCluster * DiskInfo.bytesPerSector));
        }






        public struct DiskRequirements
        {
            public ulong neededDiskSize;
            public uint availableDiskSize;
            public bool dataFitsOnDisk;

            public DiskRequirements(ulong neededDiskSize, uint availableDiskSize)
            {

                this.neededDiskSize = neededDiskSize;
                this.availableDiskSize = availableDiskSize;

                if (neededDiskSize > availableDiskSize)
                    dataFitsOnDisk = false;
                else
                    dataFitsOnDisk = true;
            }
        }




        public static DiskRequirements contentFitsInsideDisk(List<FileAndDir> files, Settings settings, bool console)
        {


            uint clusterCount = settings.NumberOfSectors / settings.SectorsPerCluster;

            // get FAT size
            double sectorsPerFAT;
            if (settings.FAT16)
            {
                sectorsPerFAT = (double)Math.Ceiling((double)((clusterCount * 2) / (double)settings.BytesPerSector)); // amount of clusters times 2 (because we adress them as words) divided by the bytes we have in a sector
                if (sectorsPerFAT < 1) sectorsPerFAT = 1;
                if ((ushort)sectorsPerFAT < sectorsPerFAT) sectorsPerFAT = (ushort)sectorsPerFAT + 1; // if we use a fraction of a sector, count it as a whole sector
            }
            else
            {
                sectorsPerFAT = (double)Math.Ceiling((clusterCount * 1.5) / settings.BytesPerSector); // amount of clusters times 1.5 (because we adress them with 1.5 bytes) divided by the bytes we have in a sector
                if (sectorsPerFAT < 1) sectorsPerFAT = 1;
                if ((ushort)sectorsPerFAT < sectorsPerFAT) sectorsPerFAT = (ushort)sectorsPerFAT + 1; // if we use a fraction of a sector, count it as a whole sector
            }


            uint availableClusters = settings.NumberOfSectors / settings.SectorsPerCluster;
            uint bytesPerCluster = (uint)(settings.BytesPerSector * settings.SectorsPerCluster);
            uint fileSystemClusters = (uint)Math.Ceiling((double)bytesPerCluster / 512);                                  // boot sector
            fileSystemClusters += (uint)sectorsPerFAT * settings.NumberFATcopies;    // FAT Size
            fileSystemClusters += (uint)Math.Ceiling(((double)settings.NumberRootEntries * DirectoryEntrySize) / bytesPerCluster); // Root Directory
            ulong usedClusters = fileSystemClusters;

            usedClusters += getFileSizeOnDisk(files.ToArray(), bytesPerCluster,console);

            return new DiskRequirements(usedClusters * bytesPerCluster, availableClusters * bytesPerCluster);
        }

        public static ulong getFileSizeOnDisk(FileAndDir[] files, uint bytesPerCluster, bool console)
        {
            ulong usedClusters = 0;
            try
            {
                foreach (FileAndDir file in files)
                {
                    if (File.Exists(file.path))
                    {
                        ulong tmp = (uint)Math.Ceiling((double)new FileInfo(file.path).Length / bytesPerCluster); // the used clusters based on the filesize
                        usedClusters += tmp;
                        if (tmp == 0) usedClusters++; // file size of 0 still allocates 1 cluster
                    }
                    else if (Directory.Exists(file.path))
                    {
                        uint directoryEntries = 2; // all dirs (except root) have at lest 2 dir entries
                        directoryEntries += (uint)file.children.Length; // add the other entries

                        usedClusters += (uint)Math.Ceiling(((double)directoryEntries * DirectoryEntrySize) / bytesPerCluster); // add allocated space for the directory entries
                        usedClusters += getFileSizeOnDisk(file.children, bytesPerCluster, console); // add size of files in the directory
                    }
                    else
                    {
                        Core.output(Core.getStringFormated("filenotfound_sizecalc", file.path), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Warning,console);
                    }
                }
            }
            catch (IOException e)
            {
                Core.output(Core.getStringFormated("ioexcept_sizecalc", e.ToString()), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK, MessageBoxIcon.Hand,console);
            }

            return usedClusters;
        }
    }

    

}





