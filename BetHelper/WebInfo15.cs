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
 * Version 1.0.0.0
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
    public class WebInfo15 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(10);

            if (ElementExistsAndVisible(browser, "document.getElementById('c-p-bn')", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementById('c-p-bn').click();");
                Sleep(100);
            }

            bool smallExists = ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-small')[0]", false);
            bool largeExists = ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-large')[0]", false);
            if (smallExists || largeExists) {
                OnProgress(Properties.Resources.MessageClosingAdvertisement);
                if (smallExists) {
                    browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-small')[0].style.display = 'none';");
                }
                if (largeExists) {
                    browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-large')[0].style.display = 'none';");
                }
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (!ElementExistsAndVisible(browser, new string[] { "document.getElementsByClassName('a-link_label')[0]", "document.getElementsByClassName('a-navbar-toggle')[0]" }, true)) {
                OnError();
                return;
            }

            Sleep(50);
            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            browser.ExecuteScriptAsync("document.getElementsByClassName('a-nav-link')[0].click();");
            Wait(browser);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUsernameBox);
                browser.ExecuteScriptAsync("document.getElementById('frm-signInForm-form-email').value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('frm-signInForm-form-email')")) {
                    OnError();
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingUsername);
                SendString(browser, Username);
            } while (!GetValueById("frm-signInForm-form-email").Equals(Username));

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('frm-signInForm-form-password').value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('frm-signInForm-form-password')")) {
                    OnError();
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("frm-signInForm-form-password").Equals(Password));

            SendKey(browser, Keys.Tab);
            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(100);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-small')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-small')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-large')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-large')[0].style.display = 'none';");
            }

            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementById('c-p-bn')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('c-p-bn').click();");
                Sleep(100);
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-small')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-small')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-large')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-large')[0].style.display = 'none';");
            }
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
        }

        public override bool IsLoggedIn() {
            if (string.IsNullOrEmpty(Script) || !frameInitialLoadEnded) {
                return false;
            }
            string javaScript = "document.getElementsByClassName('a-nav-link')[0].innerText;";
            string notLoggedInText = "PŘIHLÁSIT SE";
            try {
                if (Browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = Browser.EvaluateScriptAsync(javaScript).GetAwaiter().GetResult();
                    if (javascriptResponse != null && javascriptResponse.Success) {
                        return string.Compare(notLoggedInText, (string)javascriptResponse.Result, string.IsNullOrEmpty(IetfLanguageTag) ?
                            CultureInfo.InvariantCulture : CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag), CompareOptions.IgnoreCase) != 0;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return true;
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-small')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-small')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('banner-hide-for-large')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('banner-hide-for-large')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('o-footer_top')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('o-footer_top')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('o-footer_top-bg')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('o-footer_top-bg')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('o-footer')[0].children[2]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('o-footer')[0].children[2].remove();");
            }
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
        }

        protected override Tip[] GetTips(ChromiumWebBrowser browser) {
            if (BrowserAddress != UrlTips) {
                return null;
            }
            string response = null;
            try {
                if (browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = browser.EvaluateScriptAsync("document.getElementsByClassName('my-tips')[0].innerHTML").GetAwaiter().GetResult();
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
                DateTime dateTimeNow = DateTime.Now;
                Regex endHtmlTagRegex = new Regex("\\s*</.*>.*$", RegexOptions.Singleline);
                Regex lineRegex = new Regex("\\s*(</[^>]+>)*\\s*<\\w+[^>]+>\\s*");
                Regex matchRegex = new Regex("^.*</span>\\s*(.*)</div>\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex splitTipsRegex = new Regex("<div\\s+class=\"o-tbody[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex sportRegex = new Regex("^.*<use\\s+xlink:.*\\.svg#sprite-(\\w+(-\\w+)*)\"\\s*>.*$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                int i = 0;
                foreach (string serviceBlock in response.Split(new string[] { "<div class=\"o-thead\">" }, StringSplitOptions.None)) {
                    if (i++ == 0) {
                        continue;
                    }
                    string service = null;
                    string[] splitTips = splitTipsRegex.Split(serviceBlock);
                    for (int j = 0; j < splitTips.Length; j++) {
                        if (j == 0) {
                            int k = 0;
                            foreach (string rawHeaderItem in lineRegex.Split(splitTips[j])) {
                                if (string.IsNullOrEmpty(rawHeaderItem)) {
                                    continue;
                                }
                                if (k++ == 1) {
                                    service = endHtmlTagRegex.Replace(rawHeaderItem, string.Empty);
                                    break;
                                }
                            }
                        } else {
                            List<string> sportList = new List<string>();
                            int k = 0;
                            foreach (string rawSportItem in splitTips[j].Split(new string[] { "<div class=\"m-tr sortable-item\">" }, StringSplitOptions.None)) {
                                if (k++ == 0) {
                                    continue;
                                }
                                string sport = sportRegex.Replace(rawSportItem, Constants.ReplaceFirst);
                                switch (sport) {
                                    case "athletics":
                                        sport = "Atletika";
                                        break;
                                    case "basketball":
                                        sport = "Basketbal";
                                        break;
                                    case "darts":
                                        sport = "Šipky";
                                        break;
                                    case "e-sports":
                                        sport = "E-sporty";
                                        break;
                                    case "floorball":
                                        sport = "Florbal";
                                        break;
                                    case "football":
                                        sport = "Fotbal";
                                        break;
                                    case "handball":
                                        sport = "Házená";
                                        break;
                                    case "hockey":
                                        sport = "Hokej";
                                        break;
                                    case "motorsport":
                                        sport = "Motorsport";
                                        break;
                                    case "tennis":
                                        sport = "Tenis";
                                        break;
                                    case "volleyball":
                                        sport = "Volejbal";
                                        break;
                                    case "winter-sport":
                                        sport = "Zimní sport";
                                        break;
                                }
                                sportList.Add(sport.Length < 30 ? StaticMethods.UppercaseFirst(sport, CultureInfo.GetCultureInfoByIetfLanguageTag(IetfLanguageTag)) : null);
                            }
                            List<string> tipList = new List<string>();
                            foreach (string rawHeaderItem in lineRegex.Split(splitTips[j])) {
                                if (string.IsNullOrEmpty(rawHeaderItem) || rawHeaderItem.Equals("</div>")) {
                                    continue;
                                }
                                tipList.Add(rawHeaderItem);
                            }
                            if (tipList.Count < 12) {
                                continue;
                            }
                            float trustDegree = float.Parse(Regex.Replace(tipList[8], Constants.TrustDegreePattern, Constants.ReplaceFirst));
                            float odd = float.Parse(Regex.Replace(tipList[9].Replace(Constants.Comma, Constants.Period), Constants.OddPattern, string.Empty), CultureInfo.InvariantCulture);
                            string bookmaker = tipList[10];
                            tipList.RemoveRange(8, 4);
                            List<Game> games = new List<Game>();
                            for (k = 0; k < tipList.Count / 8; k++) {
                                DateTime dateTime;
                                if (tipList[0 + k * 8].Contains("se hraje")) {
                                    dateTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, int.Parse(tipList[1 + k * 8].Substring(0, 2)), int.Parse(tipList[1 + k * 8].Substring(3, 2)), 0);
                                } else {
                                    string[] span = tipList[1 + k * 8].Split(Constants.Colon);
                                    if (span.Length < 3) {
                                        continue;
                                    }
                                    dateTime = dateTimeNow.Add(new TimeSpan(int.Parse(span[0]), int.Parse(span[1]), int.Parse(span[2])));
                                }
                                StringBuilder match = new StringBuilder(tipList[4 + k * 8]).Append(Constants.Space).Append(Constants.EnDash).Append(Constants.Space).Append(matchRegex.Replace(tipList[5 + k * 8], Constants.ReplaceFirst).TrimEnd());
                                games.Add(new Game(dateTime.AddMinutes(((int)Math.Round(dateTime.Minute / 5.0)) * 5 - dateTime.Minute), sportList[k], endHtmlTagRegex.Replace(tipList[6 + k * 8], string.Empty), match.ToString(), endHtmlTagRegex.Replace(tipList[7 + k * 8], string.Empty)));
                            }
                            list.Add(new Tip(dateTimeNow, games.ToArray(), bookmaker, odd, trustDegree, service, Tip.TipStatus.Received));
                        }
                    }
                }
            }
            return list.ToArray();
        }
    }
}
