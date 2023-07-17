using System.ComponentModel;
using System.Windows.Forms;

namespace WolvenKit
{
    partial class frmAnims
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnims));
            this.txw2rig = new System.Windows.Forms.TextBox();
            this.lblRig = new System.Windows.Forms.Label();
            this.btnBrowseRig = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btBrowseAnims = new System.Windows.Forms.Button();
            this.lblAnimSet = new System.Windows.Forms.Label();
            this.txw2anims = new System.Windows.Forms.TextBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.W3exeTickLBL = new System.Windows.Forms.Label();
            this.WCCexeTickLBL = new System.Windows.Forms.Label();
            this.comboBoxAnim = new System.Windows.Forms.ComboBox();
            this.radioButtonAnim1 = new System.Windows.Forms.RadioButton();
            this.radioButtonAnim2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txw2rig
            // 
            this.txw2rig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txw2rig.Location = new System.Drawing.Point(47, 33);
            this.txw2rig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txw2rig.Name = "txw2rig";
            this.txw2rig.Size = new System.Drawing.Size(601, 22);
            this.txw2rig.TabIndex = 0;
            this.txw2rig.TextChanged += new System.EventHandler(this.txExecutablePath_TextChanged);
            // 
            // lblRig
            // 
            this.lblRig.AutoSize = true;
            this.lblRig.Location = new System.Drawing.Point(47, 11);
            this.lblRig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRig.Name = "lblRig";
            this.lblRig.Size = new System.Drawing.Size(217, 17);
            this.lblRig.TabIndex = 1;
            this.lblRig.Text = "w2rig or w3fac (for w2anims only)";
            // 
            // btnBrowseRig
            // 
            this.btnBrowseRig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRig.Location = new System.Drawing.Point(657, 32);
            this.btnBrowseRig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseRig.Name = "btnBrowseRig";
            this.btnBrowseRig.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseRig.TabIndex = 2;
            this.btnBrowseRig.Text = "Browse...";
            this.btnBrowseRig.UseVisualStyleBackColor = true;
            this.btnBrowseRig.Click += new System.EventHandler(this.btnBrowseRig_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Location = new System.Drawing.Point(657, 220);
            this.btSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(100, 28);
            this.btSave.TabIndex = 3;
            this.btSave.Text = "Export";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btBrowseAnims
            // 
            this.btBrowseAnims.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowseAnims.Location = new System.Drawing.Point(657, 81);
            this.btBrowseAnims.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btBrowseAnims.Name = "btBrowseAnims";
            this.btBrowseAnims.Size = new System.Drawing.Size(100, 28);
            this.btBrowseAnims.TabIndex = 10;
            this.btBrowseAnims.Text = "Browse...";
            this.btBrowseAnims.UseVisualStyleBackColor = true;
            this.btBrowseAnims.Click += new System.EventHandler(this.btBrowseAnims_Click);
            // 
            // lblAnimSet
            // 
            this.lblAnimSet.AutoSize = true;
            this.lblAnimSet.Location = new System.Drawing.Point(47, 62);
            this.lblAnimSet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimSet.Name = "lblAnimSet";
            this.lblAnimSet.Size = new System.Drawing.Size(161, 17);
            this.lblAnimSet.TabIndex = 9;
            this.lblAnimSet.Text = "w2anims or w2cutscene:";
            // 
            // txw2anims
            // 
            this.txw2anims.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txw2anims.Location = new System.Drawing.Point(47, 82);
            this.txw2anims.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txw2anims.Name = "txw2anims";
            this.txw2anims.Size = new System.Drawing.Size(600, 22);
            this.txw2anims.TabIndex = 8;
            this.txw2anims.TextChanged += new System.EventHandler(this.txWCC_Lite_TextChanged);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(536, 220);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(100, 28);
            this.btCancel.TabIndex = 11;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // W3exeTickLBL
            // 
            this.W3exeTickLBL.AutoSize = true;
            this.W3exeTickLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.W3exeTickLBL.ForeColor = System.Drawing.Color.Red;
            this.W3exeTickLBL.Location = new System.Drawing.Point(23, 38);
            this.W3exeTickLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.W3exeTickLBL.Name = "W3exeTickLBL";
            this.W3exeTickLBL.Size = new System.Drawing.Size(18, 17);
            this.W3exeTickLBL.TabIndex = 13;
            this.W3exeTickLBL.Text = "X";
            // 
            // WCCexeTickLBL
            // 
            this.WCCexeTickLBL.AutoSize = true;
            this.WCCexeTickLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WCCexeTickLBL.ForeColor = System.Drawing.Color.Red;
            this.WCCexeTickLBL.Location = new System.Drawing.Point(23, 87);
            this.WCCexeTickLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WCCexeTickLBL.Name = "WCCexeTickLBL";
            this.WCCexeTickLBL.Size = new System.Drawing.Size(18, 17);
            this.WCCexeTickLBL.TabIndex = 14;
            this.WCCexeTickLBL.Text = "X";
            // 
            // comboBoxAnim
            // 
            this.comboBoxAnim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnim.FormattingEnabled = true;
            this.comboBoxAnim.Location = new System.Drawing.Point(51, 165);
            this.comboBoxAnim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxAnim.Name = "comboBoxAnim";
            this.comboBoxAnim.Size = new System.Drawing.Size(596, 24);
            this.comboBoxAnim.TabIndex = 16;
            this.comboBoxAnim.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnim_SelectedIndexChanged);
            // 
            // radioButtonAnim1
            // 
            this.radioButtonAnim1.AutoSize = true;
            this.radioButtonAnim1.Checked = true;
            this.radioButtonAnim1.Location = new System.Drawing.Point(49, 126);
            this.radioButtonAnim1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButtonAnim1.Name = "radioButtonAnim1";
            this.radioButtonAnim1.Size = new System.Drawing.Size(134, 21);
            this.radioButtonAnim1.TabIndex = 17;
            this.radioButtonAnim1.TabStop = true;
            this.radioButtonAnim1.Text = "Select Animation";
            this.radioButtonAnim1.UseVisualStyleBackColor = true;
            // 
            // radioButtonAnim2
            // 
            this.radioButtonAnim2.AutoSize = true;
            this.radioButtonAnim2.Location = new System.Drawing.Point(195, 126);
            this.radioButtonAnim2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButtonAnim2.Name = "radioButtonAnim2";
            this.radioButtonAnim2.Size = new System.Drawing.Size(142, 21);
            this.radioButtonAnim2.TabIndex = 18;
            this.radioButtonAnim2.Text = "Full Animation Set";
            this.radioButtonAnim2.UseVisualStyleBackColor = true;
            // 
            // frmAnims
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(797, 263);
            this.Controls.Add(this.radioButtonAnim2);
            this.Controls.Add(this.radioButtonAnim1);
            this.Controls.Add(this.comboBoxAnim);
            this.Controls.Add(this.WCCexeTickLBL);
            this.Controls.Add(this.W3exeTickLBL);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btBrowseAnims);
            this.Controls.Add(this.lblAnimSet);
            this.Controls.Add(this.txw2anims);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btnBrowseRig);
            this.Controls.Add(this.lblRig);
            this.Controls.Add(this.txw2rig);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmAnims";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Animation File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txw2rig;
        private Label lblRig;
        private Button btnBrowseRig;
        private Button btSave;
        private Button btBrowseAnims;
        private Label lblAnimSet;
        private TextBox txw2anims;
        private Button btCancel;
        private Label W3exeTickLBL;
        private Label WCCexeTickLBL;
        private ComboBox comboBoxAnim;
        private RadioButton radioButtonAnim1;
        private RadioButton radioButtonAnim2;
    }
}