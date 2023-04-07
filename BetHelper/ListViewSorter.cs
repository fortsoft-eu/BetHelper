/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2012-2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.1.0
 */

using System;
using System.Collections;
using System.Windows.Forms;

namespace FortSoft.Tools {

    /// <summary>
    /// Implements a custom ListView sorter sorting items by the Tag property.
    /// </summary>
    public sealed class ListViewSorter : IComparer {

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSorter"/> class.
        /// </summary>
        public ListViewSorter() {
            SortOrder = SortOrder.None;
        }

        /// <summary>
        /// Sort column.
        /// </summary>
        public int SortColumn { get; set; }

        /// <summary>
        /// Sort order.
        /// </summary>
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// The method performing the comparison.
        /// </summary>
        public int Compare(object a, object b) {
            ListViewItem listViewItemA = (ListViewItem)a;
            ListViewItem listViewItemB = (ListViewItem)b;
            int result = 0;
            if (listViewItemA.SubItems.Count > SortColumn && listViewItemB.SubItems.Count > SortColumn) {
                object objectA = listViewItemA.SubItems[SortColumn].Tag;
                object objectB = listViewItemB.SubItems[SortColumn].Tag;
                if (objectA != null && objectB != null) {
                    Type type = objectA.GetType();
                    if (type.Equals(objectB.GetType())) {
                        if (type == typeof(string)) {
                            result = ((string)objectA).CompareTo((string)objectB);
                        } else if (type == typeof(int)) {
                            result = (int)objectA > (int)objectB ? 1 : (int)objectA < (int)objectB ? -1 : 0;
                        } else if (type == typeof(uint)) {
                            result = (uint)objectA > (uint)objectB ? 1 : (uint)objectA < (uint)objectB ? -1 : 0;
                        } else if (type == typeof(short)) {
                            result = (short)objectA > (short)objectB ? 1 : (short)objectA < (short)objectB ? -1 : 0;
                        } else if (type == typeof(ushort)) {
                            result = (ushort)objectA > (ushort)objectB ? 1 : (ushort)objectA < (ushort)objectB ? -1 : 0;
                        } else if (type == typeof(long)) {
                            result = (long)objectA > (long)objectB ? 1 : (long)objectA < (long)objectB ? -1 : 0;
                        } else if (type == typeof(ulong)) {
                            result = (ulong)objectA > (ulong)objectB ? 1 : (ulong)objectA < (ulong)objectB ? -1 : 0;
                        } else if (type == typeof(float)) {
                            result = (float)objectA > (float)objectB ? 1 : (float)objectA < (float)objectB ? -1 : 0;
                        } else if (type == typeof(decimal)) {
                            result = (decimal)objectA > (decimal)objectB ? 1 : (decimal)objectA < (decimal)objectB ? -1 : 0;
                        } else if (type == typeof(DateTime)) {
                            result = (DateTime)objectA > (DateTime)objectB ? 1 : (DateTime)objectA < (DateTime)objectB ? -1 : 0;
                        } else if (type == typeof(TimeSpan)) {
                            result = (TimeSpan)objectA > (TimeSpan)objectB ? 1 : (TimeSpan)objectA < (TimeSpan)objectB ? -1 : 0;
                        }
                    }
                } else {
                    result = (listViewItemA.SubItems[SortColumn].Text).CompareTo(listViewItemB.SubItems[SortColumn].Text);
                }
            }
            switch (SortOrder) {
                case SortOrder.Ascending:
                    return result;
                case SortOrder.Descending:
                    return -result;
                default:
                    return 0;
            }
        }
    }
}
