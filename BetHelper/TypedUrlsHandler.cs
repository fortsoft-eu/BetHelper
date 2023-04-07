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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace BetHelper {
    public class TypedUrlsHandler {
        private List<string> typedUrls;
        private string typedUrlsFilePath;

        public event EventHandler Deleted;
        public event EventHandler Loaded;
        public event EventHandler Saved;

        public TypedUrlsHandler() {
            typedUrls = new List<string>();
            typedUrlsFilePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.TypedUrlsFileName);
        }

        public int MaximumTypedUrls => Constants.MaximumTypedUrls;

        public void Add(string url) {
            List<string> typedUrls = new List<string>();
            typedUrls.Add(url);
            int count = 1;
            foreach (string str in this.typedUrls) {
                if (++count > Constants.MaximumTypedUrls) {
                    break;
                }
                if (!str.Equals(url)) {
                    typedUrls.Add(str);
                }
            }
            this.typedUrls = typedUrls;
        }

        public void Delete() {
            try {
                if (File.Exists(typedUrlsFilePath)) {
                    File.Delete(typedUrlsFilePath);
                }
                if (!File.Exists(typedUrlsFilePath)) {
                    Deleted?.Invoke(this, EventArgs.Empty);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        public string[] Get() => typedUrls.ToArray();

        public void Load() {
            try {
                if (File.Exists(typedUrlsFilePath)) {
                    using (FileStream fileStream = File.OpenRead(typedUrlsFilePath)) {
                        string[] typedUrls = (string[])new BinaryFormatter().Deserialize(fileStream);
                        this.typedUrls = typedUrls.ToList();
                    }
                    Loaded?.Invoke(this, EventArgs.Empty);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        public void Save() {
            try {
                using (FileStream fileStream = File.Create(typedUrlsFilePath)) {
                    new BinaryFormatter().Serialize(fileStream, typedUrls.ToArray());
                }
                Saved?.Invoke(this, EventArgs.Empty);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}
