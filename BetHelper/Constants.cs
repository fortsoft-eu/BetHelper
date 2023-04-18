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
 * Version 1.1.3.0
 */

namespace BetHelper {

    /// <summary>
    /// Constants used in many places in the application.
    /// </summary>
    public static class Constants {

        /// <summary>
        /// This value is chosen considering the size of the calculator in the
        /// right panel. Dimensionless value.
        /// </summary>
        public const double CalculatorTabsDefaultZoomLevel = -1.5;

        /// <summary>
        /// This value is chosen considering the size of the calculator in a
        /// separate window. Dimensionless value.
        /// </summary>
        public const double CalculatorWindowDefaultZoomLevel = -0.7;

        /// <summary>
        /// Above this value there is no more resizing. Empirically verified.
        /// Dimensionless value.
        /// </summary>
        public const double MaximumZoomLevel = 9.0;

        /// <summary>
        /// Below this value there is no more resizing. Empirically verified.
        /// Dimensionless value.
        /// </summary>
        public const double MinimumZoomLevel = -7.5;

        /// <summary>
        /// The time interval in milliseconds after which the application will be
        /// forcibly terminated after the main application window is closed.
        /// </summary>
        public const int AfterCloseTimeOut = 1800;

        /// <summary>
        /// Minimum empirically determined browser width in pixels required for
        /// reliable automatic login. If a lower value is detected, the main
        /// window of the application will be enlarged during the automatic login
        /// request.
        /// </summary>
        public const int BrowserMinHeight = 520;

        /// <summary>
        /// Minimum empirically determined browser height in pixels required for
        /// reliable automatic login. If a lower value is detected, the main
        /// window of the application will be enlarged during the automatic login
        /// request.
        /// </summary>
        public const int BrowserMinWidth = 920;

        /// <summary>
        /// The time interval in minutes after which the cache size of the
        /// internal Chromium web browser will be refreshed.
        /// </summary>
        public const int CacheSizeRefreshInterval = 15;

        /// <summary>
        /// Maximum number of tabs with bet calculators.
        /// </summary>
        public const int CalculatorTabsMaxCounts = 10;

        /// <summary>
        /// The delay in milliseconds after which the focus will be given to the
        /// input element of the form after the form is loaded.
        /// </summary>
        public const int FormLoadToFocusDelay = 50;

        /// <summary>
        /// Maximum number of tabs with a match on the ticket form.
        /// </summary>
        public const int GameTabsMaxCounts = 100;

        /// <summary>
        /// Number of heart beats cycles.
        /// </summary>
        public const int HeartBeatCycles = 3;

        /// <summary>
        /// The time interval in milliseconds between each heart beat.
        /// </summary>
        public const int HeartBeatInterval = 500;

        /// <summary>
        /// Initial heart beat time interval in milliseconds.
        /// </summary>
        public const int InitialHeartBeatInterval = 10;

        /// <summary>
        /// Initial time interval in milliseconds to get balances and tips
        /// displayed at right pane.
        /// </summary>
        public const int InitialRightPaneInterval = 10000;

        /// <summary>
        /// The shortest possible time interval in milliseconds of the next check
        /// of the client's IP address against the server.
        /// </summary>
        public const int IPAddressCheckInterval = 250;

        /// <summary>
        /// In the case of a request to wait for JavaScript execution, the
        /// specified number of JavaScript execution attempts will be made.
        /// </summary>
        public const int JScriptWaitCycles = 500;

        /// <summary>
        /// The time interval in milliseconds after which the automatic login
        /// procedure will be started after the last FrameLoadEnd.
        /// </summary>
        public const int LoadedToLogInInterval = 6000;

        /// <summary>
        /// Maximum number of previous searches in the combobox.
        /// </summary>
        public const int MaximumSearches = 30;

        /// <summary>
        /// The maximum number of open URLs in the combobox.
        /// </summary>
        public const int MaximumTypedUrls = 30;

