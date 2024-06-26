﻿/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.7
 */

using CefSharp;
using CefSharp.WinForms;
using System.Text;

namespace BetHelper {
    public class WebInfo11 : WebInfo {

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('seoAdWrapper')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('seoAdWrapper')[0].style.display = 'none';");
            }
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("for (let i = 0; i < document.getElementsByClassName('adsenvelope').length; i++) ")
                .Append("document.getElementsByClassName('adsenvelope')[i].style.display = 'none';")
                .ToString());

            browser.ExecuteScriptAsync("document.body.classList.remove('background-add-on');");
            browser.ExecuteScriptAsync(new StringBuilder()
                .Append("document.querySelectorAll('[id^=\"box-over-content\"]').forEach(function(element) ")
                .Append("{ element.style.display = 'none'; });")
                .ToString());
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
