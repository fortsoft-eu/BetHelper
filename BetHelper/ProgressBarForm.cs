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

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public partial class ProgressBarForm : Form {
        private bool disableClose, setFinished;
        private Timer timer;

        protected delegate void SetMessageCallback(string message);
        protected delegate void SetValueCallback(int value);

        public ProgressBarForm() {
            Text = Program.GetTitle();
            InitializeComponent();
            progressBar.Minimum = 0;
            progressBar.Style = ProgressBarStyle.Continuous;
            timer = new Timer();
            timer.Interval = Constants.ProgressBarFormFinishDelay;
            timer.Tick += new EventHandler((sender, e) => {
                timer.Stop();
                Close();
            });
        }

        public bool DisableClose {
            get {
                return disableClose;
            }
            set {
                if (!setFinished) {
                    if (value) {
                        button.Enabled = false;
                        DisableCloseButton();
                    } else {
                        button.Enabled = true;
                        EnableCloseButton();
                    }
                    disableClose = value;
                }
            }
        }

        public Button Button => button;

        public Label LabelProgress => labelProgress;

        public Rectangle ParentRectangle { get; set; }

        public ProgressBar ProgressBar => progressBar;

        public bool ProgressBarMarquee {
            get {
                return progressBar.Style.Equals(ProgressBarStyle.Marquee);
            }
            set {
                progressBar.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            }
        }

        private void DisableCloseButton() {
            NativeMethods.EnableMenuItem(NativeMethods.GetSystemMenu(Handle, false), Constants.SC_CLOSE, 1);
        }

        private void EnableCloseButton() {
            NativeMethods.EnableMenuItem(NativeMethods.GetSystemMenu(Handle, false), Constants.SC_CLOSE, 0);
        }

        private void OnCancelButtonClick(object sender, EventArgs e) => Close();

        private void OnFormClosed(object sender, FormClosedEventArgs e) {
            timer.Stop();
            timer.Dispose();
        }

        private void OnFormLoad(object sender, EventArgs e) {
            if (!ParentRectangle.IsEmpty) {
                Location = new Point(
                    ParentRectangle.X + (ParentRectangle.Width - Width) / 2,
                    ParentRectangle.Y + (ParentRectangle.Height - Height) / 2);
            }
        }

        public virtual void SetFinished(string message) {
            if (InvokeRequired) {
                Invoke(new SetMessageCallback(SetFinished), message);
            } else {
                setFinished = true;
                button.Enabled = false;
                DisableCloseButton();
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = 1;
                progressBar.Value = 1;
                progressBar.Invalidate();
                labelMessage.Text = message;
                labelProgress.Text = new StringBuilder()
                    .Append(100)
                    .Append(Constants.Space)
                    .Append(Constants.Percent)
                    .ToString();
                labelProgress.Invalidate();
                timer.Start();
            }
        }

        public virtual void SetMaximum(int maximum) {
            if (InvokeRequired) {
                Invoke(new SetValueCallback(SetMaximum), maximum);
            } else {
                progressBar.Maximum = maximum;
            }
        }

        public virtual void SetMessage(string message) {
            if (InvokeRequired) {
                Invoke(new SetMessageCallback(SetMessage), message);
            } else {
                labelMessage.Text = message;
            }
        }

        public virtual void SetValue(int value) {
            if (InvokeRequired) {
                Invoke(new SetValueCallback(SetValue), value);
            } else {
                progressBar.Style = ProgressBarStyle.Continuous;
                if (value > progressBar.Maximum) {
                    value = progressBar.Maximum;
                }
                progressBar.Value = value;
                progressBar.Invalidate();
                labelProgress.Text = new StringBuilder()
                    .Append(value * 100 / progressBar.Maximum)
                    .Append(Constants.Space)
                    .Append(Constants.Percent)
                    .ToString();
                labelProgress.Invalidate();
            }
        }
    }
}
