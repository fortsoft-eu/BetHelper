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
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo03 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(11);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('uk-button sticky-notification__cta')[1]", false)) {
                OnProgress(Properties.Resources.MessageClosingAdvertisement);
                browser.ExecuteScriptAsync("document.getElementsByClassName('uk-button sticky-notification__cta')[1].click();");
                Wait(browser);
                Sleep(30);
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-modal__close__btn')[0]", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementsByClassName('sb-modal__close__btn')[0].click();");
                Wait(browser);
                Sleep(30);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            Reload(browser);
            OnProgress(Properties.Resources.MessageReloading);
            Wait(browser);
            Sleep(1000);

            if (!ElementExistsAndVisible(browser, "document.getElementsByClassName('GTM-login')[0]", true)) {
                OnError();
                return;
            }
            Sleep(30);

            OnProgress(Properties.Resources.MessageDisplayingLoginBlock);
            browser.ExecuteScriptAsync("document.getElementsByClassName('GTM-login')[0].click();");
            Wait(browser);
            Sleep(1000);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync(
                    "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('username').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (ElementExists(browser,
                        "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('username')",
                        true)) {

                    browser.ExecuteScriptAsync(
                        "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('username').focus();");
                    Sleep(250);
                } else {
                    OnError();
                    return;
                }

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueById("username", "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document")
                .Equals(UserName));

            Wait(browser);
            Sleep(200);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync(
                    "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('password').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (ElementExists(browser,
                        "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('password')",
                        true)) {

                    browser.ExecuteScriptAsync(
                        "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document.getElementById('password').focus();");
                    Sleep(250);
                } else {
                    OnError();
                    return;
                }

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("password", "document.getElementById('iframe-modal').children[0].children[0].contentWindow.document")
                .Equals(Password));

            SendKey(browser, Keys.Tab);
            OnProgress(Properties.Resources.MessageLoggingIn);
            SendKey(browser, Keys.Space);
            Wait(browser);
            Sleep(800);

            if (ElementExists(browser, "document.getElementsByClassName('sb-header__user-verification-banner')[0]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('sb-header__user-verification-banner')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-header__timers')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sb-header__timers')[0].remove();");
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('uk-button sticky-notification__cta')[1]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('uk-button sticky-notification__cta')[1].click();");
                Wait(browser);
                Sleep(30);
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-modal__close__btn')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sb-modal__close__btn')[0].click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-header__user-verification-banner')[0]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('sb-header__user-verification-banner')[0].style.display = 'none';");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExists(browser, "document.getElementById('notifications-slider')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('notifications-slider').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementsByClassName('sb-header__user-verification-banner')[0]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('sb-header__user-verification-banner')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-header__header__navigation')[0]", false)) {
                browser.ExecuteScriptAsync(
                    "document.getElementsByClassName('sb-header__header__navigation')[0].children[1].children[0].children[2].remove();");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 0; i < document.getElementsByClassName('casino-banners').length; i++)")
                .Append(" document.getElementsByClassName('casino-banners')[i].style.display = 'none';")
                .ToString());

            if (ElementExists(browser, "document.getElementsByClassName('mini-banners')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('mini-banners')[0].style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementsByClassName('offers__container')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('offers__container')[0].style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementsByClassName('bet-mentor-widget')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('bet-mentor-widget')[0].style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('bet-mentor-modal')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('bet-mentor-modal').style.display = 'none';");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("var jipq4Cx7 = document.getElementsByClassName('sport-picker')[0].children.length > 8 ? 1 : 0;")
                .Append("for (let i = 1; i <= 4; i++) {")
                .Append("    var eFs57Wq3 = document.getElementsByClassName('sport-picker')[0].children[i + jipq4Cx7];")
                .Append("    eFs57Wq3.style.visibility = 'hidden';")
                .Append("    eFs57Wq3.style.height = 0; }")
                .ToString());

            if (ElementExists(browser, "document.getElementsByClassName('story-groups-wrapper')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('story-groups-wrapper')[0].style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-sponsorships')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sb-sponsorships')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('GTM-footer')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('GTM-footer')[0].children[1].remove();");
            }
            Wait(browser);
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('sb-header__timers')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('sb-header__timers')[0].remove();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('top-notification')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('top-notification')[0].remove();");
            }
        }
    }
}
