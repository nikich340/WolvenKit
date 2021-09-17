using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPlus.Extensibility;
using WolvenKit.App;
using WolvenKit.Common.Services;
using WolvenKit.CR2W;
using WolvenKit.CR2W.Editors;
using static WolvenKit.frmChunkProperties;

namespace WolvenKit.Forms
{
    public partial class frmCR2WtoText : Form
    {
        private readonly string[] extExclude = { ".txt", ".json", ".csv", ".xml", ".jpg", ".png", ".buffer", ".navgraph", ".navtile",
                                                 ".usm", ".wem", ".dds", ".bnk", ".xbm", ".bundle", ".w3strings", ".store", ".navconfig",
                                                 ".srt", ".naviobstacles", ".navmesh", ".sav", ".subs", ".yml" };
        private StatusController statusController;
        private readonly List<string> Files = new List<string>();

        private CancellationTokenSource Cancel;

        private bool _outputSingleFile = false;
        private bool OutputSingleFile
        {
            get => _outputSingleFile;
            set
            {
                _outputSingleFile = value;
                txtOutputDestination.Clear();
            }
        }

        private bool _running = false;
        private bool _stopping = false;

        public frmCR2WtoText()
        {
            InitializeComponent();
            SetDefaults();
            InitDataGrid();
            InitStatusController();
            foreach (var ext in extExclude)
            {   // Add the list of excluded file extensions to info box so user knows what files will never be opened.
                rtfDescription.AppendText(ext + " ");
            }
        }
        private void InitDataGrid()
        {
            object[] row = {0, 0, 0, 0, 0, 0};
            if (dataStatus.Rows.Count == 0)
                dataStatus.Rows.Add(row);
            else
                dataStatus.Rows[0].SetValues(row);
        }
        private void InitStatusController()
        {
            statusController = new StatusController();
            statusController.OnTotalFilesUpdated += (x) => { UpdateStatusCell(0, x); };
            statusController.OnNonCR2WUpdated += (x) => { UpdateStatusCell(1, x); };
            statusController.OnMatchingUpdated += (x) => { UpdateStatusCell(2, x); };
            statusController.OnProcessedUpdated += (x) => { UpdateStatusCell(3, x); };
            statusController.OnProcessedUpdated += UpdateProgressBarStatic;
            statusController.OnSkippedUpdated += (x) => { UpdateStatusCell(4, x); };
            statusController.OnExceptionsUpdated += (x) => { UpdateStatusCell(5, x); };
        }
        private void UpdateStatusCell(int cell, int value)
        {
            dataStatus.Rows[0].Cells[cell].Value = value;
        }
        private void SetDefaults()
        {
            radExistingOverwrite.Checked = true;
            radExistingSkip.Checked = !radExistingOverwrite.Checked;
            radOutputModeSingleFile.Checked = OutputSingleFile;
            radOutputModeSeparateFiles.Checked = !OutputSingleFile;
            chkDumpSDB.Checked = true;
            chkDumpFCD.Checked = false;
            chkDumpYML.Checked = true;
            chkDumpEmbedded.Checked = true;
            numThreads.Value = Environment.ProcessorCount;
            numThreads.Maximum = Environment.ProcessorCount * 2;

            // nikich340 temp
            txtPath.Text = "D:/_w3.tools/WKit_projects/replacer1/files/Mod/Cooked/gameplay/templates/characters/player/dump";
            txtOutputDestination.Text = txtPath.Text;
            btnRun.Enabled = true;
            chkPrefixFileName.Checked = false;
            chkDumpSDB.Checked = false;
            chkDumpEmbedded.Checked = false;
            UpdateSourceFolder(txtPath.Text);
        }
        // Delegates for cross-thread updating of progress bar and console textbox.
        private delegate void UpdateProgressBarDelegate(int processed);
        private delegate void LogLineDelegate(string line);
        private void UpdateProgressBarStatic(int processed)
        {
            Invoke(new UpdateProgressBarDelegate(UpdateProgressBar), processed);
        }
        private void UpdateProgressBar(int processed)
        {
            if (processed > prgProgressBar.Value)
                prgProgressBar.PerformStep();
        }
        private void LogLineStatic(string line)
        {
            Invoke(new LogLineDelegate(LogLine), line);
        }
        private void LogLine(string line)
        {
            txtLog.AppendText(DateTime.Now + ": " + line + "\r\n");
        }
        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Choose path containing unbundled CR2W files."
            };
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtPath.Text = dlg.FileName;
            }
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!_running)
                StartRun();
            else
                StopRun();
        }
        private void StopRun()
        {
            if (!_stopping)
            {
                btnRun.Text = "Stopping...";
                LogLineStatic("Stopping dump once in-progress files complete.");
                Cancel.Cancel();
                _stopping = true;
            }
        }
        private void ControlsEnabledDuringRun(bool b)
        {
            // During run, all controls are disabled, and the run button changes to abort
            pnlControls.Enabled = b;
            btnRun.Text = b ? "Run CR2W Dump" : "Abort";
        }
        private async void StartRun()
        {
            ControlsEnabledDuringRun(false);
            prgProgressBar.Value = 0;
            prgProgressBar.Visible = true;
            LogLine($"Dump starting ({txtPath.Text})");
            _running = true;

            await Task.Run(DoRun);

            _running = false;
            _stopping = false;
            LogLine($"Dump finished ({txtPath.Text})");
            prgProgressBar.Visible = false;
            ControlsEnabledDuringRun(true);
        }
        private async Task DoRun()
        {
            Console.WriteLine("frmCR2WtoText::DoRun()");
            // Clear status prior to new run.
            statusController.Processed = statusController.Skipped = statusController.Exceptions = 0;
            if (Files.Any())
            {
                string sourcePath = txtPath.Text;

                var cr2wOptions = new LoggerCR2WOptions
                {
                    StartingIndentLevel = 1,
                    ListEmbedded = chkDumpEmbedded.Checked,
                    DumpFCD = chkDumpFCD.Checked,
                    DumpSDB = chkDumpSDB.Checked,
                    DumpYML = chkDumpYML.Checked,
                    DumpTXT = true,
                    LocalizeStrings = chkLocalizedString.Checked
                };

                using (Cancel = new CancellationTokenSource())
                {
                    var loggerOptions = new LoggerWriterData
                    {
                        CancelToken = Cancel.Token,
                        Status = statusController,
                        OutputSingleFile = this.OutputSingleFile,
                        NumThreads = (int) numThreads.Value,
                        SourcePath = sourcePath,
                        OutputLocation = txtOutputDestination.Text,
                        CreateFolders = chkCreateFolders.Checked,
                        PrefixFileName = chkPrefixFileName.Checked,
                        OverwriteFiles = radExistingOverwrite.Checked
                    };

                    LoggerWriter writer;

                    if (OutputSingleFile)
                        writer = new LoggerWriterSingle(Files, loggerOptions, cr2wOptions);
                    else
                        writer = new LoggerWriterSeparate(Files, loggerOptions, cr2wOptions);

                    writer.OnExceptionFile += (fileName, msg) =>
                    {
                        var fileNameNoSource = LoggerWriter.FileNameNoSourcePath(fileName, sourcePath);
                        msg = msg.Replace("\r\n", " ");
                        LogLineStatic($"Exception: {fileNameNoSource} : {msg}");
                    };
                    writer.OnNonCR2WFile += fileName =>
                    {
                        var fileNameNoSource = LoggerWriter.FileNameNoSourcePath(fileName, sourcePath);
                        LogLineStatic($"Non CR2W file: {fileNameNoSource}");
                    };

                    LogLineStatic("Starting counts: " + statusLogMessage(false));

                    await writer.StartDump();

                    LogLineStatic("Completed dump stats: " + statusLogMessage(true));
                }
            }
        }
        private string statusLogMessage(bool after)
        {
            var log = $"Files in source folder: {statusController.TotalFiles}, " +
                            $"Non CR2W: {statusController.NonCR2W}. " +
                            $"To be processed: {statusController.Matching}. ";
            if (after)
            {
                log += $"Processed: {statusController.Processed}, " +
                       $"Skipped: {statusController.Skipped}, " +
                       $"Exceptions in/while reading: {statusController.Exceptions}. " +
                       $"Not processed: {statusController.Matching - statusController.Processed - statusController.Skipped}.";
            }
            return log;
        }
        private void CheckEnableRunButton()
        {
            btnRun.Enabled = txtPath.Text != "" && txtOutputDestination.Text != "" && Files.Any();
        }
        private async Task UpdateSourceFolder(string path)
        {
            Files.Clear();
            statusController.UpdateAll(0,0,0,0,0); // Clear stats
            if (Directory.Exists(path))
            {
                btnRun.Enabled = false;
                pnlControls.Enabled = false;
                LogLine("Reading source folder...");
                await Task.Run(() =>
                {
                    foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                    {
                        if (!extExclude.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                        {
                            Files.Add(file);
                            statusController.Matching++;
                        }
                        else
                            statusController.NonCR2W++;
                    }
                });

                ResetProgressBar(Files.Count());

                LogLine("Finished reading source folder.");
                pnlControls.Enabled = true;
            }
            CheckEnableRunButton();
        }
        private void ResetProgressBar(int filesCount)
        {
            prgProgressBar.Maximum = filesCount;
            prgProgressBar.Value = 0;
            prgProgressBar.Step = 1;
        }
        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            UpdateSourceFolder(txtPath.Text);
        }
        private void txtOutputDestination_TextChanged(object sender, EventArgs e)
        {
            CheckEnableRunButton();
        }
        private void btnPickOutput_Click(object sender, EventArgs e)
        {
            if (OutputSingleFile)
            {
                SaveFileDialog fileDlg = new SaveFileDialog
                {
                    Title = "Enter the filename to output to",
                    Filter = "txt | *.txt"
                };
                if (fileDlg.ShowDialog() == DialogResult.OK)
                    txtOutputDestination.Text = fileDlg.FileName;
            }
            else
            {
                CommonOpenFileDialog folderDlg = new CommonOpenFileDialog
                {
                    Title = "Select folder to write text dumps to",
                    IsFolderPicker = true,
                    EnsurePathExists = false,
                    EnsureFileExists = false,
                    EnsureValidNames = true
                };

                if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                    txtOutputDestination.Text = folderDlg.FileName;
            }
        }
        private void RadioOutputModeChanged(object sender, EventArgs e)
        {
            OutputSingleFile = radOutputModeSingleFile.Checked;
            // In Separate Files mode, enable controls for:
            //   Creating intermediate folders; choosing file overwrite/skip; multi-threaded operation
            chkCreateFolders.Enabled = radOutputModeSeparateFiles.Checked;
            grpExistingFiles.Enabled = radOutputModeSeparateFiles.Checked;
            numThreads.Enabled = radOutputModeSeparateFiles.Checked;
        }
        private void frmCR2WtoText_FormClosing(object sender, FormClosingEventArgs e)
        {   // If user clicks form close while dump is running, abort close and offer chance to cancel dump.
            if (_running)
            {
                if (!_stopping)
                {
                    var msg = "A dump is running. Did you want to cancel this operation?";
                    var caption = "Cancel running dump?";
                    var icon = MessageBoxIcon.Exclamation;
                    var buttons = MessageBoxButtons.YesNo;

                    if (MessageBox.Show(msg, caption, buttons, icon) == DialogResult.Yes)
                        StopRun();
                }
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        private void pnlControls_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    internal class StatusController
    {
        public delegate void StatusDelegate(int value);
        public StatusDelegate OnTotalFilesUpdated;
        public StatusDelegate OnNonCR2WUpdated;
        public StatusDelegate OnMatchingUpdated;
        public StatusDelegate OnProcessedUpdated;
        public StatusDelegate OnSkippedUpdated;
        public StatusDelegate OnExceptionsUpdated;
        public void UpdateAll(int nonCR2W, int matching, int processed, int skipped, int exceptions)
        {
            NonCR2W = nonCR2W;
            Matching = matching;
            Processed = processed;
            Skipped = skipped;
            Exceptions = exceptions;
        }
        public int TotalFiles
        {
            get => Matching + NonCR2W;
        }
        private int _nonCR2W;
        public int NonCR2W
        {
            get => _nonCR2W;
            set
            {
                _nonCR2W = value;
                OnNonCR2WUpdated?.Invoke(_nonCR2W);
                OnTotalFilesUpdated?.Invoke(TotalFiles);
            }
        }
        private int _matching;
        public int Matching
        {
            get => _matching;
            set
            {
                _matching = value;
                OnMatchingUpdated?.Invoke(_matching);
                OnTotalFilesUpdated?.Invoke(TotalFiles);
            }
        }
        private int _processed;
        public int Processed
        {
            get => _processed;
            set
            {
                _processed = value;
                OnProcessedUpdated?.Invoke(_processed);
            }
        }
        private int _skipped;
        public int Skipped
        {
            get => _skipped;
            set
            {
                _skipped = value;
                OnSkippedUpdated?.Invoke(_skipped);
            }
        }
        private int _exceptions;
        public int Exceptions
        {
            get => _exceptions;
            set
            {
                _exceptions = value;
                OnExceptionsUpdated?.Invoke(_exceptions);
            }
        }
    }
    internal class LoggerWriterSeparate : LoggerWriter
    {
        internal LoggerWriterSeparate(List<string> files, LoggerWriterData writerData, LoggerCR2WOptions cr2wOptions)
            : base(files, writerData, cr2wOptions) { }
        public override async Task StartDump()
        {
            Console.WriteLine("LoggerWriterSeparate::StartDump()");
            var parOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = WriterData.NumThreads,
                CancellationToken = WriterData.CancelToken
            };

            if (!Directory.Exists(WriterData.OutputLocation))
                Directory.CreateDirectory(WriterData.OutputLocation);

            try
            {
                Parallel.ForEach(Files, parOptions, async fileName =>
                    {
                        string outputDestination;
                        string outputDestinationTxt;
                        string outputDestinationYml;
                        string fileBaseName = Path.GetFileName(fileName);
                        string fileNameNoSourcePath = FileNameNoSourcePath(fileName, WriterData.SourcePath);

                        if (WriterData.CreateFolders)
                        {   // Recreate the file structure of the source folder in the destination folder.
                            // Strip sourcePath from the start of the filename, strip filename from end, then create the remaining folders.
                            var i = fileNameNoSourcePath.LastIndexOf(fileBaseName, StringComparison.Ordinal);
                            outputDestination = WriterData.OutputLocation + fileNameNoSourcePath.Substring(0, i);

                            Directory.CreateDirectory(outputDestination);
                        }
                        else
                            outputDestination = WriterData.OutputLocation;

                        outputDestinationTxt = outputDestination + "\\" + fileBaseName + ".txt";
                        outputDestinationYml = outputDestination + ".yml";

                        try
                        {
                            bool skip = false;
                            if (File.Exists(outputDestinationTxt))
                                if (WriterData.OverwriteFiles)
                                    File.Delete(outputDestinationTxt);
                                else
                                    skip = true;

                            if (File.Exists(outputDestinationYml))
                                File.Delete(outputDestinationYml);

                            if (!skip)
                            {
                            using (StreamWriter streamDestination = new StreamWriter(outputDestinationTxt, false))
                            using (StreamWriter streamDestinationYml = new StreamWriter(outputDestinationYml, false))
                                {
                                    await Dump(streamDestination, streamDestinationYml, fileName);
                                    lock (statusLock)
                                        WriterData.Status.Processed++;
                                }
                            }
                            else
                                lock (statusLock)
                                    WriterData.Status.Skipped++;
                        }
                        catch (UnauthorizedAccessException)
                        {   // Couldn't write to destination file for some reason, eg read-only
                            string msg = "Could not write to file - is it readonly? Skipping.";
                            ExceptionOccurred(fileName, msg);
                        }
                    });
            }
            catch (OperationCanceledException)
            {
                //TODO: Do we need to do anything here?
            }
        }
    }
    internal class LoggerWriterSingle : LoggerWriter
    {
        internal LoggerWriterSingle(List<string> files, LoggerWriterData writerData, LoggerCR2WOptions cr2wOptions)
            : base(files, writerData, cr2wOptions) { }
        public override async Task StartDump()
        {
            Console.WriteLine("LoggerWriterSeparate::StartDump()");
            string outputDestination = WriterData.OutputLocation;
            string outputDestinationYml = WriterData.OutputLocation + ".yml";
            if (File.Exists(outputDestination))
                File.Delete(outputDestination);

            using (StreamWriter streamDestination = new StreamWriter(outputDestination, false))
            using (StreamWriter streamDestinationYml = new StreamWriter(outputDestinationYml, false))
                foreach (var fileName in Files)
                {
                    if (WriterData.CancelToken.IsCancellationRequested)
                        break;
                    await Dump(streamDestination, streamDestinationYml, fileName);
                    WriterData.Status.Processed++;
                }
        }
    }
    internal abstract class LoggerWriter
    {
        protected readonly object statusLock = new object();
        public delegate void OnExceptionFileDelegate(string fileName, string msg);
        public delegate void OnNonCR2WFileDelegate(string msg);
        public OnExceptionFileDelegate OnExceptionFile;
        public OnNonCR2WFileDelegate OnNonCR2WFile;
        protected LoggerCR2WOptions CR2WOptions { get; }
        protected List<string> Files { get; }
        protected LoggerWriterData WriterData { get; }
        internal LoggerWriter(List<string> files, LoggerWriterData writerData, LoggerCR2WOptions cr2wOptions)
        {
            Files = files;
            WriterData = writerData;
            CR2WOptions = cr2wOptions;
        }
        public abstract Task StartDump();

        public static string FileNameNoSourcePath(string fileName, string sourcePath)
        {
            fileName.ReplaceFirst(sourcePath, "", out var fileNameNoSourcePath);
            return fileNameNoSourcePath;
        }
        protected void ExceptionOccurred(string fileName, string msg)
        {
            lock (statusLock)
            {
                WriterData.Status.Skipped++;
                WriterData.Status.Exceptions++;
            }
            OnExceptionFile?.Invoke(fileName, msg);
        }
        protected async Task Dump(StreamWriter streamDestination, StreamWriter streamDestinationYml, string fileName)
        {
            LoggerOutputFileTxt outputFile = new LoggerOutputFileTxt(streamDestination, WriterData.PrefixFileName,
                Path.GetFileName(fileName));
            LoggerOutputFileYml outputFileYml = new LoggerOutputFileYml(streamDestinationYml, WriterData.PrefixFileName,
                Path.GetFileName(fileName));
            Console.WriteLine("LoggerWriter::Dump() " + fileName);
            try
            {
                var lCR2W = new LoggerCR2W(fileName, outputFile, outputFileYml, CR2WOptions);
                outputFile.WriteLine("FILE: " + fileName);
                outputFileYml.WriteLine("templates:");
                outputFileYml.WriteLine("   ### FILE: " + fileName);
                lCR2W.OnException += (msg, ex) =>
                {
                    OnExceptionFile?.Invoke(fileName, msg + ex.Message);
                };
                lCR2W.processCR2W();
                if (lCR2W.ExceptionCount > 0)
                    lock(statusLock)
                        WriterData.Status.Exceptions++;
            }
            catch (FormatException)
            {   // Non CR2W file.
                string msg = fileName + ": Not a valid CR2W file, or file is damaged.";
                Console.WriteLine(msg);
                lock (statusLock)
                {   // File wasn't CR2W, so move its count from Matching to NonCR2W.
                    WriterData.Status.NonCR2W++;
                    WriterData.Status.Matching--;
                }
                var fileNameNoSourcePath = FileNameNoSourcePath(fileName, WriterData.SourcePath);
                OnNonCR2WFile?.Invoke(fileNameNoSourcePath);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                string msg = "Could not find file or directory - did it get deleted? Skipping.";
                ExceptionOccurred(fileName, msg);
            }
            catch (Exception ex)
            {
                string msg = fileName + ": Exception: " + ex.ToString();
                outputFile.WriteLine(msg);
                Console.WriteLine(msg);
                ExceptionOccurred(fileName, "An exception occurred processing this file. Skipping. Details: " + ex.Message);
            }
        }
    }

    internal class LoggerOutputFileYml : LoggerOutputFile
    {
        private string Prefix { get; }
        private bool PrefixLine { get; }
        private string TempLine { get; set; }
        internal LoggerOutputFileYml(StreamWriter streamDestination, bool prefixLine, string prefix)
            : base(streamDestination)
        {
            PrefixLine = prefixLine;
            Prefix = prefix;
        }
        public void WriteLine(string line)
        {
            OutputFile.WriteLine(line);
        }
        public override void Write(string text, int level = 0)
        {
            string line;
            string indent = "";

            for (int i = 0; i < level; i++)
                indent += "  ";

            line = indent + text;

            WriteLine(line);
        }
    }

    internal class LoggerOutputFileTxt : LoggerOutputFile
    {
        private string Prefix { get; }
        private bool PrefixLine { get; }
        internal LoggerOutputFileTxt(StreamWriter streamDestination, bool prefixLine, string prefix)
            : base(streamDestination)
        {
            PrefixLine = prefixLine;
            Prefix = prefix;
        }
        public void WriteLine(string line)
        {
            OutputFile.WriteLine(line);
        }
        public override void Write(string text, int level = 0)
        {
            string line;
            string indent = "";

            for (int i = 0; i < level; i++)
                indent += "    ";

            if (PrefixLine)
                line = Prefix + ":" + indent + text;
            else
                line = indent + text;

            WriteLine(line);
        }
    }
    internal abstract class LoggerOutputFile
    {
        protected StreamWriter OutputFile { get; }
        internal LoggerOutputFile(StreamWriter streamDestination)
        {
            OutputFile = streamDestination;
        }
        public abstract void Write(string text, int level = 0);
    }

    internal class LoggerWriterData
    {
        public CancellationToken CancelToken { get; set; }
        public StatusController Status { get; set; }
        public List<string> Files { get; set; }
        public bool OutputSingleFile { get; set; }
        public int NumThreads { get; set; }
        public string SourcePath { get; set; }
        public string OutputLocation { get; set; }
        public bool CreateFolders { get; set; }
        public bool PrefixFileName { get; set; }
        public bool OverwriteFiles { get; set; }
    }
    internal struct LoggerCR2WOptions
    {
        public int StartingIndentLevel { get; set; }
        public bool ListEmbedded { get; set; }
        public bool DumpSDB { get; set; }
        public bool DumpFCD { get; set; }
        public bool DumpYML { get; set; }
        public bool DumpTXT { get; set; }
        public bool LocalizeStrings { get; set; }
    }
    internal class LoggerCR2W
    {
        public int ExceptionCount = 0;
        public delegate void OnExceptionDelegate(string msg, Exception e);
        public OnExceptionDelegate OnException;
        private CR2WFile CR2W { get; }
        private string CR2WFilePath { get; set; }
        private LoggerOutputFile Writer { get; }
        private LoggerOutputFile WriterYml { get; }
        private LoggerCR2WOptions Options { get; set; }
        private List<CR2WExportWrapper> Chunks { get; }
        private List<CR2WEmbeddedWrapper> Embedded { get; }
        private HashSet<string> wasDumped = new HashSet<string>();
        private List<string> simpleTypes = new List<string> { "CName", "Float", "Int", "Bool", "CGUID", "String" };
        private List<string> enumTypes = new List<string> { "EDrawableFlags", "ELightChannel", "EInterpMethodType", "EInterpCurveMode", "ECompareOp", "ESpaceFillMode", "EComboAttackResponse", "ECameraState", "ECameraShakeState", "ECameraShakeMagnitude", "EDismembermentEffectTypeFlag", "ETriggerChannel", "EDayPart", "EGameplayMimicMode", "EPlayerGameplayMimicMode", "ESoundGameState", "ESoundEventSaveBehavior", "EStaticCameraAnimState", "EStaticCameraGuiEffect", "ECharacterPowerStats", "ECharacterRegenStats", "EDirection", "EDirectionZ", "EMoonState", "EWeatherEffect", "EScriptedEventCategory", "EScriptedEventType", "EInputDeviceType", "EncumbranceBoyMode", "EActorImmortalityMode", "EActorImmortalityChanel", "ETerrainType", "EAreaName", "EDlcAreaName", "EZoneName", "EHitReactionType", "EFocusHitReaction", "EAttackSwingType", "EAttackSwingDirection", "EManageGravity", "ECounterAttackSwitch", "EAttitudeGroupPriority", "ETimescaleSource", "EMonsterCategory", "EButtonStage", "EStaminaActionType", "EFocusModeSoundEffectType", "EStatistic", "EAchievement", "ETutorialHintDurationType", "ETutorialHintPositionType", "ESpeedType", "EBloodType", "EStatOwner", "ETestSubject", "ETargetName", "EMonsterTactic", "EOperator", "ESpawnPositionPattern", "ESpawnRotation", "EFlyingCheck", "ECriticalEffectCounterType", "EFairytaleWitchAction", "EActionInfoType", "EBossAction", "EBossSpecialAttacks", "EEredinPhaseChangeAction", "ESpawnCondition", "ENPCCollisionStance", "ENPCBaseType", "EGuardState", "ENPCType", "EChosenTarget", "ETeleportType", "ECameraAnimPriority", "ECameraBlendSpeedMode", "EMerchantMapPinType", "EScriptedDetroyableComponentState", "EFoodGroup", "EClimbProbeUsed", "ESideSelected", "EPlayerCollisionStance", "EMovementCorrectionType", "EGameplayContextInput", "EExplorationStateType", "EBehGraphConfirmationState", "EAirCollisionSide", "EClimbRequirementType", "EClimbRequirementVault", "EClimbRequirementPlatform", "EClimbHeightType", "EClimbDistanceType", "EClimbEndReady", "EOutsideCapsuleState", "EPlayerIdleSubstate", "ExplorationInteractionType", "EJumpSubState", "EJumpType", "ELandPredictionType", "ELandType", "ELandRunForcedMode", "EPushSide", "ESlidingSubState", "ESlideCameraShakeState", "EFallType", "ECollisionTrajecoryStatus", "ECollisionTrajecoryExplorationStatus", "ECollisionTrajectoryPart", "ECollisionTrajectoryToWaterState", "EDoorMarkingState", "EntityType", "ESkillColor", "ESkillPath", "ESkillSubPath", "EPlayerMutationType", "EActionHitAnim", "EAlchemyExceptions", "EAlchemyCookedItemType", "EBirdType", "EWhaleMovementPatern", "EJobTreeType", "ECraftsmanType", "ECraftingException", "ECraftsmanLevel", "EItemUpgradeException", "EElevatorSwitchType", "ETrapOperation", "EEffectInteract", "EEffectType", "ECriticalHandling", "EEncounterMonitorCounterType", "EEncounterSpawnGroup", "EFocusModeChooseEntityStrategy", "ETriggeredDamageType", "EIllusionDiscoveredOneliner", "EDoorOperation", "EMonsterNestType", "ENestType", "ENewDoorOperation", "EShrineBuffs", "EToxicCloudOperation", "EOilBarrelOperation", "EArmorType", "EEquipmentSlots", "EItemGroup", "EInventoryFilterType", "EInventoryActionType", "ECompareType", "ESpendablePointType", "EEP2PoiType", "EPhysicalDamagemechanismOperation", "ESwitchState", "EResetSwitchMode", "ERequiredSwitchState", "EEncounterOperation", "EFactOperation", "EOcurrenceTime", "ELogicalOperator", "EBoidClueState", "EMonsterCluesTypes", "EMonsterSize", "EMonsterEmittedSound", "EMonsterDamageMarks", "EMonsterVictimState", "EMonsterApperance", "EMonsterSkinFacture", "EMonsterMovement", "EMonsterBehaviour", "EMonsterAttitude", "EMonsterAttackTime", "EMonsterHideout", "EFocusClueAttributeAction", "EClueOperation", "EFocusClueMedallionReaction", "EPlayerVoicesetType", "EMonsterClueAnim", "EReputationLevel", "EFactionName", "ETutorialMessageType", "EUITutorialTriggerCondition", "EUserDialogButtons", "EUniqueMessageIDs", "ELockedControlScheme", "EGamepadType", "ECursorType", "EGuiSceneControllerRenderFocus", "EQuantityTransferFunction", "EHudVisibilitySource", "EFloatingValueType", "EUpdateEventType", "EMutationFeedbackType", "EIngameMenuConstants", "EMutationResourceType", "EBonusSkillSlot", "EInventoryMenuState", "ENotificationType", "EItemSelectionPopupMode", "EUserMessageAction", "EUserMessageProgressType", "EPreporationItemType", "EBackgroundNPCWork_Single", "EBackgroundNPCWork_Paired", "EBgNPCType", "EBackgroundNPCWomanWork", "EConverserType", "EDeathType", "EFinisherDeathType", "EActionFail", "ETauntType", "EBehaviorGraph", "EExplorationMode", "EAgonyType", "ENPCFightStage", "ECriticalStateType", "EHitReactionDirection", "EHitReactionSide", "EDetailedHitType", "EAttackType", "EChargeAttackType", "EDodgeType", "EDodgeDirection", "ETurnDirection", "ETargetDirection", "ENpcPose", "EFlightStance", "ENPCRightItemType", "ENPCLeftItemType", "EInventoryFundsType", "EWeaponSubType1Handed", "EWeaponSubType2Handed", "EWeaponSubTypeRanged", "ENpcWeapons", "ENpcFightingStyles", "EAnimalType", "EPlayerDeathType", "EAimType", "EPlayerMode", "EForceCombatModeReason", "EGeneralEnum", "EPlayerExplorationAction", "EPlayerBoatMountFacing", "EPlayerAttackType", "ESkill", "EItemSetBonus", "EItemSetType", "EPlayerCommentary", "EPlayerWeapon", "EPlayerRangedWeapon", "EPlayerCombatStance", "ESignType", "EMoveSwitchDirection", "EPlayerEvadeType", "EPlayerEvadeDirection", "EPlayerParryDirection", "EPlayerRepelType", "ERotationRate", "EItemType", "ESpecialAbilityInput", "EThrowStage", "EParryStage", "EParryType", "EAttackSwingRange", "EInputActionBlock", "EPlayerMoveType", "EPlayerActionToRestore", "EPlayerInteractionLock", "EPlayerPreviewInventory", "EDismembermentWoundTypes", "ERecoilLevel", "EPlayerMovementLockType", "EHorseMode", "ECustomCameraType", "ECustomCameraController", "EInitialAction", "EDir", "EPlayerStopPose", "EVehicleCombatAction", "EBookDirection", "EQuestSword", "EFactValueChangeMethod", "EMapPinStatus", "EFocusEffectActivationAction", "ECameraEffect", "EQuestReplacerEntities", "EItemSelectionType", "EQuestNPCStates", "EDrawWeaponQuestType", "ESwarmStateOnArrival", "EAnimalReaction", "EDoorQuestState", "EGeraltPath", "EDM_MappinType", "EGwentCardFaction", "EGwentDeckUnlock", "EEnableMode", "EHudTimeOutAction", "EQuestPadVibrationStrength", "ELanguageCheckType", "ECheckedLanguage", "EQuestConditionDLCType", "EContainerMode", "EQuestPlayerSkillLevel", "EQuestPlayerSkillCondition", "EQuestConditionPlayerState", "ESwitchStateCondition", "EPlayerReplacerType", "EStorySceneOutputAction", "EStorySceneGameplayAction", "ENegotiationResult", "ECollectItemsRes", "ECollectItemsCustomRes", "EHorseWaterTestResult" };
        private List<string> exclNames = new List<string> { "unknownBytes", "Parent", "flatCompiledData", "SharedDataBuffer", "attachments reference" };

        private static CR2WFile LoadCR2W(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                using (var reader = new BinaryReader(fs))
                    return new CR2WFile(reader);
        }
        internal LoggerCR2W(string fileName, LoggerOutputFile writer, LoggerOutputFile writerYml, LoggerCR2WOptions options)
            : this(LoadCR2W(fileName), fileName, writer, writerYml, options) {}
        internal LoggerCR2W(CR2WFile cr2wFile, string filePath, LoggerOutputFile writer, LoggerOutputFile writerYml, LoggerCR2WOptions options)
        {
            Console.WriteLine("LoggerCR2W:: LoggerCR2W()");
            CR2WFilePath = filePath;
            CR2W = cr2wFile;
            Chunks = CR2W.chunks;
            Embedded = CR2W.embedded;
            Writer = writer;
            WriterYml = writerYml;
            Options = options;
            if (Options.LocalizeStrings)
                CR2W.LocalizedStringSource = MainController.Get();
        }
        string prepareName(string name)
        {
            if ( name.All(Char.IsDigit) )
            {
                name = '\"' + name + '\"';
            }
            int pos = name.IndexOf('#');
            if (pos > 0)
                return name.Substring(0, pos - 1);
            else
                return name;
        }
        bool isArrayType(string type)
        {
            return !string.IsNullOrEmpty(type) && (type.StartsWith("array:") || type == "TagList" || enumTypes.Contains(type) || type.StartsWith("[") || type.Contains("<WolvenKit.CR2W.Types.SCurveData>"));
        }
        public void processCR2W(int overrideLevel = 0)
        {
            Console.WriteLine("LoggerCR2W:: processCR2W()");
            int level = (overrideLevel > 0) ? overrideLevel : Options.StartingIndentLevel;
            if (Options.ListEmbedded && Embedded != null && Embedded.Any())
            {
                Writer.Write("Embedded files:", level);
                ProcessEmbedded(level);
            }
            Writer.Write("Chunks:", level);
            foreach (var chunk in Chunks)
            {
                var dumpYml = Options.DumpYML && !wasDumped.Contains(chunk.Name);
                wasDumped.Add(chunk.Name);
                if (dumpYml)
                    WriterYml.Write(CR2WFilePath.Split('\\').Last() + ":", level);
                if (Options.DumpTXT)
                    Writer.Write(chunk.Name + " >>(" + chunk.Type + ")<< : " + chunk.Preview, level);

                var node = GetNodes(chunk);
                if (Options.DumpTXT)
                    ProcessNode(node, level + 1);
                if (dumpYml)
                    ProcessNodeYml(node, level + 1, false, true);
            }
        }
        private void ProcessEmbedded(int level)
        {
            //TODO: Also dump the embedded files? (optionally)
            int fileCounter = 1;
            foreach (CR2WEmbeddedWrapper embed in Embedded)
            {
                Writer.Write("(" + fileCounter++ + "):", level);
                Writer.Write("Index: " + embed.Embedded.importIndex, level + 1);
                Writer.Write("ImportPath: " + embed.ImportPath, level + 1);
                Writer.Write("ImportClass: " + embed.ImportClass, level + 1);
                Writer.Write("Size: " + embed.Embedded.dataSize, level + 1);
                Writer.Write("ClassName: " + embed.ClassName, level + 1);
                Writer.Write("Handle: " + embed.Handle, level + 1);
            }
        }
        List<string> getUnsupportedVarsByType(string type)
        {
            if (type.StartsWith("CFXTrackItem"))
                type = "CFXTrackItem";
            List<string> ret = new List<string>();
            switch (type)
            {
                case "CFXTrackItem":
                    ret.Add("count");
                    ret.Add("unk");
                    ret.Add("buffername");
                    break;
                case "CFXDefinition":
                case "CFXTrack":
                case "CFXTrackGroup":
                    ret.Add("name");
                    break;
            }
            return ret;
        }
        string prepareValue(VariableListNode node, string type = "")
        {
            if (type == "")
                type = node.Type;
            Func<string, string> wrapValue = x =>
            {
                string ret = x.Replace('\\', '/');
                int pos = ret.LastIndexOf(": ");
                if (pos > 0)
                    ret = ret.Substring(pos + 2);
                return ret;
            };
            if (string.IsNullOrEmpty(type))
            {
                return string.IsNullOrEmpty(node.Value) ? "" : wrapValue(node.Value);
            } else if (isArrayType(type))
            {
                return "# " + type;
            }
            else if (type == "Bool")
            {
                return (type == "True" ? "true" : "false");
            }
            else if (type == "Float")
            {
                string ret = node.Value.Replace(',','.');
                if (!ret.Contains('.'))
                {
                    ret += ".0";
                }
                return ret;
            }
            else if (type == "String")
            {
                return "\"" + node.Value + "\"";
            }
            else if (type == "CName")
            {
                return node.Value;
            }
            else
            {
                return wrapValue(node.Value);
            }
        }
        private void ProcessNodeYml_EngineTransform(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            WriterYml.Write((isArrayElement ? "- " : "") + prepareName(node.Name) + ":", cur_level);
            string x = "0.0", y = "0.0", z = "0.0", pitch = "0.0", yaw = "0.0", roll = "0.0", scale_x = "1.0", scale_y = "1.0", scale_z = "1.0";
            if (node.ChildCount > 0)
                foreach (var child in node.Children)
                {
                    switch (child.Name)
                    {
                        case "x":
                            x = prepareValue(child);
                            break;
                        case "y":
                            y = prepareValue(child);
                            break;
                        case "z":
                            z = prepareValue(child);
                            break;
                        case "pitch":
                            pitch = prepareValue(child);
                            break;
                        case "yaw":
                            yaw = prepareValue(child);
                            break;
                        case "roll":
                            roll = prepareValue(child);
                            break;
                        case "scale x":
                            scale_x = prepareValue(child);
                            break;
                        case "scale y":
                            scale_y = prepareValue(child);
                            break;
                        case "scale z":
                            scale_z = prepareValue(child);
                            break;
                    }
                }
            WriterYml.Write("pos: [ " + x + ", " + y + ", " + z + " ]", cur_level + 1);
            WriterYml.Write("rot: [ " + pitch + ", " + yaw + ", " + roll + " ]", cur_level + 1);
            WriterYml.Write("scale: [ " + scale_x + ", " + scale_y + ", " + scale_z + " ]", cur_level + 1);
        }
        private void ProcessNodeYml_Vector(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            string X = "0.0", Y = "0.0", Z = "0.0", W = "0.0";
            if (node.ChildCount > 0)
                foreach (var child in node.Children)
                {
                    switch (child.Name)
                    {
                        case "X":
                            X = prepareValue(child);
                            break;
                        case "Y":
                            Y = prepareValue(child);
                            break;
                        case "Z":
                            Z = prepareValue(child);
                            break;
                        case "W":
                            W = prepareValue(child);
                            break;
                    }
                }
            WriterYml.Write((isArrayElement ? "- " : "") + prepareName(node.Name) + ": [ " + X + ", " + Y + ", " + Z + ", " + W + " ]", cur_level);
        }
        private void ProcessNodeYml_Color(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            string Red = "0.0", Green = "0.0", Blue = "0.0", Alpha = "-1.0";
            if (node.ChildCount > 0)
                foreach (var child in node.Children)
                {
                    switch (child.Name)
                    {
                        case "Red":
                            Red = prepareValue(child);
                            break;
                        case "Green":
                            Green = prepareValue(child);
                            break;
                        case "Blue":
                            Blue = prepareValue(child);
                            break;
                        case "Alpha":
                            Alpha = prepareValue(child);
                            break;
                    }
                }
            WriterYml.Write((isArrayElement ? "- " : "") + prepareName(node.Name) + ": [ " + Red + ", " + Green + ", " + Blue + ((Alpha == "-1.0") ? "" : (", " + Alpha)) + " ]", cur_level);
        }
        private void ProcessNodeYml_cookedEffects(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            if (node.ChildCount < 1)
                return;
            WriterYml.Write("effects:", cur_level);
            ++cur_level;
            int nonameEffectsCount = 0;

            foreach (var i_child in node.Children)
            {
                if (i_child.ChildCount < 1)
                    return;
                string name = "";
                string buffer_size = "";
                VariableListNode bufferNode = null;
                foreach (var j_child in i_child.Children)
                {
                    if (j_child.Name == "name")
                        name = j_child.Value;
                    else if (j_child.Type == "SharedDataBuffer")
                    {
                        bufferNode = j_child;
                        buffer_size = j_child.Value;
                    }
                }
                if (bufferNode != null)
                {
                    if (string.IsNullOrEmpty(name))
                        name = "noname_effect_" + nonameEffectsCount++;
                    WriterYml.Write("#buffer: " + buffer_size, cur_level);
                    bufferNode.Name = name;
                    ProcessCR2WChunk(bufferNode, cur_level, true);
                }
            }
        }
        private void ProcessNodeYml_interpolationBuffer(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            if (node.ChildCount < 1)
                return;
            string[,] vars = new string[16, 4];
            int rows = 0;

            foreach (var child in node.Children)
            {
                if (child.ChildCount < 16)
                    return;
                for (int j = 0; j < child.ChildCount && j < 16; ++j)
                {
                    vars[j, rows] = prepareValue(child.Children[j], "Float");
                }
                ++rows;
            }

            WriterYml.Write("interpolation:", cur_level);
            for (int j = 0; j < 16; ++j)
            {
                string tmp_var = "- [ " + vars[j, 0];
                for (int i = 1; i < rows; ++i)
                {
                    tmp_var += ", " + vars[j, i];
                }
                WriterYml.Write(tmp_var + " ]", cur_level + 1);
            }
        }
        private void ProcessNodeYml(VariableListNode node, int cur_level, bool isArrayElement = false, bool isNonamed = false)
        {
            if (node == null)
            {
                Console.WriteLine(" > ProcessNodeYml::NULL node!");
                return;
            }
            Console.WriteLine("  > ProcessNodeYML::name=" + node.Name + ", type=" + node.Type +", Value="+node.Value+", childCount="+node.ChildCount+", arrayElement="+isArrayElement);
            // special cases for rmemr encoder
            if (node.Type == "EngineTransform")
            {
                ProcessNodeYml_EngineTransform(node, cur_level, isArrayElement, isNonamed);
                return;
            }
            if (node.Type == "Vector")
            {
                ProcessNodeYml_Vector(node, cur_level, isArrayElement, isNonamed);
                return;
            }
            if (node.Type == "Color")
            {
                ProcessNodeYml_Color(node, cur_level, isArrayElement, isNonamed);
                return;
            }
            if (node.Name == "cookedEffects")
            {
                ProcessNodeYml_cookedEffects(node, cur_level, isArrayElement, isNonamed);
                return;
            }
            if (node.Name == "buffer" && string.IsNullOrEmpty(node.Type))
            {
                ProcessNodeYml_interpolationBuffer(node, cur_level, isArrayElement, isNonamed);
                return;
            }

            bool isMeArray = isArrayType(node.Type);
            string suggestedArrayType = "";
            if (isMeArray || isArrayElement)
            {
                suggestedArrayType = node.Type.Substring( node.Type.LastIndexOfAny(new char[] { ',', ':' }) + 1 );
                Console.WriteLine("      ! suggested type: " + suggestedArrayType);
            }

            if (node.Name == "Buffer_v1" || node.Name == "Buffer_v2" || node.Name == "unknownBytes"
                || node.Name == "unk1" && node.Value == "0")
                return;

            if (node.Name == "Parent" && node.Value == "NULL")
                return;

            if (node.Name == "child attachments" && node.ChildCount == 0)
                return;

            if ( !exclNames.Contains(node.Name) && node.Value != "NULL" )
            {
                if (node.Name == node.Value)
                {
                    Console.WriteLine("  vv ProcessNodeYML::Good! Duplicated map");
                    if (node.ChildCount > 0)
                    {
                        List<string> unsupportedVars = getUnsupportedVarsByType(node.Type);
                        foreach (var child in node.Children)
                        {
                            if (isMeArray || isArrayElement)
                            {
                                if (string.IsNullOrEmpty(child.Type))
                                {
                                    child.Variable.Type = suggestedArrayType;
                                }
                            }
                            if (!unsupportedVars.Contains(child.Name))
                                ProcessNodeYml(child, cur_level, isMeArray || isArrayElement, isNonamed);
                        }
                    }
                    return;
                }
                else if (node.Value.Contains("#")) // chunk reference - dump it now
                {
                    Console.WriteLine("  >> ProcessNodeYML::Good! Chunk");

                    bool chunkFound = false;
                    foreach (var chunk in Chunks)
                    {
                        if (!wasDumped.Contains(chunk.Name) && chunk.Name == node.Value)
                        {
                            VariableListNode nodeDFS = GetNodes(chunk);
                            wasDumped.Add(chunk.Name);
                            if (isMeArray || isArrayElement)
                            {
                                if (string.IsNullOrEmpty(nodeDFS.Type))
                                {
                                    nodeDFS.Variable.Type = suggestedArrayType;
                                }
                            }
                            if (!isArrayElement)
                            {
                                WriterYml.Write(prepareName(node.Name) + ":", cur_level);
                                //WriterYml.Write("- \".type\": " + node.Type, cur_level);
                            }
                            if (isArrayElement)
                                ProcessNodeYml(nodeDFS, cur_level, true, false);
                            else
                                ProcessNodeYml(nodeDFS, cur_level + 1, false, true);
                            chunkFound = true;
                            break;
                        }
                    }
                    if (!chunkFound)
                    {
                        WriterYml.Write("#" + prepareName(node.Name) + ": (looped reference) " + node.Value, cur_level);
                    }
                }
                else
                {
                    Console.WriteLine("  ## ProcessNodeYML::Good! Scalar");
                    if (isArrayElement)
                    {
                        if ( node.ChildCount == 0 )
                        {
                            WriterYml.Write("- " + prepareValue(node), cur_level);
                        }
                        else
                        {
                            // special case for rmemr encoder (though they are array elements in real)
                            if (node.Type == "CFXTrackGroup" || node.Type == "CFXTrack")
                            {
                                string name = "unnamed";
                                foreach (var child in node.Children)
                                {
                                    if (child.Name == "name")
                                    {
                                        name = child.Value;
                                        //node.Children.Remove(child);
                                        break;
                                    }
                                }
                                WriterYml.Write(prepareName(name) + ":", cur_level);
                                WriterYml.Write("\".type\": " + node.Type, cur_level + 1);
                            } else
                            {
                                WriterYml.Write("- \".type\": " + node.Type, cur_level);
                            }
                        }
                    }
                    else
                    {
                        if (node.ChildCount == 0)
                        {
                            WriterYml.Write(prepareName(node.Name) + ": " + prepareValue(node), cur_level);
                        }
                        else
                        {
                            if (isNonamed)
                                WriterYml.Write("\".type\": " + node.Type, cur_level);
                            else
                                WriterYml.Write(prepareName(node.Name) + ":  #" + node.Type, cur_level);
                        }
                    }
                }
                if (node.ChildCount > 0)
                {
                    List<string> unsupportedVars = getUnsupportedVarsByType(node.Type);
                    foreach (var child in node.Children)
                    {
                        if (isMeArray)
                        {
                            if (string.IsNullOrEmpty(child.Type))
                            {
                                child.Variable.Type = suggestedArrayType;
                            }
                        }
                        if (!unsupportedVars.Contains(child.Name))
                            ProcessNodeYml(child, cur_level + (isNonamed ? 0 : 1), isMeArray);
                    }
                }
            }

            if ( (node.Type == "SharedDataBuffer" && Options.DumpSDB) 
                || (node.Type == "array:2,0,Uint8" && node.Name != "deltaTimes") ) 
            {   // Embedded CR2W dump:
                // Dump SharedDataBuffer if option is set.
                // Dump "array:2,0,Uint8", unless it's called "deltaTimes" (not CR2W)
                // And dump FCD only if options.dumpFCD is set.
                if (node.Name == "flatCompiledData" && !Options.DumpFCD)
                    return;
                ProcessCR2WChunk(node, cur_level, true);
            }
        }
        private void ProcessCR2WChunk(VariableListNode node, int cur_level, bool ymlDump)
        {
            try
            {
                var ls = new LoggerService();
                CR2WFile embedcr2w = new CR2WFile(((IByteSource)node.Variable).Bytes, ls);
                LoggerCR2WOptions newOptions = Options;
                if (ymlDump)
                    newOptions.DumpTXT = false;
                else
                    newOptions.DumpYML = false;

                var lc = new LoggerCR2W(embedcr2w, node.Name, Writer, WriterYml, newOptions);
                lc.processCR2W(cur_level);
            }
            catch (FormatException)
            {
                // Embedded buffer/array:2,0,Uint8 was not a CR2W file. Do nothing.
            }
            catch (Exception e)
            {
                string msg = node.Name + ":" + node.Type + ": ";
                string logMsg = msg + ": Buffer or 'array:2,0,Uint8' caught exception: ";
                WriterYml.Write(logMsg + e, cur_level);
                Console.WriteLine(logMsg + e);
                OnException?.Invoke(msg, e);
                ExceptionCount++;
            }
        }
        private void ProcessNode(VariableListNode node, int level)
        {
            //Console.WriteLine("  > ProcessNode::name=" + node.Name + ", Value="+node.Value+", childCount="+node.ChildCount);
            if (node.Name == "unknownBytes" && node.Value == "0 bytes"
                || node.Name == "unk1" && node.Value == "0")
                return;

            if (node.Name == "Parent" && node.Value == "NULL")
                return;

            if (node.Name != node.Value) // Chunk node is already printed in processCR2W, so don't print it again.
            {
                Writer.Write(node.Name + " (" + node.Type + ") : " + node.Value, level);
                level++;
            }

            if (node.ChildCount > 0)
                foreach (var child in node.Children)
                    ProcessNode(child, level);

            if ((node.Type == "SharedDataBuffer" && Options.DumpSDB)
                || (node.Type == "array:2,0,Uint8" && node.Name != "deltaTimes"))
            {   // Embedded CR2W dump:
                // Dump SharedDataBuffer if option is set.
                // Dump "array:2,0,Uint8", unless it's called "deltaTimes" (not CR2W)
                // And dump FCD only if options.dumpFCD is set.
                if (node.Name == "flatCompiledData" && !Options.DumpFCD)
                    return;
                ProcessCR2WChunk(node, level, false);
            }
        }
        private VariableListNode GetNodes(CR2WExportWrapper chunk)
        {
            return frmChunkProperties.AddListViewItems(chunk);
        }
    }
}
