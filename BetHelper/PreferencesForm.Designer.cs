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
 * Version 1.1.8.0
 */

namespace BetHelper {
    partial class PreferencesForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.labelWarning = new System.Windows.Forms.Label();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageBrowser = new System.Windows.Forms.TabPage();
            this.groupBoxProxy = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableProxy = new System.Windows.Forms.CheckBox();
            this.groupBoxHeaders = new System.Windows.Forms.GroupBox();
            this.comboBoxAcceptLanguage = new System.Windows.Forms.ComboBox();
            this.labelAcceptLanguage = new System.Windows.Forms.Label();
            this.comboBoxUserAgent = new System.Windows.Forms.ComboBox();
            this.labelUserAgent = new System.Windows.Forms.Label();
            this.groupBoxMultimedia = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableAudio = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableDrmContent = new System.Windows.Forms.CheckBox();
            this.checkBoxEnablePrintPreview = new System.Windows.Forms.CheckBox();
            this.groupBoxCache = new System.Windows.Forms.GroupBox();
            this.checkBoxPersistUserPreferences = new System.Windows.Forms.CheckBox();
            this.checkBoxPersistSessionCookies = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableCache = new System.Windows.Forms.CheckBox();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.groupBoxLogViewer = new System.Windows.Forms.GroupBox();
            this.labelExternalEditor = new System.Windows.Forms.Label();
            this.buttonLogViewer = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxExternalEditor = new System.Windows.Forms.TextBox();
            this.labelMiB = new System.Windows.Forms.Label();
            this.numericUpDownLargeLogsLimit = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRestrictSomeFeatures = new System.Windows.Forms.CheckBox();
            this.labelKiB = new System.Windows.Forms.Label();
            this.numericUpDownPreloadLimit = new System.Windows.Forms.NumericUpDown();
            this.checkBoxEnableLogPreloadLimit = new System.Windows.Forms.CheckBox();
            this.groupBoxJSErrors = new System.Windows.Forms.GroupBox();
            this.checkBoxLogConsoleMessages = new System.Windows.Forms.CheckBox();
            this.checkBoxShowConsoleMessages = new System.Windows.Forms.CheckBox();
            this.groupBoxNavigation = new System.Windows.Forms.GroupBox();
            this.checkBoxLogPopUpFrameHandler = new System.Windows.Forms.CheckBox();
            this.checkBoxLogLoadErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxShowLoadErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxLogForeignUrls = new System.Windows.Forms.CheckBox();
            this.groupBoxDomInspection = new System.Windows.Forms.GroupBox();
            this.comboBoxOverlayCrosshairColor = new System.Windows.Forms.ComboBox();
            this.labelOverlayCrosshairColor = new System.Windows.Forms.Label();
            this.comboBoxOverlayBackgroundColor = new System.Windows.Forms.ComboBox();
            this.labelOverlayBackgroundColor = new System.Windows.Forms.Label();
            this.labelPerCent = new System.Windows.Forms.Label();
            this.numericUpDownOverlayOpacity = new System.Windows.Forms.NumericUpDown();
            this.labelOverlayOpacity = new System.Windows.Forms.Label();
            this.groupBoxFind = new System.Windows.Forms.GroupBox();
            this.checkBoxF3MainFormFind = new System.Windows.Forms.CheckBox();
            this.checkBoxOutlineSearchResults = new System.Windows.Forms.CheckBox();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.buttonEditRemoteConfig = new System.Windows.Forms.Button();
            this.groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this.checkBoxIgnoreCertificateErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockRequestsToForeignUrls = new System.Windows.Forms.CheckBox();
            this.buttonAllowedClientsIP = new System.Windows.Forms.Button();
            this.checkBoxKeepAnEyeOnTheClientsIP = new System.Windows.Forms.CheckBox();
            this.groupBoxPrinting = new System.Windows.Forms.GroupBox();
            this.radioButtonHardMargins = new System.Windows.Forms.RadioButton();
            this.radioButtonSoftMargins = new System.Windows.Forms.RadioButton();
            this.labelPrinting = new System.Windows.Forms.Label();
            this.groupBoxApplication = new System.Windows.Forms.GroupBox();
            this.buttonChime = new System.Windows.Forms.Button();
            this.comboBoxBellSound = new System.Windows.Forms.ComboBox();
            this.labelBellSound = new System.Windows.Forms.Label();
            this.checkBoxBoldErrorBell = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableBell = new System.Windows.Forms.CheckBox();
            this.buttonManageBookmarks = new System.Windows.Forms.Button();
            this.checkBoxTruncateBookmarkTitles = new System.Windows.Forms.CheckBox();
            this.checkBoxSortBookmarks = new System.Windows.Forms.CheckBox();
            this.checkBoxPingWhenIdle = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoLogInAfterInitialLoad = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayPromptBeforeClosing = new System.Windows.Forms.CheckBox();
            this.comboBoxUnitPrefix = new System.Windows.Forms.ComboBox();
            this.labelUnitPrefix = new System.Windows.Forms.Label();
            this.comboBoxNumberFormat = new System.Windows.Forms.ComboBox();
            this.labelNumberFormat = new System.Windows.Forms.Label();
            this.checkBoxStatusBarNotifOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.tabPageUserInterface = new System.Windows.Forms.TabPage();
            this.groupBoxMisc = new System.Windows.Forms.GroupBox();
            this.checkBoxDisableThemes = new System.Windows.Forms.CheckBox();
            this.groupBoxRightPane = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoAdjustRightPaneWidth = new System.Windows.Forms.CheckBox();
            this.groupBoxStatusStrip = new System.Windows.Forms.GroupBox();
            this.comboBoxBorderStyle = new System.Windows.Forms.ComboBox();
            this.labelStripRenderMode = new System.Windows.Forms.Label();
            this.labelBorderStyle = new System.Windows.Forms.Label();
            this.comboBoxStripRenderMode = new System.Windows.Forms.ComboBox();
            this.groupBoxUserInterface = new System.Windows.Forms.GroupBox();
            this.comboBoxCalculatorSColor = new System.Windows.Forms.ComboBox();
            this.labelCalculatorSColor = new System.Windows.Forms.Label();
            this.comboBoxCalculatorDColor = new System.Windows.Forms.ComboBox();
            this.labelCalculatorDColor = new System.Windows.Forms.Label();
            this.comboBoxDashboardSColor = new System.Windows.Forms.ComboBox();
            this.labelDashboardSColor = new System.Windows.Forms.Label();
            this.comboBoxDashboardDColor = new System.Windows.Forms.ComboBox();
            this.labelDashboardDColor = new System.Windows.Forms.Label();
            this.comboBoxSportInfo2SColor = new System.Windows.Forms.ComboBox();
            this.labelSportInfo2SColor = new System.Windows.Forms.Label();
            this.comboBoxSportInfo2DColor = new System.Windows.Forms.ComboBox();
            this.labelSportInfo2DColor = new System.Windows.Forms.Label();
            this.comboBoxSportInfo1SColor = new System.Windows.Forms.ComboBox();
            this.labelSportInfo1SColor = new System.Windows.Forms.Label();
            this.comboBoxSportInfo1DColor = new System.Windows.Forms.ComboBox();
            this.labelSportInfo1DColor = new System.Windows.Forms.Label();
            this.comboBoxBookmakerSColor = new System.Windows.Forms.ComboBox();
            this.labelBookmakerSColor = new System.Windows.Forms.Label();
            this.comboBoxBookmakerDColor = new System.Windows.Forms.ComboBox();
            this.labelBookmakerDColor = new System.Windows.Forms.Label();
            this.checkBoxBackgroundColor = new System.Windows.Forms.CheckBox();
            this.checkBoxBoldFont = new System.Windows.Forms.CheckBox();
            this.comboBoxTabAppearance = new System.Windows.Forms.ComboBox();
            this.labelTabAppearance = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageBrowser.SuspendLayout();
            this.groupBoxProxy.SuspendLayout();
            this.groupBoxHeaders.SuspendLayout();
            this.groupBoxMultimedia.SuspendLayout();
            this.groupBoxCache.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.groupBoxLogViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLargeLogsLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPreloadLimit)).BeginInit();
            this.groupBoxJSErrors.SuspendLayout();
            this.groupBoxNavigation.SuspendLayout();
            this.groupBoxDomInspection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOverlayOpacity)).BeginInit();
            this.groupBoxFind.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxSecurity.SuspendLayout();
            this.groupBoxPrinting.SuspendLayout();
            this.groupBoxApplication.SuspendLayout();
            this.tabPageUserInterface.SuspendLayout();
            this.groupBoxMisc.SuspendLayout();
            this.groupBoxRightPane.SuspendLayout();
            this.groupBoxStatusStrip.SuspendLayout();
            this.groupBoxUserInterface.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelWarning
            // 
            this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelWarning.AutoSize = true;
            this.labelWarning.Location = new System.Drawing.Point(43, 531);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(120, 26);
            this.labelWarning.TabIndex = 1;
            this.labelWarning.Text = "Some changes requires\r\nrestart of the application";
            this.labelWarning.Visible = false;
            // 
            // pictureBoxWarning
            // 
            this.pictureBoxWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxWarning.Location = new System.Drawing.Point(20, 536);
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxWarning.TabIndex = 4;
            this.pictureBoxWarning.TabStop = false;
            this.pictureBoxWarning.Visible = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(244, 535);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.Save);
            this.buttonSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(325, 535);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Canc&el";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageBrowser);
            this.tabControl.Controls.Add(this.tabPageDebug);
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageUserInterface);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(388, 514);
            this.tabControl.TabIndex = 0;
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageBrowser
            // 
            this.tabPageBrowser.Controls.Add(this.groupBoxProxy);
            this.tabPageBrowser.Controls.Add(this.groupBoxHeaders);
            this.tabPageBrowser.Controls.Add(this.groupBoxMultimedia);
            this.tabPageBrowser.Controls.Add(this.groupBoxCache);
            this.tabPageBrowser.Location = new System.Drawing.Point(4, 22);
            this.tabPageBrowser.Name = "tabPageBrowser";
            this.tabPageBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser.Size = new System.Drawing.Size(380, 488);
            this.tabPageBrowser.TabIndex = 0;
            this.tabPageBrowser.Text = "Browser";
            this.tabPageBrowser.UseVisualStyleBackColor = true;
            // 
            // groupBoxProxy
            // 
            this.groupBoxProxy.Controls.Add(this.checkBoxEnableProxy);
            this.groupBoxProxy.Location = new System.Drawing.Point(6, 284);
            this.groupBoxProxy.Name = "groupBoxProxy";
            this.groupBoxProxy.Size = new System.Drawing.Size(368, 198);
            this.groupBoxProxy.TabIndex = 3;
            this.groupBoxProxy.TabStop = false;
            this.groupBoxProxy.Text = "Proxy server";
            // 
            // checkBoxEnableProxy
            // 
            this.checkBoxEnableProxy.AutoSize = true;
            this.checkBoxEnableProxy.Location = new System.Drawing.Point(12, 19);
            this.checkBoxEnableProxy.Name = "checkBoxEnableProxy";
            this.checkBoxEnableProxy.Size = new System.Drawing.Size(119, 17);
            this.checkBoxEnableProxy.TabIndex = 0;
            this.checkBoxEnableProxy.Text = "Enable pro&xy server";
            this.checkBoxEnableProxy.UseVisualStyleBackColor = true;
            this.checkBoxEnableProxy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxHeaders
            // 
            this.groupBoxHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHeaders.Controls.Add(this.comboBoxAcceptLanguage);
            this.groupBoxHeaders.Controls.Add(this.labelAcceptLanguage);
            this.groupBoxHeaders.Controls.Add(this.comboBoxUserAgent);
            this.groupBoxHeaders.Controls.Add(this.labelUserAgent);
            this.groupBoxHeaders.Location = new System.Drawing.Point(6, 6);
            this.groupBoxHeaders.Name = "groupBoxHeaders";
            this.groupBoxHeaders.Size = new System.Drawing.Size(368, 80);
            this.groupBoxHeaders.TabIndex = 0;
            this.groupBoxHeaders.TabStop = false;
            this.groupBoxHeaders.Text = "Headers";
            // 
            // comboBoxAcceptLanguage
            // 
            this.comboBoxAcceptLanguage.FormattingEnabled = true;
            this.comboBoxAcceptLanguage.Location = new System.Drawing.Point(110, 45);
            this.comboBoxAcceptLanguage.Name = "comboBoxAcceptLanguage";
            this.comboBoxAcceptLanguage.Size = new System.Drawing.Size(250, 21);
            this.comboBoxAcceptLanguage.TabIndex = 3;
            this.comboBoxAcceptLanguage.DropDown += new System.EventHandler(this.OnDropDown);
            this.comboBoxAcceptLanguage.SelectedIndexChanged += new System.EventHandler(this.SetWarning);
            this.comboBoxAcceptLanguage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.comboBoxAcceptLanguage.Leave += new System.EventHandler(this.SetWarning);
            this.comboBoxAcceptLanguage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnComboBoxMouseDown);
            // 
            // labelAcceptLanguage
            // 
            this.labelAcceptLanguage.AutoSize = true;
            this.labelAcceptLanguage.Location = new System.Drawing.Point(9, 48);
            this.labelAcceptLanguage.Name = "labelAcceptLanguage";
            this.labelAcceptLanguage.Size = new System.Drawing.Size(95, 13);
            this.labelAcceptLanguage.TabIndex = 2;
            this.labelAcceptLanguage.Text = "Accept-&Language:";
            // 
            // comboBoxUserAgent
            // 
            this.comboBoxUserAgent.FormattingEnabled = true;
            this.comboBoxUserAgent.Location = new System.Drawing.Point(110, 19);
            this.comboBoxUserAgent.Name = "comboBoxUserAgent";
            this.comboBoxUserAgent.Size = new System.Drawing.Size(250, 21);
            this.comboBoxUserAgent.TabIndex = 1;
            this.comboBoxUserAgent.DropDown += new System.EventHandler(this.OnDropDown);
            this.comboBoxUserAgent.SelectedIndexChanged += new System.EventHandler(this.SetWarning);
            this.comboBoxUserAgent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.comboBoxUserAgent.Leave += new System.EventHandler(this.SetWarning);
            this.comboBoxUserAgent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnComboBoxMouseDown);
            // 
            // labelUserAgent
            // 
            this.labelUserAgent.AutoSize = true;
            this.labelUserAgent.Location = new System.Drawing.Point(9, 22);
            this.labelUserAgent.Name = "labelUserAgent";
            this.labelUserAgent.Size = new System.Drawing.Size(63, 13);
            this.labelUserAgent.TabIndex = 0;
            this.labelUserAgent.Text = "&User-Agent:";
            // 
            // groupBoxMultimedia
            // 
            this.groupBoxMultimedia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMultimedia.Controls.Add(this.checkBoxEnableAudio);
            this.groupBoxMultimedia.Controls.Add(this.checkBoxEnableDrmContent);
            this.groupBoxMultimedia.Controls.Add(this.checkBoxEnablePrintPreview);
            this.groupBoxMultimedia.Location = new System.Drawing.Point(6, 188);
            this.groupBoxMultimedia.Name = "groupBoxMultimedia";
            this.groupBoxMultimedia.Size = new System.Drawing.Size(368, 90);
            this.groupBoxMultimedia.TabIndex = 2;
            this.groupBoxMultimedia.TabStop = false;
            this.groupBoxMultimedia.Text = "Multimedia";
            // 
            // checkBoxEnableAudio
            // 
            this.checkBoxEnableAudio.AutoSize = true;
            this.checkBoxEnableAudio.Location = new System.Drawing.Point(12, 65);
            this.checkBoxEnableAudio.Name = "checkBoxEnableAudio";
            this.checkBoxEnableAudio.Size = new System.Drawing.Size(88, 17);
            this.checkBoxEnableAudio.TabIndex = 2;
            this.checkBoxEnableAudio.Text = "Enable &audio";
            this.checkBoxEnableAudio.UseVisualStyleBackColor = true;
            this.checkBoxEnableAudio.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxEnableAudio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxEnableDrmContent
            // 
            this.checkBoxEnableDrmContent.AutoSize = true;
            this.checkBoxEnableDrmContent.Location = new System.Drawing.Point(12, 42);
            this.checkBoxEnableDrmContent.Name = "checkBoxEnableDrmContent";
            this.checkBoxEnableDrmContent.Size = new System.Drawing.Size(194, 17);
            this.checkBoxEnableDrmContent.TabIndex = 1;
            this.checkBoxEnableDrmContent.Text = "Enable &DRM content (experimental)";
            this.checkBoxEnableDrmContent.UseVisualStyleBackColor = true;
            this.checkBoxEnableDrmContent.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxEnableDrmContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxEnablePrintPreview
            // 
            this.checkBoxEnablePrintPreview.AutoSize = true;
            this.checkBoxEnablePrintPreview.Location = new System.Drawing.Point(12, 19);
            this.checkBoxEnablePrintPreview.Name = "checkBoxEnablePrintPreview";
            this.checkBoxEnablePrintPreview.Size = new System.Drawing.Size(122, 17);
            this.checkBoxEnablePrintPreview.TabIndex = 0;
            this.checkBoxEnablePrintPreview.Text = "E&nable print preview";
            this.checkBoxEnablePrintPreview.UseVisualStyleBackColor = true;
            this.checkBoxEnablePrintPreview.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxEnablePrintPreview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxCache
            // 
            this.groupBoxCache.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCache.Controls.Add(this.checkBoxPersistUserPreferences);
            this.groupBoxCache.Controls.Add(this.checkBoxPersistSessionCookies);
            this.groupBoxCache.Controls.Add(this.checkBoxEnableCache);
            this.groupBoxCache.Location = new System.Drawing.Point(6, 92);
            this.groupBoxCache.Name = "groupBoxCache";
            this.groupBoxCache.Size = new System.Drawing.Size(368, 90);
            this.groupBoxCache.TabIndex = 1;
            this.groupBoxCache.TabStop = false;
            this.groupBoxCache.Text = "Cache";
            // 
            // checkBoxPersistUserPreferences
            // 
            this.checkBoxPersistUserPreferences.AutoSize = true;
            this.checkBoxPersistUserPreferences.Location = new System.Drawing.Point(31, 65);
            this.checkBoxPersistUserPreferences.Name = "checkBoxPersistUserPreferences";
            this.checkBoxPersistUserPreferences.Size = new System.Drawing.Size(139, 17);
            this.checkBoxPersistUserPreferences.TabIndex = 2;
            this.checkBoxPersistUserPreferences.Text = "&Persist user preferences";
            this.checkBoxPersistUserPreferences.UseVisualStyleBackColor = true;
            this.checkBoxPersistUserPreferences.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxPersistUserPreferences.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxPersistSessionCookies
            // 
            this.checkBoxPersistSessionCookies.AutoSize = true;
            this.checkBoxPersistSessionCookies.Location = new System.Drawing.Point(31, 42);
            this.checkBoxPersistSessionCookies.Name = "checkBoxPersistSessionCookies";
            this.checkBoxPersistSessionCookies.Size = new System.Drawing.Size(135, 17);
            this.checkBoxPersistSessionCookies.TabIndex = 1;
            this.checkBoxPersistSessionCookies.Text = "Persist session &cookies";
            this.checkBoxPersistSessionCookies.UseVisualStyleBackColor = true;
            this.checkBoxPersistSessionCookies.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxPersistSessionCookies.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxEnableCache
            // 
            this.checkBoxEnableCache.AutoSize = true;
            this.checkBoxEnableCache.Location = new System.Drawing.Point(12, 19);
            this.checkBoxEnableCache.Name = "checkBoxEnableCache";
            this.checkBoxEnableCache.Size = new System.Drawing.Size(92, 17);
            this.checkBoxEnableCache.TabIndex = 0;
            this.checkBoxEnableCache.Text = "Ena&ble cache";
            this.checkBoxEnableCache.UseVisualStyleBackColor = true;
            this.checkBoxEnableCache.CheckedChanged += new System.EventHandler(this.OnEnableCacheCheckedChanged);
            this.checkBoxEnableCache.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.groupBoxLogViewer);
            this.tabPageDebug.Controls.Add(this.groupBoxJSErrors);
            this.tabPageDebug.Controls.Add(this.groupBoxNavigation);
            this.tabPageDebug.Controls.Add(this.groupBoxDomInspection);
            this.tabPageDebug.Controls.Add(this.groupBoxFind);
            this.tabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebug.Size = new System.Drawing.Size(380, 488);
            this.tabPageDebug.TabIndex = 1;
            this.tabPageDebug.Text = "Debug";
            this.tabPageDebug.UseVisualStyleBackColor = true;
            // 
            // groupBoxLogViewer
            // 
            this.groupBoxLogViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLogViewer.Controls.Add(this.labelExternalEditor);
            this.groupBoxLogViewer.Controls.Add(this.buttonLogViewer);
            this.groupBoxLogViewer.Controls.Add(this.buttonBrowse);
            this.groupBoxLogViewer.Controls.Add(this.textBoxExternalEditor);
            this.groupBoxLogViewer.Controls.Add(this.labelMiB);
            this.groupBoxLogViewer.Controls.Add(this.numericUpDownLargeLogsLimit);
            this.groupBoxLogViewer.Controls.Add(this.checkBoxRestrictSomeFeatures);
            this.groupBoxLogViewer.Controls.Add(this.labelKiB);
            this.groupBoxLogViewer.Controls.Add(this.numericUpDownPreloadLimit);
            this.groupBoxLogViewer.Controls.Add(this.checkBoxEnableLogPreloadLimit);
            this.groupBoxLogViewer.Location = new System.Drawing.Point(6, 351);
            this.groupBoxLogViewer.Name = "groupBoxLogViewer";
            this.groupBoxLogViewer.Size = new System.Drawing.Size(368, 131);
            this.groupBoxLogViewer.TabIndex = 4;
            this.groupBoxLogViewer.TabStop = false;
            this.groupBoxLogViewer.Text = "Log Viewer";
            // 
            // labelExternalEditor
            // 
            this.labelExternalEditor.AutoSize = true;
            this.labelExternalEditor.Location = new System.Drawing.Point(9, 63);
            this.labelExternalEditor.Name = "labelExternalEditor";
            this.labelExternalEditor.Size = new System.Drawing.Size(156, 13);
            this.labelExternalEditor.TabIndex = 6;
            this.labelExternalEditor.Text = "External editor executable &path:";
            // 
            // buttonLogViewer
            // 
            this.buttonLogViewer.Location = new System.Drawing.Point(12, 102);
            this.buttonLogViewer.Name = "buttonLogViewer";
            this.buttonLogViewer.Size = new System.Drawing.Size(130, 23);
            this.buttonLogViewer.TabIndex = 9;
            this.buttonLogViewer.Text = "Open &Log Viewer...";
            this.buttonLogViewer.UseVisualStyleBackColor = true;
            this.buttonLogViewer.Click += new System.EventHandler(this.OnLogViewerClick);
            this.buttonLogViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(285, 77);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 8;
            this.buttonBrowse.Text = "Bro&wse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.OnButtonBrowseClick);
            this.buttonBrowse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // textBoxExternalEditor
            // 
            this.textBoxExternalEditor.Location = new System.Drawing.Point(12, 79);
            this.textBoxExternalEditor.Name = "textBoxExternalEditor";
            this.textBoxExternalEditor.Size = new System.Drawing.Size(267, 20);
            this.textBoxExternalEditor.TabIndex = 7;
            this.textBoxExternalEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxExternalEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelMiB
            // 
            this.labelMiB.AutoSize = true;
            this.labelMiB.Location = new System.Drawing.Point(313, 42);
            this.labelMiB.Name = "labelMiB";
            this.labelMiB.Size = new System.Drawing.Size(25, 13);
            this.labelMiB.TabIndex = 5;
            this.labelMiB.Text = "MiB";
            // 
            // numericUpDownLargeLogsLimit
            // 
            this.numericUpDownLargeLogsLimit.Location = new System.Drawing.Point(257, 40);
            this.numericUpDownLargeLogsLimit.Name = "numericUpDownLargeLogsLimit";
            this.numericUpDownLargeLogsLimit.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownLargeLogsLimit.TabIndex = 4;
            this.numericUpDownLargeLogsLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxRestrictSomeFeatures
            // 
            this.checkBoxRestrictSomeFeatures.AutoSize = true;
            this.checkBoxRestrictSomeFeatures.Location = new System.Drawing.Point(12, 41);
            this.checkBoxRestrictSomeFeatures.Name = "checkBoxRestrictSomeFeatures";
            this.checkBoxRestrictSomeFeatures.Size = new System.Drawing.Size(221, 17);
            this.checkBoxRestrictSomeFeatures.TabIndex = 3;
            this.checkBoxRestrictSomeFeatures.Text = "&Restrict some features for logs larger than";
            this.checkBoxRestrictSomeFeatures.UseVisualStyleBackColor = true;
            this.checkBoxRestrictSomeFeatures.CheckedChanged += new System.EventHandler(this.OnRestrictFeaturesCheckedChanged);
            this.checkBoxRestrictSomeFeatures.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelKiB
            // 
            this.labelKiB.AutoSize = true;
            this.labelKiB.Location = new System.Drawing.Point(313, 20);
            this.labelKiB.Name = "labelKiB";
            this.labelKiB.Size = new System.Drawing.Size(23, 13);
            this.labelKiB.TabIndex = 2;
            this.labelKiB.Text = "KiB";
            // 
            // numericUpDownPreloadLimit
            // 
            this.numericUpDownPreloadLimit.Location = new System.Drawing.Point(257, 18);
            this.numericUpDownPreloadLimit.Name = "numericUpDownPreloadLimit";
            this.numericUpDownPreloadLimit.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownPreloadLimit.TabIndex = 1;
            this.numericUpDownPreloadLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxEnableLogPreloadLimit
            // 
            this.checkBoxEnableLogPreloadLimit.AutoSize = true;
            this.checkBoxEnableLogPreloadLimit.Location = new System.Drawing.Point(12, 19);
            this.checkBoxEnableLogPreloadLimit.Name = "checkBoxEnableLogPreloadLimit";
            this.checkBoxEnableLogPreloadLimit.Size = new System.Drawing.Size(180, 17);
            this.checkBoxEnableLogPreloadLimit.TabIndex = 0;
            this.checkBoxEnableLogPreloadLimit.Text = "E&nable log preload maximum limit";
            this.checkBoxEnableLogPreloadLimit.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogPreloadLimit.CheckedChanged += new System.EventHandler(this.OnMaxPreloadCheckedChanged);
            this.checkBoxEnableLogPreloadLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxJSErrors
            // 
            this.groupBoxJSErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxJSErrors.Controls.Add(this.checkBoxLogConsoleMessages);
            this.groupBoxJSErrors.Controls.Add(this.checkBoxShowConsoleMessages);
            this.groupBoxJSErrors.Location = new System.Drawing.Point(6, 285);
            this.groupBoxJSErrors.Name = "groupBoxJSErrors";
            this.groupBoxJSErrors.Size = new System.Drawing.Size(368, 60);
            this.groupBoxJSErrors.TabIndex = 3;
            this.groupBoxJSErrors.TabStop = false;
            this.groupBoxJSErrors.Text = "JavaScript Errors";
            // 
            // checkBoxLogConsoleMessages
            // 
            this.checkBoxLogConsoleMessages.AutoSize = true;
            this.checkBoxLogConsoleMessages.Location = new System.Drawing.Point(12, 38);
            this.checkBoxLogConsoleMessages.Name = "checkBoxLogConsoleMessages";
            this.checkBoxLogConsoleMessages.Size = new System.Drawing.Size(134, 17);
            this.checkBoxLogConsoleMessages.TabIndex = 1;
            this.checkBoxLogConsoleMessages.Text = "Log &console messages";
            this.checkBoxLogConsoleMessages.UseVisualStyleBackColor = true;
            this.checkBoxLogConsoleMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxShowConsoleMessages
            // 
            this.checkBoxShowConsoleMessages.AutoSize = true;
            this.checkBoxShowConsoleMessages.Location = new System.Drawing.Point(12, 19);
            this.checkBoxShowConsoleMessages.Name = "checkBoxShowConsoleMessages";
            this.checkBoxShowConsoleMessages.Size = new System.Drawing.Size(143, 17);
            this.checkBoxShowConsoleMessages.TabIndex = 0;
            this.checkBoxShowConsoleMessages.Text = "Show console &messages";
            this.checkBoxShowConsoleMessages.UseVisualStyleBackColor = true;
            this.checkBoxShowConsoleMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxNavigation
            // 
            this.groupBoxNavigation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNavigation.Controls.Add(this.checkBoxLogPopUpFrameHandler);
            this.groupBoxNavigation.Controls.Add(this.checkBoxLogLoadErrors);
            this.groupBoxNavigation.Controls.Add(this.checkBoxShowLoadErrors);
            this.groupBoxNavigation.Controls.Add(this.checkBoxLogForeignUrls);
            this.groupBoxNavigation.Location = new System.Drawing.Point(6, 168);
            this.groupBoxNavigation.Name = "groupBoxNavigation";
            this.groupBoxNavigation.Size = new System.Drawing.Size(368, 111);
            this.groupBoxNavigation.TabIndex = 2;
            this.groupBoxNavigation.TabStop = false;
            this.groupBoxNavigation.Text = "Navigation";
            // 
            // checkBoxLogPopUpFrameHandler
            // 
            this.checkBoxLogPopUpFrameHandler.AutoSize = true;
            this.checkBoxLogPopUpFrameHandler.Location = new System.Drawing.Point(12, 89);
            this.checkBoxLogPopUpFrameHandler.Name = "checkBoxLogPopUpFrameHandler";
            this.checkBoxLogPopUpFrameHandler.Size = new System.Drawing.Size(162, 17);
            this.checkBoxLogPopUpFrameHandler.TabIndex = 3;
            this.checkBoxLogPopUpFrameHandler.Text = "&PopUpFrameHandler logging";
            this.checkBoxLogPopUpFrameHandler.UseVisualStyleBackColor = true;
            this.checkBoxLogPopUpFrameHandler.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxLogLoadErrors
            // 
            this.checkBoxLogLoadErrors.AutoSize = true;
            this.checkBoxLogLoadErrors.Location = new System.Drawing.Point(12, 70);
            this.checkBoxLogLoadErrors.Name = "checkBoxLogLoadErrors";
            this.checkBoxLogLoadErrors.Size = new System.Drawing.Size(96, 17);
            this.checkBoxLogLoadErrors.TabIndex = 2;
            this.checkBoxLogLoadErrors.Text = "Log lo&ad errors";
            this.checkBoxLogLoadErrors.UseVisualStyleBackColor = true;
            this.checkBoxLogLoadErrors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxShowLoadErrors
            // 
            this.checkBoxShowLoadErrors.AutoSize = true;
            this.checkBoxShowLoadErrors.Location = new System.Drawing.Point(12, 51);
            this.checkBoxShowLoadErrors.Name = "checkBoxShowLoadErrors";
            this.checkBoxShowLoadErrors.Size = new System.Drawing.Size(105, 17);
            this.checkBoxShowLoadErrors.TabIndex = 1;
            this.checkBoxShowLoadErrors.Text = "Show loa&d errors";
            this.checkBoxShowLoadErrors.UseVisualStyleBackColor = true;
            this.checkBoxShowLoadErrors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxLogForeignUrls
            // 
            this.checkBoxLogForeignUrls.AutoSize = true;
            this.checkBoxLogForeignUrls.Location = new System.Drawing.Point(12, 19);
            this.checkBoxLogForeignUrls.Name = "checkBoxLogForeignUrls";
            this.checkBoxLogForeignUrls.Size = new System.Drawing.Size(318, 30);
            this.checkBoxLogForeignUrls.TabIndex = 0;
            this.checkBoxLogForeignUrls.Text = "L&og all requests to URLs not belonging to the URL set by their\r\nsecond-level dom" +
    "ains or the exception list by their hostname.";
            this.checkBoxLogForeignUrls.UseVisualStyleBackColor = true;
            this.checkBoxLogForeignUrls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxDomInspection
            // 
            this.groupBoxDomInspection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDomInspection.Controls.Add(this.comboBoxOverlayCrosshairColor);
            this.groupBoxDomInspection.Controls.Add(this.labelOverlayCrosshairColor);
            this.groupBoxDomInspection.Controls.Add(this.comboBoxOverlayBackgroundColor);
            this.groupBoxDomInspection.Controls.Add(this.labelOverlayBackgroundColor);
            this.groupBoxDomInspection.Controls.Add(this.labelPerCent);
            this.groupBoxDomInspection.Controls.Add(this.numericUpDownOverlayOpacity);
            this.groupBoxDomInspection.Controls.Add(this.labelOverlayOpacity);
            this.groupBoxDomInspection.Location = new System.Drawing.Point(6, 72);
            this.groupBoxDomInspection.Name = "groupBoxDomInspection";
            this.groupBoxDomInspection.Size = new System.Drawing.Size(368, 90);
            this.groupBoxDomInspection.TabIndex = 1;
            this.groupBoxDomInspection.TabStop = false;
            this.groupBoxDomInspection.Text = "DOM Element Inspection";
            // 
            // comboBoxOverlayCrosshairColor
            // 
            this.comboBoxOverlayCrosshairColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOverlayCrosshairColor.FormattingEnabled = true;
            this.comboBoxOverlayCrosshairColor.Location = new System.Drawing.Point(196, 63);
            this.comboBoxOverlayCrosshairColor.Name = "comboBoxOverlayCrosshairColor";
            this.comboBoxOverlayCrosshairColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxOverlayCrosshairColor.TabIndex = 6;
            this.comboBoxOverlayCrosshairColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelOverlayCrosshairColor
            // 
            this.labelOverlayCrosshairColor.AutoSize = true;
            this.labelOverlayCrosshairColor.Location = new System.Drawing.Point(9, 66);
            this.labelOverlayCrosshairColor.Name = "labelOverlayCrosshairColor";
            this.labelOverlayCrosshairColor.Size = new System.Drawing.Size(117, 13);
            this.labelOverlayCrosshairColor.TabIndex = 5;
            this.labelOverlayCrosshairColor.Text = "Overlay cross&hair color:";
            // 
            // comboBoxOverlayBackgroundColor
            // 
            this.comboBoxOverlayBackgroundColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOverlayBackgroundColor.FormattingEnabled = true;
            this.comboBoxOverlayBackgroundColor.Location = new System.Drawing.Point(196, 40);
            this.comboBoxOverlayBackgroundColor.Name = "comboBoxOverlayBackgroundColor";
            this.comboBoxOverlayBackgroundColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxOverlayBackgroundColor.TabIndex = 4;
            this.comboBoxOverlayBackgroundColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelOverlayBackgroundColor
            // 
            this.labelOverlayBackgroundColor.AutoSize = true;
            this.labelOverlayBackgroundColor.Location = new System.Drawing.Point(9, 43);
            this.labelOverlayBackgroundColor.Name = "labelOverlayBackgroundColor";
            this.labelOverlayBackgroundColor.Size = new System.Drawing.Size(132, 13);
            this.labelOverlayBackgroundColor.TabIndex = 3;
            this.labelOverlayBackgroundColor.Text = "Overlay &background color:";
            // 
            // labelPerCent
            // 
            this.labelPerCent.AutoSize = true;
            this.labelPerCent.Location = new System.Drawing.Point(252, 20);
            this.labelPerCent.Name = "labelPerCent";
            this.labelPerCent.Size = new System.Drawing.Size(15, 13);
            this.labelPerCent.TabIndex = 2;
            this.labelPerCent.Text = "%";
            // 
            // numericUpDownOverlayOpacity
            // 
            this.numericUpDownOverlayOpacity.Location = new System.Drawing.Point(196, 18);
            this.numericUpDownOverlayOpacity.Name = "numericUpDownOverlayOpacity";
            this.numericUpDownOverlayOpacity.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownOverlayOpacity.TabIndex = 1;
            this.numericUpDownOverlayOpacity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelOverlayOpacity
            // 
            this.labelOverlayOpacity.AutoSize = true;
            this.labelOverlayOpacity.Location = new System.Drawing.Point(9, 20);
            this.labelOverlayOpacity.Name = "labelOverlayOpacity";
            this.labelOverlayOpacity.Size = new System.Drawing.Size(83, 13);
            this.labelOverlayOpacity.TabIndex = 0;
            this.labelOverlayOpacity.Text = "O&verlay opacity:";
            // 
            // groupBoxFind
            // 
            this.groupBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFind.Controls.Add(this.checkBoxF3MainFormFind);
            this.groupBoxFind.Controls.Add(this.checkBoxOutlineSearchResults);
            this.groupBoxFind.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFind.Name = "groupBoxFind";
            this.groupBoxFind.Size = new System.Drawing.Size(368, 60);
            this.groupBoxFind.TabIndex = 0;
            this.groupBoxFind.TabStop = false;
            this.groupBoxFind.Text = "Finding String in Page";
            // 
            // checkBoxF3MainFormFind
            // 
            this.checkBoxF3MainFormFind.AutoSize = true;
            this.checkBoxF3MainFormFind.Location = new System.Drawing.Point(12, 38);
            this.checkBoxF3MainFormFind.Name = "checkBoxF3MainFormFind";
            this.checkBoxF3MainFormFind.Size = new System.Drawing.Size(160, 17);
            this.checkBoxF3MainFormFind.TabIndex = 1;
            this.checkBoxF3MainFormFind.Text = "&F3 focuses opened find form";
            this.checkBoxF3MainFormFind.UseVisualStyleBackColor = true;
            this.checkBoxF3MainFormFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxOutlineSearchResults
            // 
            this.checkBoxOutlineSearchResults.AutoSize = true;
            this.checkBoxOutlineSearchResults.Location = new System.Drawing.Point(12, 19);
            this.checkBoxOutlineSearchResults.Name = "checkBoxOutlineSearchResults";
            this.checkBoxOutlineSearchResults.Size = new System.Drawing.Size(122, 17);
            this.checkBoxOutlineSearchResults.TabIndex = 0;
            this.checkBoxOutlineSearchResults.Text = "O&utline search result";
            this.checkBoxOutlineSearchResults.UseVisualStyleBackColor = true;
            this.checkBoxOutlineSearchResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.buttonEditRemoteConfig);
            this.tabPageGeneral.Controls.Add(this.groupBoxSecurity);
            this.tabPageGeneral.Controls.Add(this.groupBoxPrinting);
            this.tabPageGeneral.Controls.Add(this.groupBoxApplication);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(380, 488);
            this.tabPageGeneral.TabIndex = 2;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // buttonEditRemoteConfig
            // 
            this.buttonEditRemoteConfig.Location = new System.Drawing.Point(30, 453);
            this.buttonEditRemoteConfig.Name = "buttonEditRemoteConfig";
            this.buttonEditRemoteConfig.Size = new System.Drawing.Size(182, 23);
            this.buttonEditRemoteConfig.TabIndex = 3;
            this.buttonEditRemoteConfig.Text = "Edit &Remote Configuration File...";
            this.buttonEditRemoteConfig.UseVisualStyleBackColor = true;
            this.buttonEditRemoteConfig.Click += new System.EventHandler(this.EditRemoteConfig);
            this.buttonEditRemoteConfig.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxSecurity
            // 
            this.groupBoxSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSecurity.Controls.Add(this.checkBoxIgnoreCertificateErrors);
            this.groupBoxSecurity.Controls.Add(this.checkBoxBlockRequestsToForeignUrls);
            this.groupBoxSecurity.Controls.Add(this.buttonAllowedClientsIP);
            this.groupBoxSecurity.Controls.Add(this.checkBoxKeepAnEyeOnTheClientsIP);
            this.groupBoxSecurity.Location = new System.Drawing.Point(6, 317);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(368, 131);
            this.groupBoxSecurity.TabIndex = 2;
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Security";
            // 
            // checkBoxIgnoreCertificateErrors
            // 
            this.checkBoxIgnoreCertificateErrors.AutoSize = true;
            this.checkBoxIgnoreCertificateErrors.Location = new System.Drawing.Point(12, 109);
            this.checkBoxIgnoreCertificateErrors.Name = "checkBoxIgnoreCertificateErrors";
            this.checkBoxIgnoreCertificateErrors.Size = new System.Drawing.Size(134, 17);
            this.checkBoxIgnoreCertificateErrors.TabIndex = 3;
            this.checkBoxIgnoreCertificateErrors.Text = "&Ignore certificate errors";
            this.checkBoxIgnoreCertificateErrors.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreCertificateErrors.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxIgnoreCertificateErrors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxBlockRequestsToForeignUrls
            // 
            this.checkBoxBlockRequestsToForeignUrls.AutoSize = true;
            this.checkBoxBlockRequestsToForeignUrls.Location = new System.Drawing.Point(12, 78);
            this.checkBoxBlockRequestsToForeignUrls.Name = "checkBoxBlockRequestsToForeignUrls";
            this.checkBoxBlockRequestsToForeignUrls.Size = new System.Drawing.Size(327, 30);
            this.checkBoxBlockRequestsToForeignUrls.TabIndex = 2;
            this.checkBoxBlockRequestsToForeignUrls.Text = "Block all re&quests to URLs not belonging to the URL set by their\r\nsecond-level d" +
    "omains or the exception list by their hostname.";
            this.checkBoxBlockRequestsToForeignUrls.UseVisualStyleBackColor = true;
            this.checkBoxBlockRequestsToForeignUrls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonAllowedClientsIP
            // 
            this.buttonAllowedClientsIP.Location = new System.Drawing.Point(24, 52);
            this.buttonAllowedClientsIP.Name = "buttonAllowedClientsIP";
            this.buttonAllowedClientsIP.Size = new System.Drawing.Size(182, 23);
            this.buttonAllowedClientsIP.TabIndex = 1;
            this.buttonAllowedClientsIP.Text = "Allowed Client\'s I&P Addresses...";
            this.buttonAllowedClientsIP.UseVisualStyleBackColor = true;
            this.buttonAllowedClientsIP.Click += new System.EventHandler(this.OnAllowedClientsClick);
            this.buttonAllowedClientsIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxKeepAnEyeOnTheClientsIP
            // 
            this.checkBoxKeepAnEyeOnTheClientsIP.AutoSize = true;
            this.checkBoxKeepAnEyeOnTheClientsIP.Location = new System.Drawing.Point(12, 19);
            this.checkBoxKeepAnEyeOnTheClientsIP.Name = "checkBoxKeepAnEyeOnTheClientsIP";
            this.checkBoxKeepAnEyeOnTheClientsIP.Size = new System.Drawing.Size(305, 30);
            this.checkBoxKeepAnEyeOnTheClientsIP.TabIndex = 0;
            this.checkBoxKeepAnEyeOnTheClientsIP.Text = "Al&ways keep an eye on the client\'s IP address and block all\r\noutgoing requests i" +
    "f the client\'s IP address changes.";
            this.checkBoxKeepAnEyeOnTheClientsIP.UseVisualStyleBackColor = true;
            this.checkBoxKeepAnEyeOnTheClientsIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxPrinting
            // 
            this.groupBoxPrinting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPrinting.Controls.Add(this.radioButtonHardMargins);
            this.groupBoxPrinting.Controls.Add(this.radioButtonSoftMargins);
            this.groupBoxPrinting.Controls.Add(this.labelPrinting);
            this.groupBoxPrinting.Location = new System.Drawing.Point(6, 236);
            this.groupBoxPrinting.Name = "groupBoxPrinting";
            this.groupBoxPrinting.Size = new System.Drawing.Size(368, 75);
            this.groupBoxPrinting.TabIndex = 1;
            this.groupBoxPrinting.TabStop = false;
            this.groupBoxPrinting.Text = "Printing";
            // 
            // radioButtonHardMargins
            // 
            this.radioButtonHardMargins.AutoSize = true;
            this.radioButtonHardMargins.Location = new System.Drawing.Point(187, 52);
            this.radioButtonHardMargins.Name = "radioButtonHardMargins";
            this.radioButtonHardMargins.Size = new System.Drawing.Size(107, 17);
            this.radioButtonHardMargins.TabIndex = 2;
            this.radioButtonHardMargins.TabStop = true;
            this.radioButtonHardMargins.Text = "Use &hard margins";
            this.radioButtonHardMargins.UseVisualStyleBackColor = true;
            this.radioButtonHardMargins.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // radioButtonSoftMargins
            // 
            this.radioButtonSoftMargins.AutoSize = true;
            this.radioButtonSoftMargins.Location = new System.Drawing.Point(12, 52);
            this.radioButtonSoftMargins.Name = "radioButtonSoftMargins";
            this.radioButtonSoftMargins.Size = new System.Drawing.Size(103, 17);
            this.radioButtonSoftMargins.TabIndex = 1;
            this.radioButtonSoftMargins.TabStop = true;
            this.radioButtonSoftMargins.Text = "&Use soft margins";
            this.radioButtonSoftMargins.UseVisualStyleBackColor = true;
            this.radioButtonSoftMargins.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelPrinting
            // 
            this.labelPrinting.AutoSize = true;
            this.labelPrinting.Location = new System.Drawing.Point(9, 21);
            this.labelPrinting.Name = "labelPrinting";
            this.labelPrinting.Size = new System.Drawing.Size(323, 26);
            this.labelPrinting.TabIndex = 0;
            this.labelPrinting.Text = "This setting applies only when printing screenshots or plain text and\r\nnot when p" +
    "rinting from the embedded Chromium web browser.";
            // 
            // groupBoxApplication
            // 
            this.groupBoxApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxApplication.Controls.Add(this.buttonChime);
            this.groupBoxApplication.Controls.Add(this.comboBoxBellSound);
            this.groupBoxApplication.Controls.Add(this.labelBellSound);
            this.groupBoxApplication.Controls.Add(this.checkBoxBoldErrorBell);
            this.groupBoxApplication.Controls.Add(this.checkBoxEnableBell);
            this.groupBoxApplication.Controls.Add(this.buttonManageBookmarks);
            this.groupBoxApplication.Controls.Add(this.checkBoxTruncateBookmarkTitles);
            this.groupBoxApplication.Controls.Add(this.checkBoxSortBookmarks);
            this.groupBoxApplication.Controls.Add(this.checkBoxPingWhenIdle);
            this.groupBoxApplication.Controls.Add(this.checkBoxAutoLogInAfterInitialLoad);
            this.groupBoxApplication.Controls.Add(this.checkBoxDisplayPromptBeforeClosing);
            this.groupBoxApplication.Controls.Add(this.comboBoxUnitPrefix);
            this.groupBoxApplication.Controls.Add(this.labelUnitPrefix);
            this.groupBoxApplication.Controls.Add(this.comboBoxNumberFormat);
            this.groupBoxApplication.Controls.Add(this.labelNumberFormat);
            this.groupBoxApplication.Controls.Add(this.checkBoxStatusBarNotifOnly);
            this.groupBoxApplication.Controls.Add(this.checkBoxCheckForUpdates);
            this.groupBoxApplication.Location = new System.Drawing.Point(6, 6);
            this.groupBoxApplication.Name = "groupBoxApplication";
            this.groupBoxApplication.Size = new System.Drawing.Size(368, 224);
            this.groupBoxApplication.TabIndex = 0;
            this.groupBoxApplication.TabStop = false;
            this.groupBoxApplication.Text = "Application";
            // 
            // buttonChime
            // 
            this.buttonChime.Location = new System.Drawing.Point(285, 197);
            this.buttonChime.Name = "buttonChime";
            this.buttonChime.Size = new System.Drawing.Size(75, 23);
            this.buttonChime.TabIndex = 16;
            this.buttonChime.Text = "&Chime";
            this.buttonChime.UseVisualStyleBackColor = true;
            this.buttonChime.Click += new System.EventHandler(this.OnChimeClick);
            this.buttonChime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxBellSound
            // 
            this.comboBoxBellSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBellSound.FormattingEnabled = true;
            this.comboBoxBellSound.Location = new System.Drawing.Point(102, 198);
            this.comboBoxBellSound.Name = "comboBoxBellSound";
            this.comboBoxBellSound.Size = new System.Drawing.Size(177, 21);
            this.comboBoxBellSound.TabIndex = 15;
            this.comboBoxBellSound.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelBellSound
            // 
            this.labelBellSound.AutoSize = true;
            this.labelBellSound.Location = new System.Drawing.Point(28, 201);
            this.labelBellSound.Name = "labelBellSound";
            this.labelBellSound.Size = new System.Drawing.Size(59, 13);
            this.labelBellSound.TabIndex = 14;
            this.labelBellSound.Text = "Bell s&ound:";
            // 
            // checkBoxBoldErrorBell
            // 
            this.checkBoxBoldErrorBell.AutoSize = true;
            this.checkBoxBoldErrorBell.Location = new System.Drawing.Point(31, 178);
            this.checkBoxBoldErrorBell.Name = "checkBoxBoldErrorBell";
            this.checkBoxBoldErrorBell.Size = new System.Drawing.Size(121, 17);
            this.checkBoxBoldErrorBell.TabIndex = 13;
            this.checkBoxBoldErrorBell.Text = "Bol&d error bell status";
            this.checkBoxBoldErrorBell.UseVisualStyleBackColor = true;
            this.checkBoxBoldErrorBell.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxEnableBell
            // 
            this.checkBoxEnableBell.AutoSize = true;
            this.checkBoxEnableBell.Location = new System.Drawing.Point(12, 160);
            this.checkBoxEnableBell.Name = "checkBoxEnableBell";
            this.checkBoxEnableBell.Size = new System.Drawing.Size(78, 17);
            this.checkBoxEnableBell.TabIndex = 12;
            this.checkBoxEnableBell.Text = "Ena&ble bell";
            this.checkBoxEnableBell.UseVisualStyleBackColor = true;
            this.checkBoxEnableBell.CheckedChanged += new System.EventHandler(this.OnEnableBellCheckedChanged);
            this.checkBoxEnableBell.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonManageBookmarks
            // 
            this.buttonManageBookmarks.Location = new System.Drawing.Point(187, 165);
            this.buttonManageBookmarks.Name = "buttonManageBookmarks";
            this.buttonManageBookmarks.Size = new System.Drawing.Size(173, 23);
            this.buttonManageBookmarks.TabIndex = 11;
            this.buttonManageBookmarks.Text = "Mana&ge Bookmarks...";
            this.buttonManageBookmarks.UseVisualStyleBackColor = true;
            this.buttonManageBookmarks.Click += new System.EventHandler(this.OnManageBookmarksClick);
            this.buttonManageBookmarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxTruncateBookmarkTitles
            // 
            this.checkBoxTruncateBookmarkTitles.AutoSize = true;
            this.checkBoxTruncateBookmarkTitles.Location = new System.Drawing.Point(187, 142);
            this.checkBoxTruncateBookmarkTitles.Name = "checkBoxTruncateBookmarkTitles";
            this.checkBoxTruncateBookmarkTitles.Size = new System.Drawing.Size(143, 17);
            this.checkBoxTruncateBookmarkTitles.TabIndex = 10;
            this.checkBoxTruncateBookmarkTitles.Text = "&Truncate bookmark titles";
            this.checkBoxTruncateBookmarkTitles.UseVisualStyleBackColor = true;
            this.checkBoxTruncateBookmarkTitles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxSortBookmarks
            // 
            this.checkBoxSortBookmarks.AutoSize = true;
            this.checkBoxSortBookmarks.Location = new System.Drawing.Point(187, 124);
            this.checkBoxSortBookmarks.Name = "checkBoxSortBookmarks";
            this.checkBoxSortBookmarks.Size = new System.Drawing.Size(133, 17);
            this.checkBoxSortBookmarks.TabIndex = 9;
            this.checkBoxSortBookmarks.Text = "Sort book&marks by title";
            this.checkBoxSortBookmarks.UseVisualStyleBackColor = true;
            this.checkBoxSortBookmarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxPingWhenIdle
            // 
            this.checkBoxPingWhenIdle.AutoSize = true;
            this.checkBoxPingWhenIdle.Location = new System.Drawing.Point(12, 142);
            this.checkBoxPingWhenIdle.Name = "checkBoxPingWhenIdle";
            this.checkBoxPingWhenIdle.Size = new System.Drawing.Size(149, 17);
            this.checkBoxPingWhenIdle.TabIndex = 8;
            this.checkBoxPingWhenIdle.Text = "Try to &keep user logged in";
            this.checkBoxPingWhenIdle.UseVisualStyleBackColor = true;
            this.checkBoxPingWhenIdle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxAutoLogInAfterInitialLoad
            // 
            this.checkBoxAutoLogInAfterInitialLoad.AutoSize = true;
            this.checkBoxAutoLogInAfterInitialLoad.Location = new System.Drawing.Point(12, 124);
            this.checkBoxAutoLogInAfterInitialLoad.Name = "checkBoxAutoLogInAfterInitialLoad";
            this.checkBoxAutoLogInAfterInitialLoad.Size = new System.Drawing.Size(149, 17);
            this.checkBoxAutoLogInAfterInitialLoad.TabIndex = 7;
            this.checkBoxAutoLogInAfterInitialLoad.Text = "Auto &log in after initial load";
            this.checkBoxAutoLogInAfterInitialLoad.UseVisualStyleBackColor = true;
            this.checkBoxAutoLogInAfterInitialLoad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxDisplayPromptBeforeClosing
            // 
            this.checkBoxDisplayPromptBeforeClosing.AutoSize = true;
            this.checkBoxDisplayPromptBeforeClosing.Location = new System.Drawing.Point(12, 106);
            this.checkBoxDisplayPromptBeforeClosing.Name = "checkBoxDisplayPromptBeforeClosing";
            this.checkBoxDisplayPromptBeforeClosing.Size = new System.Drawing.Size(282, 17);
            this.checkBoxDisplayPromptBeforeClosing.TabIndex = 6;
            this.checkBoxDisplayPromptBeforeClosing.Text = "Displa&y prompt before closing main application window";
            this.checkBoxDisplayPromptBeforeClosing.UseVisualStyleBackColor = true;
            this.checkBoxDisplayPromptBeforeClosing.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxUnitPrefix
            // 
            this.comboBoxUnitPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnitPrefix.FormattingEnabled = true;
            this.comboBoxUnitPrefix.Location = new System.Drawing.Point(102, 80);
            this.comboBoxUnitPrefix.Name = "comboBoxUnitPrefix";
            this.comboBoxUnitPrefix.Size = new System.Drawing.Size(100, 21);
            this.comboBoxUnitPrefix.TabIndex = 5;
            this.comboBoxUnitPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelUnitPrefix
            // 
            this.labelUnitPrefix.AutoSize = true;
            this.labelUnitPrefix.Location = new System.Drawing.Point(9, 84);
            this.labelUnitPrefix.Name = "labelUnitPrefix";
            this.labelUnitPrefix.Size = new System.Drawing.Size(57, 13);
            this.labelUnitPrefix.TabIndex = 4;
            this.labelUnitPrefix.Text = "Unit prefi&x:";
            // 
            // comboBoxNumberFormat
            // 
            this.comboBoxNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumberFormat.FormattingEnabled = true;
            this.comboBoxNumberFormat.Location = new System.Drawing.Point(102, 57);
            this.comboBoxNumberFormat.Name = "comboBoxNumberFormat";
            this.comboBoxNumberFormat.Size = new System.Drawing.Size(258, 21);
            this.comboBoxNumberFormat.TabIndex = 3;
            this.comboBoxNumberFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelNumberFormat
            // 
            this.labelNumberFormat.AutoSize = true;
            this.labelNumberFormat.Location = new System.Drawing.Point(9, 60);
            this.labelNumberFormat.Name = "labelNumberFormat";
            this.labelNumberFormat.Size = new System.Drawing.Size(79, 13);
            this.labelNumberFormat.TabIndex = 2;
            this.labelNumberFormat.Text = "Number &format:";
            // 
            // checkBoxStatusBarNotifOnly
            // 
            this.checkBoxStatusBarNotifOnly.AutoSize = true;
            this.checkBoxStatusBarNotifOnly.Location = new System.Drawing.Point(31, 37);
            this.checkBoxStatusBarNotifOnly.Name = "checkBoxStatusBarNotifOnly";
            this.checkBoxStatusBarNotifOnly.Size = new System.Drawing.Size(153, 17);
            this.checkBoxStatusBarNotifOnly.TabIndex = 1;
            this.checkBoxStatusBarNotifOnly.Text = "&Notify only in the status bar";
            this.checkBoxStatusBarNotifOnly.UseVisualStyleBackColor = true;
            this.checkBoxStatusBarNotifOnly.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxCheckForUpdates
            // 
            this.checkBoxCheckForUpdates.AutoSize = true;
            this.checkBoxCheckForUpdates.Location = new System.Drawing.Point(12, 19);
            this.checkBoxCheckForUpdates.Name = "checkBoxCheckForUpdates";
            this.checkBoxCheckForUpdates.Size = new System.Drawing.Size(177, 17);
            this.checkBoxCheckForUpdates.TabIndex = 0;
            this.checkBoxCheckForUpdates.Text = "&Automatically check for updates";
            this.checkBoxCheckForUpdates.UseVisualStyleBackColor = true;
            this.checkBoxCheckForUpdates.CheckedChanged += new System.EventHandler(this.OnAutoUpdatesCheckedChanged);
            this.checkBoxCheckForUpdates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageUserInterface
            // 
            this.tabPageUserInterface.Controls.Add(this.groupBoxMisc);
            this.tabPageUserInterface.Controls.Add(this.groupBoxRightPane);
            this.tabPageUserInterface.Controls.Add(this.groupBoxStatusStrip);
            this.tabPageUserInterface.Controls.Add(this.groupBoxUserInterface);
            this.tabPageUserInterface.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserInterface.Name = "tabPageUserInterface";
            this.tabPageUserInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserInterface.Size = new System.Drawing.Size(380, 488);
            this.tabPageUserInterface.TabIndex = 3;
            this.tabPageUserInterface.Text = "User Interface";
            this.tabPageUserInterface.UseVisualStyleBackColor = true;
            // 
            // groupBoxMisc
            // 
            this.groupBoxMisc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMisc.Controls.Add(this.checkBoxDisableThemes);
            this.groupBoxMisc.Location = new System.Drawing.Point(6, 443);
            this.groupBoxMisc.Name = "groupBoxMisc";
            this.groupBoxMisc.Size = new System.Drawing.Size(368, 39);
            this.groupBoxMisc.TabIndex = 3;
            this.groupBoxMisc.TabStop = false;
            this.groupBoxMisc.Text = "Miscellaneous";
            // 
            // checkBoxDisableThemes
            // 
            this.checkBoxDisableThemes.AutoSize = true;
            this.checkBoxDisableThemes.Location = new System.Drawing.Point(12, 17);
            this.checkBoxDisableThemes.Name = "checkBoxDisableThemes";
            this.checkBoxDisableThemes.Size = new System.Drawing.Size(98, 17);
            this.checkBoxDisableThemes.TabIndex = 0;
            this.checkBoxDisableThemes.Text = "&Disable themes";
            this.checkBoxDisableThemes.UseVisualStyleBackColor = true;
            this.checkBoxDisableThemes.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxDisableThemes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxRightPane
            // 
            this.groupBoxRightPane.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRightPane.Controls.Add(this.checkBoxAutoAdjustRightPaneWidth);
            this.groupBoxRightPane.Location = new System.Drawing.Point(6, 398);
            this.groupBoxRightPane.Name = "groupBoxRightPane";
            this.groupBoxRightPane.Size = new System.Drawing.Size(368, 39);
            this.groupBoxRightPane.TabIndex = 2;
            this.groupBoxRightPane.TabStop = false;
            this.groupBoxRightPane.Text = "Right Pane";
            // 
            // checkBoxAutoAdjustRightPaneWidth
            // 
            this.checkBoxAutoAdjustRightPaneWidth.AutoSize = true;
            this.checkBoxAutoAdjustRightPaneWidth.Location = new System.Drawing.Point(12, 17);
            this.checkBoxAutoAdjustRightPaneWidth.Name = "checkBoxAutoAdjustRightPaneWidth";
            this.checkBoxAutoAdjustRightPaneWidth.Size = new System.Drawing.Size(157, 17);
            this.checkBoxAutoAdjustRightPaneWidth.TabIndex = 0;
            this.checkBoxAutoAdjustRightPaneWidth.Text = "Auto adjust right pane &width";
            this.checkBoxAutoAdjustRightPaneWidth.UseVisualStyleBackColor = true;
            this.checkBoxAutoAdjustRightPaneWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxStatusStrip
            // 
            this.groupBoxStatusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStatusStrip.Controls.Add(this.comboBoxBorderStyle);
            this.groupBoxStatusStrip.Controls.Add(this.labelStripRenderMode);
            this.groupBoxStatusStrip.Controls.Add(this.labelBorderStyle);
            this.groupBoxStatusStrip.Controls.Add(this.comboBoxStripRenderMode);
            this.groupBoxStatusStrip.Location = new System.Drawing.Point(6, 326);
            this.groupBoxStatusStrip.Name = "groupBoxStatusStrip";
            this.groupBoxStatusStrip.Size = new System.Drawing.Size(368, 66);
            this.groupBoxStatusStrip.TabIndex = 1;
            this.groupBoxStatusStrip.TabStop = false;
            this.groupBoxStatusStrip.Text = "Status Strip";
            // 
            // comboBoxBorderStyle
            // 
            this.comboBoxBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderStyle.FormattingEnabled = true;
            this.comboBoxBorderStyle.Location = new System.Drawing.Point(196, 40);
            this.comboBoxBorderStyle.Name = "comboBoxBorderStyle";
            this.comboBoxBorderStyle.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBorderStyle.TabIndex = 5;
            this.comboBoxBorderStyle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelStripRenderMode
            // 
            this.labelStripRenderMode.AutoSize = true;
            this.labelStripRenderMode.Location = new System.Drawing.Point(9, 20);
            this.labelStripRenderMode.Name = "labelStripRenderMode";
            this.labelStripRenderMode.Size = new System.Drawing.Size(124, 13);
            this.labelStripRenderMode.TabIndex = 2;
            this.labelStripRenderMode.Text = "Stat&us strip render mode:";
            // 
            // labelBorderStyle
            // 
            this.labelBorderStyle.AutoSize = true;
            this.labelBorderStyle.Location = new System.Drawing.Point(9, 43);
            this.labelBorderStyle.Name = "labelBorderStyle";
            this.labelBorderStyle.Size = new System.Drawing.Size(139, 13);
            this.labelBorderStyle.TabIndex = 4;
            this.labelBorderStyle.Text = "Status &label border 3D style:";
            // 
            // comboBoxStripRenderMode
            // 
            this.comboBoxStripRenderMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStripRenderMode.FormattingEnabled = true;
            this.comboBoxStripRenderMode.Location = new System.Drawing.Point(196, 17);
            this.comboBoxStripRenderMode.Name = "comboBoxStripRenderMode";
            this.comboBoxStripRenderMode.Size = new System.Drawing.Size(164, 21);
            this.comboBoxStripRenderMode.TabIndex = 3;
            this.comboBoxStripRenderMode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxUserInterface
            // 
            this.groupBoxUserInterface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUserInterface.Controls.Add(this.comboBoxCalculatorSColor);
            this.groupBoxUserInterface.Controls.Add(this.labelCalculatorSColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxCalculatorDColor);
            this.groupBoxUserInterface.Controls.Add(this.labelCalculatorDColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxDashboardSColor);
            this.groupBoxUserInterface.Controls.Add(this.labelDashboardSColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxDashboardDColor);
            this.groupBoxUserInterface.Controls.Add(this.labelDashboardDColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxSportInfo2SColor);
            this.groupBoxUserInterface.Controls.Add(this.labelSportInfo2SColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxSportInfo2DColor);
            this.groupBoxUserInterface.Controls.Add(this.labelSportInfo2DColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxSportInfo1SColor);
            this.groupBoxUserInterface.Controls.Add(this.labelSportInfo1SColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxSportInfo1DColor);
            this.groupBoxUserInterface.Controls.Add(this.labelSportInfo1DColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxBookmakerSColor);
            this.groupBoxUserInterface.Controls.Add(this.labelBookmakerSColor);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxBookmakerDColor);
            this.groupBoxUserInterface.Controls.Add(this.labelBookmakerDColor);
            this.groupBoxUserInterface.Controls.Add(this.checkBoxBackgroundColor);
            this.groupBoxUserInterface.Controls.Add(this.checkBoxBoldFont);
            this.groupBoxUserInterface.Controls.Add(this.comboBoxTabAppearance);
            this.groupBoxUserInterface.Controls.Add(this.labelTabAppearance);
            this.groupBoxUserInterface.Location = new System.Drawing.Point(6, 6);
            this.groupBoxUserInterface.Name = "groupBoxUserInterface";
            this.groupBoxUserInterface.Size = new System.Drawing.Size(368, 314);
            this.groupBoxUserInterface.TabIndex = 0;
            this.groupBoxUserInterface.TabStop = false;
            this.groupBoxUserInterface.Text = "Tab Headers in the Main Window";
            // 
            // comboBoxCalculatorSColor
            // 
            this.comboBoxCalculatorSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalculatorSColor.FormattingEnabled = true;
            this.comboBoxCalculatorSColor.Location = new System.Drawing.Point(196, 288);
            this.comboBoxCalculatorSColor.Name = "comboBoxCalculatorSColor";
            this.comboBoxCalculatorSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxCalculatorSColor.TabIndex = 23;
            this.comboBoxCalculatorSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelCalculatorSColor
            // 
            this.labelCalculatorSColor.AutoSize = true;
            this.labelCalculatorSColor.Location = new System.Drawing.Point(9, 291);
            this.labelCalculatorSColor.Name = "labelCalculatorSColor";
            this.labelCalculatorSColor.Size = new System.Drawing.Size(144, 13);
            this.labelCalculatorSColor.TabIndex = 22;
            this.labelCalculatorSColor.Text = "Bet calculato&r selected color:";
            // 
            // comboBoxCalculatorDColor
            // 
            this.comboBoxCalculatorDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalculatorDColor.FormattingEnabled = true;
            this.comboBoxCalculatorDColor.Location = new System.Drawing.Point(196, 265);
            this.comboBoxCalculatorDColor.Name = "comboBoxCalculatorDColor";
            this.comboBoxCalculatorDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxCalculatorDColor.TabIndex = 21;
            this.comboBoxCalculatorDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelCalculatorDColor
            // 
            this.labelCalculatorDColor.AutoSize = true;
            this.labelCalculatorDColor.Location = new System.Drawing.Point(9, 268);
            this.labelCalculatorDColor.Name = "labelCalculatorDColor";
            this.labelCalculatorDColor.Size = new System.Drawing.Size(136, 13);
            this.labelCalculatorDColor.TabIndex = 20;
            this.labelCalculatorDColor.Text = "Bet c&alculator default color:";
            // 
            // comboBoxDashboardSColor
            // 
            this.comboBoxDashboardSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDashboardSColor.FormattingEnabled = true;
            this.comboBoxDashboardSColor.Location = new System.Drawing.Point(196, 242);
            this.comboBoxDashboardSColor.Name = "comboBoxDashboardSColor";
            this.comboBoxDashboardSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxDashboardSColor.TabIndex = 19;
            this.comboBoxDashboardSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelDashboardSColor
            // 
            this.labelDashboardSColor.AutoSize = true;
            this.labelDashboardSColor.Location = new System.Drawing.Point(9, 245);
            this.labelDashboardSColor.Name = "labelDashboardSColor";
            this.labelDashboardSColor.Size = new System.Drawing.Size(171, 13);
            this.labelDashboardSColor.TabIndex = 18;
            this.labelDashboardSColor.Text = "Dashboard and ti&ps selected color:";
            // 
            // comboBoxDashboardDColor
            // 
            this.comboBoxDashboardDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDashboardDColor.FormattingEnabled = true;
            this.comboBoxDashboardDColor.Location = new System.Drawing.Point(196, 219);
            this.comboBoxDashboardDColor.Name = "comboBoxDashboardDColor";
            this.comboBoxDashboardDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxDashboardDColor.TabIndex = 17;
            this.comboBoxDashboardDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelDashboardDColor
            // 
            this.labelDashboardDColor.AutoSize = true;
            this.labelDashboardDColor.Location = new System.Drawing.Point(9, 222);
            this.labelDashboardDColor.Name = "labelDashboardDColor";
            this.labelDashboardDColor.Size = new System.Drawing.Size(163, 13);
            this.labelDashboardDColor.TabIndex = 16;
            this.labelDashboardDColor.Text = "Das&hboard and tips default color:";
            // 
            // comboBoxSportInfo2SColor
            // 
            this.comboBoxSportInfo2SColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo2SColor.FormattingEnabled = true;
            this.comboBoxSportInfo2SColor.Location = new System.Drawing.Point(196, 196);
            this.comboBoxSportInfo2SColor.Name = "comboBoxSportInfo2SColor";
            this.comboBoxSportInfo2SColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo2SColor.TabIndex = 15;
            this.comboBoxSportInfo2SColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo2SColor
            // 
            this.labelSportInfo2SColor.AutoSize = true;
            this.labelSportInfo2SColor.Location = new System.Drawing.Point(9, 199);
            this.labelSportInfo2SColor.Name = "labelSportInfo2SColor";
            this.labelSportInfo2SColor.Size = new System.Drawing.Size(133, 13);
            this.labelSportInfo2SColor.TabIndex = 14;
            this.labelSportInfo2SColor.Text = "Sport info &2 selected color:";
            // 
            // comboBoxSportInfo2DColor
            // 
            this.comboBoxSportInfo2DColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo2DColor.FormattingEnabled = true;
            this.comboBoxSportInfo2DColor.Location = new System.Drawing.Point(196, 173);
            this.comboBoxSportInfo2DColor.Name = "comboBoxSportInfo2DColor";
            this.comboBoxSportInfo2DColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo2DColor.TabIndex = 13;
            this.comboBoxSportInfo2DColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo2DColor
            // 
            this.labelSportInfo2DColor.AutoSize = true;
            this.labelSportInfo2DColor.Location = new System.Drawing.Point(9, 176);
            this.labelSportInfo2DColor.Name = "labelSportInfo2DColor";
            this.labelSportInfo2DColor.Size = new System.Drawing.Size(125, 13);
            this.labelSportInfo2DColor.TabIndex = 12;
            this.labelSportInfo2DColor.Text = "Sport i&nfo 2 default color:";
            // 
            // comboBoxSportInfo1SColor
            // 
            this.comboBoxSportInfo1SColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo1SColor.FormattingEnabled = true;
            this.comboBoxSportInfo1SColor.Location = new System.Drawing.Point(196, 150);
            this.comboBoxSportInfo1SColor.Name = "comboBoxSportInfo1SColor";
            this.comboBoxSportInfo1SColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo1SColor.TabIndex = 11;
            this.comboBoxSportInfo1SColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo1SColor
            // 
            this.labelSportInfo1SColor.AutoSize = true;
            this.labelSportInfo1SColor.Location = new System.Drawing.Point(9, 153);
            this.labelSportInfo1SColor.Name = "labelSportInfo1SColor";
            this.labelSportInfo1SColor.Size = new System.Drawing.Size(133, 13);
            this.labelSportInfo1SColor.TabIndex = 10;
            this.labelSportInfo1SColor.Text = "Sport info &1 selected color:";
            // 
            // comboBoxSportInfo1DColor
            // 
            this.comboBoxSportInfo1DColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo1DColor.FormattingEnabled = true;
            this.comboBoxSportInfo1DColor.Location = new System.Drawing.Point(196, 127);
            this.comboBoxSportInfo1DColor.Name = "comboBoxSportInfo1DColor";
            this.comboBoxSportInfo1DColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo1DColor.TabIndex = 9;
            this.comboBoxSportInfo1DColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo1DColor
            // 
            this.labelSportInfo1DColor.AutoSize = true;
            this.labelSportInfo1DColor.Location = new System.Drawing.Point(9, 130);
            this.labelSportInfo1DColor.Name = "labelSportInfo1DColor";
            this.labelSportInfo1DColor.Size = new System.Drawing.Size(125, 13);
            this.labelSportInfo1DColor.TabIndex = 8;
            this.labelSportInfo1DColor.Text = "Sport &info 1 default color:";
            // 
            // comboBoxBookmakerSColor
            // 
            this.comboBoxBookmakerSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBookmakerSColor.FormattingEnabled = true;
            this.comboBoxBookmakerSColor.Location = new System.Drawing.Point(196, 104);
            this.comboBoxBookmakerSColor.Name = "comboBoxBookmakerSColor";
            this.comboBoxBookmakerSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBookmakerSColor.TabIndex = 7;
            this.comboBoxBookmakerSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelBookmakerSColor
            // 
            this.labelBookmakerSColor.AutoSize = true;
            this.labelBookmakerSColor.Location = new System.Drawing.Point(9, 107);
            this.labelBookmakerSColor.Name = "labelBookmakerSColor";
            this.labelBookmakerSColor.Size = new System.Drawing.Size(133, 13);
            this.labelBookmakerSColor.TabIndex = 6;
            this.labelBookmakerSColor.Text = "Boo&kmaker selected color:";
            // 
            // comboBoxBookmakerDColor
            // 
            this.comboBoxBookmakerDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBookmakerDColor.FormattingEnabled = true;
            this.comboBoxBookmakerDColor.Location = new System.Drawing.Point(196, 81);
            this.comboBoxBookmakerDColor.Name = "comboBoxBookmakerDColor";
            this.comboBoxBookmakerDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBookmakerDColor.TabIndex = 5;
            this.comboBoxBookmakerDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelBookmakerDColor
            // 
            this.labelBookmakerDColor.AutoSize = true;
            this.labelBookmakerDColor.Location = new System.Drawing.Point(9, 84);
            this.labelBookmakerDColor.Name = "labelBookmakerDColor";
            this.labelBookmakerDColor.Size = new System.Drawing.Size(125, 13);
            this.labelBookmakerDColor.TabIndex = 4;
            this.labelBookmakerDColor.Text = "B&ookmaker default color:";
            // 
            // checkBoxBackgroundColor
            // 
            this.checkBoxBackgroundColor.AutoSize = true;
            this.checkBoxBackgroundColor.Location = new System.Drawing.Point(12, 60);
            this.checkBoxBackgroundColor.Name = "checkBoxBackgroundColor";
            this.checkBoxBackgroundColor.Size = new System.Drawing.Size(180, 17);
            this.checkBoxBackgroundColor.TabIndex = 3;
            this.checkBoxBackgroundColor.Text = "Ba&ckground color in tab headers";
            this.checkBoxBackgroundColor.UseVisualStyleBackColor = true;
            this.checkBoxBackgroundColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxBoldFont
            // 
            this.checkBoxBoldFont.AutoSize = true;
            this.checkBoxBoldFont.Location = new System.Drawing.Point(12, 42);
            this.checkBoxBoldFont.Name = "checkBoxBoldFont";
            this.checkBoxBoldFont.Size = new System.Drawing.Size(138, 17);
            this.checkBoxBoldFont.TabIndex = 2;
            this.checkBoxBoldFont.Text = "&Bold font in tab headers";
            this.checkBoxBoldFont.UseVisualStyleBackColor = true;
            this.checkBoxBoldFont.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxTabAppearance
            // 
            this.comboBoxTabAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTabAppearance.FormattingEnabled = true;
            this.comboBoxTabAppearance.Location = new System.Drawing.Point(196, 17);
            this.comboBoxTabAppearance.Name = "comboBoxTabAppearance";
            this.comboBoxTabAppearance.Size = new System.Drawing.Size(164, 21);
            this.comboBoxTabAppearance.TabIndex = 1;
            this.comboBoxTabAppearance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelTabAppearance
            // 
            this.labelTabAppearance.AutoSize = true;
            this.labelTabAppearance.Location = new System.Drawing.Point(9, 20);
            this.labelTabAppearance.Name = "labelTabAppearance";
            this.labelTabAppearance.Size = new System.Drawing.Size(130, 13);
            this.labelTabAppearance.TabIndex = 0;
            this.labelTabAppearance.Text = "&Tab headers appearance:";
            // 
            // PreferencesForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(412, 570);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.pictureBoxWarning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageBrowser.ResumeLayout(false);
            this.groupBoxProxy.ResumeLayout(false);
            this.groupBoxProxy.PerformLayout();
            this.groupBoxHeaders.ResumeLayout(false);
            this.groupBoxHeaders.PerformLayout();
            this.groupBoxMultimedia.ResumeLayout(false);
            this.groupBoxMultimedia.PerformLayout();
            this.groupBoxCache.ResumeLayout(false);
            this.groupBoxCache.PerformLayout();
            this.tabPageDebug.ResumeLayout(false);
            this.groupBoxLogViewer.ResumeLayout(false);
            this.groupBoxLogViewer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLargeLogsLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPreloadLimit)).EndInit();
            this.groupBoxJSErrors.ResumeLayout(false);
            this.groupBoxJSErrors.PerformLayout();
            this.groupBoxNavigation.ResumeLayout(false);
            this.groupBoxNavigation.PerformLayout();
            this.groupBoxDomInspection.ResumeLayout(false);
            this.groupBoxDomInspection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOverlayOpacity)).EndInit();
            this.groupBoxFind.ResumeLayout(false);
            this.groupBoxFind.PerformLayout();
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBoxSecurity.ResumeLayout(false);
            this.groupBoxSecurity.PerformLayout();
            this.groupBoxPrinting.ResumeLayout(false);
            this.groupBoxPrinting.PerformLayout();
            this.groupBoxApplication.ResumeLayout(false);
            this.groupBoxApplication.PerformLayout();
            this.tabPageUserInterface.ResumeLayout(false);
            this.groupBoxMisc.ResumeLayout(false);
            this.groupBoxMisc.PerformLayout();
            this.groupBoxRightPane.ResumeLayout(false);
            this.groupBoxRightPane.PerformLayout();
            this.groupBoxStatusStrip.ResumeLayout(false);
            this.groupBoxStatusStrip.PerformLayout();
            this.groupBoxUserInterface.ResumeLayout(false);
            this.groupBoxUserInterface.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageBrowser;
        private System.Windows.Forms.GroupBox groupBoxMultimedia;
        private System.Windows.Forms.CheckBox checkBoxPersistSessionCookies;
        private System.Windows.Forms.CheckBox checkBoxPersistUserPreferences;
        private System.Windows.Forms.CheckBox checkBoxEnablePrintPreview;
        private System.Windows.Forms.GroupBox groupBoxCache;
        private System.Windows.Forms.CheckBox checkBoxEnableCache;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.CheckBox checkBoxEnableAudio;
        private System.Windows.Forms.CheckBox checkBoxEnableDrmContent;
        private System.Windows.Forms.GroupBox groupBoxHeaders;
        private System.Windows.Forms.ComboBox comboBoxAcceptLanguage;
        private System.Windows.Forms.Label labelAcceptLanguage;
        private System.Windows.Forms.ComboBox comboBoxUserAgent;
        private System.Windows.Forms.Label labelUserAgent;
        private System.Windows.Forms.GroupBox groupBoxNavigation;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.GroupBox groupBoxFind;
        private System.Windows.Forms.CheckBox checkBoxLogPopUpFrameHandler;
        private System.Windows.Forms.CheckBox checkBoxLogForeignUrls;
        private System.Windows.Forms.CheckBox checkBoxOutlineSearchResults;
        private System.Windows.Forms.GroupBox groupBoxPrinting;
        private System.Windows.Forms.RadioButton radioButtonHardMargins;
        private System.Windows.Forms.RadioButton radioButtonSoftMargins;
        private System.Windows.Forms.GroupBox groupBoxApplication;
        private System.Windows.Forms.ComboBox comboBoxNumberFormat;
        private System.Windows.Forms.Label labelNumberFormat;
        private System.Windows.Forms.CheckBox checkBoxStatusBarNotifOnly;
        private System.Windows.Forms.CheckBox checkBoxCheckForUpdates;
        private System.Windows.Forms.TabPage tabPageUserInterface;
        private System.Windows.Forms.GroupBox groupBoxStatusStrip;
        private System.Windows.Forms.ComboBox comboBoxTabAppearance;
        private System.Windows.Forms.Label labelTabAppearance;
        private System.Windows.Forms.ComboBox comboBoxBorderStyle;
        private System.Windows.Forms.Label labelStripRenderMode;
        private System.Windows.Forms.Label labelBorderStyle;
        private System.Windows.Forms.ComboBox comboBoxStripRenderMode;
        private System.Windows.Forms.GroupBox groupBoxUserInterface;
        private System.Windows.Forms.CheckBox checkBoxBackgroundColor;
        private System.Windows.Forms.CheckBox checkBoxBoldFont;
        private System.Windows.Forms.Label labelPrinting;
        private System.Windows.Forms.ComboBox comboBoxDashboardSColor;
        private System.Windows.Forms.Label labelDashboardSColor;
        private System.Windows.Forms.ComboBox comboBoxDashboardDColor;
        private System.Windows.Forms.Label labelDashboardDColor;
        private System.Windows.Forms.ComboBox comboBoxSportInfo2SColor;
        private System.Windows.Forms.Label labelSportInfo2SColor;
        private System.Windows.Forms.ComboBox comboBoxSportInfo2DColor;
        private System.Windows.Forms.Label labelSportInfo2DColor;
        private System.Windows.Forms.ComboBox comboBoxSportInfo1SColor;
        private System.Windows.Forms.Label labelSportInfo1SColor;
        private System.Windows.Forms.ComboBox comboBoxSportInfo1DColor;
        private System.Windows.Forms.Label labelSportInfo1DColor;
        private System.Windows.Forms.ComboBox comboBoxBookmakerSColor;
        private System.Windows.Forms.Label labelBookmakerSColor;
        private System.Windows.Forms.ComboBox comboBoxBookmakerDColor;
        private System.Windows.Forms.Label labelBookmakerDColor;
        private System.Windows.Forms.GroupBox groupBoxRightPane;
        private System.Windows.Forms.CheckBox checkBoxAutoAdjustRightPaneWidth;
        private System.Windows.Forms.GroupBox groupBoxMisc;
        private System.Windows.Forms.CheckBox checkBoxDisableThemes;
        private System.Windows.Forms.CheckBox checkBoxAutoLogInAfterInitialLoad;
        private System.Windows.Forms.CheckBox checkBoxDisplayPromptBeforeClosing;
        private System.Windows.Forms.GroupBox groupBoxSecurity;
        private System.Windows.Forms.CheckBox checkBoxKeepAnEyeOnTheClientsIP;
        private System.Windows.Forms.CheckBox checkBoxBlockRequestsToForeignUrls;
        private System.Windows.Forms.CheckBox checkBoxIgnoreCertificateErrors;
        private System.Windows.Forms.GroupBox groupBoxLogViewer;
        private System.Windows.Forms.GroupBox groupBoxJSErrors;
        private System.Windows.Forms.CheckBox checkBoxLogConsoleMessages;
        private System.Windows.Forms.CheckBox checkBoxShowConsoleMessages;
        private System.Windows.Forms.CheckBox checkBoxLogLoadErrors;
        private System.Windows.Forms.CheckBox checkBoxShowLoadErrors;
        private System.Windows.Forms.Label labelKiB;
        private System.Windows.Forms.NumericUpDown numericUpDownPreloadLimit;
        private System.Windows.Forms.CheckBox checkBoxEnableLogPreloadLimit;
        private System.Windows.Forms.ComboBox comboBoxUnitPrefix;
        private System.Windows.Forms.Label labelUnitPrefix;
        private System.Windows.Forms.Button buttonEditRemoteConfig;
        private System.Windows.Forms.Button buttonAllowedClientsIP;
        private System.Windows.Forms.ComboBox comboBoxCalculatorSColor;
        private System.Windows.Forms.Label labelCalculatorSColor;
        private System.Windows.Forms.ComboBox comboBoxCalculatorDColor;
        private System.Windows.Forms.Label labelCalculatorDColor;
        private System.Windows.Forms.CheckBox checkBoxTruncateBookmarkTitles;
        private System.Windows.Forms.GroupBox groupBoxDomInspection;
        private System.Windows.Forms.ComboBox comboBoxOverlayCrosshairColor;
        private System.Windows.Forms.Label labelOverlayCrosshairColor;
        private System.Windows.Forms.ComboBox comboBoxOverlayBackgroundColor;
        private System.Windows.Forms.Label labelOverlayBackgroundColor;
        private System.Windows.Forms.Label labelPerCent;
        private System.Windows.Forms.NumericUpDown numericUpDownOverlayOpacity;
        private System.Windows.Forms.Label labelOverlayOpacity;
        private System.Windows.Forms.Button buttonLogViewer;
        private System.Windows.Forms.CheckBox checkBoxSortBookmarks;
        private System.Windows.Forms.Label labelMiB;
        private System.Windows.Forms.NumericUpDown numericUpDownLargeLogsLimit;
        private System.Windows.Forms.CheckBox checkBoxRestrictSomeFeatures;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxExternalEditor;
        private System.Windows.Forms.Label labelExternalEditor;
        private System.Windows.Forms.CheckBox checkBoxPingWhenIdle;
        private System.Windows.Forms.CheckBox checkBoxEnableBell;
        private System.Windows.Forms.GroupBox groupBoxProxy;
        private System.Windows.Forms.CheckBox checkBoxEnableProxy;
        private System.Windows.Forms.CheckBox checkBoxF3MainFormFind;
        private System.Windows.Forms.Button buttonManageBookmarks;
        private System.Windows.Forms.CheckBox checkBoxBoldErrorBell;
        private System.Windows.Forms.ComboBox comboBoxBellSound;
        private System.Windows.Forms.Label labelBellSound;
        private System.Windows.Forms.Button buttonChime;
    }
}