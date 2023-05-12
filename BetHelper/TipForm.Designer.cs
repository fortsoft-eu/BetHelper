/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
    partial class TipForm {
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
            this.labelBookmaker = new System.Windows.Forms.Label();
            this.textBoxBookmaker = new System.Windows.Forms.TextBox();
            this.labelOdd = new System.Windows.Forms.Label();
            this.textBoxOdd = new System.Windows.Forms.TextBox();
            this.labelTrustDegree = new System.Windows.Forms.Label();
            this.textBoxTrustDegree = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.buttonAddGame = new System.Windows.Forms.Button();
            this.buttonRemoveGame = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.textBoxService = new System.Windows.Forms.TextBox();
            this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelBookmaker
            // 
            this.labelBookmaker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBookmaker.AutoSize = true;
            this.labelBookmaker.Location = new System.Drawing.Point(185, 184);
            this.labelBookmaker.Name = "labelBookmaker";
            this.labelBookmaker.Size = new System.Drawing.Size(64, 13);
            this.labelBookmaker.TabIndex = 5;
            this.labelBookmaker.Text = "&Bookmaker:";
            // 
            // textBoxBookmaker
            // 
            this.textBoxBookmaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBookmaker.Location = new System.Drawing.Point(271, 181);
            this.textBoxBookmaker.Name = "textBoxBookmaker";
            this.textBoxBookmaker.Size = new System.Drawing.Size(121, 20);
            this.textBoxBookmaker.TabIndex = 6;
            this.textBoxBookmaker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxBookmaker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelOdd
            // 
            this.labelOdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOdd.AutoSize = true;
            this.labelOdd.Location = new System.Drawing.Point(22, 184);
            this.labelOdd.Name = "labelOdd";
            this.labelOdd.Size = new System.Drawing.Size(30, 13);
            this.labelOdd.TabIndex = 3;
            this.labelOdd.Text = "&Odd:";
            // 
            // textBoxOdd
            // 
            this.textBoxOdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxOdd.Location = new System.Drawing.Point(108, 181);
            this.textBoxOdd.Name = "textBoxOdd";
            this.textBoxOdd.Size = new System.Drawing.Size(50, 20);
            this.textBoxOdd.TabIndex = 4;
            this.textBoxOdd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxOdd.Leave += new System.EventHandler(this.OnTextBoxOddLeave);
            this.textBoxOdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelTrustDegree
            // 
            this.labelTrustDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrustDegree.AutoSize = true;
            this.labelTrustDegree.Location = new System.Drawing.Point(22, 210);
            this.labelTrustDegree.Name = "labelTrustDegree";
            this.labelTrustDegree.Size = new System.Drawing.Size(70, 13);
            this.labelTrustDegree.TabIndex = 7;
            this.labelTrustDegree.Text = "Trust &degree:";
            // 
            // textBoxTrustDegree
            // 
            this.textBoxTrustDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTrustDegree.Location = new System.Drawing.Point(108, 207);
            this.textBoxTrustDegree.Name = "textBoxTrustDegree";
            this.textBoxTrustDegree.Size = new System.Drawing.Size(50, 20);
            this.textBoxTrustDegree.TabIndex = 8;
            this.textBoxTrustDegree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxTrustDegree.Leave += new System.EventHandler(this.OnTextBoxTrustDegreeLeave);
            this.textBoxTrustDegree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(279, 161);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonAddGame
            // 
            this.buttonAddGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddGame.Location = new System.Drawing.Point(297, 35);
            this.buttonAddGame.Name = "buttonAddGame";
            this.buttonAddGame.Size = new System.Drawing.Size(95, 23);
            this.buttonAddGame.TabIndex = 0;
            this.buttonAddGame.Text = "&Add Match";
            this.buttonAddGame.UseVisualStyleBackColor = true;
            this.buttonAddGame.Click += new System.EventHandler(this.AddGame);
            this.buttonAddGame.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonRemoveGame
            // 
            this.buttonRemoveGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveGame.Location = new System.Drawing.Point(297, 139);
            this.buttonRemoveGame.Name = "buttonRemoveGame";
            this.buttonRemoveGame.Size = new System.Drawing.Size(95, 23);
            this.buttonRemoveGame.TabIndex = 1;
            this.buttonRemoveGame.Text = "&Remove Match";
            this.buttonRemoveGame.UseVisualStyleBackColor = true;
            this.buttonRemoveGame.Click += new System.EventHandler(this.RemoveGame);
            this.buttonRemoveGame.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(236, 233);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 13;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.Save);
            this.buttonSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(317, 233);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "Canc&el";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(22, 237);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.TabIndex = 11;
            this.labelStatus.Text = "Stat&us";
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(108, 234);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(80, 21);
            this.comboBoxStatus.TabIndex = 12;
            this.comboBoxStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // labelService
            // 
            this.labelService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelService.AutoSize = true;
            this.labelService.Location = new System.Drawing.Point(185, 210);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(75, 13);
            this.labelService.TabIndex = 9;
            this.labelService.Text = "Ser&vice name:";
            // 
            // textBoxService
            // 
            this.textBoxService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxService.Location = new System.Drawing.Point(271, 207);
            this.textBoxService.Name = "textBoxService";
            this.textBoxService.Size = new System.Drawing.Size(121, 20);
            this.textBoxService.TabIndex = 10;
            this.textBoxService.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxService.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // checkBoxTopMost
            // 
            this.checkBoxTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTopMost.AutoSize = true;
            this.checkBoxTopMost.Location = new System.Drawing.Point(298, 12);
            this.checkBoxTopMost.Name = "checkBoxTopMost";
            this.checkBoxTopMost.Size = new System.Drawing.Size(70, 17);
            this.checkBoxTopMost.TabIndex = 15;
            this.checkBoxTopMost.Text = "&Top most";
            this.checkBoxTopMost.UseVisualStyleBackColor = true;
            this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.OnTopMostCheckedChanged);
            this.checkBoxTopMost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // TipForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(404, 268);
            this.Controls.Add(this.checkBoxTopMost);
            this.Controls.Add(this.textBoxService);
            this.Controls.Add(this.labelService);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonRemoveGame);
            this.Controls.Add(this.buttonAddGame);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBoxTrustDegree);
            this.Controls.Add(this.labelTrustDegree);
            this.Controls.Add(this.textBoxOdd);
            this.Controls.Add(this.labelOdd);
            this.Controls.Add(this.textBoxBookmaker);
            this.Controls.Add(this.labelBookmaker);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 307);
            this.Name = "TipForm";
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelBookmaker;
        private System.Windows.Forms.TextBox textBoxBookmaker;
        private System.Windows.Forms.Label labelOdd;
        private System.Windows.Forms.TextBox textBoxOdd;
        private System.Windows.Forms.Label labelTrustDegree;
        private System.Windows.Forms.TextBox textBoxTrustDegree;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button buttonAddGame;
        private System.Windows.Forms.Button buttonRemoveGame;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.TextBox textBoxService;
        private System.Windows.Forms.CheckBox checkBoxTopMost;
    }
}