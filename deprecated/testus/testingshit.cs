using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static ImageEditCore.Core;
using System.IO;

namespace testus
{
    class testingshit
    {


      
        static Stopwatch sw;
        static void Main(string[] args)
        {
            readAllBytes("ksfnaskf54as4f293rdol ;<!?O\\!=R§%()§%\"§%", 5000, true);
            Console.ReadKey();
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


    }
}
