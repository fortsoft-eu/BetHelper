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
 * Version 1.1.3.0
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class PreferencesForm : Form {
        private bool disableThemes, persistSessionCookies, persistUserPreferences;
        private Form dialog;
        private IEnumerable<object> colors;
        private int textBoxClicks;
        private OpenFileDialog openFileDialog;
        private PersistWindowState persistWindowState;
        private Point location;
        private Settings settings;
        private Timer textBoxClicksTimer;

        public PreferencesForm() {
            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = true;
            openFileDialog.DefaultExt = Constants.ExtensionExe;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            colors = typeof(Color)
                .GetProperties()
                .Where(new Func<PropertyInfo, bool>(propertyInfo => propertyInfo.PropertyType == typeof(Color)))
                .Select(new Func<PropertyInfo, object>(propertyInfo => propertyInfo.GetValue(null)));

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveSize = true;
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.Parent = this;

            InitializeComponent();

            SuspendLayout();
            BuildContextMenuAsync();

            comboBoxUnitPrefix.Items.AddRange(new string[] {
                Constants.UnitPrefixBinary,
                Constants.UnitPrefixDecimal
            });

            pictureBoxWarning.Image = Properties.Resources.Warning.ToBitmap();

            comboBoxAcceptLanguage.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxBookmakerDColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxBookmakerSColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxBorderStyle.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxCalculatorDColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxCalculatorSColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxDashboardDColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxDashboardSColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxNumberFormat.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxOverlayBackgroundColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxOverlayCrosshairColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxSportInfo1DColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxSportInfo1SColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxSportInfo2DColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxSportInfo2SColor.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxStripRenderMode.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxTabAppearance.MaxDropDownItems = Constants.PrefsMaxDropDownItems;
            comboBoxUserAgent.MaxDropDownItems = Constants.PrefsMaxDropDownItems;

            comboBoxBookmakerDColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxBookmakerSColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxBorderStyle.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxCalculatorDColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxCalculatorSColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxDashboardDColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxDashboardSColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxNumberFormat.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxOverlayBackgroundColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxOverlayCrosshairColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxSportInfo1DColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxSportInfo1SColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxSportInfo2DColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxSportInfo2SColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxStripRenderMode.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxTabAppearance.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxUnitPrefix.DrawMode = DrawMode.OwnerDrawFixed;

            comboBoxBookmakerDColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxBookmakerSColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxBorderStyle.DrawItem += new DrawItemEventHandler(OnDrawItem);
            comboBoxCalculatorDColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxCalculatorSColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxDashboardDColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxDashboardSColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxNumberFormat.DrawItem += new DrawItemEventHandler(OnDrawItem);
            comboBoxOverlayBackgroundColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxOverlayCrosshairColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxSportInfo1DColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxSportInfo1SColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxSportInfo2DColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxSportInfo2SColor.DrawItem += new DrawItemEventHandler(OnColorDrawItem);
            comboBoxStripRenderMode.DrawItem += new DrawItemEventHandler(OnDrawItem);
            comboBoxTabAppearance.DrawItem += new DrawItemEventHandler(OnDrawItem);
            comboBoxUnitPrefix.DrawItem += new DrawItemEventHandler(OnDrawItem);

            comboBoxTabAppearance.DataSource = Enum.GetValues(typeof(TabAppearance));
            List<ToolStripRenderMode> list = new List<ToolStripRenderMode>();
            foreach (ToolStripRenderMode toolStripRenderMode in Enum.GetValues(typeof(ToolStripRenderMode))) {
                if (toolStripRenderMode > 0) {
                    list.Add(toolStripRenderMode);
                }
            }
            comboBoxStripRenderMode.DataSource = list.ToArray();
            comboBoxBorderStyle.DataSource = Enum.GetValues(typeof(Border3DStyle));

            comboBoxBookmakerDColor.DataSource = colors.ToList();
            comboBoxBookmakerSColor.DataSource = colors.ToList();
            comboBoxCalculatorDColor.DataSource = colors.ToList();
            comboBoxCalculatorSColor.DataSource = colors.ToList();
            comboBoxDashboardDColor.DataSource = colors.ToList();
            comboBoxDashboardSColor.DataSource = colors.ToList();
            comboBoxOverlayBackgroundColor.DataSource = colors.ToList();
            comboBoxOverlayCrosshairColor.DataSource = colors.ToList();
            comboBoxSportInfo1DColor.DataSource = colors.ToList();
            comboBoxSportInfo1SColor.DataSource = colors.ToList();
            comboBoxSportInfo2DColor.DataSource = colors.ToList();
            comboBoxSportInfo2SColor.DataSource = colors.ToList();

            MouseWheel += new MouseEventHandler(OnMouseWheel);
            MouseMove += new MouseEventHandler(OnMouseWheel);
        }

        public bool RestartRequired { get; private set; }

        public Settings Settings {
            get {
                return settings;
            }
            set {
                settings = value;
                comboBoxUserAgent.Items.AddRange(settings.UserAgents);
                comboBoxAcceptLanguage.Items.AddRange(settings.AcceptLanguages);
                comboBoxUserAgent.Text = settings.UserAgent;
                comboBoxAcceptLanguage.Text = settings.AcceptLanguage;
                checkBoxEnableCache.Checked = settings.EnableCache;
                checkBoxPersistSessionCookies.Checked = settings.PersistSessionCookies;
                checkBoxPersistUserPreferences.Checked = settings.PersistUserPreferences;
                checkBoxEnablePrintPreview.Checked = settings.EnablePrintPreview;
                checkBoxEnableDrmContent.Checked = settings.EnableDrmContent;
                checkBoxEnableAudio.Checked = settings.EnableAudio;
                checkBoxEnableProxy.Checked = settings.EnableProxy;
                checkBoxOutlineSearchResults.Checked = settings.OutlineSearchResults;
                checkBoxF3MainFormFind.Checked = settings.F3MainFormFocusesFindForm;
                checkBoxLogForeignUrls.Checked = settings.LogForeignUrls;
                checkBoxLogPopUpFrameHandler.Checked = settings.LogPopUpFrameHandler;
                checkBoxShowLoadErrors.Checked = settings.ShowLoadErrors;
                checkBoxLogLoadErrors.Checked = settings.LogLoadErrors;
                checkBoxShowConsoleMessages.Checked = settings.ShowConsoleMessages;
                checkBoxLogConsoleMessages.Checked = settings.LogConsoleMessages;
                checkBoxEnableLogPreloadLimit.Checked = settings.EnableLogPreloadLimit;
                numericUpDownPreloadLimit.Minimum = settings.LogPreloadLimitMin;
                numericUpDownPreloadLimit.Maximum = settings.LogPreloadLimitMax;
                numericUpDownPreloadLimit.Value = settings.LogPreloadLimit;
                checkBoxRestrictSomeFeatures.Checked = settings.RestrictForLargeLogs;
                numericUpDownLargeLogsLimit.Minimum = settings.LargeLogsLimitMin;
                numericUpDownLargeLogsLimit.Maximum = settings.LargeLogsLimitMax;
                numericUpDownLargeLogsLimit.Value = settings.LargeLogsLimit;
                textBoxExternalEditor.Text = settings.ExternalEditor;
                numericUpDownOverlayOpacity.Minimum = settings.InspectOverlayOpacityMin;
                numericUpDownOverlayOpacity.Maximum = settings.InspectOverlayOpacityMax;
                numericUpDownOverlayOpacity.Value = settings.InspectOverlayOpacity;
                checkBoxCheckForUpdates.Checked = settings.CheckForUpdates;
                checkBoxStatusBarNotifOnly.Checked = settings.StatusBarNotifOnly;
                comboBoxUnitPrefix.SelectedIndex = settings.UseDecimalPrefix ? 1 : 0;
                labelKiB.Text = settings.UseDecimalPrefix ? Constants.Kilobyte : Constants.Kibibyte;
                comboBoxNumberFormat.DataSource = settings.NumberFormatHandler.NumberFormats;
                comboBoxNumberFormat.SelectedIndex = settings.NumberFormatInt;
                checkBoxAutoAdjustRightPaneWidth.Checked = settings.AutoAdjustRightPaneWidth;
                checkBoxAutoLogInAfterInitialLoad.Checked = settings.AutoLogInAfterInitialLoad;
                checkBoxDisplayPromptBeforeClosing.Checked = settings.DisplayPromptBeforeClosing;
                checkBoxEnableBell.Checked = settings.EnableBell;
                checkBoxSortBookmarks.Checked = settings.SortBookmarks;
                checkBoxTruncateBookmarkTitles.Checked = settings.TruncateBookmarkTitles;
                checkBoxPingWhenIdle.Checked = settings.TryToKeepUserLoggedIn;
                checkBoxBlockRequestsToForeignUrls.Checked = settings.BlockRequestsToForeignUrls;
                checkBoxKeepAnEyeOnTheClientsIP.Checked = settings.KeepAnEyeOnTheClientsIP;
                checkBoxIgnoreCertificateErrors.Checked = settings.IgnoreCertificateErrors;
                checkBoxBoldFont.Checked = settings.TabsBoldFont;
                checkBoxBackgroundColor.Checked = settings.TabsBackgroundColor;
                comboBoxTabAppearance.SelectedItem = settings.TabAppearance;
                comboBoxStripRenderMode.SelectedItem = settings.StripRenderMode;
                comboBoxBorderStyle.SelectedItem = settings.Border3DStyle;
                comboBoxBookmakerDColor.SelectedItem = GetNamedColor(settings.BookmakerDefaultColor);
                comboBoxBookmakerSColor.SelectedItem = GetNamedColor(settings.BookmakerSelectedColor);
                comboBoxSportInfo1DColor.SelectedItem = GetNamedColor(settings.SportInfo1DefaultColor);
                comboBoxSportInfo1SColor.SelectedItem = GetNamedColor(settings.SportInfo1SelectedColor);
                comboBoxSportInfo2DColor.SelectedItem = GetNamedColor(settings.SportInfo2DefaultColor);
                comboBoxSportInfo2SColor.SelectedItem = GetNamedColor(settings.SportInfo2SelectedColor);
                comboBoxDashboardDColor.SelectedItem = GetNamedColor(settings.DashboardDefaultColor);
                comboBoxDashboardSColor.SelectedItem = GetNamedColor(settings.DashboardSelectedColor);
                comboBoxCalculatorDColor.SelectedItem = GetNamedColor(settings.CalculatorDefaultColor);
                comboBoxCalculatorSColor.SelectedItem = GetNamedColor(settings.CalculatorSelectedColor);
                comboBoxOverlayBackgroundColor.SelectedItem = GetNamedColor(settings.OverlayBackgroundColor);
                comboBoxOverlayCrosshairColor.SelectedItem = GetNamedColor(settings.OverlayCrosshairColor);
                checkBoxDisableThemes.Checked = settings.DisableThemes;
                checkBoxDisableThemes.Visible = settings.DisableThemes || settings.RenderWithVisualStyles;
                disableThemes = settings.DisableThemes;
                if (settings.PrintSoftMargins) {
                    radioButtonSoftMargins.Checked = true;
                } else {
                    radioButtonHardMargins.Checked = true;
                }
                checkBoxStatusBarNotifOnly.Enabled = checkBoxCheckForUpdates.Checked;
                if (checkBoxEnableCache.Checked) {
                    checkBoxPersistSessionCookies.Enabled = true;
                    checkBoxPersistUserPreferences.Enabled = true;
                } else {
                    checkBoxPersistSessionCookies.Enabled = false;
                    checkBoxPersistUserPreferences.Enabled = false;
                    checkBoxPersistSessionCookies.Checked = false;
                    checkBoxPersistUserPreferences.Checked = false;
                }
                numericUpDownPreloadLimit.Enabled = checkBoxEnableLogPreloadLimit.Checked;
                labelKiB.Enabled = checkBoxEnableLogPreloadLimit.Checked;
                numericUpDownLargeLogsLimit.Enabled = checkBoxRestrictSomeFeatures.Checked;
                labelMiB.Enabled = checkBoxRestrictSomeFeatures.Checked;
                if (settings.ActivePreferencesPanel < 0 || settings.ActivePreferencesPanel > tabControl.TabCount - 1) {
                    tabControl.SelectedIndex = 0;
                } else {
                    tabControl.SelectedIndex = settings.ActivePreferencesPanel;
                }
                SetWarning();
                ResumeLayout(false);
                PerformLayout();
            }
        }

        public TabControl TabControl => tabControl;

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysCtrlZ))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CUT,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_COPY,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_PASTE,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CLEAR,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    ComboBox comboBox = (ComboBox)contextMenu.SourceControl;
                    if (!comboBox.Focused) {
                        comboBox.Focus();
                    }
                    bool enabled = comboBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = enabled;
                    contextMenu.MenuItems[3].Enabled = enabled;
                    contextMenu.MenuItems[5].Enabled = enabled;
                    contextMenu.MenuItems[7].Enabled = comboBox.SelectionLength < comboBox.Text.Length;
                    try {
                        contextMenu.MenuItems[4].Enabled = Clipboard.ContainsText();
                    } catch (Exception exception) {
                        contextMenu.MenuItems[4].Enabled = true;
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
                comboBoxUserAgent.ContextMenu = contextMenu;
                comboBoxAcceptLanguage.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Undo())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CUT,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_COPY,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_PASTE,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CLEAR,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.CanUndo;
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
                numericUpDownOverlayOpacity.ContextMenu = contextMenu;
                numericUpDownPreloadLimit.ContextMenu = contextMenu;
                numericUpDownLargeLogsLimit.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Undo())));
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
                    contextMenu.MenuItems[0].Enabled = textBox.CanUndo;
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
                textBoxExternalEditor.ContextMenu = contextMenu;
            }));
        }

        private void EditRemoteConfig(object sender, EventArgs e) {
            try {
                Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWE);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog(this);
            }
        }

        private Color GetNamedColor(Color color) {
            foreach (Color item in colors) {
                if (item.ToArgb().Equals(color.ToArgb())) {
                    return item;
                }
            }
            return Color.Empty;
        }

        private void OnAllowedClientsClick(object sender, EventArgs e) {
            dialog = new AllowedAddrForm(settings);
            dialog.HelpButtonClicked += new CancelEventHandler((s, t) => OnHelpRequested(new HelpEventArgs(Point.Empty)));
            dialog.ShowDialog(this);
        }

        private void OnAutoUpdatesCheckedChanged(object sender, EventArgs e) {
            checkBoxStatusBarNotifOnly.Enabled = checkBoxCheckForUpdates.Checked;
        }

        private void OnButtonBrowseClick(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(textBoxExternalEditor.Text)) {
                    if (File.Exists(textBoxExternalEditor.Text)) {
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(textBoxExternalEditor.Text);
                        openFileDialog.FileName = Path.GetFileName(textBoxExternalEditor.Text);
                    } else if (Directory.Exists(textBoxExternalEditor.Text)) {
                        openFileDialog.InitialDirectory = textBoxExternalEditor.Text;
                        openFileDialog.FileName = string.Empty;
                    } else {
                        string path = Path.GetDirectoryName(textBoxExternalEditor.Text);
                        if (Directory.Exists(path)) {
                            openFileDialog.InitialDirectory = path;
                            openFileDialog.FileName = string.Empty;
                        }
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (openFileDialog.ShowDialog().Equals(DialogResult.OK)) {
                    textBoxExternalEditor.Text = openFileDialog.FileName;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void OnColorDrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            ComboBox comboBox = (ComboBox)sender;
            if (e.Index >= 0) {
                Color color;
                Pen pen;
                if (comboBox.DroppedDown) {
                    if ((e.State & DrawItemState.Selected).Equals(DrawItemState.Selected)
                            || (e.State & DrawItemState.HotLight).Equals(DrawItemState.HotLight)) {

                        color = SystemColors.HighlightText;
                        pen = SystemPens.HighlightText;
                    } else {
                        color = comboBox.ForeColor;
                        pen = Pens.Black;
                    }
                } else if ((e.State & DrawItemState.Focus).Equals(DrawItemState.Focus)) {
                    color = SystemColors.HighlightText;
                    pen = SystemPens.HighlightText;
                } else {
                    color = comboBox.ForeColor;
                    pen = Pens.Black;
                }
                string text = comboBox.GetItemText(comboBox.Items[e.Index]);
                Rectangle rectangle1 = new Rectangle(
                    e.Bounds.Left + 1,
                    e.Bounds.Top + 1,
                    2 * (e.Bounds.Height - 2),
                    e.Bounds.Height - 2);
                Rectangle rectangle2 = Rectangle.FromLTRB(
                    rectangle1.Right + 2,
                    e.Bounds.Top,
                    e.Bounds.Right,
                    e.Bounds.Bottom);
                using (SolidBrush solidBrush = new SolidBrush((Color)comboBox.Items[e.Index])) {
                    e.Graphics.FillRectangle(solidBrush, rectangle1);
                }
                e.Graphics.DrawRectangle(pen, rectangle1);
                TextRenderer.DrawText(e.Graphics, text, comboBox.Font, rectangle2, color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            if (!comboBox.DroppedDown) {
                e.DrawFocusRectangle();
            }
        }

        private void OnComboBoxMouseDown(object sender, MouseEventArgs e) {
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            ComboBox comboBox = (ComboBox)sender;
            textBoxClicksTimer.Stop();
            if (comboBox.SelectionLength > 0) {
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
                comboBox.SelectAll();
                comboBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            ComboBox comboBox = (ComboBox)sender;
            if (e.Index >= 0) {
                Color color;
                Pen pen;
                if (comboBox.DroppedDown) {
                    if ((e.State & DrawItemState.Selected).Equals(DrawItemState.Selected)
                            || (e.State & DrawItemState.HotLight).Equals(DrawItemState.HotLight)) {

                        color = SystemColors.HighlightText;
                        pen = SystemPens.HighlightText;
                    } else {
                        color = comboBox.ForeColor;
                        pen = Pens.Black;
                    }
                } else if ((e.State & DrawItemState.Focus).Equals(DrawItemState.Focus)) {
                    color = SystemColors.HighlightText;
                    pen = SystemPens.HighlightText;
                } else {
                    color = comboBox.ForeColor;
                    pen = Pens.Black;
                }
                string text = comboBox.GetItemText(comboBox.Items[e.Index]);
                TextRenderer.DrawText(e.Graphics, text, comboBox.Font, e.Bounds, color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            if (!comboBox.DroppedDown) {
                e.DrawFocusRectangle();
            }
        }

        private void OnDropDown(object sender, EventArgs e) {
            ComboBox comboBox = (ComboBox)sender;
            Graphics graphics = comboBox.CreateGraphics();
            int scrollBarWidth = comboBox.Items.Count > comboBox.MaxDropDownItems ? SystemInformation.VerticalScrollBarWidth : 0;
            int dropDownWidth = comboBox.Width;
            foreach (object obj in comboBox.Items) {
                string str = obj.ToString();
                int newWidth = (int)graphics.MeasureString(str, comboBox.Font).Width + scrollBarWidth;
                if (newWidth > dropDownWidth) {
                    dropDownWidth = newWidth;
                }
            }
            comboBox.DropDownWidth = dropDownWidth;
        }

        private void OnEnableCacheCheckedChanged(object sender, EventArgs e) {
            if (checkBoxEnableCache.Checked) {
                checkBoxPersistSessionCookies.Enabled = true;
                checkBoxPersistUserPreferences.Enabled = true;
                checkBoxPersistSessionCookies.Checked = persistSessionCookies;
                checkBoxPersistUserPreferences.Checked = persistUserPreferences;
            } else {
                checkBoxPersistSessionCookies.Enabled = false;
                checkBoxPersistUserPreferences.Enabled = false;
                persistSessionCookies = checkBoxPersistSessionCookies.Checked;
                persistUserPreferences = checkBoxPersistUserPreferences.Checked;
                checkBoxPersistSessionCookies.Checked = false;
                checkBoxPersistUserPreferences.Checked = false;
            }
            SetWarning();
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => textBoxClicksTimer.Dispose();

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is NumericUpDown) {
                    NumericUpDown numericUpDown = (NumericUpDown)sender;
                    numericUpDown.Select(0, numericUpDown.Text.Length);
                } else if (sender is ComboBox) {
                    ((ComboBox)sender).SelectAll();
                }
            }
        }

        private void OnLogViewerClick(object sender, EventArgs e) {
            try {
                Process.Start(Application.ExecutablePath, Constants.CommandLineSwitchWL);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog(this);
            }
        }

        private void OnMaxPreloadCheckedChanged(object sender, EventArgs e) {
            numericUpDownPreloadLimit.Enabled = checkBoxEnableLogPreloadLimit.Checked;
            labelKiB.Enabled = checkBoxEnableLogPreloadLimit.Checked;
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

        private void OnRestrictFeaturesCheckedChanged(object sender, EventArgs e) {
            numericUpDownLargeLogsLimit.Enabled = checkBoxRestrictSomeFeatures.Checked;
            labelMiB.Enabled = checkBoxRestrictSomeFeatures.Checked;
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
                    int selectionEnd = Math.Min(Array.IndexOf(chars,
                        Constants.CarriageReturn, textBox.SelectionStart),
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

        private void Save(object sender, EventArgs e) {
            string externalEditor = string.Empty;
            try {
                if (!string.IsNullOrWhiteSpace(textBoxExternalEditor.Text)) {
                    if (File.Exists(textBoxExternalEditor.Text)) {
                        externalEditor = textBoxExternalEditor.Text;
                    } else {
                        dialog = new MessageForm(this, Properties.Resources.MessageExternalEditorWarning, null, MessageForm.Buttons.OK,
                            MessageForm.BoxIcon.Exclamation);
                        dialog.ShowDialog(this);
                        tabControl.SelectedIndex = 1;
                        textBoxExternalEditor.Focus();
                        textBoxExternalEditor.SelectAll();
                        return;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }

            SetWarning();

            settings.ExternalEditor = externalEditor;
            settings.UserAgent = comboBoxUserAgent.Text;
            settings.AcceptLanguage = comboBoxAcceptLanguage.Text;
            settings.EnableCache = checkBoxEnableCache.Checked;
            settings.PersistSessionCookies = checkBoxPersistSessionCookies.Checked;
            settings.PersistUserPreferences = checkBoxPersistUserPreferences.Checked;
            settings.EnablePrintPreview = checkBoxEnablePrintPreview.Checked;
            settings.EnableDrmContent = checkBoxEnableDrmContent.Checked;
            settings.EnableAudio = checkBoxEnableAudio.Checked;
            settings.EnableProxy = checkBoxEnableProxy.Checked;
            settings.OutlineSearchResults = checkBoxOutlineSearchResults.Checked;
            settings.F3MainFormFocusesFindForm = checkBoxF3MainFormFind.Checked;
            settings.LogForeignUrls = checkBoxLogForeignUrls.Checked;
            settings.LogPopUpFrameHandler = checkBoxLogPopUpFrameHandler.Checked;
            settings.ShowLoadErrors = checkBoxShowLoadErrors.Checked;
            settings.LogLoadErrors = checkBoxLogLoadErrors.Checked;
            settings.ShowConsoleMessages = checkBoxShowConsoleMessages.Checked;
            settings.LogConsoleMessages = checkBoxLogConsoleMessages.Checked;
            settings.EnableLogPreloadLimit = checkBoxEnableLogPreloadLimit.Checked;
            settings.LogPreloadLimit = (ushort)numericUpDownPreloadLimit.Value;
            settings.RestrictForLargeLogs = checkBoxRestrictSomeFeatures.Checked;
            settings.LargeLogsLimit = (ushort)numericUpDownLargeLogsLimit.Value;
            settings.InspectOverlayOpacity = (ushort)numericUpDownOverlayOpacity.Value;
            settings.CheckForUpdates = checkBoxCheckForUpdates.Checked;
            settings.StatusBarNotifOnly = checkBoxStatusBarNotifOnly.Checked;
            settings.NumberFormatInt = comboBoxNumberFormat.SelectedIndex;
            settings.UseDecimalPrefix = comboBoxUnitPrefix.SelectedIndex > 0;
            settings.AutoAdjustRightPaneWidth = checkBoxAutoAdjustRightPaneWidth.Checked;
            settings.AutoLogInAfterInitialLoad = checkBoxAutoLogInAfterInitialLoad.Checked;
            settings.DisplayPromptBeforeClosing = checkBoxDisplayPromptBeforeClosing.Checked;
            settings.EnableBell = checkBoxEnableBell.Checked;
            settings.SortBookmarks = checkBoxSortBookmarks.Checked;
            settings.TruncateBookmarkTitles = checkBoxTruncateBookmarkTitles.Checked;
            settings.TryToKeepUserLoggedIn = checkBoxPingWhenIdle.Checked;
            settings.BlockRequestsToForeignUrls = checkBoxBlockRequestsToForeignUrls.Checked;
            settings.KeepAnEyeOnTheClientsIP = checkBoxKeepAnEyeOnTheClientsIP.Checked;
            settings.IgnoreCertificateErrors = checkBoxIgnoreCertificateErrors.Checked;
            settings.TabsBoldFont = checkBoxBoldFont.Checked;
            settings.TabsBackgroundColor = checkBoxBackgroundColor.Checked;
            settings.TabAppearance = (TabAppearance)comboBoxTabAppearance.SelectedItem;
            settings.StripRenderMode = (ToolStripRenderMode)comboBoxStripRenderMode.SelectedItem;
            settings.Border3DStyle = (Border3DStyle)comboBoxBorderStyle.SelectedItem;
            settings.BookmakerDefaultColor = (Color)comboBoxBookmakerDColor.SelectedItem;
            settings.BookmakerSelectedColor = (Color)comboBoxBookmakerSColor.SelectedItem;
            settings.SportInfo1DefaultColor = (Color)comboBoxSportInfo1DColor.SelectedItem;
            settings.SportInfo1SelectedColor = (Color)comboBoxSportInfo1SColor.SelectedItem;
            settings.SportInfo2DefaultColor = (Color)comboBoxSportInfo2DColor.SelectedItem;
            settings.SportInfo2SelectedColor = (Color)comboBoxSportInfo2SColor.SelectedItem;
            settings.DashboardDefaultColor = (Color)comboBoxDashboardDColor.SelectedItem;
            settings.DashboardSelectedColor = (Color)comboBoxDashboardSColor.SelectedItem;
            settings.CalculatorDefaultColor = (Color)comboBoxCalculatorDColor.SelectedItem;
            settings.CalculatorSelectedColor = (Color)comboBoxCalculatorSColor.SelectedItem;
            settings.OverlayBackgroundColor = (Color)comboBoxOverlayBackgroundColor.SelectedItem;
            settings.OverlayCrosshairColor = (Color)comboBoxOverlayCrosshairColor.SelectedItem;
            settings.DisableThemes = checkBoxDisableThemes.Checked;
            settings.PrintSoftMargins = radioButtonSoftMargins.Checked;
            settings.ActivePreferencesPanel = tabControl.SelectedIndex;

            settings.Save();

            DialogResult = DialogResult.OK;
        }

        private void SetWarning() {
            if ((settings.DisableThemes || settings.RenderWithVisualStyles)
                        && (checkBoxDisableThemes.Checked && Application.RenderWithVisualStyles
                            || !checkBoxDisableThemes.Checked && !Application.RenderWithVisualStyles)
                    || !settings.UserAgent.Equals(comboBoxUserAgent.Text)
                    || !settings.AcceptLanguage.Equals(comboBoxAcceptLanguage.Text)
                    || !settings.EnableDrmContent.Equals(checkBoxEnableDrmContent.Checked)
                    || !settings.EnableAudio.Equals(checkBoxEnableAudio.Checked)
                    || !settings.EnableProxy.Equals(checkBoxEnableProxy.Checked)
                    || !settings.EnablePrintPreview.Equals(checkBoxEnablePrintPreview.Checked)
                    || !settings.EnableCache.Equals(checkBoxEnableCache.Checked)
                    || !settings.PersistSessionCookies.Equals(checkBoxPersistSessionCookies.Checked)
                    || !settings.PersistUserPreferences.Equals(checkBoxPersistUserPreferences.Checked)
                    || !settings.IgnoreCertificateErrors.Equals(checkBoxIgnoreCertificateErrors.Checked)) {

                RestartRequired = true;
                pictureBoxWarning.Visible = true;
                labelWarning.Visible = true;
            } else {
                RestartRequired = false;
                pictureBoxWarning.Visible = false;
                labelWarning.Visible = false;
            }
        }

        private void SetWarning(object sender, EventArgs e) => SetWarning();
    }
}
