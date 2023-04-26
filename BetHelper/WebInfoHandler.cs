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
 * Version 1.1.5.0
 */

using CefSharp;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace BetHelper {
    public sealed class WebInfoHandler : IDisposable {
        private bool stopped;
        private int index, n, ordinal, pingIndex;
        private string assemblyName;
        private System.Timers.Timer getTimer, pingTimer, suspendTimer;

        private delegate void WebInfoHandlerCallback();

        public event EventHandler BalanceGot, BrowserInitializedChanged, Enter, Suspended, TipsGot, ZoomLevelChanged;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<CanceledEventArgs> Canceled;
        public event EventHandler<ConsoleMessageEventArgs> BrowserConsoleMessage;
        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler<FindEventArgs> Find;
        public event EventHandler<FinishedEventArgs> Finished;
        public event EventHandler<FocusEventArgs> CurrentSet, Initialized;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ProgressEventArgs> Progress;
        public event EventHandler<StartedEventArgs> Started;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        public event HelpEventHandler Help;

        public WebInfoHandler(Form form, decimal[] balances) {
            Form = form;
            Form.Load += new EventHandler((sender, e) => {
                SetTimer();
                SetPingTimer();
            });
            index = -1;
            pingIndex = -1;
            ParseConfig(((MainForm)form).Settings.Config);
            SetBalances(balances);
            SubscribeEvents();
            getTimer = new System.Timers.Timer();
            getTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                getTimer.Interval = Constants.RightPaneInterval;
                GetBalance();
                GetTips();
            });
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = Constants.PingNextTabInterval;
            pingTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                int i = NextPing();
                if (i >= 0) {
                    WebInfos[i].Browser.GetBrowser().Reload();
                }
            });
            suspendTimer = new System.Timers.Timer();
            suspendTimer.Interval = Constants.WebInfoHeartBeatInterval * 2;
            suspendTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                if (n++ < 1) {
                    CloseBrowsers();
                } else if (stopped) {
                    suspendTimer.Stop();
                    IsSuspended = true;
                    Suspended?.Invoke(this, EventArgs.Empty);
                } else {
                    foreach (WebInfo webInfo in WebInfos) {
                        if (webInfo != null && webInfo.Browser != null && webInfo.Browser.IsLoading) {
                            return;
                        }
                    }
                    stopped = true;
                }
            });
        }

        public bool IsSuspended { get; private set; }

        public decimal Balance { get; private set; }

        public Form Form { get; private set; }

        public List<WebInfo> WebInfos { get; private set; }

        public Tip[] Tips { get; private set; }

        public WebInfo Current { get; private set; }

        private void CloseBrowsers() {
            if (Form.InvokeRequired) {
                Form.Invoke(new WebInfoHandlerCallback(CloseBrowsers));
            } else {
                foreach (WebInfo webInfo in WebInfos) {
                    if (webInfo != null && webInfo.Browser != null) {
                        try {
                            webInfo.Browser.Stop();
                            webInfo.Browser.GetBrowser().CloseBrowser(true);
                        } catch (Exception exception) {
                            Debug.WriteLine(exception);
                            ErrorLog.WriteLine(exception);
                        }
                    }
                }
            }
        }

        private void ParseConfig(string config) {
            WebInfo webInfo = null;
            WebInfos = new List<WebInfo>();
            StringReader stringReader = new StringReader(config);
            for (string line; (line = stringReader.ReadLine()) != null;) {
                string[] configLine = line.Split(new char[] { Constants.EqualSign }, 2);
                if (configLine.Length.Equals(2)) {
                    if (webInfo == null) {
                        webInfo = GetWebInfo();
                    }
                    string value = configLine[1].Trim();
                    int val;
                    switch (configLine[0].Trim()) {
                        case Constants.ConfigTitle:
                            if (!string.IsNullOrEmpty(webInfo.Title)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Title = value;
                            break;
                        case Constants.ConfigUrl:
                            if (!string.IsNullOrEmpty(webInfo.Url)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Url = value;
                            break;
                        case Constants.ConfigUrlLive:
                            if (!string.IsNullOrEmpty(webInfo.UrlLive)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlLive = value;
                            break;
                        case Constants.ConfigUrlToLoad:
                            if (!string.IsNullOrEmpty(webInfo.UrlToLoad)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlNext = value;
                            break;
                        case Constants.ConfigUrlTips:
                            if (!string.IsNullOrEmpty(webInfo.UrlTips)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlTips = value;
                            break;
                        case Constants.ConfigUserName:
                            if (!string.IsNullOrEmpty(webInfo.UserName)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UserName = value;
                            break;
                        case Constants.ConfigPassword:
                            if (!string.IsNullOrEmpty(webInfo.Password)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Password = value;
                            break;
                        case Constants.ConfigScript:
                            if (!string.IsNullOrEmpty(webInfo.Script)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Script = value;
                            break;
                        case Constants.ConfigPattern:
                            if (!string.IsNullOrEmpty(webInfo.Pattern)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Pattern = value;
                            break;
                        case Constants.ConfigIetfLanguageTag:
                            if (!string.IsNullOrEmpty(webInfo.IetfLanguageTag)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.IetfLanguageTag = value;
                            break;
                        case Constants.ConfigFields:
                            if (!string.IsNullOrEmpty(webInfo.Fields)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Fields = value;
                            break;
                        case Constants.ConfigDisplayName:
                            if (!string.IsNullOrEmpty(webInfo.DisplayName)) {
                                WebInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.DisplayName = value;
                            break;
                        case Constants.ConfigPopUpWidth:
                            if (int.TryParse(value, out val)) {
                                webInfo.PopUpWidth = val;
                            }
                            break;
                        case Constants.ConfigPopUpHeight:
                            if (int.TryParse(value, out val)) {
                                webInfo.PopUpHeight = val;
                            }
                            break;
                        case Constants.ConfigPopUpLeft:
                            if (int.TryParse(value, out val)) {
                                webInfo.PopUpLeft = val;
                            }
                            break;
                        case Constants.ConfigPopUpTop:
                            if (int.TryParse(value, out val)) {
                                webInfo.PopUpTop = val;
                            }
                            break;
                        case Constants.ConfigService:
                            webInfo.IsService = string
                                .Compare(value, Constants.ConfigYes, StringComparison.OrdinalIgnoreCase)
                                .Equals(0);
                            break;
                        case Constants.ConfigMute:
                            webInfo.AudioMutedByDefault = string
                                .Compare(value, Constants.ConfigYes, StringComparison.OrdinalIgnoreCase)
                                .Equals(0);
                            break;
                        case Constants.ConfigHandlePopUps:
                            webInfo.HandlePopUps = string
                                .Compare(value, Constants.ConfigYes, StringComparison.OrdinalIgnoreCase)
                                .Equals(0);
                            break;
                        case Constants.ConfigTabNavigation:
                            webInfo.TabNavigation = string
                                .Compare(value, Constants.ConfigYes, StringComparison.OrdinalIgnoreCase)
                                .Equals(0);
                            break;
                        case Constants.ConfigBackNavigation:
                            webInfo.BackNavigation = (WebInfo.BackNavigationType)Enum
                                .Parse(typeof(WebInfo.BackNavigationType), value, true);
                            break;
                        case Constants.ConfigAllowedHosts:
                            webInfo.AllowedHosts = webInfo.AllowedHosts ?? new List<string>();
                            foreach (string host in value.Split(Constants.Comma)) {
                                webInfo.AllowedHosts.Add(host.Trim());
                            }
                            break;
                        case Constants.ConfigChat:
                            webInfo.ChatHosts = webInfo.ChatHosts ?? new List<string>();
                            foreach (string host in value.Split(Constants.Comma)) {
                                webInfo.ChatHosts.Add(host.Trim());
                            }
                            break;
                    }
                }
            }
            if (webInfo != null && !string.IsNullOrEmpty(webInfo.Title)) {
                WebInfos.Add(webInfo);
            }
        }

        private void SetBalances(decimal[] balances) {
            if (balances != null) {
                for (int i = 0; i < WebInfos.Count; i++) {
                    WebInfos[i].SetBalance(i < balances.Length ? balances[i] : decimal.MinValue);
                }
            }
        }

        private void SubscribeEvents() {
            foreach (WebInfo webInfo in WebInfos) {
                webInfo.Parent = this;
                webInfo.AddressChanged += new EventHandler<AddressChangedEventArgs>(OnAddressChanged);
                webInfo.BrowserConsoleMessage += new EventHandler<ConsoleMessageEventArgs>(OnBrowserConsoleMessage);
                webInfo.BrowserInitializedChanged += new EventHandler(OnBrowserInitializedChanged);
                webInfo.BrowserStatusMessage += new EventHandler<StatusMessageEventArgs>(OnStatusMessage);
                webInfo.Canceled += new EventHandler<CanceledEventArgs>(OnCancel);
                webInfo.Enter += new EventHandler(OnEnter);
                webInfo.Error += new EventHandler<ErrorEventArgs>(OnError);
                webInfo.Find += new EventHandler<FindEventArgs>(OnFind);
                webInfo.Finished += new EventHandler<FinishedEventArgs>(OnFinished);
                webInfo.Focus += new EventHandler(OnFocus);
                webInfo.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEnd);
                webInfo.FrameLoadStart += new EventHandler<FrameLoadStartEventArgs>(OnFrameLoadStart);
                webInfo.Help += new HelpEventHandler(OnHelp);
                webInfo.Initialized += new EventHandler<FocusEventArgs>(OnInitialized);
                webInfo.LoadError += new EventHandler<LoadErrorEventArgs>(OnLoadError);
                webInfo.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(OnLoadingStateChanged);
                webInfo.Progress += new EventHandler<ProgressEventArgs>(OnProgress);
                webInfo.Relay += new EventHandler<UrlEventArgs>(OnRelay);
                webInfo.Started += new EventHandler<StartedEventArgs>(OnStarted);
                webInfo.TitleChanged += new EventHandler<TitleChangedEventArgs>(OnTitleChanged);
                webInfo.ZoomLevelChanged += new EventHandler(OnZoomLevelChanged);
            }
        }

        public void Dispose() {
            foreach (WebInfo webInfo in WebInfos) {
                webInfo.Dispose();
            }
            getTimer.Stop();
            getTimer.Dispose();
            pingTimer.Stop();
            pingTimer.Dispose();
            suspendTimer.Dispose();
        }

        private void GetBalance() {
            decimal balance = decimal.Zero;
            foreach (WebInfo webInfo in WebInfos) {
                decimal bal = webInfo.GetBalance();
                if (bal != decimal.MinValue) {
                    balance += bal;
                }
            }
            Balance = balance;
            BalanceGot?.Invoke(this, EventArgs.Empty);
        }

        public decimal[] GetBalances() {
            decimal[] balances = new decimal[WebInfos.Count];
            for (int i = 0; i < WebInfos.Count; i++) {
                balances[i] = WebInfos[i].GetBalance();
            }
            return balances;
        }

        public string GetCurrentAddress() {
            if (Current != null) {
                if (!string.IsNullOrWhiteSpace(Current.BrowserAddress) && Current.BrowserAddress.StartsWith(Constants.SchemeHttps)) {
                    return Current.BrowserAddress;
                } else if (Current.Browser != null
                        && !string.IsNullOrWhiteSpace(Current.Browser.Address)
                        && Current.Browser.Address.StartsWith(Constants.SchemeHttps)) {

                    return Current.Browser.Address;
                }
            }
            return null;
        }

        private void GetTips() {
            List<Tip> list = new List<Tip>();
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.IsService) {
                    Tip[] tips = webInfo.GetTips();
                    if (tips != null) {
                        list.AddRange(tips);
                    }
                }
            }
            Tips = list.ToArray();
            TipsGot?.Invoke(this, EventArgs.Empty);
        }

        public int LoadUrl(string url) {
            for (int i = 0; i < WebInfos.Count; i++) {
                if (WebInfos[i].EqualsSecondLevelDomain(url)) {
                    SetCurrent(i);
                    WebInfos[i].SetUrlToLoad(url);
                    return i;
                }
            }
            return -1;
        }

        public void SetCurrent(int index) {
            if (index.Equals(this.index)) {
                return;
            }
            this.index = index;
            if (Current != null) {
                Current.Suspend();
            }
            Current = WebInfos[index];
            if (Current.Browser == null) {
                Current.Initialize();
            } else {
                Current.Resume();
            }
            CurrentSet?.Invoke(this, new FocusEventArgs(Current, index));
            return;
        }

        public void StopFinding() {
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.Browser != null) {
                    webInfo.StopFinding(false);
                }
            }
        }

        public void SendKey(Keys key) {
            if (Current != null) {
                Current.SendKey(key);
            }
        }

        public void SetPingTimer() {
            if (((MainForm)Form).Settings.TryToKeepUserLoggedIn) {
                pingTimer.Start();
            } else {
                pingTimer.Stop();
            }
        }

        private void SetTimer() {
            getTimer.Interval = Constants.InitialRightPaneInterval;
            getTimer.Start();
        }

        public void Suspend() {
            getTimer.Stop();
            pingTimer.Stop();
            suspendTimer.Start();
        }

        private string GetAssemblyName() {
            if (string.IsNullOrEmpty(assemblyName)) {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length > 0) {
                    AssemblyProductAttribute assemblyCopyrightAttribute = (AssemblyProductAttribute)attributes[0];
                    assemblyName = assemblyCopyrightAttribute.Product;
                }
            }
            return assemblyName;
        }

        private WebInfo GetWebInfo() {
            switch (ordinal++) {
                case 0:
                    return new WebInfo01() { Ordinal = ordinal };
                case 1:
                    return new WebInfo02() { Ordinal = ordinal };
                case 2:
                    return new WebInfo03() { Ordinal = ordinal };
                case 3:
                    return new WebInfo04() { Ordinal = ordinal };
                case 4:
                    return new WebInfo05() { Ordinal = ordinal };
                case 5:
                    return new WebInfo06() { Ordinal = ordinal };
                case 6:
                    return new WebInfo07() { Ordinal = ordinal };
                case 7:
                    return new WebInfo08() { Ordinal = ordinal };
                case 8:
                    return new WebInfo09() { Ordinal = ordinal };
                case 9:
                    return new WebInfo10() { Ordinal = ordinal };
                case 10:
                    return new WebInfo11() { Ordinal = ordinal };
                case 11:
                    return new WebInfo12() { Ordinal = ordinal };
                case 12:
                    return new WebInfo13() { Ordinal = ordinal };
                case 13:
                    return new WebInfo14() { Ordinal = ordinal };
                case 14:
                    return new WebInfo15() { Ordinal = ordinal };
                case 15:
                    return new WebInfo16() { Ordinal = ordinal };
                case 16:
                    return new WebInfo17() { Ordinal = ordinal };
                case 17:
                    return new WebInfo18() { Ordinal = ordinal };
                case 18:
                    return new WebInfo19() { Ordinal = ordinal };
                case 19:
                    return new WebInfo20() { Ordinal = ordinal };
                default:
                    throw new NotImplementedException();
            }
        }

        private int NextPing() {
            int i = 0;
            do {
                if (++pingIndex == WebInfos.Count) {
                    pingIndex = 0;
                }
            } while (++i < WebInfos.Count && !WebInfos[pingIndex].IsBookmaker && !WebInfos[pingIndex].IsService);
            return WebInfos[pingIndex].CanPing ? pingIndex : -1;
        }

        private void OnCancel(object sender, CanceledEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<CanceledEventArgs>(OnCancel), sender, e);
                    } else {
                        Canceled?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnError(object sender, ErrorEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<ErrorEventArgs>(OnError), sender, e);
                    } else {
                        Error?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnFind(object sender, FindEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<FindEventArgs>(OnFind), sender, e);
                    } else {
                        Find?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnFinished(object sender, FinishedEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<FinishedEventArgs>(OnFinished), sender, e);
                    } else {
                        Finished?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnFocus(object sender, EventArgs e) => SetCurrent(((WebInfo)sender).Ordinal - 1);

        private void OnInitialized(object sender, FocusEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<FocusEventArgs>(OnInitialized), sender, e);
                    } else {
                        Initialized?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnProgress(object sender, ProgressEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<ProgressEventArgs>(OnProgress), sender, e);
                    } else {
                        Progress?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnRelay(object sender, UrlEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<UrlEventArgs>(OnRelay), sender, e);
                    } else {
                        LoadUrl(e.Url);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnStarted(object sender, StartedEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<StartedEventArgs>(OnStarted), sender, e);
                    } else {
                        Started?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnAddressChanged(object sender, AddressChangedEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<AddressChangedEventArgs>(OnAddressChanged), sender, e);
                    } else {
                        AddressChanged?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<ConsoleMessageEventArgs>(OnBrowserConsoleMessage), sender, e);
                    } else {
                        BrowserConsoleMessage?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnBrowserInitializedChanged(object sender, EventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler(OnBrowserInitializedChanged), sender, e);
                    } else {
                        BrowserInitializedChanged?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEnd), sender, e);
                    } else {
                        FrameLoadEnd?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<FrameLoadStartEventArgs>(OnFrameLoadStart), sender, e);
                    } else {
                        FrameLoadStart?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnLoadError(object sender, LoadErrorEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<LoadErrorEventArgs>(OnLoadError), sender, e);
                    } else {
                        LoadError?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<LoadingStateChangedEventArgs>(OnLoadingStateChanged), sender, e);
                    } else {
                        LoadingStateChanged?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnStatusMessage(object sender, StatusMessageEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<StatusMessageEventArgs>(OnStatusMessage), sender, e);
                    } else {
                        StatusMessage?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnTitleChanged(object sender, TitleChangedEventArgs e) {
            if (Current.Equals(sender)) {
                try {
                    if (Form.InvokeRequired) {
                        Form.Invoke(new EventHandler<TitleChangedEventArgs>(OnTitleChanged), sender, e);
                    } else {
                        TitleChanged?.Invoke(sender, e);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void OnEnter(object sender, EventArgs e) {
            if (Current.Equals(sender)) {
                Enter?.Invoke(((WebInfo)sender).Browser, e);
            }
        }

        private void OnHelp(object sender, HelpEventArgs hlpEvent) {
            if (Current.Equals(sender)) {
                Help?.Invoke(sender, hlpEvent);
            }
        }

        private void OnZoomLevelChanged(object sender, EventArgs e) {
            if (Current.Equals(sender)) {
                ZoomLevelChanged?.Invoke(sender, e);
            }
        }

        public void ZoomInCoarse() {
            if (Current != null) {
                Current.ZoomInCoarseAsync();
            }
        }

        public void ZoomOutCoarse() {
            if (Current != null) {
                Current.ZoomOutCoarseAsync();
            }
        }

        public void ZoomInFine() {
            if (Current != null) {
                Current.ZoomInFineAsync();
            }
        }

        public void ZoomOutFine() {
            if (Current != null) {
                Current.ZoomOutFineAsync();
            }
        }

        public void ActualSize() {
            if (Current != null) {
                Current.ActualSizeAsync();
            }
        }
    }
}
