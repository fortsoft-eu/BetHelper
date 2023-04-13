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
 * Version 1.1.2.0
 */

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BetHelper {
    public partial class PopUpBrowserForm : Form {
        private int heartBeatTimerRun, heartBeatTimerStart;
        private PopUpEventArgs popUpEventArgs;
        private PopUpFrameHandler popUpFrameHandler;
        private SemaphoreSlim heartBeatSemaphore;
        private Settings settings;
        private StatusStripHandler statusStripHandler;
        private string browserConsoleMessage;
        private System.Timers.Timer heartBeatTimer, closeTimer;
        private Thread popUpThread;
        private WebInfo webInfo;

        public event EventHandler<UrlEventArgs> UrlChanged;

        public PopUpBrowserForm(WebInfo webInfo, Settings settings, PopUpEventArgs popUpEventArgs) {
            this.webInfo = webInfo;
            this.settings = settings;
            MainForm mainForm = (MainForm)webInfo.Parent.Form;

            browserConsoleMessage = string.Empty;

            Icon = Properties.Resources.Executable;

            heartBeatSemaphore = new SemaphoreSlim(0, 1);
            heartBeatTimer = new System.Timers.Timer();
            heartBeatTimer.Interval = Constants.HeartBeatInterval;
            heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                if (heartBeatTimerRun++ < Constants.HeartBeatCycles) {
                    heartBeatSemaphore.Wait();
                    if (Browser.CanExecuteJavascriptInMainFrame) {
                        webInfo.HeartBeat(Browser);
                    }
                    heartBeatSemaphore.Release();
                } else {
                    heartBeatTimerRun = Constants.HeartBeatCycles;
                }
            });
            closeTimer = new System.Timers.Timer();
            closeTimer.Interval = 1500;
            closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(SafeClose);
            mainForm.FormClosed += new FormClosedEventHandler(SafeClose);

            InitializeComponent();
            SuspendLayout();

            statusStripHandler = new StatusStripHandler(
                statusStrip,
                StatusStripHandler.DisplayMode.Standard,
                settings,
                mainForm.ExtBrowsHandler);

            mainForm.BrowserCacheManager.CacheSizeComputed += new EventHandler<BrowserCacheEventArgs>((sender, e) =>
                statusStripHandler.SetDataSize(e.BrowserCacheSize));

            popUpFrameHandler = new PopUpFrameHandler(
                new ChromiumWebBrowser(popUpEventArgs.TargetUrl),
                new FrameHandler(),
                new LoadHandler(),
                settings);

            Browser = popUpFrameHandler.Browser;
            popUpFrameHandler.Close += new EventHandler(OnPopUpFrameHandlerClose);
            FindHandler findHandler = new FindHandler();
            findHandler.Find += new EventHandler<FindEventArgs>((sender, e) => {

            });
            Browser.FindHandler = findHandler;
            RequestHandler requestHandler = new RequestHandler() { WebInfo = webInfo };
            requestHandler.Canceled += new EventHandler((sender, e) => mainForm.StatusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.PersistentA, Properties.Resources.MessageActionCanceled));

            Browser.RequestHandler = requestHandler;

            if (webInfo.HandlePopUps
                    || !webInfo.PopUpLeft.Equals(0)
                    || !webInfo.PopUpTop.Equals(0)
                    || !webInfo.PopUpWidth.Equals(0)
                    || !webInfo.PopUpHeight.Equals(0)) {

                LifeSpanHandlerPopUp lifeSpanHandler = new LifeSpanHandlerPopUp();
                lifeSpanHandler.BrowserPopUp += new EventHandler<PopUpEventArgs>((sender, popUpArgs) => {
                    if (!webInfo.CanNavigate(popUpArgs.TargetUrl)) {
                        statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA,
                            Properties.Resources.MessageActionCanceled);
                        mainForm.StatusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA,
                            Properties.Resources.MessageActionCanceled);
                        return;
                    }
                    bool navigateToMainWindow = false;
                    switch (webInfo.BackNavigation) {
                        case WebInfo.BackNavigationType.Fragment:
                            try {
                                Uri uri = new Uri(popUpArgs.TargetUrl);
                                if (string.IsNullOrEmpty(uri.Fragment)) {
                                    navigateToMainWindow = true;
                                }
                            } catch (Exception exception) {
                                Debug.WriteLine(exception);
                                ErrorLog.WriteLine(exception);
                            }
                            break;
                    }
                    if (navigateToMainWindow) {
                        UrlChanged?.Invoke(sender, new UrlEventArgs(popUpArgs.TargetUrl));
                        SafeMinimize(this, EventArgs.Empty);
                    } else {
                        this.popUpEventArgs = popUpArgs;
                        popUpThread = new Thread(new ThreadStart(PopUpBrowser));
                        popUpThread.Start();
                    }
                });
                Browser.LifeSpanHandler = lifeSpanHandler;
            }
            Browser.AddressChanged += new EventHandler<AddressChangedEventArgs>(OnAddressChanged);
            Browser.ConsoleMessage += new EventHandler<ConsoleMessageEventArgs>(OnBrowserConsoleMessage);
            Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((sender, e) => {
                HeartBeatReset();
                statusStripHandler.SetFinished();
            });
            Browser.FrameLoadStart += new EventHandler<FrameLoadStartEventArgs>((sender, e) => {
                HeartBeatReset();
                statusStripHandler.SetMaximum(0);
            });
            Browser.IsBrowserInitializedChanged += new EventHandler(OnIsBrowserInitializedChanged);
            Browser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(OnLoadingStateChanged);
            Browser.Resize += new EventHandler((sender, e) => HeartBeatReset());
            Browser.StatusMessage += new EventHandler<StatusMessageEventArgs>(OnBrowserStatusMessage);
            Browser.TitleChanged += new EventHandler<TitleChangedEventArgs>(OnBrowserTitleChanged);
            panel.Controls.Add(Browser);

            Load += new EventHandler((sender, e) => {
                Size acceptedMax = new Size(SystemInformation.VirtualScreen.Width - 200, SystemInformation.VirtualScreen.Height - 80);
                int nLeft = 0;
                if (popUpEventArgs.Location.X > 0) {
                    if (popUpEventArgs.Location.X > acceptedMax.Width) {
                        nLeft = acceptedMax.Width;
                    } else {
                        nLeft = popUpEventArgs.Location.X;
                    }
                } else if (popUpEventArgs.Location.X.Equals(0)) {
                    nLeft = Left;
                } else {
                    nLeft = 0;
                }
                int nTop = 0;
                if (popUpEventArgs.Location.Y > 0) {
                    if (popUpEventArgs.Location.Y > acceptedMax.Height) {
                        nTop = acceptedMax.Height;
                    } else {
                        nTop = popUpEventArgs.Location.Y;
                    }
                } else if (popUpEventArgs.Location.Y.Equals(0)) {
                    nTop = Top;
                } else {
                    nTop = 0;
                }
                Location = new Point(nLeft, nTop);
                int nWidth = 0;
                if (popUpEventArgs.Size.Width > 0) {
                    if (popUpEventArgs.Size.Width > acceptedMax.Width) {
                        nWidth = acceptedMax.Width;
                    } else {
                        nWidth = popUpEventArgs.Size.Width;
                    }
                } else if (popUpEventArgs.Size.Width.Equals(0)) {
                    nWidth = Width;
                } else {
                    nWidth = acceptedMax.Width;
                }
                int nHeight = 0;
                if (popUpEventArgs.Size.Height > 0) {
                    if (popUpEventArgs.Size.Height > acceptedMax.Height) {
                        nHeight = acceptedMax.Height;
                    } else {
                        nHeight = popUpEventArgs.Size.Height;
                    }
                } else if (popUpEventArgs.Size.Height.Equals(0)) {
                    nHeight = Height;
                } else {
                    nHeight = acceptedMax.Height;
                }
                Size = new Size(nWidth, nHeight);
            });
            ResumeLayout(false);
            PerformLayout();
            heartBeatSemaphore.Release(1);
        }

        public ChromiumWebBrowser Browser { get; private set; }

        private void HeartBeatReset() {
            heartBeatTimerRun = 0;
            if (heartBeatTimerStart++ < 1) {
                heartBeatTimer.Start();
            } else {
                heartBeatTimerStart = 1;
            }
        }

        private void PopUpBrowser() {
            popUpEventArgs.Location = new Point(webInfo.PopUpLeft, webInfo.PopUpTop);
            popUpEventArgs.Size = new Size(webInfo.PopUpWidth, webInfo.PopUpHeight);
            PopUpBrowserForm popUpBrowserForm = new PopUpBrowserForm(webInfo, settings, popUpEventArgs);
            popUpBrowserForm.UrlChanged += new EventHandler<UrlEventArgs>(OnUrlChanged);
            popUpBrowserForm.ShowDialog();
        }

        private void OnUrlChanged(object sender, UrlEventArgs e) {
            if (webInfo.Browser.InvokeRequired) {
                webInfo.Browser.Invoke(new EventHandler<UrlEventArgs>(OnUrlChanged), sender, e);
            } else {
                webInfo.Browser.Load(e.Url);
            }
        }

        private void OnPopUpFrameHandlerClose(object sender, EventArgs e) {
            try {
                if (InvokeRequired) {
                    Invoke(new EventHandler(OnPopUpFrameHandlerClose), sender, e);
                } else {
                    UrlChanged?.Invoke(sender, new UrlEventArgs(((PopUpFrameHandler)sender).NewUrl));
                    Label label = new Label();
                    label.Dock = DockStyle.Fill;
                    panel.Controls.Add(label);
                    label.BringToFront();
                    SendToBack();
                    closeTimer.Start();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void SafeMinimize(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(SafeMinimize), sender, e);
            } else {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void SafeClose(object sender, EventArgs e) {
            if (Browser.IsDisposed) {
                return;
            }
            if (Browser.InvokeRequired) {
                Browser.Invoke(new EventHandler(SafeClose), sender, e);
            } else {
                closeTimer.Stop();
                closeTimer.Dispose();
                heartBeatTimer.Stop();
                heartBeatTimer.Dispose();
                heartBeatSemaphore.Dispose();
                statusStripHandler.Dispose();
                if (!Browser.IsDisposed) {
                    Browser.GetBrowser().CloseBrowser(true);
                }
            }
        }

        private void SetText(string str) => Text = string.IsNullOrEmpty(str) ? Program.GetTitle() : str;

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e) {
            try {
                if (InvokeRequired) {
                    Invoke(new EventHandler(OnIsBrowserInitializedChanged), sender, e);
                } else {
                    Browser.Focus();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {
            HeartBeatReset();
            if (e.IsLoading) {
                statusStripHandler.SetMaximum(0);
            } else {
                statusStripHandler.SetFinished();
            }
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e) {
            if (settings.ShowConsoleMessages) {
                statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentB,
                    string.Format(Constants.BrowserConsoleMessageFormat1, e.Line, e.Source, e.Message));
            }
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs e) {
            statusStripHandler.SetMessage(StatusStripHandler.StatusMessageType.PersistentA, e.Value);
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs e) {
            try {
                if (InvokeRequired) {
                    Invoke(new EventHandler<TitleChangedEventArgs>(OnBrowserTitleChanged), sender, e);
                } else {
                    SetText(e.Title);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnAddressChanged(object sender, AddressChangedEventArgs e) {
            if (!webInfo.EqualsSecondLevelDomain(e.Address)) {
                SafeClose(sender, e);
            }
            statusStripHandler.SetUrl(e.Address);
        }
    }
}