        /// <summary>
        /// The time interval in milliseconds after which an attempt to keep the
        /// user logged in on the next tab will be performed.
        /// </summary>
        public const int PingNextTabInterval = 60000;

        /// <summary>
        /// The time interval in milliseconds after which an attempt to keep the
        /// user logged in on the same tab will be performed.
        /// </summary>
        public const int PingTabExpInterval = 300000;

        /// <summary>
        /// Maximum number of items in comboboxes in PreferencesForm.
        /// </summary>
        public const int PrefsMaxDropDownItems = 30;

        /// <summary>
        /// The time interval in milliseconds for smooth ProgressBar shift in the
        /// ProgressBarForm.
        /// </summary>
        public const int ProgressBarFormInterval = 30;

        /// <summary>
        /// Default right panel width in pixels.
        /// </summary>
        public const int RightPaneDefaultWidth = 568;

        /// <summary>
        /// The time interval in milliseconds to get balances and tips displayed
        /// at right pane.
        /// </summary>
        public const int RightPaneInterval = 30000;

        /// <summary>
        /// The time delay in milliseconds after which the window area will be
        /// captured.
        /// </summary>
        public const int ScreenFormCaptureDelay = 500;

        /// <summary>
        /// Default column index for sorting services in the listing.
        /// </summary>
        public const int ServicesDefaultSortColumn = 3;

        /// <summary>
        /// The maximum width in pixels of the SplitContainer in the main
        /// application window for calculations.
        /// </summary>
        public const int SplitContainerMaxWidth = 1920;

        /// <summary>
        /// Maximum internal width in pixels for calculations of the ProgressBar
        /// in the StatusStrip regardless of displayed size.
        /// </summary>
        public const int StripProgressBarInternalMax = 360;

        /// <summary>
        /// The time interval in milliseconds for smooth ProgressBar shift in the
        /// StatusStrip.
        /// </summary>
        public const int StripProgressBarInterval = 30;

        /// <summary>
        /// Width limit in pixels for displaying the ProgressBar in the
        /// StatusStrip.
        /// </summary>
        public const int StripProgressBarVLimit = 1200;

        /// <summary>
        /// Ratio for displaying the ProgressBar in the StatusStrip according to
        /// its width.
        /// </summary>
        public const int StripProgressBarWRatio = 18;

        /// <summary>
        /// Width limit in pixels for displaying the browser cache size label in
        /// the StatusStrip.
        /// </summary>
        public const int StripStatusLblCacheVLimit = 1000;

        /// <summary>
        /// Width limit in pixels for displaying the reduced browser cache size
        /// label in the StatusStrip.
        /// </summary>
        public const int StripStatusLblCacheVLimitReduced = 600;

        /// <summary>
        /// Width limit in pixels for displaying the search results label in the
        /// StatusStrip.
        /// </summary>
        public const int StripStatusLblSearchResVLimit = 800;

        /// <summary>
        /// Width limit in pixels for displaying the URL label ProgressBar in the
        /// StatusStrip.
        /// </summary>
        public const int StripStatusLblUrlVLimit = 600;

        /// <summary>
        /// Default column index for sorting tips in the listing.
        /// </summary>
        public const int TipsDefaultSortColumn = 3;

        /// <summary>
        /// The time interval in seconds after which the monitors will be turned
        /// off after entering the command to turn off the monitors.
        /// </summary>
        public const int TurnOffTheMonitorsInterval = 5;

        /// <summary>
        /// Tabs in the log files will be replaced with the following number of
        /// spaces for displaying in the log viewer window and when printing.
        /// </summary>
        public const int VerticalTabNumberOfSpaces = 3;

