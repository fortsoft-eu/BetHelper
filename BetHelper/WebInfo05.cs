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
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo05 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(11);

            if (ElementExists(browser, "document.getElementsByClassName('mwc-btn')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('mwc-btn')[0].click();");
                Wait(browser);
                Sleep(500);
            }

            if (ElementExists(browser, "document.getElementsByClassName('consent-bar__actions')[0].children[0]", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementsByClassName('consent-bar__actions')[0].children[0].click();");
                Wait(browser);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            Sleep(1000);
            if (!ElementExistsAndVisible(browser, "document.getElementsByClassName('user-menu__login')[0]", true)) {
                OnError(Properties.Resources.MessageLogInButtonNotExistError);
                return;
            }
            Sleep(1000);
            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            if (!ClickElement(browser, "document.getElementsByClassName('user-menu__login')[0]")) {
                OnError(Properties.Resources.MessageClickingLoginButtonError);
                return;
            }
            Wait(browser);
            Sleep(1000);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementById('user').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('user')")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(150);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueById("user").Equals(UserName));

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('password').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('password')")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(150);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("password").Equals(Password));

            SendKey(browser, Keys.Tab);
            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(200);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sazka-toast__close')[0]", true)) {
                OnProgress(Properties.Resources.MessageClosingAdvertisement);
                browser.ExecuteScriptAsync("document.getElementsByClassName('sazka-toast__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('gw-sticker__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('gw-sticker__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementById('fixed-help')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('fixed-help').remove();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('quick-nav')[0].children[3]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('quick-nav')[0].children[3].remove();");
            }

            if (decimal.MinValue.Equals(GetBalance())) {
                OnProgress(Properties.Resources.MessageDisplayingBalance);
                if (ElementExists(browser, "document.getElementById('userName')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('userName').click();");
                }
                if (ElementExists(browser, "document.getElementById('toggleBalance')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('toggleBalance').click();");
                }
            }

            if (ElementExists(browser, "document.getElementsByClassName('footer-social')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('footer-social')[0].style.display = 'none';");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementsByClassName('consent-bar__actions')[0].children[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('consent-bar__actions')[0].children[0].click();");
            }
            Wait(browser);
            Sleep(100);
            if (ElementExists(browser, "document.getElementsByClassName('sazka-toast__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sazka-toast__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('gw-sticker__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('gw-sticker__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementById('fixed-help')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('fixed-help').remove();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('quick-nav')[0].children[3]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('quick-nav')[0].children[3].remove();");
            }

            if (decimal.MinValue.Equals(GetBalance())) {
                if (ElementExists(browser, "document.getElementById('userName')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('userName').click();");
                }
                if (ElementExists(browser, "document.getElementById('toggleBalance')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('toggleBalance').click();");
                }
            }

            if (ElementExists(browser, "document.getElementsByClassName('footer-social')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('footer-social')[0].style.display = 'none';");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementsByClassName('sazka-toast__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sazka-toast__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('gw-sticker__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('gw-sticker__close')[0].click();");
            }
            if (ElementExists(browser, "document.getElementById('fixed-help')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('fixed-help').remove();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('quick-nav')[0].children[3]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('quick-nav')[0].children[3].remove();");
            }

            if (decimal.MinValue.Equals(GetBalance())) {
                if (ElementExists(browser, "document.getElementById('userName')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('userName').click();");
                }
                if (ElementExists(browser, "document.getElementById('toggleBalance')", false)) {
                    browser.ExecuteScriptAsync("document.getElementById('toggleBalance').click();");
                }
            }

            if (ElementExists(browser, "document.getElementsByClassName('footer-social')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('footer-social')[0].style.display = 'none';");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 2; i < 7; i++) document")
                .Append(".getElementsByClassName('sports-bets-menu')[0].children[0].children[1].children[i].style.display = 'none';")
                .ToString());
            if (ElementExists(browser, "document.getElementsByClassName('sazka-notification__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sazka-notification__close')[0].click();");
            }
        }
    }
}
