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
 * Version 1.1.14.0
 */

using FortSoft.Tools;
using NonInvasiveKeyboardHookLibrary;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BetHelper {
    public sealed class ShortcutManager : IDisposable {
        private Form form;
        private int registered;
        private KeyboardHookManager keyboardHookManager;

        public event EventHandler ActualSize;
        public event EventHandler AddBookmark;
        public event EventHandler ClearBrowserCacheInclUserData;
        public event EventHandler CtrlDown;
        public event EventHandler CtrlEnd;
        public event EventHandler CtrlHome;
        public event EventHandler CtrlPageDown;
        public event EventHandler CtrlPageUp;
        public event EventHandler CtrlUp;
        public event EventHandler Escape;
        public event EventHandler Export;
        public event EventHandler ExportWindowAsync;
        public event EventHandler Find;
        public event EventHandler FindNext;
        public event EventHandler FindPrevious;
        public event EventHandler FocusTrustDegrees;
        public event EventHandler GoBack;
        public event EventHandler GoForward;
        public event EventHandler GoHome;
        public event EventHandler LaunchCalculator;
        public event EventHandler LaunchNotepad;
        public event EventHandler LogIn;
        public event EventHandler LogInAtInitialPage;
        public event EventHandler MuteAudio;
        public event EventHandler NewTabBetCalculator;
        public event EventHandler Open;
        public event EventHandler OpenBetCalculator;
        public event EventHandler OpenDeveloperTools;
        public event EventHandler OpenEncoderDecoder;
        public event EventHandler OpenHelp;
        public event EventHandler OpenLogViewer;
        public event EventHandler ShowWebInfo;
        public event EventHandler Shortcut;
        public event EventHandler Print;
        public event EventHandler PrintImage;
        public event EventHandler PrintToPdf;
        public event EventHandler Reload;
        public event EventHandler ReloadIgnoreCache;
        public event EventHandler ResetIpCheckLock;
        public event EventHandler ShowPreferences;
        public event EventHandler StopLoad;
        public event EventHandler StopRinging;
        public event EventHandler TestBell;
        public event EventHandler ToggleRightPane;
        public event EventHandler ToggleRightPanetWidth;
        public event EventHandler ToggleTopMost;
        public event EventHandler TurnOffMonitors;
        public event EventHandler UnloadPage;
        public event EventHandler ViewSource;
        public event EventHandler ZoomInCoarse;
        public event EventHandler ZoomInFine;
        public event EventHandler ZoomOutCoarse;
        public event EventHandler ZoomOutFine;

        public ShortcutManager() {
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
        }

        public void AddForm(Form form) {
            if (this.form == null) {
                this.form = form;
                this.form.Activated += new EventHandler(Register);
                this.form.Deactivate += new EventHandler(Unregister);
                this.form.Enter += new EventHandler(Register);
                this.form.Leave += new EventHandler(Unregister);
            } else {
                throw new ApplicationException();
            }
        }

        public void Dispose() {
            if (keyboardHookManager != null) {
                keyboardHookManager.UnregisterAll();
                keyboardHookManager.Stop();
                keyboardHookManager = null;
            }
        }

        private void Register() {
            if (registered++ > 0) {
                return;
            }
            registered = 1;

            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_BACK,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoBack?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_FORWARD,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoForward?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_HOME,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoHome?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_REFRESH,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Reload?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_SEARCH,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Find?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_STOP,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        StopLoad?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.ESCAPE,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Escape?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F1,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        OpenHelp?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F11,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        TurnOffMonitors?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F12,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        OpenDeveloperTools?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F2,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        FocusTrustDegrees?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F3,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        FindNext?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F4,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        UnloadPage?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F5,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Reload?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F7,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        StopRinging?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F8,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        LogIn?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F9,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ToggleRightPane?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_E,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ExportWindowAsync?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_P,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        PrintImage?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F10,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        OpenBetCalculator?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F11,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        LaunchCalculator?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F12,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        LaunchNotepad?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F6,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ResetIpCheckLock?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F7,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        TestBell?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F8,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        LogInAtInitialPage?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F9,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ToggleRightPanetWidth?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.HOME,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoHome?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.KEY_L,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        OpenLogViewer?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.LEFT,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoBack?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.RIGHT,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        GoForward?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.ADD,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomInCoarse?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.DELETE,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ClearBrowserCacheInclUserData?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_N,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        OpenEncoderDecoder?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_P,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        PrintToPdf?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_T,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ToggleTopMost?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.OEM_MINUS,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomOutCoarse?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.OEM_PLUS,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomInCoarse?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.SUBTRACT,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomOutCoarse?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.ADD,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomInFine?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.BROWSER_REFRESH,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ReloadIgnoreCache?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.DOWN,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlDown?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.END,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlEnd?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.F5,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ReloadIgnoreCache?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.HOME,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlHome?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_0,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ActualSize?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_D,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        AddBookmark?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_E,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Export?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_F,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Find?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_G,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ShowPreferences?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_I,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ShowWebInfo?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_M,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        MuteAudio?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_O,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Open?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_P,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Print?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_R,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        Reload?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_T,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        NewTabBetCalculator?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_U,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ViewSource?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.NEXT,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlPageDown?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.NUMPAD0,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ActualSize?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.OEM_MINUS,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomOutFine?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.OEM_PLUS,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomInFine?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.PRIOR,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlPageUp?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.SUBTRACT,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        ZoomOutFine?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.UP,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        CtrlUp?.Invoke(this, EventArgs.Empty);
                    }
                }));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Shift, (int)VirtualKeyCode.F3,
                new Action(() => {
                    if (!form.WindowState.Equals(FormWindowState.Minimized)) {
                        Shortcut?.Invoke(this, EventArgs.Empty);
                        FindPrevious?.Invoke(this, EventArgs.Empty);
                    }
                }));
        }

        private void Register(object sender, EventArgs e) {
            try {
                Register();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
        }

        private void Unregister() {
            if (keyboardHookManager != null) {
                keyboardHookManager.UnregisterAll();
                registered = 0;
            }
        }

        private void Unregister(object sender, EventArgs e) {
            try {
                Unregister();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}
