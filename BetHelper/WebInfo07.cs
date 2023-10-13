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
 * Version 1.1.15.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo07 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(9);

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (ElementExists(browser, "document.getElementById('app-header').getElementsByClassName('green')[0]", false)) {
                OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
                browser.ExecuteScriptAsync("document.getElementById('app-header').getElementsByClassName('green')[0].click();");
                Wait(browser);
            }

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementsByName('login')[0].value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('login')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(150);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueBy(ElementAttribute.Name, "login").Equals(UserName));

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementsByName('password')[0].value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('password')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(150);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueBy(ElementAttribute.Name, "password").Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);

            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
            if (ElementExists(browser, "document.getElementsByClassName('footer')[0].children[0].children[2]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('footer')[0].children[0].children[2].style.display = 'none';");
            }

            Sleep(500);
            if (decimal.MinValue.Equals(GetBalance())) {
                OnProgress(Properties.Resources.MessageDisplayingBalance);
                if (ElementExists(browser, "document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0]", false)) {
                    browser.ExecuteScriptAsync("document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0].click();");
                }
            }

            Sleep(250);
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('close')[0].click();");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
            if (ElementExists(browser, "document.getElementsByClassName('footer')[0].children[0].children[2]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('footer')[0].children[0].children[2].style.display = 'none';");
            }

            if (decimal.MinValue.Equals(GetBalance())) {
                if (ElementExists(browser, "document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0]", false)) {
                    browser.ExecuteScriptAsync("document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0].click();");
                }
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (RemoveChat) {
                if (ElementExistsAndVisible(browser, "document.getElementById('chat-application')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('chat-application').remove();");
                }
            }
            if (ElementExists(browser, "document.getElementsByClassName('footer')[0].children[0].children[2]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('footer')[0].children[0].children[2].style.display = 'none';");
            }

            if (decimal.MinValue.Equals(GetBalance())) {
                if (ElementExists(browser, "document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0]", false)) {
                    browser.ExecuteScriptAsync("document.getElementsByClassName('logged')[0].getElementsByTagName('small')[0].click();");
                }
            }
        }
    }
}
