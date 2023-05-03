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
 * Version 1.1.7.0
 */

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {

    /// <summary>
    /// Implements the extractor of the calculator files.
    /// </summary>
    public class CalculatorExtractor {

        /// <summary>
        /// Field with file names of the calculator files.
        /// </summary>
        private string[] calculatorFileNames;

        /// <summary>
        /// Field with the calculator directory full path.
        /// </summary>
        private string calculatorDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorExtractor"/>
        /// class.
        /// </summary>
        public CalculatorExtractor() {
            calculatorFileNames = new string[] {
                Constants.CalculatorFileName01,
                Constants.CalculatorFileName02,
                Constants.CalculatorFileName03,
                Constants.CalculatorFileName04,
                Constants.CalculatorFileName05,
                Constants.CalculatorFileName06,
                Constants.CalculatorFileName07,
                Constants.CalculatorFileName08,
                Constants.CalculatorFileName09,
                Constants.CalculatorFileName10,
                Constants.CalculatorFileName11,
                Constants.CalculatorFileName12,
                Constants.CalculatorFileName13,
                Constants.CalculatorFileName14,
                Constants.CalculatorFileName15,
                Constants.CalculatorFileName16,
                Constants.CalculatorFileName17,
                Constants.CalculatorFileName18
            };

            string appDataPath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
            calculatorDirectory = Path.Combine(appDataPath, Constants.CalculatorDirectoryName);
            ExtractCalculatorAsync();
        }

        /// <summary>
        /// Checks if exists all files of the calculator.
        /// </summary>
        private bool ExistsAll() {
            foreach (string fileName in calculatorFileNames) {
                if (!File.Exists(Path.Combine(calculatorDirectory, fileName))) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Extracts the calculator files in the ZIP file imported in the
        /// resources called 'calculator' and saves them to the application data
        /// folder.
        /// </summary>
        private async void ExtractCalculatorAsync() {
            await Task.Run(new Action(() => {
                try {
                    if (!Directory.Exists(calculatorDirectory)) {
                        Directory.CreateDirectory(calculatorDirectory);
                    }
                    if (!ExistsAll()) {
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
                                            File.WriteAllBytes(filePath, StaticMethods.ReadToEnd(stream));
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
    }
}
