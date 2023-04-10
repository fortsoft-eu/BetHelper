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

namespace BetHelper {
    public class WebInfo16 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(9);

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            if (ElementExists(browser, "document.getElementById('ctr-hamburger-1')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('ctr-hamburger-1').click();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('open-login-modal')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('open-login-modal')[0].click();");
            }
            Wait(browser);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUsernameBox);
                browser.ExecuteScriptAsync("document.getElementById('login-email').value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('login-email')")) {
                    OnError();
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingUsername);
                SendString(browser, Username);
            } while (!GetValueById("login-email").Equals(Username));

            OnProgress(Properties.Resources.MessageLoggingIn);
            browser.ExecuteScriptAsync("document.getElementById('login-btn').click();");
            Wait(browser);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('login-password').value = '';");
                Wait(browser);
                Sleep(30);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('login-password')")) {
                    OnError();
                    return;
                }
                Sleep(200);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("login-password").Equals(Password));

            OnProgress(Properties.Resources.MessageLoggingIn);
            browser.ExecuteScriptAsync("document.getElementById('login-btn').click();");
            Wait(browser);

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('tw-bg-customTheme01')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('tw-bg-customTheme01')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('ctr-img-w-h')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('ctr-img-w-h')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('support-section')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('support-section').remove();");
            }
            if (ElementExistsAndVisible(browser,
                    "document.getElementsByClassName('header-r-sec')[0].getElementsByClassName('scaleOnHover')[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('header-r-sec')[0].getElementsByClassName('scaleOnHover')[1].style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('mem--btn-seemore-slider')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('mem--btn-seemore-slider')[0].click();");
            }
            if (ElementExistsAndVisible(browser, "document.querySelector('[id^=\"edit-\"]').children[4].children[1]", false)) {
                browser.ExecuteScriptAsync("document.querySelector('[id^=\"edit-\"]').children[4].children[1].style.display = 'none';");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('tw-bg-customTheme01')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('tw-bg-customTheme01')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('ctr-img-w-h')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('ctr-img-w-h')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('support-section')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('support-section').remove();");
            }
            if (ElementExistsAndVisible(browser,
                    "document.getElementsByClassName('header-r-sec')[0].getElementsByClassName('scaleOnHover')[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('header-r-sec')[0].getElementsByClassName('scaleOnHover')[1].style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('mem--btn-seemore-slider')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('mem--btn-seemore-slider')[0].click();");
            }
            if (ElementExistsAndVisible(browser, "document.querySelector('[id^=\"edit-\"]').children[4].children[1]", false)) {
                browser.ExecuteScriptAsync("document.querySelector('[id^=\"edit-\"]').children[4].children[1].style.display = 'none';");
            }
        }
    }
}
