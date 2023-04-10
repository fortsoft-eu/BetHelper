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
 * Version 1.1.0.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo08 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(9);

            OnProgress(Properties.Resources.MessageClosingCookieConsent);
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('otPlaceholder')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('otPlaceholder')[0].style.display = 'none';");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 0; i < document.getElementsByClassName('adsenvelope').length; i++) ")
                .Append("document.getElementsByClassName('adsenvelope')[i].style.display = 'none';")
                .ToString());
            Wait(browser);
            Sleep(30);

            if (ElementExistsAndVisible(browser, "document.getElementById('onetrust-accept-btn-handler')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('onetrust-accept-btn-handler').click();");
                Wait(browser);
                Sleep(30);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            if (!ElementExistsAndVisible(browser, "document.getElementById('user-menu')", true)) {
                OnError();
                return;
            }
            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            browser.ExecuteScriptAsync("document.getElementById('user-menu').click();");
            Wait(browser);
            Sleep(500);
            browser.ExecuteScriptAsync("document.getElementsByClassName('social__button email')[0].click();");

            do {
                Sleep(500);
                OnProgress(Properties.Resources.MessageClearingUsernameBox);
                browser.ExecuteScriptAsync("document.getElementById('email').value = '';");
                Wait(browser);
                Sleep(100);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('email')")) {
                    OnError();
                    return;
                }
                Sleep(500);

                if (IsLoggedIn()) {
                    OnFinished();
                    return;
                }
                OnProgress(Properties.Resources.MessageSendingUsername);
                SendString(browser, Username);
            } while (!GetValueById("email").Equals(Username));

            do {
                Sleep(500);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('passwd').value = '';");
                Wait(browser);
                Sleep(100);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('passwd')")) {
                    OnError();
                    return;
                }
                Sleep(500);

                if (IsLoggedIn()) {
                    OnFinished();
                    return;
                }
                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("passwd").Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(1000);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('modal__window')[0]", false)
                    || ElementExistsAndVisible(browser, "document.getElementsByClassName('ui-processResult')[0]", false)) {

                if (!ClickElement(browser, "document.getElementsByClassName('close')[0]")) {
                    OnFinished();
                    return;
                }
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('otPlaceholder')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('otPlaceholder')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('onetrust-accept-btn-handler')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('onetrust-accept-btn-handler').click();");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('langBoxModule__close')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('langBoxModule__close')[0].click();");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 0; i < document.getElementsByClassName('adsenvelope').length; i++) ")
                .Append("document.getElementsByClassName('adsenvelope')[i].style.display = 'none';")
                .ToString());

            browser.ExecuteScriptAsync("document.body.classList.remove('background-add-on');");
            browser.ExecuteScriptAsync("document.querySelector('[id^=\"box-over-content\"]').remove();");
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('otPlaceholder')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('otPlaceholder')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('onetrust-accept-btn-handler')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('onetrust-accept-btn-handler').click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('selfPromo')[0].children[0].children[1]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('selfPromo')[0].children[0].children[1].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('footer__block')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('footer__block')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('footer__mobileScreen')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('footer__mobileScreen')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('detailLeaderboard')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('detailLeaderboard')[0].remove();");
            }
        }
    }
}
