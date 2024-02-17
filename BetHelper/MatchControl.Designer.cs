/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.4
 */

namespace BetHelper {
    partial class MatchControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.textBoxOpportunity = new System.Windows.Forms.TextBox();
            this.labelOpportunity = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.textBoxLeague = new System.Windows.Forms.TextBox();
            this.labelLeague = new System.Windows.Forms.Label();
            this.textBoxMatch = new System.Windows.Forms.TextBox();
            this.labelGame = new System.Windows.Forms.Label();
            this.textBoxSport = new System.Windows.Forms.TextBox();
            this.labelSport = new System.Windows.Forms.Label();
            this.textBoxClicksTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBoxOpportunity
            // 
            this.textBoxOpportunity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOpportunity.Location = new System.Drawing.Point(92, 107);
            this.textBoxOpportunity.Name = "textBoxOpportunity";
            this.textBoxOpportunity.Size = new System.Drawing.Size(180, 20);
            this.textBoxOpportunity.TabIndex = 9;
            this.textBoxOpportunity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxOpportunity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelOpportunity
            // 
            this.labelOpportunity.AutoSize = true;
            this.labelOpportunity.Location = new System.Drawing.Point(6, 110);
            this.labelOpportunity.Name = "labelOpportunity";
            this.labelOpportunity.Size = new System.Drawing.Size(64, 13);
            this.labelOpportunity.TabIndex = 8;
            this.labelOpportunity.Text = "Opportu&nity:";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.Location = new System.Drawing.Point(92, 81);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(180, 20);
            this.dateTimePicker.TabIndex = 7;
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.Location = new System.Drawing.Point(6, 84);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(76, 13);
            this.labelDateTime.TabIndex = 6;
            this.labelDateTime.Text = "Date and ti&me:";
            // 
            // textBoxLeague
            // 
            this.textBoxLeague.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLeague.Location = new System.Drawing.Point(92, 55);
            this.textBoxLeague.Name = "textBoxLeague";
            this.textBoxLeague.Size = new System.Drawing.Size(180, 20);
            this.textBoxLeague.TabIndex = 5;
            this.textBoxLeague.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxLeague.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelLeague
            // 
            this.labelLeague.AutoSize = true;
            this.labelLeague.Location = new System.Drawing.Point(6, 58);
            this.labelLeague.Name = "labelLeague";
            this.labelLeague.Size = new System.Drawing.Size(46, 13);
            this.labelLeague.TabIndex = 4;
            this.labelLeague.Text = "&League:";
            // 
            // textBoxMatch
            // 
            this.textBoxMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMatch.Location = new System.Drawing.Point(92, 29);
            this.textBoxMatch.Name = "textBoxMatch";
            this.textBoxMatch.Size = new System.Drawing.Size(180, 20);
            this.textBoxMatch.TabIndex = 3;
            this.textBoxMatch.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textBoxMatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxMatch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelGame
            // 
            this.labelGame.AutoSize = true;
            this.labelGame.Location = new System.Drawing.Point(6, 32);
            this.labelGame.Name = "labelGame";
            this.labelGame.Size = new System.Drawing.Size(40, 13);
            this.labelGame.TabIndex = 2;
            this.labelGame.Text = "Mat&ch:";
            // 
            // textBoxSport
            // 
            this.textBoxSport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSport.Location = new System.Drawing.Point(92, 3);
            this.textBoxSport.Name = "textBoxSport";
            this.textBoxSport.Size = new System.Drawing.Size(180, 20);
            this.textBoxSport.TabIndex = 1;
            this.textBoxSport.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.textBoxSport.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseDown);
            // 
            // labelSport
            // 
            this.labelSport.AutoSize = true;
            this.labelSport.Location = new System.Drawing.Point(6, 6);
            this.labelSport.Name = "labelSport";
            this.labelSport.Size = new System.Drawing.Size(35, 13);
            this.labelSport.TabIndex = 0;
            this.labelSport.Text = "S&port:";
            // 
            // textBoxClicksTimer
            // 
            this.textBoxClicksTimer.Tick += new System.EventHandler(this.OnTextBoxClicksTimerTick);
            // 
            // MatchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxOpportunity);
            this.Controls.Add(this.labelOpportunity);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.labelDateTime);
            this.Controls.Add(this.textBoxLeague);
            this.Controls.Add(this.labelLeague);
            this.Controls.Add(this.textBoxMatch);
            this.Controls.Add(this.labelGame);
            this.Controls.Add(this.textBoxSport);
            this.Controls.Add(this.labelSport);
            this.Name = "MatchControl";
            this.Size = new System.Drawing.Size(275, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOpportunity;
        private System.Windows.Forms.Label labelOpportunity;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.TextBox textBoxLeague;
        private System.Windows.Forms.Label labelLeague;
        private System.Windows.Forms.TextBox textBoxMatch;
        private System.Windows.Forms.Label labelGame;
        private System.Windows.Forms.TextBox textBoxSport;
        private System.Windows.Forms.Label labelSport;
        private System.Windows.Forms.Timer textBoxClicksTimer;
    }
}
