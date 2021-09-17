namespace WolvenKit.Forms
{
    partial class frmCR2WtoText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCR2WtoText));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnRun = new System.Windows.Forms.Button();
            this.rtfDescription = new System.Windows.Forms.RichTextBox();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.chkDumpYML = new System.Windows.Forms.CheckBox();
            this.chkLocalizedString = new System.Windows.Forms.CheckBox();
            this.labNumThreads = new System.Windows.Forms.Label();
            this.numThreads = new System.Windows.Forms.NumericUpDown();
            this.chkCreateFolders = new System.Windows.Forms.CheckBox();
            this.chkDumpEmbedded = new System.Windows.Forms.CheckBox();
            this.grpExistingFiles = new System.Windows.Forms.GroupBox();
            this.radExistingSkip = new System.Windows.Forms.RadioButton();
            this.radExistingOverwrite = new System.Windows.Forms.RadioButton();
            this.chkPrefixFileName = new System.Windows.Forms.CheckBox();
            this.chkDumpFCD = new System.Windows.Forms.CheckBox();
            this.chkDumpSDB = new System.Windows.Forms.CheckBox();
            this.labOutputFileMode = new System.Windows.Forms.Label();
            this.btnPickOutput = new System.Windows.Forms.Button();
            this.labOverwrite = new System.Windows.Forms.Label();
            this.labDumpOptions = new System.Windows.Forms.Label();
            this.labOutputFile = new System.Windows.Forms.Label();
            this.labPath = new System.Windows.Forms.Label();
            this.txtOutputDestination = new System.Windows.Forms.TextBox();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.grpRadioOutputMode = new System.Windows.Forms.GroupBox();
            this.radOutputModeSeparateFiles = new System.Windows.Forms.RadioButton();
            this.radOutputModeSingleFile = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.prgProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.dataStatus = new System.Windows.Forms.DataGridView();
            this.colAllFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNonCR2W = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchingFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessedFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSkipped = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExceptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            this.grpExistingFiles.SuspendLayout();
            this.grpRadioOutputMode.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Image = global::WolvenKit.Properties.Resources.Output_16x;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRun.Location = new System.Drawing.Point(260, 633);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(560, 28);
            this.btnRun.TabIndex = 30;
            this.btnRun.Text = "Run CR2W Dump";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // rtfDescription
            // 
            this.rtfDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfDescription.CausesValidation = false;
            this.rtfDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtfDescription.Font = new System.Drawing.Font("Calibri", 9F);
            this.rtfDescription.Location = new System.Drawing.Point(16, 15);
            this.rtfDescription.Margin = new System.Windows.Forms.Padding(4);
            this.rtfDescription.Name = "rtfDescription";
            this.rtfDescription.ReadOnly = true;
            this.rtfDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtfDescription.Size = new System.Drawing.Size(987, 209);
            this.rtfDescription.TabIndex = 20;
            this.rtfDescription.TabStop = false;
            this.rtfDescription.Text = resources.GetString("rtfDescription.Text");
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.chkDumpYML);
            this.pnlControls.Controls.Add(this.chkLocalizedString);
            this.pnlControls.Controls.Add(this.labNumThreads);
            this.pnlControls.Controls.Add(this.numThreads);
            this.pnlControls.Controls.Add(this.chkCreateFolders);
            this.pnlControls.Controls.Add(this.chkDumpEmbedded);
            this.pnlControls.Controls.Add(this.grpExistingFiles);
            this.pnlControls.Controls.Add(this.chkPrefixFileName);
            this.pnlControls.Controls.Add(this.chkDumpFCD);
            this.pnlControls.Controls.Add(this.chkDumpSDB);
            this.pnlControls.Controls.Add(this.labOutputFileMode);
            this.pnlControls.Controls.Add(this.btnPickOutput);
            this.pnlControls.Controls.Add(this.labOverwrite);
            this.pnlControls.Controls.Add(this.labDumpOptions);
            this.pnlControls.Controls.Add(this.labOutputFile);
            this.pnlControls.Controls.Add(this.labPath);
            this.pnlControls.Controls.Add(this.txtOutputDestination);
            this.pnlControls.Controls.Add(this.btnChoosePath);
            this.pnlControls.Controls.Add(this.txtPath);
            this.pnlControls.Controls.Add(this.grpRadioOutputMode);
            this.pnlControls.Location = new System.Drawing.Point(16, 236);
            this.pnlControls.Margin = new System.Windows.Forms.Padding(4);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(987, 386);
            this.pnlControls.TabIndex = 21;
            this.pnlControls.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlControls_Paint);
            // 
            // chkDumpYML
            // 
            this.chkDumpYML.AutoSize = true;
            this.chkDumpYML.Checked = true;
            this.chkDumpYML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDumpYML.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.985075F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkDumpYML.Location = new System.Drawing.Point(486, 156);
            this.chkDumpYML.Margin = new System.Windows.Forms.Padding(4);
            this.chkDumpYML.Name = "chkDumpYML";
            this.chkDumpYML.Size = new System.Drawing.Size(214, 20);
            this.chkDumpYML.TabIndex = 38;
            this.chkDumpYML.Text = "Dump YML for radish encoders";
            this.chkDumpYML.UseVisualStyleBackColor = true;
            this.chkDumpYML.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkLocalizedString
            // 
            this.chkLocalizedString.AutoSize = true;
            this.chkLocalizedString.Checked = true;
            this.chkLocalizedString.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLocalizedString.Location = new System.Drawing.Point(192, 282);
            this.chkLocalizedString.Margin = new System.Windows.Forms.Padding(4);
            this.chkLocalizedString.Name = "chkLocalizedString";
            this.chkLocalizedString.Size = new System.Drawing.Size(249, 20);
            this.chkLocalizedString.TabIndex = 15;
            this.chkLocalizedString.Text = "Dump localized strings instead of IDs";
            this.chkLocalizedString.UseVisualStyleBackColor = true;
            // 
            // labNumThreads
            // 
            this.labNumThreads.AutoSize = true;
            this.labNumThreads.Location = new System.Drawing.Point(407, 65);
            this.labNumThreads.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labNumThreads.Name = "labNumThreads";
            this.labNumThreads.Size = new System.Drawing.Size(121, 16);
            this.labNumThreads.TabIndex = 37;
            this.labNumThreads.Text = "Number of threads:";
            // 
            // numThreads
            // 
            this.numThreads.Location = new System.Drawing.Point(541, 63);
            this.numThreads.Margin = new System.Windows.Forms.Padding(4);
            this.numThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(48, 22);
            this.numThreads.TabIndex = 6;
            this.numThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numThreads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // chkCreateFolders
            // 
            this.chkCreateFolders.AutoSize = true;
            this.chkCreateFolders.Checked = true;
            this.chkCreateFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateFolders.Location = new System.Drawing.Point(192, 156);
            this.chkCreateFolders.Margin = new System.Windows.Forms.Padding(4);
            this.chkCreateFolders.Name = "chkCreateFolders";
            this.chkCreateFolders.Size = new System.Drawing.Size(191, 20);
            this.chkCreateFolders.TabIndex = 10;
            this.chkCreateFolders.Text = "Create intermediate folders";
            this.chkCreateFolders.UseVisualStyleBackColor = true;
            // 
            // chkDumpEmbedded
            // 
            this.chkDumpEmbedded.AutoSize = true;
            this.chkDumpEmbedded.Checked = true;
            this.chkDumpEmbedded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDumpEmbedded.Location = new System.Drawing.Point(192, 207);
            this.chkDumpEmbedded.Margin = new System.Windows.Forms.Padding(4);
            this.chkDumpEmbedded.Name = "chkDumpEmbedded";
            this.chkDumpEmbedded.Size = new System.Drawing.Size(147, 20);
            this.chkDumpEmbedded.TabIndex = 12;
            this.chkDumpEmbedded.Text = "List embedded files";
            this.chkDumpEmbedded.UseVisualStyleBackColor = true;
            // 
            // grpExistingFiles
            // 
            this.grpExistingFiles.Controls.Add(this.radExistingSkip);
            this.grpExistingFiles.Controls.Add(this.radExistingOverwrite);
            this.grpExistingFiles.Location = new System.Drawing.Point(192, 314);
            this.grpExistingFiles.Margin = new System.Windows.Forms.Padding(4);
            this.grpExistingFiles.Name = "grpExistingFiles";
            this.grpExistingFiles.Padding = new System.Windows.Forms.Padding(4);
            this.grpExistingFiles.Size = new System.Drawing.Size(189, 69);
            this.grpExistingFiles.TabIndex = 20;
            this.grpExistingFiles.TabStop = false;
            // 
            // radExistingSkip
            // 
            this.radExistingSkip.AutoSize = true;
            this.radExistingSkip.Location = new System.Drawing.Point(8, 41);
            this.radExistingSkip.Margin = new System.Windows.Forms.Padding(4);
            this.radExistingSkip.Name = "radExistingSkip";
            this.radExistingSkip.Size = new System.Drawing.Size(131, 20);
            this.radExistingSkip.TabIndex = 22;
            this.radExistingSkip.TabStop = true;
            this.radExistingSkip.Text = "Skip existing files";
            this.radExistingSkip.UseVisualStyleBackColor = true;
            // 
            // radExistingOverwrite
            // 
            this.radExistingOverwrite.AutoSize = true;
            this.radExistingOverwrite.Checked = true;
            this.radExistingOverwrite.Location = new System.Drawing.Point(8, 15);
            this.radExistingOverwrite.Margin = new System.Windows.Forms.Padding(4);
            this.radExistingOverwrite.Name = "radExistingOverwrite";
            this.radExistingOverwrite.Size = new System.Drawing.Size(160, 20);
            this.radExistingOverwrite.TabIndex = 21;
            this.radExistingOverwrite.TabStop = true;
            this.radExistingOverwrite.Text = "Overwrite existing files";
            this.radExistingOverwrite.UseVisualStyleBackColor = true;
            // 
            // chkPrefixFileName
            // 
            this.chkPrefixFileName.AutoSize = true;
            this.chkPrefixFileName.Checked = true;
            this.chkPrefixFileName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrefixFileName.Location = new System.Drawing.Point(192, 182);
            this.chkPrefixFileName.Margin = new System.Windows.Forms.Padding(4);
            this.chkPrefixFileName.Name = "chkPrefixFileName";
            this.chkPrefixFileName.Size = new System.Drawing.Size(202, 20);
            this.chkPrefixFileName.TabIndex = 11;
            this.chkPrefixFileName.Text = "Prefix each line with file name";
            this.chkPrefixFileName.UseVisualStyleBackColor = true;
            // 
            // chkDumpFCD
            // 
            this.chkDumpFCD.AutoSize = true;
            this.chkDumpFCD.Location = new System.Drawing.Point(192, 257);
            this.chkDumpFCD.Margin = new System.Windows.Forms.Padding(4);
            this.chkDumpFCD.Name = "chkDumpFCD";
            this.chkDumpFCD.Size = new System.Drawing.Size(173, 20);
            this.chkDumpFCD.TabIndex = 14;
            this.chkDumpFCD.Text = "Dump flatCompiledData";
            this.chkDumpFCD.UseVisualStyleBackColor = true;
            // 
            // chkDumpSDB
            // 
            this.chkDumpSDB.AutoSize = true;
            this.chkDumpSDB.Checked = true;
            this.chkDumpSDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDumpSDB.Location = new System.Drawing.Point(192, 233);
            this.chkDumpSDB.Margin = new System.Windows.Forms.Padding(4);
            this.chkDumpSDB.Name = "chkDumpSDB";
            this.chkDumpSDB.Size = new System.Drawing.Size(219, 20);
            this.chkDumpSDB.TabIndex = 13;
            this.chkDumpSDB.Text = "Dump SharedDataBuffer buffers";
            this.chkDumpSDB.UseVisualStyleBackColor = true;
            // 
            // labOutputFileMode
            // 
            this.labOutputFileMode.AutoSize = true;
            this.labOutputFileMode.Location = new System.Drawing.Point(73, 65);
            this.labOutputFileMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labOutputFileMode.Name = "labOutputFileMode";
            this.labOutputFileMode.Size = new System.Drawing.Size(107, 16);
            this.labOutputFileMode.TabIndex = 30;
            this.labOutputFileMode.Text = "Output file mode:";
            this.labOutputFileMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPickOutput
            // 
            this.btnPickOutput.Location = new System.Drawing.Point(804, 114);
            this.btnPickOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnPickOutput.Name = "btnPickOutput";
            this.btnPickOutput.Size = new System.Drawing.Size(100, 28);
            this.btnPickOutput.TabIndex = 8;
            this.btnPickOutput.Text = "Set output";
            this.btnPickOutput.UseVisualStyleBackColor = true;
            this.btnPickOutput.Click += new System.EventHandler(this.btnPickOutput_Click);
            // 
            // labOverwrite
            // 
            this.labOverwrite.AutoSize = true;
            this.labOverwrite.Location = new System.Drawing.Point(83, 342);
            this.labOverwrite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labOverwrite.Name = "labOverwrite";
            this.labOverwrite.Size = new System.Drawing.Size(100, 16);
            this.labOverwrite.TabIndex = 22;
            this.labOverwrite.Text = "File overwriting:";
            this.labOverwrite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labDumpOptions
            // 
            this.labDumpOptions.AutoSize = true;
            this.labDumpOptions.Location = new System.Drawing.Point(89, 207);
            this.labDumpOptions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labDumpOptions.Name = "labDumpOptions";
            this.labDumpOptions.Size = new System.Drawing.Size(94, 16);
            this.labDumpOptions.TabIndex = 22;
            this.labDumpOptions.Text = "Dump options:";
            this.labDumpOptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labOutputFile
            // 
            this.labOutputFile.AutoSize = true;
            this.labOutputFile.Location = new System.Drawing.Point(73, 119);
            this.labOutputFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labOutputFile.Name = "labOutputFile";
            this.labOutputFile.Size = new System.Drawing.Size(105, 16);
            this.labOutputFile.TabIndex = 23;
            this.labOutputFile.Text = "Output results to:";
            this.labOutputFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labPath
            // 
            this.labPath.AutoSize = true;
            this.labPath.Location = new System.Drawing.Point(92, 17);
            this.labPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labPath.Name = "labPath";
            this.labPath.Size = new System.Drawing.Size(91, 16);
            this.labPath.TabIndex = 21;
            this.labPath.Text = "Source folder:";
            this.labPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOutputDestination
            // 
            this.txtOutputDestination.Location = new System.Drawing.Point(192, 116);
            this.txtOutputDestination.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputDestination.Name = "txtOutputDestination";
            this.txtOutputDestination.Size = new System.Drawing.Size(603, 22);
            this.txtOutputDestination.TabIndex = 7;
            this.txtOutputDestination.TextChanged += new System.EventHandler(this.txtOutputDestination_TextChanged);
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(804, 14);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(100, 25);
            this.btnChoosePath.TabIndex = 2;
            this.btnChoosePath.Text = "Select path";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(192, 14);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(603, 22);
            this.txtPath.TabIndex = 1;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // grpRadioOutputMode
            // 
            this.grpRadioOutputMode.Controls.Add(this.radOutputModeSeparateFiles);
            this.grpRadioOutputMode.Controls.Add(this.radOutputModeSingleFile);
            this.grpRadioOutputMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpRadioOutputMode.Location = new System.Drawing.Point(192, 41);
            this.grpRadioOutputMode.Margin = new System.Windows.Forms.Padding(4);
            this.grpRadioOutputMode.Name = "grpRadioOutputMode";
            this.grpRadioOutputMode.Padding = new System.Windows.Forms.Padding(4);
            this.grpRadioOutputMode.Size = new System.Drawing.Size(189, 63);
            this.grpRadioOutputMode.TabIndex = 3;
            this.grpRadioOutputMode.TabStop = false;
            // 
            // radOutputModeSeparateFiles
            // 
            this.radOutputModeSeparateFiles.AutoSize = true;
            this.radOutputModeSeparateFiles.Checked = true;
            this.radOutputModeSeparateFiles.Location = new System.Drawing.Point(8, 14);
            this.radOutputModeSeparateFiles.Margin = new System.Windows.Forms.Padding(4);
            this.radOutputModeSeparateFiles.Name = "radOutputModeSeparateFiles";
            this.radOutputModeSeparateFiles.Size = new System.Drawing.Size(161, 20);
            this.radOutputModeSeparateFiles.TabIndex = 4;
            this.radOutputModeSeparateFiles.TabStop = true;
            this.radOutputModeSeparateFiles.Text = "One file per source file";
            this.radOutputModeSeparateFiles.UseVisualStyleBackColor = true;
            this.radOutputModeSeparateFiles.CheckedChanged += new System.EventHandler(this.RadioOutputModeChanged);
            // 
            // radOutputModeSingleFile
            // 
            this.radOutputModeSingleFile.AutoSize = true;
            this.radOutputModeSingleFile.Location = new System.Drawing.Point(8, 36);
            this.radOutputModeSingleFile.Margin = new System.Windows.Forms.Padding(4);
            this.radOutputModeSingleFile.Name = "radOutputModeSingleFile";
            this.radOutputModeSingleFile.Size = new System.Drawing.Size(87, 20);
            this.radOutputModeSingleFile.TabIndex = 5;
            this.radOutputModeSingleFile.Text = "Single file";
            this.radOutputModeSingleFile.UseVisualStyleBackColor = true;
            this.radOutputModeSingleFile.CheckedChanged += new System.EventHandler(this.RadioOutputModeChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prgProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 1026);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1019, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // prgProgressBar
            // 
            this.prgProgressBar.Name = "prgProgressBar";
            this.prgProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.prgProgressBar.Size = new System.Drawing.Size(1013, 20);
            // 
            // dataStatus
            // 
            this.dataStatus.AllowUserToAddRows = false;
            this.dataStatus.AllowUserToDeleteRows = false;
            this.dataStatus.AllowUserToResizeColumns = false;
            this.dataStatus.AllowUserToResizeRows = false;
            this.dataStatus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAllFiles,
            this.colNonCR2W,
            this.colMatchingFiles,
            this.colProcessedFiles,
            this.colSkipped,
            this.colExceptions});
            this.dataStatus.Location = new System.Drawing.Point(260, 674);
            this.dataStatus.Margin = new System.Windows.Forms.Padding(4);
            this.dataStatus.MultiSelect = false;
            this.dataStatus.Name = "dataStatus";
            this.dataStatus.ReadOnly = true;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dataStatus.RowHeadersVisible = false;
            this.dataStatus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.Format = "N0";
            dataGridViewCellStyle15.NullValue = "-";
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.dataStatus.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dataStatus.RowTemplate.ReadOnly = true;
            this.dataStatus.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataStatus.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataStatus.ShowEditingIcon = false;
            this.dataStatus.Size = new System.Drawing.Size(560, 70);
            this.dataStatus.TabIndex = 31;
            this.dataStatus.TabStop = false;
            // 
            // colAllFiles
            // 
            this.colAllFiles.Frozen = true;
            this.colAllFiles.HeaderText = "All Files";
            this.colAllFiles.Name = "colAllFiles";
            this.colAllFiles.ReadOnly = true;
            this.colAllFiles.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAllFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAllFiles.Width = 70;
            // 
            // colNonCR2W
            // 
            this.colNonCR2W.Frozen = true;
            this.colNonCR2W.HeaderText = "Non-CR2W Files";
            this.colNonCR2W.Name = "colNonCR2W";
            this.colNonCR2W.ReadOnly = true;
            this.colNonCR2W.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNonCR2W.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNonCR2W.Width = 70;
            // 
            // colMatchingFiles
            // 
            this.colMatchingFiles.Frozen = true;
            this.colMatchingFiles.HeaderText = "Matching Files";
            this.colMatchingFiles.Name = "colMatchingFiles";
            this.colMatchingFiles.ReadOnly = true;
            this.colMatchingFiles.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMatchingFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMatchingFiles.Width = 70;
            // 
            // colProcessedFiles
            // 
            this.colProcessedFiles.Frozen = true;
            this.colProcessedFiles.HeaderText = "Processed Files";
            this.colProcessedFiles.Name = "colProcessedFiles";
            this.colProcessedFiles.ReadOnly = true;
            this.colProcessedFiles.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colProcessedFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProcessedFiles.Width = 70;
            // 
            // colSkipped
            // 
            this.colSkipped.Frozen = true;
            this.colSkipped.HeaderText = "Skipped Files";
            this.colSkipped.Name = "colSkipped";
            this.colSkipped.ReadOnly = true;
            this.colSkipped.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSkipped.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSkipped.Width = 70;
            // 
            // colExceptions
            // 
            this.colExceptions.Frozen = true;
            this.colExceptions.HeaderText = "Files with exceptions";
            this.colExceptions.Name = "colExceptions";
            this.colExceptions.ReadOnly = true;
            this.colExceptions.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colExceptions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colExceptions.Width = 70;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(16, 754);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(985, 278);
            this.txtLog.TabIndex = 32;
            this.txtLog.TabStop = false;
            this.txtLog.WordWrap = false;
            // 
            // frmCR2WtoText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 1052);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataStatus);
            this.Controls.Add(this.rtfDescription);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.pnlControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCR2WtoText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dump CR2W Files To Text";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCR2WtoText_FormClosing);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit();
            this.grpExistingFiles.ResumeLayout(false);
            this.grpExistingFiles.PerformLayout();
            this.grpRadioOutputMode.ResumeLayout(false);
            this.grpRadioOutputMode.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RichTextBox rtfDescription;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.CheckBox chkDumpFCD;
        private System.Windows.Forms.CheckBox chkDumpSDB;
        private System.Windows.Forms.Label labOutputFileMode;
        private System.Windows.Forms.Button btnPickOutput;
        private System.Windows.Forms.Label labDumpOptions;
        private System.Windows.Forms.Label labOutputFile;
        private System.Windows.Forms.Label labPath;
        private System.Windows.Forms.TextBox txtOutputDestination;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.GroupBox grpRadioOutputMode;
        private System.Windows.Forms.RadioButton radOutputModeSeparateFiles;
        private System.Windows.Forms.RadioButton radOutputModeSingleFile;
        private System.Windows.Forms.CheckBox chkPrefixFileName;
        private System.Windows.Forms.Label labOverwrite;
        private System.Windows.Forms.GroupBox grpExistingFiles;
        private System.Windows.Forms.RadioButton radExistingSkip;
        private System.Windows.Forms.RadioButton radExistingOverwrite;
        private System.Windows.Forms.CheckBox chkDumpEmbedded;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar prgProgressBar;
        private System.Windows.Forms.CheckBox chkCreateFolders;
        private System.Windows.Forms.DataGridView dataStatus;
        private System.Windows.Forms.Label labNumThreads;
        private System.Windows.Forms.NumericUpDown numThreads;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNonCR2W;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatchingFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessedFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSkipped;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExceptions;
        private System.Windows.Forms.CheckBox chkLocalizedString;
        private System.Windows.Forms.CheckBox chkDumpYML;
    }
}