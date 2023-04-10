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
using System;
using System.Drawing;

namespace BetHelper {
    public class PopUpEventArgs : EventArgs {

        public IWebBrowser BrowserControl { get; private set; }

        public IBrowser Browser { get; private set; }

        public IFrame Frame { get; private set; }

        public string TargetUrl { get; private set; }

        public string TargetFrameName { get; private set; }

        public WindowOpenDisposition TargetDisposition { get; private set; }

        public bool UserGesture { get; private set; }

        public IPopupFeatures PopupFeatures { get; private set; }

        public IWindowInfo WindowInfo { get; private set; }

        public IBrowserSettings BrowserSettings { get; private set; }

        public bool NoJavascriptAccess { get; private set; }

        public Point Location { get; set; }

        public Size Size { get; set; }

        public PopUpEventArgs(
                IWebBrowser browserControl,
                IBrowser browser,
                IFrame frame,
                string targetUrl,
                string targetFrameName,
                WindowOpenDisposition targetDisposition,
                bool userGesture,
                IPopupFeatures popupFeatures,
                IWindowInfo windowInfo,
                IBrowserSettings browserSettings,
                bool noJavascriptAccess) {

            BrowserControl = browserControl;
            Browser = browser;
            Frame = frame;
            TargetUrl = targetUrl;
            TargetFrameName = targetFrameName;
            TargetDisposition = targetDisposition;
            UserGesture = userGesture;
            PopupFeatures = popupFeatures;
            WindowInfo = windowInfo;
            BrowserSettings = browserSettings;
            NoJavascriptAccess = noJavascriptAccess;
        }
    }
}
