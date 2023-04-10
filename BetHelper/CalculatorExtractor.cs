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
 * Version 1.1.1.0
 */

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public class CalculatorExtractor {

        public CalculatorExtractor() {
            ExtractCalculatorAsync();
        }

        private static async void ExtractCalculatorAsync() {
            await Task.Run(new Action(() => {
                try {
                    string appDataPath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
                    string calculatorDirectory = Path.Combine(appDataPath, Constants.CalculatorDirectoryName);
                    if (!Directory.Exists(calculatorDirectory)) {
                        Directory.CreateDirectory(calculatorDirectory);
                    }
                    if (!File.Exists(Constants.CalculatorFileName01) || !File.Exists(Constants.CalculatorFileName02)
                            || !File.Exists(Constants.CalculatorFileName03) || !File.Exists(Constants.CalculatorFileName04)
                            || !File.Exists(Constants.CalculatorFileName05) || !File.Exists(Constants.CalculatorFileName06)
                            || !File.Exists(Constants.CalculatorFileName07) || !File.Exists(Constants.CalculatorFileName08)
                            || !File.Exists(Constants.CalculatorFileName09) || !File.Exists(Constants.CalculatorFileName10)
                            || !File.Exists(Constants.CalculatorFileName11) || !File.Exists(Constants.CalculatorFileName12)
                            || !File.Exists(Constants.CalculatorFileName13) || !File.Exists(Constants.CalculatorFileName14)
                            || !File.Exists(Constants.CalculatorFileName15) || !File.Exists(Constants.CalculatorFileName16)
                            || !File.Exists(Constants.CalculatorFileName17) || !File.Exists(Constants.CalculatorFileName18)) {

                        string calculatorZipFilePath = Path.Combine(calculatorDirectory, Constants.CalculatorZipFileName);
                        if (!File.Exists(calculatorZipFilePath)) {
                            File.WriteAllBytes(calculatorZipFilePath, Properties.Resources.calculator);
                        }
                        using (ZipArchive zipArchive = ZipFile.OpenRead(calculatorZipFilePath)) {
                            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries) {
                                string filePath = Path.Combine(calculatorDirectory, zipArchiveEntry.Name);
                                try {
                                    if (!File.Exists(filePath)) {
                                        using (Stream stream = zipArchiveEntry.Open()) {
                                            File.WriteAllBytes(filePath, ReadToEnd(stream));
                                        }
                                    }
                                } catch (Exception exception) {
                                    Debug.WriteLine(exception);
                                    ErrorLog.WriteLine(exception);
                                }
                            }
                        }
                        File.Delete(calculatorZipFilePath);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }));
        }

        protected static byte[] ReadToEnd(Stream stream) {
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
    }
}
