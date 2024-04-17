/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.7
 */

using CefSharp;
using CefSharp.WinForms;
using FortSoft.Tools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class CalculatorForm : Form {
        private Bitmap bitmap;
        private bool canReload;
        private BrowserCacheManager browserCacheManager;
        private CalculatorExtractor calculatorExtractor;
        private ChromiumWebBrowser browser;
        private CountDownForm countDownForm;
        private FileExtensionFilter fileExtensionFilter;
        private Form dialog;
        private PrintAction printAction;
        private PrintDialog printDialog;
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        private SaveFileDialog saveFileDialog;
        private Settings settings;
        private ShortcutManager shortcutManager;
        private StatusStripHandler statusStripHandler;
        private string browserTitle;
        private Thread turnOffThread;
        private UpdateChecker updateChecker;

        public CalculatorForm(Settings settings) {
            browserCacheManager = new BrowserCacheManager();
            calculatorExtractor = new CalculatorExtractor();

            this.settings = settings;

            Icon = Properties.Resources.Table;
            Text = Properties.Resources.CaptionBetCalculator;

            fileExtensionFilter = new FileExtensionFilter(settings.ExtensionFilterIndex);

            InitializePrintAsync();
            InitializeUpdateCheckerAsync();
            InitializeSaveFileDialogAsync();

            BuildMainMenuAsync();
            InitializeShortcutManager();

            InitializeComponent();

            SuspendLayout();
            InitializeCef(settings);
            InitializeBrowser();
            InitializeStatusStripHandler();
            ResumeLayout(false);
            PerformLayout();
            SubscribeEvents();
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
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExport + Constants.ShortcutCtrlE,
                    new EventHandler(ExportAsync)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrint + Constants.ShortcutCtrlP,
                    new EventHandler(Print)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintToPdf + Constants.ShortcutShiftCtrlP,
                    new EventHandler(PrintToPdf)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImagePreview,
                    new EventHandler(PrintImagePreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImage + Constants.ShortcutAltShiftCtrlP,
                    new EventHandler(PrintImage)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExit,
                    new EventHandler((sender, e) => Close()), Shortcut.AltF4));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo + Constants.ShortcutCtrlZ,
                    new EventHandler((sender, e) => browser.Undo())));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemRedo + Constants.ShortcutCtrlY,
                    new EventHandler((sender, e) => browser.Redo())));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut + Constants.ShortcutCtrlX,
                    new EventHandler((sender, e) => browser.Cut())));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy + Constants.ShortcutCtrlC,
                    new EventHandler((sender, e) => browser.Copy())));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste + Constants.ShortcutCtrlV,
                    new EventHandler((sender, e) => browser.Paste())));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete + Constants.ShortcutDelete,
                    new EventHandler((sender, e) => browser.Delete())));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll + Constants.ShortcutCtrlA,
                    new EventHandler((sender, e) => browser.SelectAll())));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomInCoarse + Constants.ShortcutShiftCtrlPlus,
                    new EventHandler(ZoomInCoarseAsync)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomInFine + Constants.ShortcutCtrlPlus,
                    new EventHandler(ZoomInFineAsync)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemActualSize + Constants.ShortcutCtrl0,
                    new EventHandler(ActualSizeAsync)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomOutFine + Constants.ShortcutCtrlMinus,
                    new EventHandler(ZoomOutFineAsync)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomOutCoarse + Constants.ShortcutShiftCtrlMinus,
                    new EventHandler(ZoomOutCoarseAsync)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReload + Constants.ShortcutF5,
                    new EventHandler(Reload)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReloadIgnoreCache + Constants.ShortcutCtrlF5,
                    new EventHandler(ReloadIgnoreCache)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAlwaysOnTop + Constants.ShortcutShiftCtrlT,
                    new EventHandler(ToggleTopMost)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemBetCalculator + Constants.ShortcutAltF10,
                    new EventHandler(OpenBetCalculator)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLogViewer + Constants.ShortcutAltL,
                    new EventHandler(OpenLogViewer)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemEncoderDecoder + Constants.ShortcutShiftCtrlN,
                    new EventHandler(OpenEncoderDecoder)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTurnOffMonitors + Constants.ShortcutF11,
                    new EventHandler(TurnOffMonitors)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchCalculator + Constants.ShortcutAltF11,
                    new EventHandler(LaunchCalculator)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchNotepad + Constants.ShortcutAltF12,
                    new EventHandler(LaunchNotepad)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchWordPad,
                    new EventHandler(LaunchWordPad)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchCharMap,
                    new EventHandler(LaunchCharMap)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLaunchTelegram,
                    new EventHandler(LaunchTelegram)));
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOnlineHelp + Constants.ShortcutF1,
                    new EventHandler(OpenHelp)));
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCheckForUpdates,
                    new EventHandler(CheckUpdates)));
                menuItemHelp.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemHelp.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAbout,
                    new EventHandler(ShowAbout)));
                Menu = mainMenu;
                menuItemView.Popup += new EventHandler((sender, e) => {
                    Menu.MenuItems[2].MenuItems[5].Enabled = canReload;
                    Menu.MenuItems[2].MenuItems[6].Enabled = canReload;
                });
                menuItemTools.Popup += new EventHandler((sender, e) => {
                    bool enabled = false;
                    try {
                        enabled = File.Exists(Path.Combine(settings.TelegramAppDirectory, Constants.TelegramDesktopFileName));
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                    menuItemTools.MenuItems[12].Enabled = enabled;
                });
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

        private async void InitializeUpdateCheckerAsync() {
            await Task.Run(new Action(() => {
                updateChecker = new UpdateChecker(settings);
                updateChecker.Parent = this;
                updateChecker.StateChanged += new EventHandler<UpdateCheckEventArgs>(OnUpdateCheckerStateChanged);
                updateChecker.Help += new HelpEventHandler(OpenHelp);
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

        private void InitializeBrowser() {
            try {
                Uri uri = new Uri(
                    Path.Combine(
                        Path.GetDirectoryName(Application.LocalUserAppDataPath),
                        Constants.CalculatorDirectoryName,
                        Constants.CalculatorFileName03));
                browser = new ChromiumWebBrowser(uri.AbsoluteUri);
                browser.AddressChanged += new EventHandler<AddressChangedEventArgs>((sender, e) =>
                    statusStripHandler.SetUrl(e.Address));
                browser.ConsoleMessage += new EventHandler<ConsoleMessageEventArgs>((sender, e) => {
                    if (settings.LogConsoleMessages) {
                        LogConsoleMessage(e.Line, e.Source, e.Message);
                    }
                    if (settings.ShowConsoleMessages) {
                        SetConsoleMessage(e.Line, e.Source, e.Message);
                    }
                });
                browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEndAsync);
                browser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>((sender, e) =>
                    canReload = e.CanReload);
                browser.LoadError += new EventHandler<LoadErrorEventArgs>((sender, e) => {
                    if (settings.LogLoadErrors) {
                        LogLoadError(e.ErrorCode, e.ErrorText, e.FailedUrl);
                    }
                    if (settings.ShowLoadErrors) {
                        SetLoadErrorMessage(e.ErrorCode, e.ErrorText, e.FailedUrl);
                    }
                });
                browser.StatusMessage += new EventHandler<StatusMessageEventArgs>((sender, e) =>
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.Value));
                browser.TitleChanged += new EventHandler<TitleChangedEventArgs>((sender, e) =>
                    browserTitle = e.Title);
                panel.Controls.Add(browser);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void InitializeStatusStripHandler() {
            statusStripHandler = new StatusStripHandler(statusStrip, StatusStripHandler.DisplayMode.Reduced, settings);
        }

        private static void InitializeCef(Settings settings) {
            CefSettings cefSettings = new CefSettings();
            if (settings.EnableDrmContent) {
                cefSettings.CefCommandLineArgs.Add(Constants.CefCLiArgEnableCdmKey, Constants.CefCLiArgEnableCdmVal);
            }
            if (!settings.EnableAudio) {
                cefSettings.CefCommandLineArgs.Add(Constants.CefCLiArgMuteAudioKey, Constants.CefCLiArgMuteAudioVal);
            }
            cefSettings.AcceptLanguageList = settings.AcceptLanguage;
            cefSettings.IgnoreCertificateErrors = settings.IgnoreCertificateErrors;
            cefSettings.LogFile = Path.Combine(Application.LocalUserAppDataPath, Constants.CefDebugLogFileName);
            cefSettings.UserAgent = settings.UserAgent;
            if (settings.EnableCache) {
                cefSettings.RootCachePath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
                cefSettings.CachePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath),
                    Constants.BrowserCacheMngrCacheSubDirName);
                cefSettings.PersistSessionCookies = settings.PersistSessionCookies;
                cefSettings.PersistUserPreferences = settings.PersistUserPreferences;
            }
            if (settings.EnablePrintPreview) {
                cefSettings.EnablePrintPreview();
            }
            Cef.Initialize(cefSettings, performDependencyCheck: false, browserProcessHandler: null);
        }

        private void InitializeShortcutManager() {
            shortcutManager = new ShortcutManager();
            shortcutManager.ActualSize += new EventHandler(ActualSizeAsync);
            shortcutManager.Export += new EventHandler(ExportAsync);
            shortcutManager.LaunchCalculator += new EventHandler(LaunchCalculator);
            shortcutManager.LaunchNotepad += new EventHandler(LaunchNotepad);
            shortcutManager.OpenBetCalculator += new EventHandler(OpenBetCalculator);
            shortcutManager.OpenEncoderDecoder += new EventHandler(OpenEncoderDecoder);
            shortcutManager.OpenHelp += new EventHandler(OpenHelp);
            shortcutManager.OpenLogViewer += new EventHandler(OpenLogViewer);
            shortcutManager.Print += new EventHandler(Print);
            shortcutManager.PrintImage += new EventHandler(PrintImage);
            shortcutManager.PrintToPdf += new EventHandler(PrintToPdf);
            shortcutManager.Reload += new EventHandler(Reload);
            shortcutManager.ReloadIgnoreCache += new EventHandler(ReloadIgnoreCache);
            shortcutManager.ToggleTopMost += new EventHandler(ToggleTopMost);
            shortcutManager.TurnOffMonitors += new EventHandler(TurnOffMonitors);
            shortcutManager.ZoomInCoarse += new EventHandler(ZoomInCoarseAsync);
            shortcutManager.ZoomInFine += new EventHandler(ZoomInFineAsync);
            shortcutManager.ZoomOutCoarse += new EventHandler(ZoomOutCoarseAsync);
            shortcutManager.ZoomOutFine += new EventHandler(ZoomOutFineAsync);
            shortcutManager.AddForm(this);
        }

        private void SubscribeEvents() {
            Activated += new EventHandler(OnFormActivated);
            FormClosing += new FormClosingEventHandler(OnFormClosing);
            browserCacheManager.CacheSizeComputed += new EventHandler<BrowserCacheEventArgs>((sender, e) =>
                statusStripHandler.SetDataSize(e.BrowserCacheSize));
        }

        private void ToggleTopMost(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ToggleTopMost), sender, e);
            } else {
                Menu.MenuItems[3].MenuItems[0].Checked = !Menu.MenuItems[3].MenuItems[0].Checked;
                TopMost = Menu.MenuItems[3].MenuItems[0].Checked;
                if (!TopMost) {
                    SendToBack();
                }
            }
        }

        private async void ZoomInCoarseAsync(object sender, EventArgs e) {
            double zoomLevel = await browser.GetZoomLevelAsync();
            browser.SetZoomLevel((Math.Floor(zoomLevel * 2) + 1) / 2);
            zoomLevel = await browser.GetZoomLevelAsync();
            statusStripHandler.SetZoomLevel(zoomLevel);
            Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
        }

        private async void ZoomOutCoarseAsync(object sender, EventArgs e) {
            double zoomLevel = await browser.GetZoomLevelAsync();
            browser.SetZoomLevel((Math.Ceiling(zoomLevel * 2) - 1) / 2);
            zoomLevel = await browser.GetZoomLevelAsync();
            statusStripHandler.SetZoomLevel(zoomLevel);
            Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
        }

        private async void ZoomInFineAsync(object sender, EventArgs e) {
            double zoomLevel = await browser.GetZoomLevelAsync();
            browser.SetZoomLevel(zoomLevel + 0.1);
            zoomLevel = await browser.GetZoomLevelAsync();
            statusStripHandler.SetZoomLevel(zoomLevel);
            Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
        }

        private async void ZoomOutFineAsync(object sender, EventArgs e) {
            double zoomLevel = await browser.GetZoomLevelAsync();
            browser.SetZoomLevel(zoomLevel - 0.1);
            zoomLevel = await browser.GetZoomLevelAsync();
            statusStripHandler.SetZoomLevel(zoomLevel);
            Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
        }

        private async void ActualSizeAsync(object sender, EventArgs e) {
            double zoomLevel = await browser.GetZoomLevelAsync();
            if (!zoomLevel.Equals(0)) {
                browser.SetZoomLevel(0);
                zoomLevel = await browser.GetZoomLevelAsync();
                statusStripHandler.SetZoomLevel(zoomLevel);
                Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
            }
        }

        private async void OnFrameLoadEndAsync(object sender, EventArgs e) {
            try {
                if (((ChromiumWebBrowser)sender).InvokeRequired) {
                    Invoke(new EventHandler(OnFrameLoadEndAsync), sender, e);
                } else {
                    ((ChromiumWebBrowser)sender).SetZoomLevel(Constants.CalculatorWindowDefaultZoomLevel);
                    double zoomLevel = await browser.GetZoomLevelAsync();
                    statusStripHandler.SetZoomLevel(zoomLevel);
                    Menu.MenuItems[2].MenuItems[2].Enabled = !zoomLevel.Equals(0);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private async void ExportAsync(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ExportAsync), sender, e);
            } else {
                try {
                    saveFileDialog.Filter = fileExtensionFilter.GetFilter();
                    saveFileDialog.FilterIndex = fileExtensionFilter.GetFilterIndex();
                    saveFileDialog.Title = Properties.Resources.CaptionExport;
                    saveFileDialog.FileName = new StringBuilder()
                        .Append(Application.ProductName)
                        .Append(Constants.Underscore)
                        .Append(DateTime.Now.ToString(Constants.TimeFormatForFilename))
                        .Append(Constants.Underscore)
                        .Append(ASCII.Convert(browserTitle, Encoding.Unicode, ASCII.ConversionOptions.Alphanumeric))
                        .ToString();
                    if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessageExporting);
                        using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height, PixelFormat.Format32bppArgb)) {
                            Point upperLeftSource = browser.PointToScreen(Point.Empty);
                            await Task.Run(new Action(() => {
                                Thread.Sleep(Constants.ScreenFormCaptureDelay);
                                using (Graphics graphics = Graphics.FromImage(bitmap)) {
                                    graphics.CopyFromScreen(
                                        upperLeftSource,
                                        Point.Empty,
                                        bitmap.Size,
                                        CopyPixelOperation.SourceCopy);
                                }
                                StaticMethods.SaveBitmap(bitmap, saveFileDialog.FileName);
                                fileExtensionFilter.SetFilterIndex(saveFileDialog.FilterIndex);
                                settings.ExtensionFilterIndex = saveFileDialog.FilterIndex;
                                settings.LastExportDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                            }))
                            .ContinueWith(new Action<Task>(task => statusStripHandler.SetMessage(
                                StatusStripHandler.StatusMessageType.Temporary,
                                Properties.Resources.MessageExportFinished)));
                        }
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessageExportFailed);
                }
            }
        }

        private void Print(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Print), sender, e);
            } else {
                try {
                    browser.Print();
                    statusStripHandler.SetMessage(
                        StatusStripHandler.StatusMessageType.Temporary,
                        Properties.Resources.MessagePrintingFinished);
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintToPdf(object sender, EventArgs e) {
            if (browser.InvokeRequired) {
                browser.Invoke(new EventHandler(PrintToPdf), sender, e);
            } else {
                try {
                    saveFileDialog.Filter = Properties.Resources.ExtensionFilterPdf;
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.Title = Properties.Resources.CaptionPrintToPdf;
                    saveFileDialog.FileName = new StringBuilder()
                        .Append(Application.ProductName)
                        .Append(Constants.Underscore)
                        .Append(DateTime.Now.ToString(Constants.TimeFormatForFilename))
                        .Append(Constants.Underscore)
                        .Append(ASCII.Convert(browserTitle, Encoding.Unicode, ASCII.ConversionOptions.Alphanumeric))
                        .ToString();
                    if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessagePrinting);
                        browser.PrintToPdfAsync(saveFileDialog.FileName)
                            .ContinueWith(new Action<Task>(task => statusStripHandler.SetMessage(
                                StatusStripHandler.StatusMessageType.Temporary,
                                Properties.Resources.MessagePrintingFinished)));
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintImagePreview(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintImagePreview), sender, e);
            } else {
                try {
                    Thread.Sleep(Constants.ScreenFormCaptureDelay);
                    bitmap = new Bitmap(browser.Width, browser.Height, PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(bitmap)) {
                        graphics.CopyFromScreen(
                            browser.PointToScreen(Point.Empty),
                            Point.Empty,
                            bitmap.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    dialog = printPreviewDialog;
                    dialog.WindowState = WindowState.Equals(FormWindowState.Minimized)
                        ? FormWindowState.Normal
                        : WindowState;
                    if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        printDocument.Print();
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintImage(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintImage), sender, e);
            } else {
                try {
                    Thread.Sleep(Constants.ScreenFormCaptureDelay);
                    bitmap = new Bitmap(browser.Width, browser.Height, PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(bitmap)) {
                        graphics.CopyFromScreen(
                            browser.PointToScreen(Point.Empty),
                            Point.Empty,
                            bitmap.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    printDialog.AllowPrintToFile = true;
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
            StringBuilder title = new StringBuilder(Program.GetTitle())
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
            if (bitmap != null) {
                printAction = e.PrintAction;
                printDocument.OriginAtMargins = settings.PrintSoftMargins;
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.Temporary,
                    e.PrintAction.Equals(PrintAction.PrintToPreview)
                        ? Properties.Resources.MessageGeneratingPreview
                        : Properties.Resources.MessagePrinting);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e) {
            if (bitmap != null) {
                RectangleF marginBounds = e.MarginBounds;
                RectangleF printableArea = e.PageSettings.PrintableArea;
                if (printAction.Equals(PrintAction.PrintToPreview)) {
                    e.Graphics.TranslateTransform(printableArea.X, printableArea.Y);
                }
                int availableWidth = (int)Math.Floor(printDocument.OriginAtMargins
                    ? marginBounds.Width
                    : e.PageSettings.Landscape ? printableArea.Height : printableArea.Width);
                int availableHeight = (int)Math.Floor(printDocument.OriginAtMargins
                    ? marginBounds.Height
                    : e.PageSettings.Landscape ? printableArea.Width : printableArea.Height);
                Size availableSize = new Size(availableWidth, availableHeight);
                bool rotate = StaticMethods.IsGraphicsRotationNeeded(bitmap.Size, availableSize);
                if (rotate) {
                    e.Graphics.RotateTransform(90, MatrixOrder.Prepend);
                }
                Size size = StaticMethods.GetNewGraphicsSize(bitmap.Size, availableSize);
                e.Graphics.DrawImage(bitmap, 0, rotate ? -availableWidth : 0, size.Width, size.Height);
                e.HasMorePages = false;
            }
        }

        private void EndPrint(object sender, PrintEventArgs e) {
            statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.Temporary,
                e.PrintAction.Equals(PrintAction.PrintToPreview)
                    ? Properties.Resources.MessagePreviewGenerated
                    : Properties.Resources.MessagePrintingFinished);
        }

        private void OnUpdateCheckerStateChanged(object sender, UpdateCheckEventArgs e) {
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary, e.Message);
            if (dialog == null || !dialog.Visible) {
                dialog = e.Dialog;
            }
        }

        private void Reload(object sender, EventArgs e) {
            if (canReload) {
                browser.GetBrowser().Reload(false);
            }
        }

        private void ReloadIgnoreCache(object sender, EventArgs e) {
            if (canReload) {
                browser.GetBrowser().Reload(true);
            }
        }

        private void OpenBetCalculator(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenBetCalculator), sender, e);
            } else {
                try {
                    Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWC);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    StringBuilder title = new StringBuilder()
                        .Append(Program.GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionError);
                    dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK,
                        MessageForm.BoxIcon.Error);
                    dialog.HelpRequested += new HelpEventHandler(OpenHelp);
                    dialog.ShowDialog(this);
                }
            }
        }

        private void OpenLogViewer(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenLogViewer), sender, e);
            } else {
                try {
                    Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWL);
                } catch (Exception exception) {
                    ShowException(exception);
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

        private void TurnOffMonitors(object sender, EventArgs e) {
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
            if (InvokeRequired) {
                Invoke(new EventHandler(LaunchWordPad), sender, e);
            } else {
                try {
                    Process.Start(Path.Combine(Environment.SystemDirectory, Constants.WordPadFileName));
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void LaunchCharMap(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(LaunchCharMap), sender, e);
            } else {
                try {
                    Process.Start(Path.Combine(Environment.SystemDirectory, Constants.CharMapFileName));
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void LaunchTelegram(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(LaunchTelegram), sender, e);
            } else {
                try {
                    Process.Start(Path.Combine(settings.TelegramAppDirectory, Constants.TelegramDesktopFileName));
                } catch (Exception exception) {
                    ShowException(exception);
                }
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

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            if (InvokeRequired) {
                Invoke(new FormClosingEventHandler(OnFormClosing), sender, e);
            } else {
                if (countDownForm != null) {
                    countDownForm.HelpRequested -= new HelpEventHandler(OpenHelp);
                    if (countDownForm.Visible) {
                        countDownForm.SafeClose();
                    }
                }
                if (bitmap != null) {
                    bitmap.Dispose();
                }
                browserCacheManager.Dispose();
                settings.Dispose();
                shortcutManager.Dispose();
                statusStripHandler.Dispose();
                updateChecker.Dispose();
                browser.Dispose();
                Cef.Shutdown();
            }
        }

        private void SetConsoleMessage(int line, string source, string message) {
            if (!string.IsNullOrEmpty(source)) {
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.PersistentB,
                    string.IsNullOrWhiteSpace(message)
                        ? string.Format(Constants.BrowserConsoleMessageFormat2, line, source.Trim())
                        : string.Format(Constants.BrowserConsoleMessageFormat1, line, source.Trim(), message.Trim()));
            }
        }

        private void SetLoadErrorMessage(CefErrorCode errorCode, string errorText, string failedUrl) {
            if (!errorCode.Equals(CefErrorCode.None) && !string.IsNullOrEmpty(errorText) && !string.IsNullOrEmpty(failedUrl)) {
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.PersistentB,
                    string.Format(Constants.BrowserLoadErrorMessageFormat1, errorText.Trim(), failedUrl.Trim()));
            }
        }

        private static void LogConsoleMessage(int line, string source, string message) {
            if (!string.IsNullOrEmpty(source)) {
                try {
                    string fileName = Path.Combine(Application.LocalUserAppDataPath, Constants.ConsoleMessageLogFileName);
                    using (StreamWriter streamWriter = File.AppendText(fileName)) {
                        StringBuilder stringBuilder = new StringBuilder()
                            .Append(DateTime.Now.ToString(Constants.ErrorLogTimeFormat))
                            .Append(Constants.VerticalTab)
                            .Append(Properties.Resources.CaptionBetCalculator)
                            .Append(Constants.VerticalTab)
                            .Append(string.IsNullOrWhiteSpace(message)
                                ? string.Format(Constants.BrowserConsoleMessageFormat2, line, source.Trim())
                                : string.Format(Constants.BrowserConsoleMessageFormat1, line, source.Trim(), message.Trim()));
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private static void LogLoadError(CefErrorCode errorCode, string errorText, string failedUrl) {
            if (!errorCode.Equals(CefErrorCode.None) && !string.IsNullOrEmpty(errorText) && !string.IsNullOrEmpty(failedUrl)) {
                try {
                    string fileName = Path.Combine(Application.LocalUserAppDataPath, Constants.LoadErrorLogFileName);
                    using (StreamWriter streamWriter = File.AppendText(fileName)) {
                        StringBuilder stringBuilder = new StringBuilder()
                            .Append(DateTime.Now.ToString(Constants.ErrorLogTimeFormat))
                            .Append(Constants.VerticalTab)
                            .Append(Properties.Resources.CaptionBetCalculator)
                            .Append(Constants.VerticalTab)
                            .Append(string.Format(Constants.BrowserLoadErrorMessageFormat1, errorText.Trim(), failedUrl.Trim()));
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }
    }
}
