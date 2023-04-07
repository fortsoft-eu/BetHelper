namespace BetHelper {
    partial class WebInfoForm {
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonCopyUrl = new System.Windows.Forms.Button();
            this.labelUserName = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.buttonCopyUserName = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.maskedTextBoxPassword = new System.Windows.Forms.MaskedTextBox();
            this.buttonCopyPassword = new System.Windows.Forms.Button();
            this.labelScript = new System.Windows.Forms.Label();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.labelPattern = new System.Windows.Forms.Label();
            this.textBoxPattern = new System.Windows.Forms.TextBox();
            this.labelFields = new System.Windows.Forms.Label();
            this.textBoxFields = new System.Windows.Forms.TextBox();
            this.labelDisplayName = new System.Windows.Forms.Label();
            this.textBoxDisplayName = new System.Windows.Forms.TextBox();
            this.checkBoxHandlePopUps = new System.Windows.Forms.CheckBox();
            this.labelPopUpWidth = new System.Windows.Forms.Label();
            this.textBoxPopUpWidth = new System.Windows.Forms.TextBox();
            this.labelPopUpHeight = new System.Windows.Forms.Label();
            this.textBoxPopUpHeight = new System.Windows.Forms.TextBox();
            this.labelPopUpLeft = new System.Windows.Forms.Label();
            this.textBoxPopUpLeft = new System.Windows.Forms.TextBox();
            this.labelPopUpTop = new System.Windows.Forms.Label();
            this.textBoxPopUpTop = new System.Windows.Forms.TextBox();
            this.labelIetfLanguageTag = new System.Windows.Forms.Label();
            this.textBoxIetfLanguageTag = new System.Windows.Forms.TextBox();
            this.labelBackNavigation = new System.Windows.Forms.Label();
            this.textBoxBackNavigation = new System.Windows.Forms.TextBox();
            this.labelAllowedHosts = new System.Windows.Forms.Label();
            this.textBoxAllowedHosts = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxWillHandlePopUps = new System.Windows.Forms.CheckBox();
            this.labelChatHosts = new System.Windows.Forms.Label();
            this.textBoxChatHosts = new System.Windows.Forms.TextBox();
            this.labelUrlLive = new System.Windows.Forms.Label();
            this.textBoxUrlLive = new System.Windows.Forms.TextBox();
            this.checkBoxIsService = new System.Windows.Forms.CheckBox();
            this.checkBoxAudioMutedByDefault = new System.Windows.Forms.CheckBox();
            this.labelOrdinal = new System.Windows.Forms.Label();
            this.labelUrlNext = new System.Windows.Forms.Label();
            this.textBoxUrlNext = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxTabNavigation = new System.Windows.Forms.CheckBox();
            this.checkBoxWillTryToKeep = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(30, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "&Title:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(127, 12);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.ReadOnly = true;
            this.textBoxTitle.Size = new System.Drawing.Size(304, 20);
            this.textBoxTitle.TabIndex = 1;
            this.textBoxTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(12, 38);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(32, 13);
            this.labelUrl.TabIndex = 3;
            this.labelUrl.Text = "U&RL:";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(127, 35);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.ReadOnly = true;
            this.textBoxUrl.Size = new System.Drawing.Size(304, 20);
            this.textBoxUrl.TabIndex = 4;
            this.textBoxUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonCopyUrl
            // 
            this.buttonCopyUrl.Location = new System.Drawing.Point(437, 33);
            this.buttonCopyUrl.Name = "buttonCopyUrl";
            this.buttonCopyUrl.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyUrl.TabIndex = 5;
            this.buttonCopyUrl.Text = "Co&py";
            this.buttonCopyUrl.UseVisualStyleBackColor = true;
            this.buttonCopyUrl.Click += new System.EventHandler(this.OnCopyUrlClick);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(12, 107);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(61, 13);
            this.labelUserName.TabIndex = 10;
            this.labelUserName.Text = "User na&me:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(127, 104);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(304, 20);
            this.textBoxUserName.TabIndex = 11;
            this.textBoxUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUserName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonCopyUserName
            // 
            this.buttonCopyUserName.Location = new System.Drawing.Point(437, 102);
            this.buttonCopyUserName.Name = "buttonCopyUserName";
            this.buttonCopyUserName.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyUserName.TabIndex = 12;
            this.buttonCopyUserName.Text = "C&opy";
            this.buttonCopyUserName.UseVisualStyleBackColor = true;
            this.buttonCopyUserName.Click += new System.EventHandler(this.OnCopyUserNameClick);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(12, 130);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 13;
            this.labelPassword.Text = "Pass&word:";
            // 
            // maskedTextBoxPassword
            // 
            this.maskedTextBoxPassword.Location = new System.Drawing.Point(127, 127);
            this.maskedTextBoxPassword.Name = "maskedTextBoxPassword";
            this.maskedTextBoxPassword.ReadOnly = true;
            this.maskedTextBoxPassword.Size = new System.Drawing.Size(304, 20);
            this.maskedTextBoxPassword.TabIndex = 14;
            this.maskedTextBoxPassword.UseSystemPasswordChar = true;
            this.maskedTextBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnPasswordKeyDown);
            this.maskedTextBoxPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPasswordMouseDown);
            // 
            // buttonCopyPassword
            // 
            this.buttonCopyPassword.Location = new System.Drawing.Point(437, 125);
            this.buttonCopyPassword.Name = "buttonCopyPassword";
            this.buttonCopyPassword.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyPassword.TabIndex = 16;
            this.buttonCopyPassword.Text = "&Copy";
            this.buttonCopyPassword.UseVisualStyleBackColor = true;
            this.buttonCopyPassword.Click += new System.EventHandler(this.OnCopyPasswordClick);
            // 
            // labelScript
            // 
            this.labelScript.AutoSize = true;
            this.labelScript.Location = new System.Drawing.Point(12, 153);
            this.labelScript.Name = "labelScript";
            this.labelScript.Size = new System.Drawing.Size(60, 13);
            this.labelScript.TabIndex = 17;
            this.labelScript.Text = "&JavaScript:";
            // 
            // textBoxScript
            // 
            this.textBoxScript.Location = new System.Drawing.Point(127, 150);
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ReadOnly = true;
            this.textBoxScript.Size = new System.Drawing.Size(304, 20);
            this.textBoxScript.TabIndex = 18;
            this.textBoxScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxScript.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelPattern
            // 
            this.labelPattern.AutoSize = true;
            this.labelPattern.Location = new System.Drawing.Point(12, 176);
            this.labelPattern.Name = "labelPattern";
            this.labelPattern.Size = new System.Drawing.Size(44, 13);
            this.labelPattern.TabIndex = 19;
            this.labelPattern.Text = "Patter&n:";
            // 
            // textBoxPattern
            // 
            this.textBoxPattern.Location = new System.Drawing.Point(127, 173);
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.ReadOnly = true;
            this.textBoxPattern.Size = new System.Drawing.Size(304, 20);
            this.textBoxPattern.TabIndex = 20;
            this.textBoxPattern.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPattern.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelFields
            // 
            this.labelFields.AutoSize = true;
            this.labelFields.Location = new System.Drawing.Point(12, 199);
            this.labelFields.Name = "labelFields";
            this.labelFields.Size = new System.Drawing.Size(37, 13);
            this.labelFields.TabIndex = 21;
            this.labelFields.Text = "&Fields:";
            // 
            // textBoxFields
            // 
            this.textBoxFields.Location = new System.Drawing.Point(127, 196);
            this.textBoxFields.Name = "textBoxFields";
            this.textBoxFields.ReadOnly = true;
            this.textBoxFields.Size = new System.Drawing.Size(304, 20);
            this.textBoxFields.TabIndex = 22;
            this.textBoxFields.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.AutoSize = true;
            this.labelDisplayName.Location = new System.Drawing.Point(12, 222);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(73, 13);
            this.labelDisplayName.TabIndex = 23;
            this.labelDisplayName.Text = "&Display name:";
            // 
            // textBoxDisplayName
            // 
            this.textBoxDisplayName.Location = new System.Drawing.Point(127, 219);
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.ReadOnly = true;
            this.textBoxDisplayName.Size = new System.Drawing.Size(304, 20);
            this.textBoxDisplayName.TabIndex = 24;
            this.textBoxDisplayName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxDisplayName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxHandlePopUps
            // 
            this.checkBoxHandlePopUps.AutoSize = true;
            this.checkBoxHandlePopUps.Location = new System.Drawing.Point(15, 262);
            this.checkBoxHandlePopUps.Name = "checkBoxHandlePopUps";
            this.checkBoxHandlePopUps.Size = new System.Drawing.Size(98, 17);
            this.checkBoxHandlePopUps.TabIndex = 27;
            this.checkBoxHandlePopUps.Text = "Handle popups";
            this.checkBoxHandlePopUps.UseVisualStyleBackColor = true;
            this.checkBoxHandlePopUps.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // labelPopUpWidth
            // 
            this.labelPopUpWidth.AutoSize = true;
            this.labelPopUpWidth.Location = new System.Drawing.Point(12, 304);
            this.labelPopUpWidth.Name = "labelPopUpWidth";
            this.labelPopUpWidth.Size = new System.Drawing.Size(72, 13);
            this.labelPopUpWidth.TabIndex = 31;
            this.labelPopUpWidth.Text = "Pop up w&idth:";
            // 
            // textBoxPopUpWidth
            // 
            this.textBoxPopUpWidth.Location = new System.Drawing.Point(127, 300);
            this.textBoxPopUpWidth.Name = "textBoxPopUpWidth";
            this.textBoxPopUpWidth.ReadOnly = true;
            this.textBoxPopUpWidth.Size = new System.Drawing.Size(55, 20);
            this.textBoxPopUpWidth.TabIndex = 32;
            this.textBoxPopUpWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPopUpWidth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelPopUpHeight
            // 
            this.labelPopUpHeight.AutoSize = true;
            this.labelPopUpHeight.Location = new System.Drawing.Point(261, 304);
            this.labelPopUpHeight.Name = "labelPopUpHeight";
            this.labelPopUpHeight.Size = new System.Drawing.Size(76, 13);
            this.labelPopUpHeight.TabIndex = 33;
            this.labelPopUpHeight.Text = "Pop up &height:";
            // 
            // textBoxPopUpHeight
            // 
            this.textBoxPopUpHeight.Location = new System.Drawing.Point(376, 300);
            this.textBoxPopUpHeight.Name = "textBoxPopUpHeight";
            this.textBoxPopUpHeight.ReadOnly = true;
            this.textBoxPopUpHeight.Size = new System.Drawing.Size(55, 20);
            this.textBoxPopUpHeight.TabIndex = 34;
            this.textBoxPopUpHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPopUpHeight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelPopUpLeft
            // 
            this.labelPopUpLeft.AutoSize = true;
            this.labelPopUpLeft.Location = new System.Drawing.Point(12, 326);
            this.labelPopUpLeft.Name = "labelPopUpLeft";
            this.labelPopUpLeft.Size = new System.Drawing.Size(61, 13);
            this.labelPopUpLeft.TabIndex = 35;
            this.labelPopUpLeft.Text = "Pop up &left:";
            // 
            // textBoxPopUpLeft
            // 
            this.textBoxPopUpLeft.Location = new System.Drawing.Point(127, 323);
            this.textBoxPopUpLeft.Name = "textBoxPopUpLeft";
            this.textBoxPopUpLeft.ReadOnly = true;
            this.textBoxPopUpLeft.Size = new System.Drawing.Size(55, 20);
            this.textBoxPopUpLeft.TabIndex = 36;
            this.textBoxPopUpLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPopUpLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelPopUpTop
            // 
            this.labelPopUpTop.AutoSize = true;
            this.labelPopUpTop.Location = new System.Drawing.Point(261, 326);
            this.labelPopUpTop.Name = "labelPopUpTop";
            this.labelPopUpTop.Size = new System.Drawing.Size(62, 13);
            this.labelPopUpTop.TabIndex = 37;
            this.labelPopUpTop.Text = "Pop &up top:";
            // 
            // textBoxPopUpTop
            // 
            this.textBoxPopUpTop.Location = new System.Drawing.Point(376, 323);
            this.textBoxPopUpTop.Name = "textBoxPopUpTop";
            this.textBoxPopUpTop.ReadOnly = true;
            this.textBoxPopUpTop.Size = new System.Drawing.Size(55, 20);
            this.textBoxPopUpTop.TabIndex = 38;
            this.textBoxPopUpTop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPopUpTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelIetfLanguageTag
            // 
            this.labelIetfLanguageTag.AutoSize = true;
            this.labelIetfLanguageTag.Location = new System.Drawing.Point(12, 349);
            this.labelIetfLanguageTag.Name = "labelIetfLanguageTag";
            this.labelIetfLanguageTag.Size = new System.Drawing.Size(98, 13);
            this.labelIetfLanguageTag.TabIndex = 39;
            this.labelIetfLanguageTag.Text = "IETF lan&guage tag:";
            // 
            // textBoxIetfLanguageTag
            // 
            this.textBoxIetfLanguageTag.Location = new System.Drawing.Point(127, 346);
            this.textBoxIetfLanguageTag.Name = "textBoxIetfLanguageTag";
            this.textBoxIetfLanguageTag.ReadOnly = true;
            this.textBoxIetfLanguageTag.Size = new System.Drawing.Size(55, 20);
            this.textBoxIetfLanguageTag.TabIndex = 40;
            this.textBoxIetfLanguageTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxIetfLanguageTag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelBackNavigation
            // 
            this.labelBackNavigation.AutoSize = true;
            this.labelBackNavigation.Location = new System.Drawing.Point(261, 349);
            this.labelBackNavigation.Name = "labelBackNavigation";
            this.labelBackNavigation.Size = new System.Drawing.Size(87, 13);
            this.labelBackNavigation.TabIndex = 41;
            this.labelBackNavigation.Text = "&Back navigation:";
            // 
            // textBoxBackNavigation
            // 
            this.textBoxBackNavigation.Location = new System.Drawing.Point(376, 346);
            this.textBoxBackNavigation.Name = "textBoxBackNavigation";
            this.textBoxBackNavigation.ReadOnly = true;
            this.textBoxBackNavigation.Size = new System.Drawing.Size(55, 20);
            this.textBoxBackNavigation.TabIndex = 42;
            this.textBoxBackNavigation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxBackNavigation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelAllowedHosts
            // 
            this.labelAllowedHosts.AutoSize = true;
            this.labelAllowedHosts.Location = new System.Drawing.Point(12, 372);
            this.labelAllowedHosts.Name = "labelAllowedHosts";
            this.labelAllowedHosts.Size = new System.Drawing.Size(75, 13);
            this.labelAllowedHosts.TabIndex = 43;
            this.labelAllowedHosts.Text = "&Allowed hosts:";
            // 
            // textBoxAllowedHosts
            // 
            this.textBoxAllowedHosts.Location = new System.Drawing.Point(127, 369);
            this.textBoxAllowedHosts.Multiline = true;
            this.textBoxAllowedHosts.Name = "textBoxAllowedHosts";
            this.textBoxAllowedHosts.ReadOnly = true;
            this.textBoxAllowedHosts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAllowedHosts.Size = new System.Drawing.Size(304, 45);
            this.textBoxAllowedHosts.TabIndex = 44;
            this.textBoxAllowedHosts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxAllowedHosts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(437, 439);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 47;
            this.buttonOk.Text = "Clos&e";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // checkBoxWillHandlePopUps
            // 
            this.checkBoxWillHandlePopUps.AutoSize = true;
            this.checkBoxWillHandlePopUps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBoxWillHandlePopUps.Location = new System.Drawing.Point(15, 281);
            this.checkBoxWillHandlePopUps.Name = "checkBoxWillHandlePopUps";
            this.checkBoxWillHandlePopUps.Size = new System.Drawing.Size(155, 17);
            this.checkBoxWillHandlePopUps.TabIndex = 29;
            this.checkBoxWillHandlePopUps.Text = "Will actually handle popups";
            this.checkBoxWillHandlePopUps.UseVisualStyleBackColor = true;
            this.checkBoxWillHandlePopUps.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // labelChatHosts
            // 
            this.labelChatHosts.AutoSize = true;
            this.labelChatHosts.Location = new System.Drawing.Point(12, 420);
            this.labelChatHosts.Name = "labelChatHosts";
            this.labelChatHosts.Size = new System.Drawing.Size(60, 13);
            this.labelChatHosts.TabIndex = 45;
            this.labelChatHosts.Text = "Chat ho&sts:";
            // 
            // textBoxChatHosts
            // 
            this.textBoxChatHosts.Location = new System.Drawing.Point(127, 417);
            this.textBoxChatHosts.Multiline = true;
            this.textBoxChatHosts.Name = "textBoxChatHosts";
            this.textBoxChatHosts.ReadOnly = true;
            this.textBoxChatHosts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChatHosts.Size = new System.Drawing.Size(304, 45);
            this.textBoxChatHosts.TabIndex = 46;
            this.textBoxChatHosts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxChatHosts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelUrlLive
            // 
            this.labelUrlLive.AutoSize = true;
            this.labelUrlLive.Location = new System.Drawing.Point(12, 61);
            this.labelUrlLive.Name = "labelUrlLive";
            this.labelUrlLive.Size = new System.Drawing.Size(55, 13);
            this.labelUrlLive.TabIndex = 6;
            this.labelUrlLive.Text = "Li&ve URL:";
            // 
            // textBoxUrlLive
            // 
            this.textBoxUrlLive.Location = new System.Drawing.Point(127, 58);
            this.textBoxUrlLive.Name = "textBoxUrlLive";
            this.textBoxUrlLive.ReadOnly = true;
            this.textBoxUrlLive.Size = new System.Drawing.Size(304, 20);
            this.textBoxUrlLive.TabIndex = 7;
            this.textBoxUrlLive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrlLive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxIsService
            // 
            this.checkBoxIsService.AutoSize = true;
            this.checkBoxIsService.Location = new System.Drawing.Point(15, 243);
            this.checkBoxIsService.Name = "checkBoxIsService";
            this.checkBoxIsService.Size = new System.Drawing.Size(71, 17);
            this.checkBoxIsService.TabIndex = 25;
            this.checkBoxIsService.Text = "Is service";
            this.checkBoxIsService.UseVisualStyleBackColor = true;
            this.checkBoxIsService.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // checkBoxAudioMutedByDefault
            // 
            this.checkBoxAudioMutedByDefault.AutoSize = true;
            this.checkBoxAudioMutedByDefault.Location = new System.Drawing.Point(264, 243);
            this.checkBoxAudioMutedByDefault.Name = "checkBoxAudioMutedByDefault";
            this.checkBoxAudioMutedByDefault.Size = new System.Drawing.Size(134, 17);
            this.checkBoxAudioMutedByDefault.TabIndex = 26;
            this.checkBoxAudioMutedByDefault.Text = "Audio muted by default";
            this.checkBoxAudioMutedByDefault.UseVisualStyleBackColor = true;
            this.checkBoxAudioMutedByDefault.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // labelOrdinal
            // 
            this.labelOrdinal.AutoSize = true;
            this.labelOrdinal.Location = new System.Drawing.Point(437, 15);
            this.labelOrdinal.Name = "labelOrdinal";
            this.labelOrdinal.Size = new System.Drawing.Size(62, 13);
            this.labelOrdinal.TabIndex = 2;
            this.labelOrdinal.Text = "labelOrdinal";
            // 
            // labelUrlNext
            // 
            this.labelUrlNext.AutoSize = true;
            this.labelUrlNext.Location = new System.Drawing.Point(12, 84);
            this.labelUrlNext.Name = "labelUrlNext";
            this.labelUrlNext.Size = new System.Drawing.Size(92, 13);
            this.labelUrlNext.TabIndex = 8;
            this.labelUrlNext.Text = "Ne&xt URL to load:";
            // 
            // textBoxUrlNext
            // 
            this.textBoxUrlNext.Location = new System.Drawing.Point(127, 81);
            this.textBoxUrlNext.Name = "textBoxUrlNext";
            this.textBoxUrlNext.ReadOnly = true;
            this.textBoxUrlNext.Size = new System.Drawing.Size(304, 20);
            this.textBoxUrlNext.TabIndex = 9;
            this.textBoxUrlNext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrlNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(127, 127);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.ReadOnly = true;
            this.textBoxPassword.Size = new System.Drawing.Size(304, 20);
            this.textBoxPassword.TabIndex = 15;
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxTabNavigation
            // 
            this.checkBoxTabNavigation.AutoSize = true;
            this.checkBoxTabNavigation.Location = new System.Drawing.Point(264, 262);
            this.checkBoxTabNavigation.Name = "checkBoxTabNavigation";
            this.checkBoxTabNavigation.Size = new System.Drawing.Size(170, 17);
            this.checkBoxTabNavigation.TabIndex = 28;
            this.checkBoxTabNavigation.Text = "Allow navigation between tabs";
            this.checkBoxTabNavigation.UseVisualStyleBackColor = true;
            this.checkBoxTabNavigation.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // checkBoxWillTryToKeep
            // 
            this.checkBoxWillTryToKeep.AutoSize = true;
            this.checkBoxWillTryToKeep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBoxWillTryToKeep.Location = new System.Drawing.Point(264, 281);
            this.checkBoxWillTryToKeep.Name = "checkBoxWillTryToKeep";
            this.checkBoxWillTryToKeep.Size = new System.Drawing.Size(165, 17);
            this.checkBoxWillTryToKeep.TabIndex = 30;
            this.checkBoxWillTryToKeep.Text = "Will try to keep user logged in";
            this.checkBoxWillTryToKeep.UseVisualStyleBackColor = true;
            this.checkBoxWillTryToKeep.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // WebInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(524, 474);
            this.Controls.Add(this.checkBoxWillTryToKeep);
            this.Controls.Add(this.checkBoxTabNavigation);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUrlNext);
            this.Controls.Add(this.labelUrlNext);
            this.Controls.Add(this.labelOrdinal);
            this.Controls.Add(this.checkBoxAudioMutedByDefault);
            this.Controls.Add(this.checkBoxIsService);
            this.Controls.Add(this.textBoxUrlLive);
            this.Controls.Add(this.labelUrlLive);
            this.Controls.Add(this.textBoxChatHosts);
            this.Controls.Add(this.labelChatHosts);
            this.Controls.Add(this.checkBoxWillHandlePopUps);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxAllowedHosts);
            this.Controls.Add(this.labelAllowedHosts);
            this.Controls.Add(this.textBoxBackNavigation);
            this.Controls.Add(this.labelBackNavigation);
            this.Controls.Add(this.textBoxIetfLanguageTag);
            this.Controls.Add(this.labelIetfLanguageTag);
            this.Controls.Add(this.textBoxPopUpTop);
            this.Controls.Add(this.labelPopUpTop);
            this.Controls.Add(this.textBoxPopUpLeft);
            this.Controls.Add(this.labelPopUpLeft);
            this.Controls.Add(this.textBoxPopUpHeight);
            this.Controls.Add(this.labelPopUpHeight);
            this.Controls.Add(this.textBoxPopUpWidth);
            this.Controls.Add(this.labelPopUpWidth);
            this.Controls.Add(this.checkBoxHandlePopUps);
            this.Controls.Add(this.textBoxDisplayName);
            this.Controls.Add(this.labelDisplayName);
            this.Controls.Add(this.textBoxFields);
            this.Controls.Add(this.labelFields);
            this.Controls.Add(this.textBoxPattern);
            this.Controls.Add(this.labelPattern);
            this.Controls.Add(this.textBoxScript);
            this.Controls.Add(this.labelScript);
            this.Controls.Add(this.buttonCopyPassword);
            this.Controls.Add(this.maskedTextBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.buttonCopyUserName);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.buttonCopyUrl);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "WebInfoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonCopyUrl;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Button buttonCopyUserName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPassword;
        private System.Windows.Forms.Button buttonCopyPassword;
        private System.Windows.Forms.Label labelScript;
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.Label labelPattern;
        private System.Windows.Forms.TextBox textBoxPattern;
        private System.Windows.Forms.Label labelFields;
        private System.Windows.Forms.TextBox textBoxFields;
        private System.Windows.Forms.Label labelDisplayName;
        private System.Windows.Forms.TextBox textBoxDisplayName;
        private System.Windows.Forms.CheckBox checkBoxHandlePopUps;
        private System.Windows.Forms.Label labelPopUpWidth;
        private System.Windows.Forms.TextBox textBoxPopUpWidth;
        private System.Windows.Forms.Label labelPopUpHeight;
        private System.Windows.Forms.TextBox textBoxPopUpHeight;
        private System.Windows.Forms.Label labelPopUpLeft;
        private System.Windows.Forms.TextBox textBoxPopUpLeft;
        private System.Windows.Forms.Label labelPopUpTop;
        private System.Windows.Forms.TextBox textBoxPopUpTop;
        private System.Windows.Forms.Label labelIetfLanguageTag;
        private System.Windows.Forms.TextBox textBoxIetfLanguageTag;
        private System.Windows.Forms.Label labelBackNavigation;
        private System.Windows.Forms.TextBox textBoxBackNavigation;
        private System.Windows.Forms.Label labelAllowedHosts;
        private System.Windows.Forms.TextBox textBoxAllowedHosts;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBoxWillHandlePopUps;
        private System.Windows.Forms.Label labelChatHosts;
        private System.Windows.Forms.TextBox textBoxChatHosts;
        private System.Windows.Forms.Label labelUrlLive;
        private System.Windows.Forms.TextBox textBoxUrlLive;
        private System.Windows.Forms.CheckBox checkBoxIsService;
        private System.Windows.Forms.CheckBox checkBoxAudioMutedByDefault;
        private System.Windows.Forms.Label labelOrdinal;
        private System.Windows.Forms.Label labelUrlNext;
        private System.Windows.Forms.TextBox textBoxUrlNext;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxTabNavigation;
        private System.Windows.Forms.CheckBox checkBoxWillTryToKeep;
    }
}