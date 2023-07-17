﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WolvenKit.Bundles;
using WolvenKit.Common.Services;
using WolvenKit.Common.Wcc;
using WolvenKit.Services;

namespace WolvenKit
{
    using BrightIdeasSoftware;
    using Common;
    using IrrlichtLime;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading.Tasks;
    using UsefulThings;
    using WolvenKit.App;
    using WolvenKit.App.Commands;
    using WolvenKit.App.ViewModels;
    using WolvenKit.Common.Extensions;
    using WolvenKit.Common.Model;
    using WolvenKit.CR2W;
    using WolvenKit.Extensions;
    using WolvenKit.Render;
    using WolvenKit.Render.Animation;

    public partial class frmModExplorer : DockContent, IThemedContent
    {
        private readonly ModExplorerViewModel vm;
        private bool usecachedNodeList;

        public frmModExplorer(ILoggerService logger)
        {
            // initialize Viewmodel
            vm = MockKernel.Get().GetModExplorerModel();
            vm.PropertyChanged += ViewModel_PropertyChanged;
            vm.UpdateMonitoringRequest += (sender, e) => this.ViewModel_UpdateMonitoringRequest(e);

            InitializeComponent();
            ApplyCustomTheme();

            // Init ObjectListView
            this.treeListView.CanExpandGetter = delegate (object x) {
                return (x is DirectoryInfo) && vm.IsTreeview && (x as DirectoryInfo).HasFilesOrFolders();
            };
            this.treeListView.ChildrenGetter = delegate (object x) {
                DirectoryInfo dir = (DirectoryInfo)x;
                return dir.Exists ? new ArrayList(dir.GetFileSystemInfos()
                    .Where(_ => _.Extension != ".bat")
                    .ToArray()) : new ArrayList();
            };
            treeListView.SmallImageList = new ImageList();
            this.olvColumnName.ImageGetter = delegate (object row) {
                FileSystemInfo file = (FileSystemInfo)row;
                string extension = this.GetFileExtension(file);
                if (!this.treeListView.SmallImageList.Images.ContainsKey(extension))
                {
                    Image smallImage = this.GetSmallIconForFileType(extension);
                    this.treeListView.SmallImageList.Images.Add(extension, smallImage);
                }
                return extension;
            };


            // Update the TreeView
            vm.RepopulateTreeView();
            treeListView.ExpandAll();
        }

        private void ViewModel_UpdateMonitoringRequest(UpdateMonitoringEventArgs e)
        {
            if (e.Monitor)
                ResumeMonitoring();
            else
                PauseMonitoring();
        }

        #region Properties
        public W3Mod ActiveMod
        {
            get => MainController.Get().ActiveMod;
            set => MainController.Get().ActiveMod = value;
        }

        public event EventHandler<RequestFileArgs> RequestAssetBrowser;


        public event EventHandler<RequestFileArgs> RequestFileOpen;
        public event EventHandler<RequestFileArgs> RequestFileRename;

