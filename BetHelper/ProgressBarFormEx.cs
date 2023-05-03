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
 * Version 1.1.7.0
 */

using System;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class ProgressBarFormEx : ProgressBarForm {
        private int maximum, temp;
        private Timer timer;

        public event EventHandler F7Pressed;

        public ProgressBarFormEx() {
            timer = new Timer();
            timer.Interval = Constants.ProgressBarFormInterval;
            timer.Tick += new EventHandler(OnTick);
            FormClosed += new FormClosedEventHandler((sender, e) => {
                timer.Stop();
                timer.Dispose();
            });
            Button.KeyDown += new KeyEventHandler(OnKeyDown);
            KeyDown += new KeyEventHandler(OnKeyDown);
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode.Equals(Keys.F7)) {
                F7Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnTick(object sender, EventArgs e) {
            if (ProgressBar.Value < ProgressBar.Maximum) {
                temp++;
                ProgressBar.Value++;
                ProgressBar.Invalidate();
            }
            LabelProgress.Text = new StringBuilder()
                .Append(ProgressBar.Value * 100 / ProgressBar.Maximum)
                .Append(Constants.Space)
                .Append(Constants.Percent)
                .ToString();
            LabelProgress.Invalidate();
            if (temp > Constants.ProgressBarFormInterval * 2) {
                timer.Interval = temp / 2;
            }
            if ((float)ProgressBar.Value / ProgressBar.Maximum < 0.95f) {
                return;
            }
            timer.Stop();
        }

        public override void SetMaximum(int maximum) {
            if (InvokeRequired) {
                Invoke(new SetValueCallback(SetMaximum), maximum);
            } else {
                ProgressBar.Style = ProgressBarStyle.Continuous;
                ProgressBar.Maximum = ProgressBar.Width;
                this.maximum = maximum;
                timer.Start();
            }
        }

        public override void SetValue(int value) {
            if (InvokeRequired) {
                Invoke(new SetValueCallback(SetValue), value);
            } else {
                ProgressBar.Style = ProgressBarStyle.Continuous;
                if (value > maximum) {
                    value = maximum;
                }
                int pValue = value * ProgressBar.Maximum / maximum;
                if (pValue > ProgressBar.Value) {
                    temp = Constants.ProgressBarFormInterval;
                    timer.Interval = Constants.ProgressBarFormInterval;
                    ProgressBar.Value = pValue;
                    ProgressBar.Invalidate();
                    LabelProgress.Text = new StringBuilder()
                        .Append(pValue * 100 / ProgressBar.Maximum)
                        .Append(Constants.Space)
                        .Append(Constants.Percent)
                        .ToString();
                    LabelProgress.Invalidate();
                }
            }
        }
    }
}
