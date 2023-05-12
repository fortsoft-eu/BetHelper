/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
 * Copyright (c) 2022 Guilherme Ribeiro [Version 2.1.2]
 * Copyright (c) 2018-2021 Kfir Eichenblat [Version 2.1.1]
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
 * Version 2.1.3.0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace NonInvasiveKeyboardHookLibrary {
    internal class KeyboardHookManager {
        private readonly Dictionary<KeybindStruct, Action> _registeredCallbacks;
        private readonly HashSet<ModifierKeys> _downModifierKeys;
        private readonly HashSet<int> _downKeys;
        private readonly object _modifiersLock = new object();
        private LowLevelKeyboardProc _hook;
        private bool _isStarted;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 256;
        private const int WM_KEYUP = 257;
        private const int WM_SYSKEYDOWN = 260;
        private const int WM_SYSKEYUP = 261;

        private static IntPtr _hookId = IntPtr.Zero;

        internal KeyboardHookManager() {
            _registeredCallbacks = new Dictionary<KeybindStruct, Action>();
            _downModifierKeys = new HashSet<ModifierKeys>();
            _downKeys = new HashSet<int>();
        }

        internal void Start() {
            if (_isStarted) {
                return;
            }
            _hook = new LowLevelKeyboardProc(HookCallback);
            _hookId = SetHook(_hook);
            _isStarted = true;
        }

        internal void Stop() {
            if (!_isStarted) {
                return;
            }
            UnhookWindowsHookEx(_hookId);
            _isStarted = false;
        }

        internal Guid RegisterHotkey(int virtualKeyCode, Action action) => RegisterHotkey(new ModifierKeys[0], virtualKeyCode, action);

        internal Guid RegisterHotkey(ModifierKeys modifiers, int virtualKeyCode, Action action) {
            return RegisterHotkey(((IEnumerable<ModifierKeys>)Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>().ToArray())
                .Where(new Func<ModifierKeys, bool>(modifier => modifiers.HasFlag((Enum)modifier)))
                .ToArray(), virtualKeyCode, action);
        }

        internal Guid RegisterHotkey(ModifierKeys[] modifiers, int virtualKeyCode, Action action) {
            Guid guid = Guid.NewGuid();
            KeybindStruct key = new KeybindStruct(modifiers, virtualKeyCode, new Guid?(guid));
            if (_registeredCallbacks.ContainsKey(key)) {
                throw new HotkeyAlreadyRegisteredException();
            }
            _registeredCallbacks[key] = action;
            return guid;
        }

        internal void UnregisterAll() => _registeredCallbacks.Clear();

        internal void UnregisterHotkey(int virtualKeyCode) => UnregisterHotkey(new ModifierKeys[0], virtualKeyCode);

        internal void UnregisterHotkey(ModifierKeys[] modifiers, int virtualKeyCode) {
            if (!_registeredCallbacks.Remove(new KeybindStruct(modifiers, virtualKeyCode))) {
                throw new HotkeyNotRegisteredException();
            }
        }

        internal void UnregisterHotkey(Guid keybindIdentity) {
            KeybindStruct key = _registeredCallbacks.Keys.FirstOrDefault(new Func<KeybindStruct, bool>(
                keybind => keybind.Identifier.HasValue && keybind.Identifier.Value.Equals(keybindIdentity)));
            if (key == null || !_registeredCallbacks.Remove(key)) {
                throw new HotkeyNotRegisteredException();
            }
        }

        private void HandleKeyPress(int virtualKeyCode) {
            KeybindStruct key = new KeybindStruct(_downModifierKeys, virtualKeyCode);
            Action action;
            if (!_registeredCallbacks.ContainsKey(key) || !_registeredCallbacks.TryGetValue(key, out action)) {
                return;
            }
            action();
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc) {
            IntPtr hMod = LoadLibrary("User32");
            return SetWindowsHookEx(13, proc, hMod, 0U);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                int vkCode = Marshal.ReadInt32(lParam);
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandleSingleKeyboardInput), new KeyboardParams(wParam, vkCode));
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private void HandleSingleKeyboardInput(object keyboardParamsObj) {
            KeyboardParams keyboardParams = (KeyboardParams)keyboardParamsObj;
            IntPtr wParam = keyboardParams.wParam;
            int vkCode = keyboardParams.vkCode;
            ModifierKeys? modifierKeyFromCode = ModifierKeysUtilities.GetModifierKeyFromCode(vkCode);
            if (wParam == (IntPtr)256 || wParam == (IntPtr)260) {
                if (modifierKeyFromCode.HasValue) {
                    lock (_modifiersLock) {
                        _downModifierKeys.Add(modifierKeyFromCode.Value);
                    }
                }
                if (!_downKeys.Contains(vkCode)) {
                    HandleKeyPress(vkCode);
                    _downKeys.Add(vkCode);
                }
            }
            if (wParam != (IntPtr)257 && wParam != (IntPtr)261) {
                return;
            }
            if (modifierKeyFromCode.HasValue) {
                lock (_modifiersLock) {
                    _downModifierKeys.Remove(modifierKeyFromCode.Value);
                }
            }
            _downKeys.Remove(vkCode);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }

    internal class HotkeyAlreadyRegisteredException : NonInvasiveKeyboardHookException { }

    internal class HotkeyNotRegisteredException : NonInvasiveKeyboardHookException { }

    internal class NonInvasiveKeyboardHookException : Exception { }
}
