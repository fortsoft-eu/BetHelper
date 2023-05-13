/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.11.2
 */

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo17 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(7);

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementById('username-133').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('username-133')")) {
                    OnError();
                    return;
                }
                Sleep(100);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueById("username-133").Equals(UserName));

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('user_password-133').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('user_password-133')")) {
                    OnError();
                    return;
                }
                Sleep(100);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("user_password-133").Equals(Password));

            SendKey(browser, Keys.Tab);
            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);

            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolnibanmobil')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolnibanmobil').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolnibanmobil')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolnibanmobil').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolnibanmobil')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolnibanmobil').remove();");
            }
            if (ElementExists(browser, "document.getElementById('dolniban')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('dolniban').remove();");
            }
        }

        protected override Tip[] GetTips(ChromiumWebBrowser browser) {
            if (BrowserAddress != UrlTips) {
                return null;
            }
            string response = null;
            try {
                if (browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = browser
                        .EvaluateScriptAsync("document.getElementsByClassName('tips')[0].innerHTML")
                        .GetAwaiter()
                        .GetResult();
                    if (javascriptResponse.Success) {
                        response = (string)javascriptResponse.Result;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            List<Tip> list = new List<Tip>();
            if (!string.IsNullOrWhiteSpace(response)) {
                Regex bookmakerRegex = new Regex("^.*<img\\s+style.*\"([^\"]*)\"\\s*></a></div>.*$",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex dateTimeSplitRegex = new Regex("^(.*)\\((.*)\\)$");
                Regex endHtmlTagRegex = new Regex("(\\s+\\?)?</.*>$");
                Regex lineRegex = new Regex("\\s*(</[^>]+>)*\\s*<\\w+[^>]+>\\s*");
                int i = 0;
                foreach (string rawTipLine in Regex.Split(response, "<li\\s+class=\"reason\"[^>]+>", RegexOptions.IgnoreCase)) {
                    if (i++ == 0) {
                        continue;
                    }
                    string bookmaker = bookmakerRegex.Replace(rawTipLine, Constants.ReplaceFirst);
                    string[] splitRawTipLine = lineRegex.Split(rawTipLine);
                    List<string> tipLineItems = new List<string>(splitRawTipLine.Length);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (string rawItem in splitRawTipLine) {
                        string item = rawItem.Replace("</span>", string.Empty);
                        if (string.IsNullOrEmpty(item)) {
                            continue;
                        }
                        if (item.Equals("</div>")) {
                            if (stringBuilder.Length > 0) {
                                tipLineItems.Add(endHtmlTagRegex.Replace(stringBuilder.ToString().TrimEnd(), string.Empty));
                                stringBuilder = new StringBuilder();
                            }
                            continue;
                        }
                        if (stringBuilder.Length.Equals(0)) {
                            stringBuilder.Append(item);
                            stringBuilder.Append(Constants.Space);
                        } else {
                            stringBuilder.Append(item);
                        }
                    }
                    if (stringBuilder.Length > 0) {
                        tipLineItems.Add(endHtmlTagRegex.Replace(stringBuilder.ToString().TrimEnd(), string.Empty));
                    }
                    try {
                        DateTime dateTime = new DateTime(
                            int.Parse(tipLineItems[6].Substring(7, 4)),
                            int.Parse(tipLineItems[6].Substring(3, 2)),
                            int.Parse(tipLineItems[6].Substring(0, 2)),
                            int.Parse(tipLineItems[7].Substring(0, 2)),
                            int.Parse(tipLineItems[7].Substring(3, 2)),
                            0);
                        Game game = new Game(
                            dateTime,
                            StaticMethods.UppercaseFirst(tipLineItems[0], CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)),
                            StaticMethods.UppercaseFirst(tipLineItems[5], CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)),
                            StaticMethods.UppercaseFirst(dateTimeSplitRegex.Replace(tipLineItems[1], Constants.ReplaceFirst),
                                CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)),
                            new StringBuilder()
                                .Append(StaticMethods.UppercaseFirst(
                                    dateTimeSplitRegex.Replace(tipLineItems[1], Constants.ReplaceSecond),
                                    CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)))
                                .Append(Constants.Space)
                                .Append(tipLineItems[3])
                                .ToString());
                        list.Add(new Tip(
                            DateTime.Now,
                            new Game[] { game },
                            bookmaker.Length < 30 ? bookmaker : null,
                            float.Parse(tipLineItems[2], NumberStyles.Float, CultureInfo.InvariantCulture),
                            10f,
                            StaticMethods.UppercaseFirst(Title, CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)),
                            Tip.TipStatus.Received));
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
            return list.ToArray();
        }
    }
}
