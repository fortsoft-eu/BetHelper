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
 * Version 1.1.17.0
 */

using CefSharp;
using CefSharp.WinForms;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class WebInfo19 : WebInfo {

        protected override void LogIn(ChromiumWebBrowser browser) {
            OnStarted(8);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('cmplz-accept')[0]", false)) {
                OnProgress(Properties.Resources.MessageClosingCookieConsent);
                browser.ExecuteScriptAsync("document.getElementsByClassName('cmplz-accept')[0].click();");
                Wait(browser);
                Sleep(30);
            }

            if (IsLoggedIn()) {
                OnFinished();
                return;
            }

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingUserNameBox);
                browser.ExecuteScriptAsync("document.getElementById('user_login').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('user_login')")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(50);

                OnProgress(Properties.Resources.MessageSendingUserName);
                SendString(browser, UserName);
            } while (!GetValueById("user_login").Equals(UserName));

            SendKey(browser, Keys.Tab);

            do {
                Sleep(50);
                OnProgress(Properties.Resources.MessageClearingPasswordBox);
                browser.ExecuteScriptAsync("document.getElementById('user_pass').value = '';");
                Wait(browser);
                Sleep(50);

                OnProgress(Properties.Resources.MessageSettingInputFocus);
                if (!ClickElement(browser, "document.getElementById('user_pass')")) {
                    OnError(Properties.Resources.MessageLogInCannotSetFocusError);
                    return;
                }
                Sleep(50);

                OnProgress(Properties.Resources.MessageSendingPassword);
                SendString(browser, Password);
            } while (!GetValueById("user_pass").Equals(Password));

            OnProgress(Properties.Resources.MessageLoggingIn);
            browser.ExecuteScriptAsync("document.getElementsByClassName('sazkyPreLogin')[0].click();");
            Wait(browser);
            Sleep(2000);
            if (!ElementExistsAndVisible(browser, "document.getElementsByClassName('sazkyHeaderAccountName')[0]", true)) {
                OnError();
                return;
            }

            OnFinished();
        }

        protected override void NoLogIn(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('mediad')[0]", false)) {
                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("Array.from(document.getElementsByClassName('mediad')).forEach(function(element, index, array)")
                    .Append("{ element.style.display = 'none'; });")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('footerNewsletterWrap')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('footerNewsletterWrap').style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('newsletterButton')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('newsletterButton').style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('wpadminbar')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('wpadminbar').style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.querySelector('[id^=\"brave_popup_\"]')", false)) {
                browser.ExecuteScriptAsync("document.querySelector('[id^=\"brave_popup_\"]').remove();");
            }
        }

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('mediad')[0]", false)) {
                browser.ExecuteScriptAsync(new StringBuilder()
                    .Append("Array.from(document.getElementsByClassName('mediad')).forEach(function(element, index, array)")
                    .Append("{ element.style.display = 'none'; });")
                    .ToString());
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('footerNewsletterWrap')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('footerNewsletterWrap').style.display = 'none';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('newsletterButton')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('newsletterButton').style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.getElementById('wpadminbar')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('wpadminbar').style.visibility = 'hidden';");
            }
            if (ElementExistsAndVisible(browser, "document.querySelector('[id^=\"brave_popup_\"]')", false)) {
                browser.ExecuteScriptAsync("document.querySelector('[id^=\"brave_popup_\"]').remove();");
            }
        }
    }
}
