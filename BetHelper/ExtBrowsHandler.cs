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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BetHelper {
    public class ExtBrowsHandler {
        private Dictionary<Browser, string> executables;

        public ExtBrowsHandler() {
            executables = new Dictionary<Browser, string>();
        }

        public bool Exists(Browser browser) {
            if (!executables.ContainsKey(browser)) {
                executables.Add(browser, GetExecutablePath(browser));
            }
            return executables[browser] != null;
        }

        public void Open(Browser browser, string url) {
            if (!executables.ContainsKey(browser)) {
                executables.Add(browser, GetExecutablePath(browser));
            }
            Process.Start(StaticMethods.EscapeArgument(executables[browser]), StaticMethods.EscapeArgument(url));
        }

        private static string GetExecutablePath(Browser browser) {
            string execRelPath, lnkFileName;
            switch (browser) {
                case Browser.GoogleChrome:
                    execRelPath = Constants.ExtBrowsChromeExecRelPath;
                    lnkFileName = Constants.ExtBrowsChromeLnkFileName;
                    break;
                case Browser.Firefox:
                    execRelPath = Constants.ExtBrowsFirefoxExecRelPath;
                    lnkFileName = Constants.ExtBrowsFirefoxLnkFileName;
                    break;
                default:
                    throw new NotImplementedException();
            }
            try {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), lnkFileName);
                if (File.Exists(shortcutPath)) {
                    return shortcutPath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), lnkFileName);
                if (File.Exists(shortcutPath)) {
                    return shortcutPath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), execRelPath);
                if (File.Exists(shortcutPath)) {
                    return shortcutPath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), execRelPath);
                if (File.Exists(shortcutPath)) {
                    return shortcutPath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return null;
        }

        public enum Browser {
            Firefox, GoogleChrome
        }
    }
}
