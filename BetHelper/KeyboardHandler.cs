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
 * Version 1.1.9.0
 */

using CefSharp;
using System;

namespace BetHelper {
    public class KeyboardHandler : IKeyboardHandler {
        public event EventHandler<KeyboardEventArgs> KeyEvent;

        public bool OnKeyEvent(
                IWebBrowser browserControl,
                IBrowser browser,
                KeyType type,
                int windowsKeyCode,
                int nativeKeyCode,
                CefEventFlags modifiers,
                bool isSystemKey) {

            KeyEvent?.Invoke(this,
                new KeyboardEventArgs(browserControl, browser, type, windowsKeyCode, nativeKeyCode, modifiers, isSystemKey));
            return false;
        }

        public bool OnPreKeyEvent(
                IWebBrowser browserControl,
                IBrowser browser,
                KeyType type,
                int windowsKeyCode,
                int nativeKeyCode,
                CefEventFlags modifiers,
                bool isSystemKey,
                ref bool isKeyboardShortcut) {

            return false;
        }
    }
}