        public event EventHandler<RequestFileArgs> RequestFastRender;
        #endregion


        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(vm.treenodes))
            {
                this.treeListView.SetObjects(vm.treenodes);
                UpdateTreeView();
            }
        }

        private void frmModExplorer_Shown(object sender, EventArgs e)
        {
            if (ActiveMod != null)
                modexplorerSlave.Path = ActiveMod.FileDirectory;
        }

        #region FileSystemWatcher
        public void PauseMonitoring()
        {
            modexplorerSlave.EnableRaisingEvents = false;
        }

        public void ResumeMonitoring()
        {
            if (ActiveMod != null)
            {
                modexplorerSlave.Path = ActiveMod.FileDirectory;
                modexplorerSlave.EnableRaisingEvents = true;
                usecachedNodeList = true;
                UpdateTreeView();
            }
        }
        public void StopMonitoringDirectory()
        {
            modexplorerSlave.Dispose();
        }
        private void FileChanges_Detected(object sender, RenamedEventArgs e)
        {

        }
        private void FileChanges_Detected(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    {
                        UpdateTreeView(e.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                    {
                        UpdateTreeView(e.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Renamed:
                    {
                        UpdateTreeView((e as RenamedEventArgs).OldFullPath);
                        break;
                    }
                case WatcherChangeTypes.Changed:
                case WatcherChangeTypes.All:
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Methods
        public void ApplyCustomTheme()
        {
            var theme = UIController.Get().GetTheme();
            UIController.Get().ToolStripExtender.SetStyle(searchstrip, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);

            this.treeListView.BackColor = theme.ColorPalette.ToolWindowTabSelectedInactive.Background;
            this.treeListView.ForeColor = theme.ColorPalette.CommandBarMenuDefault.Text;
            HeaderFormatStyle hfs = new HeaderFormatStyle()
            {
                Normal = new HeaderStateStyle()
                {
                    BackColor = theme.ColorPalette.DockTarget.Background,
                    ForeColor = theme.ColorPalette.CommandBarMenuDefault.Text,
                },
                Hot = new HeaderStateStyle()
                {
                    BackColor = theme.ColorPalette.OverflowButtonHovered.Background,
                    ForeColor = theme.ColorPalette.CommandBarMenuDefault.Text,
                },
                Pressed = new HeaderStateStyle()
                {
                    BackColor = theme.ColorPalette.CommandBarToolbarButtonPressed.Background,
                    ForeColor = theme.ColorPalette.CommandBarMenuDefault.Text,
                }
            };
            this.treeListView.HeaderFormatStyle = hfs;
            treeListView.UnfocusedSelectedBackColor = theme.ColorPalette.CommandBarToolbarButtonPressed.Background;


            this.searchBox.BackColor = theme.ColorPalette.ToolWindowCaptionButtonInactiveHovered.Background;
        }

        private void UpdateTreeView(params string[] nodesToUpdate)
        {
            if (MainController.Get().ActiveMod == null)
                return;

            // get branches to update
            var rootNodesToUpdate = new List<FileSystemInfo>();
            // if nodes are specified, update only these branches
            if (nodesToUpdate.Length > 0)
            {
                foreach (var node in nodesToUpdate)
                {
                    var splits = node.Substring(MainController.Get().ActiveMod.FileDirectory.Length + 1).Split(Path.DirectorySeparatorChar);
                    var rn = treeListView.TreeModel.RootObjects.OfType<FileSystemInfo>()
                        .FirstOrDefault(_ => _.Name == splits.First());
                    if (!rootNodesToUpdate.Contains(rn))
                        rootNodesToUpdate.Add(rn);
                }
            }
            // if nothing is specified, update all branches
            else
                rootNodesToUpdate = treeListView.TreeModel.RootObjects.OfType<FileSystemInfo>().ToList();


            // rebuild the branches
            foreach (var rootNode in rootNodesToUpdate)
            {
                if (rootNode == null)
                    return;
                // log expanded state
                if (usecachedNodeList)
                {
                    usecachedNodeList = false;
                }   
                else
                {
                    var branch = treeListView.TreeModel.GetBranch(rootNode);
                    var fbr = branch.Flatten();
                    var expandedNodes = fbr.OfType<FileSystemInfo>()
                        .Select(_ => _.GetParent().FullName)
                        .Where(_ => _ != rootNode.FullName)
                        .Distinct()
                        .ToList();

                    if (vm.ExpandedNodesDict.ContainsKey(rootNode.Name))
                        vm.ExpandedNodesDict[rootNode.Name] = expandedNodes;
                    else
                        vm.ExpandedNodesDict.Add(rootNode.Name, expandedNodes);
                }
                

                treeListView.RefreshObject(rootNode);

                // rebuild branch
                foreach (string fullpath in vm.ExpandedNodesDict[rootNode.Name])
                {
                    var count = treeListView.TreeModel.GetObjectCount();
                    for (int i = 0; i < count; i++)
                    {
                        var nthobj = treeListView.TreeModel.GetNthObject(i);
                        if ((nthobj as FileSystemInfo).FullName == fullpath)
                        {
                            treeListView.Expand(nthobj);
                            break;
                        }
                    }
                }
            }
        }

        private const string openDirImageKey = "<ODIR>";
        private const string closedDirImageKey = "<CDIR>";
        private Image GetSmallIconForFileType(string extension)
        {
            extension = extension.TrimStart('.');
            switch (extension)
            {
                case "w2ent": return WolvenKit.Properties.Resources.w2ent;
                case "w2faces": return WolvenKit.Properties.Resources.w2faces;
                case "w2fnt": return WolvenKit.Properties.Resources.w2fnt;
                case "w2je": return WolvenKit.Properties.Resources.w2je;
                case "w2job": return WolvenKit.Properties.Resources.w2job;
                case "w2l": return WolvenKit.Properties.Resources.w2l;
                case "w2mesh": return WolvenKit.Properties.Resources.w2mesh;
                case "w2mg": return WolvenKit.Properties.Resources.w2mg;
                case "w2mi": return WolvenKit.Properties.Resources.w2mi;
                case "w2p": return WolvenKit.Properties.Resources.w2p;
                case "w2phase": return WolvenKit.Properties.Resources.w2phase;
                case "w2quest": return WolvenKit.Properties.Resources.w2quest;
                case "w2rag": return WolvenKit.Properties.Resources.w2rag;
                case "w2ragdoll": return WolvenKit.Properties.Resources.w2ragdoll;
                case "w2rig": return WolvenKit.Properties.Resources.w2rig;
                case "w2scene": return WolvenKit.Properties.Resources.w2scene;
                case "w2steer": return WolvenKit.Properties.Resources.w2steer;
                case "w2w": return WolvenKit.Properties.Resources.w2w;
                case "csv": return WolvenKit.Properties.Resources.csv;
                case "env": return WolvenKit.Properties.Resources.env;
                case "journal": return WolvenKit.Properties.Resources.journal;
                case "redgame": return WolvenKit.Properties.Resources.redgame;
                case "redswf": return WolvenKit.Properties.Resources.redswf;
                case "spawntree": return WolvenKit.Properties.Resources.spawntree;
                case "swf": return WolvenKit.Properties.Resources.swf;
                case "vbrush": return WolvenKit.Properties.Resources.vbrush;
                case "w2anim": return WolvenKit.Properties.Resources.w2anim;
                case "w2animev": return WolvenKit.Properties.Resources.w2animev;
                case "w2anims": return WolvenKit.Properties.Resources.w2anims;
                case "w2beh": return WolvenKit.Properties.Resources.w2beh;
                case "w2behtree": return WolvenKit.Properties.Resources.w2behtree;
                case "w2cent": return WolvenKit.Properties.Resources.w2cent;
                case "w2comm": return WolvenKit.Properties.Resources.w2comm;
                case "w2conv": return WolvenKit.Properties.Resources.w2conv;
                case "w2cube": return WolvenKit.Properties.Resources.w2cube;
                case "w2cutscene": return WolvenKit.Properties.Resources.w2cutscene;

                case closedDirImageKey: return WolvenKit.Properties.Resources.FolderClosed_16x;
                case openDirImageKey: return WolvenKit.Properties.Resources.FolderOpened_16x;
                default: return WolvenKit.Properties.Resources.BlankFile_16x;

            }
        }
        private string GetFileExtension(FileSystemInfo node)
        {
            if (node.IsDirectory())
            {
                if (treeListView.IsExpanded(node))
                    return openDirImageKey;
                else
                    return closedDirImageKey;
            }
            else
                return (node as FileInfo).Extension;
        }

        #endregion

        #region Control Events

        private void ExpandBTN_Click(object sender, EventArgs e) => treeListView.ExpandAll();
        private void CollapseBTN_Click(object sender, EventArgs e) => treeListView.CollapseAll();
        private void UpdatefilelistButtonClick(object sender, EventArgs e) => searchBox.Clear();

        private void showhideButton_Click(object sender, EventArgs e)
        {
            vm.IsTreeview = !vm.IsTreeview;
            vm.RepopulateTreeView();
        }
        private void modFileList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                RequestFileRename?.Invoke(this, new RequestFileArgs { File = selectedobject.FullName });
            }
        }
        private void treeListView_CellClick(object sender, CellClickEventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject && e.Item != null)
            {
                var node = (FileSystemInfo)e.Item.RowObject;

                if (e.ClickCount == 1)
                {
                    if (!selectedobject.IsDirectory())
                        RequestFileOpen?.Invoke(this, new RequestFileArgs { File = node.FullName, Inspect = true });
                }
            }
        }

        private void treeListView_ItemActivate(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                if (!selectedobject.IsDirectory())
                    RequestFileOpen?.Invoke(this, new RequestFileArgs { File = selectedobject.FullName });
                else
                    treeListView.ToggleExpansion(selectedobject);
            }
        }





        private void contextMenu_Opened(object sender, EventArgs e)
        {
            string ext = "";

            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var fi = new FileInfo(selectedobject.FullName);
                ext = fi.Extension.TrimStart('.');
                bool isbundle = Path.Combine(ActiveMod.FileDirectory, fi.ToString())
                    .Contains(Path.Combine(ActiveMod.ModDirectory, EBundleType.Bundle.ToString()));
                bool israw = Path.Combine(ActiveMod.FileDirectory, fi.ToString())
                    .Contains(ActiveMod.RadishDirectory);
                bool isToplevelDir = selectedobject.FullName == ActiveMod.ModDirectory
                        || selectedobject.FullName == ActiveMod.DlcDirectory
                        || selectedobject.FullName == ActiveMod.RawDirectory
                        || selectedobject.FullName == ActiveMod.RadishDirectory
                        || selectedobject.FullName == ActiveMod.TextureCacheDirectory
                        || selectedobject.FullName == ActiveMod.CollisionCacheDirectory
                        || selectedobject.FullName == ActiveMod.BundleDirectory
                        ;

                createW2cutsceneToolStripMenuItem.Enabled = !isToplevelDir;
                createW2animsToolStripMenuItem.Enabled = !isToplevelDir;
                exportToolStripMenuItem.Enabled = !isToplevelDir || (ext == "w3fac" || ext == "w2cutscene" || ext == "w2anims" || ext == "w2rig" || ext == "xbm"
                    || Enum.GetNames(typeof(EExportable)).Contains(ext));

                exportW2animsjsonToolStripMenuItem.Visible = ext == "w2anims";
                exportW2cutscenejsonToolStripMenuItem.Visible = !isToplevelDir;
                exportw2rigjsonToolStripMenuItem.Visible = ext == "w2rig";
                exportW3facjsonToolStripMenuItem.Visible = ext == "w3fac";
                exportWithWccToolStripMenuItem.Visible = Enum.GetNames(typeof(EExportable)).Contains(ext) || ext == "xbm";

                removeFileToolStripMenuItem.Enabled = !isToplevelDir;
                renameToolStripMenuItem.Enabled = !isToplevelDir;
                copyRelativePathToolStripMenuItem.Enabled = !isToplevelDir;
                copyToolStripMenuItem.Enabled = !isToplevelDir;
                pasteToolStripMenuItem.Enabled = File.Exists(Clipboard.GetText());

                cookToolStripMenuItem.Enabled = (!Enum.GetNames(typeof(EImportable)).Contains(ext) && !isbundle && !israw);
                markAsModDlcFileToolStripMenuItem.Enabled = isbundle && !isToplevelDir;

                showFileInExplorerToolStripMenuItem.Text = selectedobject.IsDirectory() ? "Open Folder in Explorer" : "Open File in Explorer";
            }

            showFileInExplorerToolStripMenuItem.Enabled = treeListView.SelectedObject != null;
            

        }

        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                exportw2rigjsonToolStripMenuItem.Visible = Path.GetExtension(selectedobject.Name) == ".w2rig";
                exportW2animsjsonToolStripMenuItem.Visible = Path.GetExtension(selectedobject.Name) == ".w2anims";
                exportW2cutscenejsonToolStripMenuItem.Visible = Path.GetExtension(selectedobject.Name) == ".w2cutscene";
                exportW3facjsonToolStripMenuItem.Visible = Path.GetExtension(selectedobject.Name) == ".w3fac";
                exportW3facposejsonToolStripMenuItem.Visible = Path.GetExtension(selectedobject.Name) == ".w3fac";
                fastRenderToolStripMenuItem.Enabled = Path.GetExtension(selectedobject.Name) == ".w2mesh";
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveMod == null)
                return;

            if (!string.IsNullOrEmpty(searchBox.Text))
                treeListView.ModelFilter = TextMatchFilter.Contains(treeListView, searchBox.Text.ToUpper());
            else
                treeListView.ModelFilter = null;
        }

        private void treeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = treeListView.SelectedObjects.Cast<FileSystemInfo>().ToList();
            vm.SelectedItems = s;
        }


        #endregion

        #region Context Menu
        private void openAssetBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                RequestAssetBrowser(this, new RequestFileArgs { File = GetExplorerString(selectedobject.FullName ?? "") });
            }

            string GetExplorerString(string s)
            {
                s = s.Substring(ActiveMod.FileDirectory.Length + 1);
                if (s.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length > 1)
                {
                    int skip = s.StartsWith("Raw") ? 2 : 1;
                    if (s.StartsWith("Mod\\Uncooked"))
                    {
                        s = s.Substring("Mod\\Uncooked".Length);
                        s = $"Mod\\Bundle{s}";
                    }

                    var r = string
                        .Join(Path.DirectorySeparatorChar.ToString(), new[] { "Root" }
                        .Concat(s.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Skip(skip)).ToArray());
                    return r;
                }
                else
                    return s;
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                if (MockKernel.Get().GetMainViewModel().OpenDocuments.Any(_ => _.FileName == selectedobject.FullName))
                {
                    MainController.LogString("Please close the file in WolvenKit before renaming.", Logtype.Error);
                    return;
                }
                RequestFileRename?.Invoke(this, new RequestFileArgs { File = selectedobject.FullName });
            }
        }

        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var vm = MockKernel.Get().GetMainViewModel();
                if (vm.OpenDocuments.Any(_ => _.FileName == selectedobject.FullName))
                {
                    MainController.LogString("Please close the file in WolvenKit before deleting.", Logtype.Error);
                    return;
                }
            }

            if (MessageBox.Show(
                     "Are you sure you want to permanently delete this?", "Confirmation", MessageBoxButtons.OKCancel
                 ) == DialogResult.OK)
            {
                vm.DeleteFilesCommand.SafeExecute();
            }
        }

        private void addAllDependenciesToolStripMenuItem_Click(object sender, EventArgs e) => vm.AddAllImportsCommand.SafeExecute();

        private void cookToolStripMenuItem_Click(object sender, EventArgs e) => vm.CookCommand.SafeExecute();

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => vm.CopyFileCommand.SafeExecute();

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => vm.PasteFileCommand.SafeExecute();

        private void showFileInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                Commonfunctions.ShowFileInExplorer(selectedobject.FullName);
            }
        }

        private void copyRelativePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
                Clipboard.SetText(GetArchivePath(selectedobject.FullName));

            string GetArchivePath(string s)
            {
                if (s.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length > 2)
                {
                    var relpath = s.Substring(ActiveMod.FileDirectory.Length + 1);
                    return string.Join(Path.DirectorySeparatorChar.ToString(), relpath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Skip(2).ToArray());
                }
                else
                    return s;
            }
        }

        private void markAsModDlcFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var filename = selectedobject.FullName;
                var fullpath = Path.Combine(ActiveMod.FileDirectory, filename);
                if (!File.Exists(fullpath))
                    return;
                var newfullpath = Path.Combine(new[] { ActiveMod.FileDirectory, filename.Split('\\')[0] == "DLC" ? "Mod" : "DLC" }.Concat(filename.Split('\\').Skip(1).ToArray()).ToArray());

                if (File.Exists(newfullpath))
                    return;
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newfullpath));
                }
                catch
                {
                }
                File.Move(fullpath, newfullpath);
                MainController.Get().ProjectStatus = "File moved";
            }
        }


        private void exportw2rigjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                Console.WriteLine(selectedobject.FullName);
                string w2rigFilePath = selectedobject.FullName;
                using (var sf = new SaveFileDialog())
                {
                    sf.Filter = "W3 json | *.json";
                    sf.FileName = Path.GetFileName(selectedobject.FullName + ".json");
                    if (sf.ShowDialog() == DialogResult.OK)
                    {
                        CommonData cdata = new CommonData();
                        Rig exportRig = new Rig(cdata);
                        byte[] data;
                        data = File.ReadAllBytes(w2rigFilePath);
                        using (MemoryStream ms = new MemoryStream(data))
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            CR2WFile rigFile = new CR2WFile(br);
                            if ((sender as ToolStripMenuItem).Name == "exportW3dyngjsonToolStripMenuItem")
                                exportRig.LoadData(rigFile, 1);
                            else
                                exportRig.LoadData(rigFile);
                            exportRig.SaveRig(sf.FileName);
                        }
                        MessageBox.Show(this, "Sucessfully wrote file!", "WolvenKit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }   
        }

        private void exportW3dyngjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportw2rigjsonToolStripMenuItem_Click(sender, e);
        }

        private void exportW2animsjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var settings = new frmAnims(selectedobject.FullName,
                                        ActiveMod.FileDirectory + "\\" + "Mod\\Bundle\\characters\\base_entities\\woman_base\\woman_base.w2rig");
                settings.ShowDialog();
            }
        }
        private void exportW2cutscenejsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptName = "dumpW2CUTSCENE";
            LoggerService logger = MainController.Get().Logger;
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var filename = selectedobject.FullName;
                var fullpath = Path.Combine(ActiveMod.FileDirectory, filename);
                if (File.Exists(fullpath))
                {
                    logger?.LogString($"[{scriptName}] Processing single file.", Logtype.Important);
                    var settings = new frmAnims(fullpath,
                                        ActiveMod.FileDirectory + "\\" + "Mod\\Bundle\\characters\\base_entities\\man_base\\man_base.w2rig");
                    settings.ShowDialog();
                }
                else if (Directory.Exists(fullpath))
                {
                    logger?.LogString($"[{scriptName}] Processing folder.", Logtype.Important);
                    MainController.Get().ProjectStatus = "Busy";
                    List<string> cr2wPaths = new List<string>();
                    string rootDir = "";

                    cr2wPaths = new List<string>(Directory.EnumerateFiles(fullpath, "*.w2cutscene", SearchOption.AllDirectories));
                    rootDir = fullpath;
                    logger?.LogString($"[{scriptName}] Files to process: {cr2wPaths.Count}", Logtype.Important);

                    string savePath;
                    int percent_old = -1;
                    Task.Run(() => //Run the method in another thread to prevent freezing UI
                    {
                        List<string> errors = new List<string>();
                        for (int i = 0; i < cr2wPaths.Count; ++i)
                        {
                            Debug.WriteLine($"[{i}]: {fullpath}");
                            string ext = Path.GetExtension(cr2wPaths[i]);
                            int percent = (int)((i) / (float)cr2wPaths.Count * 100.0);
                            if (percent > percent_old)
                            {
                                //logger?.LogString($"[{scriptName}] ({percent}%) Processing: {cr2wPaths[i]}..", Logtype.Normal);
                                percent_old = percent;
                            }
                            logger?.LogString($"[{scriptName}] ({percent}%) Processing: {cr2wPaths[i]}..", Logtype.Normal);

                            if (string.IsNullOrEmpty(rootDir))
                            {
                                savePath = $"{cr2wPaths[i]}.json";
                            }
                            else
                            {
                                savePath = $"{rootDir}_{scriptName}\\{cr2wPaths[i].Substring(rootDir.Length + 1, cr2wPaths[i].Length - (rootDir.Length + 1))}.json";
                                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                            }
                            if (File.Exists(savePath))
                            {
                                if (MessageBox.Show(savePath + " already exists. Are you sure you want to overwrite it?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                {
                                    continue;
                                }
                            }

                            var csExportTool = new ExportCutscene();
                            using (var stream = File.Open(cr2wPaths[i], FileMode.Open))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    var cr2wCS = new CR2WFile(reader)
                                    {
                                        FileName = cr2wPaths[i]
                                    };
                                    csExportTool.LoadData(cr2wCS);
                                    csExportTool.LoadCutsceneData(cr2wCS, App.MainController.Get().BundleManager);
                                }
                            }

                            try
                            {
                                csExportTool.SaveJson(savePath);
                                logger?.LogString($"[{scriptName}] ({percent}%) OK created: {savePath}..", Logtype.Success);
                            }
                            catch (Exception ex)
                            {
                                logger?.LogString($"[{scriptName}] ({percent}%) Can't create: {savePath}..", Logtype.Error);
                                errors.Add($"{cr2wPaths[i]}: Can't create JSON.");
                            }
                        }
                        //logFile.Close();
                        if (errors.Count > 0)
                        {
                            logger?.LogString($"[{scriptName}] Finished with errors:\n\t{String.Join("\n\t", errors)}", Logtype.Error);
                        }
                        else
                        {
                            logger?.LogString($"[{scriptName}] Finished.", Logtype.Success);
                        }
                        MainController.Get().ProjectStatus = "Ready";
                    });
                } else
                {
                    logger?.LogString($"[{scriptName}] No files to process.", Logtype.Important);
                }
            }
        }
        private void exportW3facjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportw2rigjsonToolStripMenuItem_Click(sender, e);
        }
        private void exportW3facposejsonToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void createW2animsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptName = "createW2ANIMS";
            LoggerService logger = MainController.Get().Logger;
            MainController.Get().ProjectStatus = "Busy";

            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var filename = selectedobject.FullName;
                var fullpath = Path.Combine(ActiveMod.FileDirectory, filename);
                if (!File.Exists(fullpath) && !Directory.Exists(fullpath))
                {
                    logger?.LogString($"[{scriptName}] No files to process.", Logtype.Important);
                    return;
                }
                string dir;
                if (File.Exists(fullpath))
                    dir = Path.GetDirectoryName(fullpath);
                else
                    dir = fullpath;
                var files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).ToList();
                if (files.Count < 1)
                {
                    logger?.LogString($"[{scriptName}] No json files to process.", Logtype.Important);
                    return;
                }
                var folderName = Path.GetFileName(fullpath);
                ConvertAnimation animTool = new ConvertAnimation();
                if (File.Exists(fullpath + ".w2anims"))
                {
                    if (MessageBox.Show(
                         folderName + ".w2anims already exists. This file will be overwritten. Are you sure you want to permanently overwrite " + folderName + " w2anims?"
                         , "Confirmation", MessageBoxButtons.YesNo
                     ) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                Task.Run(() =>
                {
                    logger?.LogString($"[{scriptName}] Processing: {files.Count} files..", Logtype.Normal);
                    if (animTool.CreateW2Anims(files, fullpath + ".w2anims"))
                    {
                        logger?.LogString($"[{scriptName}] Finished.", Logtype.Success);
                    }
                    else
                    {
                        logger?.LogString($"[{scriptName}] Finished with errors.", Logtype.Error);
                    }
                    MainController.Get().ProjectStatus = "Ready";
                });
            }
        }

        protected void createW2cutsceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptName = "createW2CUTSCENE"; // save folder suffix
            LoggerService logger = MainController.Get().Logger;
            MainController.Get().ProjectStatus = "Busy";

            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                var filename = selectedobject.FullName;
                var fullpath = Path.Combine(ActiveMod.FileDirectory, filename);
                List<string> cr2wPaths = new List<string>();
                string rootDir = "";

                if (Directory.Exists(fullpath))
                {
                    cr2wPaths = new List<string>(Directory.EnumerateFiles(fullpath, "*.json", SearchOption.AllDirectories));
                    rootDir = fullpath;
                    logger?.LogString($"[{scriptName}] Files to process: {cr2wPaths.Count}", Logtype.Important);
                }
                else if (File.Exists(fullpath))
                {
                    cr2wPaths.Add(fullpath);
                }
                else
                {
                    logger?.LogString($"[{scriptName}] No valid files to process.", Logtype.Important);
                    MainController.Get().ProjectStatus = "Ready";
                    return;
                }

                string savePath;
                int percent_old = -1;
                Task.Run(() => //Run the method in another thread to prevent freezing UI
                {
                    List<string> errors = new List<string>();
                    for (int i = 0; i < cr2wPaths.Count; ++i)
                    {
                        Debug.WriteLine($"[{i}]: {fullpath}");
                        string ext = Path.GetExtension(cr2wPaths[i]);
                        int percent = (int)((i) / (float)cr2wPaths.Count * 100.0);
                        if (percent > percent_old)
                        {
                            logger?.LogString($"[{scriptName}] ({percent}%) Processing: {cr2wPaths[i]}..", Logtype.Normal);
                            percent_old = percent;
                        }

                        if (string.IsNullOrEmpty(rootDir))
                        {
                            savePath = $"{cr2wPaths[i]}.w2cutscene";
                        }
                        else
                        {
                            savePath = $"{rootDir}_{scriptName}\\{cr2wPaths[i].Substring(rootDir.Length + 1, cr2wPaths[i].Length - (rootDir.Length + 1))}.w2cutscene";
                            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        }
                        if (File.Exists(savePath))
                        {
                            if (MessageBox.Show(savePath + " already exists. Are you sure you want to overwrite it?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            {
                                continue;
                            }
                        }

                        var csTool = new ConvertCutscene();
                        if (csTool.CreateW2Cutscene(cr2wPaths[i], savePath))
                        {
                            logger?.LogString($"[{scriptName}] ({percent}%) OK created: {savePath}..", Logtype.Success);
                        }
                        else
                        {
                            logger?.LogString($"[{scriptName}] ({percent}%) Can't create: {savePath}..", Logtype.Error);
                            errors.Add($"{cr2wPaths[i]}: Can't export W2CUTSCENE.");
                        }
                    }
                    //logFile.Close();
                    if (errors.Count > 0)
                    {
                        logger?.LogString($"[{scriptName}] Finished with errors:\n\t{String.Join("\n\t", errors)}", Logtype.Error);
                    }
                    else
                    {
                        logger?.LogString($"[{scriptName}] Finished.", Logtype.Success);
                    }
                    MainController.Get().ProjectStatus = "Ready";
                });
            }
        }

        private async void exportW2meshToFbxToolStripMenuItem_Click(object sender, EventArgs e) => vm.ExportMeshCommand.SafeExecute();
        
        private async void dumpWccliteXMLToolStripMenuItem_Click(object sender, EventArgs e) => vm.DumpXMLCommand.SafeExecute();


        private void fastRenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                vm.AddAllImportsCommand.SafeExecute();
                RequestFastRender?.Invoke(this, new RequestFileArgs { File = selectedobject.FullName });
            }
        }







        #endregion

        private void treeListView_Expanded(object sender, TreeBranchExpandedEventArgs e)
        {
        }

        private void treeListView_Collapsed(object sender, TreeBranchCollapsedEventArgs e)
        {
        }

        private void exportW2entjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeListView.SelectedObject is FileSystemInfo selectedobject)
            {
                string w2entFilePath = selectedobject.FullName;

                SaveFileDialog sFileDialog = new SaveFileDialog
                {
                    Filter = "Witcher Entity |*w2ent.json",
                    Title = "Save Entity File",
                    InitialDirectory = selectedobject.FullName,
                    OverwritePrompt = true,
                    FileName = Path.GetFileName(selectedobject.FullName + ".json")
                };

                DialogResult result = sFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //FileStream writer = new FileStream(sFileDialog.FileName, FileMode.Create);

                    EntityGenerator generator = new EntityGenerator();
                    generator.LoadManager(MainController.Get().BundleManager);
                    generator.LoadWccHelper(MainController.Get().WccHelper);
                    generator.readCEntityTemplate(w2entFilePath);
                    generator.SaveEntAsync(sFileDialog.FileName);
                    MessageBox.Show(this, "Sucessfully wrote file!", "WolvenKit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
    }
}