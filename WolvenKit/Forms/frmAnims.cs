using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParserLTK;
//using Microsoft.Win32;
using WolvenKit.CR2W;
using WolvenKit.Render;
using WolvenKit.Render.Animation;

namespace WolvenKit
{
    public partial class frmAnims : Form
    {
        protected CR2WFile animsFile;
        protected ExportAnimation exportAnims { get; set; }
        protected Rig exportRig { get; set; }
        protected bool isCutscene { get; set; }

        public frmAnims(string w2animsFilePath = null, string w2rigFilePath = null)
        {
            InitializeComponent();
            //var config = MainController.Get().Configuration;
            txw2rig.Text = w2rigFilePath;//config.ExecutablePath;
            txw2anims.Text = w2animsFilePath;//config.WccLite;
            //comboBoxAnim.Items.AddRange(Enum.GetValues(typeof(EColorThemes)).Cast<object>().ToArray());
            //comboBoxTheme.SelectedItem = config.ColorTheme;
            btSave.Enabled = checkEnabled();

            comboBoxAnim.DataBindings.Add("Enabled", radioButtonAnim1, "Checked");
            setupcomboBox();
        }

        /// <summary>
        /// setup visibility of rendere context menu
        /// </summary>
        private void setupcomboBox()
        {
            if (File.Exists(txw2anims.Text))
            {
                {
                    string w2rigFilePath = txw2rig.Text;
                    string w2animsFilePath = txw2anims.Text;
                    isCutscene = false;
                    txw2rig.Enabled = true;
                    radioButtonAnim1.Enabled = true;
                    radioButtonAnim2.Enabled = true;

                    if (File.Exists(w2animsFilePath) && (Path.GetExtension(w2animsFilePath) == ".w2cutscene"))
                    {
                        exportAnims = new ExportCutscene();
                        byte[] animsData;
                        animsData = File.ReadAllBytes(w2animsFilePath);
                        using (MemoryStream ms = new MemoryStream(animsData))
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            animsFile = new CR2WFile(br)
                            {
                                FileName = w2animsFilePath
                            };
                            exportAnims.LoadData(animsFile);
                            (exportAnims as ExportCutscene).LoadCutsceneData(animsFile, App.MainController.Get().BundleManager);
                            isCutscene = true;
                        }
                        Console.WriteLine("This is a cutscene file");
                        txw2rig.Enabled = false;
                        radioButtonAnim1.Enabled = false;
                        radioButtonAnim2.Enabled = false;
                    }
                    else if (File.Exists(w2rigFilePath))
                    {
                        CommonData cdata = new CommonData();
                        exportRig = new Rig(cdata);
                        byte[] data;
                        data = File.ReadAllBytes(w2rigFilePath);
                        using (MemoryStream ms = new MemoryStream(data))
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            CR2WFile rigFile = new CR2WFile(br);

                            if (File.Exists(w2rigFilePath) && (Path.GetExtension(w2rigFilePath) == ".w3fac"))
                                exportRig.LoadData(rigFile, 2); //2 is skeleton chunk index for face
                            else
                                exportRig.LoadData(rigFile);
                        }

                        exportAnims = new ExportAnimation();
                        byte[] animsData;
                        animsData = File.ReadAllBytes(w2animsFilePath);
                        using (MemoryStream ms = new MemoryStream(animsData))
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            animsFile = new CR2WFile(br)
                            {
                                FileName = w2animsFilePath
                            };
                            exportAnims.LoadData(animsFile, exportRig);
                        }
                    }
                    comboBoxAnim.Items.Clear();
                    for (int i = 0; i < exportAnims.AnimationNames.Count; i++)
                        comboBoxAnim.Items.Add(exportAnims.AnimationNames[i].Key);
                    if(exportAnims.AnimationNames.Count > 0) comboBoxAnim.SelectedItem = exportAnims.AnimationNames[0].Key;
                }
            }
            else
            {
                //loadAnimToolStripMenuItem.Enabled = true;
            }
        }

        

        private void btnBrowseRig_Click(object sender, EventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = "Select Rig File.";
            dlg.FileName = txw2rig.Text;
            dlg.Filter = "Rig (*.w2rig)|*.w2rig|Mimic Rig (*.w3fac)|*.w3fac";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                txw2rig.Text = dlg.FileName;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txw2rig.Text) && !isCutscene)
            {
                DialogResult = DialogResult.None;
                txw2rig.Focus();
                MessageBox.Show("Invalid w2rig path", "failed to save.");
                return;
            }

            if (!File.Exists(txw2anims.Text))
            {
                DialogResult = DialogResult.None;
                txw2anims.Focus();
                MessageBox.Show("Invalid file path", "failed to save.");
                return;
            }
            Task.Run(() =>
            {
                using (var sf = new SaveFileDialog())
                {
                    sf.Filter = "W3 json | *.json";
                    sf.FileName = Path.GetFileName(txw2anims.Text) + ".json";
                    if (sf.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(txw2anims.Text) && (Path.GetExtension(txw2anims.Text) == ".w2cutscene"))
                        {
                            (exportAnims as ExportCutscene).SaveJson(sf.FileName);
                        }
                        else
                        {
                            if (radioButtonAnim1.Checked)
                            {
                                exportAnims.Apply(exportRig);
                                exportAnims.SaveJson(sf.FileName);
                            }
                            else
                            {
                                exportAnims.Apply(exportRig);
                                exportAnims.LoadAllAnims();
                                exportAnims.SaveSet(sf.FileName);
                            }
                        }
                        MessageBox.Show(this, "Sucessfully wrote file!", "WolvenKit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            });
        }

        private void btBrowseAnims_Click(object sender, EventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog
            {
                Title = "Select Witcher 3 Animation File.",
                FileName = txw2rig.Text,
                Filter = "Witcher 3 Animation File (*.w2anims)|*.w2anims|Witcher 3 Cutscene File (*.w2cutscene)|*.w2cutscene"
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                txw2anims.Text = dlg.FileName;
            }
        }

        private void txWCC_Lite_TextChanged(object sender, EventArgs e)
        {
            var path = txw2anims.Text;
            if (File.Exists(path) && (Path.GetExtension(path) == ".w2anims" || Path.GetExtension(path) == ".w2cutscene"))
            {
                WCCexeTickLBL.Text = "✓";
                WCCexeTickLBL.ForeColor = Color.Green;
                setupcomboBox();
            }
            else
            {
                WCCexeTickLBL.Text = "X";
                WCCexeTickLBL.ForeColor = Color.Red;
            }
            btSave.Enabled = checkEnabled();
        }

        private void txExecutablePath_TextChanged(object sender, EventArgs e)
        {
            var path = txw2rig.Text;
            if (File.Exists(path) && (Path.GetExtension(path) == ".w2rig" || Path.GetExtension(path) == ".w3fac"))
            {
                W3exeTickLBL.Text = "✓";
                W3exeTickLBL.ForeColor = Color.Green;
                setupcomboBox();
            }
            else
            {
                W3exeTickLBL.Text = "X";
                W3exeTickLBL.ForeColor = Color.Red;
            }
            btSave.Enabled = checkEnabled();
        }

        private bool checkEnabled()
        {
            return (File.Exists(txw2anims.Text) && (Path.GetExtension(txw2anims.Text) == ".w2anims" || Path.GetExtension(txw2anims.Text) == ".w2cutscene")) &&
                (File.Exists(txw2rig.Text) && Path.GetExtension(txw2rig.Text) == ".w2rig") || (File.Exists(txw2rig.Text) && Path.GetExtension(txw2rig.Text) == ".w3fac");
        }

        private void comboBoxAnim_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBoxAnim = (ComboBox)sender;
            int resultIndex = -1;
            string selectedAnim = (string)comboBoxAnim.SelectedItem;
            resultIndex = comboBoxAnim.FindStringExact(selectedAnim);
            exportAnims.SelectAnimation(animsFile, resultIndex);
        }
    }
}