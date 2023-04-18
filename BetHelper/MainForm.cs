﻿/**
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
 * Version 1.1.3.1
 */

using CefSharp;
using CefSharp.WinForms;
using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class MainForm : Form {
        private Bitmap bitmap;
        private BookmarkManager bookmarkManager;
        private bool close, restart, searching, servicesAlreadySorted, suppressSaveData, tipsAlreadySorted, undone;
        private BrowserCacheManager browserCacheManager;
        private CalculatorExtractor calculatorExtractor;
        private CalculatorHandler calculatorHandler;
        private ControlInfo controlInfo;
        private CountDownForm countDownForm;
        private decimal[] trustDegrees;
        private ExtBrowsHandler extBrowsHandler;
        private FileExtensionFilter fileExtensionFilter;
        private FindForm findForm;
        private Form dialog, form;
        private Graphics graphics;
        private Image image;
        private int textBoxClicks;
        private ListViewItem clickedListViewItemTip, clickedListViewItemService;
        private ListViewSorter listViewTipsSorter, listViewServicesSorter;
        private Pen crosshairPen;
        private PersistWindowState persistWindowState;
        private Point location;
        private PrintAction printAction;
        private PrintDialog printDialog;
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        private SaveFileDialog saveFileDialog;
        private Search search;
        private Settings settings;
        private ShortcutManager shortcutManager;
        private StatusStripHandler statusStripHandler;
        private string dataFilePath, undoneNotes;
        private TelephoneBell telephoneBell;
        private Thread findThread, turnOffThread;
        private System.Windows.Forms.Timer textBoxClicksTimer;
        private UpdateChecker updateChecker;
        private WebInfoHandler webInfoHandler;

        private delegate void MainFormSizeEventHandler(Size size);
        private delegate void SetTabControlCallback();
        private delegate void InspectDomElementCallback();

        public MainForm(Settings settings) {
            browserCacheManager = new BrowserCacheManager();
            calculatorExtractor = new CalculatorExtractor();
            extBrowsHandler = new ExtBrowsHandler();
            telephoneBell = new TelephoneBell();

            this.settings = settings;
            trustDegrees = new decimal[10];

            Icon = Properties.Resources.Icon;
            Text = Program.GetTitle();

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

            dataFilePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.RightPaneDataFileName);

            InitializePrintAsync();
            InitializeUpdateCheckerAsync();
            InitializeSaveFileDialogAsync();
            BuildMainMenuAsync();
            InitializeShortcutManager();

            InitializeComponent();
            BuildContextMenuAsync();
            BuildListViewHeadersAsync();

            SuspendLayout();
            SetTabControlAsync();
            InitializeCef(settings);
            InitializeCalculatorHandler();
            InitializeStatusStripHandler();
            InitializeWebInfoHandler(LoadRightPaneData());
            textBoxNotes.Font = new Font(Constants.MonospaceFontName, 10, FontStyle.Regular, GraphicsUnit.Point, 238);
            buttonClearNotes.Enabled = textBoxNotes.TextLength > 0;
            ResumeLayout(false);
            PerformLayout();
            SetTabControls();
        }

        public BrowserCacheManager BrowserCacheManager => browserCacheManager;

        public ExtBrowsHandler ExtBrowsHandler => extBrowsHandler;

        public Settings Settings => settings;

        public ShortcutManager ShortcutManager => shortcutManager;

        public StatusStripHandler StatusStripHandler => statusStripHandler;

        public TabControl TabControlLeft => tabControlLeft;

        private async void BuildMainMenuAsync() {
            await Task.Run(new Action(() => {
                MainMenu mainMenu = new MainMenu();
                MenuItem menuItemFile = mainMenu.MenuItems.Add(Properties.Resources.MenuItemFile);
                MenuItem menuItemEdit = mainMenu.MenuItems.Add(Properties.Resources.MenuItemEdit);
                MenuItem menuItemView = mainMenu.MenuItems.Add(Properties.Resources.MenuItemView);
                MenuItem menuItemBookmarks = mainMenu.MenuItems.Add(Properties.Resources.MenuItemBookmarks);
                MenuItem menuItemOptions = mainMenu.MenuItems.Add(Properties.Resources.MenuItemOptions);
                MenuItem menuItemTools = mainMenu.MenuItems.Add(Properties.Resources.MenuItemTools);
                MenuItem menuItemHelp = mainMenu.MenuItems.Add(Properties.Resources.MenuItemHelp);
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpen + Constants.ShortcutCtrlO,
                    new EventHandler(Open)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExport + Constants.ShortcutCtrlE,
                    new EventHandler(ExportAsync)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExportWindow + Constants.ShortcutShiftCtrlE,
                    new EventHandler(ExportWindowAsync)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrint + Constants.ShortcutCtrlP,
                    new EventHandler(Print)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintToPdf + Constants.ShortcutShiftCtrlP,
                    new EventHandler(PrintToPdf)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImagePreview,
                    new EventHandler(PrintImagePreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintImage + Constants.ShortcutAltShiftCtrlP,
                    new EventHandler(PrintImage)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintWindowPreview,
                    new EventHandler(PrintWindowPreview)));
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPrintWindow,
                    new EventHandler(PrintWindow)));
                menuItemFile.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemFile.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExit,
                    new EventHandler((sender, e) => Close()), Shortcut.AltF4));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo + Constants.ShortcutCtrlZ,
                    new EventHandler(BrowserUndo)));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemRedo + Constants.ShortcutCtrlY,
                    new EventHandler(BrowserRedo)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut + Constants.ShortcutCtrlX,
                    new EventHandler(BrowserCut)));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy + Constants.ShortcutCtrlC,
                    new EventHandler(BrowserCopy)));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste + Constants.ShortcutCtrlV,
                    new EventHandler(BrowserPaste)));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete + Constants.ShortcutDelete,
                    new EventHandler(BrowserDelete)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll + Constants.ShortcutCtrlA,
                    new EventHandler(BrowserSelectAll)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTrustDegrees + Constants.ShortcutF2,
                    new EventHandler(FocusTrustDegrees)));
                menuItemEdit.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemFind + Constants.ShortcutCtrlF,
                    new EventHandler(Find)));
                menuItemEdit.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemFindNext + Constants.ShortcutF3,
                    new EventHandler(FindNext)));
                menuItemView.MenuItems.Add(new MenuItem(string.Empty, new EventHandler(ToggleRightPane)));
                menuItemView.MenuItems.Add(new MenuItem(string.Empty, new EventHandler(ToggleRightPanetWidth)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomInCoarse + Constants.ShortcutShiftCtrlPlus,
                    new EventHandler(ZoomInCoarse)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomInFine + Constants.ShortcutCtrlPlus,
                    new EventHandler(ZoomInFine)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemActualSize + Constants.ShortcutCtrl0,
                    new EventHandler(ActualSize)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomOutFine + Constants.ShortcutCtrlMinus,
                    new EventHandler(ZoomOutFine)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemZoomOutCoarse + Constants.ShortcutShiftCtrlMinus,
                    new EventHandler(ZoomOutCoarse)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemGoBack + Constants.ShortcutAltLeft,
                    new EventHandler(GoBack)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemGoForward + Constants.ShortcutAltRight,
                    new EventHandler(GoForward)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemGoHome + Constants.ShortcutAltHome,
                    new EventHandler(GoHome)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemStopLoad + Constants.ShortcutEsc,
                    new EventHandler(StopLoad)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUnload + Constants.ShortcutF4,
                    new EventHandler(UnloadPage)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReload + Constants.ShortcutF5,
                    new EventHandler(Reload)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemReloadIgnoreCache + Constants.ShortcutCtrlF5,
                    new EventHandler(ReloadIgnoreCache)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemViewSource + Constants.ShortcutCtrlU,
                    new EventHandler(ViewSource)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(string.Empty, new EventHandler(ToggleMuteAudio)));
                menuItemView.MenuItems.Add(new MenuItem(string.Empty, new EventHandler(ToggleShowChat)));
                menuItemView.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLogIn + Constants.ShortcutF8,
                    new EventHandler(LogIn)));
                menuItemView.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLogInAtInitialPage + Constants.ShortcutAltF8,
                    new EventHandler(LogInAtInitialPage)));
                menuItemBookmarks.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAddBookmark + Constants.ShortcutCtrlD,
                    new EventHandler(AddBookmark)));
                menuItemBookmarks.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemRemoveBookmark,
                    new EventHandler(RemoveBookmark)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAutoAdjustRightPaneWidth,
                    new EventHandler(AutoAdjustRightPaneWidth)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAutoLogInAfterInitialLoad,
                    new EventHandler(AutoLogInAfterInitialLoad)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemTryToKeepUserLoggedIn,
                    new EventHandler(TryToKeepUserLoggedIn)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemEnableBell,
                    new EventHandler(ToggleBell)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemBellTest,
                    new EventHandler((sender, e) => telephoneBell.Ring())));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemStopRinging + Constants.ShortcutF6,
                    new EventHandler((sender, e) => telephoneBell.Stop())));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemResetIpCheckLock + Constants.ShortcutF7,
                    new EventHandler((sender, e) => settings.AllowedAddrHandler.Reset())));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemClearBrowserCache,
                    new EventHandler(ClearBrowserCache)));
                menuItemOptions.MenuItems.Add(new MenuItem(
                    Properties.Resources.MenuItemClearBrowserCacheInclUserData + Constants.ShortcutShiftCtrlDelete,
                    new EventHandler(ClearBrowserCacheInclUserData)));
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemApplicationReset,
                    new EventHandler(ApplicationReset)));
                menuItemOptions.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemOptions.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPreferences + Constants.ShortcutCtrlG,
                    new EventHandler(ShowPreferences)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemNewTabBetCalculator + Constants.ShortcutCtrlT,
                    new EventHandler(NewTabBetCalculator)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemBetCalculator + Constants.ShortcutAltF10,
                    new EventHandler(OpenBetCalculator)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDeveloperTools + Constants.ShortcutF12,
                    new EventHandler(OpenDevTools)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemInspectDomElement,
                    new EventHandler((sender, e) => InspectDomElement())));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenPageInDefaultBrowser,
                    new EventHandler(OpenPageInDefaultBrowser)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenPageInGoogleChrome,
                    new EventHandler(OpenPageInGoogleChrome)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemOpenPageInMozillaFirefox,
                    new EventHandler(OpenPageInMozillaFirefox)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemAnalyzeCurrentUrl,
                    new EventHandler(AnalyzeUrl)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyCurrentUrl,
                    new EventHandler(CopyCurrentUrl)));
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyCurrentTitle,
                    new EventHandler(CopyCurrentTitle)));
                menuItemTools.MenuItems.Add(Constants.Hyphen.ToString());
                menuItemTools.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemWebInfo + Constants.ShortcutCtrlI,
                    new EventHandler(OpenWebInfo)));
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
                menuItemFile.Popup += new EventHandler((sender, e) => {
                    ControlInfo controlInfo = GetControlEntered();
                    bool enabled = controlInfo != null
                        && controlInfo.Browser != null
                        && !controlInfo.Browser.Address.Equals(Constants.BlankPageUri);
                    menuItemFile.MenuItems[2].Enabled = enabled;
                    menuItemFile.MenuItems[5].Enabled = enabled;
                    menuItemFile.MenuItems[6].Enabled = enabled;
                    menuItemFile.MenuItems[7].Enabled = enabled;
                    menuItemFile.MenuItems[8].Enabled = enabled;
                });
                menuItemEdit.Popup += new EventHandler((sender, e) => {
                    ControlInfo controlInfo = GetControlEntered();
                    bool enabled = controlInfo != null
                        && controlInfo.Browser != null
                        && !controlInfo.Browser.Address.Equals(Constants.BlankPageUri);
                    menuItemEdit.MenuItems[0].Enabled = enabled;
                    menuItemEdit.MenuItems[1].Enabled = enabled;
                    menuItemEdit.MenuItems[3].Enabled = enabled;
                    menuItemEdit.MenuItems[4].Enabled = enabled;
                    menuItemEdit.MenuItems[5].Enabled = enabled;
                    menuItemEdit.MenuItems[6].Enabled = enabled;
                    menuItemEdit.MenuItems[8].Enabled = enabled;
                    menuItemEdit.MenuItems[12].Enabled = !string.IsNullOrEmpty(webInfoHandler.GetCurrentAddress());
                    menuItemEdit.MenuItems[13].Enabled = !string.IsNullOrEmpty(search.searchString)
                        && menuItemEdit.MenuItems[10].Enabled;
                });
                menuItemView.Popup += new EventHandler(UpdateViewMenuItemsAsync);
                menuItemBookmarks.Popup += new EventHandler((sender, e) => {
                    menuItemBookmarks.MenuItems[0].Enabled = webInfoHandler.Current != null
                        && !string.IsNullOrEmpty(webInfoHandler.Current.BrowserAddress)
                        && !webInfoHandler.Current.BrowserAddress.Equals(Constants.BlankPageUri)
                        && !bookmarkManager.Contains(webInfoHandler.Current.BrowserAddress);
                    menuItemBookmarks.MenuItems[1].Enabled = webInfoHandler.Current != null
                        && bookmarkManager.Contains(webInfoHandler.Current.BrowserAddress);
                });
                menuItemOptions.Popup += new EventHandler((sender, e) => {
                    menuItemOptions.MenuItems[0].Checked = settings.AutoAdjustRightPaneWidth;
                    menuItemOptions.MenuItems[2].Checked = settings.AutoLogInAfterInitialLoad;
                    menuItemOptions.MenuItems[3].Checked = settings.TryToKeepUserLoggedIn;
                    menuItemOptions.MenuItems[5].Checked = settings.EnableBell;
                    menuItemOptions.MenuItems[10].Enabled = settings.AllowedAddrHandler.Locked;
                });
                menuItemTools.Popup += new EventHandler((sender, e) => {
                    bool enabled = !string.IsNullOrEmpty(webInfoHandler.GetCurrentAddress());
                    menuItemTools.MenuItems[3].Enabled = enabled;
                    menuItemTools.MenuItems[4].Enabled = enabled;
                    menuItemTools.MenuItems[6].Enabled = enabled;
                    menuItemTools.MenuItems[7].Enabled = enabled
                        && extBrowsHandler.Exists(ExtBrowsHandler.Browser.GoogleChrome);
                    menuItemTools.MenuItems[8].Enabled = enabled
                        && extBrowsHandler.Exists(ExtBrowsHandler.Browser.Firefox);
                    menuItemTools.MenuItems[10].Enabled = enabled;
                    menuItemTools.MenuItems[12].Enabled = enabled;
                    menuItemTools.MenuItems[13].Enabled = enabled
                        && !string.IsNullOrEmpty(webInfoHandler.Current.BrowserTitle);
                    menuItemTools.MenuItems[15].Enabled = enabled;
                    enabled = false;
                    try {
                        enabled = File.Exists(Path.Combine(settings.TelegramAppDirectory, Constants.TelegramDesktopFileName));
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                    menuItemTools.MenuItems[27].Enabled = enabled;
                });
            }))
            .ContinueWith(new Action<Task>(task => InitializeBookmarkManager(Menu.MenuItems[3])));
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Copy())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = textBox.SelectionLength < textBox.TextLength;
                });
                textBoxBalance.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => {
                        TextBox textBox = (TextBox)((MenuItem)sender).GetContextMenu().SourceControl;
                        if (textBox.CanUndo) {
                            textBox.Undo();
                        } else if (textBox.Multiline) {
                            if (!string.IsNullOrEmpty(undoneNotes)) {
                                textBox.Text = undoneNotes;
                                textBox.SelectAll();
                                undone = true;
                            } else if (undone) {
                                if (textBox.TextLength > 0) {
                                    string str = textBox.Text;
                                    textBox.Clear();
                                    undoneNotes = str;
                                }
                                undone = false;
                            }
                        }
                    })));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Cut())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Copy())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Paste())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysDelete))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.CanUndo
                        || textBox.Multiline && (!string.IsNullOrEmpty(undoneNotes)
                        || undone);
                    bool enabled = textBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = enabled;
                    contextMenu.MenuItems[3].Enabled = enabled;
                    contextMenu.MenuItems[5].Enabled = enabled;
                    contextMenu.MenuItems[7].Enabled = textBox.SelectionLength < textBox.TextLength;
                    try {
                        contextMenu.MenuItems[4].Enabled = Clipboard.ContainsText();
                    } catch (Exception exception) {
                        contextMenu.MenuItems[4].Enabled = true;
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
                foreach (Control control in groupBoxTrustDegrees.Controls) {
                    if (control is TextBox) {
                        control.ContextMenu = contextMenu;
                    }
                }
                textBoxNotes.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemEditItem,
                    new EventHandler(EditTip)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPlace,
                    new EventHandler(PlaceTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSkip,
                    new EventHandler(SkipTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemWin,
                    new EventHandler(WinTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemVoid,
                    new EventHandler(VoidTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemLose,
                    new EventHandler(LoseTip)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemNew,
                    new EventHandler(NewTip)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelect,
                    new EventHandler(SelectTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUnselect,
                    new EventHandler(UnselectTip)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler(SelectAll)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectNone,
                    new EventHandler(SelectNone)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler(DeleteItem)));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    contextMenu.MenuItems[0].Enabled = listViewTips.SelectedItems.Count.Equals(1);
                    bool enabled = listViewTips.SelectedItems.Count > 0;
                    for (int i = 2; i < 7; i++) {
                        contextMenu.MenuItems[i].Enabled = enabled;
                    }
                    contextMenu.MenuItems[12].Enabled = listViewTips.SelectedItems.Count < listViewTips.Items.Count;
                    contextMenu.MenuItems[13].Enabled = enabled;
                    contextMenu.MenuItems[15].Enabled = enabled;
                });
                listViewTips.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemEditItem,
                    new EventHandler(EditService)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemActive,
                    new EventHandler(ActiveService)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemExpire,
                    new EventHandler(ExpireService)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemNew,
                    new EventHandler(NewService)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelect,
                    new EventHandler(SelectService)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUnselect,
                    new EventHandler(UnselectService)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler(SelectAll)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectNone,
                    new EventHandler(SelectNone)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler(DeleteItem)));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    contextMenu.MenuItems[0].Enabled = listViewServices.SelectedItems.Count.Equals(1);
                    bool enabled = listViewServices.SelectedItems.Count > 0;
                    for (int i = 2; i < 4; i++) {
                        contextMenu.MenuItems[i].Enabled = enabled;
                    }
                    contextMenu.MenuItems[9].Enabled = listViewServices.SelectedItems.Count < listViewServices.Items.Count;
                    contextMenu.MenuItems[10].Enabled = enabled;
                    contextMenu.MenuItems[12].Enabled = enabled;
                });
                listViewServices.ContextMenu = contextMenu;
            }));
        }

        private async void BuildListViewHeadersAsync() {
            await Task.Run(new Action(() => {
                listViewTips.Columns.AddRange(new ColumnHeader[] {
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderSport,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderMatch,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderLeague,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderDate,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderTime,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderOpportunity,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderBookmaker,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderOdd,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderTrustDegree,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderService,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderStatus,
                        TextAlign = HorizontalAlignment.Left
                    }
                });
                listViewTips.FullRowSelect = true;
                listViewTips.GridLines = true;
                listViewTips.HeaderStyle = ColumnHeaderStyle.Clickable;
                listViewTips.HideSelection = false;
                listViewTips.SmallImageList = GetTipImageList();
                listViewTipsSorter = new ListViewSorter();
                listViewTips.ListViewItemSorter = listViewTipsSorter;
                listViewTips.MultiSelect = true;
                listViewTips.View = View.Details;
                listViewTips.ColumnClick += new ColumnClickEventHandler((sender, e) => {
                    if (listViewTipsSorter.SortColumn.Equals(e.Column)) {
                        if (!tipsAlreadySorted && e.Column.Equals(0)) {
                            listViewTipsSorter.SortOrder = SortOrder.Descending;
                        } else {
                            listViewTipsSorter.SortOrder = listViewTipsSorter.SortOrder.Equals(SortOrder.Ascending)
                                ? SortOrder.Descending
                                : SortOrder.Ascending;
                        }
                        tipsAlreadySorted = true;
                    } else {
                        listViewTipsSorter.SortColumn = e.Column;
                        listViewTipsSorter.SortOrder = SortOrder.Ascending;
                    }
                    listViewTips.Sort();
                });
                listViewServices.Columns.AddRange(new ColumnHeader[] {
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderName,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderPrice,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderPeriod,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderExpiration,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderSubscribed,
                        TextAlign = HorizontalAlignment.Right
                    },
                    new ColumnHeader() {
                        Text = Constants.ColumnHeaderStatus,
                        TextAlign = HorizontalAlignment.Left
                    }
                });
                listViewServices.FullRowSelect = true;
                listViewServices.GridLines = true;
                listViewServices.HeaderStyle = ColumnHeaderStyle.Clickable;
                listViewServices.HideSelection = false;
                listViewServices.SmallImageList = GetServiceImageList();
                listViewServicesSorter = new ListViewSorter();
                listViewServices.ListViewItemSorter = listViewServicesSorter;
                listViewServices.MultiSelect = true;
                listViewServices.View = View.Details;
                listViewServices.ColumnClick += new ColumnClickEventHandler((sender, e) => {
                    if (listViewServicesSorter.SortColumn.Equals(e.Column)) {
                        if (!servicesAlreadySorted && e.Column.Equals(0)) {
                            listViewServicesSorter.SortOrder = SortOrder.Descending;
                        } else {
                            listViewServicesSorter.SortOrder = listViewServicesSorter.SortOrder.Equals(SortOrder.Ascending)
                                ? SortOrder.Descending
                                : SortOrder.Ascending;
                        }
                        servicesAlreadySorted = true;
                    } else {
                        listViewServicesSorter.SortColumn = e.Column;
                        listViewServicesSorter.SortOrder = SortOrder.Ascending;
                    }
                    listViewServices.Sort();
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

        private async void SetTabControlAsync() {
            await Task.Run(new Action(() => {
                tabControlLeft.Appearance = settings.TabAppearance;
                tabControlLeft.DrawMode = TabDrawMode.OwnerDrawFixed;
                tabControlLeft.Font = new Font(SystemFonts.CaptionFont, settings.TabsBoldFont ? FontStyle.Bold : FontStyle.Regular);
                tabControlLeft.DrawItem += new DrawItemEventHandler(DrawTabPageHeader);
                tabControlRight.Appearance = settings.TabAppearance;
                tabControlRight.DrawMode = TabDrawMode.OwnerDrawFixed;
                tabControlRight.Font = new Font(SystemFonts.CaptionFont, settings.TabsBoldFont ? FontStyle.Bold : FontStyle.Regular);
                tabControlRight.DrawItem += new DrawItemEventHandler(DrawTabPageHeader);
                tabPageDashboard.Font = SystemFonts.DefaultFont;
                tabPageServices.Font = SystemFonts.DefaultFont;
            }));
        }

        private void SetTabControls() {
            Task.Run(new Action(() => {
                foreach (WebInfo webInfo in webInfoHandler.WebInfos) {
                    tabControlLeft.TabPages.Add(new TabPage() { Text = webInfo.Title });
                }
                tabControlLeft.SelectedIndex = settings.ActivePanelLeft < 0 || settings.ActivePanelLeft > tabControlLeft.TabCount - 1
                    ? 0
                    : settings.ActivePanelLeft;
                tabControlRight.SelectedIndex = settings.ActivePanelRight < 0 || settings.ActivePanelRight > tabControlRight.TabCount - 1
                    ? 0
                    : settings.ActivePanelRight;
            }))
            .ContinueWith(new Action<Task>(task => SubscribeEvents()));
        }

        private void InitializeBookmarkManager(MenuItem menuItem) {
            bookmarkManager = new BookmarkManager(settings);
            bookmarkManager.Activated += new EventHandler<UrlEventArgs>((sender, e) => webInfoHandler.LoadUrl(e.Url));
            bookmarkManager.Added += new EventHandler((sender, e) => statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.Temporary,
                Properties.Resources.MessageBookmarkAdded));
            bookmarkManager.Removed += new EventHandler((sender, e) => statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.Temporary,
                Properties.Resources.MessageBookmarkRemoved));
            bookmarkManager.MenuItem = menuItem;
        }

        private void InitializeStatusStripHandler() {
            statusStripHandler = new StatusStripHandler(statusStrip, StatusStripHandler.DisplayMode.Standard, settings, extBrowsHandler);
        }

        private void InitializeCalculatorHandler() {
            calculatorHandler = new CalculatorHandler(tabControlRight);
            calculatorHandler.Enter += new EventHandler((sender, e) =>
                controlInfo = new ControlInfo((ChromiumWebBrowser)sender, calculatorHandler.BrowserTitle));
            calculatorHandler.TabAdded += new EventHandler<TabAddedEventArgs>((sender, e) =>
                Menu.MenuItems[5].MenuItems[0].Enabled = !e.IsFull);
        }

        private static void InitializeCef(Settings settings) {
            CefSettings cefSettings = new CefSettings();
            if (settings.EnableDrmContent) {
                cefSettings.CefCommandLineArgs.Add(Constants.CefCLiArgEmableCdmKey, Constants.CefCLiArgEmableCdmVal);
            }
            if (!settings.EnableAudio) {
                cefSettings.CefCommandLineArgs.Add(Constants.CefCLiArgMuteAudioKey, Constants.CefCLiArgMuteAudioVal);
            }
            cefSettings.AcceptLanguageList = settings.AcceptLanguage;
            cefSettings.IgnoreCertificateErrors = settings.IgnoreCertificateErrors;
            cefSettings.LogFile = Path.Combine(Application.LocalUserAppDataPath, Constants.CefDebugLogFileName);
            cefSettings.UserAgent = settings.UserAgent;
            if (settings.EnableCache) {
                cefSettings.UserDataPath = Path.Combine(
                    Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.UserDataRelativePath);
                cefSettings.RootCachePath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
                cefSettings.CachePath = Path.Combine(
                    Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.CacheRelativePath);
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
            shortcutManager.ActualSize += new EventHandler(ActualSize);
            shortcutManager.AddBookmark += new EventHandler(AddBookmark);
            shortcutManager.ClearBrowserCacheInclUserData += new EventHandler(ClearBrowserCacheInclUserData);
            shortcutManager.CtrlDown += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.Down));
            shortcutManager.CtrlEnd += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.End));
            shortcutManager.CtrlHome += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.Home));
            shortcutManager.CtrlPageDown += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.PageDown));
            shortcutManager.CtrlPageUp += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.PageUp));
            shortcutManager.CtrlUp += new EventHandler((sender, e) => webInfoHandler.SendKey(Keys.Up));
            shortcutManager.Escape += new EventHandler(OnEscape);
            shortcutManager.Export += new EventHandler(ExportAsync);
            shortcutManager.ExportWindowAsync += new EventHandler(ExportWindowAsync);
            shortcutManager.Find += new EventHandler(Find);
            shortcutManager.FindNext += new EventHandler(FindNext);
            shortcutManager.FindPrevious += new EventHandler(FindPrevious);
            shortcutManager.FocusTrustDegrees += new EventHandler(FocusTrustDegrees);
            shortcutManager.GoBack += new EventHandler(GoBack);
            shortcutManager.GoForward += new EventHandler(GoForward);
            shortcutManager.GoHome += new EventHandler(GoHome);
            shortcutManager.LaunchCalculator += new EventHandler(LaunchCalculator);
            shortcutManager.LaunchNotepad += new EventHandler(LaunchNotepad);
            shortcutManager.LogIn += new EventHandler(LogIn);
            shortcutManager.LogInAtInitialPage += new EventHandler(LogInAtInitialPage);
            shortcutManager.MuteAudio += new EventHandler(ToggleMuteAudio);
            shortcutManager.NewTabBetCalculator += new EventHandler(NewTabBetCalculator);
            shortcutManager.Open += new EventHandler(Open);
            shortcutManager.OpenBetCalculator += new EventHandler(OpenBetCalculator);
            shortcutManager.OpenDeveloperTools += new EventHandler(OpenDevTools);
            shortcutManager.OpenEncoderDecoder += new EventHandler(OpenEncoderDecoder);
            shortcutManager.OpenHelp += new EventHandler(OpenHelp);
            shortcutManager.OpenLogViewer += new EventHandler(OpenLogViewer);
            shortcutManager.OpenWebInfo += new EventHandler(OpenWebInfo);
            shortcutManager.Print += new EventHandler(Print);
            shortcutManager.PrintImage += new EventHandler(PrintImage);
            shortcutManager.PrintToPdf += new EventHandler(PrintToPdf);
            shortcutManager.Reload += new EventHandler(Reload);
            shortcutManager.ReloadIgnoreCache += new EventHandler(ReloadIgnoreCache);
            shortcutManager.ResetIpCheckLock += new EventHandler((sender, e) => settings.AllowedAddrHandler.Reset());
            shortcutManager.ShowPreferences += new EventHandler(ShowPreferences);
            shortcutManager.StopLoad += new EventHandler(StopLoad);
            shortcutManager.StopRinging += new EventHandler((sender, e) => telephoneBell.Stop());
            shortcutManager.ToggleRightPane += new EventHandler(ToggleRightPane);
            shortcutManager.ToggleRightPanetWidth += new EventHandler(ToggleRightPanetWidth);
            shortcutManager.TurnOffMonitors += new EventHandler(TurnOffMonitors);
            shortcutManager.UnloadPage += new EventHandler(UnloadPage);
            shortcutManager.ViewSource += new EventHandler(ViewSource);
            shortcutManager.ZoomInCoarse += new EventHandler(ZoomInCoarse);
            shortcutManager.ZoomInFine += new EventHandler(ZoomInFine);
            shortcutManager.ZoomOutCoarse += new EventHandler(ZoomOutCoarse);
            shortcutManager.ZoomOutFine += new EventHandler(ZoomOutFine);
            shortcutManager.AddForm(this);
        }

        private void InitializeWebInfoHandler(decimal[] balances) {
            webInfoHandler = new WebInfoHandler(this, balances);
            webInfoHandler.AddressChanged += new EventHandler<AddressChangedEventArgs>((sender, e) =>
                statusStripHandler.SetUrl(e.Address));
            webInfoHandler.BalanceGot += new EventHandler(ShowBalance);
            webInfoHandler.BrowserConsoleMessage += new EventHandler<ConsoleMessageEventArgs>((sender, e) =>
                SetConsoleMessage(e.Line, e.Source, e.Message));
            webInfoHandler.BrowserInitializedChanged += new EventHandler((sender, e) => ((WebInfo)sender).Browser.Focus());
            webInfoHandler.Canceled += new EventHandler<CanceledEventArgs>(OnLogInCanceled);
            webInfoHandler.CurrentSet += new EventHandler<FocusEventArgs>(OnCurrentSet);
            webInfoHandler.Enter += new EventHandler((sender, e) =>
                controlInfo = new ControlInfo((ChromiumWebBrowser)sender, webInfoHandler.Current.BrowserTitle));
            webInfoHandler.Error += new EventHandler<ErrorEventArgs>(OnLogInError);
            webInfoHandler.Find += new EventHandler<FindEventArgs>(OnFind);
            webInfoHandler.Finished += new EventHandler<FinishedEventArgs>(OnLogInFinished);
            webInfoHandler.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(async (sender, e) => {
                UpdateViewMenuItemsAsync(sender, e);
                statusStripHandler.SetZoomLevel(await ((WebInfo)sender).Browser.GetZoomLevelAsync());
                statusStripHandler.SetFinished();
            });
            webInfoHandler.FrameLoadStart += new EventHandler<FrameLoadStartEventArgs>((sender, e) => statusStripHandler.SetMaximum(0));
            webInfoHandler.Help += new HelpEventHandler(OpenHelp);
            webInfoHandler.Initialized += new EventHandler<FocusEventArgs>((sender, e) => {
                if (!tabControlLeft.SelectedIndex.Equals(e.Index)) {
                    tabControlLeft.SelectedIndex = e.Index;
                }
                TabControlLeft.SelectedTab.Controls.Add(((WebInfo)sender).Browser);
                AdjustSize(new Size(Constants.BrowserMinWidth, Constants.BrowserMinHeight));
            });
            webInfoHandler.LoadError += new EventHandler<LoadErrorEventArgs>((sender, e) =>
                SetLoadErrorMessage(e.ErrorCode, e.ErrorText, e.FailedUrl));
            webInfoHandler.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(async (sender, e) => {
                UpdateViewMenuItemsAsync(sender, e);
                statusStripHandler.SetZoomLevel(await ((WebInfo)sender).Browser.GetZoomLevelAsync());
                if (e.IsLoading) {
                    statusStripHandler.SetMaximum(0);
                } else {
                    statusStripHandler.SetFinished();
                }
            });
            webInfoHandler.Progress += new EventHandler<ProgressEventArgs>(OnLogInProgress);
            webInfoHandler.Started += new EventHandler<StartedEventArgs>(OnLogInStarted);
            webInfoHandler.StatusMessage += new EventHandler<StatusMessageEventArgs>((sender, e) =>
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.Value));
            webInfoHandler.TipsGot += new EventHandler(SetTips);
            webInfoHandler.TitleChanged += new EventHandler<TitleChangedEventArgs>((sender, e) => SetText(e.Title));
            webInfoHandler.ZoomLevelChanged += new EventHandler((sender, e) =>
                statusStripHandler.SetZoomLevel(((WebInfo)sender).ZoomLevel));
        }

        private void SubscribeEvents() {
            tabControlLeft.KeyDown += new KeyEventHandler((sender, e) => {
                if (e.KeyCode.Equals(Keys.Escape)
                        || e.KeyCode.Equals(Keys.Home)
                        || e.KeyCode.Equals(Keys.End)
                        || e.KeyCode.Equals(Keys.PageUp)
                        || e.KeyCode.Equals(Keys.PageDown)
                        || e.KeyCode.Equals(Keys.Up)
                        || e.KeyCode.Equals(Keys.Down)) {

                    e.SuppressKeyPress = true;
                    SendKeys.Send(Constants.SendKeysTab);
                }
            });

            tabControlLeft.MouseUp += new MouseEventHandler((sender, e) => {
                if (e.Button.Equals(MouseButtons.Left) || e.Button.Equals(MouseButtons.Middle)) {
                    SendKeys.Send(Constants.SendKeysTab);
                }
            });

            MouseWheel += new MouseEventHandler(OnMouseWheel);
            MouseMove += new MouseEventHandler(OnMouseWheel);

            textBox1.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox2.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox3.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox4.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox5.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox6.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox7.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox8.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox9.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);
            textBox10.KeyDown += new KeyEventHandler(OnTextBoxKeyDown);

            Load += new EventHandler(OnFormLoad);
            Activated += new EventHandler(OnFormActivated);
            FormClosing += new FormClosingEventHandler(OnFormClosing);
            FormClosed += new FormClosedEventHandler(OnFormClosed);

            browserCacheManager.CacheSizeComputed += new EventHandler<BrowserCacheEventArgs>((sender, e) =>
                statusStripHandler.SetDataSize(e.BrowserCacheSize));

            splitContainerMain.Paint += new PaintEventHandler((sender, e) =>
                e.Graphics.FillRectangle(SystemBrushes.ControlLight, ((SplitContainer)sender).SplitterRectangle));
        }

        private void ZoomInCoarse(object sender, EventArgs e) => webInfoHandler.ZoomInCoarse();

        private void ZoomOutCoarse(object sender, EventArgs e) => webInfoHandler.ZoomOutCoarse();

        private void ZoomInFine(object sender, EventArgs e) => webInfoHandler.ZoomInFine();

        private void ZoomOutFine(object sender, EventArgs e) => webInfoHandler.ZoomOutFine();

        private void ActualSize(object sender, EventArgs e) => webInfoHandler.ActualSize();

        private void GoBack(object sender, EventArgs e) {
            if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && webInfoHandler.Current.Browser.GetBrowser().CanGoBack) {

                webInfoHandler.Current.Browser.GetBrowser().GoBack();
            }
        }

        private void GoForward(object sender, EventArgs e) {
            if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && webInfoHandler.Current.Browser.GetBrowser().CanGoForward) {

                webInfoHandler.Current.Browser.GetBrowser().GoForward();
            }
        }

        private void GoHome(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.Browser.Load(webInfoHandler.Current.Url);
            }
        }

        private void StopLoad(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.Browser.GetBrowser().StopLoad();
            }
        }

        private void OnEscape(object sender, EventArgs e) {
            if (searching) {
                webInfoHandler.StopFinding();
                statusStripHandler.ClearSearchResult();
                searching = false;
            }
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.Browser.GetBrowser().StopLoad();
            }
        }

        private void Open(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(Open), sender, e);
            } else {
                OpenForm openForm = new OpenForm(webInfoHandler);
                openForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
                openForm.HelpRequested += new HelpEventHandler(OpenHelp);
                dialog = openForm;
                openForm.ShowDialog(this);
            }
        }

        private async void ExportAsync(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ExportAsync), sender, e);
            } else {
                try {
                    ControlInfo controlInfo = GetControlEntered();
                    if (controlInfo != null && controlInfo.Browser != null) {
                        saveFileDialog.Filter = fileExtensionFilter.GetFilter();
                        saveFileDialog.FilterIndex = fileExtensionFilter.GetFilterIndex();
                        saveFileDialog.Title = Properties.Resources.CaptionExport;
                        saveFileDialog.FileName = new StringBuilder()
                            .Append(Application.ProductName)
                            .Append(Constants.Underscore)
                            .Append(DateTime.Now.ToString(Constants.TimeFormatForFilename))
                            .Append(Constants.Underscore)
                            .Append(ASCII.Convert(controlInfo.BrowserTitle, Encoding.Unicode, ASCII.ConversionOptions.Alphanumeric))
                            .ToString();

                        if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                            statusStripHandler.SetMessage(
                                StatusStripHandler.StatusMessageType.Temporary,
                                Properties.Resources.MessageExporting);

                            using (Bitmap bitmap = new Bitmap(
                                    controlInfo.Browser.Width,
                                    controlInfo.Browser.Height,
                                    PixelFormat.Format32bppArgb)) {

                                Point upperLeftSource = controlInfo.Browser.PointToScreen(Point.Empty);
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
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessageExportFailed);
                }
            }
        }

        private async void ExportWindowAsync(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ExportWindowAsync), sender, e);
            } else {
                try {
                    saveFileDialog.Filter = fileExtensionFilter.GetFilter();
                    saveFileDialog.FilterIndex = fileExtensionFilter.GetFilterIndex();
                    saveFileDialog.Title = Properties.Resources.CaptionExport;
                    saveFileDialog.FileName = new StringBuilder()
                        .Append(Application.ProductName)
                        .Append(Constants.Underscore)
                        .Append(DateTime.Now.ToString(Constants.TimeFormatForFilename))
                        .ToString();

                    if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.Temporary,
                            Properties.Resources.MessageExporting);

                        using (Bitmap bitmap = new Bitmap(
                                splitContainerMain.Width,
                                splitContainerMain.Height + statusStrip.Height * 2,
                                PixelFormat.Format32bppArgb)) {

                            Point upperLeftSource = splitContainerMain.PointToScreen(Point.Empty);
                            upperLeftSource.Y -= statusStrip.Height;
                            await Task.Run(new Action(() => {
                                Thread.Sleep(Constants.ScreenFormCaptureDelay);
                                using (Graphics graphics = Graphics.FromImage(bitmap)) {
                                    graphics.CopyFromScreen(upperLeftSource, Point.Empty, bitmap.Size, CopyPixelOperation.SourceCopy);
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
                    ControlInfo controlInfo = GetControlEntered();
                    if (controlInfo != null && controlInfo.Browser != null) {
                        controlInfo.Browser.Print();
                    }
                    statusStripHandler.SetMessage(
                        StatusStripHandler.StatusMessageType.Temporary,
                        Properties.Resources.MessagePrintingFinished);
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintToPdf(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintToPdf), sender, e);
            } else {
                try {
                    ControlInfo controlInfo = GetControlEntered();

                    if (controlInfo != null && controlInfo.Browser != null) {
                        saveFileDialog.Filter = Properties.Resources.ExtensionFilterPdf;
                        saveFileDialog.FilterIndex = 1;
                        saveFileDialog.Title = Properties.Resources.CaptionPrintToPdf;
                        saveFileDialog.FileName = new StringBuilder()
                            .Append(Application.ProductName)
                            .Append(Constants.Underscore)
                            .Append(DateTime.Now.ToString(Constants.TimeFormatForFilename))
                            .Append(Constants.Underscore)
                            .Append(ASCII.Convert(controlInfo.BrowserTitle, Encoding.Unicode, ASCII.ConversionOptions.Alphanumeric))
                            .ToString();

                        if (saveFileDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                            statusStripHandler.SetMessage(
                                StatusStripHandler.StatusMessageType.Temporary,
                                Properties.Resources.MessagePrinting);
                            controlInfo.Browser.PrintToPdfAsync(saveFileDialog.FileName)
                                .ContinueWith(
                                    new Action<Task>(task => statusStripHandler.SetMessage(
                                        StatusStripHandler.StatusMessageType.Temporary,
                                        Properties.Resources.MessagePrintingFinished)));
                        }
                        return;
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
                    ControlInfo controlInfo = GetControlEntered();
                    if (controlInfo != null && controlInfo.Browser != null) {
                        Thread.Sleep(Constants.ScreenFormCaptureDelay);
                        bitmap = new Bitmap(controlInfo.Browser.Width, controlInfo.Browser.Height, PixelFormat.Format32bppArgb);
                        using (Graphics graphics = Graphics.FromImage(bitmap)) {
                            graphics.CopyFromScreen(
                                controlInfo.Browser.PointToScreen(Point.Empty),
                                Point.Empty,
                                bitmap.Size,
                                CopyPixelOperation.SourceCopy);
                        }
                        dialog = printPreviewDialog;
                        dialog.WindowState = WindowState.Equals(FormWindowState.Minimized) ? FormWindowState.Normal : WindowState;
                        if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                            printDocument.Print();
                        }
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
                    ControlInfo controlInfo = GetControlEntered();
                    if (controlInfo != null && controlInfo.Browser != null) {
                        Thread.Sleep(Constants.ScreenFormCaptureDelay);
                        bitmap = new Bitmap(controlInfo.Browser.Width, controlInfo.Browser.Height, PixelFormat.Format32bppArgb);
                        using (Graphics graphics = Graphics.FromImage(bitmap)) {
                            graphics.CopyFromScreen(
                                controlInfo.Browser.PointToScreen(Point.Empty),
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
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintWindowPreview(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintWindowPreview), sender, e);
            } else {
                try {
                    Thread.Sleep(Constants.ScreenFormCaptureDelay);
                    bitmap = new Bitmap(
                        splitContainerMain.Width,
                        splitContainerMain.Height + statusStrip.Height * 2,
                        PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(bitmap)) {
                        Point upperLeftSource = splitContainerMain.PointToScreen(Point.Empty);
                        upperLeftSource.Y -= statusStrip.Height;
                        graphics.CopyFromScreen(upperLeftSource, Point.Empty, bitmap.Size, CopyPixelOperation.SourceCopy);
                    }
                    dialog = printPreviewDialog;
                    dialog.WindowState = WindowState.Equals(FormWindowState.Minimized) ? FormWindowState.Normal : WindowState;
                    if (printPreviewDialog.ShowDialog(this).Equals(DialogResult.OK)) {
                        printDocument.Print();
                    }
                } catch (Exception exception) {
                    ShowException(exception, Properties.Resources.MessagePrintingFailed);
                }
            }
        }

        private void PrintWindow(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(PrintWindow), sender, e);
            } else {
                try {
                    Thread.Sleep(Constants.ScreenFormCaptureDelay);
                    bitmap = new Bitmap(
                        splitContainerMain.Width,
                        splitContainerMain.Height + statusStrip.Height * 2,
                        PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(bitmap)) {
                        Point upperLeftSource = splitContainerMain.PointToScreen(Point.Empty);
                        upperLeftSource.Y -= statusStrip.Height;
                        graphics.CopyFromScreen(upperLeftSource, Point.Empty, bitmap.Size, CopyPixelOperation.SourceCopy);
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
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB,
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
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary,
                string.IsNullOrEmpty(statusMessage) ? exception.Message : statusMessage);
        }

        private void BeginPrint(object sender, PrintEventArgs e) {
            if (bitmap != null) {
                printAction = e.PrintAction;
                printDocument.OriginAtMargins = settings.PrintSoftMargins;
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary,
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

        private void BrowserUndo(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Undo();
            }
        }

        private void BrowserRedo(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Redo();
            }
        }

        private void BrowserCut(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Cut();
            }
        }

        private void BrowserCopy(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Copy();
            }
        }

        private void BrowserPaste(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Paste();
            }
        }

        private void BrowserDelete(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.Delete();
            }
        }

        private void BrowserSelectAll(object sender, EventArgs e) {
            ControlInfo controlInfo = GetControlEntered();
            if (controlInfo != null && controlInfo.Browser != null) {
                controlInfo.Browser.SelectAll();
            }
        }

        private void FocusTrustDegrees(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(FocusTrustDegrees), sender, e);
            } else {
                Activate();
                settings.RightPaneCollapsed = false;
                SetRightPaneCollapsed(false);
                tabControlRight.SelectedIndex = 0;
                textBox10.Focus();
                textBox10.SelectAll();
            }
        }

        private void Find(object sender, EventArgs e) {
            if (findThread != null && findThread.IsAlive) {
                try {
                    findForm.SafeSelect();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            } else {
                findThread = new Thread(new ThreadStart(Find));
                findThread.Start();
            }
        }

        private void Find() {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                Search search = this.search;
                if (!searching) {
                    search.searchString = null;
                }
                findForm = new MainFindForm(search);
                findForm.AltCtrlShiftEPressed += new EventHandler(ExportWindowAsync);
                findForm.AltCtrlShiftPPressed += new EventHandler(PrintImage);
                findForm.AltF10Pressed += new EventHandler(OpenBetCalculator);
                findForm.AltF11Pressed += new EventHandler(LaunchCalculator);
                findForm.AltF12Pressed += new EventHandler(LaunchNotepad);
                findForm.AltF8Pressed += new EventHandler(LogInAtInitialPage);
                findForm.AltF9Pressed += new EventHandler(ToggleRightPanetWidth);
                findForm.AltF9Pressed += new EventHandler(ToggleRightPanetWidth);
                findForm.AltHomePressed += new EventHandler(GoHome);
                findForm.AltLeftPressed += new EventHandler(GoBack);
                findForm.AltLPressed += new EventHandler(OpenLogViewer);
                findForm.AltRightPressed += new EventHandler(GoForward);
                findForm.CtrlDPressed += new EventHandler(AddBookmark);
                findForm.CtrlEPressed += new EventHandler(ExportAsync);
                findForm.CtrlF5Pressed += new EventHandler(ReloadIgnoreCache);
                findForm.CtrlF5Pressed += new EventHandler(ReloadIgnoreCache);
                findForm.CtrlGPressed += new EventHandler(ShowPreferences);
                findForm.CtrlIPressed += new EventHandler(OpenWebInfo);
                findForm.CtrlIPressed += new EventHandler(OpenWebInfo);
                findForm.CtrlMinusPressed += new EventHandler(ZoomOutFine);
                findForm.CtrlMPressed += new EventHandler(ToggleMuteAudio);
                findForm.CtrlOPressed += new EventHandler(Open);
                findForm.CtrlPlusPressed += new EventHandler(ZoomInFine);
                findForm.CtrlPPressed += new EventHandler(Print);
                findForm.CtrlShiftDelPressed += new EventHandler(ClearBrowserCacheInclUserData);
                findForm.CtrlShiftEPressed += new EventHandler(ExportWindowAsync);
                findForm.CtrlShiftMinusPressed += new EventHandler(ZoomOutCoarse);
                findForm.CtrlShiftNPressed += new EventHandler(OpenEncoderDecoder);
                findForm.CtrlShiftPlusPressed += new EventHandler(ZoomInCoarse);
                findForm.CtrlShiftPPressed += new EventHandler(PrintToPdf);
                findForm.CtrlTPressed += new EventHandler(NewTabBetCalculator);
                findForm.CtrlUPressed += new EventHandler(ViewSource);
                findForm.CtrlZeroPressed += new EventHandler(ActualSize);
                findForm.DownPressed += new EventHandler(OnDownPressed);
                findForm.EndPressed += new EventHandler(OnEndPressed);
                findForm.F11Pressed += new EventHandler(TurnOffMonitors);
                findForm.F12Pressed += new EventHandler(OpenDevTools);
                findForm.F2Pressed += new EventHandler(FocusTrustDegrees);
                findForm.F4Pressed += new EventHandler(UnloadPage);
                findForm.F5Pressed += new EventHandler(Reload);
                findForm.F8Pressed += new EventHandler(LogIn);
                findForm.F9Pressed += new EventHandler(ToggleRightPane);
                findForm.F9Pressed += new EventHandler(ToggleRightPane);
                findForm.Find += new EventHandler<SearchEventArgs>(OnFind);
                findForm.FormClosed += new FormClosedEventHandler(OnFindFormClosed);
                findForm.HelpRequested += new HelpEventHandler(OpenHelp);
                findForm.HomePressed += new EventHandler(OnHomePressed);
                findForm.PageDownPressed += new EventHandler(OnPageDownPressed);
                findForm.PageUpPressed += new EventHandler(OnPageUpPressed);
                findForm.UpPressed += new EventHandler(OnUpPressed);
                findForm.ShowDialog();
            }
        }

        private void OnFindFormClosed(object sender, FormClosedEventArgs e) {
            webInfoHandler.StopFinding();
            statusStripHandler.ClearSearchResult();
        }

        private void OnHomePressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.Home);

        private void OnEndPressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.End);

        private void OnPageUpPressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.PageUp);

        private void OnPageDownPressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.PageDown);

        private void OnUpPressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.Up);

        private void OnDownPressed(object sender, EventArgs e) => webInfoHandler.SendKey(Keys.Down);

        protected override void OnPaintBackground(PaintEventArgs pevent) { }

        private void OnFind(object sender, SearchEventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                if (webInfoHandler.Current.Browser.InvokeRequired) {
                    webInfoHandler.Current.Browser.Invoke(new EventHandler<SearchEventArgs>(OnFind), sender, e);
                } else {
                    if (string.IsNullOrEmpty(e.Search.searchString)) {
                        webInfoHandler.Current.StopFinding(true);
                        statusStripHandler.ClearSearchResult();
                    } else {
                        search = e.Search;
                        webInfoHandler.Current.Browser.Find(search.searchString, !search.backward, search.caseSensitive, true);
                    }
                    persistWindowState.SetVisible(e.Handle);
                }
            }
        }

        private void FindNext(object sender, EventArgs e) {
            if (settings.F3MainFormFocusesFindForm && findThread != null && findThread.IsAlive) {
                try {
                    findForm.SafeSelect();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            } else if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && !string.IsNullOrEmpty(search.searchString)) {

                searching = true;
                webInfoHandler.Current.Browser.Find(search.searchString, !search.backward, search.caseSensitive, true);
            }
        }

        private void FindPrevious(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null && !string.IsNullOrEmpty(search.searchString)) {
                searching = true;
                webInfoHandler.Current.Browser.Find(search.searchString, search.backward, search.caseSensitive, true);
            }
        }

        private void OnFind(object sender, FindEventArgs e) {
            if (e.EmptyResult) {
                statusStripHandler.ClearSearchResult();
                return;
            }
            statusStripHandler.SetSearchResult(e.Count, e.ActiveMatchOrdinal);
            Point point = webInfoHandler.Current.Browser.PointToScreen(new Point(e.SelectionRect.X, e.SelectionRect.Y));
            Size size = new Size(e.SelectionRect.Width, e.SelectionRect.Height);
            Rectangle rectangle = new Rectangle(point, size);
            if (findForm != null && findForm.Visible) {
                findForm.SafeLocation(GetNewLocation(rectangle, new Rectangle(findForm.Location, findForm.Size)));
            }
            if (settings.OutlineSearchResults) {
                OutlineSearchResultAsync(rectangle);
            }
        }

        private void ToggleRightPane(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ToggleRightPane), sender, e);
            } else {
                SetRightPaneCollapsed(true);
            }
        }

        private void ToggleRightPanetWidth(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ToggleRightPanetWidth), sender, e);
            } else {
                SetRightPaneWidth(true);
            }
        }

        private void Reload(object sender, EventArgs e) {
            if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri)
                    && webInfoHandler.Current.CanReload) {

                webInfoHandler.Current.Browser.GetBrowser().Reload(false);
            }
        }

        private void ReloadIgnoreCache(object sender, EventArgs e) {
            if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri)
                    && webInfoHandler.Current.CanReload) {

                webInfoHandler.Current.Browser.GetBrowser().Reload(true);
            }
        }

        private void ViewSource(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.Browser.ViewSource();
            }
        }

        private void ToggleMuteAudio(object sender, EventArgs e) {
            if (settings.AudioEnabled && webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.SetAudioMuted(!webInfoHandler.Current.IsAudioMuted);
                Menu.MenuItems[2].MenuItems[19].Text = (!settings.AudioEnabled || webInfoHandler.Current.IsAudioMuted
                    ? Properties.Resources.MenuItemUnmuteAudio
                    : Properties.Resources.MenuItemMuteAudio) + Constants.ShortcutCtrlM;
                statusStripHandler.SetMuted(webInfoHandler.Current.IsAudioMuted);
            }
        }

        private void ToggleShowChat(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.HasChat) {
                if (webInfoHandler.Current.IsChatHidden) {
                    webInfoHandler.Current.ShowChat();
                } else {
                    webInfoHandler.Current.HideChat();
                }
                Menu.MenuItems[2].MenuItems[20].Text = webInfoHandler.Current.IsChatHidden
                    ? Properties.Resources.MenuItemShowChat
                    : Properties.Resources.MenuItemHideChat;
            }
        }

        private void LogIn(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                AdjustSize(new Size(Constants.BrowserMinWidth, Constants.BrowserMinHeight));
                webInfoHandler.Current.LogInAsync(false);
            }
        }

        private void LogInAtInitialPage(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                AdjustSize(new Size(Constants.BrowserMinWidth, Constants.BrowserMinHeight));
                webInfoHandler.Current.LogInAsync(true);
            }
        }

        private void AddBookmark(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(AddBookmark), sender, e);
            } else if (webInfoHandler.Current != null) {
                bookmarkManager.Add(webInfoHandler.Current.BrowserAddress, webInfoHandler.Current.BrowserTitle);
            }
        }

        private void RemoveBookmark(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(RemoveBookmark), sender, e);
            } else {
                dialog = new MessageForm(this, Properties.Resources.MessageRemoveBookmark, null, MessageForm.Buttons.YesNo,
                    MessageForm.BoxIcon.Question);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);
                if (dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                    bookmarkManager.Remove(webInfoHandler.Current.BrowserAddress);
                }
            }
        }

        private void AutoAdjustRightPaneWidth(object sender, EventArgs e) {
            settings.AutoAdjustRightPaneWidth = !settings.AutoAdjustRightPaneWidth;
            ((MenuItem)sender).Checked = settings.AutoAdjustRightPaneWidth;
            if (settings.AutoAdjustRightPaneWidth) {
                AdjustRightPaneWidth();
            }
        }

        private void AutoLogInAfterInitialLoad(object sender, EventArgs e) {
            settings.AutoLogInAfterInitialLoad = !settings.AutoLogInAfterInitialLoad;
            ((MenuItem)sender).Checked = settings.AutoLogInAfterInitialLoad;
        }

        private void TryToKeepUserLoggedIn(object sender, EventArgs e) {
            settings.TryToKeepUserLoggedIn = !settings.TryToKeepUserLoggedIn;
            ((MenuItem)sender).Checked = settings.TryToKeepUserLoggedIn;
        }

        private void ToggleBell(object sender, EventArgs e) {
            settings.EnableBell = !settings.EnableBell;
            ((MenuItem)sender).Checked = settings.EnableBell;
        }

        private void ClearBrowserCache(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ClearBrowserCache), sender, e);
            } else {
                StringBuilder message = new StringBuilder()
                    .Append(Properties.Resources.MessageClearBrowserCache)
                    .Append(Environment.NewLine)
                    .Append(Properties.Resources.MessageApplicationWillBeRestarted);
                dialog = new MessageForm(this, message.ToString(), null, MessageForm.Buttons.OKCancel, MessageForm.BoxIcon.Information);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);

                if (dialog.ShowDialog(this).Equals(DialogResult.OK)) {
                    browserCacheManager.Set(BrowserCacheManager.ClearSet.BrowserCacheOnly);
                    RestartApplication();
                }
            }
        }

        private void ClearBrowserCacheInclUserData(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ClearBrowserCacheInclUserData), sender, e);
            } else {
                StringBuilder message = new StringBuilder()
                    .Append(Properties.Resources.MessageClearBrowserCache)
                    .Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append(Properties.Resources.MessageClearBrowserCacheInclUserData)
                    .Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append(Properties.Resources.MessageApplicationWillBeRestarted);
                dialog = new MessageForm(this, message.ToString(), null, MessageForm.Buttons.OKCancel, MessageForm.BoxIcon.Information);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);

                if (dialog.ShowDialog(this).Equals(DialogResult.OK)) {
                    browserCacheManager.Set(BrowserCacheManager.ClearSet.BrowserCacheIncludingUserData);
                    RestartApplication();
                }
            }
        }

        private void ApplicationReset(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ApplicationReset), sender, e);
            } else {
                StringBuilder message = new StringBuilder()
                    .Append(Properties.Resources.MessageResetWarningLine1)
                    .Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append(Properties.Resources.MessageResetWarningLine2)
                    .Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append(Properties.Resources.MessageApplicationWillBeRestarted);
                dialog = new MessageForm(this, message.ToString(), null, MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Warning,
                    MessageForm.DefaultButton.Button2);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);

                if (dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                    if (findForm != null && findForm.Visible) {
                        findForm.SafeClose();
                    }
                    browserCacheManager.Set(BrowserCacheManager.ClearSet.BrowserCacheIncludingUserData);
                    new SearchHandler(Constants.BrowserSearchFileName).Delete();
                    new SearchHandler(Constants.ConfigSearchFileName).Delete();
                    new SearchHandler(Constants.LogSearchFileName).Delete();
                    new TypedUrlsHandler().Delete();
                    bookmarkManager.Delete();
                    DeleteRightPaneData();
                    settings.Clear();
                    suppressSaveData = true;
                    RestartApplication();
                }
            }
        }

        private void RestartApplication() {
            GC.Collect(2, GCCollectionMode.Forced, true);
            if (Program.IsDebugging) {
                close = true;
                Close();
            } else {
                restart = true;
                Close();
            }
        }

        private void ShowPreferences(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ShowPreferences), sender, e);
            } else {
                PreferencesForm preferencesForm = new PreferencesForm();
                preferencesForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
                preferencesForm.HelpRequested += new HelpEventHandler(OpenHelp);
                preferencesForm.Settings = settings;
                dialog = preferencesForm;

                if (preferencesForm.ShowDialog(this).Equals(DialogResult.OK)) {
                    webInfoHandler.SetPingTimer();
                    bookmarkManager.Populate();
                    SetTabControlsAppearance();
                    UpdateTrustDegrees();
                    statusStripHandler.SetMessage(
                        StatusStripHandler.StatusMessageType.Temporary,
                        Properties.Resources.MessagePreferencesSaved);
                    Menu.MenuItems[4].MenuItems[0].Checked = settings.AutoAdjustRightPaneWidth;
                    Menu.MenuItems[4].MenuItems[1].Checked = settings.AutoLogInAfterInitialLoad;
                    if (settings.AutoAdjustRightPaneWidth) {
                        AdjustRightPaneWidth();
                    }
                    if (preferencesForm.RestartRequired) {
                        GC.Collect(2, GCCollectionMode.Forced, true);
                        if (Program.IsDebugging) {
                            close = true;
                            Close();
                        } else {
                            restart = true;
                            Close();
                        }
                    }
                } else {
                    settings.ActivePreferencesPanel = preferencesForm.TabControl.SelectedIndex;
                }
            }
        }

        private void SetTabControlsAppearance() {
            if (InvokeRequired) {
                Invoke(new SetTabControlCallback(SetTabControlsAppearance));
            } else {
                tabControlLeft.Appearance = settings.TabAppearance;
                tabControlRight.Appearance = settings.TabAppearance;
                tabControlLeft.Font = new Font(SystemFonts.CaptionFont, settings.TabsBoldFont ? FontStyle.Bold : FontStyle.Regular);
                tabControlRight.Font = new Font(SystemFonts.CaptionFont, settings.TabsBoldFont ? FontStyle.Bold : FontStyle.Regular);
                tabControlLeft.Invalidate();
                tabControlRight.Invalidate();
            }
        }

        private void SetRightPaneCollapsed(bool toggle) {
            if (toggle) {
                settings.RightPaneCollapsed = !settings.RightPaneCollapsed;
            }
            splitContainerMain.Panel2Collapsed = settings.RightPaneCollapsed;
        }

        private void SetRightPaneWidth(bool toggle) {
            if (toggle) {
                settings.RightPaneWidth = !settings.RightPaneWidth;
            }
            if (splitContainerMain.Width - splitContainerMain.Panel2MinSize - splitContainerMain.Panel1MinSize < 0) {
                return;
            }
            int splitterDistance;
            if (settings.RightPaneWidth) {
                splitterDistance = splitContainerMain.Width - splitContainerMain.Panel2MinSize;
            } else {
                splitterDistance = (int)(splitContainerMain.Width /
                    ((float)Constants.SplitContainerMaxWidth /
                        (Constants.SplitContainerMaxWidth - Constants.RightPaneDefaultWidth)));
            }
            splitContainerMain.SplitterDistance = splitterDistance < splitContainerMain.Panel1MinSize
                ? splitContainerMain.Panel1MinSize
                : splitterDistance;
        }

        private void OpenPageInDefaultBrowser(object sender, EventArgs e) {
            string currentAddress = webInfoHandler.GetCurrentAddress();
            if (!string.IsNullOrEmpty(currentAddress)) {
                try {
                    Process.Start(StaticMethods.EscapeArgument(currentAddress));
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void OpenPageInGoogleChrome(object sender, EventArgs e) {
            string currentAddress = webInfoHandler.GetCurrentAddress();
            if (!string.IsNullOrEmpty(currentAddress)) {
                try {
                    extBrowsHandler.Open(ExtBrowsHandler.Browser.GoogleChrome, currentAddress);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void OpenPageInMozillaFirefox(object sender, EventArgs e) {
            string currentAddress = webInfoHandler.GetCurrentAddress();
            if (!string.IsNullOrEmpty(currentAddress)) {
                try {
                    extBrowsHandler.Open(ExtBrowsHandler.Browser.Firefox, currentAddress);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void AnalyzeUrl(object sender, EventArgs e) {
            string currentAddress = webInfoHandler.GetCurrentAddress();
            if (!string.IsNullOrEmpty(currentAddress)) {
                try {
                    StringBuilder arguments = new StringBuilder()
                        .Append(Constants.CommandLineSwitchWD)
                        .Append(Constants.Space)
                        .Append(StaticMethods.EscapeArgument(currentAddress));
                    Process.Start(Application.ExecutablePath, arguments.ToString());
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void CopyCurrentUrl(object sender, EventArgs e) {
            string currentAddress = webInfoHandler.GetCurrentAddress();
            if (!string.IsNullOrEmpty(currentAddress)) {
                try {
                    Clipboard.SetText(currentAddress);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void CopyCurrentTitle(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(webInfoHandler.Current.BrowserTitle)) {
                try {
                    Clipboard.SetText(webInfoHandler.Current.BrowserTitle);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB, exception.Message);
                }
            }
        }

        private void UnloadPage(object sender, EventArgs e) {
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                webInfoHandler.Current.Browser.Load(Constants.BlankPageUri);
            }
        }

        private void NewTabBetCalculator(object sender, EventArgs e) => calculatorHandler.AddNewTab();

        private void OpenBetCalculator(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OpenBetCalculator), sender, e);
            } else {
                try {
                    Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWC);
                } catch (Exception exception) {
                    ShowException(exception);
                }
            }
        }

        private void OpenDevTools(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(webInfoHandler.GetCurrentAddress())) {
                webInfoHandler.Current.Browser.ShowDevTools();
            }
        }

        private void InspectDomElement() {
            if (form != null && form.Visible) {
                form.Activate();
                return;
            }
            Rectangle rectangle = webInfoHandler.Current.Browser.RectangleToScreen(webInfoHandler.Current.Browser.ClientRectangle);
            form = new Form() {
                FormBorderStyle = FormBorderStyle.None,
                Location = rectangle.Location,
                Opacity = settings.InspectOverlayOpacity / 100d,
                ShowInTaskbar = false,
                Size = rectangle.Size,
                StartPosition = FormStartPosition.Manual
            };
            form.FormClosed += new FormClosedEventHandler((sender, e) => crosshairPen.Dispose());
            form.KeyDown += new KeyEventHandler((sender, e) => form.Close());
            form.MouseDown += new MouseEventHandler((sender, e) => {
                if (e.Button.Equals(MouseButtons.Left)) {
                    location = e.Location;
                }
            });
            form.MouseLeave += new EventHandler((sender, e) => form.Close());
            form.MouseMove += new MouseEventHandler((sender, e) => {
                graphics.Clear(settings.OverlayBackgroundColor);
                graphics.DrawLine(crosshairPen, e.X, 0, e.X, form.Height);
                graphics.DrawLine(crosshairPen, 0, e.Y, form.Width, e.Y);
            });
            form.MouseUp += new MouseEventHandler((sender, e) => {
                if (e.Button.Equals(MouseButtons.Left)
                        && !string.IsNullOrEmpty(webInfoHandler.GetCurrentAddress())
                        && Math.Abs(e.X - location.X) < 2
                        && Math.Abs(e.Y - location.Y) < 2) {

                    form.Close();
                    webInfoHandler.Current.Browser.ShowDevTools(null, e.X, e.Y);
                }
            });
            form.Paint += new PaintEventHandler((sender, e) => e.Graphics.DrawImageUnscaled(image, Point.Empty));
            crosshairPen = new Pen(settings.OverlayCrosshairColor, 1);
            graphics = form.CreateGraphics();
            image = new Bitmap(form.Width, form.Height, graphics);
            form.Show();
        }

        private void OpenWebInfo(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(webInfoHandler.GetCurrentAddress())) {
                webInfoHandler.Current.ShowInfo();
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
            countDownForm = new CountDownForm();
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

        private void LaunchTelegram(object sender, EventArgs e) {
            try {
                Process.Start(Path.Combine(settings.TelegramAppDirectory, Constants.TelegramDesktopFileName));
            } catch (Exception exception) {
                ShowException(exception);
            }
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

        private void CheckUpdates(object sender, EventArgs e) => updateChecker.Check(UpdateChecker.CheckType.User);

        private void ShowAbout(object sender, EventArgs e) {
            dialog = new AboutForm();
            dialog.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog.ShowDialog(this);
        }

        private void OnFormLoad(object sender, EventArgs e) {
            SetRightPaneCollapsed(false);
            SetRightPaneWidth(false);
            if (settings.AutoAdjustRightPaneWidth) {
                AdjustRightPaneWidth();
            }
            tabControlLeft.SelectedIndexChanged += new EventHandler((s, t) =>
                webInfoHandler.SetCurrent(tabControlLeft.SelectedIndex));
            tabControlRight.SelectedIndexChanged += new EventHandler((s, t) => {
                if (settings.AutoAdjustRightPaneWidth) {
                    AdjustRightPaneWidth();
                }
            });
            webInfoHandler.SetCurrent(tabControlLeft.SelectedIndex);
            settings.Save();
            TipsEnableControls(sender, e);
            ServicesEnableControls(sender, e);
            SendKeys.Send(Constants.SendKeysTab);
            SendKeys.Send(Constants.SendKeysAlt);
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
                if (dialog != null && dialog.Visible) {
                    e.Cancel = true;
                    return;
                }
                if (countDownForm != null) {
                    countDownForm.HelpRequested -= new HelpEventHandler(OpenHelp);
                    if (countDownForm.Visible) {
                        countDownForm.SafeClose();
                    }
                }

                webInfoHandler.Suspend();
                Thread.Sleep(250);
                SaveRightPaneData();

                if (!close && !restart && settings.DisplayPromptBeforeClosing && e.CloseReason.Equals(CloseReason.UserClosing)) {
                    dialog = new MessageForm(this, Properties.Resources.MessageQuestionBeforeClosing,
                        Properties.Resources.CaptionQuestion, MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Question);
                    dialog.HelpRequested += new HelpEventHandler(OpenHelp);
                    if (!dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                        webInfoHandler.Resume();
                        e.Cancel = true;
                        return;
                    }
                }

                if (bitmap != null) {
                    bitmap.Dispose();
                }

                if (findForm != null) {
                    findForm.AltCtrlShiftEPressed -= new EventHandler(ExportWindowAsync);
                    findForm.AltCtrlShiftPPressed -= new EventHandler(PrintImage);
                    findForm.AltF10Pressed -= new EventHandler(OpenBetCalculator);
                    findForm.AltF11Pressed -= new EventHandler(LaunchCalculator);
                    findForm.AltF12Pressed -= new EventHandler(LaunchNotepad);
                    findForm.AltF8Pressed -= new EventHandler(LogInAtInitialPage);
                    findForm.AltF9Pressed -= new EventHandler(ToggleRightPanetWidth);
                    findForm.AltF9Pressed -= new EventHandler(ToggleRightPanetWidth);
                    findForm.AltHomePressed -= new EventHandler(GoHome);
                    findForm.AltLeftPressed -= new EventHandler(GoBack);
                    findForm.AltLPressed -= new EventHandler(OpenLogViewer);
                    findForm.AltRightPressed -= new EventHandler(GoForward);
                    findForm.CtrlDPressed -= new EventHandler(AddBookmark);
                    findForm.CtrlEPressed -= new EventHandler(ExportAsync);
                    findForm.CtrlF5Pressed -= new EventHandler(ReloadIgnoreCache);
                    findForm.CtrlF5Pressed -= new EventHandler(ReloadIgnoreCache);
                    findForm.CtrlGPressed -= new EventHandler(ShowPreferences);
                    findForm.CtrlIPressed -= new EventHandler(OpenWebInfo);
                    findForm.CtrlIPressed -= new EventHandler(OpenWebInfo);
                    findForm.CtrlMinusPressed -= new EventHandler(ZoomOutFine);
                    findForm.CtrlMPressed -= new EventHandler(ToggleMuteAudio);
                    findForm.CtrlOPressed -= new EventHandler(Open);
                    findForm.CtrlPlusPressed -= new EventHandler(ZoomInFine);
                    findForm.CtrlPPressed -= new EventHandler(Print);
                    findForm.CtrlShiftDelPressed -= new EventHandler(ClearBrowserCacheInclUserData);
                    findForm.CtrlShiftEPressed -= new EventHandler(ExportWindowAsync);
                    findForm.CtrlShiftMinusPressed -= new EventHandler(ZoomOutCoarse);
                    findForm.CtrlShiftNPressed -= new EventHandler(OpenEncoderDecoder);
                    findForm.CtrlShiftPlusPressed -= new EventHandler(ZoomInCoarse);
                    findForm.CtrlShiftPPressed -= new EventHandler(PrintToPdf);
                    findForm.CtrlTPressed -= new EventHandler(NewTabBetCalculator);
                    findForm.CtrlUPressed -= new EventHandler(ViewSource);
                    findForm.CtrlZeroPressed -= new EventHandler(ActualSize);
                    findForm.DownPressed -= new EventHandler(OnDownPressed);
                    findForm.EndPressed -= new EventHandler(OnEndPressed);
                    findForm.F11Pressed -= new EventHandler(TurnOffMonitors);
                    findForm.F12Pressed -= new EventHandler(OpenDevTools);
                    findForm.F2Pressed -= new EventHandler(FocusTrustDegrees);
                    findForm.F4Pressed -= new EventHandler(UnloadPage);
                    findForm.F5Pressed -= new EventHandler(Reload);
                    findForm.F8Pressed -= new EventHandler(LogIn);
                    findForm.F9Pressed -= new EventHandler(ToggleRightPane);
                    findForm.F9Pressed -= new EventHandler(ToggleRightPane);
                    findForm.Find -= new EventHandler<SearchEventArgs>(OnFind);
                    findForm.FormClosed -= new FormClosedEventHandler(OnFindFormClosed);
                    findForm.HelpRequested -= new HelpEventHandler(OpenHelp);
                    findForm.HomePressed -= new EventHandler(OnHomePressed);
                    findForm.PageDownPressed -= new EventHandler(OnPageDownPressed);
                    findForm.PageUpPressed -= new EventHandler(OnPageUpPressed);
                    findForm.UpPressed -= new EventHandler(OnUpPressed);
                    if (findForm.Visible) {
                        findForm.SafeClose();
                    }
                }

                if (suppressSaveData) {
                    persistWindowState.SavingOptions = PersistWindowState.PersistWindowStateSavingOptions.None;
                } else {
                    settings.ActivePanelLeft = tabControlLeft.SelectedIndex;
                    settings.ActivePanelRight = tabControlRight.SelectedIndex;
                    settings.Save();
                }
                settings.Dispose();
                telephoneBell.Dispose();
                webInfoHandler.Dispose();
                browserCacheManager.Dispose();
                calculatorHandler.Dispose();
                shortcutManager.Dispose();
                statusStripHandler.Dispose();
                updateChecker.Dispose();
                textBoxClicksTimer.Dispose();
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e) {
            if (restart) {
                Application.Restart();
            }
        }

        private void OnLogInStarted(object sender, StartedEventArgs e) {
            if (dialog != null && dialog is ProgressBarForm && dialog.Visible) {
                ProgressBarForm progressBarForm = (ProgressBarForm)dialog;
                progressBarForm.SetMessage(Properties.Resources.MessagePleaseWait);
                progressBarForm.SetMaximum(e.ItemsTotal);
                progressBarForm.Cursor = Cursors.AppStarting;
            } else {
                ProgressBarForm progressBarForm = new ProgressBarFormEx();
                progressBarForm.ProgressBarMarquee = true;
                progressBarForm.SetMessage(Properties.Resources.MessagePleaseWait);
                progressBarForm.SetMaximum(e.ItemsTotal);
                dialog = progressBarForm;
                webInfoHandler.Current.Dialog = progressBarForm;
                progressBarForm.FormClosed += new FormClosedEventHandler(OnProgressBarFormClosed);
                progressBarForm.HelpRequested += new HelpEventHandler(OpenHelp);
                progressBarForm.Cursor = Cursors.AppStarting;
                progressBarForm.Show(this);
            }
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, Properties.Resources.MessagePleaseWait);
            statusStripHandler.SetMaximum(e.ItemsTotal);
            Cursor = Cursors.WaitCursor;
            splitContainerMain.Enabled = false;
            statusStripHandler.EnableContextMenu = false;
            if (findForm != null && findForm.Visible) {
                findForm.SafeDisable();
            }
        }

        private void OnLogInProgress(object sender, ProgressEventArgs e) {
            if (dialog != null && dialog is ProgressBarForm && dialog.Visible) {
                ProgressBarForm progressBarForm = (ProgressBarForm)dialog;
                progressBarForm.SetMaximum(e.ItemsTotal);
                progressBarForm.SetMessage(e.Message);
                progressBarForm.SetValue(e.CurrentItem);
            }
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.Message);
            statusStripHandler.SetMaximum(e.ItemsTotal);
            statusStripHandler.SetValue(e.CurrentItem);
        }

        private void OnLogInFinished(object sender, FinishedEventArgs e) {
            if (dialog != null && dialog is ProgressBarForm && dialog.Visible) {
                ((ProgressBarForm)dialog).SetFinished(e.Message);
            }
            GC.Collect(2, GCCollectionMode.Forced, true);
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.Message);
            statusStripHandler.SetFinished();
            Cursor = Cursors.Default;
            splitContainerMain.Enabled = true;
            statusStripHandler.EnableContextMenu = true;
            if (findForm != null && findForm.Visible) {
                findForm.SafeEnable();
            }
        }

        private void OnLogInError(object sender, ErrorEventArgs e) {
            if (dialog != null && dialog is ProgressBarForm && dialog.Visible) {
                ((ProgressBarForm)dialog).SetFinished(e.ErrorMessage);
            }
            GC.Collect(2, GCCollectionMode.Forced, true);
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.ErrorMessage);
            statusStripHandler.SetFinished();
            Cursor = Cursors.Default;
            splitContainerMain.Enabled = true;
            statusStripHandler.EnableContextMenu = true;
            if (findForm != null && findForm.Visible) {
                findForm.SafeEnable();
            }
        }

        private void OnLogInCanceled(object sender, CanceledEventArgs e) {
            if (dialog != null && dialog is ProgressBarForm && dialog.Visible) {
                ((ProgressBarForm)dialog).SetFinished(e.CancelMessage);
            }
            GC.Collect(2, GCCollectionMode.Forced, true);
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.CancelMessage);
            statusStripHandler.SetFinished();
            Cursor = Cursors.Default;
            splitContainerMain.Enabled = true;
            statusStripHandler.EnableContextMenu = true;
            if (findForm != null && findForm.Visible) {
                findForm.SafeEnable();
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

        private void OnProgressBarFormClosed(object sender, FormClosedEventArgs e) {
            webInfoHandler.Current.CancelLogIn();
            SendKeys.Send(Constants.SendKeysTab);
            Cursor = Cursors.Default;
            splitContainerMain.Enabled = true;
            statusStripHandler.EnableContextMenu = true;
            if (findForm != null && findForm.Visible) {
                findForm.SafeEnable();
            }
        }

        private void DrawTabPageHeader(object sender, DrawItemEventArgs e) {
            TabControl tabControl = (TabControl)sender;
            Color color = e.State.Equals(DrawItemState.Selected) ? Color.White : SystemColors.Control;
            if (settings.TabsBackgroundColor) {
                if (tabControl.Name.EndsWith(Constants.TabControlLeftNameEnd, StringComparison.OrdinalIgnoreCase)) {
                    if (webInfoHandler.WebInfos[e.Index].IsBookmaker) {
                        color = e.State.Equals(DrawItemState.Selected)
                            ? settings.BookmakerSelectedColor
                            : settings.BookmakerDefaultColor;
                    } else if (webInfoHandler.WebInfos[e.Index].IsService) {
                        color = e.State.Equals(DrawItemState.Selected)
                            ? settings.SportInfo2SelectedColor
                            : settings.SportInfo2DefaultColor;
                    } else {
                        color = e.State.Equals(DrawItemState.Selected)
                            ? settings.SportInfo1SelectedColor
                            : settings.SportInfo1DefaultColor;
                    }
                } else if (tabControl.Name.EndsWith(Constants.TabControlRightNameEnd, StringComparison.OrdinalIgnoreCase)) {
                    if (e.Index > 1) {
                        color = e.State.Equals(DrawItemState.Selected)
                            ? settings.CalculatorSelectedColor
                            : settings.CalculatorDefaultColor;
                    } else {
                        color = e.State.Equals(DrawItemState.Selected)
                            ? settings.DashboardSelectedColor
                            : settings.DashboardDefaultColor;
                    }
                }
            }
            using (Brush brush = new SolidBrush(color)) {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            SizeF sizeF = e.Graphics.MeasureString(tabControl.TabPages[e.Index].Text, e.Font);
            PointF pointF = new PointF(
                e.Bounds.Left + (e.Bounds.Width - sizeF.Width) / 2,
                e.Bounds.Top + (e.Bounds.Height - sizeF.Height) / 2);
            if (tabControl.Appearance.Equals(TabAppearance.Normal)) {
                pointF.Y = pointF.Y + 1;
            } else if (e.State.Equals(DrawItemState.Selected)) {
                pointF.X = pointF.X + 1;
                pointF.Y = pointF.Y + 1;
            }
            e.Graphics.DrawString(tabControl.TabPages[e.Index].Text, e.Font, Brushes.Black, pointF);
            e.DrawFocusRectangle();
        }

        private void OnCurrentSet(object sender, FocusEventArgs e) {
            if (!tabControlLeft.SelectedIndex.Equals(e.Index)) {
                tabControlLeft.SelectedIndex = e.Index;
            }
            if (webInfoHandler.Current.IsLoading) {
                statusStripHandler.SetMaximum(0);
            } else {
                statusStripHandler.ResetProgressBar();
            }
            SetText(e.WebInfo.BrowserTitle);
            statusStripHandler.SetMessage();
            SetConsoleMessage(e.WebInfo.ConsoleLine, e.WebInfo.ConsoleSource, e.WebInfo.ConsoleMessage);
            SetLoadErrorMessage(e.WebInfo.ErrorCode, e.WebInfo.ErrorText, e.WebInfo.FailedUrl);
            statusStripHandler.SetUrl(e.WebInfo.BrowserAddress);
            statusStripHandler.SetZoomLevel(e.WebInfo.ZoomLevel);
            controlInfo = new ControlInfo(e.WebInfo.Browser, e.WebInfo.BrowserTitle);
            statusStripHandler.SetMuted(!settings.AudioEnabled || e.WebInfo.IsAudioMuted);
            Menu.MenuItems[2].MenuItems[19].Text = (!settings.AudioEnabled || e.WebInfo.IsAudioMuted
                ? Properties.Resources.MenuItemUnmuteAudio
                : Properties.Resources.MenuItemMuteAudio) + Constants.ShortcutCtrlM;
            Menu.MenuItems[2].MenuItems[20].Text = e.WebInfo.IsChatHidden
                ? Properties.Resources.MenuItemShowChat
                : Properties.Resources.MenuItemHideChat;
        }

        private async void UpdateViewMenuItemsAsync(object sender, EventArgs e) {
            Menu.MenuItems[2].MenuItems[0].Text = (settings.RightPaneCollapsed
                    ? Properties.Resources.MenuItemShowRightPane
                    : Properties.Resources.MenuItemHideRightPane) + Constants.ShortcutF9;
            Menu.MenuItems[2].MenuItems[1].Text = (settings.RightPaneWidth
                    ? Properties.Resources.MenuItemSetRightPaneDefaultWidth
                    : Properties.Resources.MenuItemSetRightPaneMinimumWidth) + Constants.ShortcutAltF9;
            Menu.MenuItems[2].MenuItems[19].Text = (!settings.AudioEnabled
                || webInfoHandler.Current != null && webInfoHandler.Current.IsAudioMuted
                    ? Properties.Resources.MenuItemUnmuteAudio
                    : Properties.Resources.MenuItemMuteAudio) + Constants.ShortcutCtrlM;
            Menu.MenuItems[2].MenuItems[20].Text = webInfoHandler.Current == null || webInfoHandler.Current.IsChatHidden
                ? Properties.Resources.MenuItemShowChat
                : Properties.Resources.MenuItemHideChat;
            if (webInfoHandler.Current != null && webInfoHandler.Current.Browser != null) {
                Menu.MenuItems[2].MenuItems[3].Enabled = true;
                Menu.MenuItems[2].MenuItems[4].Enabled = true;
                Menu.MenuItems[2].MenuItems[5].Enabled = true;
                Menu.MenuItems[2].MenuItems[6].Enabled = true;
                Menu.MenuItems[2].MenuItems[7].Enabled = true;
            } else {
                Menu.MenuItems[2].MenuItems[3].Enabled = false;
                Menu.MenuItems[2].MenuItems[4].Enabled = false;
                Menu.MenuItems[2].MenuItems[5].Enabled = false;
                Menu.MenuItems[2].MenuItems[6].Enabled = false;
                Menu.MenuItems[2].MenuItems[7].Enabled = false;
                Menu.MenuItems[2].MenuItems[9].Enabled = false;
                Menu.MenuItems[2].MenuItems[10].Enabled = false;
                Menu.MenuItems[2].MenuItems[11].Enabled = false;
                Menu.MenuItems[2].MenuItems[12].Enabled = false;
                Menu.MenuItems[2].MenuItems[13].Enabled = false;
                Menu.MenuItems[2].MenuItems[14].Enabled = false;
                Menu.MenuItems[2].MenuItems[15].Enabled = false;
                Menu.MenuItems[2].MenuItems[17].Enabled = false;
                Menu.MenuItems[2].MenuItems[19].Enabled = false;
                Menu.MenuItems[2].MenuItems[20].Enabled = false;
                Menu.MenuItems[2].MenuItems[22].Enabled = false;
                Menu.MenuItems[2].MenuItems[23].Enabled = false;
                return;
            }
            await Task.Run(new Action(() => {
                double zoomLevel = webInfoHandler.Current.Browser.GetZoomLevelAsync().GetAwaiter().GetResult();
                bool isLoggedIn = webInfoHandler.Current.IsLoggedIn();
                Invoke(new EventHandler((s, t) => {
                    Menu.MenuItems[2].MenuItems[5].Enabled = !zoomLevel.Equals(0);
                    Menu.MenuItems[2].MenuItems[9].Enabled = webInfoHandler.Current.CanGoBack;
                    Menu.MenuItems[2].MenuItems[10].Enabled = webInfoHandler.Current.CanGoForward;
                    Menu.MenuItems[2].MenuItems[11].Enabled = !webInfoHandler.Current.Browser.Address.Equals(webInfoHandler.Current.Url);
                    Menu.MenuItems[2].MenuItems[12].Enabled = webInfoHandler.Current.Browser.IsLoading;
                    Menu.MenuItems[2].MenuItems[13].Enabled = !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri);
                    Menu.MenuItems[2].MenuItems[14].Enabled = webInfoHandler.Current.CanReload
                        && !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri);
                    Menu.MenuItems[2].MenuItems[15].Enabled = webInfoHandler.Current.CanReload
                        && !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri);
                    Menu.MenuItems[2].MenuItems[17].Enabled = !webInfoHandler.Current.Browser.Address.Equals(Constants.BlankPageUri);
                    Menu.MenuItems[2].MenuItems[19].Enabled = settings.AudioEnabled;
                    Menu.MenuItems[2].MenuItems[20].Enabled = webInfoHandler.Current.HasChat;
                    Menu.MenuItems[2].MenuItems[22].Enabled = !isLoggedIn;
                    Menu.MenuItems[2].MenuItems[23].Enabled = !isLoggedIn
                        && !webInfoHandler.Current.Browser.Address.Equals(webInfoHandler.Current.Url);
                }));
            }));
        }

        private void AdjustRightPaneWidth() {
            settings.RightPaneWidth = tabControlRight.SelectedIndex < 2;
            SetRightPaneWidth(false);
            splitContainerMain.Invalidate();
        }

        private void SetText(string str) {
            if (string.IsNullOrEmpty(str) || Constants.BlankPageUri.Equals(str)) {
                Text = Program.GetTitle();
            } else {
                Text = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(str)
                    .ToString();
            }
        }

        private void SetConsoleMessage(int line, string source, string message) {
            if (settings.ShowConsoleMessages && !string.IsNullOrEmpty(source)) {
                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.PersistentB,
                    string.IsNullOrWhiteSpace(message)
                        ? string.Format(Constants.BrowserConsoleMessageFormat2, line, source.Trim())
                        : string.Format(Constants.BrowserConsoleMessageFormat1, line, source.Trim(), message.Trim()));
            }
        }

        private void SetLoadErrorMessage(CefErrorCode errorCode, string errorText, string failedUrl) {
            if (settings.ShowLoadErrors
                    && !errorCode.Equals(CefErrorCode.None)
                    && !string.IsNullOrEmpty(errorText)
                    && !string.IsNullOrEmpty(failedUrl)) {

                statusStripHandler.SetMessage(
                    StatusStripHandler.StatusMessageType.PersistentB,
                    string.Format(Constants.BrowserLoadErrorMessageFormat1, errorText.Trim(), failedUrl.Trim()));
            }
        }

        private void ShowBalance(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(ShowBalance), sender, e);
            } else {
                textBoxBalance.Text = FormatBalance(webInfoHandler.Balance);
            }
        }

        private string FormatBalance(decimal balance) {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(balance.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                .Replace(Constants.Hyphen, Constants.MinusSign)
                .Append(Constants.Space)
                .Append(Properties.Resources.LabelCurrencySymbol);
            return stringBuilder.ToString();
        }

        private void SetTips(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(SetTips), sender, e);
            } else if (AddTips(webInfoHandler.Tips) && settings.EnableBell) {
                telephoneBell.Ring();
            }
        }

        private int GetTipUidIndex(string uid) {
            for (int i = 0; i < listViewTips.Items.Count; i++) {
                if (((Tip)listViewTips.Items[i].Tag).Uid.Equals(uid)) {
                    return i;
                }
            }
            return -1;
        }

        private int GetServiceUidIndex(string uid) {
            for (int i = 0; i < listViewServices.Items.Count; i++) {
                if (((Tip)listViewServices.Items[i].Tag).Uid.Equals(uid)) {
                    return i;
                }
            }
            return -1;
        }

        private bool AddTips(Tip[] tips) {
            if (tips == null) {
                return false;
            }
            bool added = false;
            List<ListViewItem> listViewItems = new List<ListViewItem>();
            try {
                foreach (Tip tip in tips) {
                    if (GetTipUidIndex(tip.Uid) < 0) {
                        listViewItems.Add(SetTip(tip));
                        added = true;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                if (added) {
                    listViewTips.Items.AddRange(listViewItems.ToArray());
                    listViewTips.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listViewTips.Sort();
                }
            }
            return added;
        }

        private void AddServices(Service[] services) {
            if (services == null) {
                return;
            }
            List<ListViewItem> listViewItems = new List<ListViewItem>();
            try {
                foreach (Service service in services) {
                    if (GetServiceUidIndex(service.Uid) < 0) {
                        listViewItems.Add(SetService(service));
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                if (listViewItems.Count > 0) {
                    listViewServices.Items.AddRange(listViewItems.ToArray());
                    listViewServices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listViewServices.Sort();
                }
            }
        }

        private ListViewItem SetTip(Tip tip) {
            string sport, match, league, opportunity;
            DateTime dateTime;
            if (tip.Games.Length > 0) {
                sport = tip.Games[0].Sport;
                match = tip.Games[0].Match;
                league = tip.Games[0].League;
                dateTime = tip.Games[0].DateTime;
                opportunity = tip.Games[0].Opportunity;
            } else {
                sport = string.Empty;
                match = string.Empty;
                league = string.Empty;
                dateTime = tip.DateTime;
                opportunity = string.Empty;
            }
            ListViewItem listViewItem = new ListViewItem(
                new ListViewItem.ListViewSubItem[] {
                    new ListViewItem.ListViewSubItem() {
                        Text = sport,
                        Tag = sport
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = match,
                        Tag = match
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = league,
                        Tag = league
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = dateTime.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern),
                        Tag = dateTime
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = dateTime.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortTimePattern),
                        Tag = dateTime
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = opportunity,
                        Tag = opportunity
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = tip.Bookmaker,
                        Tag = tip.Bookmaker
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = tip.Odd.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo),
                        Tag = tip.Odd
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = new StringBuilder()
                            .Append(tip.TrustDegree.ToString(Constants.ZeroDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                            .Append(Constants.Slash)
                            .Append(10)
                            .ToString(),
                        Tag = tip.TrustDegree
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = tip.Service,
                        Tag = tip.Service
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = tip.Status.ToString(),
                        Tag = tip.Status.ToString()
                    }
                },
                (int)tip.Status);
            listViewItem.Tag = tip;
            return listViewItem;
        }

        private ListViewItem SetTip(Tip tip, ListViewItem listViewItem) {
            string sport, match, league, opportunity;
            DateTime dateTime;
            if (tip.Games.Length > 0) {
                sport = tip.Games[0].Sport;
                match = tip.Games[0].Match;
                league = tip.Games[0].League;
                dateTime = tip.Games[0].DateTime;
                opportunity = tip.Games[0].Opportunity;
            } else {
                sport = string.Empty;
                match = string.Empty;
                league = string.Empty;
                dateTime = tip.DateTime;
                opportunity = string.Empty;
            }
            listViewItem.SubItems[0].Text = sport;
            listViewItem.SubItems[0].Tag = sport;
            listViewItem.SubItems[1].Text = match;
            listViewItem.SubItems[1].Tag = match;
            listViewItem.SubItems[2].Text = league;
            listViewItem.SubItems[2].Tag = league;
            listViewItem.SubItems[3].Text = dateTime.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern);
            listViewItem.SubItems[3].Tag = dateTime;
            listViewItem.SubItems[4].Text = dateTime.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortTimePattern);
            listViewItem.SubItems[4].Tag = dateTime;
            listViewItem.SubItems[5].Text = opportunity;
            listViewItem.SubItems[5].Tag = opportunity;
            listViewItem.SubItems[6].Text = tip.Bookmaker;
            listViewItem.SubItems[6].Tag = tip.Bookmaker;
            listViewItem.SubItems[7].Text = tip.Odd.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo);
            listViewItem.SubItems[7].Tag = tip.Odd;
            listViewItem.SubItems[8].Text = new StringBuilder()
                .Append(tip.TrustDegree.ToString(Constants.ZeroDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                .Append(Constants.Slash)
                .Append(10)
                .ToString();
            listViewItem.SubItems[8].Tag = tip.TrustDegree;
            listViewItem.SubItems[9].Text = tip.Service;
            listViewItem.SubItems[9].Tag = tip.Service;
            listViewItem.SubItems[10].Text = tip.Status.ToString();
            listViewItem.SubItems[10].Tag = tip.Status.ToString();
            listViewItem.ImageIndex = (int)tip.Status;
            listViewItem.Tag = tip;
            return listViewItem;
        }

        private ListViewItem SetService(Service service) {
            ListViewItem listViewItem = new ListViewItem(
                new ListViewItem.ListViewSubItem[] {
                    new ListViewItem.ListViewSubItem() {
                        Text = service.Name,
                        Tag = service.Name
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = ShowPrice(service.Price),
                        Tag = service.Price
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = ShowSpan(service.Span, service.Unit),
                        Tag = service.Span * GetSpanUnitMultiplier(service.Unit)
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = service.Expiration.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern),
                        Tag = service.Expiration
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = service.Subscribed.ToString(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern),
                        Tag = service.Subscribed
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = service.Status.ToString(),
                        Tag = service.Status.ToString()
                    }
                },
                (int)service.Status);
            listViewItem.Tag = service;
            return listViewItem;
        }

        private ListViewItem SetService(Service service, ListViewItem listViewItem) {
            listViewItem.SubItems[0].Text = service.Name;
            listViewItem.SubItems[0].Tag = service.Name;
            listViewItem.SubItems[1].Text = ShowPrice(service.Price);
            listViewItem.SubItems[1].Tag = service.Price;
            listViewItem.SubItems[2].Text = ShowSpan(service.Span, service.Unit);
            listViewItem.SubItems[2].Tag = service.Span * GetSpanUnitMultiplier(service.Unit);
            listViewItem.SubItems[3].Text = service.Expiration.ToString(
                settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern);
            listViewItem.SubItems[3].Tag = service.Expiration;
            listViewItem.SubItems[4].Text = service.Subscribed.ToString(
                settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern);
            listViewItem.SubItems[4].Tag = service.Subscribed;
            listViewItem.SubItems[5].Text = service.Status.ToString();
            listViewItem.SubItems[5].Tag = service.Status.ToString();
            listViewItem.ImageIndex = (int)service.Status;
            listViewItem.Tag = service;
            return listViewItem;
        }

        private static int GetSpanUnitMultiplier(Service.SpanUnit spanUnit) {
            switch (spanUnit) {
                case Service.SpanUnit.Month:
                    return 31;
                case Service.SpanUnit.Year:
                    return 366;
                default:
                    return 1;
            }
        }

        private string ShowPrice(decimal price) {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(price.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                .Append(Constants.Space)
                .Append(Properties.Resources.LabelCurrencySymbol);
            return stringBuilder.ToString();
        }

        private string ShowSpan(int span, Service.SpanUnit spanUnit) {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(span.ToString(Constants.ZeroDecimalDigitsFormat, CultureInfo.InvariantCulture))
                .Append(Constants.Space)
                .Append(spanUnit.ToString());
            return stringBuilder.ToString();
        }

        private void AdjustSize(Size size) {
            if (InvokeRequired) {
                Invoke(new MainFormSizeEventHandler(AdjustSize), size);
            } else if (webInfoHandler.Current.Browser.Width < size.Width && webInfoHandler.Current.Browser.Height < size.Height) {
                if (!splitContainerMain.Panel2Collapsed) {
                    ToggleRightPane(null, EventArgs.Empty);
                }
                Size newSize = new Size(size.Width + 10, size.Height + 100);
                Size = new Size(Width < newSize.Width ? newSize.Width : Width, Height < newSize.Height ? newSize.Height : Height);
            } else if (webInfoHandler.Current.Browser.Width < size.Width) {
                if (!splitContainerMain.Panel2Collapsed) {
                    ToggleRightPane(null, EventArgs.Empty);
                }
                if (Width < size.Width) {
                    Size = new Size(size.Width + 10, Height);
                }
            } else if (webInfoHandler.Current.Browser.Height < size.Height) {
                if (Height < size.Height) {
                    Size = new Size(Width, size.Height + 100);
                }
            }
        }

        private static Point GetNewLocation(Rectangle rectangle, Rectangle dialogRectangle) {
            Rectangle findRectangle = rectangle;
            findRectangle.Inflate(100, 100);
            Rectangle intersectRectangle = rectangle;
            intersectRectangle.Intersect(dialogRectangle);
            if (intersectRectangle.IsEmpty) {
                return dialogRectangle.Location;
            }
            intersectRectangle = findRectangle;
            intersectRectangle.Intersect(dialogRectangle);
            Point point = new Point(dialogRectangle.X, dialogRectangle.Y);
            point.X = dialogRectangle.X - intersectRectangle.Width - 1;
            if (point.X < SystemInformation.VirtualScreen.Left) {
                point.X = SystemInformation.VirtualScreen.Left;
            }
            Rectangle newDialogRectangle = new Rectangle(point, dialogRectangle.Size);
            intersectRectangle = rectangle;
            intersectRectangle.Intersect(newDialogRectangle);
            if (intersectRectangle.IsEmpty) {
                return point;
            }
            intersectRectangle = findRectangle;
            intersectRectangle.Intersect(dialogRectangle);
            point = new Point(dialogRectangle.X, dialogRectangle.Y);
            point.X = dialogRectangle.X + intersectRectangle.Width;
            if (point.X + dialogRectangle.Width > SystemInformation.VirtualScreen.Width) {
                point.X = SystemInformation.VirtualScreen.Width - dialogRectangle.Width;
            }
            newDialogRectangle = new Rectangle(point, dialogRectangle.Size);
            intersectRectangle = rectangle;
            intersectRectangle.Intersect(newDialogRectangle);
            if (intersectRectangle.IsEmpty) {
                return point;
            }
            intersectRectangle = findRectangle;
            intersectRectangle.Intersect(dialogRectangle);
            point = new Point(dialogRectangle.X, dialogRectangle.Y);
            point.Y = dialogRectangle.Y - intersectRectangle.Height - 1;
            if (point.Y < SystemInformation.VirtualScreen.Top) {
                point.Y = SystemInformation.VirtualScreen.Top;
            }
            newDialogRectangle = new Rectangle(point, dialogRectangle.Size);
            intersectRectangle = rectangle;
            intersectRectangle.Intersect(newDialogRectangle);
            if (intersectRectangle.IsEmpty) {
                return point;
            }
            intersectRectangle = findRectangle;
            intersectRectangle.Intersect(dialogRectangle);
            point = new Point(dialogRectangle.X, dialogRectangle.Y);
            point.Y = dialogRectangle.Y + intersectRectangle.Height;
            if (point.Y + dialogRectangle.Height > SystemInformation.VirtualScreen.Height) {
                point.Y = SystemInformation.VirtualScreen.Height - dialogRectangle.Height;
            }
            return point;
        }

        private async void OutlineSearchResultAsync(Rectangle rectangle) {
            if (rectangle.IsEmpty
                    || !rectangle.IntersectsWith(
                        webInfoHandler.Current.Browser.RectangleToScreen(webInfoHandler.Current.Browser.ClientRectangle))
                    || !rectangle.IntersectsWith(
                        new Rectangle(SystemInformation.VirtualScreen.Location, SystemInformation.VirtualScreen.Size))) {

                return;
            }
            await Task.Run(new Action(() => {
                rectangle.Inflate(2, 2);
                IntPtr desktopPtr = NativeMethods.GetDC(IntPtr.Zero);
                Graphics graphics = Graphics.FromHdc(desktopPtr);
                Pen pen = new Pen(Color.Red, 5);
                pen.LineJoin = LineJoin.Bevel;
                graphics.DrawRectangle(pen, rectangle);
                graphics.Dispose();
                pen.Dispose();
                NativeMethods.ReleaseDC(IntPtr.Zero, desktopPtr);
            }));
        }

        private static decimal ParseTrustDegree(string str) {
            return decimal.Parse(
                Regex.Replace(
                    str.Replace(Constants.Comma, Constants.Period),
                    Constants.JSBalancePattern,
                    string.Empty),
                CultureInfo.InvariantCulture);
        }

        private void OnTextBox10KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees10();
            }
        }

        private void UpdateTrustDegrees10() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[9] = ParseTrustDegree(textBox10.Text);
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[9] * 9m / 10m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[9] * 8m / 10m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[9] * 7m / 10m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[9] * 6m / 10m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[9] * 5m / 10m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[9] * 4m / 10m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[9] * 3m / 10m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[9] * 2m / 10m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[9] / 10m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox10.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox10);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox9KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees9();
            }
        }

        private void UpdateTrustDegrees9() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[8] = ParseTrustDegree(textBox9.Text);
                trustDegrees[9] = trustDegrees[8] * 10m / 9m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[8] * 8m / 9m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[8] * 7m / 9m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[8] * 6m / 9m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[8] * 5m / 9m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[8] * 4m / 9m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[8] * 3m / 9m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[8] * 2m / 9m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[8] / 9m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox9.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox9);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox8KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees8();
            }
        }

        private void UpdateTrustDegrees8() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[7] = ParseTrustDegree(textBox8.Text);
                trustDegrees[9] = trustDegrees[7] * 10m / 8m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[7] * 9m / 8m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[7] * 7m / 8m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[7] * 6m / 8m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[7] * 5m / 8m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[7] * 4m / 8m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[7] * 3m / 8m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[7] * 2m / 8m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[7] / 8m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox8.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox8);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox7KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees7();
            }
        }

        private void UpdateTrustDegrees7() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[6] = ParseTrustDegree(textBox7.Text);
                trustDegrees[9] = trustDegrees[6] * 10m / 7m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[6] * 9m / 7m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[6] * 8m / 7m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[6] * 6m / 7m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[6] * 5m / 7m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[6] * 4m / 7m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[6] * 3m / 7m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[6] * 2m / 7m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[6] / 7m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox7.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox7);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox6KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees6();
            }
        }

        private void UpdateTrustDegrees6() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[5] = ParseTrustDegree(textBox6.Text);
                trustDegrees[9] = trustDegrees[5] * 10m / 6m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[5] * 9m / 6m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[5] * 8m / 6m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[5] * 7m / 6m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[5] * 5m / 6m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[5] * 4m / 6m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[5] * 3m / 6m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[5] * 2m / 6m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[5] / 6m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox6.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox6);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox5KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees5();
            }
        }

        private void UpdateTrustDegrees5() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[4] = ParseTrustDegree(textBox5.Text);
                trustDegrees[9] = trustDegrees[4] * 10m / 5m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[4] * 9m / 5m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[4] * 8m / 5m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[4] * 7m / 5m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[4] * 6m / 5m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[4] * 4m / 5m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[4] * 3m / 5m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[4] * 2m / 5m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[4] / 5m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox5.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox5);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox4KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees4();
            }
        }

        private void UpdateTrustDegrees4() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[3] = ParseTrustDegree(textBox4.Text);
                trustDegrees[9] = trustDegrees[3] * 10m / 4m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[3] * 9m / 4m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[3] * 8m / 4m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[3] * 7m / 4m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[3] * 6m / 4m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[3] * 5m / 4m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[3] * 3m / 4m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[3] * 2m / 4m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[3] / 4m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox4.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox4);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox3KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees3();
            }
        }

        private void UpdateTrustDegrees3() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[2] = ParseTrustDegree(textBox3.Text);
                trustDegrees[9] = trustDegrees[2] * 10m / 3m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[2] * 9m / 3m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[2] * 8m / 3m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[2] * 7m / 3m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[2] * 6m / 3m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[2] * 5m / 3m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[2] * 4m / 3m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[2] * 2m / 3m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[2] / 3m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox3.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox3);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox2KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees2();
            }
        }

        private void UpdateTrustDegrees2() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[1] = ParseTrustDegree(textBox2.Text);
                trustDegrees[9] = trustDegrees[1] * 10m / 2m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[1] * 9m / 2m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[1] * 8m / 2m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[1] * 7m / 2m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[1] * 6m / 2m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[1] * 5m / 2m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[1] * 4m / 2m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[1] * 3m / 2m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                textBox2.Text = ShowPrice(trustDegrees[1]);
                trustDegrees[0] = trustDegrees[1] / 2m;
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox2.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox2);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void OnTextBox1KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.Enter)) {
                UpdateTrustDegrees1();
            }
        }

        private void UpdateTrustDegrees1() {
            try {
                UnsubscribeOnTBTextChanged();
                trustDegrees[0] = ParseTrustDegree(textBox1.Text);
                trustDegrees[9] = trustDegrees[0] * 10m;
                textBox10.Text = ShowPrice(trustDegrees[9]);
                trustDegrees[8] = trustDegrees[0] * 9m;
                textBox9.Text = ShowPrice(trustDegrees[8]);
                trustDegrees[7] = trustDegrees[0] * 8m;
                textBox8.Text = ShowPrice(trustDegrees[7]);
                trustDegrees[6] = trustDegrees[0] * 7m;
                textBox7.Text = ShowPrice(trustDegrees[6]);
                trustDegrees[5] = trustDegrees[0] * 6m;
                textBox6.Text = ShowPrice(trustDegrees[5]);
                trustDegrees[4] = trustDegrees[0] * 5m;
                textBox5.Text = ShowPrice(trustDegrees[4]);
                trustDegrees[3] = trustDegrees[0] * 4m;
                textBox4.Text = ShowPrice(trustDegrees[3]);
                trustDegrees[2] = trustDegrees[0] * 3m;
                textBox3.Text = ShowPrice(trustDegrees[2]);
                trustDegrees[1] = trustDegrees[0] * 2m;
                textBox2.Text = ShowPrice(trustDegrees[1]);
                textBox1.Text = ShowPrice(trustDegrees[0]);
                textBox1.BackColor = SystemColors.Window;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetTBErrorBgColor(textBox1);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void UpdateTrustDegrees() {
            try {
                UnsubscribeOnTBTextChanged();
                textBox10.Text = ShowPrice(trustDegrees[9]);
                textBox9.Text = ShowPrice(trustDegrees[8]);
                textBox8.Text = ShowPrice(trustDegrees[7]);
                textBox7.Text = ShowPrice(trustDegrees[6]);
                textBox6.Text = ShowPrice(trustDegrees[5]);
                textBox5.Text = ShowPrice(trustDegrees[4]);
                textBox4.Text = ShowPrice(trustDegrees[3]);
                textBox3.Text = ShowPrice(trustDegrees[2]);
                textBox2.Text = ShowPrice(trustDegrees[1]);
                textBox1.Text = ShowPrice(trustDegrees[0]);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                SubscribeOnTBTextChanged();
            }
        }

        private void SubscribeOnTBTextChanged() {
            foreach (Control control in groupBoxTrustDegrees.Controls) {
                if (control is TextBox) {
                    control.TextChanged += new EventHandler(OnTBTextChanged);
                }
            }
        }

        private void UnsubscribeOnTBTextChanged() {
            foreach (Control control in groupBoxTrustDegrees.Controls) {
                if (control is TextBox) {
                    control.BackColor = SystemColors.Window;
                    control.TextChanged -= new EventHandler(OnTBTextChanged);
                }
            }
        }

        private void OnTBTextChanged(object sender, EventArgs e) {
            foreach (Control control in groupBoxTrustDegrees.Controls) {
                if (control is TextBox) {
                    if (control.Equals(sender)) {
                        control.BackColor = Color.FromArgb(255, 230, 204);
                    } else if (!control.BackColor.Equals(Color.FromArgb(255, 128, 128))) {
                        control.BackColor = SystemColors.Window;
                    }
                }
            }
        }

        private void SetTBErrorBgColor(TextBox sender) {
            foreach (Control control in groupBoxTrustDegrees.Controls) {
                if (control is TextBox) {
                    control.BackColor = SystemColors.Window;
                    if (!control.Equals(sender)) {
                        ((TextBox)control).Text = string.Empty;
                    }
                }
            }
            sender.BackColor = Color.FromArgb(255, 128, 128);
        }

        private ControlInfo GetControlEntered() {
            if (webInfoHandler.Current != null
                    && webInfoHandler.Current.Browser != null
                    && (tabControlLeft.ContainsFocus || tabControlRight.SelectedIndex < 2)) {

                controlInfo = new ControlInfo(webInfoHandler.Current.Browser, webInfoHandler.Current.BrowserTitle);
            } else if (tabControlRight.ContainsFocus) {
                if (tabControlRight.SelectedIndex > 1) {
                    controlInfo = new ControlInfo(calculatorHandler.LastEntered, calculatorHandler.BrowserTitle);
                } else {
                    controlInfo = null;
                }
            } else {
                controlInfo = null;
            }
            return controlInfo;
        }

        private void OnButton10Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox10.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees10();
                textBox10.Focus();
                textBox10.Select(0, trustDegrees[9]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton9Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox9.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees9();
                textBox9.Focus();
                textBox9.Select(0, trustDegrees[8]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton8Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox8.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees8();
                textBox8.Focus();
                textBox8.Select(0, trustDegrees[7]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton7Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox7.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees7();
                textBox7.Focus();
                textBox7.Select(0, trustDegrees[6]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton6Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox6.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees6();
                textBox6.Focus();
                textBox6.Select(0, trustDegrees[5]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton5Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox5.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees5();
                textBox5.Focus();
                textBox5.Select(0, trustDegrees[4]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton4Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox4.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees4();
                textBox4.Focus();
                textBox4.Select(0, trustDegrees[3]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton3Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox3.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees3();
                textBox3.Focus();
                textBox3.Select(0, trustDegrees[2]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton2Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox2.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees2();
                textBox2.Focus();
                textBox2.Select(0, trustDegrees[1]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnButton1Click(object sender, EventArgs e) {
            try {
                Clipboard.SetText(ParseTrustDegree(textBox1.Text)
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo));
                UpdateTrustDegrees1();
                textBox1.Focus();
                textBox1.Select(0, trustDegrees[0]
                    .ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo).Length);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnTBNTextChanged(object sender, EventArgs e) {
            buttonClearNotes.Enabled = textBoxNotes.TextLength > 0;
            undoneNotes = null;
            undone = false;
        }

        private void OnAddMovementButtonClick(object sender, EventArgs e) {

        }

        private void OnClearNotesButtonClick(object sender, EventArgs e) {
            if (textBoxNotes.TextLength > 0) {
                string str = textBoxNotes.Text;
                textBoxNotes.Clear();
                undoneNotes = str;
                textBoxNotes.Focus();
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.Temporary);
            }
        }

        private void PlaceTip(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewTips.SelectedItems) {
                Tip tip = (Tip)listViewItem.Tag;
                tip.Status = Tip.TipStatus.Placed;
                SetTip(tip, listViewItem);
            }
        }

        private void SkipTip(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewTips.SelectedItems) {
                Tip tip = (Tip)listViewItem.Tag;
                tip.Status = Tip.TipStatus.Skipped;
                SetTip(tip, listViewItem);
            }
        }

        private void WinTip(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewTips.SelectedItems) {
                Tip tip = (Tip)listViewItem.Tag;
                tip.Status = Tip.TipStatus.Win;
                SetTip(tip, listViewItem);
            }
        }

        private void VoidTip(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewTips.SelectedItems) {
                Tip tip = (Tip)listViewItem.Tag;
                tip.Status = Tip.TipStatus.Void;
                SetTip(tip, listViewItem);
            }
        }

        private void LoseTip(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewTips.SelectedItems) {
                Tip tip = (Tip)listViewItem.Tag;
                tip.Status = Tip.TipStatus.Lose;
                SetTip(tip, listViewItem);
            }
        }

        private void NewTip(object sender, EventArgs e) {
            TipForm tipForm = new TipForm(settings);
            tipForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
            tipForm.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog = tipForm;
            if (tipForm.ShowDialog(this).Equals(DialogResult.OK)) {
                listViewTips.Items.Add(SetTip(tipForm.Tip));
                listViewTips.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void EditTip(object sender, EventArgs e) {
            if (listViewTips.SelectedItems.Count.Equals(1)) {
                TipForm tipForm = new TipForm(settings);
                tipForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
                tipForm.HelpRequested += new HelpEventHandler(OpenHelp);
                tipForm.Tip = (Tip)listViewTips.SelectedItems[0].Tag;
                dialog = tipForm;
                if (tipForm.ShowDialog(this).Equals(DialogResult.OK)) {
                    SetTip(tipForm.Tip, listViewTips.SelectedItems[0]);
                    listViewTips.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void ActiveService(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewServices.SelectedItems) {
                Service service = (Service)listViewItem.Tag;
                service.Status = Service.ServiceStatus.Active;
                SetService(service, listViewItem);
            }
        }

        private void ExpireService(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listViewServices.SelectedItems) {
                Service service = (Service)listViewItem.Tag;
                service.Status = Service.ServiceStatus.Expired;
                SetService(service, listViewItem);
            }
        }

        private void NewService(object sender, EventArgs e) {
            ServiceForm serviceForm = new ServiceForm(settings);
            serviceForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
            serviceForm.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog = serviceForm;
            if (serviceForm.ShowDialog(this).Equals(DialogResult.OK)) {
                listViewServices.Items.Add(SetService(serviceForm.Service));
                listViewServices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void EditService(object sender, EventArgs e) {
            if (listViewServices.SelectedItems.Count.Equals(1)) {
                ServiceForm serviceForm = new ServiceForm(settings);
                serviceForm.HelpButtonClicked += new CancelEventHandler(OpenHelp);
                serviceForm.HelpRequested += new HelpEventHandler(OpenHelp);
                serviceForm.Service = (Service)listViewServices.SelectedItems[0].Tag;
                dialog = serviceForm;
                if (serviceForm.ShowDialog(this).Equals(DialogResult.OK)) {
                    SetService(serviceForm.Service, listViewServices.SelectedItems[0]);
                    listViewServices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void DeleteItem(object sender, EventArgs e) {
            ListView listView = sender.GetType().Equals(typeof(ListView))
                ? (ListView)sender
                : (ListView)((MenuItem)sender).GetContextMenu().SourceControl;
            if (listView.SelectedItems.Count > 0) {
                string message;
                if (listView.SelectedItems[0].Tag.GetType().Equals(typeof(Tip))) {
                    message = listView.SelectedItems.Count > 1
                        ? string.Format(Properties.Resources.MessageDeleteTips, listView.SelectedItems.Count)
                        : Properties.Resources.MessageDeleteTip;
                } else {
                    message = listView.SelectedItems.Count > 1
                        ? string.Format(Properties.Resources.MessageDeleteServices, listView.SelectedItems.Count)
                        : Properties.Resources.MessageDeleteService;
                }
                dialog = new MessageForm(this, message, Properties.Resources.CaptionQuestion, MessageForm.Buttons.YesNo,
                    MessageForm.BoxIcon.Question);
                dialog.HelpRequested += new HelpEventHandler(OpenHelp);
                if (dialog.ShowDialog(this).Equals(DialogResult.Yes)) {
                    foreach (ListViewItem listViewItem in listView.SelectedItems) {
                        listView.Items.Remove(listViewItem);
                    }
                }
            }
        }

        private void TipsEnableControls(object sender, EventArgs e) {
            bool enabled = listViewTips.SelectedItems.Count > 0;
            buttonPlace.Enabled = enabled;
            buttonSkip.Enabled = enabled;
            buttonWin.Enabled = enabled;
            buttonVoid.Enabled = enabled;
            buttonLose.Enabled = enabled;
        }

        private void ServicesEnableControls(object sender, EventArgs e) {
            bool enabled = listViewServices.SelectedItems.Count > 0;
            buttonActive.Enabled = enabled;
            buttonExpire.Enabled = enabled;
        }

        private void SelectTip(object sender, EventArgs e) {
            if (clickedListViewItemTip != null) {
                clickedListViewItemTip.Selected = true;
            }
        }

        private void UnselectTip(object sender, EventArgs e) {
            if (clickedListViewItemTip != null) {
                clickedListViewItemTip.Selected = false;
            }
        }

        private void SelectService(object sender, EventArgs e) {
            if (clickedListViewItemService != null) {
                clickedListViewItemService.Selected = true;
            }
        }

        private void UnselectService(object sender, EventArgs e) {
            if (clickedListViewItemService != null) {
                clickedListViewItemService.Selected = false;
            }
        }

        private void SelectAll(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in ((ListView)((MenuItem)sender).GetContextMenu().SourceControl).Items) {
                listViewItem.Selected = true;
            }
        }

        private void SelectNone(object sender, EventArgs e) {
            ((ListView)((MenuItem)sender).GetContextMenu().SourceControl).SelectedItems.Clear();
        }

        private void OnListViewTipsMouseClick(object sender, MouseEventArgs e) {
            if (e.Button.Equals(MouseButtons.Right)) {
                clickedListViewItemTip = ((ListView)sender).GetItemAt(e.X, e.Y);
            }
        }

        private void OnListViewServicesClick(object sender, MouseEventArgs e) {
            if (e.Button.Equals(MouseButtons.Right)) {
                clickedListViewItemService = ((ListView)sender).GetItemAt(e.X, e.Y);
            }
        }

        private void OnListViewKeyDown(object sender, KeyEventArgs e) {
            ListView listView = (ListView)sender;
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                foreach (ListViewItem listViewItem in listView.Items) {
                    listViewItem.Selected = true;
                }
            } else if (e.KeyCode.Equals(Keys.Delete)) {
                DeleteItem(sender, e);
            }
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                ((TextBox)sender).SelectAll();
            }
        }

        private void OnTBNKeyDown(object sender, KeyEventArgs e) {
            TextBox textBox = (TextBox)sender;
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                textBox.SelectAll();
            } else if (e.Control && e.KeyCode.Equals(Keys.Z)) {
                e.SuppressKeyPress = true;
                if (textBox.CanUndo) {
                    textBox.Undo();
                } else {
                    if (!string.IsNullOrEmpty(undoneNotes)) {
                        textBox.Text = undoneNotes;
                        textBox.SelectAll();
                        undone = true;
                    } else if (undone) {
                        if (textBox.TextLength > 0) {
                            string str = textBox.Text;
                            textBox.Clear();
                            undoneNotes = str;
                        }
                        undone = false;
                    }
                }
            }
        }

        private void OnTextBoxMouseDown(object sender, MouseEventArgs e) {
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
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
                textBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private decimal[] LoadRightPaneData() {
            try {
                if (File.Exists(dataFilePath)) {
                    using (FileStream fileStream = File.OpenRead(dataFilePath)) {
                        Data data = (Data)new BinaryFormatter().Deserialize(fileStream);
                        textBoxBalance.Text = FormatBalance(data.balance);
                        trustDegrees = data.trustDegrees ?? new decimal[10];
                        UpdateTrustDegrees();
                        textBoxNotes.Text = data.notes;
                        if (data.sortOrderTips.Equals(SortOrder.None)) {
                            listViewTipsSorter.SortColumn = Constants.TipsDefaultSortColumn;
                            listViewTipsSorter.SortOrder = SortOrder.Descending;
                        } else {
                            listViewTipsSorter.SortColumn = data.sortColumnTips;
                            listViewTipsSorter.SortOrder = data.sortOrderTips;
                        }
                        AddTips(data.tips);
                        if (data.sortOrderServices.Equals(SortOrder.None)) {
                            listViewServicesSorter.SortColumn = Constants.ServicesDefaultSortColumn;
                            listViewServicesSorter.SortOrder = SortOrder.Descending;
                        } else {
                            listViewServicesSorter.SortColumn = data.sortColumnServices;
                            listViewServicesSorter.SortOrder = data.sortOrderServices;
                        }
                        AddServices(data.services);
                        return data.balances;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return null;
        }

        private void SaveRightPaneData() {
            List<Tip> tips = new List<Tip>(listViewTips.Items.Count);
            foreach (ListViewItem listViewItem in listViewTips.Items) {
                tips.Add((Tip)listViewItem.Tag);
            }
            List<Service> services = new List<Service>(listViewServices.Items.Count);
            foreach (ListViewItem listViewItem in listViewServices.Items) {
                services.Add((Service)listViewItem.Tag);
            }

            Data data = new Data(
                webInfoHandler.Balance,
                webInfoHandler.GetBalances(),
                trustDegrees,
                new Dictionary<DateTime, decimal>(),
                textBoxNotes.Text,
                tips.ToArray(),
                listViewTipsSorter.SortColumn,
                listViewTipsSorter.SortOrder,
                services.ToArray(),
                listViewServicesSorter.SortColumn,
                listViewServicesSorter.SortOrder);

            try {
                using (FileStream fileStream = File.Create(dataFilePath)) {
                    new BinaryFormatter().Serialize(fileStream, data);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void DeleteRightPaneData() {
            try {
                if (File.Exists(dataFilePath)) {
                    File.Delete(dataFilePath);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private static ImageList GetTipImageList() {
            ImageList imageList = new ImageList();
            imageList.Images.Add(Properties.Resources.ImgTipReceived.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipPublished.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipSkipped.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipPlaced.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipWin.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipVoid.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgTipLose.ToBitmap());
            return imageList;
        }

        private static ImageList GetServiceImageList() {
            ImageList imageList = new ImageList();
            imageList.Images.Add(Properties.Resources.ImgServiceActive.ToBitmap());
            imageList.Images.Add(Properties.Resources.ImgServiceExpired.ToBitmap());
            return imageList;
        }
    }
}