        /// <summary>
        /// Windows API constants.
        /// </summary>
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MONITORPOWER = 0xF170;
        public const int SC_SCREENSAVE = 0xF140;
        public const int SC_TASKLIST = 0xF130;
        public const int WM_CLEAR = 0x0303;
        public const int WM_COPY = 0x0301;
        public const int WM_CUT = 0x0300;
        public const int WM_HSCROLL = 0x114;
        public const int WM_PASTE = 0x0302;
        public const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// Characters used in many places in the application code.
        /// </summary>
        public const char Ampersand = '&';
        public const char Asterisk = '*';
        public const char CarriageReturn = '\r';
        public const char ClosingBracket = ']';
        public const char Colon = ':';
        public const char Comma = ',';
        public const char Ellipsis = '…';
        public const char EmDash = '—';
        public const char EnDash = '–';
        public const char EqualSign = '=';
        public const char Hyphen = '-';
        public const char LineFeed = '\n';
        public const char MinusSign = '−';
        public const char NumberSign = '#';
        public const char OpeningBracket = '[';
        public const char Percent = '%';
        public const char Period = '.';
        public const char QuestionMark = '?';
        public const char QuotationMark = '"';
        public const char Semicolon = ';';
        public const char Slash = '/';
        public const char Space = ' ';
        public const char Underscore = '_';
        public const char UpperCaseT = 'T';
        public const char UpperCaseZ = 'Z';
        public const char VerticalBar = '|';
        public const char VerticalTab = '\t';
        public const char Zero = '0';

