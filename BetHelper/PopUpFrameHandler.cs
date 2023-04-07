/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class PopUpFrameHandler {
        private int closeCount;
        private long frameIdentifier0, frameIdentifier1, frameIdentifier2, frameIdentifier3;
        private string frameUrl;

        public event EventHandler Close;

        public PopUpFrameHandler(ChromiumWebBrowser browser, FrameHandler frameHandler, LoadHandler loadHandler, Settings settings) {
            Browser = browser;
            FrameHandler = frameHandler;
            LoadHandler = loadHandler;
            browser.FrameHandler = frameHandler;
            browser.LoadHandler = loadHandler;
            Browser.AddressChanged += new EventHandler<AddressChangedEventArgs>(OnAddressChanged);

            FrameHandler.FrameCreated += new EventHandler<FrameEventArgs>((sender, e) => {
                StringBuilder stringBuilder = new StringBuilder();
                if (frameIdentifier0 == 0) {
                    frameIdentifier0 = e.Identifier;
                    if (settings.LogPopUpFrameHandler) {
                        stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 0, e.Identifier));
                    }
                }
                frameIdentifier2 = 0;
                if (settings.LogPopUpFrameHandler) {
                    if (stringBuilder.Length > 0) {
                        stringBuilder.Append(Constants.Semicolon);
                        stringBuilder.Append(Constants.Space);
                    }
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 2, 0));
                    Log(stringBuilder.ToString());
                }
            });
            LoadHandler.FrameLoadEnd += new EventHandler<LoadEventArgs>((sender, e) => {
                StringBuilder stringBuilder = new StringBuilder();
                frameIdentifier0 = 0;
                frameIdentifier2 = 0;
                if (settings.LogPopUpFrameHandler) {
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 0, 0));
                    stringBuilder.Append(Constants.Semicolon);
                    stringBuilder.Append(Constants.Space);
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 2, 0));
                    Log(stringBuilder.ToString());
                }
            });
            LoadHandler.FrameLoadStart += new EventHandler<LoadEventArgs>((sender, e) => {
                StringBuilder stringBuilder = new StringBuilder();
                frameIdentifier0 = 0;
                if (settings.LogPopUpFrameHandler) {
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 0, 0));
                }
                if (frameIdentifier2 == e.Identifier) {
                    frameIdentifier3 = e.Identifier;
                    if (settings.LogPopUpFrameHandler) {
                        if (stringBuilder.Length > 0) {
                            stringBuilder.Append(Constants.Semicolon);
                            stringBuilder.Append(Constants.Space);
                        }
                        stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 3, e.Identifier));
                    }
                }
                frameIdentifier2 = 0;
                if (settings.LogPopUpFrameHandler) {
                    stringBuilder.Append(Constants.Semicolon);
                    stringBuilder.Append(Constants.Space);
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 2, 0));
                    Log(stringBuilder.ToString());
                }
            });
            LoadHandler.LoadingStateChange += new EventHandler<LoadEventArgs>((sender, e) => {
                StringBuilder stringBuilder = new StringBuilder();
                if (frameIdentifier1 == e.Identifier) {
                    frameIdentifier2 = e.Identifier;
                    if (settings.LogPopUpFrameHandler) {
                        stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 2, e.Identifier));
                    }
                }
                if (frameIdentifier0 == e.Identifier) {
                    frameIdentifier1 = e.Identifier;
                    if (settings.LogPopUpFrameHandler) {
                        if (stringBuilder.Length > 0) {
                            stringBuilder.Append(Constants.Semicolon);
                            stringBuilder.Append(Constants.Space);
                        }
                        stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 1, e.Identifier));
                    }
                }
                frameIdentifier0 = 0;
                if (settings.LogPopUpFrameHandler) {
                    if (stringBuilder.Length > 0) {
                        stringBuilder.Append(Constants.Semicolon);
                        stringBuilder.Append(Constants.Space);
                    }
                    stringBuilder.Append(string.Format(Constants.PopUpFrameHandlerLogFormat, 0, 0));
                    Log(stringBuilder.ToString());
                }
            });
        }

        public ChromiumWebBrowser Browser { get; private set; }
        public FrameHandler FrameHandler { get; private set; }
        public LoadHandler LoadHandler { get; private set; }
        public string NewUrl { get; private set; }

        private void OnAddressChanged(object sender, AddressChangedEventArgs e) {
            if (frameIdentifier3 == e.Browser.FocusedFrame.Identifier && !EqualsLeftPart(frameUrl, e.Address)) {
                NewUrl = e.Address;
                if (closeCount++ < 1) {
                    Close?.Invoke(this, e);
                } else {
                    closeCount = 1;
                }
            }
            if (frameIdentifier1 == e.Browser.FocusedFrame.Identifier) {
                frameUrl = e.Address;
            }
            frameIdentifier0 = 0;
        }

        private static bool EqualsLeftPart(string url1, string url2) {
            if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url1);
                Uri uri2 = new Uri(url2);
                return string.Compare(uri1.GetLeftPart(UriPartial.Path), uri2.GetLeftPart(UriPartial.Path), StringComparison.OrdinalIgnoreCase) == 0;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        private static void Log(string message) {
            try {
                using (StreamWriter streamWriter = File.AppendText(Path.Combine(Application.LocalUserAppDataPath, Constants.PopUpFrameHandlerLogFileName))) {
                    StringBuilder stringBuilder = new StringBuilder(DateTime.Now.ToString(Constants.ErrorLogTimeFormat));
                    stringBuilder.Append(Constants.VerticalTab);
                    stringBuilder.Append(message);
                    streamWriter.WriteLine(stringBuilder.ToString());
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}
