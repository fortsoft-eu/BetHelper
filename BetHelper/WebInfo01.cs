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
 * Version 1.1.0.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo01 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(6);

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

            do {
                Sleep(100);
                OnProgress(Properties.Resources.MessageClearingUsernameBox);
                browser.ExecuteScriptAsync("document.getElementById('userNameId').value = '';");
                Wait(browser);
                Sleep(100);

                if (IsLoggedIn()) {
                    OnFinished();
                    return;
                }
                OnProgress(Properties.Resources.MessageSendingUsername);
                SendString(browser, Username);
            } while (!GetValueById("userNameId").Equals(Username));

            SendKey(browser, Keys.Tab);

            do {
                Sleep(100);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('passwordId').value = '';");
                Wait(browser);
                Sleep(100);

                if (IsLoggedIn()) {
                    OnFinished();
                    return;
                }
                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("passwordId").Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
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
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('m-mainMenu')[0].children[1]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('m-mainMenu')[0].children[1].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('m-mainMenu')[0].children[2]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('m-mainMenu')[0].children[2].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('promoWrapId')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('promoWrapId').style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('o-subMenu')[0].children[10]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('o-subMenu')[0].children[10].style.display = 'none';");
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
