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
using System;

namespace BetHelper {
    public class LifeSpanHandler : ILifeSpanHandler {
        public event EventHandler<PopUpEventArgs> BrowserPopUp;

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
                string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
                IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {

            newBrowser = null;
            PopUpEventArgs popUpEventArgs = new PopUpEventArgs(browserControl, browser, frame, targetUrl, targetFrameName,
                targetDisposition, userGesture, popupFeatures, windowInfo, browserSettings, noJavascriptAccess);
            BrowserPopUp?.Invoke(this, popUpEventArgs);
            return true;
        }

        public virtual bool DoClose(IWebBrowser browserControl, IBrowser browser) => WebInfo.IsAllowedUrl(browserControl.Address);

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser) { }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser) { }
    }
}
