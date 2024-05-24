/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.12
 */

using CefSharp;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BetHelper {
    public sealed class WebInfoHandler : IDisposable {
        private bool stopped;
        private int index;
        private int n;
        private int ordinal;
        private int pingIndex;
        private string assemblyName;
        private System.Timers.Timer getTimer;
        private System.Timers.Timer pingTimer;
        private System.Timers.Timer teardownTimer;

        private delegate void WebInfoHandlerCallback();

        public event EventHandler AlertableFOGot;
        public event EventHandler AlertableNTGot;
        public event EventHandler BalanceGot;
        public event EventHandler BrowserInitializedChanged;
        public event EventHandler Enter;
        public event EventHandler FastOpportunityGot;
        public event EventHandler Suspended;
        public event EventHandler TipsGot;
        public event EventHandler ZoomLevelChanged;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<CanceledEventArgs> Canceled;
        public event EventHandler<ConsoleMessageEventArgs> BrowserConsoleMessage;
        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler<FindEventArgs> Find;
        public event EventHandler<FinishedEventArgs> Finished;
        public event EventHandler<FocusEventArgs> CurrentSet;
        public event EventHandler<FocusEventArgs> Initialized;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ProgressEventArgs> Progress;
        public event EventHandler<StartedEventArgs> Started;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        public event HelpEventHandler Help;

        public event EventHandler AltCtrlShiftEPressed;
        public event EventHandler AltCtrlShiftPPressed;
        public event EventHandler AltF10Pressed;
        public event EventHandler AltF11Pressed;
        public event EventHandler AltF12Pressed;
        public event EventHandler AltF5Pressed;
        public event EventHandler AltF7Pressed;
        public event EventHandler AltF8Pressed;
        public event EventHandler AltF9Pressed;
        public event EventHandler AltHomePressed;
        public event EventHandler AltLeftPressed;
        public event EventHandler AltLPressed;
        public event EventHandler AltRightPressed;
        public event EventHandler CtrlDPressed;
        public event EventHandler CtrlEPressed;
        public event EventHandler CtrlFPressed;
        public event EventHandler CtrlF5Pressed;
        public event EventHandler CtrlGPressed;
        public event EventHandler CtrlMinusPressed;
        public event EventHandler CtrlMPressed;
        public event EventHandler CtrlOPressed;
        public event EventHandler CtrlPlusPressed;
        public event EventHandler CtrlPPressed;
        public event EventHandler CtrlShiftDelPressed;
        public event EventHandler CtrlShiftEPressed;
        public event EventHandler CtrlShiftMinusPressed;
        public event EventHandler CtrlShiftNPressed;
        public event EventHandler CtrlShiftPlusPressed;
        public event EventHandler CtrlShiftPPressed;
        public event EventHandler CtrlTPressed;
        public event EventHandler CtrlUPressed;
        public event EventHandler CtrlZeroPressed;
        public event EventHandler DownPressed;
        public event EventHandler EndPressed;
        public event EventHandler F11Pressed;
        public event EventHandler F12Pressed;
        public event EventHandler F2Pressed;
        public event EventHandler F3Pressed;
        public event EventHandler F5Pressed;
        public event EventHandler F7Pressed;
        public event EventHandler F8Pressed;
        public event EventHandler F9Pressed;
        public event EventHandler HomePressed;
        public event EventHandler PageDownPressed;
        public event EventHandler PageUpPressed;
        public event EventHandler ShiftF3Pressed;
        public event EventHandler UpPressed;

        public WebInfoHandler(Form form, decimal[] balances) {
            Form = form;
            Form.Load += new EventHandler((sender, e) => {
                SetTimer();
                SetPingTimer();
            });
            index = -1;
            pingIndex = -1;
            ParseConfig(((MainForm)form).Settings.Config);
            SetWebInfoIndex();
            SetBalances(balances);
            SubscribeEvents();
            getTimer = new System.Timers.Timer();
            getTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                getTimer.Interval = Constants.RightPaneInterval;
                GetAlertableNT();
                GetAlertableFO();
                GetBalance();
                GetTips();
                GetFastOpportunity();
            });
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = Constants.PingNextTabInterval;
            pingTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                int i = NextPing();
                if (i >= 0) {
                    WebInfos[i].Browser.GetBrowser().Reload();
                }
            });
            teardownTimer = new System.Timers.Timer();
            teardownTimer.Interval = Constants.WebInfoHeartBeatInterval * 2;
            teardownTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                if (n++ < 1) {
                    CloseBrowsers();
                } else if (stopped) {
                    teardownTimer.Stop();
                    IsTornDown = true;
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

        public bool AlertableNT { get; private set; }

        public bool AlertableFO { get; private set; }

        public bool HasFastOpportunity { get; private set; }

        public bool IsTornDown { get; private set; }

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
                    if (webInfo != null) {
                        webInfo.Teardown();
                    }
                }
            }
        }

        private void ParseConfig(string config) {
            WebInfo webInfo = null;
            WebInfos = new List<WebInfo>();
            StringReader stringReader = new StringReader(config);
            for (string line; (line = stringReader.ReadLine()) != null;) {
                char[] chars = line.ToCharArray();
                if (chars.Length > 0 && (chars[0].Equals(Constants.Semicolon) || chars[0].Equals(Constants.NumberSign))) {
                    continue;
                }
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
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Title = value;
                            break;
                        case Constants.ConfigUrl:
                            if (!string.IsNullOrEmpty(webInfo.Url)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Url = value;
                            break;
                        case Constants.ConfigUrlLive:
                            if (!string.IsNullOrEmpty(webInfo.UrlLive)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlLive = value;
                            break;
                        case Constants.ConfigUrlToLoad:
                            if (!string.IsNullOrEmpty(webInfo.UrlToLoad)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlNext = value;
                            break;
                        case Constants.ConfigUrlTips:
                            if (!string.IsNullOrEmpty(webInfo.UrlTips)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.UrlTips = value;
                            break;
                        case Constants.ConfigUserName:
                            if (!string.IsNullOrEmpty(webInfo.UserName)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.UserName = value;
                            break;
                        case Constants.ConfigPassword:
                            if (!string.IsNullOrEmpty(webInfo.Password)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Password = value;
                            break;
                        case Constants.ConfigScript:
                            if (!string.IsNullOrEmpty(webInfo.Script)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Script = value;
                            break;
                        case Constants.ConfigPattern:
                            if (!string.IsNullOrEmpty(webInfo.Pattern)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Pattern = value;
                            break;
                        case Constants.ConfigIetfLanguageTag:
                            if (!string.IsNullOrEmpty(webInfo.IetfLanguageTag)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.IetfLanguageTag = value;
                            break;
                        case Constants.ConfigFields:
                            if (!string.IsNullOrEmpty(webInfo.Fields)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
                                webInfo = GetWebInfo();
                            }
                            webInfo.Fields = value;
                            break;
                        case Constants.ConfigDisplayName:
                            if (!string.IsNullOrEmpty(webInfo.DisplayName)) {
                                if (webInfo.Show) {
                                    WebInfos.Add(webInfo);
                                }
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
                        case Constants.ConfigShow:
                            webInfo.Show = string
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
                if (webInfo.Show) {
                    WebInfos.Add(webInfo);
                }
            }
        }

        private void SetWebInfoIndex() {
            for (int i = 0; i < WebInfos.Count; i++) {
                WebInfos[i].Index = i;
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

                webInfo.AltCtrlShiftEPressed += new EventHandler((sender, e) => AltCtrlShiftEPressed?.Invoke(sender, e));
                webInfo.AltCtrlShiftPPressed += new EventHandler((sender, e) => AltCtrlShiftPPressed?.Invoke(sender, e));
                webInfo.AltF10Pressed += new EventHandler((sender, e) => AltF10Pressed?.Invoke(sender, e));
                webInfo.AltF11Pressed += new EventHandler((sender, e) => AltF11Pressed?.Invoke(sender, e));
                webInfo.AltF12Pressed += new EventHandler((sender, e) => AltF12Pressed?.Invoke(sender, e));
                webInfo.AltF5Pressed += new EventHandler((sender, e) => AltF5Pressed?.Invoke(sender, e));
                webInfo.AltF7Pressed += new EventHandler((sender, e) => AltF7Pressed?.Invoke(sender, e));
                webInfo.AltF8Pressed += new EventHandler((sender, e) => AltF8Pressed?.Invoke(sender, e));
                webInfo.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
                webInfo.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
                webInfo.AltHomePressed += new EventHandler((sender, e) => AltHomePressed?.Invoke(sender, e));
                webInfo.AltLeftPressed += new EventHandler((sender, e) => AltLeftPressed?.Invoke(sender, e));
                webInfo.AltLPressed += new EventHandler((sender, e) => AltLPressed?.Invoke(sender, e));
                webInfo.AltRightPressed += new EventHandler((sender, e) => AltRightPressed?.Invoke(sender, e));
                webInfo.CtrlDPressed += new EventHandler((sender, e) => CtrlDPressed?.Invoke(sender, e));
                webInfo.CtrlEPressed += new EventHandler((sender, e) => CtrlEPressed?.Invoke(sender, e));
                webInfo.CtrlFPressed += new EventHandler((sender, e) => CtrlFPressed?.Invoke(sender, e));
                webInfo.CtrlF5Pressed += new EventHandler((sender, e) => CtrlF5Pressed?.Invoke(sender, e));
                webInfo.CtrlGPressed += new EventHandler((sender, e) => CtrlGPressed?.Invoke(sender, e));
                webInfo.CtrlMinusPressed += new EventHandler((sender, e) => CtrlMinusPressed?.Invoke(sender, e));
                webInfo.CtrlMPressed += new EventHandler((sender, e) => CtrlMPressed?.Invoke(sender, e));
                webInfo.CtrlOPressed += new EventHandler((sender, e) => CtrlOPressed?.Invoke(sender, e));
                webInfo.CtrlPlusPressed += new EventHandler((sender, e) => CtrlPlusPressed?.Invoke(sender, e));
                webInfo.CtrlPPressed += new EventHandler((sender, e) => CtrlPPressed?.Invoke(sender, e));
                webInfo.CtrlShiftDelPressed += new EventHandler((sender, e) => CtrlShiftDelPressed?.Invoke(sender, e));
                webInfo.CtrlShiftEPressed += new EventHandler((sender, e) => CtrlShiftEPressed?.Invoke(sender, e));
                webInfo.CtrlShiftMinusPressed += new EventHandler((sender, e) => CtrlShiftMinusPressed?.Invoke(sender, e));
                webInfo.CtrlShiftNPressed += new EventHandler((sender, e) => CtrlShiftNPressed?.Invoke(sender, e));
                webInfo.CtrlShiftPlusPressed += new EventHandler((sender, e) => CtrlShiftPlusPressed?.Invoke(sender, e));
                webInfo.CtrlShiftPPressed += new EventHandler((sender, e) => CtrlShiftPPressed?.Invoke(sender, e));
                webInfo.CtrlTPressed += new EventHandler((sender, e) => CtrlTPressed?.Invoke(sender, e));
                webInfo.CtrlUPressed += new EventHandler((sender, e) => CtrlUPressed?.Invoke(sender, e));
                webInfo.CtrlZeroPressed += new EventHandler((sender, e) => CtrlZeroPressed?.Invoke(sender, e));
                webInfo.DownPressed += new EventHandler((sender, e) => DownPressed?.Invoke(sender, e));
                webInfo.EndPressed += new EventHandler((sender, e) => EndPressed?.Invoke(sender, e));
                webInfo.F11Pressed += new EventHandler((sender, e) => F11Pressed?.Invoke(sender, e));
                webInfo.F12Pressed += new EventHandler((sender, e) => F12Pressed?.Invoke(sender, e));
                webInfo.F2Pressed += new EventHandler((sender, e) => F2Pressed?.Invoke(sender, e));
                webInfo.F3Pressed += new EventHandler((sender, e) => F3Pressed?.Invoke(sender, e));
                webInfo.F5Pressed += new EventHandler((sender, e) => F5Pressed?.Invoke(sender, e));
                webInfo.F7Pressed += new EventHandler((sender, e) => F7Pressed?.Invoke(sender, e));
                webInfo.F8Pressed += new EventHandler((sender, e) => F8Pressed?.Invoke(sender, e));
                webInfo.F9Pressed += new EventHandler((sender, e) => F9Pressed?.Invoke(sender, e));
                webInfo.HomePressed += new EventHandler((sender, e) => HomePressed?.Invoke(sender, e));
                webInfo.PageDownPressed += new EventHandler((sender, e) => PageDownPressed?.Invoke(sender, e));
                webInfo.PageUpPressed += new EventHandler((sender, e) => PageUpPressed?.Invoke(sender, e));
                webInfo.ShiftF3Pressed += new EventHandler((sender, e) => ShiftF3Pressed?.Invoke(sender, e));
                webInfo.UpPressed += new EventHandler((sender, e) => UpPressed?.Invoke(sender, e));
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
            teardownTimer.Dispose();
        }

        private void GetAlertableNT() {
            bool alertable = true;
            bool atLeastOneService = false;
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.IsActuallyService && !string.IsNullOrEmpty(webInfo.UrlTips)) {
                    atLeastOneService = true;
                    if (!webInfo.AlertableNT) {
                        alertable = false;
                        break;
                    }
                }
            }
            AlertableNT = alertable && atLeastOneService;
            AlertableNTGot?.Invoke(this, EventArgs.Empty);
        }

        private void GetAlertableFO() {
            bool alertable = true;
            bool atLeastOneService = false;
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.IsBookmaker && !string.IsNullOrEmpty(webInfo.UrlLive)) {
                    atLeastOneService = true;
                    if (!webInfo.AlertableFO) {
                        alertable = false;
                        break;
                    }
                }
            }
            AlertableFO = alertable && atLeastOneService;
            AlertableFOGot?.Invoke(this, EventArgs.Empty);
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

        public IntPtr[] GetOpenedWebInfoForms() {
            List<IntPtr> list = new List<IntPtr>(WebInfos.Count);
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo != null) {
                    IntPtr handle = webInfo.GetWebInfoFormHandle();
                    if (!handle.Equals(IntPtr.Zero)) {
                        list.Add(handle);
                    }
                }
            }
            return list.ToArray();
        }

        private void GetTips() {
            List<Tip> list = new List<Tip>();
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.IsActuallyService) {
                    Tip[] tips = webInfo.GetTips();
                    if (tips != null) {
                        list.AddRange(tips);
                    }
                }
            }
            Tips = list.ToArray();
            TipsGot?.Invoke(this, EventArgs.Empty);
        }


        private void GetFastOpportunity() {
            bool hasFastOpportunity = false;
            foreach (WebInfo webInfo in WebInfos) {
                if (webInfo.HasFastOpportunity()) {
                    hasFastOpportunity = true;
                    break;
                }
            }
            HasFastOpportunity = hasFastOpportunity;
            FastOpportunityGot?.Invoke(this, EventArgs.Empty);
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

        public void CloseInfos() {
            foreach (WebInfo webInfo in WebInfos) {
                webInfo.CloseInfo();
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

        public void Teardown() {
            if (!IsTornDown) {
                getTimer.Stop();
                pingTimer.Stop();
                teardownTimer.Start();
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
            StringBuilder className = new StringBuilder()
                .Append(GetAssemblyName())
                .Append(Constants.Period)
                .Append(Constants.WebInfo)
                .Append((++ordinal).ToString(Constants.TwoDigitPad));
            try {
                Type type = Type.GetType(className.ToString());
                if (type == null) {
                    throw new ApplicationException(string.Format(Properties.Resources.MessageMissingClassError, className));
                }
                WebInfo webInfo = (WebInfo)Activator.CreateInstance(type);
                webInfo.Ordinal = ordinal;
                return webInfo;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                throw new InvalidOperationException(string.Format(Properties.Resources.MessageCreatingInstanceError, className),
                    exception);
            }
        }

        private int NextPing() {
            if (WebInfos.Count.Equals(0)) {
                return -1;
            }
            int i = pingIndex < 0 ? 0 : pingIndex;
            int j = i;
            do {
                if (WebInfos.Count.Equals(++i)) {
                    i = 0;
                }
                if (i.Equals(j)) {
                    return -1;
                }
            } while (!((WebInfos[i].IsBookmaker || WebInfos[i].IsActuallyService) && WebInfos[i].CanPing));
            pingIndex = i;
            return i;
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

        private void OnFocus(object sender, EventArgs e) => SetCurrent(((WebInfo)sender).Index);

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

        public void PingReset() {
            if (!IsTornDown && Current != null) {
                Current.PingReset();
            }
        }
    }
}