        /// <summary>
        /// Strings used in many places in the application code.
        /// </summary>
        public const string AllowedAddrFileName = "AllowedAddr.dat";
        public const string AllowedUrlStarts = "devtools:,about:";
        public const string BlankPageUri = "about:blank";
        public const string BookmarksFileName = "Bookmarks.dat";
        public const string BrowserCacheMngrCacheSubDirName = "Cache";
        public const string BrowserCacheMngrLocalPrefsFileName = "LocalPrefs.json";
        public const string BrowserCacheMngrSetClearFileName = "ClearBrowserCache.tmp";
        public const string BrowserCacheMngrUserDataSubDirName = "UserData";
        public const string BrowserConsoleMessageFormat1 = "Line: {0}, Source: {1}, Message: {2}";
        public const string BrowserConsoleMessageFormat2 = "Line: {0}, Source: {1}";
        public const string BrowserConsoleMessageFormat3 = "Line: {0}\tSource: {1}\tMessage: {2}";
        public const string BrowserConsoleMessageFormat4 = "Line: {0}\tSource: {1}";
        public const string BrowserLoadErrorMessageFormat1 = "{0}: {1}";
        public const string BrowserLoadErrorMessageFormat2 = "{0}\t{1}";
        public const string BrowserSearchFileName = "BrowserSearch.dat";
        public const string ButtonRemoveName = "buttonRemove";
        public const string CacheRelativePath = "Cache";
        public const string CalcFileName = "calc.exe";
        public const string CalculatorDirectoryName = "Calculator";
        public const string CalculatorFileName01 = "app-base.css";
        public const string CalculatorFileName02 = "application.js";
        public const string CalculatorFileName03 = "calculator.html";
        public const string CalculatorFileName04 = "fa-brands-400.eot";
        public const string CalculatorFileName05 = "fa-brands-400.svg";
        public const string CalculatorFileName06 = "fa-brands-400.ttf";
        public const string CalculatorFileName07 = "fa-brands-400.woff";
        public const string CalculatorFileName08 = "fa-brands-400.woff2";
        public const string CalculatorFileName09 = "fa-regular-400.eot";
        public const string CalculatorFileName10 = "fa-regular-400.svg";
        public const string CalculatorFileName11 = "fa-regular-400.ttf";
        public const string CalculatorFileName12 = "fa-regular-400.woff";
        public const string CalculatorFileName13 = "fa-regular-400.woff2";
        public const string CalculatorFileName14 = "fa-solid-900.eot";
        public const string CalculatorFileName15 = "fa-solid-900.svg";
        public const string CalculatorFileName16 = "fa-solid-900.ttf";
        public const string CalculatorFileName17 = "fa-solid-900.woff";
        public const string CalculatorFileName18 = "fa-solid-900.woff2";
        public const string CalculatorZipFileName = "calculator.zip";
        public const string CefCLiArgEmableCdmKey = "enable-widevine-cdm";
        public const string CefCLiArgEmableCdmVal = "1";
        public const string CefCLiArgMuteAudioKey = "mute-audio";
        public const string CefCLiArgMuteAudioVal = "true";
        public const string CefDebugLogFileName = "CefDebug.log";
        public const string CharMapFileName = "charmap.exe";
        public const string ColumnHeaderBookmaker = "Bookmaker";
        public const string ColumnHeaderDate = "Date";
        public const string ColumnHeaderExpiration = "Expiration";
        public const string ColumnHeaderLeague = "League";
        public const string ColumnHeaderMatch = "Match";
        public const string ColumnHeaderName = "Name";
        public const string ColumnHeaderOdd = "Odd";
        public const string ColumnHeaderOpportunity = "Opportunity";
        public const string ColumnHeaderPeriod = "Period";
        public const string ColumnHeaderPrice = "Price";
        public const string ColumnHeaderPurchased = "Purchased";
        public const string ColumnHeaderService = "Service";
        public const string ColumnHeaderSport = "Sport";
        public const string ColumnHeaderStatus = "Status";
        public const string ColumnHeaderSubscribed = "Subscribed";
        public const string ColumnHeaderTime = "Time";
        public const string ColumnHeaderTrustDegree = "Trust Degree";
        public const string ColumnHeaderValidUntil = "Valid Until";
        public const string CommandLineSwitchUC = "-c";
        public const string CommandLineSwitchUD = "-d";
        public const string CommandLineSwitchUE = "-e";
        public const string CommandLineSwitchUL = "-l";
        public const string CommandLineSwitchWC = "/c";
        public const string CommandLineSwitchWD = "/d";
        public const string CommandLineSwitchWE = "/e";
        public const string CommandLineSwitchWL = "/l";
        public const string ConfigAllowedHosts = "ALLOWED";
        public const string ConfigBackNavigation = "BACK_NAV";
        public const string ConfigChat = "CHAT";
        public const string ConfigDisplayName = "DISPLAY";
        public const string ConfigFields = "FIELDS";
        public const string ConfigHandlePopUps = "POPUPS";
        public const string ConfigIetfLanguageTag = "LANGUAGE";
        public const string ConfigMute = "MUTE";
        public const string ConfigPassword = "PASSWORD";
        public const string ConfigPattern = "PATTERN";
        public const string ConfigPopUpHeight = "POPUP_H";
        public const string ConfigPopUpLeft = "POPUP_L";
        public const string ConfigPopUpTop = "POPUP_T";
        public const string ConfigPopUpWidth = "POPUP_W";
        public const string ConfigScript = "SCRIPT";
        public const string ConfigSearchFileName = "ConfigSearch.dat";
        public const string ConfigService = "SERVICE";
        public const string ConfigTabNavigation = "TAB_NAV";
        public const string ConfigTitle = "TITLE";
        public const string ConfigUrl = "URL";
        public const string ConfigUrlLive = "URL_LIVE";
        public const string ConfigUrlTips = "URL_TIPS";
        public const string ConfigUrlToLoad = "URL_NEXT";
        public const string ConfigUserName = "USERNAME";
        public const string ConfigYes = "YES";
        public const string ConsoleMessageLogFileName = "ConsoleMessages.log";
        public const string DOMRootElementName = "document";
        public const string DumpFileNameTimeFormat = "yyyy-MM-dd_HHmmss_fff";
        public const string ErrorLogEmptyString = "[Empty String]";
        public const string ErrorLogErrorMessage = "ERROR MESSAGE";
        public const string ErrorLogFileName = "Error.log";
        public const string ErrorLogNull = "[null]";
        public const string ErrorLogTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public const string ErrorLogWhiteSpace = "[White Space]";
        public const string ExplorerFileName = "explorer.exe";
        public const string ExplorerSwitchE = "/e";
        public const string ExtBrowsChromeExecRelPath = "Google\\Chrome\\Application\\chrome.exe";
        public const string ExtBrowsChromeLnkFileName = "Google Chrome.lnk";
        public const string ExtBrowsFirefoxExecRelPath = "Mozilla Firefox\\Firefox.exe";
        public const string ExtBrowsFirefoxLnkFileName = "Firefox.lnk";
        public const string ExtensionBmp = ".bmp";
        public const string ExtensionExe = ".exe";
        public const string ExtensionFilterBmp = "Windows Bitmap BMP (*.bmp)|*.bmp";
        public const string ExtensionFilterGif = "CompuServe GIF 89a (*.gif)|*.gif";
        public const string ExtensionFilterJpg = "JPEG File Interchange Format (*.jpg)|*.jpg";
        public const string ExtensionFilterPng = "Portable Network Graphics PNG (*.png)|*.png";
        public const string ExtensionFilterTif = "Tagged Image File Format TIFF (*.tif)|*.tif";
        public const string ExtensionFilterWebP = "Google WebP (*.webp)|*.webp";
        public const string ExtensionGif = ".gif";
        public const string ExtensionJpg = ".jpg";
        public const string ExtensionPng = ".png";
        public const string ExtensionTif = ".tif";
        public const string ExtensionTxt = ".txt";
        public const string ExtensionWebP = ".webp";
        public const string FieldBalance = "Balance";
        public const string FieldDisplayName = "DisplayName";
        public const string FieldUserName = "UserName";
        public const string ForbiddenHostContents = "casino,poker,vegas";
        public const string ForbiddenPathContents = "priloha";
        public const string ForeignUrlsLogFileName = "ForeignUrls.log";
        public const string GetSelectorPattern = "^.*'(.*)'.*$";
        public const string IetfLanguageTagEnUs = "en-US";
        public const string IPv4Pattern = "^\\d{1,4}\\.\\d{1,4}\\.\\d{1,4}\\.\\d{1,4}$";
        public const string IPv6SimplifiedPattern = "^([a-f0-9]{1,4}::?){1,7}[a-f0-9]{1,4}$";
        public const string JSBalancePattern = "[^-\\.,\\d]";
        public const string JSGetBoundingClientRect = ".getBoundingClientRect();";
        public const string JSGetElementByIdFormat = "{0}.getElementById('{1}').value";
        public const string JSGetElementByIdFuncName = "getElementById";
        public const string JSGetElementsByClassNameFormat = "{0}.getElementsByClassName('{1}')[{2}].value";
        public const string JSGetElementsByClassNameFuncName = "getElementsByClassName";
        public const string JSGetElementsByNameFormat = "{0}.getElementsByName('{1}')[{2}].value";
        public const string JSGetElementsByTagNameFormat = "{0}.getElementsByTagName('{1}')[{2}].value";
        public const string JSIndexAndInnerTextPattern = "\\[\\d+\\]\\.innerText";
        public const string JSIsNotEqualToNullFormat = "({0}) !== null;";
        public const string JSPropertyBottom = "bottom";
        public const string JSPropertyLeft = "left";
        public const string JSPropertyRight = "right";
        public const string JSPropertyTop = "top";
        public const string LibWebPX64FileName = "libwebp_x64.dll";
        public const string LibWebPX86FileName = "libwebp_x86.dll";
        public const string LoadErrorLogFileName = "LoadErrors.log";
        public const string LogFileExtension = "*.log";
        public const string LogSearchFileName = "LogSearch.dat";
        public const string MonospaceFontName = "Consolas";
        public const string Muted = "Muted";
        public const string NotepadFileName = "notepad.exe";
        public const string NumberFormatSystem = "[system]";
        public const string NumericUpDownEdit = "upDownEdit";
        public const string OddPattern = "[^\\.,\\d]";
        public const string OneDecimalDigitFormat = "f1";
        public const string PopUpFrameHandlerLogFileName = "PopUpFrameHandler.log";
        public const string PopUpFrameHandlerLogFormat = "frameIdentifier{0} = {1}";
        public const string ExternalEditorApplicationName = "Notepad++";
        public const string ExternalEditorFileName = "notepad++.exe";
        public const string PrintOutputInput = "Input";
        public const string PrintOutputOutput = "Output";
        public const string RemoteApiScriptName = "api.php";
        public const string RemoteApplicationConfig = "ApplicationConfig";
        public const string RemoteClientRemoteAddress = "ClientRemoteAddress";
        public const string RemoteProductLatestVersion = "ProductLatestVersion";
        public const string RemoteVariableNameGet = "get";
        public const string RemoteVariableNameSet = "set";
        public const string ReplaceFirst = "$1";
        public const string ReplaceIndex = "${0}";
        public const string ReplaceSecond = "$2";
        public const string ReplaceThird = "$3";
        public const string RightPaneDataFileName = "RightPane.dat";
        public const string SchemeHttp = "http";
        public const string SchemeHttps = "https";
        public const string SchemePresenceTestPattern = "^[a-z][a-z0-9-\\.]*[a-z0-9]://.+$";
        public const string SecondLevelDomainPattern = "^([\\w-]+\\.)*(\\w+)\\.(\\w+)$";
        public const string SendKeysAlt = "{%}";
        public const string SendKeysCtrlZ = "^{z}";
        public const string SendKeysDelete = "{DELETE}";
        public const string SendKeysDown = "{DOWN}";
        public const string SendKeysEnd = "{END}";
        public const string SendKeysHome = "{HOME}";
        public const string SendKeysPgDn = "{PGDN}";
        public const string SendKeysPgUp = "{PGUP}";
        public const string SendKeysTab = "{TAB}";
        public const string SendKeysUp = "{UP}";
        public const string Settings = "Settings";
        public const string ShortcutAltF10 = "\tAlt+F10";
        public const string ShortcutAltF11 = "\tAlt+F11";
        public const string ShortcutAltF12 = "\tAlt+F12";
        public const string ShortcutAltF8 = "\tAlt+F8";
        public const string ShortcutAltF9 = "\tAlt+F9";
        public const string ShortcutAltHome = "\tAlt+Home";
        public const string ShortcutAltL = "\tAlt+L";
        public const string ShortcutAltLeft = "\tAlt+←";
        public const string ShortcutAltRight = "\tAlt+→";
        public const string ShortcutAltShiftCtrlP = "\tAlt+Shift+Ctrl+P";
        public const string ShortcutCtrl0 = "\tCtrl+0";
        public const string ShortcutCtrlA = "\tCtrl+A";
        public const string ShortcutCtrlC = "\tCtrl+C";
        public const string ShortcutCtrlD = "\tCtrl+D";
        public const string ShortcutCtrlE = "\tCtrl+E";
        public const string ShortcutCtrlF = "\tCtrl+F";
        public const string ShortcutCtrlF5 = "\tCtrl+F5";
        public const string ShortcutCtrlG = "\tCtrl+G";
        public const string ShortcutCtrlI = "\tCtrl+I";
        public const string ShortcutCtrlM = "\tCtrl+M";
        public const string ShortcutCtrlMinus = "\tCtrl+−";
        public const string ShortcutCtrlO = "\tCtrl+O";
        public const string ShortcutCtrlP = "\tCtrl+P";
        public const string ShortcutCtrlPlus = "\tCtrl++";
        public const string ShortcutCtrlT = "\tCtrl+T";
        public const string ShortcutCtrlU = "\tCtrl+U";
        public const string ShortcutCtrlV = "\tCtrl+V";
        public const string ShortcutCtrlX = "\tCtrl+X";
        public const string ShortcutCtrlY = "\tCtrl+Y";
        public const string ShortcutCtrlZ = "\tCtrl+Z";
        public const string ShortcutDelete = "\tDelete";
        public const string ShortcutEsc = "\tEsc";
        public const string ShortcutF1 = "\tF1";
        public const string ShortcutF11 = "\tF11";
        public const string ShortcutF12 = "\tF12";
        public const string ShortcutF2 = "\tF2";
        public const string ShortcutF3 = "\tF3";
        public const string ShortcutF4 = "\tF4";
        public const string ShortcutF5 = "\tF5";
        public const string ShortcutF6 = "\tF6";
        public const string ShortcutF7 = "\tF7";
        public const string ShortcutF8 = "\tF8";
        public const string ShortcutF9 = "\tF9";
        public const string ShortcutShiftCtrlDelete = "\tShift+Ctrl+Delete";
        public const string ShortcutShiftCtrlE = "\tShift+Ctrl+E";
        public const string ShortcutShiftCtrlL = "\tShift+Ctrl+L";
        public const string ShortcutShiftCtrlMinus = "\tShift+Ctrl+−";
        public const string ShortcutShiftCtrlN = "\tShift+Ctrl+N";
        public const string ShortcutShiftCtrlP = "\tShift+Ctrl+P";
        public const string ShortcutShiftCtrlPlus = "\tShift+Ctrl++";
        public const string ShortcutShiftCtrlT = "\tShift+Ctrl+T";
        public const string SplitWordsPattern = "\\s+";
        public const string StatusOk = "Ok";
        public const string StripSearchFormat = "{0}/{1} {2}";
        public const string StripSearchMatches = "matches";
        public const string StripSearchMatchesShort1 = "match";
        public const string StripSearchMatchesShort2 = "m.";
        public const string StripSearchNotFound = "String not found";
        public const string TabControlLeftNameEnd = "Left";
        public const string TabControlRightNameEnd = "Right";
        public const string TelegramDesktopApplicationName = "Telegram Desktop";
        public const string TelegramDesktopFileName = "Telegram.exe";
        public const string ThreeDots = "...";
        public const string TimeFormatForFilename = "yyyyMMdd_HHmmss";
        public const string TimeFormatForUid = "yyyyMMddHH";
        public const string ToStringTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string TrimBookmarkPattern = "^\\s*(.+)\\s*-.*$";
        public const string TrustDegreePattern = "^[^\\d]*(\\d+)[^\\d]*.*$";
        public const string TwoDecimalDigitsFormat = "f2";
        public const string TypedUrlsFileName = "TypedUrls.dat";
        public const string UnitPrefixBinary = "Binary";
        public const string UnitPrefixDecimal = "Decimal";
        public const string UriFragment = "Fragment";
        public const string UriHost = "Host";
        public const string UriPassword = "Password";
        public const string UriPath = "Path";
        public const string UriPort = "Port";
        public const string UriQuery = "Query String";
        public const string UriScheme = "Scheme";
        public const string UriUserInfo = "User Info";
        public const string UriUserName = "User Name";
        public const string UriVariables = "Variables";
        public const string UserDataRelativePath = "UserData";
        public const string VersionRegexPattern = "^\\d+\\.\\d+\\.\\d+\\.\\d$";
        public const string WordPadFileName = "write.exe";
        public const string XmlElementRemoteAddress = "RemoteAddress";
        public const string XmlElementStatus = "Status";
        public const string XmlElementVersion = "Version";
        public const string ZeroDecimalDigitsFormat = "f0";

        /// <summary>
        /// Data size units with their binary and decimal prefixes.
        /// </summary>
        public const string Byte = "B";
        public const string Kibibyte = "KiB";
        public const string Kilobyte = "kB";
        public const string Mebibyte = "MiB";
        public const string Megabyte = "MB";
        public const string Gibibyte = "GiB";
        public const string Gigabyte = "GB";
        public const string Tebibyte = "TiB";
        public const string Terabyte = "TB";
        public const string Pebibyte = "PiB";
        public const string Petabyte = "PB";
        public const string Exbibyte = "EiB";
        public const string Exabyte = "EB";
        public const string Zebibyte = "ZiB";
        public const string Zettabyte = "ZB";
        public const string Yobibyte = "YiB";
        public const string Yottabyte = "YB";
        public const string Robibyte = "RiB";
        public const string Ronnabyte = "RB";
        public const string Qubibyte = "QiB";
        public const string Quettabyte = "QB";
    }
}
