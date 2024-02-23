using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageEditCore;

namespace WFA_ImgEdit
{
    public partial class Rename_Preset : Form
    {
        ImageOptions parrent;
        public Rename_Preset(ImageOptions parrent)
        {
            this.parrent = parrent;
            InitializeComponent();
            tbx_presetName.Text = parrent.setName;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            parrent.setName = tbx_presetName.Text;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Rename_Preset_Load(object sender, EventArgs e)
        {
            this.Text = Core.getString("renamepreset_title");
            lbl_presetName.Text = Core.getString("presetname");
            btn_apply.Text = Core.getString("apply");
            btn_cancel.Text = Core.getString("cancel");

        }
    }
}
