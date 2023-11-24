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

namespace BetHelper {
    public class WebInfo21 : WebInfo {

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser,
                    "document.getElementById('wrapper-footer').children[0].children[0].children[1]", false)) {

                browser.ExecuteScriptAsync(
                    "document.getElementById('wrapper-footer').children[0].children[0].children[1].style.display = 'none';");
            }

            if (ElementExistsAndVisible(browser, "document.getElementById('wrapper-navbar').children[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementById('wrapper-navbar').children[0].remove();");
            }

            Wait(browser);

            Sleep(1200);

            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('gdpr-fbo-0')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('gdpr-fbo-0')[0].click();");
            }
        }
    }
}
