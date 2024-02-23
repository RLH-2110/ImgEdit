using System;
using System.Windows.Forms;
using System.IO;
using ImageEditCore;
using System.Diagnostics;

namespace WFA_ImgEdit
{ 

    static class Program
    {
        // global variables for the entire progamm
        public const string VERSION_NUMBER = "0.6";
        public const bool console = false;


        [STAThread]
        static void Main(string[] args)
        {

            string workAround = Core.VERSION_NUMBER;
            if (VERSION_NUMBER != workAround)
            {
                /*int coreMajorVer;
                int thisMajorVer;
                if (int.TryParse(Core.VERSION_NUMBER.Split('.')[0],out coreMajorVer))
                    if (int.TryParse(VERSION_NUMBER.Split('.')[0], out thisMajorVer))
                        if (thisMajorVer != coreMajorVer)*/
                            
                             Core.output(Core.getString("version_nomatch"), Core.getString("errormessagebox_tile"), MessageBoxButtons.OK,MessageBoxIcon.Hand, console);
            }


            Core.Main(console);


            if (args.Length != 0) // for if we need to launch the console version
            {
                string cmdArgs = "";
                foreach (string arg in args)
                {
                    cmdArgs += arg + ' ';
                }

            repeat: // you jump here if (the check without .\ failed, LOGICAL_AND the check with .\ passed)

                if (File.Exists(Core.consoleVerPath))
                {
                    // lauch console version, if it exits
                    Process cmdVersion = new Process();
                    cmdVersion.StartInfo.FileName = Core.consoleVerPath;
                    cmdVersion.StartInfo.Arguments = cmdArgs;
                    cmdVersion.Start();
                    return;
                }

                if (File.Exists(@".\" + Core.consoleVerPath))
                {
                    Core.consoleVerPath = @".\" + Core.consoleVerPath;
                    goto repeat;
                }

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new form_main());
        }
    }
}
