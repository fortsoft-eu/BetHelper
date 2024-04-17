/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.7
 */

using CefSharp;
using CefSharp.WinForms;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo16 : WebInfo {
        private string consentElement = "document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll')";

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(9);

            if (ElementExistsAndVisible(browser, consentElement, false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync(consentElement + ".click();");
                Sleep(100);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (!ElementExistsAndVisible(browser, new string[] {
                        "document.getElementsByClassName('header__login-button')[0]",
                        "document.getElementsByClassName('js-header__toggler')[0]"
                    }, true)) {

                OnError();
                return;
            }

            Sleep(50);
            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            browser.ExecuteScriptAsync("document.getElementsByClassName('header__login-button')[0].click();");
            Wait(browser);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementById('frm-signInForm-form-email').value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('frm-signInForm-form-email')")) {
                    OnError();
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueById("frm-signInForm-form-email").Equals(UserName));

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
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(100);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('highlight')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('highlight')[0].remove();");
            }
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, consentElement, false)) {
                browser.ExecuteScriptAsync(consentElement + ".click();");
                Sleep(100);
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('highlight')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('highlight')[0].remove();");
            }
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("var closeButtons = document.getElementsByClassName('alert-close'); ")
                .Append("for (let i = 0; i < closeButtons.length; i++) closeButtons[i].click()")
                .ToString());
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('highlight')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('highlight')[0].remove();");
            }
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
        }
    }
}
