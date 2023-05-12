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
 * Version 1.1.9.0
 */

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace BetHelper {

    /// <summary>
    /// Implements standard landline telephone ringing based on the Tesla P51p
    /// telephone exchange. It requires an imported ZIP file in the resources
    /// called 'bell'.
    /// </summary>
    public class TelephoneBell : IDisposable {

        /// <summary>
        /// Index of the selected telephone bell.
        /// </summary>
        private int index;

        /// <summary>
        /// Field with the bell file names.
        /// </summary>
        private string[] telephoneBellFileNames;

        /// <summary>
        /// Field with the bell directory full path.
        /// </summary>
        private string telephoneBellDirectoryPath;

        /// <summary>
        /// Field with a Timer object.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Field with Windows Media Player object.
        /// </summary>
        private WindowsMediaPlayer windowsMediaPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneBell"/> class.
        /// </summary>
        public TelephoneBell() {
            telephoneBellFileNames = new string[] {
                Constants.TelephoneBellFileName01,
                Constants.TelephoneBellFileName02,
                Constants.TelephoneBellFileName03,
                Constants.TelephoneBellFileName04,
                Constants.TelephoneBellFileName05,
                Constants.TelephoneBellFileName06
            };

            Titles = new string[] {
                Constants.TelephoneBellTitle01,
                Constants.TelephoneBellTitle02,
                Constants.TelephoneBellTitle03,
                Constants.TelephoneBellTitle04,
                Constants.TelephoneBellTitle05,
                Constants.TelephoneBellTitle06
            };

            string appDataPath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
            telephoneBellDirectoryPath = Path.Combine(appDataPath, Constants.TelephoneBellDirectoryName);
            ExtractBellAsync();
            windowsMediaPlayer = new WindowsMediaPlayer();

            timer = new System.Timers.Timer(Constants.TelephoneBellRingPeriod);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => Chime());
        }

        /// <summary>
        /// A property to determine if the bell is ringing.
        /// </summary>
        public bool IsRinging => timer.Enabled;

        /// <summary>
        /// Property with the bell titles.
        /// </summary>
        public string[] Titles { get; private set; }

        /// <summary>
        /// Performs chime.
        /// </summary>
        public void Chime() {
            try {
                if (File.Exists(Path.Combine(telephoneBellDirectoryPath, telephoneBellFileNames[index]))) {
                    windowsMediaPlayer.controls.play();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        /// <summary>
        /// Performs chime.
        /// </summary>
        public void Chime(int index) {
            timer.Stop();
            if (index < 0 || index > telephoneBellFileNames.Length - 1) {
                index = 0;
            }
            this.index = index;
            string filePath = Path.Combine(telephoneBellDirectoryPath, telephoneBellFileNames[index]);
            try {
                if (File.Exists(filePath)) {
                    windowsMediaPlayer.URL = filePath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        public void Dispose() => timer.Dispose();

        /// <summary>
        /// Checks if exists all bell sound files.
        /// </summary>
        private bool ExistsAll() {
            foreach (string fileName in telephoneBellFileNames) {
                if (!File.Exists(Path.Combine(telephoneBellDirectoryPath, fileName))) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Extracts the telephone bell sounds in the ZIP file imported in the
        /// resources called 'bell' and saves it to the application data folder
        /// as an MP3 files.
        /// </summary>
        private async void ExtractBellAsync() {
            await Task.Run(new Action(() => {
                try {
                    if (!Directory.Exists(telephoneBellDirectoryPath)) {
                        Directory.CreateDirectory(telephoneBellDirectoryPath);
                    }
                    if (!ExistsAll()) {
                        string telephoneBellZipFilePath = Path.Combine(telephoneBellDirectoryPath, Constants.TelephoneBellZipFileName);
                        if (!File.Exists(telephoneBellZipFilePath)) {
                            File.WriteAllBytes(telephoneBellZipFilePath, Properties.Resources.bell);
                        }
                        using (ZipArchive zipArchive = ZipFile.OpenRead(telephoneBellZipFilePath)) {
                            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries) {
                                string filePath = Path.Combine(telephoneBellDirectoryPath, zipArchiveEntry.Name);
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
                        File.Delete(telephoneBellZipFilePath);
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }));
        }

        /// <summary>
        /// Starts ringing.
        /// </summary>
        public void Ring(int index) {
            if (index < 0 || index > telephoneBellFileNames.Length - 1) {
                index = 0;
            }
            this.index = index;
            string filePath = Path.Combine(telephoneBellDirectoryPath, telephoneBellFileNames[index]);
            try {
                if (File.Exists(filePath)) {
                    windowsMediaPlayer.URL = filePath;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            timer.Start();
        }

        /// <summary>
        /// Stops ringing.
        /// </summary>
        public void Stop() {
            timer.Stop();
            try {
                windowsMediaPlayer.controls.stop();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}
