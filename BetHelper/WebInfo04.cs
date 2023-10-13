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
 * Version 1.1.15.1
 */

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo04 : WebInfo {
        protected List<string> matchingGameIds;

        protected override void LogIn(ChromiumWebBrowser browser) {
            if (BrowserAddress.StartsWith(UrlLive, StringComparison.OrdinalIgnoreCase)) {
                LogInLive(browser);
            } else {
                LogInMain(browser);
            }
        }

        private void LogInMain(ChromiumWebBrowser browser) {
            OnStarted(10);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('webmessage-close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('webmessage-close')[0].click();");
                Wait(browser);
                Sleep(30);
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('cookie-consent-button-accept')", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementById('cookie-consent-button-accept').click();");
                Wait(browser);
                Sleep(200);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (!ElementExists(browser, "document.getElementById('user-box-login')", true)) {
                OnError(Properties.Resources.MessageLogInBoxNotFoundError);
                return;
            }

            do {
                Sleep(30);
                OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
                browser.ExecuteScriptAsync("document.getElementById('user-box-login').classList.add('user-box-login--visible');");
                Wait(browser);
                Sleep(200);

                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementsByName('username')[0].value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('username')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueBy(ElementAttribute.Name, "username").Equals(UserName));

            do {
                Sleep(30);
                OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
                browser.ExecuteScriptAsync("document.getElementById('user-box-login').classList.add('user-box-login--visible');");
                Wait(browser);
                Sleep(200);

                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementsByName('password')[0].value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('password')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueBy(ElementAttribute.Name, "password").Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(500);

            OnFinished();
        }

        private void LogInLive(ChromiumWebBrowser browser) {
            OnStarted(10);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('webMessageContainer')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('webMessageContainer')[0].style.display = 'none';");
                Wait(browser);
                Sleep(30);
            }
            if (ElementExistsAndVisible(browser,
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1]", false)) {

                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1].click();");
                Wait(browser);
                Sleep(50);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (!ElementExistsAndVisible(browser, "document.getElementsByClassName('login_form__button')[1]", true)) {
                OnError(Properties.Resources.MessageLogInBoxNotFoundError);
                return;
            }

            do {
                Sleep(30);
                OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
                browser.ExecuteScriptAsync("document.getElementsByClassName('login_form__button')[1].click();");
                Wait(browser);
                Sleep(500);

                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementsByName('username')[0].value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('username')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(500);

                if (IsLoggedIn()) {
                    OnFinished();
                    return;
                }
                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueBy(ElementAttribute.Name, "username").Equals(UserName));

            do {
                Sleep(30);
                OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
                browser.ExecuteScriptAsync("document.getElementById('user-box-login').classList.add('user-box-login--visible');");
                Wait(browser);
                Sleep(200);

                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementsByName('password')[0].value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('password')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueBy(ElementAttribute.Name, "password").Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(500);

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementById('cookie-consent-button-accept')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('cookie-consent-button-accept').click();");
            }
            if (ElementExistsAndVisible(browser,
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1].click();");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementById('cookie-consent-button-accept')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('cookie-consent-button-accept').click();");
            }
            if (ElementExistsAndVisible(browser,
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('cookie-consents__modal')[0].children[1].children[1].click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('dy-lb-close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('dy-lb-close')[0].click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('xtrm_prompt-decline')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('xtrm_prompt-decline')[0].click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('communications')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('communications')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('account-banner')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('account-banner').style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('hp-carousel')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('hp-carousel').style.display = 'none';");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 2; i < 7; i++) if (i != 5) document")
                .Append(".getElementsByClassName('main-navigation')[0].children[1].children[0].children[i].style.display = 'none';")
                .ToString());

            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 3; i < 8; i++) if (i != 6) document")
                .Append(".getElementsByClassName('top_panel')[0].children[0].children[0].children[i].style.display = 'none';")
                .ToString());
        }

        protected override bool HasFastOpportunity(ChromiumWebBrowser browser) {
            if (!StaticMethods.EqualsHostsAndSchemes(UrlLive, BrowserAddress)) {
                return false;
            }
            string response = null;
            try {
                if (browser.CanExecuteJavascriptInMainFrame) {
                    JavascriptResponse javascriptResponse = browser
                        .EvaluateScriptAsync("document.getElementsByClassName('sport--FOOTBALL')[0].innerHTML")
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
            if (string.IsNullOrWhiteSpace(response)) {
                return false;
            }
            FastOpportunity fastOpportunity = new FastOpportunity(response);
            bool unique = false;
            foreach (string id in fastOpportunity.GetFOMatchingGameIds()) {
                if (!matchingGameIds.Contains(id)) {
                    matchingGameIds.Add(id);
                    unique = true;
                }
            }
            return unique;
        }
    }
}
