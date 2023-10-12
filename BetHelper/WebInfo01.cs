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
 * Version 1.1.14.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo01 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(9);

            if (ElementExistsAndVisible(browser,
                    "document.getElementById('js-LayersReact').children[1].children[0].children[2].children[1]", false)) {

                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync(
                    "document.getElementById('js-LayersReact').children[1].children[0].children[2].children[1].click();");
                Wait(browser);
                Sleep(50);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("document.getElementById('js-app').children[1].children[1].children[0].children[1].children[2].children[0]")
                .Append(".children[1].children[0].click();")
                .ToString());
            Wait(browser);
            Sleep(150);

            do {
                Sleep(100);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementsByName('username')[0].value = '';");
                Wait(browser);
                Sleep(100);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (ElementExists(browser, "document.getElementsByName('username')[0]", true)) {
                    browser.ExecuteScriptAsync("document.getElementsByName('username')[0].focus();");
                    Sleep(250);
                } else {
                    OnError();
                    return;
                }

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueBy(ElementAttribute.Name, "username").Equals(UserName));

            SendKey(browser, Keys.Tab);

            do {
                Sleep(100);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementsByName('password')[0].value = '';");
                Wait(browser);
                Sleep(100);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (ElementExists(browser, "document.getElementsByName('password')[0]", true)) {
                    browser.ExecuteScriptAsync("document.getElementsByName('password')[0].focus();");
                    Sleep(250);
                } else {
                    OnError();
                    return;
                }

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueBy(ElementAttribute.Name, "password").Equals(Password));

            OnProgress(Properties.Resources.MessageLoggingIn);
            if (!ClickElement(browser,
                    "document.getElementsByClassName('row middle-xs')[1].parentElement.getElementsByTagName('button')[0]")) {

                OnError();
                return;
            }
            Wait(browser);

            if (ElementExists(browser, "document.getElementById('modalDialogCloseId')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('modalDialogCloseId').click();");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser,
                    "document.getElementById('js-LayersReact').children[1].children[0].children[2].children[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementById('js-LayersReact').children[1].children[0].children[2].children[1].click();");
            }
            if (ElementExists(browser, "document.getElementById('modalDialogCloseId')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('modalDialogCloseId').click();");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[1]")
                    .ToString(), false)) {

                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[1].style.display = 'none';")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[2]")
                    .ToString(), false)) {

                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[2].style.display = 'none';")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[3]")
                    .ToString(), false)) {

                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("document.getElementById('js-app').children[2].children[1].children[0].children[0].children[1].children[0]")
                    .Append(".children[3].style.display = 'none';")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('promoWrapId')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('promoWrapId').style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser,
                        "document.getElementById('js-headNavMenu').children[0].children[0].children[0].children[10]", false)) {

                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("document.getElementById('js-headNavMenu').children[0].children[0].children[0].children[10].style.display =")
                    .Append(" 'none';")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('m-marketingNotification')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('m-marketingNotification')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('o-modalTicket__rightBanner')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('o-modalTicket__rightBanner')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('appMessages')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('appMessages')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('a-ticketBanner')[0].parentElement", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('a-ticketBanner')[0].parentElement.style.display = 'none';");
            }
        }
    }
}
