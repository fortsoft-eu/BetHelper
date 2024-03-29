﻿/**
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
 * Version 1.1.14.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WebPWrapper;

namespace BetHelper {
    internal static class StaticMethods {
        internal static string AbbreviateMatchName(string text, Font font, int width) {
            char[] separators = new char[] { Constants.EnDash, Constants.EmDash };
            Regex regex = new Regex(Constants.SplitWordsPattern, RegexOptions.Singleline);
            if (text.IndexOfAny(separators) > -1) {
                string[] split = text.Split(separators, 2);
                string[] left = regex.Split(split[0])
                    .Where(new Func<string, bool>(item => !string.IsNullOrEmpty(item)))
                    .ToArray();
                string[] right = regex.Split(split[1])
                    .Where(new Func<string, bool>(item => !string.IsNullOrEmpty(item)))
                    .ToArray();
                StringBuilder leftPart = new StringBuilder();
                StringBuilder rightPart = new StringBuilder();
                for (int i = 0; i < Math.Max(left.Length, right.Length); i++) {
                    if (i.Equals(0)) {
                        if (left.Length > i) {
                            leftPart.Append(left[i]);
                        }
                        if (right.Length > i) {
                            rightPart.Append(right[i]);
                        }
                    } else {
                        StringBuilder builder = new StringBuilder(leftPart.ToString());
                        if (left.Length > i) {
                            builder.Append(Constants.Space)
                                .Append(left[i]);
                        }
                        builder.Append(Constants.Space)
                            .Append(Constants.EnDash)
                            .Append(rightPart.ToString());
                        if (right.Length > i) {
                            builder.Append(Constants.Space).Append(right[i]);
                        }
                        if (TextRenderer.MeasureText(builder.ToString(), font).Width <= width) {
                            if (left.Length > i) {
                                leftPart.Append(Constants.Space)
                                    .Append(left[i]);
                            }
                            if (right.Length > i) {
                                rightPart.Append(Constants.Space)
                                    .Append(right[i]);
                            }
                        } else {
                            return new StringBuilder()
                                .Append(leftPart.ToString())
                                .Append(Constants.Space)
                                .Append(Constants.EnDash)
                                .Append(Constants.Space)
                                .Append(rightPart.ToString())
                                .ToString();
                        }
                    }
                }
                return new StringBuilder()
                    .Append(leftPart.ToString())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(rightPart.ToString())
                    .ToString();
            } else {
                StringBuilder builder = new StringBuilder();
                foreach (string word in regex.Split(text)
                        .Where(new Func<string, bool>(item => !string.IsNullOrEmpty(item)))
                        .ToArray()) {

                    if (builder.Length.Equals(0)) {
                        builder.Append(word);
                    } else {
                        string str = new StringBuilder()
                            .Append(builder.ToString())
                            .Append(Constants.Space)
                            .Append(word)
                            .ToString();
                        if (TextRenderer.MeasureText(str, font).Width <= width) {
                            builder.Append(Constants.Space)
                                .Append(word);
                        } else {
                            return builder.ToString();
                        }
                    }
                }
                return builder.ToString();
            }
        }

        internal static bool CheckSelectedUrl(TextBox textBox) {
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

        internal static void DumpEnumerable(IEnumerable data) {
            try {
                string filePath = Path.Combine(
                    Application.LocalUserAppDataPath,
                    DateTime.Now.ToString(Constants.DumpFileNameTimeFormat) + Constants.ExtensionTxt);
                using (StreamWriter streamWriter = File.AppendText(filePath)) {
                    StringBuilder stringBuilder = new StringBuilder();
                    int i = 0;
                    foreach (object item in data) {
                        stringBuilder.Append(i++)
                            .Append(Constants.Colon)
                            .AppendLine(item.ToString());
                    }
                    streamWriter.WriteLine(stringBuilder.ToString());
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        internal static void DumpString(string str) {
            try {
                string filePath = Path.Combine(
                    Application.LocalUserAppDataPath,
                    DateTime.Now.ToString(Constants.DumpFileNameTimeFormat) + Constants.ExtensionTxt);
                using (StreamWriter streamWriter = File.AppendText(filePath)) {
                    streamWriter.WriteLine(str);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        internal static bool EqualsHostsAndSchemes(string url1, string url2) {
            if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url1);
                Uri uri2 = new Uri(url2);
                return uri2.GetComponents(UriComponents.StrongAuthority, UriFormat.UriEscaped)
                        .Equals(uri1.GetComponents(UriComponents.StrongAuthority, UriFormat.UriEscaped), StringComparison.Ordinal)
                    && uri2.GetComponents(UriComponents.Scheme, UriFormat.UriEscaped)
                        .Equals(uri1.GetComponents(UriComponents.Scheme, UriFormat.UriEscaped), StringComparison.Ordinal);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        internal static bool EqualsSecondLevelDomains(string url1, string url2, bool includeTld = true) {
            if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url1);
                Uri uri2 = new Uri(url2);
                if (uri1.HostNameType.Equals(UriHostNameType.Dns) && uri2.HostNameType.Equals(UriHostNameType.Dns)) {
                    Regex regex = new Regex(Constants.SecondLevelDomainPattern, RegexOptions.RightToLeft);
                    bool eq = regex.Replace(uri2.Host, Constants.ReplaceSecond)
                            .Equals(regex.Replace(uri1.Host, Constants.ReplaceSecond), StringComparison.Ordinal);
                    return includeTld
                        ? eq && regex.Replace(uri2.Host, Constants.ReplaceThird)
                            .Equals(regex.Replace(uri1.Host, Constants.ReplaceThird), StringComparison.Ordinal)
                        : eq;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        internal static string EscapeArgument(string argument) {
            argument = Regex.Replace(argument, @"(\\*)" + Constants.QuotationMark, @"$1$1\" + Constants.QuotationMark);
            return Constants.QuotationMark + Regex.Replace(argument, @"(\\+)$", @"$1$1") + Constants.QuotationMark;
        }

        internal static void ExportAsImage(Control control, string filePath) {
            using (Bitmap bitmap = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppArgb)) {
                control.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                SaveBitmap(bitmap, filePath);
            }
        }

        internal static Control FindControlAtCursor(Form form) {
            Point point = Cursor.Position;
            if (form.Bounds.Contains(point)) {
                return FindControlAtPoint(form, form.PointToClient(point));
            }
            return null;
        }

        internal static Control FindControlAtPoint(Control container, Point point) {
            foreach (Control control in container.Controls) {
                if (control.Visible && control.Bounds.Contains(point)) {
                    return FindControlAtPoint(control, new Point(point.X - control.Left, point.Y - control.Top)) ?? control;
                }
            }
            return null;
        }

        internal static Size GetNewGraphicsSize(Size graphicSize, Size canvasSize) {
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

        internal static int GetScrollPosition(TabControl tabControl) {
            int multiplier = -1;
            while (tabControl.GetTabRect(multiplier++ + 1).Left < 0 && multiplier < tabControl.TabCount) { }
            return multiplier;
        }

        internal static bool IsGraphicsRotationNeeded(Size graphicSize, Size canvasSize) {
            if (graphicSize.Width <= 0 || graphicSize.Height <= 0 || canvasSize.Width <= 0 || canvasSize.Height <= 0) {
                return false;
            }
            if (graphicSize.Width / (float)graphicSize.Height == 1f || canvasSize.Width / (float)canvasSize.Height == 1f) {
                return false;
            }
            if (graphicSize.Width < canvasSize.Width && graphicSize.Height < canvasSize.Height) {
                return false;
            }
            if (graphicSize.Width / (float)graphicSize.Height < 1f && canvasSize.Width / (float)canvasSize.Height < 1f ||
                graphicSize.Width / (float)graphicSize.Height > 1f && canvasSize.Width / (float)canvasSize.Height > 1f) {
                return false;
            }
            return true;
        }

        internal static bool IsHoverTabRectangle(TabControl tabControl) {
            if (tabControl.TabCount > 0) {
                Rectangle rectangle = tabControl.GetTabRect(0);
                rectangle.X = 0;
                rectangle.Width = tabControl.Width;
                rectangle.Location = tabControl.PointToScreen(rectangle.Location);
                return rectangle.Contains(Cursor.Position);
            }
            return false;
        }

        internal static byte[] ReadToEnd(Stream stream) {
            long originalPosition = 0;
            if (stream.CanSeek) {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead.Equals(readBuffer.Length)) {
                        int nextByte = stream.ReadByte();
                        if (!nextByte.Equals(-1)) {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }
                byte[] buffer = readBuffer;
                if (!totalBytesRead.Equals(readBuffer.Length)) {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                return null;
            } finally {
                if (stream.CanSeek) {
                    stream.Position = originalPosition;
                }
            }
        }

        internal static void SaveBitmap(Bitmap bitmap, string finePath) {
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

        internal static string ShowSize(long length, IFormatProvider provider, bool useDecimalPrefix) {
            double num = length;
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.ZeroDecimalDigitsFormat, provider))
                    .Append(Constants.Space)
                    .Append(Constants.Byte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Kilobyte : Constants.Kibibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Megabyte : Constants.Mebibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Gigabyte : Constants.Gibibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Terabyte : Constants.Tebibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Petabyte : Constants.Pebibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Exabyte : Constants.Exbibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Zettabyte : Constants.Zebibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Yottabyte : Constants.Yobibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            if (num < 1000d) {
                return new StringBuilder()
                    .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                    .Append(Constants.Space)
                    .Append(useDecimalPrefix ? Constants.Ronnabyte : Constants.Robibyte)
                    .ToString();
            }
            num = num / (useDecimalPrefix ? 1000d : 1024d);
            return new StringBuilder()
                .Append(num.ToString(Constants.OneDecimalDigitFormat, provider))
                .Append(Constants.Space)
                .Append(useDecimalPrefix ? Constants.Quettabyte : Constants.Qubibyte)
                .ToString();
        }

        internal static List<SearchLine> SplitToLines(string str) {
            List<SearchLine> lines = new List<SearchLine>();
            StringBuilder stringBuilder = new StringBuilder();
            int charIndex = 0, index = 0;
            foreach (char c in str.ToCharArray()) {
                if (c.Equals(Constants.CarriageReturn) || c.Equals(Constants.LineFeed)) {
                    lines.Add(new SearchLine(stringBuilder.ToString(), index));
                    stringBuilder = new StringBuilder();
                } else {
                    if (stringBuilder.Length.Equals(0)) {
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

        internal static void TabControlScroll(TabControl tabControl, int direction) {
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

        internal static string UppercaseFirst(string str, CultureInfo cultureInfo) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }
            char[] c = str.ToCharArray();
            c[0] = char.ToUpper(c[0], cultureInfo);
            return new string(c);
        }
    }
}
