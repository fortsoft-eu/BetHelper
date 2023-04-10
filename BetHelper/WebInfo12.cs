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

namespace BetHelper {
    public class WebInfo12 : WebInfo {

        public override void HeartBeat(ChromiumWebBrowser browser) {
            if (ElementExistsAndVisible(browser, "document.getElementById('onetrust-accept-btn-handler')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('onetrust-accept-btn-handler').click();");
            }
            if (ElementExistsAndVisible(browser, "document.getElementsByClassName('Xm')[0]", false)) {
                browser.ExecuteScriptAsync("document.getElementsByClassName('Xm')[0].click();");
            }
            if (ElementExists(browser, "document.getElementById('header-ads-holder')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('header-ads-holder').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('right-column-top-ad-holder')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('right-column-top-ad-holder').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('right-column-middle-ad-holder')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('right-column-middle-ad-holder').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('right-column-sticky-ad-holder')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('right-column-sticky-ad-holder').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('content-sticky-footer')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('content-sticky-footer').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('gad-news-featured-wrapper')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('gad-news-featured-wrapper').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('gad-news-featured-wrapper')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('gad-news-featured-wrapper').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('gad-news-top-stories-wrapper')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('gad-news-top-stories-wrapper').style.display = 'none';");
            }
            for (int i = 1; i <= 100; i++) {
                if (ElementExists(browser, string.Format("document.getElementById('gad-news-list-item-{0}-wrapper')", i), false)) {
                    browser.ExecuteScriptAsync(string.Format(
                        "document.getElementById('gad-news-list-item-{0}-wrapper').style.display = 'none';", i));
                }
                if (ElementExists(browser, string.Format("document.getElementById('ad-holder-gad-news-article-item-{0}')", i), false)) {
                    browser.ExecuteScriptAsync(string.Format(
                        "document.getElementById('ad-holder-gad-news-article-item-{0}').style.display = 'none';", i));
                }
            }
            if (ElementExists(browser, "document.getElementById('gad-right-small-wrapper')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('gad-right-small-wrapper').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('ad-holder-gad-news-article-item')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('ad-holder-gad-news-article-item').style.display = 'none';");
            }
            if (ElementExists(browser, "document.getElementById('Mev_Mpu-wrapper')", false)) {
                browser.ExecuteScriptAsync("document.getElementById('Mev_Mpu-wrapper').style.display = 'none';");
            }
        }
    }
}
