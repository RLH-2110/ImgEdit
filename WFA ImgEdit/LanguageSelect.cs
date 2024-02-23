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
    public partial class languageSelect : Form
    {
        public languageSelect()
        {
            InitializeComponent();
        }

        private bool foundBritishEnglish = false;
        private int britishEnglishIndex = 0;
        private List<string> langs;
        private void languageSelect_Load(object sender, EventArgs e)
        {
            // find the language files
            langs = Directory.GetFiles(Core.languageFolder).ToList<string>();

            // filter the files, so only the language files remain
            for (int i = 0;i < langs.Count; i++)
            {

                langs[i] = langs[i].Substring(Core.languageFolder.Length);

                if (langs[i].Length < Core.languageFileExt.Length + 1) // if he filename is smaller than the file extentsion + 1 character
                {
                    langs.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!langs[i].Substring(langs[i].Length - Core.languageFileExt.Length).Equals(Core.languageFileExt)) // the the filename does not have the correct extension
                {
                    langs.RemoveAt(i);
                    i--;
                    continue;
                }

                if (langs[i].ToLower().Equals("en-gb" + Core.languageFileExt)) // if we find british english (if we dont find it, we want to expose the buildin Language as british english)
                {
                    foundBritishEnglish = true;
                    britishEnglishIndex = i;
                }

            }

            lbox_langs.Items.Add(Core.getString("en-gb")); // we will always have english as a language, and since it is a common language, it should be at the top.
            
            for (int i = 0;i < langs.Count; i++)
            {
                if (foundBritishEnglish && i == britishEnglishIndex) continue;

                string lang = langs[i].Substring(0, langs[i].Length - Core.languageFileExt.Length);

                lbox_langs.Items.Add(Core.getString(lang));
            }

            loadLanguage();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            // save language in config file
            try
            {
                string[] lines = Core.readAllLines(Core.configFile, 500 * 1024, Program.console);
                if (lines == null)
                {
                    return;
                }

                string lineToUpdate = lines.FirstOrDefault(line => line.StartsWith($"language:", StringComparison.OrdinalIgnoreCase));

                if (lineToUpdate != null)
                {
                    // Update the value in the found line
                    int index = Array.IndexOf(lines, lineToUpdate);
                    lines[index] = "language:" + getLanguageFromIndex(lbox_langs.SelectedIndex);

                    File.WriteAllLines(Core.configFile, lines);
                }
                else
                {
                    File.AppendAllText(Core.configFile, "language:" + getLanguageFromIndex(lbox_langs.SelectedIndex));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }



            Core.output(Core.getString("restartprog"), Core.getString("restartprog_title"), MessageBoxButtons.OK, MessageBoxIcon.Information, Program.console);
            this.Close();
        }

        private void loadLanguage()
        {
            this.Text = Core.getString("languageselection_windowtitle");
            lbl_selLang.Text = Core.getString("selectlanguage");
        }

        private string getLanguageFromIndex(int index) // since we map english to id 0
        {
            if (index == 0)
            {
                return "en-gb";
            }

            int i = index - 1; // the index of the listbox != the index of the language list. so we must translate it


            if (foundBritishEnglish && i >= britishEnglishIndex) // if we hit the britithEnglish index, increasse the index again, because we found english and need to skip it, since we already have it on index 0
            {
                i++;
            }
            

            if (i >= langs.Count) // should never happen
            {
                throw new Exception();
            }

            return langs[i].Substring(0, langs[i].Length - Core.languageFileExt.Length);

        }
    }
}
