/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022-2026 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.18.2
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
            this.tabPageBrowser1 = new System.Windows.Forms.TabPage();
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
            this.tabPageBrowser2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.panelProxy = new System.Windows.Forms.Panel();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.groupBoxLogViewer = new System.Windows.Forms.GroupBox();
            this.buttonLogViewer = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxExternalEditor = new System.Windows.Forms.TextBox();
            this.labelExternalEditor = new System.Windows.Forms.Label();
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
            this.tabPageGeneral1 = new System.Windows.Forms.TabPage();
            this.groupBoxConfiguration = new System.Windows.Forms.GroupBox();
            this.buttonEditRemoteConfig = new System.Windows.Forms.Button();
            this.groupBoxBookmarks = new System.Windows.Forms.GroupBox();
            this.buttonManageBookmarks = new System.Windows.Forms.Button();
            this.checkBoxSortBookmarks = new System.Windows.Forms.CheckBox();
            this.checkBoxTruncateBookmarkTitles = new System.Windows.Forms.CheckBox();
            this.groupBoxApplication = new System.Windows.Forms.GroupBox();
            this.checkBoxPingWhenIdle = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoLogInAfterInitialLoad = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayPromptBeforeClosing = new System.Windows.Forms.CheckBox();
            this.comboBoxUnitPrefix = new System.Windows.Forms.ComboBox();
            this.labelUnitPrefix = new System.Windows.Forms.Label();
            this.comboBoxNumberFormat = new System.Windows.Forms.ComboBox();
            this.labelNumberFormat = new System.Windows.Forms.Label();
            this.checkBoxStatusBarNotifOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.groupBoxBell = new System.Windows.Forms.GroupBox();
            this.buttonFOChime = new System.Windows.Forms.Button();
            this.comboBoxFOSound = new System.Windows.Forms.ComboBox();
            this.labelFOSound = new System.Windows.Forms.Label();
            this.checkBoxEnableFOBell = new System.Windows.Forms.CheckBox();
            this.buttonNTChime = new System.Windows.Forms.Button();
            this.comboBoxNTSound = new System.Windows.Forms.ComboBox();
            this.labelNTSound = new System.Windows.Forms.Label();
            this.checkBoxEnableNTBell = new System.Windows.Forms.CheckBox();
            this.checkBoxBoldErrorBell = new System.Windows.Forms.CheckBox();
            this.tabPageGeneral2 = new System.Windows.Forms.TabPage();
            this.groupBoxCountDown = new System.Windows.Forms.GroupBox();
            this.labelSecond = new System.Windows.Forms.Label();
            this.numericUpDownSecond = new System.Windows.Forms.NumericUpDown();
            this.labelCountDown = new System.Windows.Forms.Label();
            this.groupBoxPrinting = new System.Windows.Forms.GroupBox();
            this.radioButtonHardMargins = new System.Windows.Forms.RadioButton();
            this.radioButtonSoftMargins = new System.Windows.Forms.RadioButton();
            this.labelPrinting = new System.Windows.Forms.Label();
            this.groupBoxFind = new System.Windows.Forms.GroupBox();
            this.checkBoxF3MainFormFind = new System.Windows.Forms.CheckBox();
            this.checkBoxOutlineSearchResults = new System.Windows.Forms.CheckBox();
            this.groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this.checkBoxIgnoreCertificateErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockRequestsToForeignUrls = new System.Windows.Forms.CheckBox();
            this.buttonAllowedClientsIP = new System.Windows.Forms.Button();
            this.checkBoxKeepAnEyeOnTheClientsIP = new System.Windows.Forms.CheckBox();
            this.tabPageUserInterface = new System.Windows.Forms.TabPage();
            this.groupBoxMisc = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoAdjustRightPaneWidth = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableThemes = new System.Windows.Forms.CheckBox();
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
            this.groupBoxStatusStrip = new System.Windows.Forms.GroupBox();
            this.comboBoxBorderStyle = new System.Windows.Forms.ComboBox();
            this.labelStripRenderMode = new System.Windows.Forms.Label();
            this.labelBorderStyle = new System.Windows.Forms.Label();
            this.comboBoxStripRenderMode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageBrowser1.SuspendLayout();
            this.groupBoxHeaders.SuspendLayout();
            this.groupBoxMultimedia.SuspendLayout();
            this.groupBoxCache.SuspendLayout();
            this.tabPageBrowser2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.tabPageDebug.SuspendLayout();
            this.groupBoxLogViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLargeLogsLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPreloadLimit)).BeginInit();
            this.groupBoxJSErrors.SuspendLayout();
            this.groupBoxNavigation.SuspendLayout();
            this.groupBoxDomInspection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOverlayOpacity)).BeginInit();
            this.tabPageGeneral1.SuspendLayout();
            this.groupBoxConfiguration.SuspendLayout();
            this.groupBoxBookmarks.SuspendLayout();
            this.groupBoxApplication.SuspendLayout();
            this.groupBoxBell.SuspendLayout();
            this.tabPageGeneral2.SuspendLayout();
            this.groupBoxCountDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSecond)).BeginInit();
            this.groupBoxPrinting.SuspendLayout();
            this.groupBoxFind.SuspendLayout();
            this.groupBoxSecurity.SuspendLayout();
            this.tabPageUserInterface.SuspendLayout();
            this.groupBoxMisc.SuspendLayout();
            this.groupBoxUserInterface.SuspendLayout();
            this.groupBoxStatusStrip.SuspendLayout();
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
            this.tabControl.Controls.Add(this.tabPageBrowser1);
            this.tabControl.Controls.Add(this.tabPageBrowser2);
            this.tabControl.Controls.Add(this.tabPageDebug);
            this.tabControl.Controls.Add(this.tabPageGeneral1);
            this.tabControl.Controls.Add(this.tabPageGeneral2);
            this.tabControl.Controls.Add(this.tabPageUserInterface);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(388, 514);
            this.tabControl.TabIndex = 0;
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageBrowser1
            // 
            this.tabPageBrowser1.Controls.Add(this.groupBoxHeaders);
            this.tabPageBrowser1.Controls.Add(this.groupBoxMultimedia);
            this.tabPageBrowser1.Controls.Add(this.groupBoxCache);
            this.tabPageBrowser1.Location = new System.Drawing.Point(4, 22);
            this.tabPageBrowser1.Name = "tabPageBrowser1";
            this.tabPageBrowser1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser1.Size = new System.Drawing.Size(380, 488);
            this.tabPageBrowser1.TabIndex = 0;
            this.tabPageBrowser1.Text = "Browser (1)";
            this.tabPageBrowser1.UseVisualStyleBackColor = true;
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
            this.comboBoxAcceptLanguage.Location = new System.Drawing.Point(110, 46);
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
            this.labelAcceptLanguage.Location = new System.Drawing.Point(9, 49);
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
            // tabPageBrowser2
            // 
            this.tabPageBrowser2.Controls.Add(this.groupBox1);
            this.tabPageBrowser2.Location = new System.Drawing.Point(4, 22);
            this.tabPageBrowser2.Name = "tabPageBrowser2";
            this.tabPageBrowser2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser2.Size = new System.Drawing.Size(380, 488);
            this.tabPageBrowser2.TabIndex = 5;
            this.tabPageBrowser2.Text = "Browser (2)";
            this.tabPageBrowser2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.radioButton6);
            this.groupBox1.Controls.Add(this.panelProxy);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 368);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Proxy settings";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(8, 314);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox6.Size = new System.Drawing.Size(352, 45);
            this.textBox6.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 298);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "&No proxy for:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(41, 267);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(319, 20);
            this.textBox5.TabIndex = 5;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(12, 244);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(192, 17);
            this.radioButton6.TabIndex = 4;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "&Automatic proxy configuration URL:";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // panelProxy
            // 
            this.panelProxy.Controls.Add(this.radioButton5);
            this.panelProxy.Controls.Add(this.radioButton4);
            this.panelProxy.Controls.Add(this.textBox1);
            this.panelProxy.Controls.Add(this.numericUpDown4);
            this.panelProxy.Controls.Add(this.checkBox1);
            this.panelProxy.Controls.Add(this.label1);
            this.panelProxy.Controls.Add(this.label8);
            this.panelProxy.Controls.Add(this.label2);
            this.panelProxy.Controls.Add(this.textBox4);
            this.panelProxy.Controls.Add(this.numericUpDown1);
            this.panelProxy.Controls.Add(this.label7);
            this.panelProxy.Controls.Add(this.label3);
            this.panelProxy.Controls.Add(this.numericUpDown3);
            this.panelProxy.Controls.Add(this.textBox2);
            this.panelProxy.Controls.Add(this.label6);
            this.panelProxy.Controls.Add(this.label4);
            this.panelProxy.Controls.Add(this.textBox3);
            this.panelProxy.Controls.Add(this.numericUpDown2);
            this.panelProxy.Controls.Add(this.label5);
            this.panelProxy.Location = new System.Drawing.Point(3, 88);
            this.panelProxy.Name = "panelProxy";
            this.panelProxy.Size = new System.Drawing.Size(362, 150);
            this.panelProxy.TabIndex = 3;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(195, 129);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(76, 17);
            this.radioButton5.TabIndex = 18;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "SOCKS &v5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(103, 129);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(76, 17);
            this.radioButton4.TabIndex = 17;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "SOC&KS v4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 20);
            this.textBox1.TabIndex = 1;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(292, 103);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown4.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(103, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(198, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Use th&is proxy server for all protocols";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "HTTP pro&xy:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(257, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Por&t:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Port:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(103, 103);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(142, 20);
            this.textBox4.TabIndex = 14;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(292, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "SO&CKS host:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&HTTPS proxy:";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(292, 77);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown3.TabIndex = 12;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(103, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(142, 20);
            this.textBox2.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(257, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Po&rt:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "P&ort:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(103, 77);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(142, 20);
            this.textBox3.TabIndex = 10;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(292, 51);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "&FTP proxy:";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(12, 65);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(155, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "&Manual proxy configuration:";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(146, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "&Use system proxy settings";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(67, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "No prox&y";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.groupBoxLogViewer);
            this.tabPageDebug.Controls.Add(this.groupBoxJSErrors);
            this.tabPageDebug.Controls.Add(this.groupBoxNavigation);
            this.tabPageDebug.Controls.Add(this.groupBoxDomInspection);
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
            this.groupBoxLogViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLogViewer.Controls.Add(this.buttonLogViewer);
            this.groupBoxLogViewer.Controls.Add(this.buttonBrowse);
            this.groupBoxLogViewer.Controls.Add(this.textBoxExternalEditor);
            this.groupBoxLogViewer.Controls.Add(this.labelExternalEditor);
            this.groupBoxLogViewer.Controls.Add(this.labelMiB);
            this.groupBoxLogViewer.Controls.Add(this.numericUpDownLargeLogsLimit);
            this.groupBoxLogViewer.Controls.Add(this.checkBoxRestrictSomeFeatures);
            this.groupBoxLogViewer.Controls.Add(this.labelKiB);
            this.groupBoxLogViewer.Controls.Add(this.numericUpDownPreloadLimit);
            this.groupBoxLogViewer.Controls.Add(this.checkBoxEnableLogPreloadLimit);
            this.groupBoxLogViewer.Location = new System.Drawing.Point(6, 329);
            this.groupBoxLogViewer.Name = "groupBoxLogViewer";
            this.groupBoxLogViewer.Size = new System.Drawing.Size(368, 153);
            this.groupBoxLogViewer.TabIndex = 4;
            this.groupBoxLogViewer.TabStop = false;
            this.groupBoxLogViewer.Text = "Log Viewer";
            // 
            // buttonLogViewer
            // 
            this.buttonLogViewer.Location = new System.Drawing.Point(89, 122);
            this.buttonLogViewer.Name = "buttonLogViewer";
            this.buttonLogViewer.Size = new System.Drawing.Size(190, 23);
            this.buttonLogViewer.TabIndex = 9;
            this.buttonLogViewer.Text = "Open &Log Viewer...";
            this.buttonLogViewer.UseVisualStyleBackColor = true;
            this.buttonLogViewer.Click += new System.EventHandler(this.OnLogViewerClick);
            this.buttonLogViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(285, 85);
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
            this.textBoxExternalEditor.Location = new System.Drawing.Point(12, 87);
            this.textBoxExternalEditor.Name = "textBoxExternalEditor";
            this.textBoxExternalEditor.Size = new System.Drawing.Size(267, 20);
            this.textBoxExternalEditor.TabIndex = 7;
            this.textBoxExternalEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxExternalEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelExternalEditor
            // 
            this.labelExternalEditor.AutoSize = true;
            this.labelExternalEditor.Location = new System.Drawing.Point(9, 71);
            this.labelExternalEditor.Name = "labelExternalEditor";
            this.labelExternalEditor.Size = new System.Drawing.Size(156, 13);
            this.labelExternalEditor.TabIndex = 6;
            this.labelExternalEditor.Text = "E&xternal editor executable path:";
            // 
            // labelMiB
            // 
            this.labelMiB.AutoSize = true;
            this.labelMiB.Location = new System.Drawing.Point(313, 46);
            this.labelMiB.Name = "labelMiB";
            this.labelMiB.Size = new System.Drawing.Size(25, 13);
            this.labelMiB.TabIndex = 5;
            this.labelMiB.Text = "MiB";
            // 
            // numericUpDownLargeLogsLimit
            // 
            this.numericUpDownLargeLogsLimit.Location = new System.Drawing.Point(257, 44);
            this.numericUpDownLargeLogsLimit.Name = "numericUpDownLargeLogsLimit";
            this.numericUpDownLargeLogsLimit.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownLargeLogsLimit.TabIndex = 4;
            this.numericUpDownLargeLogsLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxRestrictSomeFeatures
            // 
            this.checkBoxRestrictSomeFeatures.AutoSize = true;
            this.checkBoxRestrictSomeFeatures.Location = new System.Drawing.Point(12, 45);
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
            this.groupBoxJSErrors.Location = new System.Drawing.Point(6, 253);
            this.groupBoxJSErrors.Name = "groupBoxJSErrors";
            this.groupBoxJSErrors.Size = new System.Drawing.Size(368, 70);
            this.groupBoxJSErrors.TabIndex = 3;
            this.groupBoxJSErrors.TabStop = false;
            this.groupBoxJSErrors.Text = "JavaScript Errors";
            // 
            // checkBoxLogConsoleMessages
            // 
            this.checkBoxLogConsoleMessages.AutoSize = true;
            this.checkBoxLogConsoleMessages.Location = new System.Drawing.Point(12, 42);
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
            this.groupBoxNavigation.Location = new System.Drawing.Point(6, 117);
            this.groupBoxNavigation.Name = "groupBoxNavigation";
            this.groupBoxNavigation.Size = new System.Drawing.Size(368, 130);
            this.groupBoxNavigation.TabIndex = 2;
            this.groupBoxNavigation.TabStop = false;
            this.groupBoxNavigation.Text = "Navigation";
            // 
            // checkBoxLogPopUpFrameHandler
            // 
            this.checkBoxLogPopUpFrameHandler.AutoSize = true;
            this.checkBoxLogPopUpFrameHandler.Location = new System.Drawing.Point(12, 101);
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
            this.checkBoxLogLoadErrors.Location = new System.Drawing.Point(12, 78);
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
            this.checkBoxShowLoadErrors.Location = new System.Drawing.Point(12, 55);
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
            this.groupBoxDomInspection.Location = new System.Drawing.Point(6, 6);
            this.groupBoxDomInspection.Name = "groupBoxDomInspection";
            this.groupBoxDomInspection.Size = new System.Drawing.Size(368, 105);
            this.groupBoxDomInspection.TabIndex = 1;
            this.groupBoxDomInspection.TabStop = false;
            this.groupBoxDomInspection.Text = "DOM Element Inspection";
            // 
            // comboBoxOverlayCrosshairColor
            // 
            this.comboBoxOverlayCrosshairColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOverlayCrosshairColor.FormattingEnabled = true;
            this.comboBoxOverlayCrosshairColor.Location = new System.Drawing.Point(196, 71);
            this.comboBoxOverlayCrosshairColor.Name = "comboBoxOverlayCrosshairColor";
            this.comboBoxOverlayCrosshairColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxOverlayCrosshairColor.TabIndex = 6;
            this.comboBoxOverlayCrosshairColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelOverlayCrosshairColor
            // 
            this.labelOverlayCrosshairColor.AutoSize = true;
            this.labelOverlayCrosshairColor.Location = new System.Drawing.Point(9, 74);
            this.labelOverlayCrosshairColor.Name = "labelOverlayCrosshairColor";
            this.labelOverlayCrosshairColor.Size = new System.Drawing.Size(117, 13);
            this.labelOverlayCrosshairColor.TabIndex = 5;
            this.labelOverlayCrosshairColor.Text = "Overlay cross&hair color:";
            // 
            // comboBoxOverlayBackgroundColor
            // 
            this.comboBoxOverlayBackgroundColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOverlayBackgroundColor.FormattingEnabled = true;
            this.comboBoxOverlayBackgroundColor.Location = new System.Drawing.Point(196, 44);
            this.comboBoxOverlayBackgroundColor.Name = "comboBoxOverlayBackgroundColor";
            this.comboBoxOverlayBackgroundColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxOverlayBackgroundColor.TabIndex = 4;
            this.comboBoxOverlayBackgroundColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelOverlayBackgroundColor
            // 
            this.labelOverlayBackgroundColor.AutoSize = true;
            this.labelOverlayBackgroundColor.Location = new System.Drawing.Point(9, 47);
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
            // tabPageGeneral1
            // 
            this.tabPageGeneral1.Controls.Add(this.groupBoxConfiguration);
            this.tabPageGeneral1.Controls.Add(this.groupBoxBookmarks);
            this.tabPageGeneral1.Controls.Add(this.groupBoxApplication);
            this.tabPageGeneral1.Controls.Add(this.groupBoxBell);
            this.tabPageGeneral1.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral1.Name = "tabPageGeneral1";
            this.tabPageGeneral1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral1.Size = new System.Drawing.Size(380, 488);
            this.tabPageGeneral1.TabIndex = 2;
            this.tabPageGeneral1.Text = "General (1)";
            this.tabPageGeneral1.UseVisualStyleBackColor = true;
            // 
            // groupBoxConfiguration
            // 
            this.groupBoxConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConfiguration.Controls.Add(this.buttonEditRemoteConfig);
            this.groupBoxConfiguration.Location = new System.Drawing.Point(6, 428);
            this.groupBoxConfiguration.Name = "groupBoxConfiguration";
            this.groupBoxConfiguration.Size = new System.Drawing.Size(368, 54);
            this.groupBoxConfiguration.TabIndex = 3;
            this.groupBoxConfiguration.TabStop = false;
            this.groupBoxConfiguration.Text = "Configuration";
            // 
            // buttonEditRemoteConfig
            // 
            this.buttonEditRemoteConfig.Location = new System.Drawing.Point(89, 23);
            this.buttonEditRemoteConfig.Name = "buttonEditRemoteConfig";
            this.buttonEditRemoteConfig.Size = new System.Drawing.Size(190, 23);
            this.buttonEditRemoteConfig.TabIndex = 0;
            this.buttonEditRemoteConfig.Text = "Edit &Remote Configuration File...";
            this.buttonEditRemoteConfig.UseVisualStyleBackColor = true;
            this.buttonEditRemoteConfig.Click += new System.EventHandler(this.EditRemoteConfig);
            this.buttonEditRemoteConfig.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxBookmarks
            // 
            this.groupBoxBookmarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBookmarks.Controls.Add(this.buttonManageBookmarks);
            this.groupBoxBookmarks.Controls.Add(this.checkBoxSortBookmarks);
            this.groupBoxBookmarks.Controls.Add(this.checkBoxTruncateBookmarkTitles);
            this.groupBoxBookmarks.Location = new System.Drawing.Point(6, 329);
            this.groupBoxBookmarks.Name = "groupBoxBookmarks";
            this.groupBoxBookmarks.Size = new System.Drawing.Size(368, 93);
            this.groupBoxBookmarks.TabIndex = 2;
            this.groupBoxBookmarks.TabStop = false;
            this.groupBoxBookmarks.Text = "Bookmarks";
            // 
            // buttonManageBookmarks
            // 
            this.buttonManageBookmarks.Location = new System.Drawing.Point(89, 62);
            this.buttonManageBookmarks.Name = "buttonManageBookmarks";
            this.buttonManageBookmarks.Size = new System.Drawing.Size(190, 23);
            this.buttonManageBookmarks.TabIndex = 2;
            this.buttonManageBookmarks.Text = "Mana&ge Bookmarks...";
            this.buttonManageBookmarks.UseVisualStyleBackColor = true;
            this.buttonManageBookmarks.Click += new System.EventHandler(this.OnManageBookmarksClick);
            this.buttonManageBookmarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxSortBookmarks
            // 
            this.checkBoxSortBookmarks.AutoSize = true;
            this.checkBoxSortBookmarks.Location = new System.Drawing.Point(12, 19);
            this.checkBoxSortBookmarks.Name = "checkBoxSortBookmarks";
            this.checkBoxSortBookmarks.Size = new System.Drawing.Size(133, 17);
            this.checkBoxSortBookmarks.TabIndex = 0;
            this.checkBoxSortBookmarks.Text = "Sort book&marks by title";
            this.checkBoxSortBookmarks.UseVisualStyleBackColor = true;
            this.checkBoxSortBookmarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxTruncateBookmarkTitles
            // 
            this.checkBoxTruncateBookmarkTitles.AutoSize = true;
            this.checkBoxTruncateBookmarkTitles.Location = new System.Drawing.Point(12, 40);
            this.checkBoxTruncateBookmarkTitles.Name = "checkBoxTruncateBookmarkTitles";
            this.checkBoxTruncateBookmarkTitles.Size = new System.Drawing.Size(143, 17);
            this.checkBoxTruncateBookmarkTitles.TabIndex = 1;
            this.checkBoxTruncateBookmarkTitles.Text = "&Truncate bookmark titles";
            this.checkBoxTruncateBookmarkTitles.UseVisualStyleBackColor = true;
            this.checkBoxTruncateBookmarkTitles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxApplication
            // 
            this.groupBoxApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBoxApplication.Size = new System.Drawing.Size(368, 175);
            this.groupBoxApplication.TabIndex = 0;
            this.groupBoxApplication.TabStop = false;
            this.groupBoxApplication.Text = "Application";
            // 
            // checkBoxPingWhenIdle
            // 
            this.checkBoxPingWhenIdle.AutoSize = true;
            this.checkBoxPingWhenIdle.Location = new System.Drawing.Point(12, 153);
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
            this.checkBoxAutoLogInAfterInitialLoad.Location = new System.Drawing.Point(12, 132);
            this.checkBoxAutoLogInAfterInitialLoad.Name = "checkBoxAutoLogInAfterInitialLoad";
            this.checkBoxAutoLogInAfterInitialLoad.Size = new System.Drawing.Size(149, 17);
            this.checkBoxAutoLogInAfterInitialLoad.TabIndex = 7;
            this.checkBoxAutoLogInAfterInitialLoad.Text = "Aut&o log in after initial load";
            this.checkBoxAutoLogInAfterInitialLoad.UseVisualStyleBackColor = true;
            this.checkBoxAutoLogInAfterInitialLoad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxDisplayPromptBeforeClosing
            // 
            this.checkBoxDisplayPromptBeforeClosing.AutoSize = true;
            this.checkBoxDisplayPromptBeforeClosing.Location = new System.Drawing.Point(12, 111);
            this.checkBoxDisplayPromptBeforeClosing.Name = "checkBoxDisplayPromptBeforeClosing";
            this.checkBoxDisplayPromptBeforeClosing.Size = new System.Drawing.Size(282, 17);
            this.checkBoxDisplayPromptBeforeClosing.TabIndex = 6;
            this.checkBoxDisplayPromptBeforeClosing.Text = "Dis&play prompt before closing main application window";
            this.checkBoxDisplayPromptBeforeClosing.UseVisualStyleBackColor = true;
            this.checkBoxDisplayPromptBeforeClosing.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxUnitPrefix
            // 
            this.comboBoxUnitPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnitPrefix.FormattingEnabled = true;
            this.comboBoxUnitPrefix.Location = new System.Drawing.Point(110, 86);
            this.comboBoxUnitPrefix.Name = "comboBoxUnitPrefix";
            this.comboBoxUnitPrefix.Size = new System.Drawing.Size(100, 21);
            this.comboBoxUnitPrefix.TabIndex = 5;
            this.comboBoxUnitPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelUnitPrefix
            // 
            this.labelUnitPrefix.AutoSize = true;
            this.labelUnitPrefix.Location = new System.Drawing.Point(9, 90);
            this.labelUnitPrefix.Name = "labelUnitPrefix";
            this.labelUnitPrefix.Size = new System.Drawing.Size(57, 13);
            this.labelUnitPrefix.TabIndex = 4;
            this.labelUnitPrefix.Text = "Unit prefi&x:";
            // 
            // comboBoxNumberFormat
            // 
            this.comboBoxNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumberFormat.FormattingEnabled = true;
            this.comboBoxNumberFormat.Location = new System.Drawing.Point(110, 61);
            this.comboBoxNumberFormat.Name = "comboBoxNumberFormat";
            this.comboBoxNumberFormat.Size = new System.Drawing.Size(250, 21);
            this.comboBoxNumberFormat.TabIndex = 3;
            this.comboBoxNumberFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelNumberFormat
            // 
            this.labelNumberFormat.AutoSize = true;
            this.labelNumberFormat.Location = new System.Drawing.Point(9, 64);
            this.labelNumberFormat.Name = "labelNumberFormat";
            this.labelNumberFormat.Size = new System.Drawing.Size(79, 13);
            this.labelNumberFormat.TabIndex = 2;
            this.labelNumberFormat.Text = "Number &format:";
            // 
            // checkBoxStatusBarNotifOnly
            // 
            this.checkBoxStatusBarNotifOnly.AutoSize = true;
            this.checkBoxStatusBarNotifOnly.Location = new System.Drawing.Point(31, 40);
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
            // groupBoxBell
            // 
            this.groupBoxBell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBell.Controls.Add(this.buttonFOChime);
            this.groupBoxBell.Controls.Add(this.comboBoxFOSound);
            this.groupBoxBell.Controls.Add(this.labelFOSound);
            this.groupBoxBell.Controls.Add(this.checkBoxEnableFOBell);
            this.groupBoxBell.Controls.Add(this.buttonNTChime);
            this.groupBoxBell.Controls.Add(this.comboBoxNTSound);
            this.groupBoxBell.Controls.Add(this.labelNTSound);
            this.groupBoxBell.Controls.Add(this.checkBoxEnableNTBell);
            this.groupBoxBell.Controls.Add(this.checkBoxBoldErrorBell);
            this.groupBoxBell.Location = new System.Drawing.Point(6, 187);
            this.groupBoxBell.Name = "groupBoxBell";
            this.groupBoxBell.Size = new System.Drawing.Size(368, 136);
            this.groupBoxBell.TabIndex = 1;
            this.groupBoxBell.TabStop = false;
            this.groupBoxBell.Text = "Bell";
            // 
            // buttonFOChime
            // 
            this.buttonFOChime.Location = new System.Drawing.Point(285, 106);
            this.buttonFOChime.Name = "buttonFOChime";
            this.buttonFOChime.Size = new System.Drawing.Size(75, 23);
            this.buttonFOChime.TabIndex = 8;
            this.buttonFOChime.Text = "C&hime";
            this.buttonFOChime.UseVisualStyleBackColor = true;
            this.buttonFOChime.Click += new System.EventHandler(this.OnOpportunityChimeClick);
            this.buttonFOChime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxFOSound
            // 
            this.comboBoxFOSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFOSound.FormattingEnabled = true;
            this.comboBoxFOSound.Location = new System.Drawing.Point(110, 107);
            this.comboBoxFOSound.Name = "comboBoxFOSound";
            this.comboBoxFOSound.Size = new System.Drawing.Size(169, 21);
            this.comboBoxFOSound.TabIndex = 7;
            this.comboBoxFOSound.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelFOSound
            // 
            this.labelFOSound.AutoSize = true;
            this.labelFOSound.Location = new System.Drawing.Point(9, 110);
            this.labelFOSound.Name = "labelFOSound";
            this.labelFOSound.Size = new System.Drawing.Size(86, 13);
            this.labelFOSound.TabIndex = 6;
            this.labelFOSound.Text = "Fast opp. so&und:";
            // 
            // checkBoxEnableFOBell
            // 
            this.checkBoxEnableFOBell.AutoSize = true;
            this.checkBoxEnableFOBell.Location = new System.Drawing.Point(12, 86);
            this.checkBoxEnableFOBell.Name = "checkBoxEnableFOBell";
            this.checkBoxEnableFOBell.Size = new System.Drawing.Size(153, 17);
            this.checkBoxEnableFOBell.TabIndex = 5;
            this.checkBoxEnableFOBell.Text = "Enab&le fast opportunity bell";
            this.checkBoxEnableFOBell.UseVisualStyleBackColor = true;
            this.checkBoxEnableFOBell.CheckedChanged += new System.EventHandler(this.OnOpportunityCheckedChanged);
            // 
            // buttonNTChime
            // 
            this.buttonNTChime.Location = new System.Drawing.Point(285, 60);
            this.buttonNTChime.Name = "buttonNTChime";
            this.buttonNTChime.Size = new System.Drawing.Size(75, 23);
            this.buttonNTChime.TabIndex = 4;
            this.buttonNTChime.Text = "&Chime";
            this.buttonNTChime.UseVisualStyleBackColor = true;
            this.buttonNTChime.Click += new System.EventHandler(this.OnTipArrivalChimeClick);
            this.buttonNTChime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxNTSound
            // 
            this.comboBoxNTSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNTSound.FormattingEnabled = true;
            this.comboBoxNTSound.Location = new System.Drawing.Point(110, 61);
            this.comboBoxNTSound.Name = "comboBoxNTSound";
            this.comboBoxNTSound.Size = new System.Drawing.Size(169, 21);
            this.comboBoxNTSound.TabIndex = 3;
            this.comboBoxNTSound.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelNTSound
            // 
            this.labelNTSound.AutoSize = true;
            this.labelNTSound.Location = new System.Drawing.Point(9, 64);
            this.labelNTSound.Name = "labelNTSound";
            this.labelNTSound.Size = new System.Drawing.Size(83, 13);
            this.labelNTSound.TabIndex = 2;
            this.labelNTSound.Text = "New tips soun&d:";
            // 
            // checkBoxEnableNTBell
            // 
            this.checkBoxEnableNTBell.AutoSize = true;
            this.checkBoxEnableNTBell.Location = new System.Drawing.Point(12, 40);
            this.checkBoxEnableNTBell.Name = "checkBoxEnableNTBell";
            this.checkBoxEnableNTBell.Size = new System.Drawing.Size(120, 17);
            this.checkBoxEnableNTBell.TabIndex = 1;
            this.checkBoxEnableNTBell.Text = "Enable ne&w tips bell";
            this.checkBoxEnableNTBell.UseVisualStyleBackColor = true;
            this.checkBoxEnableNTBell.CheckedChanged += new System.EventHandler(this.OnTipArrivalCheckedChanged);
            this.checkBoxEnableNTBell.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxBoldErrorBell
            // 
            this.checkBoxBoldErrorBell.AutoSize = true;
            this.checkBoxBoldErrorBell.Location = new System.Drawing.Point(12, 19);
            this.checkBoxBoldErrorBell.Name = "checkBoxBoldErrorBell";
            this.checkBoxBoldErrorBell.Size = new System.Drawing.Size(121, 17);
            this.checkBoxBoldErrorBell.TabIndex = 0;
            this.checkBoxBoldErrorBell.Text = "&Bold error bell status";
            this.checkBoxBoldErrorBell.UseVisualStyleBackColor = true;
            this.checkBoxBoldErrorBell.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageGeneral2
            // 
            this.tabPageGeneral2.Controls.Add(this.groupBoxCountDown);
            this.tabPageGeneral2.Controls.Add(this.groupBoxPrinting);
            this.tabPageGeneral2.Controls.Add(this.groupBoxFind);
            this.tabPageGeneral2.Controls.Add(this.groupBoxSecurity);
            this.tabPageGeneral2.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral2.Name = "tabPageGeneral2";
            this.tabPageGeneral2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral2.Size = new System.Drawing.Size(380, 488);
            this.tabPageGeneral2.TabIndex = 3;
            this.tabPageGeneral2.Text = "General (2)";
            this.tabPageGeneral2.UseVisualStyleBackColor = true;
            // 
            // groupBoxCountDown
            // 
            this.groupBoxCountDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCountDown.Controls.Add(this.labelSecond);
            this.groupBoxCountDown.Controls.Add(this.numericUpDownSecond);
            this.groupBoxCountDown.Controls.Add(this.labelCountDown);
            this.groupBoxCountDown.Location = new System.Drawing.Point(6, 404);
            this.groupBoxCountDown.Name = "groupBoxCountDown";
            this.groupBoxCountDown.Size = new System.Drawing.Size(368, 78);
            this.groupBoxCountDown.TabIndex = 3;
            this.groupBoxCountDown.TabStop = false;
            this.groupBoxCountDown.Text = "Turn off the Monitors";
            // 
            // labelSecond
            // 
            this.labelSecond.AutoSize = true;
            this.labelSecond.Location = new System.Drawing.Point(313, 20);
            this.labelSecond.Name = "labelSecond";
            this.labelSecond.Size = new System.Drawing.Size(12, 13);
            this.labelSecond.TabIndex = 2;
            this.labelSecond.Text = "s";
            // 
            // numericUpDownSecond
            // 
            this.numericUpDownSecond.Location = new System.Drawing.Point(257, 18);
            this.numericUpDownSecond.Name = "numericUpDownSecond";
            this.numericUpDownSecond.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownSecond.TabIndex = 1;
            // 
            // labelCountDown
            // 
            this.labelCountDown.AutoSize = true;
            this.labelCountDown.Location = new System.Drawing.Point(9, 20);
            this.labelCountDown.Name = "labelCountDown";
            this.labelCountDown.Size = new System.Drawing.Size(222, 13);
            this.labelCountDown.TabIndex = 0;
            this.labelCountDown.Text = "&Interval after which monitors will be turned off:";
            // 
            // groupBoxPrinting
            // 
            this.groupBoxPrinting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPrinting.Controls.Add(this.radioButtonHardMargins);
            this.groupBoxPrinting.Controls.Add(this.radioButtonSoftMargins);
            this.groupBoxPrinting.Controls.Add(this.labelPrinting);
            this.groupBoxPrinting.Location = new System.Drawing.Point(6, 102);
            this.groupBoxPrinting.Name = "groupBoxPrinting";
            this.groupBoxPrinting.Size = new System.Drawing.Size(368, 120);
            this.groupBoxPrinting.TabIndex = 1;
            this.groupBoxPrinting.TabStop = false;
            this.groupBoxPrinting.Text = "Printing";
            // 
            // radioButtonHardMargins
            // 
            this.radioButtonHardMargins.AutoSize = true;
            this.radioButtonHardMargins.Location = new System.Drawing.Point(12, 77);
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
            this.radioButtonSoftMargins.Location = new System.Drawing.Point(12, 54);
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
            // groupBoxFind
            // 
            this.groupBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFind.Controls.Add(this.checkBoxF3MainFormFind);
            this.groupBoxFind.Controls.Add(this.checkBoxOutlineSearchResults);
            this.groupBoxFind.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFind.Name = "groupBoxFind";
            this.groupBoxFind.Size = new System.Drawing.Size(368, 90);
            this.groupBoxFind.TabIndex = 0;
            this.groupBoxFind.TabStop = false;
            this.groupBoxFind.Text = "Finding String in Page";
            // 
            // checkBoxF3MainFormFind
            // 
            this.checkBoxF3MainFormFind.AutoSize = true;
            this.checkBoxF3MainFormFind.Location = new System.Drawing.Point(12, 42);
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
            this.checkBoxOutlineSearchResults.Text = "&Outline search result";
            this.checkBoxOutlineSearchResults.UseVisualStyleBackColor = true;
            this.checkBoxOutlineSearchResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // groupBoxSecurity
            // 
            this.groupBoxSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSecurity.Controls.Add(this.checkBoxIgnoreCertificateErrors);
            this.groupBoxSecurity.Controls.Add(this.checkBoxBlockRequestsToForeignUrls);
            this.groupBoxSecurity.Controls.Add(this.buttonAllowedClientsIP);
            this.groupBoxSecurity.Controls.Add(this.checkBoxKeepAnEyeOnTheClientsIP);
            this.groupBoxSecurity.Location = new System.Drawing.Point(6, 228);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(368, 170);
            this.groupBoxSecurity.TabIndex = 2;
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Security";
            // 
            // checkBoxIgnoreCertificateErrors
            // 
            this.checkBoxIgnoreCertificateErrors.AutoSize = true;
            this.checkBoxIgnoreCertificateErrors.Location = new System.Drawing.Point(12, 120);
            this.checkBoxIgnoreCertificateErrors.Name = "checkBoxIgnoreCertificateErrors";
            this.checkBoxIgnoreCertificateErrors.Size = new System.Drawing.Size(134, 17);
            this.checkBoxIgnoreCertificateErrors.TabIndex = 3;
            this.checkBoxIgnoreCertificateErrors.Text = "Ignore &certificate errors";
            this.checkBoxIgnoreCertificateErrors.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreCertificateErrors.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxIgnoreCertificateErrors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxBlockRequestsToForeignUrls
            // 
            this.checkBoxBlockRequestsToForeignUrls.AutoSize = true;
            this.checkBoxBlockRequestsToForeignUrls.Location = new System.Drawing.Point(12, 84);
            this.checkBoxBlockRequestsToForeignUrls.Name = "checkBoxBlockRequestsToForeignUrls";
            this.checkBoxBlockRequestsToForeignUrls.Size = new System.Drawing.Size(327, 30);
            this.checkBoxBlockRequestsToForeignUrls.TabIndex = 2;
            this.checkBoxBlockRequestsToForeignUrls.Text = "&Block all requests to URLs not belonging to the URL set by their\r\nsecond-level d" +
    "omains or the exception list by their hostname.";
            this.checkBoxBlockRequestsToForeignUrls.UseVisualStyleBackColor = true;
            this.checkBoxBlockRequestsToForeignUrls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonAllowedClientsIP
            // 
            this.buttonAllowedClientsIP.Location = new System.Drawing.Point(89, 55);
            this.buttonAllowedClientsIP.Name = "buttonAllowedClientsIP";
            this.buttonAllowedClientsIP.Size = new System.Drawing.Size(190, 23);
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
            this.checkBoxKeepAnEyeOnTheClientsIP.Text = "&Always keep an eye on the client\'s IP address and block all\r\noutgoing requests i" +
    "f the client\'s IP address changes.";
            this.checkBoxKeepAnEyeOnTheClientsIP.UseVisualStyleBackColor = true;
            this.checkBoxKeepAnEyeOnTheClientsIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // tabPageUserInterface
            // 
            this.tabPageUserInterface.Controls.Add(this.groupBoxMisc);
            this.tabPageUserInterface.Controls.Add(this.groupBoxUserInterface);
            this.tabPageUserInterface.Controls.Add(this.groupBoxStatusStrip);
            this.tabPageUserInterface.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserInterface.Name = "tabPageUserInterface";
            this.tabPageUserInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserInterface.Size = new System.Drawing.Size(380, 488);
            this.tabPageUserInterface.TabIndex = 4;
            this.tabPageUserInterface.Text = "User Interface";
            this.tabPageUserInterface.UseVisualStyleBackColor = true;
            // 
            // groupBoxMisc
            // 
            this.groupBoxMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMisc.Controls.Add(this.checkBoxAutoAdjustRightPaneWidth);
            this.groupBoxMisc.Controls.Add(this.checkBoxDisableThemes);
            this.groupBoxMisc.Location = new System.Drawing.Point(6, 420);
            this.groupBoxMisc.Name = "groupBoxMisc";
            this.groupBoxMisc.Size = new System.Drawing.Size(368, 62);
            this.groupBoxMisc.TabIndex = 3;
            this.groupBoxMisc.TabStop = false;
            this.groupBoxMisc.Text = "Miscellaneous";
            // 
            // checkBoxAutoAdjustRightPaneWidth
            // 
            this.checkBoxAutoAdjustRightPaneWidth.AutoSize = true;
            this.checkBoxAutoAdjustRightPaneWidth.Location = new System.Drawing.Point(12, 19);
            this.checkBoxAutoAdjustRightPaneWidth.Name = "checkBoxAutoAdjustRightPaneWidth";
            this.checkBoxAutoAdjustRightPaneWidth.Size = new System.Drawing.Size(157, 17);
            this.checkBoxAutoAdjustRightPaneWidth.TabIndex = 0;
            this.checkBoxAutoAdjustRightPaneWidth.Text = "Auto adjust right pane &width";
            this.checkBoxAutoAdjustRightPaneWidth.UseVisualStyleBackColor = true;
            this.checkBoxAutoAdjustRightPaneWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxDisableThemes
            // 
            this.checkBoxDisableThemes.AutoSize = true;
            this.checkBoxDisableThemes.Location = new System.Drawing.Point(12, 39);
            this.checkBoxDisableThemes.Name = "checkBoxDisableThemes";
            this.checkBoxDisableThemes.Size = new System.Drawing.Size(98, 17);
            this.checkBoxDisableThemes.TabIndex = 0;
            this.checkBoxDisableThemes.Text = "&Disable themes";
            this.checkBoxDisableThemes.UseVisualStyleBackColor = true;
            this.checkBoxDisableThemes.CheckedChanged += new System.EventHandler(this.SetWarning);
            this.checkBoxDisableThemes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
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
            this.groupBoxUserInterface.Size = new System.Drawing.Size(368, 329);
            this.groupBoxUserInterface.TabIndex = 0;
            this.groupBoxUserInterface.TabStop = false;
            this.groupBoxUserInterface.Text = "Tab Headers in the Main Window";
            // 
            // comboBoxCalculatorSColor
            // 
            this.comboBoxCalculatorSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalculatorSColor.FormattingEnabled = true;
            this.comboBoxCalculatorSColor.Location = new System.Drawing.Point(196, 299);
            this.comboBoxCalculatorSColor.Name = "comboBoxCalculatorSColor";
            this.comboBoxCalculatorSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxCalculatorSColor.TabIndex = 23;
            this.comboBoxCalculatorSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelCalculatorSColor
            // 
            this.labelCalculatorSColor.AutoSize = true;
            this.labelCalculatorSColor.Location = new System.Drawing.Point(9, 302);
            this.labelCalculatorSColor.Name = "labelCalculatorSColor";
            this.labelCalculatorSColor.Size = new System.Drawing.Size(144, 13);
            this.labelCalculatorSColor.TabIndex = 22;
            this.labelCalculatorSColor.Text = "Bet calculato&r selected color:";
            // 
            // comboBoxCalculatorDColor
            // 
            this.comboBoxCalculatorDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalculatorDColor.FormattingEnabled = true;
            this.comboBoxCalculatorDColor.Location = new System.Drawing.Point(196, 275);
            this.comboBoxCalculatorDColor.Name = "comboBoxCalculatorDColor";
            this.comboBoxCalculatorDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxCalculatorDColor.TabIndex = 21;
            this.comboBoxCalculatorDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelCalculatorDColor
            // 
            this.labelCalculatorDColor.AutoSize = true;
            this.labelCalculatorDColor.Location = new System.Drawing.Point(9, 278);
            this.labelCalculatorDColor.Name = "labelCalculatorDColor";
            this.labelCalculatorDColor.Size = new System.Drawing.Size(136, 13);
            this.labelCalculatorDColor.TabIndex = 20;
            this.labelCalculatorDColor.Text = "Bet c&alculator default color:";
            // 
            // comboBoxDashboardSColor
            // 
            this.comboBoxDashboardSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDashboardSColor.FormattingEnabled = true;
            this.comboBoxDashboardSColor.Location = new System.Drawing.Point(196, 251);
            this.comboBoxDashboardSColor.Name = "comboBoxDashboardSColor";
            this.comboBoxDashboardSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxDashboardSColor.TabIndex = 19;
            this.comboBoxDashboardSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelDashboardSColor
            // 
            this.labelDashboardSColor.AutoSize = true;
            this.labelDashboardSColor.Location = new System.Drawing.Point(9, 254);
            this.labelDashboardSColor.Name = "labelDashboardSColor";
            this.labelDashboardSColor.Size = new System.Drawing.Size(171, 13);
            this.labelDashboardSColor.TabIndex = 18;
            this.labelDashboardSColor.Text = "Dashboard and ti&ps selected color:";
            // 
            // comboBoxDashboardDColor
            // 
            this.comboBoxDashboardDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDashboardDColor.FormattingEnabled = true;
            this.comboBoxDashboardDColor.Location = new System.Drawing.Point(196, 227);
            this.comboBoxDashboardDColor.Name = "comboBoxDashboardDColor";
            this.comboBoxDashboardDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxDashboardDColor.TabIndex = 17;
            this.comboBoxDashboardDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelDashboardDColor
            // 
            this.labelDashboardDColor.AutoSize = true;
            this.labelDashboardDColor.Location = new System.Drawing.Point(9, 230);
            this.labelDashboardDColor.Name = "labelDashboardDColor";
            this.labelDashboardDColor.Size = new System.Drawing.Size(163, 13);
            this.labelDashboardDColor.TabIndex = 16;
            this.labelDashboardDColor.Text = "Das&hboard and tips default color:";
            // 
            // comboBoxSportInfo2SColor
            // 
            this.comboBoxSportInfo2SColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo2SColor.FormattingEnabled = true;
            this.comboBoxSportInfo2SColor.Location = new System.Drawing.Point(196, 203);
            this.comboBoxSportInfo2SColor.Name = "comboBoxSportInfo2SColor";
            this.comboBoxSportInfo2SColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo2SColor.TabIndex = 15;
            this.comboBoxSportInfo2SColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo2SColor
            // 
            this.labelSportInfo2SColor.AutoSize = true;
            this.labelSportInfo2SColor.Location = new System.Drawing.Point(9, 206);
            this.labelSportInfo2SColor.Name = "labelSportInfo2SColor";
            this.labelSportInfo2SColor.Size = new System.Drawing.Size(133, 13);
            this.labelSportInfo2SColor.TabIndex = 14;
            this.labelSportInfo2SColor.Text = "Sport info &2 selected color:";
            // 
            // comboBoxSportInfo2DColor
            // 
            this.comboBoxSportInfo2DColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo2DColor.FormattingEnabled = true;
            this.comboBoxSportInfo2DColor.Location = new System.Drawing.Point(196, 179);
            this.comboBoxSportInfo2DColor.Name = "comboBoxSportInfo2DColor";
            this.comboBoxSportInfo2DColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo2DColor.TabIndex = 13;
            this.comboBoxSportInfo2DColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo2DColor
            // 
            this.labelSportInfo2DColor.AutoSize = true;
            this.labelSportInfo2DColor.Location = new System.Drawing.Point(9, 182);
            this.labelSportInfo2DColor.Name = "labelSportInfo2DColor";
            this.labelSportInfo2DColor.Size = new System.Drawing.Size(125, 13);
            this.labelSportInfo2DColor.TabIndex = 12;
            this.labelSportInfo2DColor.Text = "Sport i&nfo 2 default color:";
            // 
            // comboBoxSportInfo1SColor
            // 
            this.comboBoxSportInfo1SColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo1SColor.FormattingEnabled = true;
            this.comboBoxSportInfo1SColor.Location = new System.Drawing.Point(196, 155);
            this.comboBoxSportInfo1SColor.Name = "comboBoxSportInfo1SColor";
            this.comboBoxSportInfo1SColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo1SColor.TabIndex = 11;
            this.comboBoxSportInfo1SColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo1SColor
            // 
            this.labelSportInfo1SColor.AutoSize = true;
            this.labelSportInfo1SColor.Location = new System.Drawing.Point(9, 158);
            this.labelSportInfo1SColor.Name = "labelSportInfo1SColor";
            this.labelSportInfo1SColor.Size = new System.Drawing.Size(133, 13);
            this.labelSportInfo1SColor.TabIndex = 10;
            this.labelSportInfo1SColor.Text = "Sport info &1 selected color:";
            // 
            // comboBoxSportInfo1DColor
            // 
            this.comboBoxSportInfo1DColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSportInfo1DColor.FormattingEnabled = true;
            this.comboBoxSportInfo1DColor.Location = new System.Drawing.Point(196, 131);
            this.comboBoxSportInfo1DColor.Name = "comboBoxSportInfo1DColor";
            this.comboBoxSportInfo1DColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSportInfo1DColor.TabIndex = 9;
            this.comboBoxSportInfo1DColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelSportInfo1DColor
            // 
            this.labelSportInfo1DColor.AutoSize = true;
            this.labelSportInfo1DColor.Location = new System.Drawing.Point(9, 134);
            this.labelSportInfo1DColor.Name = "labelSportInfo1DColor";
            this.labelSportInfo1DColor.Size = new System.Drawing.Size(125, 13);
            this.labelSportInfo1DColor.TabIndex = 8;
            this.labelSportInfo1DColor.Text = "Sport &info 1 default color:";
            // 
            // comboBoxBookmakerSColor
            // 
            this.comboBoxBookmakerSColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBookmakerSColor.FormattingEnabled = true;
            this.comboBoxBookmakerSColor.Location = new System.Drawing.Point(196, 107);
            this.comboBoxBookmakerSColor.Name = "comboBoxBookmakerSColor";
            this.comboBoxBookmakerSColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBookmakerSColor.TabIndex = 7;
            this.comboBoxBookmakerSColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelBookmakerSColor
            // 
            this.labelBookmakerSColor.AutoSize = true;
            this.labelBookmakerSColor.Location = new System.Drawing.Point(9, 110);
            this.labelBookmakerSColor.Name = "labelBookmakerSColor";
            this.labelBookmakerSColor.Size = new System.Drawing.Size(133, 13);
            this.labelBookmakerSColor.TabIndex = 6;
            this.labelBookmakerSColor.Text = "Boo&kmaker selected color:";
            // 
            // comboBoxBookmakerDColor
            // 
            this.comboBoxBookmakerDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBookmakerDColor.FormattingEnabled = true;
            this.comboBoxBookmakerDColor.Location = new System.Drawing.Point(196, 83);
            this.comboBoxBookmakerDColor.Name = "comboBoxBookmakerDColor";
            this.comboBoxBookmakerDColor.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBookmakerDColor.TabIndex = 5;
            this.comboBoxBookmakerDColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelBookmakerDColor
            // 
            this.labelBookmakerDColor.AutoSize = true;
            this.labelBookmakerDColor.Location = new System.Drawing.Point(9, 86);
            this.labelBookmakerDColor.Name = "labelBookmakerDColor";
            this.labelBookmakerDColor.Size = new System.Drawing.Size(125, 13);
            this.labelBookmakerDColor.TabIndex = 4;
            this.labelBookmakerDColor.Text = "B&ookmaker default color:";
            // 
            // checkBoxBackgroundColor
            // 
            this.checkBoxBackgroundColor.AutoSize = true;
            this.checkBoxBackgroundColor.Location = new System.Drawing.Point(12, 63);
            this.checkBoxBackgroundColor.Name = "checkBoxBackgroundColor";
            this.checkBoxBackgroundColor.Size = new System.Drawing.Size(110, 17);
            this.checkBoxBackgroundColor.TabIndex = 3;
            this.checkBoxBackgroundColor.Text = "Ba&ckground color";
            this.checkBoxBackgroundColor.UseVisualStyleBackColor = true;
            this.checkBoxBackgroundColor.CheckedChanged += new System.EventHandler(this.OnBackgroundColorCheckedChanged);
            this.checkBoxBackgroundColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxBoldFont
            // 
            this.checkBoxBoldFont.AutoSize = true;
            this.checkBoxBoldFont.Location = new System.Drawing.Point(12, 43);
            this.checkBoxBoldFont.Name = "checkBoxBoldFont";
            this.checkBoxBoldFont.Size = new System.Drawing.Size(68, 17);
            this.checkBoxBoldFont.TabIndex = 2;
            this.checkBoxBoldFont.Text = "&Bold font";
            this.checkBoxBoldFont.UseVisualStyleBackColor = true;
            this.checkBoxBoldFont.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // comboBoxTabAppearance
            // 
            this.comboBoxTabAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTabAppearance.FormattingEnabled = true;
            this.comboBoxTabAppearance.Location = new System.Drawing.Point(196, 19);
            this.comboBoxTabAppearance.Name = "comboBoxTabAppearance";
            this.comboBoxTabAppearance.Size = new System.Drawing.Size(164, 21);
            this.comboBoxTabAppearance.TabIndex = 1;
            this.comboBoxTabAppearance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelTabAppearance
            // 
            this.labelTabAppearance.AutoSize = true;
            this.labelTabAppearance.Location = new System.Drawing.Point(9, 22);
            this.labelTabAppearance.Name = "labelTabAppearance";
            this.labelTabAppearance.Size = new System.Drawing.Size(130, 13);
            this.labelTabAppearance.TabIndex = 0;
            this.labelTabAppearance.Text = "&Tab headers appearance:";
            // 
            // groupBoxStatusStrip
            // 
            this.groupBoxStatusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStatusStrip.Controls.Add(this.comboBoxBorderStyle);
            this.groupBoxStatusStrip.Controls.Add(this.labelStripRenderMode);
            this.groupBoxStatusStrip.Controls.Add(this.labelBorderStyle);
            this.groupBoxStatusStrip.Controls.Add(this.comboBoxStripRenderMode);
            this.groupBoxStatusStrip.Location = new System.Drawing.Point(6, 341);
            this.groupBoxStatusStrip.Name = "groupBoxStatusStrip";
            this.groupBoxStatusStrip.Size = new System.Drawing.Size(368, 73);
            this.groupBoxStatusStrip.TabIndex = 1;
            this.groupBoxStatusStrip.TabStop = false;
            this.groupBoxStatusStrip.Text = "Status Strip";
            // 
            // comboBoxBorderStyle
            // 
            this.comboBoxBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderStyle.FormattingEnabled = true;
            this.comboBoxBorderStyle.Location = new System.Drawing.Point(196, 43);
            this.comboBoxBorderStyle.Name = "comboBoxBorderStyle";
            this.comboBoxBorderStyle.Size = new System.Drawing.Size(164, 21);
            this.comboBoxBorderStyle.TabIndex = 5;
            this.comboBoxBorderStyle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelStripRenderMode
            // 
            this.labelStripRenderMode.AutoSize = true;
            this.labelStripRenderMode.Location = new System.Drawing.Point(9, 22);
            this.labelStripRenderMode.Name = "labelStripRenderMode";
            this.labelStripRenderMode.Size = new System.Drawing.Size(124, 13);
            this.labelStripRenderMode.TabIndex = 2;
            this.labelStripRenderMode.Text = "Stat&us strip render mode:";
            // 
            // labelBorderStyle
            // 
            this.labelBorderStyle.AutoSize = true;
            this.labelBorderStyle.Location = new System.Drawing.Point(9, 46);
            this.labelBorderStyle.Name = "labelBorderStyle";
            this.labelBorderStyle.Size = new System.Drawing.Size(139, 13);
            this.labelBorderStyle.TabIndex = 4;
            this.labelBorderStyle.Text = "Status &label border 3D style:";
            // 
            // comboBoxStripRenderMode
            // 
            this.comboBoxStripRenderMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStripRenderMode.FormattingEnabled = true;
            this.comboBoxStripRenderMode.Location = new System.Drawing.Point(196, 19);
            this.comboBoxStripRenderMode.Name = "comboBoxStripRenderMode";
            this.comboBoxStripRenderMode.Size = new System.Drawing.Size(164, 21);
            this.comboBoxStripRenderMode.TabIndex = 3;
            this.comboBoxStripRenderMode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
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
            this.tabPageBrowser1.ResumeLayout(false);
            this.groupBoxHeaders.ResumeLayout(false);
            this.groupBoxHeaders.PerformLayout();
            this.groupBoxMultimedia.ResumeLayout(false);
            this.groupBoxMultimedia.PerformLayout();
            this.groupBoxCache.ResumeLayout(false);
            this.groupBoxCache.PerformLayout();
            this.tabPageBrowser2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelProxy.ResumeLayout(false);
            this.panelProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
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
            this.tabPageGeneral1.ResumeLayout(false);
            this.groupBoxConfiguration.ResumeLayout(false);
            this.groupBoxBookmarks.ResumeLayout(false);
            this.groupBoxBookmarks.PerformLayout();
            this.groupBoxApplication.ResumeLayout(false);
            this.groupBoxApplication.PerformLayout();
            this.groupBoxBell.ResumeLayout(false);
            this.groupBoxBell.PerformLayout();
            this.tabPageGeneral2.ResumeLayout(false);
            this.groupBoxCountDown.ResumeLayout(false);
            this.groupBoxCountDown.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSecond)).EndInit();
            this.groupBoxPrinting.ResumeLayout(false);
            this.groupBoxPrinting.PerformLayout();
            this.groupBoxFind.ResumeLayout(false);
            this.groupBoxFind.PerformLayout();
            this.groupBoxSecurity.ResumeLayout(false);
            this.groupBoxSecurity.PerformLayout();
            this.tabPageUserInterface.ResumeLayout(false);
            this.groupBoxMisc.ResumeLayout(false);
            this.groupBoxMisc.PerformLayout();
            this.groupBoxUserInterface.ResumeLayout(false);
            this.groupBoxUserInterface.PerformLayout();
            this.groupBoxStatusStrip.ResumeLayout(false);
            this.groupBoxStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageBrowser1;
        private System.Windows.Forms.GroupBox groupBoxMultimedia;
        private System.Windows.Forms.CheckBox checkBoxPersistSessionCookies;
        private System.Windows.Forms.CheckBox checkBoxPersistUserPreferences;
        private System.Windows.Forms.CheckBox checkBoxEnablePrintPreview;
        private System.Windows.Forms.GroupBox groupBoxCache;
        private System.Windows.Forms.CheckBox checkBoxEnableCache;
        private System.Windows.Forms.TabPage tabPageBrowser2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelProxy;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.CheckBox checkBoxEnableAudio;
        private System.Windows.Forms.CheckBox checkBoxEnableDrmContent;
        private System.Windows.Forms.GroupBox groupBoxHeaders;
        private System.Windows.Forms.ComboBox comboBoxAcceptLanguage;
        private System.Windows.Forms.Label labelAcceptLanguage;
        private System.Windows.Forms.ComboBox comboBoxUserAgent;
        private System.Windows.Forms.Label labelUserAgent;
        private System.Windows.Forms.GroupBox groupBoxNavigation;
        private System.Windows.Forms.TabPage tabPageGeneral1;
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
        private System.Windows.Forms.TabPage tabPageGeneral2;
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
        private System.Windows.Forms.CheckBox checkBoxEnableNTBell;
        private System.Windows.Forms.CheckBox checkBoxF3MainFormFind;
        private System.Windows.Forms.Button buttonManageBookmarks;
        private System.Windows.Forms.CheckBox checkBoxBoldErrorBell;
        private System.Windows.Forms.ComboBox comboBoxNTSound;
        private System.Windows.Forms.Label labelNTSound;
        private System.Windows.Forms.Button buttonNTChime;
        private System.Windows.Forms.TabPage tabPageUserInterface;
        private System.Windows.Forms.GroupBox groupBoxBell;
        private System.Windows.Forms.Label labelFOSound;
        private System.Windows.Forms.Button buttonFOChime;
        private System.Windows.Forms.ComboBox comboBoxFOSound;
        private System.Windows.Forms.GroupBox groupBoxBookmarks;
        private System.Windows.Forms.GroupBox groupBoxConfiguration;
        private System.Windows.Forms.GroupBox groupBoxCountDown;
        private System.Windows.Forms.Label labelSecond;
        private System.Windows.Forms.NumericUpDown numericUpDownSecond;
        private System.Windows.Forms.Label labelCountDown;
        private System.Windows.Forms.CheckBox checkBoxEnableFOBell;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label9;
    }
}