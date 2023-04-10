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
 * Version 1.1.0.0
 */

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public sealed class CalculatorHandler : IDisposable {
        private List<ChromiumWebBrowser> browsers;
        private TabControl tabControl;
        private Uri uri;

        private delegate void AddNewTabCallback();

        public event EventHandler Enter;
        public event EventHandler<TabAddedEventArgs> TabAdded;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        public CalculatorHandler(TabControl tabControl) {
            this.tabControl = tabControl;
            browsers = new List<ChromiumWebBrowser>(2);
            uri = new Uri(Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath),
                Constants.CalculatorDirectoryName, Constants.CalculatorFileName03));
            AddNewTab();
        }

        public string BrowserTitle { get; private set; }
        public ChromiumWebBrowser LastEntered { get; private set; }

        public void AddNewTab() {
            if (browsers.Count / 2 == Constants.CalculatorTabsMaxCounts) {
                return;
            }
            if (tabControl.InvokeRequired) {
                tabControl.Invoke(new AddNewTabCallback(AddNewTab));
            } else {
                SplitContainer splitContainer = new SplitContainer();
                splitContainer.Dock = DockStyle.Fill;
                splitContainer.Orientation = Orientation.Horizontal;
                splitContainer.BackColor = SystemColors.Control;
                for (int i = 0; i < 2; i++) {
                    ChromiumWebBrowser browser = new ChromiumWebBrowser(uri.AbsoluteUri);
                    browser.Enter += new EventHandler((sender, e) => {
                        LastEntered = (ChromiumWebBrowser)sender;
                        Enter?.Invoke(sender, e);
                    });
                    browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(OnCalculatorFrameLoadEnd);
                    browser.TitleChanged += new EventHandler<TitleChangedEventArgs>((sender, e) => {
                        BrowserTitle = e.Title;
                        TitleChanged?.Invoke(this, e);
                    });
                    browsers.Add(browser);
                    if (i % 2 == 0) {
                        splitContainer.Panel1.Controls.Add(browser);
                    } else {
                        splitContainer.Panel2.Controls.Add(browser);
                    }
                }
                TabPage tabPage = new TabPage {
                    Text = Properties.Resources.CaptionCalculator,
                    UseVisualStyleBackColor = false
                };
                splitContainer.SizeChanged += new EventHandler((sender, e) => {
                    if (splitContainer.Width - splitContainer.Panel2MinSize - splitContainer.Panel1MinSize < 0) {
                        return;
                    }
                    int splitterDistance = splitContainer.Height / 2 - 2;
                    splitContainer.SplitterDistance = splitterDistance < splitContainer.Panel1MinSize
                    ? splitContainer.Panel1MinSize
                    : splitterDistance;
                });
                tabPage.Controls.Add(splitContainer);
                tabControl.TabPages.Add(tabPage);
                if (browsers.Count > 2) {
                    tabPage.Text += new StringBuilder()
                        .Append(Constants.Space)
                        .Append(Constants.OpeningBracket)
                        .Append(browsers.Count / 2)
                        .Append(Constants.ClosingBracket)
                        .ToString();
                    tabControl.SelectedIndex = tabControl.TabCount - 1;
                }
                TabAdded?.Invoke(this,
                    new TabAddedEventArgs(tabPage, browsers.Count / 2, browsers.Count / 2 == Constants.CalculatorTabsMaxCounts));
            }
        }

        public void Dispose() {
            foreach (ChromiumWebBrowser browser in browsers) {
                browser.Dispose();
            }
        }

        private void OnCalculatorFrameLoadEnd(object sender, EventArgs e) {
            try {
                if (((ChromiumWebBrowser)sender).InvokeRequired) {
                    ((ChromiumWebBrowser)sender).Invoke(new EventHandler(OnCalculatorFrameLoadEnd), sender, e);
                } else {
                    ((ChromiumWebBrowser)sender).SetZoomLevel(Constants.CalculatorTabsDefaultZoomLevel);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}
