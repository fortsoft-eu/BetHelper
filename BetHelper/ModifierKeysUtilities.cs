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

namespace NonInvasiveKeyboardHookLibrary {
    internal static class ModifierKeysUtilities {
        internal static ModifierKeys? GetModifierKeyFromCode(int keyCode) {
            switch (keyCode) {
                case 16:
                case 160:
                case 161:
                    return new ModifierKeys?(ModifierKeys.Shift);
                case 17:
                case 162:
                case 163:
                    return new ModifierKeys?(ModifierKeys.Control);
                case 18:
                case 164:
                case 165:
                    return new ModifierKeys?(ModifierKeys.Alt);
                case 91:
                case 92:
                    return new ModifierKeys?(ModifierKeys.WindowsKey);
                default:
                    return new ModifierKeys?();
            }
        }
    }

    [Flags]
    internal enum ModifierKeys {
        Alt = 1,
        Control = 2,
        Shift = 4,
        WindowsKey = 8,
    }
}
