﻿namespace WolvenKit.Forms
{
    partial class frmScriptEditor
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scintillaControl = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // scintillaControl
            // 
            this.scintillaControl.CaretLineVisible = true;
            this.scintillaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaControl.Location = new System.Drawing.Point(0, 0);
            this.scintillaControl.Name = "scintillaControl";
            this.scintillaControl.Size = new System.Drawing.Size(784, 561);
            this.scintillaControl.TabIndex = 0;
            // 
            // frmScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.scintillaControl);
            this.Name = "frmScriptEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScriptEditor_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla scintillaControl;
    }
}