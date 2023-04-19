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
 * Version 1.1.4.1
 */

using CefSharp;
using CefSharp.WinForms;
using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo : IDisposable {
        protected bool frameInitialLoadEnded, audioMutedByDefault, pingTimerElapsed;
        protected decimal balance;
        protected FindEventArgs findEventArgs;
        protected int currentItem, finished, heartBeatTimerRun, heartBeatTimerStart, itemsTotal, loaded, loadingBeforeLogIn;
        protected PersistWindowState persistWindowState;
        protected Regex secondLevelDomain, urlScheme;
        protected SemaphoreSlim heartBeatSemaphore, loadingBeforeLogInSemaphore;
        protected string urlNext;
        protected System.Timers.Timer loadingBeforeLogInTimer, heartBeatTimer, pingTimer;
        protected Thread logInThread, popUpThread, infoThread;
        protected WebInfoForm webInfoForm;

        public event EventHandler BrowserInitializedChanged, Enter, Focus, ZoomLevelChanged;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<CanceledEventArgs> Canceled;
        public event EventHandler<ConsoleMessageEventArgs> BrowserConsoleMessage;
        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler<FindEventArgs> Find;
        public event EventHandler<FinishedEventArgs> Finished;
        public event EventHandler<FocusEventArgs> Initialized;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ProgressEventArgs> Progress;
        public event EventHandler<StartedEventArgs> Started;
        public event EventHandler<StatusMessageEventArgs> BrowserStatusMessage;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        public event EventHandler<UrlEventArgs> Relay;
        public event HelpEventHandler Help;

        public WebInfo() {
            balance = decimal.MinValue;
            secondLevelDomain = new Regex(Constants.SecondLevelDomainPattern);
            urlScheme = new Regex(Constants.SchemePresenceTestPattern, RegexOptions.IgnoreCase);
            findEventArgs = new FindEventArgs();
            persistWindowState = new PersistWindowState();
            persistWindowState.SavingOptions = PersistWindowState.PersistWindowStateSavingOptions.None;
            heartBeatSemaphore = new SemaphoreSlim(0, 1);
            loadingBeforeLogInSemaphore = new SemaphoreSlim(0, 1);
            loadingBeforeLogInTimer = new System.Timers.Timer();
            loadingBeforeLogInTimer.Interval = Constants.LoadedToLogInInterval;
            loadingBeforeLogInTimer.AutoReset = false;
            loadingBeforeLogInTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                loadingBeforeLogInSemaphore.Wait();
                if (Browser.CanExecuteJavascriptInMainFrame) {
                    loadingBeforeLogInTimer.Stop();
                    if (IsLoggedIn()) {
                        NoLogIn(Browser);
                        SetFinished();
                        Finished?.Invoke(this, new FinishedEventArgs(Properties.Resources.MessageActionFinished));
                        LoadUrlToLoad();
                    } else {
                        ExecuteLogIn();
                    }
                }
                loadingBeforeLogInSemaphore.Release();
            });
            heartBeatTimer = new System.Timers.Timer();
            heartBeatTimer.Interval = Constants.InitialHeartBeatInterval;
            heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                heartBeatTimer.Interval = Constants.HeartBeatInterval;
                if (heartBeatTimerRun++ < Constants.HeartBeatCycles) {
                    heartBeatSemaphore.Wait();
                    if (Browser.CanExecuteJavascriptInMainFrame) {
                        HeartBeat(Browser);
                    }
                    heartBeatSemaphore.Release();
                } else {
                    heartBeatTimerRun = Constants.HeartBeatCycles;
                }
            });
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = Constants.PingTabExpInterval;
            pingTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                pingTimer.Stop();
                pingTimerElapsed = true;
            });
            heartBeatSemaphore.Release(1);
            loadingBeforeLogInSemaphore.Release(1);
        }

        public int Ordinal { get; set; }

        public Form Dialog { get; set; }

        public WebInfoHandler Parent { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string UrlLive { get; set; }

        public string UrlToLoad { get; private set; }

        public string UrlTips { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ChromiumWebBrowser Browser { get; private set; }

        public string Script { get; set; }

        public string Pattern { get; set; }

        public string Fields { get; set; }

        public string DisplayName { get; set; }

        public bool IsService { get; set; }

        public bool HandlePopUps { get; set; }

        public int PopUpWidth { get; set; }

        public int PopUpHeight { get; set; }

        public int PopUpLeft { get; set; }

        public int PopUpTop { get; set; }

        public string IetfLanguageTag { get; set; }

        public bool TabNavigation { get; set; }

        public BackNavigationType BackNavigation { get; set; }

        public List<string> AllowedHosts { get; set; }

        public List<string> ChatHosts { get; set; }

        public string BrowserTitle { get; private set; }

        public string BrowserAddress { get; private set; }

        public PopUpEventArgs PopUpArgs { get; set; }

        public bool CanGoBack { get; private set; }

        public bool CanGoForward { get; private set; }

        public bool CanReload { get; private set; }

        public bool IsLoading { get; private set; }

        public int ConsoleLine { get; private set; }

        public string ConsoleSource { get; private set; }

        public string ConsoleMessage { get; private set; }

        public CefErrorCode ErrorCode { get; private set; }

        public string ErrorText { get; private set; }

        public string FailedUrl { get; private set; }

        public string StatusMessage { get; private set; }

        public double ZoomLevel { get; private set; }

        protected bool RemoveChat { get; private set; } = true;

        public bool IsBookmaker => Fields == null ? false : Fields.Contains(Constants.FieldBalance);

        public bool WillTryToKeepUserLoggedIn => IsBookmaker || IsService;

        public bool CanPing => WillTryToKeepUserLoggedIn && CanReload && !IsLoading && pingTimerElapsed && IsLoggedIn();

        public bool HasChat => ChatHosts != null && ChatHosts.Count > 0;

        public bool IsChatHidden => RemoveChat;

        public bool IsAudioMuted { get; private set; }

        public bool AudioMutedByDefault {
            get {
                return audioMutedByDefault;
            }
            set {
                audioMutedByDefault = value;
                IsAudioMuted = value;
            }
        }

        public string UrlNext {
            get {
                return urlNext;
            }
            set {
                urlNext = value;
                UrlToLoad = value;
            }
        }

        public bool WillActuallyHandlePopUps {
            get {
                return HandlePopUps || !PopUpLeft.Equals(0) || !PopUpTop.Equals(0) || !PopUpWidth.Equals(0) || !PopUpHeight.Equals(0);
            }
        }

        public void SetAudioMuted(bool mute) {
            Browser.GetBrowser().GetHost().SetAudioMuted(mute);
            IsAudioMuted = mute;
        }

        public void HideChat() {
            if (HasChat) {
                RemoveChat = true;
                HeartBeatReset();
            }
        }

        public void ShowChat() {
            if (HasChat) {
                RemoveChat = false;
                Reload(Browser);
            }
        }

        public void ShowInfo() {
            if (infoThread != null && infoThread.IsAlive) {
                try {
                    webInfoForm.SafeSelect();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            } else {
                infoThread = new Thread(new ThreadStart(Info));
                infoThread.Start();
            }
        }

        private void Info() {
            webInfoForm = new WebInfoForm(this);
            persistWindowState.Parent = webInfoForm;
            webInfoForm.HelpRequested += new HelpEventHandler((sender, hlpevent) => Help?.Invoke(sender, hlpevent));
            webInfoForm.ShowDialog();
        }

        public void CancelLogIn() {
            loadingBeforeLogInTimer.Stop();
            if (logInThread != null && logInThread.IsAlive) {
                logInThread.Abort();
                logInThread = null;
            }
            if (loaded++ < 1) {
                Canceled?.Invoke(this, new CanceledEventArgs(Properties.Resources.MessageLogInCanceled));
            } else {
                loaded = 1;
            }
        }

        public void SetBalance(decimal balance) {
            if (this.balance > decimal.MinValue) {
                return;
            }
            this.balance = balance;
        }

        public decimal GetBalance() {
            if (Browser != null && !string.IsNullOrEmpty(Script)) {
                for (int i = 0; i < 2; i++) {
                    if (i > 0) {
                        Thread.Sleep(50);
                    }
                    try {
                        if (Browser.CanExecuteJavascriptInMainFrame) {
                            JavascriptResponse javascriptResponse = Browser.EvaluateScriptAsync(Script).GetAwaiter().GetResult();
                            if (javascriptResponse.Success) {
                                balance = GetBalance((string)javascriptResponse.Result);
                            }
                        }
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
            return balance;
        }

        protected string GetDisplayName(string response) {
            string[] fields = Fields
                .Split(Constants.Comma)
                .Select(new Func<string, string>(field => field.Trim()))
                .ToArray();
            try {
                if (fields.Contains(Constants.FieldDisplayName)) {
                    if (string.IsNullOrEmpty(Pattern)) {
                        return fields.Length.Equals(1) ? response : null;
                    } else {
                        return Regex.Replace(response, Pattern,
                            string.Format(Constants.ReplaceIndex,
                                Array.FindIndex(fields,
                                    new Predicate<string>(field => field.Contains(Constants.FieldDisplayName))) + 1));
                    }
                } else if (fields.Contains(Constants.FieldUserName)) {
                    if (string.IsNullOrEmpty(Pattern)) {
                        return fields.Length.Equals(1) ? response : null;
                    } else {
                        return Regex.Replace(response, Pattern,
                            string.Format(Constants.ReplaceIndex,
                                Array.FindIndex(fields,
                                    new Predicate<string>(field => field.Contains(Constants.FieldUserName))) + 1));
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return null;
        }

        protected string GetDisplayName() {
            string[] fields = Fields
                .Split(Constants.Comma)
                .Select(new Func<string, string>(field => field.Trim()))
                .ToArray();
            if (fields.Contains(Constants.FieldDisplayName)) {
                return DisplayName;
            } else if (fields.Contains(Constants.FieldUserName)) {
                return UserName;
            }
            return null;
        }

        private void ExecuteLogIn() {
            if (logInThread != null && logInThread.IsAlive) {
                if (Dialog is ProgressBarForm) {
                    Dialog.Activate();
                }
                return;
            }
            logInThread = new Thread(new ThreadStart(LogInThread));
            logInThread.Start();
        }

        private void LogInThread() {
            try {
                LogIn(Browser);
            } catch (Exception exception) {
                StringBuilder message = new StringBuilder()
                    .Append(Properties.Resources.MessageLogInFailed)
                    .Append(Constants.Colon)
                    .Append(Constants.Space)
                    .Append(exception.Message);
                Error?.Invoke(this, new ErrorEventArgs(message.ToString()));
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private decimal GetBalance(string response) {
            string[] fields = Fields
                .Split(Constants.Comma)
                .Select(new Func<string, string>(field => field.Trim()))
                .ToArray();
            try {
                if (fields.Contains(Constants.FieldBalance)) {
                    if (string.IsNullOrEmpty(Pattern)) {
                        return fields.Length.Equals(1)
                            ? decimal.Parse(
                                Regex.Replace(response, Constants.JSBalancePattern, string.Empty),
                                CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag))
                            : decimal.MinValue;
                    } else {
                        return decimal.Parse(
                            Regex.Replace(
                                Regex.Replace(response, Pattern,
                                    string.Format(Constants.ReplaceIndex,
                                        Array.FindIndex(fields,
                                            new Predicate<string>(field => field.Contains(Constants.FieldBalance))) + 1)),
                                Constants.JSBalancePattern, string.Empty),
                            CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag));
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return decimal.MinValue;
        }

        private void HeartBeatReset() {
            heartBeatTimerRun = 0;
            if (heartBeatTimerStart++ < 1) {
                heartBeatTimer.Start();
            } else {
                heartBeatTimerStart = 1;
            }
        }

        private void PingReset() {
            pingTimerElapsed = false;
            pingTimer.Stop();
            pingTimer.Start();
        }

        private void SetFinished() {
            if (finished++ > 0) {
                finished = 1;
            }
        }

        private void SetZoomLevel(double zoomLevel) {
            if (Browser != null) {
                ZoomLevel = zoomLevel;
                Browser.SetZoomLevel(zoomLevel);
                HeartBeatReset();
                ZoomLevelChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void ZoomInCoarseAsync() {
            if (Browser != null) {
                double zoomLevel = await Browser.GetZoomLevelAsync();
                if (zoomLevel < Constants.MaximumZoomLevel) {
                    SetZoomLevel((Math.Floor(zoomLevel * 2) + 1) / 2);
                }
            }
        }

        public async void ZoomOutCoarseAsync() {
            if (Browser != null) {
                double zoomLevel = await Browser.GetZoomLevelAsync();
                if (zoomLevel > Constants.MinimumZoomLevel) {
                    SetZoomLevel((Math.Ceiling(zoomLevel * 2) - 1) / 2);
                }
            }
        }

        public async void ZoomInFineAsync() {
            if (Browser != null) {
                double zoomLevel = await Browser.GetZoomLevelAsync();
                if (zoomLevel < Constants.MaximumZoomLevel) {
                    SetZoomLevel(zoomLevel + 0.1);
                }
            }
        }

        public async void ZoomOutFineAsync() {
            if (Browser != null) {
                double zoomLevel = await Browser.GetZoomLevelAsync();
                if (zoomLevel > Constants.MinimumZoomLevel) {
                    SetZoomLevel(zoomLevel - 0.1);
                }
            }
        }

        public async void ActualSizeAsync() {
            if (Browser != null) {
                double zoomLevel = await Browser.GetZoomLevelAsync();
                if (!zoomLevel.Equals(0)) {
                    SetZoomLevel(0);
                }
            }
        }

        public void Suspend() {
            heartBeatTimer.Enabled = false;
            CancelLogIn();
        }

        public void Resume() {
            heartBeatTimer.Interval = Constants.InitialHeartBeatInterval;
            heartBeatTimer.Enabled = true;
            Find?.Invoke(this, findEventArgs);
        }

        public bool CanNavigate(string targetUrl) {
            if (IsAllowedUrl(targetUrl)) {
                return true;
            }
            if (IsForbiddenUrl(targetUrl)) {
                return false;
            }
            Settings settings = ((MainForm)Parent.Form).Settings;
            if (settings.BlockRequestsToForeignUrls) {
                if (EqualsSecondLevelDomain(targetUrl)) {
                    return true;
                }
                if (AllowedHosts != null) {
                    foreach (string host in AllowedHosts) {
                        if (IsSubdomainOf(targetUrl, host.Trim())) {
                            return true;
                        }
                    }
                }
                if (!RemoveChat && ChatHosts != null) {
                    foreach (string host in ChatHosts) {
                        if (IsSubdomainOf(targetUrl, host.Trim())) {
                            return true;
                        }
                    }
                }
                if (settings.LogForeignUrls) {
                    LogForeignUrl(targetUrl);
                }
                if (TabNavigation) {
                    Relay?.Invoke(this, new UrlEventArgs(targetUrl));
                }
                return false;
            }
            return true;
        }

        public void Initialize() {
            Settings settings = ((MainForm)Parent.Form).Settings;
            if (settings.AutoLogInAfterInitialLoad
                    && !string.IsNullOrWhiteSpace(UserName)
                    && !string.IsNullOrEmpty(Password)
                    && !string.IsNullOrWhiteSpace(Script)) {

                Started?.Invoke(this, new StartedEventArgs());
            }
            Browser = new ChromiumWebBrowser(new Uri(Url).AbsoluteUri);
            FindHandler findHandler = new FindHandler();
            findHandler.Find += new EventHandler<FindEventArgs>((sender, e) => {
                findEventArgs = e;
                Find?.Invoke(this, e);
            });
            Browser.FindHandler = findHandler;
            RequestHandler requestHandler = new RequestHandler() {
                WebInfo = this
            };
            StatusStripHandler statusStripHandler = ((MainForm)Parent.Form).StatusStripHandler;
            requestHandler.Canceled += new EventHandler((sender, e) => statusStripHandler.SetMessage(
                StatusStripHandler.StatusMessageType.PersistentA,
                Properties.Resources.MessageActionCanceled));
            Browser.RequestHandler = requestHandler;
            if (WillActuallyHandlePopUps) {
                LifeSpanHandler lifeSpanHandler = new LifeSpanHandler();
                lifeSpanHandler.BrowserPopUp += new EventHandler<PopUpEventArgs>((sender, popUpArgs) => {
                    if (CanNavigate(popUpArgs.TargetUrl)) {
                        if (BackNavigation.Equals(BackNavigationType.Single)) {
                            SetUrlToLoad(popUpArgs.TargetUrl);
                        } else {
                            PopUpArgs = popUpArgs;
                            popUpThread = new Thread(new ThreadStart(PopUpBrowser));
                            popUpThread.Start();
                        }
                    } else {
                        statusStripHandler.SetMessage(
                            StatusStripHandler.StatusMessageType.PersistentA,
                            Properties.Resources.MessageActionCanceled);
                    }
                });
                Browser.LifeSpanHandler = lifeSpanHandler;
            }
            Browser.AddressChanged += new EventHandler<AddressChangedEventArgs>((sender, e) => {
                if (IsAllowedUrl(e.Address) || !IsForbiddenUrl(e.Address)) {
                    BrowserAddress = e.Address;
                    AddressChanged?.Invoke(this, e);
                } else {
                    ChromiumWebBrowser browser = (ChromiumWebBrowser)sender;
                    browser.GetBrowser().StopLoad();
                    browser.Load(Url);
                    statusStripHandler.SetMessage(
                        StatusStripHandler.StatusMessageType.PersistentA,
                        Properties.Resources.MessageActionCanceled);
                }
            });
            Browser.ConsoleMessage += new EventHandler<ConsoleMessageEventArgs>((sender, e) => {
                ConsoleLine = e.Line;
                ConsoleSource = e.Source;
                ConsoleMessage = e.Message;
                if (settings.LogConsoleMessages) {
                    LogConsoleMessage(e.Line, e.Source, e.Message);
                }
                BrowserConsoleMessage?.Invoke(this, e);
            });
            Browser.Enter += new EventHandler((sender, e) => Enter?.Invoke(this, e));
            Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(async (sender, e) => {
                HeartBeatReset();
                if (loaded++ < 1) {
                    if (settings.AutoLogInAfterInitialLoad
                            && !string.IsNullOrWhiteSpace(UserName)
                            && !string.IsNullOrEmpty(Password)
                            && !string.IsNullOrWhiteSpace(Script)) {

                        loadingBeforeLogInTimer.Start();
                    }
                } else {
                    loaded = 1;
                }
                frameInitialLoadEnded = true;
                PingReset();
                FrameLoadEnd?.Invoke(this, e);
                ZoomLevel = await Browser.GetZoomLevelAsync();
                ZoomLevelChanged?.Invoke(this, e);
            });
            Browser.FrameLoadStart += new EventHandler<FrameLoadStartEventArgs>((sender, e) => {
                HeartBeatReset();
                PingReset();
                FrameLoadStart?.Invoke(this, e);
            });
            Browser.IsBrowserInitializedChanged += new EventHandler((sender, e) => {
                Browser.GetBrowser().GetHost().SetAudioMuted(IsAudioMuted);
                BrowserInitializedChanged?.Invoke(this, e);
            });
            Browser.LoadError += new EventHandler<LoadErrorEventArgs>((sender, e) => {
                ErrorCode = e.ErrorCode;
                ErrorText = e.ErrorText;
                FailedUrl = e.FailedUrl;
                if (settings.LogLoadErrors) {
                    LogLoadError(e.ErrorCode, e.ErrorText, e.FailedUrl);
                }
                LoadError?.Invoke(this, e);
            });
            Browser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(async (sender, e) => {
                if (!e.IsLoading) {
                    HeartBeatReset();
                }
                CanGoBack = e.CanGoBack;
                CanGoForward = e.CanGoForward;
                CanReload = e.CanReload;
                IsLoading = e.IsLoading;
                PingReset();
                LoadingStateChanged?.Invoke(this, e);
                ZoomLevel = await Browser.GetZoomLevelAsync();
                ZoomLevelChanged?.Invoke(this, e);
            });
            Browser.Resize += new EventHandler((sender, e) => HeartBeatReset());
            Browser.StatusMessage += new EventHandler<StatusMessageEventArgs>((sender, e) => {
                StatusMessage = e.Value;
                BrowserStatusMessage?.Invoke(this, e);
            });
            Browser.TitleChanged += new EventHandler<TitleChangedEventArgs>((sender, e) => {
                BrowserTitle = e.Title;
                TitleChanged?.Invoke(this, e);
            });
            Initialized?.Invoke(this, new FocusEventArgs(this, Ordinal - 1));
        }

        public async void LogInAsync(bool initialPage) {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Script)) {
                return;
            }
            if (Browser.Address.Equals(Url) || !initialPage && EqualsSecondLevelDomain(Browser.Address)) {
                await Task.Run(new Action(() => {
                    if (IsLoggedIn()) {
                        NoLogIn(Browser);
                        SetFinished();
                        Finished?.Invoke(this, new FinishedEventArgs(Properties.Resources.MessageActionFinished));
                        LoadUrlToLoad();
                    } else {
                        Started?.Invoke(this, new StartedEventArgs());
                        ExecuteLogIn();
                    }
                }));
            } else {
                Started?.Invoke(this, new StartedEventArgs());
                Browser.Load(Url);
                loadingBeforeLogInTimer.Start();
            }
        }

        public void Dispose() {
            if (webInfoForm != null && webInfoForm.Visible) {
                webInfoForm.SafeClose();
            }
            if (logInThread != null && logInThread.IsAlive) {
                logInThread.Abort();
            }
            loadingBeforeLogInTimer.Stop();
            loadingBeforeLogInTimer.Dispose();
            heartBeatTimer.Stop();
            heartBeatTimer.Dispose();
            pingTimer.Stop();
            pingTimer.Dispose();
            loadingBeforeLogInSemaphore.Dispose();
            heartBeatSemaphore.Dispose();
            persistWindowState.Dispose();
            if (Browser != null) {
                Browser.Dispose();
            }
        }

        private bool WaitForInitialLoad() {
            LoadUrlAsyncResponse loadUrlAsyncResponse = Browser.WaitForInitialLoadAsync().GetAwaiter().GetResult();
            return loadUrlAsyncResponse != null && loadUrlAsyncResponse.Success && loadUrlAsyncResponse.HttpStatusCode.Equals(200);
        }

        private void PopUpBrowser() {
            PopUpArgs.Location = new Point(PopUpLeft, PopUpTop);
            PopUpArgs.Size = new Size(PopUpWidth, PopUpHeight);
            PopUpBrowserForm popUpBrowserForm = new PopUpBrowserForm(this, ((MainForm)Parent.Form).Settings, PopUpArgs);
            popUpBrowserForm.UrlChanged += new EventHandler<UrlEventArgs>(OnUrlChanged);
            popUpBrowserForm.ShowDialog();
        }

        private void OnUrlChanged(object sender, UrlEventArgs e) {
            if (Browser.InvokeRequired) {
                Browser.Invoke(new EventHandler<UrlEventArgs>(OnUrlChanged), sender, e);
            } else {
                Browser.Load(e.Url);
                Focus?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetUrlToLoad(string url) {
            if (EqualsSecondLevelDomain(url)) {
                if (string.IsNullOrEmpty(UrlToLoad)) {
                    if (loaded > 0 && Browser != null) {
                        Browser.Load(url);
                    }
                } else {
                    if (loaded > 0 && Browser != null) {
                        Browser.Load(url);
                    } else {
                        UrlToLoad = url;
                    }
                }
            }
        }

        private void LoadUrlToLoad() {
            if (loaded > 0 && Browser != null && !string.IsNullOrEmpty(UrlToLoad) && EqualsSecondLevelDomain(UrlToLoad)) {
                Browser.Load(UrlToLoad);
                UrlToLoad = null;
            }
        }

        public void StopFinding(bool clearSelection) {
            Browser.StopFinding(clearSelection);
            findEventArgs = new FindEventArgs();
            Find?.Invoke(this, findEventArgs);
        }

        public virtual bool IsLoggedIn() {
            if (string.IsNullOrEmpty(Script) || !frameInitialLoadEnded) {
                return false;
            }
            try {
                if (Browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = Browser.EvaluateScriptAsync(Script).GetAwaiter().GetResult();
                    if (javascriptResponse != null && javascriptResponse.Success) {
                        return string.Compare(
                            GetDisplayName(),
                            GetDisplayName((string)javascriptResponse.Result),
                            string.IsNullOrEmpty(IetfLanguageTag)
                                ? CultureInfo.InvariantCulture
                                : CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag),
                            CompareOptions.IgnoreNonSpace).Equals(0);
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
            return false;
        }

        protected virtual void OnError() {
            Error?.Invoke(this, new ErrorEventArgs(Properties.Resources.MessageLogInFailed));
        }

        protected virtual void OnError(string errorMessage) {
            StringBuilder message = new StringBuilder()
                .Append(Properties.Resources.MessageLogInFailed)
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .Append(errorMessage);
            Error?.Invoke(this, new ErrorEventArgs(message.ToString()));
        }

        protected virtual void OnFinished() {
            loadingBeforeLogInTimer.Stop();
            currentItem = 0;
            SetFinished();
            Finished?.Invoke(this, new FinishedEventArgs(Properties.Resources.MessageActionFinished));
            LoadUrlToLoad();
        }

        protected virtual void OnProgress(string message) {
            Progress?.Invoke(this, new ProgressEventArgs(currentItem++, itemsTotal, message));
        }

        protected virtual void OnStarted(int itemsTotal) {
            this.itemsTotal = itemsTotal;
            currentItem = 0;
            Started?.Invoke(this, new StartedEventArgs(itemsTotal));
        }

        public Tip[] GetTips() => Browser != null && IsLoggedIn() ? GetTips(Browser) : null;

        protected virtual Tip[] GetTips(ChromiumWebBrowser browser) => null;

        public virtual void HeartBeat(ChromiumWebBrowser browser) { }

        protected virtual void LogIn(ChromiumWebBrowser browser) {
            Error?.Invoke(this, new ErrorEventArgs(Properties.Resources.MessageLogInNotImplementedError));
        }

        protected virtual void NoLogIn(ChromiumWebBrowser browser) { }

        protected bool ClickElement(ChromiumWebBrowser browser, string script) {
            for (int i = 0; i < Constants.JScriptWaitCycles; i++) {
                if (i > 0) {
                    Thread.Sleep(50);
                }
                Point point = GetElementCenterAsync(browser, script).GetAwaiter().GetResult();
                if (point.X > 0 && point.Y > 0) {
                    browser.GetBrowser().GetHost()
                        .SendMouseClickEvent(new MouseEvent(point.X, point.Y, CefEventFlags.None), MouseButtonType.Left, false, 1);
                    browser.GetBrowser().GetHost()
                        .SendMouseClickEvent(new MouseEvent(point.X, point.Y, CefEventFlags.None), MouseButtonType.Left, true, 1);
                    return true;
                }
            }
            string browserAddress;
            if (BrowserAddress == null) {
                browserAddress = Constants.ErrorLogNull;
            } else if (BrowserAddress.Equals(string.Empty)) {
                browserAddress = Constants.ErrorLogEmptyString;
            } else {
                browserAddress = BrowserAddress;
            }
            string errorMessage = new StringBuilder()
                .Append(Properties.Resources.MessageClickingError)
                .Append(Constants.VerticalTab)
                .Append(browserAddress)
                .Append(Constants.VerticalTab)
                .Append(script)
                .ToString();
            Debug.WriteLine(errorMessage);
            ErrorLog.WriteLine(errorMessage);
            return false;
        }

        protected bool ElementExists(ChromiumWebBrowser browser, string script, bool wait) {
            return ElementExists(browser, new string[] { script }, wait);
        }

        protected bool ElementExists(ChromiumWebBrowser browser, string[] scripts, bool wait) {
            for (int i = 0; i < (wait ? Constants.JScriptWaitCycles : 1); i++) {
                if (i > 0) {
                    Thread.Sleep(50);
                }
                foreach (string script in scripts) {
                    if (!frameInitialLoadEnded) {
                        Thread.Sleep(50);
                        continue;
                    }
                    if (browser.CanExecuteJavascriptInMainFrame) {
                        JavascriptResponse javascriptResponse = browser
                            .EvaluateScriptAsync(string.Format(Constants.JSIsNotEqualToNullFormat, script.TrimEnd(Constants.Semicolon)))
                            .GetAwaiter()
                            .GetResult();
                        if (javascriptResponse.Success && (bool)javascriptResponse.Result) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected bool ElementExistsAndVisible(ChromiumWebBrowser browser, string script, bool wait) {
            return ElementExistsAndVisible(browser, new string[] { script }, wait);
        }

        protected bool ElementExistsAndVisible(ChromiumWebBrowser browser, string[] scripts, bool wait) {
            for (int i = 0; i < (wait ? Constants.JScriptWaitCycles : 1); i++) {
                if (i > 0) {
                    Thread.Sleep(50);
                }
                foreach (string script in scripts) {
                    Point point = GetElementCenterAsync(browser, script).GetAwaiter().GetResult();
                    if (point.X > 0 && point.Y > 0) {
                        return true;
                    }
                }
            }
            return false;
        }

        protected async Task<Point> GetElementCenterAsync(ChromiumWebBrowser browser, string script) {
            if (!frameInitialLoadEnded) {
                return Point.Empty;
            }
            try {
                if (browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = await browser
                        .EvaluateScriptAsync(script.TrimEnd(Constants.Semicolon) + Constants.JSGetBoundingClientRect);
                    if (javascriptResponse.Success) {
                        IDictionary<string, object> dictionary = (IDictionary<string, object>)javascriptResponse.Result;
                        return new Point(
                            (Convert.ToInt32(dictionary[Constants.JSPropertyLeft])
                                + Convert.ToInt32(dictionary[Constants.JSPropertyRight])) / 2,
                            (Convert.ToInt32(dictionary[Constants.JSPropertyTop])
                                + Convert.ToInt32(dictionary[Constants.JSPropertyBottom])) / 2);
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
            return Point.Empty;
        }

        protected string GetValueById(
                string elementId,
                string rootElement = Constants.DOMRootElementName) {

            return GetValue(string.Format(Constants.JSGetElementByIdFormat, rootElement, elementId));
        }

        protected string GetValueBy(
                ElementAttribute elementAttribute,
                string attributeValue,
                int index = 0,
                string rootElement = Constants.DOMRootElementName) {

            switch (elementAttribute) {
                case ElementAttribute.ClassName:
                    return GetValue(string.Format(
                        Constants.JSGetElementsByClassNameFormat,
                        rootElement,
                        attributeValue,
                        index));
                case ElementAttribute.Name:
                    return GetValue(string.Format(
                        Constants.JSGetElementsByNameFormat,
                        rootElement,
                        attributeValue,
                        index));
                case ElementAttribute.TagName:
                    return GetValue(string.Format(
                        Constants.JSGetElementsByTagNameFormat,
                        rootElement,
                        attributeValue,
                        index));
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetValue(string script) {
            if (!frameInitialLoadEnded) {
                return null;
            }
            try {
                if (Browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = Browser.EvaluateScriptAsync(script).GetAwaiter().GetResult();
                    if (javascriptResponse != null && javascriptResponse.Success) {
                        return (string)javascriptResponse.Result;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return null;
        }

        public void SendKey(Keys key) {
            if (Browser != null) {
                KeyEvent keyEvent = new KeyEvent();
                keyEvent.WindowsKeyCode = (int)key;
                keyEvent.FocusOnEditableField = false;
                keyEvent.IsSystemKey = false;
                keyEvent.Type = KeyEventType.KeyDown;
                Browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
                Thread.Sleep(20);
                keyEvent.WindowsKeyCode = (int)key;
                keyEvent.FocusOnEditableField = false;
                keyEvent.IsSystemKey = false;
                keyEvent.Type = KeyEventType.KeyUp;
                Browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
                Thread.Sleep(20);
            }
        }

        private void LogForeignUrl(string url) {
            try {
                string filePath = Path.Combine(Application.LocalUserAppDataPath, Constants.ForeignUrlsLogFileName);
                using (StreamWriter streamWriter = File.AppendText(filePath)) {
                    StringBuilder stringBuilder = new StringBuilder()
                        .Append(DateTime.Now.ToString(Constants.ErrorLogTimeFormat))
                        .Append(Constants.VerticalTab)
                        .Append(Title)
                        .Append(Constants.VerticalTab)
                        .Append(url);
                    streamWriter.WriteLine(stringBuilder.ToString());
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void LogConsoleMessage(int line, string source, string message) {
            if (!string.IsNullOrEmpty(source)) {
                try {
                    string filePath = Path.Combine(Application.LocalUserAppDataPath, Constants.ConsoleMessageLogFileName);
                    using (StreamWriter streamWriter = File.AppendText(filePath)) {
                        StringBuilder stringBuilder = new StringBuilder()
                            .Append(DateTime.Now.ToString(Constants.ErrorLogTimeFormat))
                            .Append(Constants.VerticalTab)
                            .Append(Title)
                            .Append(Constants.VerticalTab);
                        if (string.IsNullOrWhiteSpace(message)) {
                            stringBuilder.AppendFormat(Constants.BrowserConsoleMessageFormat4, line, source.Trim());
                        } else {
                            stringBuilder.AppendFormat(Constants.BrowserConsoleMessageFormat3, line, source.Trim(), message.Trim());
                        }
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void LogLoadError(CefErrorCode errorCode, string errorText, string failedUrl) {
            if (!errorCode.Equals(CefErrorCode.None) && !string.IsNullOrEmpty(errorText) && !string.IsNullOrEmpty(failedUrl)) {
                try {
                    string filePath = Path.Combine(Application.LocalUserAppDataPath, Constants.LoadErrorLogFileName);
                    using (StreamWriter streamWriter = File.AppendText(filePath)) {
                        StringBuilder stringBuilder = new StringBuilder()
                            .Append(DateTime.Now.ToString(Constants.ErrorLogTimeFormat))
                            .Append(Constants.VerticalTab)
                            .Append(Title)
                            .Append(Constants.VerticalTab)
                            .AppendFormat(Constants.BrowserLoadErrorMessageFormat2, errorText.Trim(), failedUrl.Trim());
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public bool IsSubdomainOf(string subdomainUrl, string baseUrl) {
            if (string.IsNullOrEmpty(subdomainUrl) || string.IsNullOrEmpty(baseUrl)) {
                return false;
            }
            try {
                if (!urlScheme.IsMatch(subdomainUrl)) {
                    subdomainUrl = new StringBuilder()
                        .Append(Constants.SchemeHttps)
                        .Append(Constants.Colon)
                        .Append(Constants.Slash)
                        .Append(Constants.Slash)
                        .Append(subdomainUrl.TrimStart(Constants.Colon, Constants.Slash))
                        .ToString();
                }
                if (!urlScheme.IsMatch(baseUrl)) {
                    baseUrl = new StringBuilder()
                        .Append(Constants.SchemeHttps)
                        .Append(Constants.Colon)
                        .Append(Constants.Slash)
                        .Append(Constants.Slash)
                        .Append(baseUrl.TrimStart(Constants.Colon, Constants.Slash))
                        .ToString();
                }
                Uri subdomainUri = new Uri(subdomainUrl);
                Uri baseUri = new Uri(baseUrl);
                if (subdomainUri.HostNameType.Equals(UriHostNameType.Dns)
                        && baseUri.HostNameType.Equals(UriHostNameType.Dns)) {

                    return subdomainUri.Host.EndsWith(baseUri.Host, StringComparison.Ordinal);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        public bool EqualsSecondLevelDomain(string url, bool includeTld = true) {
            if (string.IsNullOrEmpty(url)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url);
                Uri uri2 = new Uri(Url);
                if (uri1.HostNameType.Equals(UriHostNameType.Dns)
                        && uri2.HostNameType.Equals(UriHostNameType.Dns)) {

                    bool eq = secondLevelDomain.Replace(uri2.Host, Constants.ReplaceSecond)
                        .Equals(secondLevelDomain.Replace(uri1.Host, Constants.ReplaceSecond), StringComparison.Ordinal);
                    return includeTld
                        ? eq && secondLevelDomain.Replace(uri2.Host, Constants.ReplaceThird)
                            .Equals(secondLevelDomain.Replace(uri1.Host, Constants.ReplaceThird), StringComparison.Ordinal)
                        : eq;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        public static bool IsAllowedUrl(string url) {
            foreach (string item in Constants.AllowedUrlStarts.Split(Constants.Comma)) {
                if (url.StartsWith(item, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }
            return false;
        }

        private static bool IsForbiddenUrl(string url) {
            try {
                Uri uri = new Uri(url);
                if (uri.HostNameType.Equals(UriHostNameType.Dns)) {
                    if (string.IsNullOrWhiteSpace(uri.Host)) {
                        return true;
                    }
                    foreach (string item in Constants.ForbiddenHostContents.Split(Constants.Comma)) {
                        if (uri.Host.Contains(item.ToLowerInvariant().Trim())) {
                            return true;
                        }
                        if (uri.AbsolutePath.Contains(item.ToLowerInvariant().Trim())) {
                            return true;
                        }
                    }
                    return false;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return true;
        }

        protected static string GetSelector(string script) {
            StringBuilder stringBuilder = new StringBuilder();
            if (script.Contains(Constants.JSGetElementsByClassNameFuncName)) {
                stringBuilder.Append(Constants.Period);
            } else if (script.Contains(Constants.JSGetElementByIdFuncName)) {
                stringBuilder.Append(Constants.NumberSign);
            } else {
                return null;
            }
            stringBuilder.Append(Regex.Replace(script, Constants.GetSelectorPattern, Constants.ReplaceFirst));
            return stringBuilder.ToString();
        }

        protected static void SendKey(ChromiumWebBrowser browser, Keys key) {
            KeyEvent keyEvent = new KeyEvent();
            keyEvent.WindowsKeyCode = (int)key;
            keyEvent.FocusOnEditableField = true;
            keyEvent.IsSystemKey = false;
            keyEvent.Type = KeyEventType.KeyDown;
            browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
            Thread.Sleep(20);
            keyEvent.WindowsKeyCode = (int)key;
            keyEvent.FocusOnEditableField = true;
            keyEvent.IsSystemKey = false;
            keyEvent.Type = KeyEventType.KeyUp;
            browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
            Thread.Sleep(30);
        }

        protected static void SendString(ChromiumWebBrowser browser, string str) {
            foreach (char c in str.ToCharArray()) {
                KeyEvent keyEvent = new KeyEvent();
                keyEvent.WindowsKeyCode = c;
                keyEvent.FocusOnEditableField = true;
                keyEvent.IsSystemKey = false;
                keyEvent.Type = KeyEventType.Char;
                browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
                Thread.Sleep(30);
            }
        }

        protected static void Wait(ChromiumWebBrowser browser) {
            for (int i = 0; i < Constants.JScriptWaitCycles; i++) {
                if (i > 0) {
                    Thread.Sleep(50);
                }
                if (!browser.IsLoading && browser.CanExecuteJavascriptInMainFrame) {
                    return;
                }
            }
        }

        protected bool Reload(ChromiumWebBrowser browser) {
            if (CanReload) {
                browser.GetBrowser().Reload();
                return true;
            }
            return false;
        }

        protected bool ReloadIfRequired(ChromiumWebBrowser browser) {
            if (CanReload && finished > 0) {
                browser.GetBrowser().Reload();
                return true;
            }
            return false;
        }

        protected void LoadInitialPage(ChromiumWebBrowser browser) {
            browser.GetBrowser().StopLoad();
            browser.Load(Url);
        }

        protected static void Sleep(int millisecondsTimeout) => Thread.Sleep(millisecondsTimeout);

        protected static void Sleep(TimeSpan timeout) => Thread.Sleep(timeout);

        public enum ElementAttribute {
            ClassName,
            Name,
            TagName
        }

        public enum BackNavigationType {
            None,
            Fragment,
            Single
        }
    }
}
