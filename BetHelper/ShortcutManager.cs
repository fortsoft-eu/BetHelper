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

using FortSoft.Tools;
using NonInvasiveKeyboardHookLibrary;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BetHelper {
    public sealed class ShortcutManager : IDisposable {
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
        public event EventHandler OpenWebInfo;
        public event EventHandler Print;
        public event EventHandler PrintImage;
        public event EventHandler PrintToPdf;
        public event EventHandler Reload;
        public event EventHandler ReloadIgnoreCache;
        public event EventHandler ResetIpCheckLock;
        public event EventHandler ShowPreferences;
        public event EventHandler StopLoad;
        public event EventHandler StopRinging;
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

        private void Register() {
            if (registered++ > 1) {
                return;
            }
            registered = 1;

            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_BACK,
                new Action(() => GoBack?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_FORWARD,
                new Action(() => GoForward?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_HOME,
                new Action(() => GoHome?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_REFRESH,
                new Action(() => Reload?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_SEARCH,
                new Action(() => Find?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.BROWSER_STOP,
                new Action(() => StopLoad?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.ESCAPE,
                new Action(() => Escape?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F1,
                new Action(() => OpenHelp?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F11,
                new Action(() => TurnOffMonitors?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F12,
                new Action(() => OpenDeveloperTools?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F2,
                new Action(() => FocusTrustDegrees?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F3,
                new Action(() => FindNext?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F4,
                new Action(() => UnloadPage?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F5,
                new Action(() => Reload?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F6,
                new Action(() => StopRinging?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F7,
                new Action(() => ResetIpCheckLock?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F8,
                new Action(() => LogIn?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey((int)VirtualKeyCode.F9,
                new Action(() => ToggleRightPane?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_E,
                new Action(() => ExportWindowAsync?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_P,
                new Action(() => PrintImage?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F10,
                new Action(() => OpenBetCalculator?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F11,
                new Action(() => LaunchCalculator?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F12,
                new Action(() => LaunchNotepad?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F8,
                new Action(() => LogInAtInitialPage?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.F9,
                new Action(() => ToggleRightPanetWidth?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.HOME,
                new Action(() => GoHome?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.KEY_L,
                new Action(() => OpenLogViewer?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.LEFT,
                new Action(() => GoBack?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Alt, (int)VirtualKeyCode.RIGHT,
                new Action(() => GoForward?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.ADD,
                new Action(() => ZoomInCoarse?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.DELETE,
                new Action(() => ClearBrowserCacheInclUserData?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_N,
                new Action(() => OpenEncoderDecoder?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_P,
                new Action(() => PrintToPdf?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.KEY_T,
                new Action(() => ToggleTopMost?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.OEM_MINUS,
                new Action(() => ZoomOutCoarse?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.OEM_PLUS,
                new Action(() => ZoomInCoarse?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control | ModifierKeys.Shift, (int)VirtualKeyCode.SUBTRACT,
                new Action(() => ZoomOutCoarse?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.ADD,
                new Action(() => ZoomInFine?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.BROWSER_REFRESH,
                new Action(() => ReloadIgnoreCache?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.DOWN,
                new Action(() => CtrlDown?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.END,
                new Action(() => CtrlEnd?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.F5,
                new Action(() => ReloadIgnoreCache?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.HOME,
                new Action(() => CtrlHome?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_0,
                new Action(() => ActualSize?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_D,
                new Action(() => AddBookmark?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_E,
                new Action(() => Export?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_F,
                new Action(() => Find?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_G,
                new Action(() => ShowPreferences?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_I,
                new Action(() => OpenWebInfo?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_M,
                new Action(() => MuteAudio?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_O,
                new Action(() => Open?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_P,
                new Action(() => Print?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_R,
                new Action(() => Reload?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_T,
                new Action(() => NewTabBetCalculator?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.KEY_U,
                new Action(() => ViewSource?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.NEXT,
                new Action(() => CtrlPageDown?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.NUMPAD0,
                new Action(() => ActualSize?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.OEM_MINUS,
                new Action(() => ZoomOutFine?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.OEM_PLUS,
                new Action(() => ZoomInFine?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.PRIOR,
                new Action(() => CtrlPageUp?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.SUBTRACT,
                new Action(() => ZoomOutFine?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, (int)VirtualKeyCode.UP,
                new Action(() => CtrlUp?.Invoke(this, EventArgs.Empty)));
            keyboardHookManager.RegisterHotkey(ModifierKeys.Shift, (int)VirtualKeyCode.F3,
                new Action(() => FindPrevious?.Invoke(this, EventArgs.Empty)));
        }

        public void AddForm(Form form) {
            form.Activated += new EventHandler(Register);
            form.Deactivate += new EventHandler(Unregister);
            form.Enter += new EventHandler(Register);
            form.Leave += new EventHandler(Unregister);
        }

        public void Dispose() {
            if (keyboardHookManager != null) {
                keyboardHookManager.UnregisterAll();
                keyboardHookManager.Stop();
                keyboardHookManager = null;
            }
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
