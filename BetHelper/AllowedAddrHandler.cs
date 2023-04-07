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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BetHelper {
    public class AllowedAddrHandler : IDisposable {
        private bool locked, valid;
        private List<string> allowedAddr;
        private Regex regexIPv4, regexIPv6Simplified;
        private string allowedAddrFilePath, remoteXmlAddress;
        private System.Timers.Timer timer;

        public event EventHandler Added;
        public event EventHandler Deleted;
        public event EventHandler IpCheckFailed;
        public event EventHandler IpCheckReset;
        public event EventHandler Loaded;
        public event EventHandler Saved;

        public AllowedAddrHandler() {
            BuildRemoteXmlAddress();

            regexIPv4 = new Regex(Constants.IPv4Pattern);
            regexIPv6Simplified = new Regex(Constants.IPv6SimplifiedPattern, RegexOptions.IgnoreCase);

            allowedAddr = new List<string>();
            allowedAddrFilePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.AllowedAddrFileName);

            timer = new System.Timers.Timer(Constants.IPAddressCheckInterval);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => {
                timer.Stop();
                valid = false;
            });

            Load();
        }

        public bool Locked => locked;

        private void BuildRemoteXmlAddress() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Properties.Resources.Website.TrimEnd(Constants.Slash).ToLowerInvariant());
            stringBuilder.Append(Constants.Slash);
            stringBuilder.Append(Application.ProductName.ToLowerInvariant());
            stringBuilder.Append(Constants.Slash);
            stringBuilder.Append(Constants.RemoteApiScriptName);
            stringBuilder.Append(Constants.QuestionMark);
            stringBuilder.Append(Constants.RemoteVariableNameGet);
            stringBuilder.Append(Constants.EqualSign);
            stringBuilder.Append(Constants.RemoteClientRemoteAddress);
            remoteXmlAddress = stringBuilder.ToString();
        }

        public string[] Items {
            get {
                return allowedAddr.ToArray();
            }
            set {
                allowedAddr = value.ToList();
                try {
                    using (FileStream fileStream = File.Create(allowedAddrFilePath)) {
                        new BinaryFormatter().Serialize(fileStream, allowedAddr.ToArray());
                    }
                    Saved?.Invoke(this, EventArgs.Empty);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void Add(string ipAddress) {
            if (!allowedAddr.Contains(ipAddress)) {
                allowedAddr.Add(ipAddress);
                try {
                    using (FileStream fileStream = File.Create(allowedAddrFilePath)) {
                        new BinaryFormatter().Serialize(fileStream, allowedAddr.ToArray());
                    }
                    Added?.Invoke(this, EventArgs.Empty);
                    Saved?.Invoke(this, EventArgs.Empty);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public bool Check() {
            if (locked) {
                return false;
            }
            if (valid) {
                return true;
            }
            string response = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            try {
                xmlDocument.Load(remoteXmlAddress);
                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName(Constants.XmlElementRemoteAddress);
                foreach (XmlElement xmlElement in xmlNodeList) {
                    response = xmlElement.InnerText;
                }
                if (allowedAddr.Count > 0) {
                    if (allowedAddr.Contains(response)) {
                        valid = true;
                        timer.Start();
                        return true;
                    }
                    locked = true;
                    IpCheckFailed?.Invoke(this, EventArgs.Empty);
                    return false;
                }
                if (CheckIpSimple(response)) {
                    Add(response);
                    valid = true;
                    timer.Start();
                    return true;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
            locked = true;
            IpCheckFailed?.Invoke(this, EventArgs.Empty);
            return false;
        }

        public bool CheckIpSimple(string ipAddress) {
            if (regexIPv4.IsMatch(ipAddress)) {
                foreach (string octet in ipAddress.Split(Constants.Period)) {
                    if (int.Parse(octet) > 255) {
                        return false;
                    }
                }
                return true;
            }
            if (regexIPv6Simplified.IsMatch(ipAddress)) {
                return true;
            }
            return false;
        }

        public void Delete() {
            try {
                if (File.Exists(allowedAddrFilePath)) {
                    File.Delete(allowedAddrFilePath);
                }
                if (!File.Exists(allowedAddrFilePath)) {
                    Deleted?.Invoke(this, EventArgs.Empty);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void Load() {
            try {
                if (File.Exists(allowedAddrFilePath)) {
                    using (FileStream fileStream = File.OpenRead(allowedAddrFilePath)) {
                        string[] allowedAddr = (string[])new BinaryFormatter().Deserialize(fileStream);
                        this.allowedAddr = allowedAddr.ToList();
                    }
                    Loaded?.Invoke(this, EventArgs.Empty);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        public void Reset() {
            locked = false;
            IpCheckReset?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose() => timer.Dispose();
    }
}
