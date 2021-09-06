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
            this.txw2rig.Location = new System.Drawing.Point(35, 27);
            this.txw2rig.Name = "txw2rig";
            this.txw2rig.Size = new System.Drawing.Size(452, 20);
            this.txw2rig.TabIndex = 0;
            this.txw2rig.TextChanged += new System.EventHandler(this.txExecutablePath_TextChanged);
            // 
            // lblRig
            // 
            this.lblRig.AutoSize = true;
            this.lblRig.Location = new System.Drawing.Point(35, 9);
            this.lblRig.Name = "lblRig";
            this.lblRig.Size = new System.Drawing.Size(76, 13);
            this.lblRig.TabIndex = 1;
            this.lblRig.Text = "w2rig or w3fac";
            // 
            // btnBrowseRig
            // 
            this.btnBrowseRig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRig.Location = new System.Drawing.Point(493, 26);
            this.btnBrowseRig.Name = "btnBrowseRig";
            this.btnBrowseRig.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseRig.TabIndex = 2;
            this.btnBrowseRig.Text = "Browse...";
            this.btnBrowseRig.UseVisualStyleBackColor = true;
            this.btnBrowseRig.Click += new System.EventHandler(this.btnBrowseRig_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Location = new System.Drawing.Point(493, 179);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 3;
            this.btSave.Text = "Export";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btBrowseAnims
            // 
            this.btBrowseAnims.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowseAnims.Location = new System.Drawing.Point(493, 66);
            this.btBrowseAnims.Name = "btBrowseAnims";
            this.btBrowseAnims.Size = new System.Drawing.Size(75, 23);
            this.btBrowseAnims.TabIndex = 10;
            this.btBrowseAnims.Text = "Browse...";
            this.btBrowseAnims.UseVisualStyleBackColor = true;
            this.btBrowseAnims.Click += new System.EventHandler(this.btBrowseAnims_Click);
            // 
            // lblAnimSet
            // 
            this.lblAnimSet.AutoSize = true;
            this.lblAnimSet.Location = new System.Drawing.Point(35, 50);
            this.lblAnimSet.Name = "lblAnimSet";
            this.lblAnimSet.Size = new System.Drawing.Size(124, 13);
            this.lblAnimSet.TabIndex = 9;
            this.lblAnimSet.Text = "w2anims or w2cutscene:";
            // 
            // txw2anims
            // 
            this.txw2anims.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txw2anims.Location = new System.Drawing.Point(35, 67);
            this.txw2anims.Name = "txw2anims";
            this.txw2anims.Size = new System.Drawing.Size(451, 20);
            this.txw2anims.TabIndex = 8;
            this.txw2anims.TextChanged += new System.EventHandler(this.txWCC_Lite_TextChanged);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(402, 179);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 11;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // W3exeTickLBL
            // 
            this.W3exeTickLBL.AutoSize = true;
            this.W3exeTickLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.W3exeTickLBL.ForeColor = System.Drawing.Color.Red;
            this.W3exeTickLBL.Location = new System.Drawing.Point(17, 31);
            this.W3exeTickLBL.Name = "W3exeTickLBL";
            this.W3exeTickLBL.Size = new System.Drawing.Size(15, 13);
            this.W3exeTickLBL.TabIndex = 13;
            this.W3exeTickLBL.Text = "X";
            // 
            // WCCexeTickLBL
            // 
            this.WCCexeTickLBL.AutoSize = true;
            this.WCCexeTickLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WCCexeTickLBL.ForeColor = System.Drawing.Color.Red;
            this.WCCexeTickLBL.Location = new System.Drawing.Point(17, 71);
            this.WCCexeTickLBL.Name = "WCCexeTickLBL";
            this.WCCexeTickLBL.Size = new System.Drawing.Size(15, 13);
            this.WCCexeTickLBL.TabIndex = 14;
            this.WCCexeTickLBL.Text = "X";
            // 
            // comboBoxAnim
            // 
            this.comboBoxAnim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnim.FormattingEnabled = true;
            this.comboBoxAnim.Location = new System.Drawing.Point(38, 134);
            this.comboBoxAnim.Name = "comboBoxAnim";
            this.comboBoxAnim.Size = new System.Drawing.Size(448, 21);
            this.comboBoxAnim.TabIndex = 16;
            this.comboBoxAnim.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnim_SelectedIndexChanged);
            // 
            // radioButtonAnim1
            // 
            this.radioButtonAnim1.AutoSize = true;
            this.radioButtonAnim1.Checked = true;
            this.radioButtonAnim1.Location = new System.Drawing.Point(37, 102);
            this.radioButtonAnim1.Name = "radioButtonAnim1";
            this.radioButtonAnim1.Size = new System.Drawing.Size(104, 17);
            this.radioButtonAnim1.TabIndex = 17;
            this.radioButtonAnim1.TabStop = true;
            this.radioButtonAnim1.Text = "Select Animation";
            this.radioButtonAnim1.UseVisualStyleBackColor = true;
            // 
            // radioButtonAnim2
            // 
            this.radioButtonAnim2.AutoSize = true;
            this.radioButtonAnim2.Location = new System.Drawing.Point(146, 102);
            this.radioButtonAnim2.Name = "radioButtonAnim2";
            this.radioButtonAnim2.Size = new System.Drawing.Size(109, 17);
            this.radioButtonAnim2.TabIndex = 18;
            this.radioButtonAnim2.Text = "Full Animation Set";
            this.radioButtonAnim2.UseVisualStyleBackColor = true;
            // 
            // frmAnims
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(598, 214);
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