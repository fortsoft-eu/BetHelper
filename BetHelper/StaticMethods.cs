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
 * Version 1.0.0.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WebPWrapper;

namespace BetHelper {
    public static class StaticMethods {
        public static bool CheckSelectedUrl(TextBox textBox) {
            if (textBox.SelectionLength <= 3072) {
                string trimmed = textBox.SelectedText.TrimStart();
                if (trimmed.Length <= 2048) {
                    try {
                        new Uri(trimmed);
                        return true;
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                    }
                }
            }
            return false;
        }

        public static void DumpEnumerable(IEnumerable data) {
            try {
                using (StreamWriter streamWriter = File.AppendText(Path.Combine(Application.LocalUserAppDataPath, DateTime.Now.ToString(Constants.DumpFileNameTimeFormat) + Constants.ExtensionTxt))) {
                    StringBuilder stringBuilder = new StringBuilder();
                    int i = 0;
                    foreach (object item in data) {
                        stringBuilder.Append(i++);
                        stringBuilder.Append(Constants.Colon);
                        stringBuilder.AppendLine(item.ToString());
                    }
                    streamWriter.WriteLine(stringBuilder.ToString());
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        public static void DumpString(string str) {
            try {
                using (StreamWriter streamWriter = File.AppendText(Path.Combine(Application.LocalUserAppDataPath, DateTime.Now.ToString(Constants.DumpFileNameTimeFormat) + Constants.ExtensionTxt))) {
                    streamWriter.WriteLine(str);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        public static bool EqualsSecondLevelDomains(string url1, string url2, bool includeTld = true) {
            if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url1);
                Uri uri2 = new Uri(url2);
                if (uri1.HostNameType == UriHostNameType.Dns && uri2.HostNameType == UriHostNameType.Dns) {
                    Regex regex = new Regex(Constants.SecondLevelDomainPattern, RegexOptions.RightToLeft);
                    bool eq = regex.Replace(uri2.Host, Constants.ReplaceSecond).Equals(regex.Replace(uri1.Host, Constants.ReplaceSecond), StringComparison.Ordinal);
                    return includeTld ? eq && regex.Replace(uri2.Host, Constants.ReplaceThird).Equals(regex.Replace(uri1.Host, Constants.ReplaceThird), StringComparison.Ordinal) : eq;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        public static string EscapeArgument(string argument) {
            argument = Regex.Replace(argument, @"(\\*)" + Constants.QuotationMark, @"$1$1\" + Constants.QuotationMark);
            return Constants.QuotationMark + Regex.Replace(argument, @"(\\+)$", @"$1$1") + Constants.QuotationMark;
        }

        public static void ExportAsImage(Control control, string filePath) {
            using (Bitmap bitmap = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppArgb)) {
                control.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                SaveBitmap(bitmap, filePath);
            }
        }

        public static Control FindControlAtCursor(Form form) {
            Point point = Cursor.Position;
            if (form.Bounds.Contains(point)) {
                return FindControlAtPoint(form, form.PointToClient(point));
            }
            return null;
        }

        public static Control FindControlAtPoint(Control container, Point point) {
            foreach (Control control in container.Controls) {
                if (control.Visible && control.Bounds.Contains(point)) {
                    return FindControlAtPoint(control, new Point(point.X - control.Left, point.Y - control.Top)) ?? control;
                }
            }
            return null;
        }

        public static Size GetNewGraphicsSize(Size graphicSize, Size canvasSize) {
            bool rotate = IsGraphicsRotationNeeded(graphicSize, canvasSize);
            float ratio = 1f;
            float ratioWidth = graphicSize.Width / (float)(rotate ? canvasSize.Height : canvasSize.Width);
            float ratioHeight = graphicSize.Height / (float)(rotate ? canvasSize.Width : canvasSize.Height);
            float ratioMax = Math.Max(ratioWidth, ratioHeight);
            if (ratioMax > ratio) {
                ratio = ratioMax;
            }
            return new Size((int)Math.Floor(graphicSize.Width / ratio), (int)Math.Floor(graphicSize.Height / ratio));
        }

        public static int GetScrollPosition(TabControl tabControl) {
            int multiplier = -1;
            while (tabControl.GetTabRect(multiplier++ + 1).Left < 0 && multiplier < tabControl.TabCount) { }
            return multiplier;
        }

        public static bool IsGraphicsRotationNeeded(Size graphicSize, Size canvasSize) {
            if (graphicSize.Width <= 0 || graphicSize.Height <= 0 || canvasSize.Width <= 0 || canvasSize.Height <= 0) {
                return false;
            }
            if (graphicSize.Width / (float)graphicSize.Height == 1f || canvasSize.Width / (float)canvasSize.Height == 1f) {
                return false;
            }
            if (graphicSize.Width < canvasSize.Width && graphicSize.Height < canvasSize.Height) {
                return false;
            }
            if (graphicSize.Width / (float)graphicSize.Height < 1f && canvasSize.Width / (float)canvasSize.Height < 1f || graphicSize.Width / (float)graphicSize.Height > 1f && canvasSize.Width / (float)canvasSize.Height > 1f) {
                return false;
            }
            return true;
        }

        public static bool IsHoverTabRectangle(TabControl tabControl) {
            if (tabControl.TabCount > 0) {
                Rectangle rectangle = tabControl.GetTabRect(0);
                rectangle.X = 0;
                rectangle.Width = tabControl.Width;
                rectangle.Location = tabControl.PointToScreen(rectangle.Location);
                return rectangle.Contains(Cursor.Position);
            }
            return false;
        }

        public static void SaveBitmap(Bitmap bitmap, string finePath) {
            switch (Path.GetExtension(finePath).ToLowerInvariant()) {
                case Constants.ExtensionBmp:
                    bitmap.Save(finePath, ImageFormat.Bmp);
                    break;
                case Constants.ExtensionGif:
                    bitmap.Save(finePath, ImageFormat.Gif);
                    break;
                case Constants.ExtensionJpg:
                    bitmap.Save(finePath, ImageFormat.Jpeg);
                    break;
                case Constants.ExtensionTif:
                    bitmap.Save(finePath, ImageFormat.Tiff);
                    break;
                case Constants.ExtensionWebP:
                    using (WebP webP = new WebP()) {
                        File.WriteAllBytes(finePath, webP.EncodeLossless(bitmap));
                    }
                    break;
                default:
                    bitmap.Save(finePath, ImageFormat.Png);
                    break;
            }
        }

        public static string ShowSize(long length, IFormatProvider provider, bool useDecimalPrefix) {
            double num = length;
            if (num < 1000d) {
                return num.ToString(provider) + Constants.Space + Constants.Byte;
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Kilobyte : Constants.Kibibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Megabyte : Constants.Mebibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Gigabyte : Constants.Gibibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Terabyte : Constants.Tebibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Petabyte : Constants.Pebibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Exabyte : Constants.Exbibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Zettabyte : Constants.Zebibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Yottabyte : Constants.Yobibyte);
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return num.ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Ronnabyte : Constants.Robibyte);
            }
            return (num / (useDecimalPrefix ? 1000d : 1024d)).ToString(Constants.OneDecimalDigitFormat, provider) + Constants.Space + (useDecimalPrefix ? Constants.Quettabyte : Constants.Qubibyte);
        }

        public static List<SearchLine> SplitToLines(string str) {
            List<SearchLine> lines = new List<SearchLine>();
            StringBuilder stringBuilder = new StringBuilder();
            int charIndex = 0, index = 0;
            foreach (char c in str.ToCharArray()) {
                if (c.Equals(Constants.CarriageReturn) || c.Equals(Constants.LineFeed)) {
                    lines.Add(new SearchLine(stringBuilder.ToString(), index));
                    stringBuilder = new StringBuilder();
                } else {
                    if (stringBuilder.Length == 0) {
                        index = charIndex;
                    }
                    stringBuilder.Append(c);
                }
                charIndex++;
            }
            if (stringBuilder.Length > 0) {
                lines.Add(new SearchLine(stringBuilder.ToString(), charIndex));
            }
            return lines;
        }

        public static void TabControlScroll(TabControl tabControl, int direction) {
            if (tabControl.TabCount > 0) {
                if (tabControl.GetTabRect(tabControl.TabCount - 1).Right < tabControl.Width - 30 && direction < 0) {
                    return;
                }
                int scrollPosition = Math.Max(0, (GetScrollPosition(tabControl) - Math.Sign(direction)) * 0x10000);
                IntPtr handle = tabControl.Handle;
                NativeMethods.PostMessage(handle, Constants.WM_HSCROLL, (IntPtr)(scrollPosition | 0x4), IntPtr.Zero);
                NativeMethods.PostMessage(handle, Constants.WM_HSCROLL, (IntPtr)(scrollPosition | 0x8), IntPtr.Zero);
            }
        }

        public static string UppercaseFirst(string str, CultureInfo cultureInfo) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }
            char[] c = str.ToCharArray();
            c[0] = char.ToUpper(c[0], cultureInfo);
            return new string(c);
        }
    }
}
