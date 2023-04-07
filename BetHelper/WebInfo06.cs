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
 * Version 1.0.0.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo06 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(8);

            if (ElementExists(browser, "document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll')", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll').click();");
                Wait(browser);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUsernameBox);
                browser.ExecuteScriptAsync("document.getElementsByName('login')[0].value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementsByName('login')[0]")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(150);

                OnProgress(Properties.Resources.MessageSendingUsername);
                SendString(browser, Username);
            } while (!GetValueBy(ElementAttribute.Name, "login").Equals(Username));

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

            if (ElementExistsAndVisible(browser, "document.getElementById('launcher')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('launcher').remove();");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll').click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('launcher')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('launcher').remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('bx-wrapper')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('bx-wrapper')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('carousel')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('carousel')[0].remove();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('middle-row__socials')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('middle-row__socials')[0].style.visibility = 'hidden';");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll').click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('launcher')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('launcher').remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('bx-wrapper')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('bx-wrapper')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('carousel')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('carousel')[0].remove();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('middle-row__socials')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('middle-row__socials')[0].style.visibility = 'hidden';");
            }
            if (ElementExists(browser, "document.getElementsByClassName('hide-notification-dialog')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('hide-notification-dialog')[0].click();");
            }
            if (ElementExists(browser, "document.getElementsByClassName('top-row')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('top-row')[0].style.display = 'none';");
            }
            if (ElementExists(browser, "document.querySelector('[class*=\"-banner-wrapper\"]')", false)) {
                browser.ExecuteScriptAsync("document.querySelector('[class*=\"-banner-wrapper\"]').style.display = 'none';");
            }
            browser.ExecuteScriptAsync("for (let i = 0; i < 2; i++) document.getElementById('menuContainer').children[i].style.display = 'none';");
            browser.ExecuteScriptAsync("for (let i = 4; i < 9; i++) document.getElementById('menuContainer').children[i].style.visibility = 'hidden';");
        }
    }
}
