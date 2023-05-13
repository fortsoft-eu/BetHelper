/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022-2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 **
 * Version 1.1.11.2
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class LogViewerForm : Form {
        private bool cancel, invertDirection, lastMatch, searching, topMost;
        private byte[] byteSettings = new byte[] { 0, 1, 1, 1 };
        private CountDownForm countDownForm;
        private FileExtensionFilter fileExtensionFilter;
        private FindForm findForm;
        private Font printFont;
        private Form dialog;
        private int availableHeight, availableWidth, doEvents, matchIndex, textBoxClicks, selectedIndex;
        private List<SearchResult> searchResults;
        private LogViewerState[] logViewerStates;
        private PersistWindowState persistWindowState;
        private Point location;
        private PrintAction printAction;
        private PrintDialog printDialog;
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        private PrintType printType;
        private SaveFileDialog saveFileDialog;
        private Search search;
        private Settings settings;
        private Size availableSize;
        private StatusStripHandler statusStripHandler;
        private string logFilePath, stringToPrint;
        private System.Windows.Forms.Timer textBoxClicksTimer;
        private Thread findThread, turnOffThread;
        private UpdateChecker updateChecker;

        public LogViewerForm(Settings settings) {
            this.settings = settings;

            Icon = Properties.Resources.Logs;
            Text = new StringBuilder()
                .Append(Program.GetTitle())
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(Properties.Resources.CaptionLogViewer)
                .ToString();

            textBoxClicksTimer = new System.Windows.Forms.Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            fileExtensionFilter = new FileExtensionFilter(settings.ExtensionFilterIndex);
            persistWindowState = new PersistWindowState();
            persistWindowState.DetectionOptions = PersistWindowState.WindowDetectionOptions.NoDetection;
            persistWindowState.Parent = this;
            persistWindowState.Loaded += new EventHandler<PersistWindowStateEventArgs>(OnWindowStateLoaded);
            persistWindowState.Saved += new EventHandler<PersistWindowStateEventArgs>(OnWindowStateSaved);

            InitializePrintAsync();
            InitializeUpdateCheckerAsync();
            InitializeSaveFileDialogAsync();
            BuildMainMenuAsync();

            InitializeComponent();

            SuspendLayout();
            SetTabControl();
            SetLogViewerStates();
            BuildContextMenuAsync();
            InitializeStatusStripHandler();
            ResumeLayout(false);
            PerformLayout();

            fileSystemWatcher.Path = Application.LocalUserAppDataPath;
            fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size;
            fileSystemWatcher.Changed += new FileSystemEventHandler(OnFileSystemWatcher);
            fileSystemWatcher.Created += new FileSystemEventHandler(OnFileSystemWatcher);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(OnFileSystemWatcher);
            fileSystemWatcher.Error += new ErrorEventHandler(OnFileSystemWatcherError);
            fileSystemWatcher.Filter = Constants.LogFileExtension;
            fileSystemWatcher.IncludeSubdirectories = false;

            MouseWheel += new MouseEventHandler(OnMouseWheel);
            MouseMove += new MouseEventHandler(OnMouseWheel);
        }

        private async void BuildMainMenuAsync() {
            await Task.Run(new Action(() => {
                MainMenu mainMenu = new MainMenu();
                MenuItem menuItemFile = mainMenu.MenuItems.Add(Properties.Resources.MenuItemFile);
                MenuItem menuItemEdit = mainMenu.MenuItems.Add(Properties.Resources.MenuItemEdit);
                MenuItem menuItemView = mainMenu.MenuItems.Add(Properties.Resources.MenuItemView);
                MenuItem menuItemOptions = mainMenu.MenuItems.Add(Properties.Resources.MenuItemOptions);
                MenuItem menuItemTools = mainMenu.MenuItems.Add(Properties.Resources.MenuItemTools);
                MenuItem menuItemHelp = mainMenu.MenuItems.Add(Properties.Resources.MenuItemHelp);
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExport,
                    new EventHandler(Export), Shortcut.CtrlE));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintPreview,
                    new EventHandler(PrintPreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrint,
                    new EventHandler(Print), Shortcut.CtrlP));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintTailPreview,
                    new EventHandler(PrintTailPreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintTail + Constants.ShortcutShiftCtrlP,
                    new EventHandler(PrintTail)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImagePreview,
                    new EventHandler(PrintImagePreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImage + Constants.ShortcutAltShiftCtrlP,
                    new EventHandler(PrintImage)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExit,
                    new EventHandler((sender, e) => Close()), Shortcut.AltF4));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy + Constants.ShortcutCtrlC,
                    new EventHandler(Copy)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll + Constants.ShortcutCtrlA,
                    new EventHandler(SelectAll)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemFind,
                    new EventHandler(Find), Shortcut.CtrlF));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemFindNext + Constants.ShortcutF3,
                    new EventHandler(FindNext)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPause,
                    new EventHandler(TogglePause), Shortcut.F9));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReload,
                    new EventHandler(Reload), Shortcut.F5));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReloadFromStart,
                    new EventHandler(ReloadFromStart), Shortcut.AltF7));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReloadTail,
                    new EventHandler(ReloadTail), Shortcut.F7));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemClear,
                    new EventHandler(Clear), Shortcut.F8));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAlwaysOnTop + Constants.ShortcutShiftCtrlT,
                    new EventHandler(ToggleTopMost)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemWordWrap + Constants.ShortcutShiftCtrlL,
                    new EventHandler(ToggleWordWrap)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectionCopies,
                    new EventHandler((sender, e) => ((MenuItem)sender).Checked = !((MenuItem)sender).Checked)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDoubleClickCopies,
                    new EventHandler((sender, e) => ((MenuItem)sender).Checked = !((MenuItem)sender).Checked)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTripleClickCopies,
                    new EventHandler((sender, e) => ((MenuItem)sender).Checked = !((MenuItem)sender).Checked)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenInExternalEditor,
                    new EventHandler(OpenInEditor), Shortcut.CtrlU));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenInDefaultEditor,
                    new EventHandler(OpenInDefaultEditor)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenLogLocation,
                    new EventHandler(OpenLogLocation), Shortcut.CtrlL));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyLogFileName,
                    new EventHandler(CopyLogFileName), Shortcut.CtrlJ));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyLogFilePath,
                    new EventHandler(CopyLogFilePath), Shortcut.CtrlM));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyDirectoryPath,
                    new EventHandler(CopyDirectoryPath), Shortcut.CtrlK));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTruncateLog,
                    new EventHandler(TruncateLog)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAnalyzeUrl,
                    new EventHandler(AnalyzeUrl)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemEncoderDecoder + Constants.ShortcutShiftCtrlN,
                    new EventHandler(OpenEncoderDecoder)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTurnOffMonitors,
                    new EventHandler(TurnOffMonitors), Shortcut.F11));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchCalculator,
                    new EventHandler(LaunchCalculator), Shortcut.AltF11));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchNotepad,
                    new EventHandler(LaunchNotepad), Shortcut.AltF12));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchWordPad,
                    new EventHandler(LaunchWordPad)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchCharMap,
                    new EventHandler(LaunchCharMap)));
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOnlineHelp,
                    new EventHandler(OpenHelp), Shortcut.F1));
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCheckForUpdates,
                    new EventHandler(CheckUpdates)));
                menuItemHelp.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAbout,
                    new EventHandler(ShowAbout)));
                Menu = mainMenu;
                menuItemEdit.Popup += new EventHandler((sender, e) => {
                    menuItemEdit.MenuItems[0].Enabled = logViewerStates[selectedIndex].TextBox.SelectionLength > 0;
                    menuItemEdit.MenuItems[2].Enabled =
                        logViewerStates[selectedIndex].TextBox.SelectionLength < logViewerStates[selectedIndex].TextBox.TextLength;
                    menuItemEdit.MenuItems[5].Enabled = !string.IsNullOrEmpty(search.searchString);
                });
                menuItemView.Popup += new EventHandler((sender, e) => menuItemView.MenuItems[3].Enabled = !IsRestricted());
                menuItemOptions.Popup += new EventHandler((sender, e) => menuItemOptions.MenuItems[2].Enabled = !IsRestricted());
                menuItemTools.Popup += new EventHandler((sender, e) => {
                    menuItemTools.MenuItems[10].Enabled = StaticMethods.CheckSelectedUrl(logViewerStates[selectedIndex].TextBox);
                    menuItemTools.MenuItems[0].Enabled = false;
                    try {
                        menuItemTools.MenuItems[0].Enabled = !string.IsNullOrWhiteSpace(settings.ExternalEditor)
                            && File.Exists(settings.ExternalEditor);
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
            }));
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler(Copy)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler(SelectAll)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAnalyzeUrl,
                    new EventHandler(AnalyzeUrl)));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = textBox.SelectionLength < textBox.TextLength;
                    bool visible = StaticMethods.CheckSelectedUrl(textBox);
                    contextMenu.MenuItems[3].Visible = visible;
                    contextMenu.MenuItems[4].Visible = visible;
                });
                foreach (LogViewerState logViewerState in logViewerStates) {
                    logViewerState.TextBox.ContextMenu = contextMenu;
                }
            }));
        }

        private async void InitializePrintAsync() {
            await Task.Run(new Action(() => {
                printDialog = new PrintDialog();
                printDocument = new PrintDocument();
                printDocument.DocumentName = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionPrintOutput)
                    .ToString();
                printDocument.BeginPrint += new PrintEventHandler(BeginPrint);
                printDocument.PrintPage += new PrintPageEventHandler(PrintPage);
                printDocument.EndPrint += new PrintEventHandler(EndPrint);
                printDialog.Document = printDocument;
                printAction = PrintAction.PrintToPrinter;
                printPreviewDialog = new PrintPreviewDialog() {
                    ShowIcon = false,
                    UseAntiAlias = true,
                    Document = printDocument
                };
                printPreviewDialog.WindowState = FormWindowState.Normal;
                printPreviewDialog.StartPosition = FormStartPosition.WindowsDefaultBounds;
                printPreviewDialog.HelpRequested += new HelpEventHandler(OpenHelp);
            }));
        }

        private async void InitializeSaveFileDialogAsync() {
            await Task.Run(new Action(() => {
                string initialDirectory = string.IsNullOrEmpty(settings.LastExportDirectory)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    : settings.LastExportDirectory;
                saveFileDialog = new SaveFileDialog() {
                    AddExtension = true,
                    CheckPathExists = true,
                    InitialDirectory = initialDirectory,
                    OverwritePrompt = true,
                    ValidateNames = true
                };
                saveFileDialog.HelpRequest += new EventHandler(OpenHelp);
            }));
        }

        private void InitializeStatusStripHandler() {
            statusStripHandler = new StatusStripHandler(statusStrip, StatusStripHandler.DisplayMode.Basic, settings);
        }

        private async void InitializeUpdateCheckerAsync() {
            await Task.Run(new Action(() => {
                updateChecker = new UpdateChecker(settings);
                updateChecker.Parent = this;
                updateChecker.StateChanged += new EventHandler<UpdateCheckEventArgs>(OnUpdateCheckerStateChanged);
                updateChecker.Help += new HelpEventHandler(OpenHelp);
            }));
        }

        private void ReadLog() {
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                if (!string.IsNullOrEmpty(logViewerStates[selectedIndex].Path) && File.Exists(logViewerStates[selectedIndex].Path)) {
                    using (FileStream fileStream = new FileStream(
                            logViewerStates[selectedIndex].Path,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite)) {

                        SetAction(fileStream);
                        fileStream.Seek(logViewerStates[selectedIndex].Position, SeekOrigin.Begin);
                        using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                            using (StringReader stringReader = new StringReader(streamReader.ReadToEnd())) {
                                long l = fileStream.Position - logViewerStates[selectedIndex].Position;
                                StringBuilder stringBuilder = new StringBuilder(l > int.MaxValue ? int.MaxValue : (int)l);
                                for (string line; (line = stringReader.ReadLine()) != null;) {
                                    stringBuilder.AppendLine(line);
                                }
                                logViewerStates[selectedIndex].TextBox.AppendText(stringBuilder.ToString());
                            }
                            logViewerStates[selectedIndex].Position = fileStream.Position;
                            statusStripHandler.SetDataSize(fileStream.Position);
                        }
                    }
                }
            } catch (OutOfMemoryException exception) {
                ShowException(exception, Properties.Resources.MessageOutOfMemoryError);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                try {
                    fileSystemWatcher.EnableRaisingEvents = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void SetAction(FileStream fileStream) {
            long l;
            switch (logViewerStates[selectedIndex].Action) {
                case LogViewerAction.Cleared:
                    logViewerStates[selectedIndex].TextBox.Clear();
                    logViewerStates[selectedIndex].Cleared = logViewerStates[selectedIndex].Position;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                    break;
                case LogViewerAction.Start:
                    logViewerStates[selectedIndex].TextBox.Clear();
                    logViewerStates[selectedIndex].Position = 0;
                    logViewerStates[selectedIndex].Cleared = -1;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                    break;
                case LogViewerAction.PreloadLimit:
                    logViewerStates[selectedIndex].TextBox.Clear();
                    l = fileStream.Length - settings.LogPreloadLimit * (settings.UseDecimalPrefix ? 1000 : 1024);
                    logViewerStates[selectedIndex].Position = l > 0 ? l : 0;
                    logViewerStates[selectedIndex].Cleared = 0;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                    break;
                case LogViewerAction.Reload:
                    logViewerStates[selectedIndex].TextBox.Clear();
                    switch (logViewerStates[selectedIndex].Cleared) {
                        case -1:
                            logViewerStates[selectedIndex].Position = 0;
                            logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                            break;
                        case 0:
                            l = fileStream.Length - settings.LogPreloadLimit * (settings.UseDecimalPrefix ? 1000 : 1024);
                            logViewerStates[selectedIndex].Position = l > 0 ? l : 0;
                            logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                            break;
                        default:
                            logViewerStates[selectedIndex].Position = logViewerStates[selectedIndex].Cleared;
                            logViewerStates[selectedIndex].Action = LogViewerAction.Append;
                            break;
                    }
                    break;
            }
        }

        private void SetLogViewerStates() {
            logViewerStates = new LogViewerState[tabControl.TabCount];
            for (int i = 0; i < tabControl.TabCount; i++) {
                logViewerStates[i] = new LogViewerState();
                logViewerStates[i].Action = settings.EnableLogPreloadLimit ? LogViewerAction.PreloadLimit : LogViewerAction.Start;
                logViewerStates[i].Cleared = settings.EnableLogPreloadLimit ? 0 : -1;
                logViewerStates[i].Path = Path.Combine(Application.LocalUserAppDataPath, tabControl.TabPages[i].Text);
                logViewerStates[i].TextBox = (TextBox)tabControl.TabPages[i].Controls[0];
            }
        }

        private void SetTabControl() {
            string[] logFiles = new string[] {
                Constants.ErrorLogFileName,
                Constants.CefDebugLogFileName,
                Constants.ForeignUrlsLogFileName,
                Constants.PopUpFrameHandlerLogFileName,
                Constants.ConsoleMessageLogFileName,
                Constants.LoadErrorLogFileName
            };
            foreach (string logFile in logFiles) {
                TabPage tabPage = new TabPage() { Text = logFile };
                TextBox textBox = new TextBox() {
                    BackColor = SystemColors.Window,
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    WordWrap = false
                };
                textBox.KeyDown += new KeyEventHandler(OnKeyDown);
                textBox.KeyUp += new KeyEventHandler(OnKeyUp);
                textBox.MouseDown += new MouseEventHandler(OnMouseDown);
                textBox.MouseUp += new MouseEventHandler(OnMouseUp);
                tabPage.Controls.Add(textBox);
                tabControl.TabPages.Add(tabPage);
            }
        }

        private void OnFileSystemWatcherError(object sender, System.IO.ErrorEventArgs e) {
            Exception exception = e.GetException();
            Debug.WriteLine(exception);
            ErrorLog.WriteLine(exception);
            StringBuilder title = new StringBuilder()
                .Append(Program.GetTitle())
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(Properties.Resources.CaptionError);
            dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
            dialog.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog.ShowDialog(this);
        }

        private void OnFileSystemWatcher(object sender, FileSystemEventArgs e) {
            if (!Menu.MenuItems[2].MenuItems[0].Checked && e.Name.Equals(tabControl.SelectedTab.Text)) {
                ReadLog();
            }
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (findForm != null && findForm.Visible && findForm.TopMost && Menu.MenuItems[3].MenuItems[0].Checked) {
                findForm.SafeBringToFront();
            }
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e) {
            if (InvokeRequired) {
                Invoke(new FormClosedEventHandler(OnFormClosed), sender, e);
            } else {
                logViewerStates[selectedIndex].TextBox.HideSelection = true;
                statusStripHandler.ClearSearchResult();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            if (!Menu.MenuItems[0].MenuItems[3].Enabled && e.CloseReason.Equals(CloseReason.UserClosing)) {
                dialog = new MessageForm(this, Properties.Resources.MessageQuestionCancelPrinting, Properties.Resources.CaptionQuestion,
                    MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Question, MessageForm.DefaultButton.Button2);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);
                if (dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                    cancel = true;
                } else {
                    e.Cancel = true;
                    return;
                }
            }
            if (countDownForm != null) {
                countDownForm.HelpRequested -= new HelpEventHandler(OpenHelp);
                if (countDownForm.Visible) {
                    countDownForm.SafeClose();
                }
            }
            if (findForm != null) {
                CloseFindForm();
            }
            settings.Dispose();
            statusStripHandler.Dispose();
            updateChecker.Dispose();
            textBoxClicksTimer.Dispose();
        }

        private void CloseFindForm() {
            findForm.AltCtrlShiftPPressed -= new EventHandler(PrintImage);
            findForm.AltF11Pressed -= new EventHandler(LaunchCalculator);
            findForm.AltF12Pressed -= new EventHandler(LaunchNotepad);
            findForm.AltF7Pressed -= new EventHandler(ReloadFromStart);
            findForm.CtrlEPressed -= new EventHandler(Export);
            findForm.CtrlJPressed -= new EventHandler(CopyLogFileName);
            findForm.CtrlKPressed -= new EventHandler(CopyDirectoryPath);
            findForm.CtrlLPressed -= new EventHandler(OpenLogLocation);
            findForm.CtrlMPressed -= new EventHandler(CopyLogFilePath);
            findForm.CtrlPPressed -= new EventHandler(Print);
            findForm.CtrlShiftLPressed -= new EventHandler(ToggleWordWrap);
            findForm.CtrlShiftNPressed -= new EventHandler(OpenEncoderDecoder);
            findForm.CtrlShiftPPressed -= new EventHandler(PrintTail);
            findForm.CtrlShiftTPressed -= new EventHandler(ToggleTopMost);
            findForm.CtrlUPressed -= new EventHandler(OpenInEditor);
            findForm.DownPressed -= new EventHandler(OnDownPressed);
            findForm.EndPressed -= new EventHandler(OnEndPressed);
            findForm.F11Pressed -= new EventHandler(TurnOffMonitors);
            findForm.F5Pressed -= new EventHandler(Reload);
            findForm.F7Pressed -= new EventHandler(ReloadTail);
            findForm.F8Pressed -= new EventHandler(Clear);
            findForm.F9Pressed -= new EventHandler(TogglePause);
            findForm.Find -= new EventHandler<SearchEventArgs>(OnFind);
            findForm.FormClosed -= new FormClosedEventHandler(OnFormClosed);
            findForm.HelpRequested -= new HelpEventHandler(OpenHelp);
            findForm.HomePressed -= new EventHandler(OnHomePressed);
            findForm.Load -= new EventHandler(OnFindFormLoad);
            findForm.PageDownPressed -= new EventHandler(OnPageDownPressed);
            findForm.PageUpPressed -= new EventHandler(OnPageUpPressed);
            findForm.UpPressed -= new EventHandler(OnUpPressed);
            if (findForm.Visible) {
                findForm.SafeClose();
            }
            findForm = null;
        }

        private void OnFormLoad(object sender, EventArgs e) {
            tabControl.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
            statusStripHandler.SetZoomLevel(0);
            if (!Menu.MenuItems[2].MenuItems[0].Checked) {
                selectedIndex = tabControl.SelectedIndex;
                Cursor = Cursors.WaitCursor;
                ReadLog();
                Cursor = Cursors.Default;
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            selectedIndex = tabControl.SelectedIndex;
            Menu.MenuItems[3].MenuItems[2].Checked = logViewerStates[selectedIndex].TextBox.WordWrap;
            if (!Menu.MenuItems[2].MenuItems[0].Checked && string.IsNullOrEmpty(logViewerStates[selectedIndex].TextBox.Text)) {
                Cursor = Cursors.WaitCursor;
                ReadLog();
                Cursor = Cursors.Default;
            }
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary);
            statusStripHandler.SetDataSize(logViewerStates[selectedIndex].Position);
        }

        private void OnWindowStateLoaded(object sender, PersistWindowStateEventArgs e) {
            try {
                if (e.RegistryKey != null) {
                    byteSettings = Settings.IntToByteArray(
                        (int)e.RegistryKey.GetValue(Name + Constants.Settings, Settings.ByteArrayToInt(byteSettings)));
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            tabControl.SelectedIndex = byteSettings[0] < 0 || byteSettings[0] > tabControl.TabCount - 1 ? 0 : byteSettings[0];
            Menu.MenuItems[3].MenuItems[4].Checked = byteSettings[1] > 0;
            Menu.MenuItems[3].MenuItems[5].Checked = byteSettings[2] > 0;
            Menu.MenuItems[3].MenuItems[6].Checked = byteSettings[3] > 0;
        }

        private void OnWindowStateSaved(object sender, PersistWindowStateEventArgs e) {
            byteSettings[0] = (byte)tabControl.SelectedIndex;
            byteSettings[1] = (byte)(Menu.MenuItems[3].MenuItems[4].Checked ? 1 : 0);
            byteSettings[2] = (byte)(Menu.MenuItems[3].MenuItems[5].Checked ? 1 : 0);
            byteSettings[3] = (byte)(Menu.MenuItems[3].MenuItems[6].Checked ? 1 : 0);
            try {
                if (e.RegistryKey != null) {
                    e.RegistryKey.SetValue(Name + Constants.Settings, Settings.ByteArrayToInt(byteSettings));
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void Export(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Export), sender, e);
            } else {
                try {
                    saveFileDialog.Filter = fileExtensionFilter.GetFilter();
                    saveFileDialog.FilterIndex = fileExtensionFilter.GetFilterIndex();
                    saveFileDialog.Title = Properties.Resources.CaptionExport;
                    saveFileDialog.FileName = Application.ProductName + Properties.Resources.CaptionExport;
                    if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessageExporting);
                        StaticMethods.ExportAsImage(tabControl, saveFileDialog.FileName);
                        fileExtensionFilter.SetFilterIndex(saveFileDialog.FilterIndex);
                        settings.ExtensionFilterIndex = saveFileDialog.FilterIndex;
                        settings.LastExportDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessageExportFinished);
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessageExportFailed);
                }
            }
        }

        private void PrintPreview(object sender, EventArgs e) {
            printType = PrintType.All;
            try {
                dialog = printPreviewDialog;
                dialog.WindowState = WindowState.Equals(FormWindowState.Minimized) ? FormWindowState.Normal : WindowState;
                if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                    printDocument.Print();
                }
            } catch (Exception exception) {
                ShowException(exception, Properties.Resources.MessagePrintingFailed);
            }
        }

        private void Print(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Print), sender, e);
            } else {
                printType = PrintType.All;
                try {
                    printDialog.AllowCurrentPage = true;
                    printDialog.AllowPrintToFile = true;
                    printDialog.AllowSelection = true;
                    printDialog.AllowSomePages = true;
                    printDialog.ShowHelp = true;
                    printDialog.UseEXDialog = true;
                    if (printDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        printDocument.Print();
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintTailPreview(object sender, EventArgs e) {
            printType = PrintType.Tail;
            try {
                dialog = printPreviewDialog;
                dialog.WindowState = WindowState.Equals(FormWindowState.Minimized) ? FormWindowState.Normal : WindowState;
                if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                    printDocument.Print();
                }
            } catch (Exception exception) {
                ShowException(exception, Properties.Resources.MessagePrintingFailed);
            }
        }

        private void PrintTail(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintTail), sender, e);
            } else {
                printType = PrintType.Tail;
                try {
                    printDialog.AllowCurrentPage = false;
                    printDialog.AllowPrintToFile = true;
                    printDialog.AllowSelection = false;
                    printDialog.AllowSomePages = false;
                    printDialog.ShowHelp = true;
                    printDialog.UseEXDialog = true;
                    if (printDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        printDocument.Print();
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintImagePreview(object sender, EventArgs e) {
            printType = PrintType.Image;
            try {
                dialog = printPreviewDialog;
                dialog.WindowState = WindowState.Equals(FormWindowState.Minimized) ? FormWindowState.Normal : WindowState;
                if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                    printDocument.Print();
                }
            } catch (Exception exception) {
                ShowException(exception, Properties.Resources.MessagePrintingFailed);
            }
        }

        private void PrintImage(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintImage), sender, e);
            } else {
                printType = PrintType.Image;
                try {
                    printDialog.AllowCurrentPage = false;
                    printDialog.AllowPrintToFile = true;
                    printDialog.AllowSelection = false;
                    printDialog.AllowSomePages = false;
                    printDialog.ShowHelp = true;
                    printDialog.UseEXDialog = true;
                    if (printDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        printDocument.Print();
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void ShowException(Exception exception) => ShowException(exception, null);

        private void ShowException(Exception exception, string statusMessage) {
            Debug.WriteLine(exception);
            ErrorLog.WriteLine(exception);
            statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.PersistentB,
                string.IsNullOrEmpty(statusMessage) ? exception.Message : statusMessage);
            StringBuilder title = new StringBuilder()
                .Append(Program.GetTitle())
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(Properties.Resources.CaptionError);
            dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
            dialog.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog.ShowDialog(this);
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB);
            statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.Temporary,
                string.IsNullOrEmpty(statusMessage) ? exception.Message : statusMessage);
        }

        private void BeginPrint(object sender, PrintEventArgs e) {
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                logFilePath = logViewerStates[selectedIndex].Path;
                printFont = logViewerStates[selectedIndex].TextBox.Font;
                availableWidth = 0;
                availableHeight = 0;
                availableSize = Size.Empty;
                doEvents = 0;
                stringToPrint = null;
                printAction = e.PrintAction;
                printDocument.OriginAtMargins = settings.PrintSoftMargins;
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.Temporary,
                    e.PrintAction.Equals(PrintAction.PrintToPreview)
                        ? Properties.Resources.MessageGeneratingPreview
                        : Properties.Resources.MessagePrinting);
                Cursor = Cursors.WaitCursor;
                for (int i = 0; i < 8; i++) {
                    Menu.MenuItems[0].MenuItems[i].Enabled = false;
                }
                for (int i = 0; i < Menu.MenuItems[2].MenuItems.Count; i++) {
                    Menu.MenuItems[2].MenuItems[i].Enabled = false;
                }
                Menu.MenuItems[5].MenuItems[1].Enabled = false;
                Menu.MenuItems[5].MenuItems[3].Enabled = false;
                Menu.MenuItems[3].MenuItems[0].Enabled = false;
                topMost = TopMost;
                if (TopMost) {
                    TopMost = false;
                    Menu.MenuItems[3].MenuItems[0].Checked = false;
                }
                e.Cancel = cancel;
            } catch (Exception exception) {
                ShowException(exception, Properties.Resources.MessagePrintingFailed);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e) {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            if (availableWidth.Equals(0)) {
                availableWidth = (int)Math.Floor(printDocument.OriginAtMargins
                    ? e.MarginBounds.Width
                    : e.PageSettings.Landscape ? e.PageSettings.PrintableArea.Height : e.PageSettings.PrintableArea.Width);
            }
            if (availableHeight.Equals(0)) {
                availableHeight = (int)Math.Floor(printDocument.OriginAtMargins
                    ? e.MarginBounds.Height
                    : e.PageSettings.Landscape ? e.PageSettings.PrintableArea.Width : e.PageSettings.PrintableArea.Height);
            }
            if (availableSize.Equals(Size.Empty)) {
                availableSize = new Size(availableWidth, availableHeight);
            }
            if (printAction.Equals(PrintAction.PrintToPreview)) {
                e.Graphics.TranslateTransform(e.PageSettings.PrintableArea.X, e.PageSettings.PrintableArea.Y);
            }
            int strLengthToMeasure = availableWidth * availableHeight / 50;
            string stringToMeasure = string.Empty;
            e.Cancel = cancel;
            switch (printType) {
                case PrintType.Image:
                    using (Bitmap bitmap = new Bitmap(tabControl.Width, tabControl.Height, PixelFormat.Format32bppArgb)) {
                        tabControl.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                        bool rotate = StaticMethods.IsGraphicsRotationNeeded(bitmap.Size, availableSize);
                        if (rotate) {
                            e.Graphics.RotateTransform(90, MatrixOrder.Prepend);
                        }
                        Size size = StaticMethods.GetNewGraphicsSize(bitmap.Size, availableSize);
                        e.Graphics.DrawImage(bitmap, 0, rotate ? -availableWidth : 0, size.Width, size.Height);
                        e.HasMorePages = false;
                    }
                    break;
                case PrintType.Tail:
                    List<string> lines = new List<string>();
                    if (File.Exists(logFilePath)) {
                        using (FileStream fileStream = new FileStream(
                                logFilePath,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.ReadWrite)) {

                            long l = fileStream.Length - strLengthToMeasure * 2;
                            fileStream.Seek(l > 0 ? l : 0, SeekOrigin.Begin);
                            using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                                using (StringReader stringReader = new StringReader(
                                        streamReader.ReadToEnd().Replace(Constants.VerticalTab.ToString(),
                                        string.Empty.PadRight(Constants.VerticalTabNumberOfSpaces)).TrimEnd())) {

                                    for (string line; (line = stringReader.ReadLine()) != null;) {
                                        lines.Add(line);
                                    }
                                }
                            }
                        }
                    }
                    int linesPerPageInitial = 0;
                    while (linesPerPage > linesPerPageInitial - 5 && lines.Count > 2) {
                        stringToMeasure = string.Join(Environment.NewLine, lines);

                        e.Graphics.MeasureString(
                            stringToMeasure.Length > strLengthToMeasure
                                ? stringToMeasure.Substring(0, strLengthToMeasure)
                                : stringToMeasure,
                            printFont,
                            availableSize,
                            StringFormat.GenericTypographic,
                            out charactersOnPage,
                            out linesPerPage);

                        if (linesPerPageInitial.Equals(0)) {
                            linesPerPageInitial = linesPerPage;
                        }
                        lines.RemoveAt(0);
                        lines.RemoveAt(0);
                    }
                    e.Graphics.DrawString(
                        string.Join(Environment.NewLine, lines),
                        printFont,
                        Brushes.Black,
                        new Rectangle(Point.Empty, availableSize),
                        StringFormat.GenericTypographic);

                    e.HasMorePages = false;
                    break;
                default:
                    if (stringToPrint == null) {
                        stringToPrint = string.Empty;
                        switch (printDocument.PrinterSettings.PrintRange) {
                            case PrintRange.AllPages:
                                if (File.Exists(logFilePath)) {
                                    using (FileStream fileStream = new FileStream(
                                            logFilePath,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.ReadWrite)) {

                                        using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                                            stringToPrint = streamReader
                                                .ReadToEnd()
                                                .Replace(Constants.VerticalTab.ToString(),
                                                    string.Empty.PadRight(Constants.VerticalTabNumberOfSpaces))
                                                .TrimEnd();
                                        }
                                    }
                                }
                                break;
                            case PrintRange.SomePages:
                                if (File.Exists(logFilePath)) {
                                    using (FileStream fileStream = new FileStream(
                                            logFilePath,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.ReadWrite)) {

                                        using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                                            stringToMeasure = streamReader
                                                .ReadToEnd()
                                                .Replace(Constants.VerticalTab.ToString(),
                                                    string.Empty.PadRight(Constants.VerticalTabNumberOfSpaces))
                                                .TrimEnd();
                                        }
                                    }
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                for (int n = 1; stringToMeasure.Length > 0 && n <= printDocument.PrinterSettings.ToPage; n++) {

                                    e.Graphics.MeasureString(
                                        stringToMeasure.Length > strLengthToMeasure
                                            ? stringToMeasure.Substring(0, strLengthToMeasure)
                                            : stringToMeasure,
                                        printFont,
                                        availableSize,
                                        StringFormat.GenericTypographic,
                                        out charactersOnPage,
                                        out linesPerPage);

                                    if (n >= printDocument.PrinterSettings.FromPage) {
                                        stringBuilder.Append(stringToMeasure.Substring(0, charactersOnPage));
                                    }
                                    stringToMeasure = stringToMeasure.Substring(charactersOnPage);
                                    if (n % 100 == 0) {
                                        e.Cancel = cancel;
                                        Application.DoEvents();
                                    }
                                }
                                stringToPrint = stringBuilder.ToString();
                                break;
                            case PrintRange.Selection:
                                stringToPrint = logViewerStates[selectedIndex].TextBox.SelectedText
                                    .Replace(Constants.VerticalTab.ToString(),
                                        string.Empty.PadRight(Constants.VerticalTabNumberOfSpaces))
                                    .TrimEnd();
                                break;
                            default:
                                if (File.Exists(logFilePath)) {
                                    using (FileStream fileStream = new FileStream(
                                            logFilePath,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.ReadWrite)) {

                                        using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                                            stringToMeasure = streamReader
                                                .ReadToEnd()
                                                .Replace(Constants.VerticalTab.ToString(),
                                                    string.Empty.PadRight(Constants.VerticalTabNumberOfSpaces))
                                                .TrimEnd();
                                        }
                                    }
                                }
                                for (int n = 1; stringToMeasure.Length > 0; n++) {

                                    e.Graphics.MeasureString(
                                        stringToMeasure.Length > strLengthToMeasure
                                            ? stringToMeasure.Substring(0, strLengthToMeasure)
                                            : stringToMeasure,
                                        printFont,
                                        availableSize,
                                        StringFormat.GenericTypographic,
                                        out charactersOnPage,
                                        out linesPerPage);

                                    stringToPrint = stringToMeasure;
                                    stringToMeasure = stringToMeasure.Substring(charactersOnPage);
                                    if (n % 100 == 0) {
                                        e.Cancel = cancel;
                                        Application.DoEvents();
                                    }
                                }
                                break;
                        }
                    }

                    e.Graphics.MeasureString(
                        stringToPrint.Length > strLengthToMeasure ? stringToPrint.Substring(0, strLengthToMeasure) : stringToPrint,
                        printFont, availableSize,
                        StringFormat.GenericTypographic,
                        out charactersOnPage,
                        out linesPerPage);

                    e.Graphics.DrawString(
                        stringToPrint.Substring(0, charactersOnPage),
                        printFont,
                        Brushes.Black,
                        new Rectangle(Point.Empty, availableSize),
                        StringFormat.GenericTypographic);

                    stringToPrint = stringToPrint.Substring(charactersOnPage);
                    e.HasMorePages = stringToPrint.Length > 0;
                    if (++doEvents % 50 == 0) {
                        Application.DoEvents();
                    }
                    break;
            }
            e.Cancel = cancel;
        }

        private void EndPrint(object sender, PrintEventArgs e) {
            statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.Temporary,
                e.PrintAction.Equals(PrintAction.PrintToPreview)
                    ? Properties.Resources.MessagePreviewGenerated
                    : Properties.Resources.MessagePrintingFinished);
            try {
                fileSystemWatcher.EnableRaisingEvents = true;
                Menu.MenuItems[5].MenuItems[1].Enabled = true;
                Menu.MenuItems[5].MenuItems[3].Enabled = true;
                for (int i = 0; i < Menu.MenuItems[2].MenuItems.Count; i++) {
                    Menu.MenuItems[2].MenuItems[i].Enabled = true;
                }
                for (int i = 0; i < 8; i++) {
                    Menu.MenuItems[0].MenuItems[i].Enabled = true;
                }
                if (topMost) {
                    TopMost = true;
                    Menu.MenuItems[3].MenuItems[0].Checked = true;
                }
                Menu.MenuItems[3].MenuItems[0].Enabled = true;
                Cursor = Cursors.Default;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnUpdateCheckerStateChanged(object sender, UpdateCheckEventArgs e) {
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, e.Message);
            if (dialog == null || !dialog.Visible) {
                dialog = e.Dialog;
            }
        }

        private void Copy(object sender, EventArgs e) {
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                CopyToClipboard();
                logViewerStates[selectedIndex].TextBox.Focus();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                try {
                    fileSystemWatcher.EnableRaisingEvents = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void SelectAll(object sender, EventArgs e) {
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                logViewerStates[selectedIndex].TextBox.SelectAll();
                logViewerStates[selectedIndex].TextBox.Focus();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                try {
                    fileSystemWatcher.EnableRaisingEvents = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary);
            }
        }

        private void TruncateLog(object sender, EventArgs e) {
            dialog = new MessageForm(this, Properties.Resources.MessageTruncateFile, null, MessageForm.Buttons.YesNo,
                MessageForm.BoxIcon.Question, MessageForm.DefaultButton.Button2);
            dialog.HelpRequested += new HelpEventHandler(OpenHelp);
            if (dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    if (!string.IsNullOrEmpty(logViewerStates[selectedIndex].Path)
                            && File.Exists(logViewerStates[selectedIndex].Path)) {

                        for (int i = 0; i < 100; i++) {
                            try {
                                using (FileStream fileStream = new FileStream(
                                    logViewerStates[selectedIndex].Path,
                                    FileMode.Truncate,
                                    FileAccess.Write,
                                    FileShare.ReadWrite)) { }
                            } catch (Exception exception) {
                                Debug.WriteLine(exception);
                                ErrorLog.WriteLine(exception);
                            }
                            try {
                                if (new FileInfo(logViewerStates[selectedIndex].Path).Length.Equals(0)) {
                                    break;
                                }
                            } catch (Exception exception) {
                                Debug.WriteLine(exception);
                                ErrorLog.WriteLine(exception);
                                break;
                            }
                        }
                        if (new FileInfo(logViewerStates[selectedIndex].Path).Length > 0) {
                            throw new ApplicationException(Properties.Resources.MessageTruncatingError);
                        }
                        logViewerStates[selectedIndex].Action = LogViewerAction.Reload;
                        Cursor = Cursors.WaitCursor;
                        ReadLog();
                        Cursor = Cursors.Default;
                    }
                } catch (Exception exception) {
                    ShowException(exception);
                } finally {
                    try {
                        fileSystemWatcher.EnableRaisingEvents = true;
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
        }

        private void AnalyzeUrl(object sender, EventArgs e) {
            if (StaticMethods.CheckSelectedUrl(logViewerStates[selectedIndex].TextBox)) {
                try {
                    StringBuilder arguments = new StringBuilder()
                        .Append(Constants.CommandLineSwitchWD)
                        .Append(Constants.Space)
                        .Append(StaticMethods.EscapeArgument(logViewerStates[selectedIndex].TextBox.SelectedText.TrimStart()));
                    Process.Start(Application.ExecutablePath, arguments.ToString());
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void Find(object sender, EventArgs e) {
            if (findThread != null && findThread.IsAlive) {
                try {
                    if (TopMost) {
                        findForm.SafeTopMost();
                    }
                    findForm.SafeSelect();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            } else {
                if (searching) {
                    logViewerStates[selectedIndex].TextBox.HideSelection = false;
                } else {
                    search.searchString = null;
                }
                findThread = new Thread(new ThreadStart(Find));
                findThread.Start();
            }
        }

        private void Find() {
            findForm = new LogFindForm(search);
            findForm.AltCtrlShiftPPressed += new EventHandler(PrintImage);
            findForm.AltF11Pressed += new EventHandler(LaunchCalculator);
            findForm.AltF12Pressed += new EventHandler(LaunchNotepad);
            findForm.AltF7Pressed += new EventHandler(ReloadFromStart);
            findForm.CtrlEPressed += new EventHandler(Export);
            findForm.CtrlJPressed += new EventHandler(CopyLogFileName);
            findForm.CtrlKPressed += new EventHandler(CopyDirectoryPath);
            findForm.CtrlLPressed += new EventHandler(OpenLogLocation);
            findForm.CtrlMPressed += new EventHandler(CopyLogFilePath);
            findForm.CtrlPPressed += new EventHandler(Print);
            findForm.CtrlShiftLPressed += new EventHandler(ToggleWordWrap);
            findForm.CtrlShiftNPressed += new EventHandler(OpenEncoderDecoder);
            findForm.CtrlShiftPPressed += new EventHandler(PrintTail);
            findForm.CtrlShiftTPressed += new EventHandler(ToggleTopMost);
            findForm.CtrlUPressed += new EventHandler(OpenInEditor);
            findForm.DownPressed += new EventHandler(OnDownPressed);
            findForm.EndPressed += new EventHandler(OnEndPressed);
            findForm.F11Pressed += new EventHandler(TurnOffMonitors);
            findForm.F5Pressed += new EventHandler(Reload);
            findForm.F7Pressed += new EventHandler(ReloadTail);
            findForm.F8Pressed += new EventHandler(Clear);
            findForm.F9Pressed += new EventHandler(TogglePause);
            findForm.Find += new EventHandler<SearchEventArgs>(OnFind);
            findForm.FormClosed += new FormClosedEventHandler(OnFormClosed);
            findForm.HelpRequested += new HelpEventHandler(OpenHelp);
            findForm.HomePressed += new EventHandler(OnHomePressed);
            findForm.Load += new EventHandler(OnFindFormLoad);
            findForm.PageDownPressed += new EventHandler(OnPageDownPressed);
            findForm.PageUpPressed += new EventHandler(OnPageUpPressed);
            findForm.UpPressed += new EventHandler(OnUpPressed);
            findForm.ShowDialog();
        }

        private void OnFindFormLoad(object sender, EventArgs e) {
            if (TopMost) {
                ((FindForm)sender).SafeTopMost();
            }
        }

        private void OnHomePressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysHome);

        private void OnEndPressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysEnd);

        private void OnPageUpPressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysPgUp);

        private void OnPageDownPressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysPgDn);

        private void OnUpPressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysUp);

        private void OnDownPressed(object sender, EventArgs e) => SendKeys.Send(Constants.SendKeysDown);

        private void OnFind(object sender, SearchEventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler<SearchEventArgs>(OnFind), sender, e);
            } else {
                if (!string.IsNullOrEmpty(e.Search.searchString)) {
                    logViewerStates[selectedIndex].TextBox.HideSelection = false;
                    search = e.Search;
                    Find(search);
                }
                persistWindowState.SetVisible(e.Handle);
            }
        }

        private void FindNext(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(search.searchString)) {
                searching = true;
                Find(search);
            }
        }

        private void FindPrevious(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(search.searchString)) {
                searching = true;
                Find(search);
            }
        }

        private void Find(Search search) {
            searchResults = new List<SearchResult>();
            TextBox textBox = logViewerStates[selectedIndex].TextBox;
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                string haystack = textBox.Text;
                if (search.regularExpression) {
                    Regex regex = new Regex(search.searchString,
                        (search.caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase)
                            | RegexOptions.CultureInvariant
                            | RegexOptions.Multiline);
                    foreach (Match match in regex.Matches(haystack)) {
                        searchResults.Add(new SearchResult(match.Index, match.Length));
                    }
                } else if (search.startsWith || search.endsWith) {
                    foreach (SearchLine searchLine in StaticMethods.SplitToLines(haystack)) {
                        if (search.startsWith && searchLine.line.StartsWith(search.searchString,
                                search.caseSensitive
                                    ? StringComparison.Ordinal
                                    : StringComparison.OrdinalIgnoreCase)) {

                            searchResults.Add(new SearchResult(searchLine.index, search.searchString.Length));
                        }
                        if (search.endsWith && searchLine.line.EndsWith(search.searchString, search.caseSensitive
                                ? StringComparison.Ordinal
                                : StringComparison.OrdinalIgnoreCase)) {

                            searchResults.Add(new SearchResult(
                                searchLine.index + searchLine.line.Length - search.searchString.Length,
                                search.searchString.Length));
                        }
                    }
                } else {
                    for (int i = 0;
                            (i = haystack.IndexOf(search.searchString, i, search.caseSensitive
                                ? StringComparison.Ordinal
                                : StringComparison.OrdinalIgnoreCase)) > -1;
                            i += search.searchString.Length) {

                        searchResults.Add(new SearchResult(i, search.searchString.Length));
                    }
                }
                if (searchResults.Count > 0) {
                    if (search.backward && !invertDirection || !search.backward && invertDirection) {
                        if (--matchIndex < 0) {
                            matchIndex = searchResults.Count - 1;
                        }
                        while (!lastMatch && textBox.SelectionStart < searchResults[matchIndex].index) {
                            if (--matchIndex < 0) {
                                matchIndex = searchResults.Count - 1;
                                break;
                            }
                        }
                    } else {
                        if (++matchIndex >= searchResults.Count) {
                            matchIndex = 0;
                        }
                        while (!lastMatch && textBox.SelectionStart > searchResults[matchIndex].index) {
                            if (++matchIndex >= searchResults.Count) {
                                matchIndex = 0;
                                break;
                            }
                        }
                    }
                    textBox.Select(searchResults[matchIndex].index, searchResults[matchIndex].length);
                    textBox.ScrollToCaret();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                try {
                    fileSystemWatcher.EnableRaisingEvents = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
                lastMatch = false;
                statusStripHandler.SetSearchResult(searchResults.Count, matchIndex + 1);
                if (matchIndex.Equals(0) || matchIndex.Equals(searchResults.Count - 1)) {
                    lastMatch = true;
                }
            }
        }

        private void TogglePause(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(TogglePause), sender, e);
            } else {
                MenuItem menuItem = (MenuItem)sender;
                menuItem.Checked = !menuItem.Checked;
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.PersistentB,
                    menuItem.Checked ? Properties.Resources.MessagePaused : string.Empty);
                if (!menuItem.Checked) {
                    ReadLog();
                }
            }
        }

        private void Reload(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Reload), sender, e);
            } else {
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Reload;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    Cursor = Cursors.WaitCursor;
                    ReadLog();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ReloadFromStart(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ReloadFromStart), sender, e);
            } else {
                if (IsRestricted()) {
                    return;
                }
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Start;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    Cursor = Cursors.WaitCursor;
                    ReadLog();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ReloadTail(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ReloadTail), sender, e);
            } else {
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    logViewerStates[selectedIndex].Action = LogViewerAction.PreloadLimit;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    Cursor = Cursors.WaitCursor;
                    ReadLog();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void Clear(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Clear), sender, e);
            } else {
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    logViewerStates[selectedIndex].Action = LogViewerAction.Cleared;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    Cursor = Cursors.WaitCursor;
                    ReadLog();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ToggleTopMost(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ToggleTopMost), sender, e);
            } else {
                Menu.MenuItems[3].MenuItems[0].Checked = !Menu.MenuItems[3].MenuItems[0].Checked;
                TopMost = Menu.MenuItems[3].MenuItems[0].Checked;
                if (TopMost) {
                    if (findForm != null && findForm.Visible) {
                        findForm.SafeTopMost();
                        findForm.SafeBringToFront();
                    }
                } else {
                    SendToBack();
                }
            }
        }

        private void ToggleWordWrap(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ToggleWordWrap), sender, e);
            } else {
                if (IsRestricted()) {
                    return;
                }
                Cursor = Cursors.WaitCursor;
                try {
                    logViewerStates[selectedIndex].TextBox.WordWrap = !logViewerStates[selectedIndex].TextBox.WordWrap;
                    Menu.MenuItems[3].MenuItems[2].Checked = logViewerStates[selectedIndex].TextBox.WordWrap;
                } catch (OutOfMemoryException exception) {
                    ShowException(exception, Properties.Resources.MessageOutOfMemoryError);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void OpenEncoderDecoder(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenEncoderDecoder), sender, e);
            } else {
                try {
                    Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWD);
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void OpenInEditor(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenInEditor), sender, e);
            } else {
                try {
                    if (!string.IsNullOrWhiteSpace(settings.ExternalEditor) && File.Exists(settings.ExternalEditor)) {
                        Process.Start(settings.ExternalEditor, StaticMethods.EscapeArgument(logViewerStates[selectedIndex].Path));
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void OpenInDefaultEditor(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(logViewerStates[selectedIndex].Path)) {
                try {
                    Process.Start(logViewerStates[selectedIndex].Path);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void OpenLogLocation(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenLogLocation), sender, e);
            } else {
                if (!string.IsNullOrEmpty(Application.LocalUserAppDataPath)) {
                    try {
                        string filePath = Path.Combine(Path.GetDirectoryName(Environment.SystemDirectory), Constants.ExplorerFileName);
                        StringBuilder arguments = new StringBuilder()
                            .Append(Constants.ExplorerSwitchE)
                            .Append(Constants.Comma)
                            .Append(Constants.Space)
                            .Append(StaticMethods.EscapeArgument(Application.LocalUserAppDataPath));
                        Process.Start(filePath, arguments.ToString());
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                    }
                }
            }
        }

        private void CopyLogFileName(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(CopyLogFileName), sender, e);
            } else {
                string fileName = tabControl.SelectedTab.Text;
                if (!string.IsNullOrEmpty(fileName)) {
                    try {
                        Clipboard.SetText(fileName);
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                    }
                }
            }
        }

        private void CopyLogFilePath(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(CopyLogFilePath), sender, e);
            } else {
                if (!string.IsNullOrEmpty(logViewerStates[selectedIndex].Path)) {
                    try {
                        Clipboard.SetText(logViewerStates[selectedIndex].Path);
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                    }
                }
            }
        }

        private void CopyDirectoryPath(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(CopyDirectoryPath), sender, e);
            } else {
                if (!string.IsNullOrEmpty(Application.LocalUserAppDataPath)) {
                    try {
                        Clipboard.SetText(Application.LocalUserAppDataPath);
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                    }
                }
            }
        }

        private void TurnOffMonitors(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(TurnOffMonitors), sender, e);
            } else {
                if (turnOffThread != null && turnOffThread.IsAlive) {
                    try {
                        countDownForm.SafeSelect();
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                } else {
                    turnOffThread = new Thread(new ThreadStart(TurnOffMonitors));
                    turnOffThread.Start();
                }
            }
        }

        private void TurnOffMonitors() {
            countDownForm = new CountDownForm(settings);
            countDownForm.HelpButtonClicked += new CancelEventHandler((sender, e) => {
                countDownForm.Close();
                OpenHelp(sender, e);
            });
            countDownForm.HelpRequested += new HelpEventHandler((sender, hlpevent) => {
                countDownForm.Close();
                OpenHelp(sender, hlpevent);
            });
            countDownForm.ShowDialog();
        }

        private void LaunchCalculator(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(LaunchCalculator), sender, e);
            } else {
                try {
                    Process.Start(Path.Combine(Environment.SystemDirectory, Constants.CalcFileName));
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void LaunchNotepad(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(LaunchNotepad), sender, e);
            } else {
                try {
                    Process.Start(Path.Combine(Environment.SystemDirectory, Constants.NotepadFileName));
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void LaunchWordPad(object sender, EventArgs e) {
            try {
                Process.Start(Path.Combine(Environment.SystemDirectory, Constants.WordPadFileName));
            } catch (Exception exception) {
                ShowException(exception);
            }
        }

        private void LaunchCharMap(object sender, EventArgs e) {
            try {
                Process.Start(Path.Combine(Environment.SystemDirectory, Constants.CharMapFileName));
            } catch (Exception exception) {
                ShowException(exception);
            }
        }

        private void CheckUpdates(object sender, EventArgs e) => updateChecker.Check(UpdateChecker.CheckType.User);

        private void ShowAbout(object sender, EventArgs e) {
            dialog = new AboutForm();
            dialog.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog.ShowDialog(this);
        }

        private void OpenHelp(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenHelp), sender, e);
            } else {
                try {
                    StringBuilder url = new StringBuilder()
                        .Append(Properties.Resources.Website.TrimEnd(Constants.Slash).ToLowerInvariant())
                        .Append(Constants.Slash)
                        .Append(Application.ProductName.ToLowerInvariant())
                        .Append(Constants.Slash);
                    Process.Start(url.ToString());
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void OpenHelp(object sender, HelpEventArgs e) => OpenHelp(sender, (EventArgs)e);

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Alt && e.Shift && e.Control && e.KeyCode.Equals(Keys.P)) {
                if (Menu.MenuItems[0].MenuItems[7].Enabled) {
                    e.SuppressKeyPress = true;
                    PrintImage(sender, e);
                }
            } else if (e.Shift && e.Control && e.KeyCode.Equals(Keys.L)) {
                e.SuppressKeyPress = true;
                ToggleWordWrap(sender, e);
            } else if (e.Shift && e.Control && e.KeyCode.Equals(Keys.N)) {
                e.SuppressKeyPress = true;
                OpenEncoderDecoder(sender, e);
            } else if (e.Shift && e.Control && e.KeyCode.Equals(Keys.P)) {
                if (Menu.MenuItems[0].MenuItems[5].Enabled) {
                    e.SuppressKeyPress = true;
                    PrintTail(sender, e);
                }
            } else if (e.Shift && e.Control && e.KeyCode.Equals(Keys.T)) {
                if (Menu.MenuItems[3].MenuItems[0].Enabled) {
                    e.SuppressKeyPress = true;
                    ToggleTopMost(sender, e);
                }
            } else if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                SelectAll(sender, e);
            } else if (e.Control && e.KeyCode.Equals(Keys.C)) {
                e.SuppressKeyPress = true;
                Copy(sender, e);
            } else if (e.KeyCode.Equals(Keys.ShiftKey)) {
                invertDirection = true;
            } else if (e.Shift && e.KeyCode.Equals(Keys.F3)) {
                e.SuppressKeyPress = true;
                invertDirection = true;
                FindPrevious(sender, e);
            } else if (e.KeyCode.Equals(Keys.F3)) {
                e.SuppressKeyPress = true;
                invertDirection = false;
                FindNext(sender, e);
            } else if (e.KeyCode.Equals(Keys.Escape)) {
                e.SuppressKeyPress = true;
                if (findForm != null) {
                    CloseFindForm();
                } else if (searching) {
                    statusStripHandler.ClearSearchResult();
                    searching = false;
                } else {
                    Menu.MenuItems[2].MenuItems[0].Checked = true;
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, Properties.Resources.MessagePaused);
                }
            } else {
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.ShiftKey)) {
                invertDirection = false;
            }
        }

        /// <summary>
        /// Modified version of OnMouseDown common method. Do not use anywhere else.
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs e) {
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary);
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
            try {
                fileSystemWatcher.EnableRaisingEvents = false;
                if (textBox.SelectionLength > 0) {
                    textBoxClicks = 2;
                } else if (textBoxClicks.Equals(0) || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                    textBoxClicks++;
                } else {
                    textBoxClicks = 0;
                }
                location = e.Location;
                if (textBoxClicks.Equals(3)) {
                    textBoxClicks = 0;
                    NativeMethods.MouseEvent(
                        Constants.MOUSEEVENTF_LEFTUP,
                        Convert.ToUInt32(Cursor.Position.X),
                        Convert.ToUInt32(Cursor.Position.Y),
                        0,
                        0);
                    Application.DoEvents();
                    if (textBox.Multiline) {
                        char[] chars = textBox.Text.ToCharArray();
                        int selectionEnd = Math.Min(
                            Array.IndexOf(chars, Constants.CarriageReturn, textBox.SelectionStart),
                            Array.IndexOf(chars, Constants.LineFeed, textBox.SelectionStart));
                        if (selectionEnd < 0) {
                            selectionEnd = textBox.TextLength;
                        }
                        selectionEnd = Math.Max(textBox.SelectionStart + textBox.SelectionLength, selectionEnd);
                        int selectionStart = Math.Min(textBox.SelectionStart, selectionEnd);
                        while (--selectionStart > 0
                            && !chars[selectionStart].Equals(Constants.LineFeed)
                            && !chars[selectionStart].Equals(Constants.CarriageReturn)) { }
                        textBox.Select(selectionStart, selectionEnd - selectionStart);
                    } else {
                        textBox.SelectAll();
                    }
                    if (Menu.MenuItems[3].MenuItems[6].Checked) {
                        CopyToClipboard();
                    }
                    textBox.Focus();
                } else {
                    if (textBoxClicks.Equals(2) && Menu.MenuItems[3].MenuItems[6].Checked) {
                        CopyToClipboard();
                    }
                    textBoxClicksTimer.Start();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                try {
                    fileSystemWatcher.EnableRaisingEvents = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e) {
            if (e.Button.Equals(MouseButtons.Left) && Menu.MenuItems[3].MenuItems[4].Checked) {
                try {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    CopyToClipboard();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                } finally {
                    try {
                        fileSystemWatcher.EnableRaisingEvents = true;
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e) {
            if (((Form)sender).Equals(ActiveForm)) {
                Control control = StaticMethods.FindControlAtCursor((Form)sender);
                if (control is TabControl) {
                    TabControl tabControl = (TabControl)control;
                    if (StaticMethods.IsHoverTabRectangle(tabControl) && !e.Delta.Equals(0)) {
                        StaticMethods.TabControlScroll(tabControl, e.Delta);
                    }
                }
            }
        }

        private void CopyToClipboard() {
            try {
                TextBox textBox = logViewerStates[selectedIndex].TextBox;
                string selected = textBox.SelectedText;
                if (!string.IsNullOrEmpty(selected)) {
                    if (selected.Equals(textBox.Text)) {
                        if (File.Exists(logViewerStates[selectedIndex].Path)) {
                            if (IsRestricted()) {
                                textBox.Copy();
                                statusStripHandler.SetMessage(
                                    StatusStripHandler.StatusMessageType.Temporary,
                                    Properties.Resources.MessageCopiedToClipboard);
                            } else {
                                Cursor = Cursors.WaitCursor;
                                using (FileStream fileStream = new FileStream(
                                        logViewerStates[selectedIndex].Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {

                                    using (StreamReader streamReader = new StreamReader(fileStream, true)) {
                                        Clipboard.SetText(streamReader.ReadToEnd());
                                        statusStripHandler.SetMessage(
                                            StatusStripHandler.StatusMessageType.Temporary,
                                            Properties.Resources.MessageCopiedToClipboard);
                                    }
                                }
                            }
                        }
                    } else {
                        textBox.Copy();
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessageCopiedToClipboard);
                    }
                }
            } catch (OutOfMemoryException exception) {
                ShowException(exception, Properties.Resources.MessageOutOfMemoryError);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                if (Cursor != Cursors.Default) {
                    Cursor = Cursors.Default;
                }
            }
        }

        private bool IsRestricted() {
            return settings.RestrictForLargeLogs
                && logViewerStates[selectedIndex].Position > settings.LargeLogsLimit * (settings.UseDecimalPrefix ? 1000000 : 1048576);
        }

        private enum PrintType {
            All,
            Tail,
            Image
        }
    }
}
