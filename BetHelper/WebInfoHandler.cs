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
 * Version 1.0.0.0
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
        private decimal balance;
        private Form form;
        private int index, ordinal, pingIndex;
        private List<WebInfo> webInfos;
        private string assemblyName;
        private System.Timers.Timer getTimer, pingTimer;
        private Tip[] tips;
        private WebInfo current;

        public event EventHandler BalanceGot, BrowserInitializedChanged, Enter, TipsGot, ZoomLevelChanged;
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
            this.form = form;
            index = -1;
            pingIndex = -1;
            ParseConfig(((MainForm)form).Settings.Config);
            SetBalances(balances);
            SubscribeEvents();
            getTimer = new System.Timers.Timer();
            getTimer.Interval = Constants.InitialRightPaneInterval;
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
                    webInfos[i].Browser.GetBrowser().Reload();
                }
            });
            SetPingTimer();
        }

        public decimal Balance => balance;
        public Form Form => form;
        public List<WebInfo> WebInfos => webInfos;
        public Tip[] Tips => tips;
        public WebInfo Current => current;

        private void ParseConfig(string config) {
            WebInfo webInfo = null;
            webInfos = new List<WebInfo>();
            StringReader stringReader = new StringReader(config);
            for (string line; (line = stringReader.ReadLine()) != null;) {
                string[] configLine = line.Split(new char[] { Constants.EqualSign }, 2);
                if (configLine.Length == 2) {
                    if (webInfo == null) {
                        webInfo = GetWebInfo();
                    }
                    int val;
                    switch (configLine[0].Trim()) {
                        case Constants.ConfigTitle:
                            if (!string.IsNullOrEmpty(webInfo.Title)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Title = configLine[1].Trim();
                            break;
                        case Constants.ConfigUrl:
                            if (!string.IsNullOrEmpty(webInfo.Url)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Url = configLine[1].Trim();
                            break;
                        case Constants.ConfigUrlLive:
                            if (!string.IsNullOrEmpty(webInfo.UrlLive)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlLive = configLine[1].Trim();
                            break;
                        case Constants.ConfigUrlToLoad:
                            if (!string.IsNullOrEmpty(webInfo.UrlToLoad)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlNext = configLine[1].Trim();
                            break;
                        case Constants.ConfigUrlTips:
                            if (!string.IsNullOrEmpty(webInfo.UrlTips)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlTips = configLine[1].Trim();
                            break;
                        case Constants.ConfigUsername:
                            if (!string.IsNullOrEmpty(webInfo.Username)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Username = configLine[1].Trim();
                            break;
                        case Constants.ConfigPassword:
                            if (!string.IsNullOrEmpty(webInfo.Password)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Password = configLine[1].Trim();
                            break;
                        case Constants.ConfigScript:
                            if (!string.IsNullOrEmpty(webInfo.Script)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Script = configLine[1].Trim();
                            break;
                        case Constants.ConfigPattern:
                            if (!string.IsNullOrEmpty(webInfo.Pattern)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Pattern = configLine[1].Trim();
                            break;
                        case Constants.ConfigIetfLanguageTag:
                            if (!string.IsNullOrEmpty(webInfo.IetfLanguageTag)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.IetfLanguageTag = configLine[1].Trim();
                            break;
                        case Constants.ConfigFields:
                            if (!string.IsNullOrEmpty(webInfo.Fields)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.Fields = configLine[1].Trim();
                            break;
                        case Constants.ConfigDisplayName:
                            if (!string.IsNullOrEmpty(webInfo.DisplayName)) {
                                webInfos.Add(webInfo);
                                webInfo = GetWebInfo();
                            }
                            webInfo.DisplayName = configLine[1].Trim();
                            break;
                        case Constants.ConfigPopUpWidth:
                            if (int.TryParse(configLine[1].Trim(), out val)) {
                                webInfo.PopUpWidth = val;
                            }
                            break;
                        case Constants.ConfigPopUpHeight:
                            if (int.TryParse(configLine[1].Trim(), out val)) {
                                webInfo.PopUpHeight = val;
                            }
                            break;
                        case Constants.ConfigPopUpLeft:
                            if (int.TryParse(configLine[1].Trim(), out val)) {
                                webInfo.PopUpLeft = val;
                            }
                            break;
                        case Constants.ConfigPopUpTop:
                            if (int.TryParse(configLine[1].Trim(), out val)) {
                                webInfo.PopUpTop = val;
                            }
                            break;
                        case Constants.ConfigService:
                            webInfo.IsService = string.Compare(configLine[1].Trim(), Constants.ConfigYes, StringComparison.OrdinalIgnoreCase) == 0;
                            break;
                        case Constants.ConfigMute:
                            webInfo.AudioMutedByDefault = string.Compare(configLine[1].Trim(), Constants.ConfigYes, StringComparison.OrdinalIgnoreCase) == 0;
                            break;
                        case Constants.ConfigHandlePopUps:
                            webInfo.HandlePopUps = string.Compare(configLine[1].Trim(), Constants.ConfigYes, StringComparison.OrdinalIgnoreCase) == 0;
                            break;
                        case Constants.ConfigTabNavigation:
                            webInfo.TabNavigation = string.Compare(configLine[1].Trim(), Constants.ConfigYes, StringComparison.OrdinalIgnoreCase) == 0;
                            break;
                        case Constants.ConfigBackNavigation:
                            webInfo.BackNavigation = (WebInfo.BackNavigationType)Enum.Parse(typeof(WebInfo.BackNavigationType), configLine[1].Trim(), true);
                            break;
                        case Constants.ConfigAllowedHosts:
                            webInfo.AllowedHosts = webInfo.AllowedHosts ?? new List<string>();
                            foreach (string host in configLine[1].Split(Constants.Comma)) {
                                webInfo.AllowedHosts.Add(host.Trim());
                            }
                            break;
                        case Constants.ConfigChat:
                            webInfo.ChatHosts = webInfo.ChatHosts ?? new List<string>();
                            foreach (string host in configLine[1].Split(Constants.Comma)) {
                                webInfo.ChatHosts.Add(host.Trim());
                            }
                            break;
                    }
                }
            }
            if (webInfo != null && !string.IsNullOrEmpty(webInfo.Title)) {
                webInfos.Add(webInfo);
            }
        }

        private void SetBalances(decimal[] balances) {
            if (balances != null) {
                for (int i = 0; i < webInfos.Count; i++) {
                    webInfos[i].SetBalance(i < balances.Length ? balances[i] : decimal.MinValue);
                }
            }
        }

        private void SubscribeEvents() {
            foreach (WebInfo webInfo in webInfos) {
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
            foreach (WebInfo webInfo in webInfos) {
                webInfo.Dispose();
            }
            getTimer.Stop();
            getTimer.Dispose();
            pingTimer.Stop();
            pingTimer.Dispose();
        }

        private void GetBalance() {
            decimal balance = decimal.Zero;
            foreach (WebInfo webInfo in webInfos) {
                decimal bal = webInfo.GetBalance();
                if (bal != decimal.MinValue) {
                    balance += bal;
                }
            }
            this.balance = balance;
            BalanceGot?.Invoke(this, EventArgs.Empty);
        }

        public decimal[] GetBalances() {
            decimal[] balances = new decimal[webInfos.Count];
            for (int i = 0; i < webInfos.Count; i++) {
                balances[i] = webInfos[i].GetBalance();
            }
            return balances;
        }

        public string GetCurrentAddress() {
            if (current != null) {
                if (!string.IsNullOrWhiteSpace(current.BrowserAddress) && current.BrowserAddress.StartsWith(Constants.SchemeHttps)) {
                    return current.BrowserAddress;
                } else if (current.Browser != null && !string.IsNullOrWhiteSpace(current.Browser.Address) && current.Browser.Address.StartsWith(Constants.SchemeHttps)) {
                    return current.Browser.Address;
                }
            }
            return null;
        }

        private void GetTips() {
            List<Tip> list = new List<Tip>();
            foreach (WebInfo webInfo in webInfos) {
                if (webInfo.IsService) {
                    Tip[] tips = webInfo.GetTips();
                    if (tips != null) {
                        list.AddRange(tips);
                    }
                }
            }
            this.tips = list.ToArray();
            TipsGot?.Invoke(this, EventArgs.Empty);
        }

        public int LoadUrl(string url) {
            for (int i = 0; i < webInfos.Count; i++) {
                if (webInfos[i].EqualsSecondLevelDomain(url)) {
                    SetCurrent(i);
                    webInfos[i].SetUrlToLoad(url);
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
            if (current != null) {
                current.Suspend();
            }
            current = webInfos[index];
            if (current.Browser == null) {
                current.Initialize();
            } else {
                current.Resume();
            }
            CurrentSet?.Invoke(this, new FocusEventArgs(current, index));
            getTimer.Start();
            return;
        }

        public void StopFinding() {
            foreach (WebInfo webInfo in webInfos) {
                if (webInfo.Browser != null) {
                    webInfo.StopFinding(false);
                }
            }
        }

        public void SendKey(Keys key) {
            if (current != null) {
                current.SendKey(key);
            }
        }

        public void SetPingTimer() {
            if (((MainForm)form).Settings.TryToKeepUserLoggedIn) {
                pingTimer.Start();
            } else {
                pingTimer.Stop();
            }
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
                if (++pingIndex == webInfos.Count) {
                    pingIndex = 0;
                }
            } while (++i < webInfos.Count && !webInfos[pingIndex].IsBookmaker && !webInfos[pingIndex].IsService);
            return webInfos[pingIndex].CanPing ? pingIndex : -1;
        }

        private void OnCancel(object sender, CanceledEventArgs e) {
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<CanceledEventArgs>(OnCancel), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<ErrorEventArgs>(OnError), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<FindEventArgs>(OnFind), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<FinishedEventArgs>(OnFinished), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<FocusEventArgs>(OnInitialized), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<ProgressEventArgs>(OnProgress), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<UrlEventArgs>(OnRelay), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<StartedEventArgs>(OnStarted), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<AddressChangedEventArgs>(OnAddressChanged), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<ConsoleMessageEventArgs>(OnBrowserConsoleMessage), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler(OnBrowserInitializedChanged), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEnd), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<FrameLoadStartEventArgs>(OnFrameLoadStart), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<LoadErrorEventArgs>(OnLoadError), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<LoadingStateChangedEventArgs>(OnLoadingStateChanged), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<StatusMessageEventArgs>(OnStatusMessage), sender, e);
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
            if (current.Equals(sender)) {
                try {
                    if (form.InvokeRequired) {
                        form.Invoke(new EventHandler<TitleChangedEventArgs>(OnTitleChanged), sender, e);
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
            if (current.Equals(sender)) {
                Enter?.Invoke(((WebInfo)sender).Browser, e);
            }
        }

        private void OnHelp(object sender, HelpEventArgs hlpEvent) {
            if (current.Equals(sender)) {
                Help?.Invoke(sender, hlpEvent);
            }
        }

        private void OnZoomLevelChanged(object sender, EventArgs e) {
            if (current.Equals(sender)) {
                ZoomLevelChanged?.Invoke(sender, e);
            }
        }

        public void ZoomInCoarse() {
            if (current != null) {
                current.ZoomInCoarseAsync();
            }
        }

        public void ZoomOutCoarse() {
            if (current != null) {
                current.ZoomOutCoarseAsync();
            }
        }

        public void ZoomInFine() {
            if (current != null) {
                current.ZoomInFineAsync();
            }
        }

        public void ZoomOutFine() {
            if (current != null) {
                current.ZoomOutFineAsync();
            }
        }

        public void ActualSize() {
            if (current != null) {
                current.ActualSizeAsync();
            }
        }
    }
}
