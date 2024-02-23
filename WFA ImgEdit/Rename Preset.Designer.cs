namespace WFA_ImgEdit
{
    partial class Rename_Preset
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rename_Preset));
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.tbx_presetName = new System.Windows.Forms.TextBox();
            this.lbl_presetName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(147, 57);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(120, 23);
            this.btn_apply.TabIndex = 0;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(12, 57);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 23);
            this.btn_cancel.TabIndex = 0;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // tbx_presetName
            // 
            this.tbx_presetName.Location = new System.Drawing.Point(12, 31);
            this.tbx_presetName.Name = "tbx_presetName";
            this.tbx_presetName.Size = new System.Drawing.Size(255, 20);
            this.tbx_presetName.TabIndex = 1;
            // 
            // lbl_presetName
            // 
            this.lbl_presetName.AutoSize = true;
            this.lbl_presetName.Location = new System.Drawing.Point(13, 12);
            this.lbl_presetName.Name = "lbl_presetName";
            this.lbl_presetName.Size = new System.Drawing.Size(69, 13);
            this.lbl_presetName.TabIndex = 2;
            this.lbl_presetName.Text = "Preset name:";
            // 
            // Rename_Preset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 89);
            this.Controls.Add(this.lbl_presetName);
            this.Controls.Add(this.tbx_presetName);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(380, 180);
            this.Name = "Rename_Preset";
            this.Text = "Rename Preset";
            this.Load += new System.EventHandler(this.Rename_Preset_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox tbx_presetName;
        private System.Windows.Forms.Label lbl_presetName;
    }
}