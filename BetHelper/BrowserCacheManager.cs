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
 * Version 1.0.0.0
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public sealed class BrowserCacheManager : IDisposable {
        private Thread monitorThread;
        private SemaphoreSlim monitorSemaphore;
        private System.Timers.Timer monitorTimer;

        public event EventHandler<BrowserCacheEventArgs> CacheSizeComputed;

        public BrowserCacheManager() {
            monitorSemaphore = new SemaphoreSlim(0, 1);
            try {
                ClearAsync();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            monitorTimer = new System.Timers.Timer(10000);
            monitorTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                monitorTimer.Interval = Constants.CacheSizeRefreshInterval * 60000;
                RunMonitor();
            });
            monitorTimer.Start();
            monitorSemaphore.Release(1);
        }

        public long CachedCacheSize { get; private set; }

        public void RunMonitor() {
            monitorThread = new Thread(new ThreadStart(ComputeCacheSize));
            monitorThread.Priority = ThreadPriority.Lowest;
            monitorThread.Start();
        }

        public void Set(ClearSet clearSet) {
            try {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.BrowserCacheMngrSetClearFileName), clearSet.ToString());
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private async void ClearAsync() {
            await Task.Run(new Action(() => {
                try {
                    string appDataPath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
                    if (File.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrSetClearFileName))) {
                        ClearSet clearSet = (ClearSet)Enum.Parse(typeof(ClearSet), File.ReadAllText(Path.Combine(appDataPath, Constants.BrowserCacheMngrSetClearFileName)).Trim(), true);
                        switch (clearSet) {
                            case ClearSet.BrowserCacheOnly:
                                ClearCacheOnly(appDataPath);
                                break;
                            case ClearSet.BrowserCacheIncludingUserData:
                                ClearCacheOnly(appDataPath);
                                ClearUserData(appDataPath);
                                break;
                        }
                        File.Delete(Path.Combine(appDataPath, Constants.BrowserCacheMngrSetClearFileName));
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }));
        }

        private void ClearCacheOnly(string appDataPath) {
            if (Directory.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrCacheSubDirName))) {
                Directory.Delete(Path.Combine(appDataPath, Constants.BrowserCacheMngrCacheSubDirName), true);
            }
        }

        private void ClearUserData(string appDataPath) {
            if (Directory.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrUserDataSubDirName))) {
                Directory.Delete(Path.Combine(appDataPath, Constants.BrowserCacheMngrUserDataSubDirName), true);
            }
            if (File.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrLocalPrefsFileName))) {
                File.Delete(Path.Combine(appDataPath, Constants.BrowserCacheMngrLocalPrefsFileName));
            }
        }

        private void ComputeCacheSize() {
            long size = 0;
            try {
                string appDataPath = Path.GetDirectoryName(Application.LocalUserAppDataPath);
                monitorSemaphore.Wait();
                if (Directory.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrCacheSubDirName))) {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(appDataPath, Constants.BrowserCacheMngrCacheSubDirName));
                    size += DirectorySize(directoryInfo);
                }
                if (Directory.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrUserDataSubDirName))) {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(appDataPath, Constants.BrowserCacheMngrUserDataSubDirName));
                    size += DirectorySize(directoryInfo);
                }
                if (File.Exists(Path.Combine(appDataPath, Constants.BrowserCacheMngrLocalPrefsFileName))) {
                    size += new FileInfo(Path.Combine(appDataPath, Constants.BrowserCacheMngrLocalPrefsFileName)).Length;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            } finally {
                monitorSemaphore.Release();
                CachedCacheSize = size;
                CacheSizeComputed?.Invoke(this, new BrowserCacheEventArgs(size));
            }
        }

        private static long DirectorySize(DirectoryInfo directoryInfo) => directoryInfo.GetFiles().Sum(new Func<FileInfo, long>(fileInfo => fileInfo.Length)) + directoryInfo.GetDirectories().Sum(new Func<DirectoryInfo, long>(dirInfo => DirectorySize(dirInfo)));

        public void Dispose() {
            if (monitorThread != null && monitorThread.IsAlive) {
                monitorThread.Abort();
                monitorThread = null;
            }
            monitorTimer.Stop();
            monitorTimer.Dispose();
            monitorSemaphore.Dispose();
        }

        public enum ClearSet {
            BrowserCacheOnly, BrowserCacheIncludingUserData
        }
    }
}
