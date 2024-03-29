﻿/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
 * Copyright (c) 2011 Mike Zhang - MSDN Community Support
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

using System.Runtime.InteropServices;

namespace FortSoft.Controls {

    /// <summary>
    /// Implements custom TabControl with only 1px border.
    /// </summary>
    public class TabControl : System.Windows.Forms.TabControl {

        /// <summary>
        /// Constant value found in the Windows.h header file.
        /// </summary>
        private const int TCM_ADJUSTRECT = 0x1328;

        /// <summary>
        /// Disabling background redraw will prevent tabs from flickering.
        /// </summary>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

        /// <summary>
        /// A callback function that processes messages sent to a window.
        /// </summary>
        protected override void WndProc(ref System.Windows.Forms.Message message) {
            if (message.Msg == TCM_ADJUSTRECT) {
                RECT rect = (RECT)message.GetLParam(typeof(RECT));
                rect.Left = Left - Margin.Left;
                rect.Right = Right + Margin.Right;
                rect.Top = Top - Margin.Top;
                rect.Bottom = Bottom + Margin.Bottom;
                Marshal.StructureToPtr(rect, message.LParam, true);
            }
            base.WndProc(ref message);
        }

        /// <summary>
        /// A rectangle structure to be marshalled.
        /// </summary>
        private struct RECT {
            public int Left, Top, Right, Bottom;
        }
    }
}
