﻿using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace WolvenKit
{
    partial class frmModExplorer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModExplorer));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createW2animsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportW2cutscenejsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportW2animsjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportw2rigjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportW3facjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportW3facposejsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportW3dyngjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.fastRenderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRelativePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsModDlcFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.assetBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dumpFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpChunksToXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpWccliteXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showFileInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.searchBox = new System.Windows.Forms.ToolStripTextBox();
            this.showhideButton = new System.Windows.Forms.ToolStripButton();
            this.ExpandBTN = new System.Windows.Forms.ToolStripButton();
            this.CollapseBTN = new System.Windows.Forms.ToolStripButton();
            this.resetfilesButton = new System.Windows.Forms.ToolStripButton();
            this.modexplorerSlave = new System.IO.FileSystemWatcher();
            this.treeListView = new BrightIdeasSoftware.TreeListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.exportW2entjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.searchstrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modexplorerSlave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createW2animsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.cookToolStripMenuItem,
            this.toolStripSeparator3,
            this.fastRenderToolStripMenuItem,
            this.toolStripSeparator5,
            this.removeFileToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.copyRelativePathToolStripMenuItem,
            this.markAsModDlcFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.assetBrowserToolStripMenuItem,
            this.addFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.dumpFileToolStripMenuItem,
            this.toolStripSeparator4,
            this.showFileInExplorerToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(197, 420);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            this.contextMenu.Opened += new System.EventHandler(this.contextMenu_Opened);
            // 
            // createW2animsToolStripMenuItem
            // 
            this.createW2animsToolStripMenuItem.Name = "createW2animsToolStripMenuItem";
            this.createW2animsToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.createW2animsToolStripMenuItem.Text = "Create w2anims";
            this.createW2animsToolStripMenuItem.Click += new System.EventHandler(this.createW2animsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportW2cutscenejsonToolStripMenuItem,
            this.exportW2animsjsonToolStripMenuItem,
            this.exportw2rigjsonToolStripMenuItem,
            this.exportW3facjsonToolStripMenuItem,
            this.exportW3facposejsonToolStripMenuItem,
            this.exportW3dyngjsonToolStripMenuItem,
            this.exportW2entjsonToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportW2cutscenejsonToolStripMenuItem
            // 
            this.exportW2cutscenejsonToolStripMenuItem.Name = "exportW2cutscenejsonToolStripMenuItem";
            this.exportW2cutscenejsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW2cutscenejsonToolStripMenuItem.Text = "Export w2cutscene.json";
            this.exportW2cutscenejsonToolStripMenuItem.Click += new System.EventHandler(this.exportW2cutscenejsonToolStripMenuItem_Click);
            // 
            // exportW2animsjsonToolStripMenuItem
            // 
            this.exportW2animsjsonToolStripMenuItem.Name = "exportW2animsjsonToolStripMenuItem";
            this.exportW2animsjsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW2animsjsonToolStripMenuItem.Text = "Export w2anims.json";
            this.exportW2animsjsonToolStripMenuItem.Visible = false;
            this.exportW2animsjsonToolStripMenuItem.Click += new System.EventHandler(this.exportW2animsjsonToolStripMenuItem_Click);
            // 
            // exportw2rigjsonToolStripMenuItem
            // 
            this.exportw2rigjsonToolStripMenuItem.Name = "exportw2rigjsonToolStripMenuItem";
            this.exportw2rigjsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportw2rigjsonToolStripMenuItem.Text = "Export w2rig.json";
            this.exportw2rigjsonToolStripMenuItem.Visible = false;
            this.exportw2rigjsonToolStripMenuItem.Click += new System.EventHandler(this.exportw2rigjsonToolStripMenuItem_Click);
            // 
            // exportW3facjsonToolStripMenuItem
            // 
            this.exportW3facjsonToolStripMenuItem.Name = "exportW3facjsonToolStripMenuItem";
            this.exportW3facjsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW3facjsonToolStripMenuItem.Text = "Export w3fac.json";
            this.exportW3facjsonToolStripMenuItem.Click += new System.EventHandler(this.exportW3facjsonToolStripMenuItem_Click);
            // 
            // exportW3facposejsonToolStripMenuItem
            // 
            this.exportW3facposejsonToolStripMenuItem.Name = "exportW3facposejsonToolStripMenuItem";
            this.exportW3facposejsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW3facposejsonToolStripMenuItem.Text = "Export w3fac.pose.json";
            this.exportW3facposejsonToolStripMenuItem.Click += new System.EventHandler(this.exportW3facposejsonToolStripMenuItem_Click);
            // 
            // exportW3dyngjsonToolStripMenuItem
            // 
            this.exportW3dyngjsonToolStripMenuItem.Name = "exportW3dyngjsonToolStripMenuItem";
            this.exportW3dyngjsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW3dyngjsonToolStripMenuItem.Text = "Export w3dyng.json";
            this.exportW3dyngjsonToolStripMenuItem.Click += new System.EventHandler(this.exportW3dyngjsonToolStripMenuItem_Click);
            // 
            // cookToolStripMenuItem
            // 
            this.cookToolStripMenuItem.Enabled = false;
            this.cookToolStripMenuItem.Name = "cookToolStripMenuItem";
            this.cookToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.cookToolStripMenuItem.Text = "Cook files in directory";
            this.cookToolStripMenuItem.Click += new System.EventHandler(this.cookToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // fastRenderToolStripMenuItem
            // 
            this.fastRenderToolStripMenuItem.Name = "fastRenderToolStripMenuItem";
            this.fastRenderToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.fastRenderToolStripMenuItem.Text = "Fast Render";
            this.fastRenderToolStripMenuItem.Click += new System.EventHandler(this.fastRenderToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(193, 6);
            // 
            // removeFileToolStripMenuItem
            // 
            this.removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            this.removeFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.removeFileToolStripMenuItem.Text = "Delete";
            this.removeFileToolStripMenuItem.Click += new System.EventHandler(this.removeFileToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // copyRelativePathToolStripMenuItem
            // 
            this.copyRelativePathToolStripMenuItem.Name = "copyRelativePathToolStripMenuItem";
            this.copyRelativePathToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.copyRelativePathToolStripMenuItem.Text = "Copy relative path";
            this.copyRelativePathToolStripMenuItem.Click += new System.EventHandler(this.copyRelativePathToolStripMenuItem_Click);
            // 
            // markAsModDlcFileToolStripMenuItem
            // 
            this.markAsModDlcFileToolStripMenuItem.Enabled = false;
            this.markAsModDlcFileToolStripMenuItem.Name = "markAsModDlcFileToolStripMenuItem";
            this.markAsModDlcFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.markAsModDlcFileToolStripMenuItem.Text = "Mark as [Mod/Dlc] file";
            this.markAsModDlcFileToolStripMenuItem.Click += new System.EventHandler(this.markAsModDlcFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // assetBrowserToolStripMenuItem
            // 
            this.assetBrowserToolStripMenuItem.Image = global::WolvenKit.Properties.Resources.AddNodefromFile_354;
            this.assetBrowserToolStripMenuItem.Name = "assetBrowserToolStripMenuItem";
            this.assetBrowserToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.assetBrowserToolStripMenuItem.Text = "Asset Browser here";
            this.assetBrowserToolStripMenuItem.Click += new System.EventHandler(this.openAssetBrowserToolStripMenuItem_Click);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Image = global::WolvenKit.Properties.Resources.AddNodefromFile_354;
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.addFileToolStripMenuItem.Text = "Add File";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // dumpFileToolStripMenuItem
            // 
            this.dumpFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpXMLToolStripMenuItem,
            this.dumpChunksToXMLToolStripMenuItem,
            this.dumpWccliteXMLToolStripMenuItem});
            this.dumpFileToolStripMenuItem.Name = "dumpFileToolStripMenuItem";
            this.dumpFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.dumpFileToolStripMenuItem.Text = "Dump File";
            // 
            // dumpXMLToolStripMenuItem
            // 
            this.dumpXMLToolStripMenuItem.Name = "dumpXMLToolStripMenuItem";
            this.dumpXMLToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.dumpXMLToolStripMenuItem.Text = "Dump Header to XML";
            this.dumpXMLToolStripMenuItem.Click += new System.EventHandler(this.dumpXMLToolStripMenuItem_Click);
            // 
            // dumpChunksToXMLToolStripMenuItem
            // 
            this.dumpChunksToXMLToolStripMenuItem.Name = "dumpChunksToXMLToolStripMenuItem";
            this.dumpChunksToXMLToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.dumpChunksToXMLToolStripMenuItem.Text = "Dump Chunks to XML";
            this.dumpChunksToXMLToolStripMenuItem.Click += new System.EventHandler(this.dumpChunksToXMLToolStripMenuItem_Click);
            // 
            // dumpWccliteXMLToolStripMenuItem
            // 
            this.dumpWccliteXMLToolStripMenuItem.Name = "dumpWccliteXMLToolStripMenuItem";
            this.dumpWccliteXMLToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.dumpWccliteXMLToolStripMenuItem.Text = "Dump Wcc_lite XML";
            this.dumpWccliteXMLToolStripMenuItem.Click += new System.EventHandler(this.dumpWccliteXMLToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(193, 6);
            // 
            // showFileInExplorerToolStripMenuItem
            // 
            this.showFileInExplorerToolStripMenuItem.Name = "showFileInExplorerToolStripMenuItem";
            this.showFileInExplorerToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.showFileInExplorerToolStripMenuItem.Text = "Show file in explorer";
            this.showFileInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showFileInExplorerToolStripMenuItem_Click);
            // 
            // searchstrip
            // 
            this.searchstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.searchstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.searchBox,
            this.showhideButton,
            this.ExpandBTN,
            this.CollapseBTN,
            this.resetfilesButton});
            this.searchstrip.Location = new System.Drawing.Point(0, 0);
            this.searchstrip.Name = "searchstrip";
            this.searchstrip.Size = new System.Drawing.Size(333, 27);
            this.searchstrip.TabIndex = 1;
            this.searchstrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(39, 24);
            this.toolStripLabel1.Text = "Filter: ";
            // 
            // searchBox
            // 
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(54, 27);
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // showhideButton
            // 
            this.showhideButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.showhideButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showhideButton.Image = global::WolvenKit.Properties.Resources.LayerGroupVisibled;
            this.showhideButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showhideButton.Name = "showhideButton";
            this.showhideButton.Size = new System.Drawing.Size(24, 24);
            this.showhideButton.Text = "Show/Hide folders";
            this.showhideButton.ToolTipText = "Show/Hide folders";
            this.showhideButton.Click += new System.EventHandler(this.showhideButton_Click);
            // 
            // ExpandBTN
            // 
            this.ExpandBTN.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ExpandBTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExpandBTN.Image = ((System.Drawing.Image)(resources.GetObject("ExpandBTN.Image")));
            this.ExpandBTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExpandBTN.Name = "ExpandBTN";
            this.ExpandBTN.Size = new System.Drawing.Size(24, 24);
            this.ExpandBTN.Text = "Expand all";
            this.ExpandBTN.Click += new System.EventHandler(this.ExpandBTN_Click);
            // 
            // CollapseBTN
            // 
            this.CollapseBTN.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CollapseBTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CollapseBTN.Image = ((System.Drawing.Image)(resources.GetObject("CollapseBTN.Image")));
            this.CollapseBTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CollapseBTN.Name = "CollapseBTN";
            this.CollapseBTN.Size = new System.Drawing.Size(24, 24);
            this.CollapseBTN.Text = "Collapse all";
            this.CollapseBTN.ToolTipText = "Collapse all";
            this.CollapseBTN.Click += new System.EventHandler(this.CollapseBTN_Click);
            // 
            // resetfilesButton
            // 
            this.resetfilesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.resetfilesButton.Image = global::WolvenKit.Properties.Resources.ExitIcon;
            this.resetfilesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetfilesButton.Name = "resetfilesButton";
            this.resetfilesButton.Size = new System.Drawing.Size(24, 24);
            this.resetfilesButton.Text = "Reset filelist";
            this.resetfilesButton.Click += new System.EventHandler(this.UpdatefilelistButtonClick);
            // 
            // modexplorerSlave
            // 
            this.modexplorerSlave.EnableRaisingEvents = true;
            this.modexplorerSlave.IncludeSubdirectories = true;
            this.modexplorerSlave.SynchronizingObject = this;
            this.modexplorerSlave.Created += new System.IO.FileSystemEventHandler(this.FileChanges_Detected);
            this.modexplorerSlave.Deleted += new System.IO.FileSystemEventHandler(this.FileChanges_Detected);
            this.modexplorerSlave.Renamed += new System.IO.RenamedEventHandler(this.FileChanges_Detected);
            // 
            // treeListView
            // 
            this.treeListView.AllColumns.Add(this.olvColumnName);
            this.treeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName});
            this.treeListView.ContextMenuStrip = this.contextMenu;
            this.treeListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView.HideSelection = false;
            this.treeListView.Location = new System.Drawing.Point(0, 27);
            this.treeListView.Name = "treeListView";
            this.treeListView.ShowGroups = false;
            this.treeListView.Size = new System.Drawing.Size(333, 345);
            this.treeListView.TabIndex = 2;
            this.treeListView.UseCompatibleStateImageBehavior = false;
            this.treeListView.UseFiltering = true;
            this.treeListView.View = System.Windows.Forms.View.Details;
            this.treeListView.VirtualMode = true;
            this.treeListView.Expanded += new System.EventHandler<BrightIdeasSoftware.TreeBranchExpandedEventArgs>(this.treeListView_Expanded);
            this.treeListView.Collapsed += new System.EventHandler<BrightIdeasSoftware.TreeBranchCollapsedEventArgs>(this.treeListView_Collapsed);
            this.treeListView.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.treeListView_CellClick);
            this.treeListView.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.treeListView_CellRightClick);
            this.treeListView.ItemActivate += new System.EventHandler(this.treeListView_ItemActivate);
            this.treeListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modFileList_KeyDown);
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.FillsFreeSpace = true;
            this.olvColumnName.Text = "Name";
            // 
            // exportW2entjsonToolStripMenuItem
            // 
            this.exportW2entjsonToolStripMenuItem.Name = "exportW2entjsonToolStripMenuItem";
            this.exportW2entjsonToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exportW2entjsonToolStripMenuItem.Text = "Export w2ent.json";
            this.exportW2entjsonToolStripMenuItem.Click += new System.EventHandler(this.exportW2entjsonToolStripMenuItem_Click);
            // 
            // frmModExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 372);
            this.Controls.Add(this.treeListView);
            this.Controls.Add(this.searchstrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "frmModExplorer";
            this.Text = "Mod Explorer";
            this.Shown += new System.EventHandler(this.frmModExplorer_Shown);
            this.contextMenu.ResumeLayout(false);
            this.searchstrip.ResumeLayout(false);
            this.searchstrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modexplorerSlave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem removeFileToolStripMenuItem;
        private ToolStripMenuItem assetBrowserToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStrip searchstrip;
        private ToolStripTextBox searchBox;
        private ToolStripButton showhideButton;
        private ToolStripButton resetfilesButton;
        private FileSystemWatcher modexplorerSlave;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem showFileInExplorerToolStripMenuItem;
        private ToolStripButton ExpandBTN;
        private ToolStripButton CollapseBTN;
        private ToolStripMenuItem copyRelativePathToolStripMenuItem;
        private ToolStripMenuItem markAsModDlcFileToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private ToolStripMenuItem addFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem cookToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem dumpXMLToolStripMenuItem;
        private ToolStripMenuItem dumpChunksToXMLToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem createW2animsToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem exportW2cutscenejsonToolStripMenuItem;
        private ToolStripMenuItem exportW2animsjsonToolStripMenuItem;
        private ToolStripMenuItem exportw2rigjsonToolStripMenuItem;
        private ToolStripMenuItem exportW3facjsonToolStripMenuItem;
        private ToolStripMenuItem exportW3facposejsonToolStripMenuItem;
        private ToolStripMenuItem dumpFileToolStripMenuItem;
        private ToolStripMenuItem dumpWccliteXMLToolStripMenuItem;
        private ToolStripMenuItem fastRenderToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private BrightIdeasSoftware.TreeListView treeListView;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private ToolStripMenuItem exportW3dyngjsonToolStripMenuItem;
        private ToolStripMenuItem exportW2entjsonToolStripMenuItem;
    }
}