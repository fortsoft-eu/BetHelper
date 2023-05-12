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

namespace NonInvasiveKeyboardHookLibrary {
    internal class KeybindStruct : IEquatable<KeybindStruct> {
        internal readonly int VirtualKeyCode;
        internal readonly List<ModifierKeys> Modifiers;
        internal readonly Guid? Identifier;

        internal KeybindStruct(IEnumerable<ModifierKeys> modifiers, int virtualKeyCode, Guid? identifier = null) {
            VirtualKeyCode = virtualKeyCode;
            Modifiers = new List<ModifierKeys>(modifiers);
            Identifier = identifier;
        }

        public bool Equals(KeybindStruct other) {
            if (other == null || VirtualKeyCode != other.VirtualKeyCode || Modifiers.Count != other.Modifiers.Count) {
                return false;
            }
            foreach (ModifierKeys modifier in this.Modifiers) {
                if (!other.Modifiers.Contains(modifier)) {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (this == obj) {
                return true;
            }
            return obj.GetType().Equals(GetType()) && Equals((KeybindStruct)obj);
        }

        public override int GetHashCode() {
            int num1 = 13 * 7 + VirtualKeyCode.GetHashCode();
            int num2 = 0;
            foreach (ModifierKeys modifier in Modifiers) {
                num2 += modifier.GetHashCode();
            }
            return num1 * 7 + num2;
        }
    }
}
