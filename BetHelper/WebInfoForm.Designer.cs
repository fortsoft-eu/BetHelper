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
 * Version 1.1.9.0
 */

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
            this.labelUrlNext = new System.Windows.Forms.Label();
            this.textBoxUrlNext = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxTabNavigation = new System.Windows.Forms.CheckBox();
            this.checkBoxWillTryToKeep = new System.Windows.Forms.CheckBox();
            this.labelTips = new System.Windows.Forms.Label();
            this.textBoxTips = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelChatHosts = new System.Windows.Forms.Panel();
            this.panelAllowedHosts = new System.Windows.Forms.Panel();
            this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            this.panelChatHosts.SuspendLayout();
            this.panelAllowedHosts.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(30, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Titl&e:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.Location = new System.Drawing.Point(138, 12);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.ReadOnly = true;
            this.textBoxTitle.Size = new System.Drawing.Size(253, 20);
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
            this.labelUrl.TabIndex = 2;
            this.labelUrl.Text = "U&RL:";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(138, 35);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.ReadOnly = true;
            this.textBoxUrl.Size = new System.Drawing.Size(253, 20);
            this.textBoxUrl.TabIndex = 3;
            this.textBoxUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonCopyUrl
            // 
            this.buttonCopyUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyUrl.Location = new System.Drawing.Point(397, 33);
            this.buttonCopyUrl.Name = "buttonCopyUrl";
            this.buttonCopyUrl.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyUrl.TabIndex = 4;
            this.buttonCopyUrl.Text = "Co&py";
            this.buttonCopyUrl.UseVisualStyleBackColor = true;
            this.buttonCopyUrl.Click += new System.EventHandler(this.OnCopyUrlClick);
            this.buttonCopyUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(12, 130);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(61, 13);
            this.labelUserName.TabIndex = 11;
            this.labelUserName.Text = "User na&me:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserName.Location = new System.Drawing.Point(138, 127);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(253, 20);
            this.textBoxUserName.TabIndex = 12;
            this.textBoxUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUserName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonCopyUserName
            // 
            this.buttonCopyUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyUserName.Location = new System.Drawing.Point(397, 125);
            this.buttonCopyUserName.Name = "buttonCopyUserName";
            this.buttonCopyUserName.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyUserName.TabIndex = 14;
            this.buttonCopyUserName.Text = "C&opy";
            this.buttonCopyUserName.UseVisualStyleBackColor = true;
            this.buttonCopyUserName.Click += new System.EventHandler(this.OnCopyUserNameClick);
            this.buttonCopyUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(12, 153);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 13;
            this.labelPassword.Text = "Pass&word:";
            // 
            // maskedTextBoxPassword
            // 
            this.maskedTextBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxPassword.Location = new System.Drawing.Point(138, 150);
            this.maskedTextBoxPassword.Name = "maskedTextBoxPassword";
            this.maskedTextBoxPassword.ReadOnly = true;
            this.maskedTextBoxPassword.Size = new System.Drawing.Size(253, 20);
            this.maskedTextBoxPassword.TabIndex = 14;
            this.maskedTextBoxPassword.UseSystemPasswordChar = true;
            this.maskedTextBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.maskedTextBoxPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPasswordMouseDown);
            // 
            // buttonCopyPassword
            // 
            this.buttonCopyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyPassword.Location = new System.Drawing.Point(397, 148);
            this.buttonCopyPassword.Name = "buttonCopyPassword";
            this.buttonCopyPassword.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyPassword.TabIndex = 16;
            this.buttonCopyPassword.Text = "&Copy";
            this.buttonCopyPassword.UseVisualStyleBackColor = true;
            this.buttonCopyPassword.Click += new System.EventHandler(this.OnCopyPasswordClick);
            this.buttonCopyPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelScript
            // 
            this.labelScript.AutoSize = true;
            this.labelScript.Location = new System.Drawing.Point(12, 176);
            this.labelScript.Name = "labelScript";
            this.labelScript.Size = new System.Drawing.Size(60, 13);
            this.labelScript.TabIndex = 17;
            this.labelScript.Text = "&JavaScript:";
            // 
            // textBoxScript
            // 
            this.textBoxScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScript.Location = new System.Drawing.Point(138, 173);
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ReadOnly = true;
            this.textBoxScript.Size = new System.Drawing.Size(253, 20);
            this.textBoxScript.TabIndex = 18;
            this.textBoxScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxScript.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelPattern
            // 
            this.labelPattern.AutoSize = true;
            this.labelPattern.Location = new System.Drawing.Point(12, 199);
            this.labelPattern.Name = "labelPattern";
            this.labelPattern.Size = new System.Drawing.Size(44, 13);
            this.labelPattern.TabIndex = 19;
            this.labelPattern.Text = "Patter&n:";
            // 
            // textBoxPattern
            // 
            this.textBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPattern.Location = new System.Drawing.Point(138, 196);
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.ReadOnly = true;
            this.textBoxPattern.Size = new System.Drawing.Size(253, 20);
            this.textBoxPattern.TabIndex = 20;
            this.textBoxPattern.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPattern.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelFields
            // 
            this.labelFields.AutoSize = true;
            this.labelFields.Location = new System.Drawing.Point(12, 222);
            this.labelFields.Name = "labelFields";
            this.labelFields.Size = new System.Drawing.Size(37, 13);
            this.labelFields.TabIndex = 21;
            this.labelFields.Text = "&Fields:";
            // 
            // textBoxFields
            // 
            this.textBoxFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFields.Location = new System.Drawing.Point(138, 219);
            this.textBoxFields.Name = "textBoxFields";
            this.textBoxFields.ReadOnly = true;
            this.textBoxFields.Size = new System.Drawing.Size(253, 20);
            this.textBoxFields.TabIndex = 22;
            this.textBoxFields.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.AutoSize = true;
            this.labelDisplayName.Location = new System.Drawing.Point(12, 245);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(73, 13);
            this.labelDisplayName.TabIndex = 23;
            this.labelDisplayName.Text = "Displa&y name:";
            // 
            // textBoxDisplayName
            // 
            this.textBoxDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDisplayName.Location = new System.Drawing.Point(138, 242);
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.ReadOnly = true;
            this.textBoxDisplayName.Size = new System.Drawing.Size(253, 20);
            this.textBoxDisplayName.TabIndex = 24;
            this.textBoxDisplayName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxDisplayName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxHandlePopUps
            // 
            this.checkBoxHandlePopUps.AutoSize = true;
            this.checkBoxHandlePopUps.Location = new System.Drawing.Point(15, 285);
            this.checkBoxHandlePopUps.Name = "checkBoxHandlePopUps";
            this.checkBoxHandlePopUps.Size = new System.Drawing.Size(98, 17);
            this.checkBoxHandlePopUps.TabIndex = 27;
            this.checkBoxHandlePopUps.Text = "Handle popups";
            this.checkBoxHandlePopUps.UseVisualStyleBackColor = true;
            this.checkBoxHandlePopUps.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxHandlePopUps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelPopUpWidth
            // 
            this.labelPopUpWidth.AutoSize = true;
            this.labelPopUpWidth.Location = new System.Drawing.Point(12, 326);
            this.labelPopUpWidth.Name = "labelPopUpWidth";
            this.labelPopUpWidth.Size = new System.Drawing.Size(72, 13);
            this.labelPopUpWidth.TabIndex = 31;
            this.labelPopUpWidth.Text = "Pop up wi&dth:";
            // 
            // textBoxPopUpWidth
            // 
            this.textBoxPopUpWidth.Location = new System.Drawing.Point(138, 323);
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
            this.labelPopUpHeight.Location = new System.Drawing.Point(210, 326);
            this.labelPopUpHeight.Name = "labelPopUpHeight";
            this.labelPopUpHeight.Size = new System.Drawing.Size(76, 13);
            this.labelPopUpHeight.TabIndex = 33;
            this.labelPopUpHeight.Text = "Pop up &height:";
            // 
            // textBoxPopUpHeight
            // 
            this.textBoxPopUpHeight.Location = new System.Drawing.Point(336, 323);
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
            this.labelPopUpLeft.Location = new System.Drawing.Point(12, 349);
            this.labelPopUpLeft.Name = "labelPopUpLeft";
            this.labelPopUpLeft.Size = new System.Drawing.Size(61, 13);
            this.labelPopUpLeft.TabIndex = 35;
            this.labelPopUpLeft.Text = "Pop up &left:";
            // 
            // textBoxPopUpLeft
            // 
            this.textBoxPopUpLeft.Location = new System.Drawing.Point(138, 346);
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
            this.labelPopUpTop.Location = new System.Drawing.Point(210, 349);
            this.labelPopUpTop.Name = "labelPopUpTop";
            this.labelPopUpTop.Size = new System.Drawing.Size(62, 13);
            this.labelPopUpTop.TabIndex = 37;
            this.labelPopUpTop.Text = "Pop &up top:";
            // 
            // textBoxPopUpTop
            // 
            this.textBoxPopUpTop.Location = new System.Drawing.Point(336, 346);
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
            this.labelIetfLanguageTag.Location = new System.Drawing.Point(12, 372);
            this.labelIetfLanguageTag.Name = "labelIetfLanguageTag";
            this.labelIetfLanguageTag.Size = new System.Drawing.Size(98, 13);
            this.labelIetfLanguageTag.TabIndex = 39;
            this.labelIetfLanguageTag.Text = "IETF lan&guage tag:";
            // 
            // textBoxIetfLanguageTag
            // 
            this.textBoxIetfLanguageTag.Location = new System.Drawing.Point(138, 369);
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
            this.labelBackNavigation.Location = new System.Drawing.Point(210, 372);
            this.labelBackNavigation.Name = "labelBackNavigation";
            this.labelBackNavigation.Size = new System.Drawing.Size(87, 13);
            this.labelBackNavigation.TabIndex = 41;
            this.labelBackNavigation.Text = "&Back navigation:";
            // 
            // textBoxBackNavigation
            // 
            this.textBoxBackNavigation.Location = new System.Drawing.Point(336, 369);
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
            this.labelAllowedHosts.Location = new System.Drawing.Point(0, 3);
            this.labelAllowedHosts.Name = "labelAllowedHosts";
            this.labelAllowedHosts.Size = new System.Drawing.Size(75, 13);
            this.labelAllowedHosts.TabIndex = 0;
            this.labelAllowedHosts.Text = "&Allowed hosts:";
            // 
            // textBoxAllowedHosts
            // 
            this.textBoxAllowedHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAllowedHosts.Location = new System.Drawing.Point(126, 0);
            this.textBoxAllowedHosts.Multiline = true;
            this.textBoxAllowedHosts.Name = "textBoxAllowedHosts";
            this.textBoxAllowedHosts.ReadOnly = true;
            this.textBoxAllowedHosts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAllowedHosts.Size = new System.Drawing.Size(253, 45);
            this.textBoxAllowedHosts.TabIndex = 1;
            this.textBoxAllowedHosts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxAllowedHosts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(397, 462);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 44;
            this.buttonOk.Text = "O&K";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxWillHandlePopUps
            // 
            this.checkBoxWillHandlePopUps.AutoSize = true;
            this.checkBoxWillHandlePopUps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBoxWillHandlePopUps.Location = new System.Drawing.Point(15, 304);
            this.checkBoxWillHandlePopUps.Name = "checkBoxWillHandlePopUps";
            this.checkBoxWillHandlePopUps.Size = new System.Drawing.Size(155, 17);
            this.checkBoxWillHandlePopUps.TabIndex = 29;
            this.checkBoxWillHandlePopUps.Text = "Will actually handle popups";
            this.checkBoxWillHandlePopUps.UseVisualStyleBackColor = true;
            this.checkBoxWillHandlePopUps.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxWillHandlePopUps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelChatHosts
            // 
            this.labelChatHosts.AutoSize = true;
            this.labelChatHosts.Location = new System.Drawing.Point(0, 5);
            this.labelChatHosts.Name = "labelChatHosts";
            this.labelChatHosts.Size = new System.Drawing.Size(60, 13);
            this.labelChatHosts.TabIndex = 0;
            this.labelChatHosts.Text = "Chat ho&sts:";
            // 
            // textBoxChatHosts
            // 
            this.textBoxChatHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChatHosts.Location = new System.Drawing.Point(126, 2);
            this.textBoxChatHosts.Multiline = true;
            this.textBoxChatHosts.Name = "textBoxChatHosts";
            this.textBoxChatHosts.ReadOnly = true;
            this.textBoxChatHosts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChatHosts.Size = new System.Drawing.Size(253, 45);
            this.textBoxChatHosts.TabIndex = 1;
            this.textBoxChatHosts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxChatHosts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // labelUrlLive
            // 
            this.labelUrlLive.AutoSize = true;
            this.labelUrlLive.Location = new System.Drawing.Point(12, 61);
            this.labelUrlLive.Name = "labelUrlLive";
            this.labelUrlLive.Size = new System.Drawing.Size(55, 13);
            this.labelUrlLive.TabIndex = 5;
            this.labelUrlLive.Text = "Li&ve URL:";
            // 
            // textBoxUrlLive
            // 
            this.textBoxUrlLive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrlLive.Location = new System.Drawing.Point(138, 58);
            this.textBoxUrlLive.Name = "textBoxUrlLive";
            this.textBoxUrlLive.ReadOnly = true;
            this.textBoxUrlLive.Size = new System.Drawing.Size(253, 20);
            this.textBoxUrlLive.TabIndex = 6;
            this.textBoxUrlLive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrlLive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxIsService
            // 
            this.checkBoxIsService.AutoSize = true;
            this.checkBoxIsService.Location = new System.Drawing.Point(15, 266);
            this.checkBoxIsService.Name = "checkBoxIsService";
            this.checkBoxIsService.Size = new System.Drawing.Size(71, 17);
            this.checkBoxIsService.TabIndex = 25;
            this.checkBoxIsService.Text = "Is service";
            this.checkBoxIsService.UseVisualStyleBackColor = true;
            this.checkBoxIsService.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxIsService.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxAudioMutedByDefault
            // 
            this.checkBoxAudioMutedByDefault.AutoSize = true;
            this.checkBoxAudioMutedByDefault.Location = new System.Drawing.Point(213, 266);
            this.checkBoxAudioMutedByDefault.Name = "checkBoxAudioMutedByDefault";
            this.checkBoxAudioMutedByDefault.Size = new System.Drawing.Size(134, 17);
            this.checkBoxAudioMutedByDefault.TabIndex = 26;
            this.checkBoxAudioMutedByDefault.Text = "Audio muted by default";
            this.checkBoxAudioMutedByDefault.UseVisualStyleBackColor = true;
            this.checkBoxAudioMutedByDefault.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxAudioMutedByDefault.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelUrlNext
            // 
            this.labelUrlNext.AutoSize = true;
            this.labelUrlNext.Location = new System.Drawing.Point(12, 84);
            this.labelUrlNext.Name = "labelUrlNext";
            this.labelUrlNext.Size = new System.Drawing.Size(92, 13);
            this.labelUrlNext.TabIndex = 7;
            this.labelUrlNext.Text = "Ne&xt URL to load:";
            // 
            // textBoxUrlNext
            // 
            this.textBoxUrlNext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrlNext.Location = new System.Drawing.Point(138, 81);
            this.textBoxUrlNext.Name = "textBoxUrlNext";
            this.textBoxUrlNext.ReadOnly = true;
            this.textBoxUrlNext.Size = new System.Drawing.Size(253, 20);
            this.textBoxUrlNext.TabIndex = 8;
            this.textBoxUrlNext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxUrlNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.Location = new System.Drawing.Point(138, 150);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.ReadOnly = true;
            this.textBoxPassword.Size = new System.Drawing.Size(253, 20);
            this.textBoxPassword.TabIndex = 15;
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // checkBoxTabNavigation
            // 
            this.checkBoxTabNavigation.AutoSize = true;
            this.checkBoxTabNavigation.Location = new System.Drawing.Point(213, 285);
            this.checkBoxTabNavigation.Name = "checkBoxTabNavigation";
            this.checkBoxTabNavigation.Size = new System.Drawing.Size(170, 17);
            this.checkBoxTabNavigation.TabIndex = 28;
            this.checkBoxTabNavigation.Text = "Allow navigation between tabs";
            this.checkBoxTabNavigation.UseVisualStyleBackColor = true;
            this.checkBoxTabNavigation.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxTabNavigation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // checkBoxWillTryToKeep
            // 
            this.checkBoxWillTryToKeep.AutoSize = true;
            this.checkBoxWillTryToKeep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBoxWillTryToKeep.Location = new System.Drawing.Point(213, 304);
            this.checkBoxWillTryToKeep.Name = "checkBoxWillTryToKeep";
            this.checkBoxWillTryToKeep.Size = new System.Drawing.Size(183, 17);
            this.checkBoxWillTryToKeep.TabIndex = 30;
            this.checkBoxWillTryToKeep.Text = "Will try to keep the user logged in";
            this.checkBoxWillTryToKeep.UseVisualStyleBackColor = true;
            this.checkBoxWillTryToKeep.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            this.checkBoxWillTryToKeep.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelTips
            // 
            this.labelTips.AutoSize = true;
            this.labelTips.Location = new System.Drawing.Point(12, 107);
            this.labelTips.Name = "labelTips";
            this.labelTips.Size = new System.Drawing.Size(55, 13);
            this.labelTips.TabIndex = 9;
            this.labelTips.Text = "T&ips URL:";
            // 
            // textBoxTips
            // 
            this.textBoxTips.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTips.Location = new System.Drawing.Point(138, 104);
            this.textBoxTips.Name = "textBoxTips";
            this.textBoxTips.ReadOnly = true;
            this.textBoxTips.Size = new System.Drawing.Size(253, 20);
            this.textBoxTips.TabIndex = 10;
            this.textBoxTips.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxTips.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.panelChatHosts, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panelAllowedHosts, 0, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 392);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(379, 93);
            this.tableLayoutPanel.TabIndex = 43;
            // 
            // panelChatHosts
            // 
            this.panelChatHosts.Controls.Add(this.textBoxChatHosts);
            this.panelChatHosts.Controls.Add(this.labelChatHosts);
            this.panelChatHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChatHosts.Location = new System.Drawing.Point(0, 46);
            this.panelChatHosts.Margin = new System.Windows.Forms.Padding(0);
            this.panelChatHosts.Name = "panelChatHosts";
            this.panelChatHosts.Size = new System.Drawing.Size(379, 47);
            this.panelChatHosts.TabIndex = 0;
            // 
            // panelAllowedHosts
            // 
            this.panelAllowedHosts.Controls.Add(this.textBoxAllowedHosts);
            this.panelAllowedHosts.Controls.Add(this.labelAllowedHosts);
            this.panelAllowedHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAllowedHosts.Location = new System.Drawing.Point(0, 0);
            this.panelAllowedHosts.Margin = new System.Windows.Forms.Padding(0);
            this.panelAllowedHosts.Name = "panelAllowedHosts";
            this.panelAllowedHosts.Size = new System.Drawing.Size(379, 46);
            this.panelAllowedHosts.TabIndex = 0;
            // 
            // checkBoxTopMost
            // 
            this.checkBoxTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTopMost.AutoSize = true;
            this.checkBoxTopMost.Location = new System.Drawing.Point(398, 14);
            this.checkBoxTopMost.Name = "checkBoxTopMost";
            this.checkBoxTopMost.Size = new System.Drawing.Size(70, 17);
            this.checkBoxTopMost.TabIndex = 45;
            this.checkBoxTopMost.Text = "&Top most";
            this.checkBoxTopMost.UseVisualStyleBackColor = true;
            this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.OnTopMostCheckedChanged);
            this.checkBoxTopMost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // WebInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(484, 497);
            this.Controls.Add(this.checkBoxTopMost);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.textBoxTips);
            this.Controls.Add(this.labelTips);
            this.Controls.Add(this.checkBoxWillTryToKeep);
            this.Controls.Add(this.checkBoxTabNavigation);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUrlNext);
            this.Controls.Add(this.labelUrlNext);
            this.Controls.Add(this.checkBoxAudioMutedByDefault);
            this.Controls.Add(this.checkBoxIsService);
            this.Controls.Add(this.textBoxUrlLive);
            this.Controls.Add(this.labelUrlLive);
            this.Controls.Add(this.checkBoxWillHandlePopUps);
            this.Controls.Add(this.buttonOk);
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
            this.MinimumSize = new System.Drawing.Size(500, 536);
            this.Name = "WebInfoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.SizeChanged += new System.EventHandler(this.GripStyle);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.Move += new System.EventHandler(this.GripStyle);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panelChatHosts.ResumeLayout(false);
            this.panelChatHosts.PerformLayout();
            this.panelAllowedHosts.ResumeLayout(false);
            this.panelAllowedHosts.PerformLayout();
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
        private System.Windows.Forms.Label labelUrlNext;
        private System.Windows.Forms.TextBox textBoxUrlNext;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxTabNavigation;
        private System.Windows.Forms.CheckBox checkBoxWillTryToKeep;
        private System.Windows.Forms.Label labelTips;
        private System.Windows.Forms.TextBox textBoxTips;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panelChatHosts;
        private System.Windows.Forms.Panel panelAllowedHosts;
        private System.Windows.Forms.CheckBox checkBoxTopMost;
    }
}