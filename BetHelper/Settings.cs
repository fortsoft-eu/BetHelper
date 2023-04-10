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
 * Version 1.1.0.0
 */

using FortSoft.Tools;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BetHelper {

    /// <summary>
    /// This is an implementation of using the PersistentSettings class for Bet
    /// Helper.
    /// </summary>
    public sealed class Settings : IDisposable {

        /// <summary>
        /// Fields
        /// </summary>
        private AllowedAddrHandler allowedAddrHandler;
        private ConfigHandler configHandler;
        private NumberFormatHandler numberFormatHandler;
        private PersistentSettings persistentSettings;
        private string[] acceptLanguages, userAgents;
        private string acceptLanguage, userAgent;

        /// <summary>
        /// Occurs on successful saving all application settings into the Windows
        /// registry.
        /// </summary>
        public event EventHandler Saved;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings() {
            userAgents = new string[] {
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/112.0.5615.46 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/112.0.5615.46 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/112.0.5615.46 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.48 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_2_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/111.0.5563.54 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/111.0.5563.54 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/111.0.5563.54 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5563.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/110.0.5481.83 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/110.0.5481.83 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/110.0.5481.83 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5481.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/109.0.5414.112 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/109.0.5414.112 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/109.0.5414.112 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.5414.117 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_0_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13.0; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (X11; Linux i686; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (X11; Linux x86_64; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (X11; Fedora; Linux x86_64; rv:107.0) Gecko/20100101 Firefox/107.0",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13.0; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (X11; Linux i686; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (Linux x86_64; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (X11; Fedora; Linux x86_64; rv:102.0) Gecko/20100101 Firefox/102.0",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.42",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_0_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.42",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/93.0.4585.21",
                "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/93.0.4585.21",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_0_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/93.0.4585.21",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/93.0.4585.21",
                "Mozilla/5.0 (Linux; Android 12; SM-S906N Build/QP1A.190711.020; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/80.0.3987.119 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G996U Build/QP1A.190711.020; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G980F Build/QP1A.190711.020; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.96 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 9; SM-G973U Build/PPR1.180610.011) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G892A Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/60.0.3112.107 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G930VC Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/58.0.3029.83 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; SM-G935S Build/MMB29K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 5.1.1; SM-G928X Build/LMY47X) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 12; Pixel 6 Build/SD1A.210817.023; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/94.0.4606.71 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 11; Pixel 5 Build/RQ3A.210805.001.A1; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/92.0.4515.159 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; Google Pixel 4 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.108 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.0.0; Pixel 2 Build/OPD1.170811.002; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/59.0.3071.125 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.1.1; Google Pixel Build/NMF26F; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/54.0.2840.85 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 9; J8110 Build/55.0.A.0.552; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/71.0.3578.99 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.1.1; G8231 Build/41.2.A.0.219; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/59.0.3071.125 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; E6653 Build/32.2.A.0.253) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; HTC Desire 21 pro 5G) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.127 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; Wildfire U20 5G) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0; HTC One X10 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0; HTC One M9 Build/MRA58K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.3"
            };

            acceptLanguages = new string[] {
                "en-US, en-GB;q=0.9, cs-CZ;q=0.7, sk-SK;q=0.5",
                "en-US, en-GB;q=0.9, sk-SK;q=0.7, cs-CZ;q=0.5",
                "cs-CZ, cs, sk-SK;q=0.8, sk;q=0.7, en-US;q=0.6",
                "cs-CZ, sk-SK;q=0.9, en-US;q=0.7, en-GB;q=0.5",
                "cs-CZ, en;q=0.8, en-GB;q=0.6",
                "cs-CZ, en;q=0.9, en-US;q=0.7, en-GB;q=0.5",
                "cs-CZ, en-US;q=0.8, en-GB;q=0.6",
                "cs-CZ, en-US;q=0.9, en-GB;q=0.7, en;q=0.5",
                "sk-SK, sk, cs-CZ;q=0.8, cs;q=0.7, en-US;q=0.6",
                "sk-SK, cs-CZ;q=0.9, en-US;q=0.7, en-GB;q=0.5",
                "sk-SK, en;q=0.8, en-GB;q=0.6",
                "sk-SK, en-US;q=0.8, en-GB;q=0.6",
                "sk-SK, en-US;q=0.9, en-GB;q=0.7, en;q=0.5",
                "en-US, de-DE;q=0.8",
                "de-DE, en-US;q=0.8",
            };

            allowedAddrHandler = new AllowedAddrHandler();
            numberFormatHandler = new NumberFormatHandler();
            persistentSettings = new PersistentSettings();
            try {
                string telegramAppDirectory = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData), Constants.TelegramDesktopApplicationName);
                if (File.Exists(Path.Combine(telegramAppDirectory, Constants.TelegramDesktopFileName))) {
                    TelegramAppDirectory = telegramAppDirectory;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            Load();
        }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string UserAgent {
            get {
                return string.IsNullOrEmpty(userAgent) ? userAgents[0] : userAgent;
            }
            set {
                userAgent = value == null || value.Equals(userAgents[0]) ? string.Empty : value;
            }
        }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string AcceptLanguage {
            get {
                return string.IsNullOrEmpty(acceptLanguage) ? AcceptLanguages[0] : acceptLanguage;
            }
            set {
                acceptLanguage = value == null || value.Equals(acceptLanguages[0]) ? string.Empty : value;
            }
        }

        /// <summary>
        /// UserAgents getter.
        /// </summary>
        public string[] UserAgents => userAgents;

        /// <summary>
        /// AcceptLanguages getter.
        /// </summary>
        public string[] AcceptLanguages => acceptLanguages;

        /// <summary>
        /// NumberFormatHandler getter.
        /// </summary>
        public AllowedAddrHandler AllowedAddrHandler => allowedAddrHandler;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public int ActivePanelLeft { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public int ActivePanelRight { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public int ActivePreferencesPanel { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool RightPaneCollapsed { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool RightPaneWidth { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool PrintSoftMargins { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool TabsBoldFont { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool TabsBackgroundColor { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public TabAppearance TabAppearance { get; set; } = TabAppearance.Buttons;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public ToolStripRenderMode StripRenderMode { get; set; } = ToolStripRenderMode.System;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Border3DStyle Border3DStyle { get; set; } = Border3DStyle.Adjust;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool DisableThemes { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string LastExportDirectory { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public int ExtensionFilterIndex { get; set; } = 4;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool CheckForUpdates { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool StatusBarNotifOnly { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public int NumberFormatInt { get; set; }

        /// <summary>
        /// NumberFormat getter.
        /// </summary>
        public NumberFormatComboBox NumberFormat => numberFormatHandler.GetNumberFormat(NumberFormatInt);

        /// <summary>
        /// NumberFormatHandler getter.
        /// </summary>
        public NumberFormatHandler NumberFormatHandler => numberFormatHandler;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool UseDecimalPrefix { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool EnableCache { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool PersistSessionCookies { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool PersistUserPreferences { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool EnablePrintPreview { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool EnableDrmContent { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool EnableAudio { get; set; } = true;

        /// <summary>
        /// This setting will not be directly stored in the Windows registry.
        /// </summary>
        public bool AudioEnabled { get; private set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool ShowConsoleMessages { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool LogConsoleMessages { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool ShowLoadErrors { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool LogLoadErrors { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool EnableLogPreloadLimit { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public ushort LogPreloadLimit { get; set; } = 64;

        /// <summary>
        /// LogPreloadLimitMin property.
        /// </summary>
        public ushort LogPreloadLimitMin { get; private set; }

        /// <summary>
        /// LogPreloadLimitMax property.
        /// </summary>
        public ushort LogPreloadLimitMax { get; private set; } = 8192;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool RestrictForLargeLogs { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public ushort LargeLogsLimit { get; set; } = 512;

        /// <summary>
        /// LargeLogsLimitMin property.
        /// </summary>
        public ushort LargeLogsLimitMin { get; private set; }

        /// <summary>
        /// LargeLogsLimitMax property.
        /// </summary>
        public ushort LargeLogsLimitMax { get; private set; } = 8192;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool LogForeignUrls { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool LogPopUpFrameHandler { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool OutlineSearchResults { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool AutoAdjustRightPaneWidth { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool AutoLogInAfterInitialLoad { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool DisplayPromptBeforeClosing { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool SortBookmarks { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool TruncateBookmarkTitles { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color BookmakerDefaultColor { get; set; } = Color.Pink;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color BookmakerSelectedColor { get; set; } = Color.MistyRose;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color SportInfo1DefaultColor { get; set; } = Color.LemonChiffon;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color SportInfo1SelectedColor { get; set; } = Color.LightYellow;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color SportInfo2DefaultColor { get; set; } = Color.PeachPuff;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color SportInfo2SelectedColor { get; set; } = Color.PapayaWhip;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color DashboardDefaultColor { get; set; } = Color.LightGreen;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color DashboardSelectedColor { get; set; } = Color.PaleGreen;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color CalculatorDefaultColor { get; set; } = Color.LightCyan;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color CalculatorSelectedColor { get; set; } = Color.Azure;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color OverlayBackgroundColor { get; set; } = Color.SteelBlue;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public Color OverlayCrosshairColor { get; set; } = Color.Yellow;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool BlockRequestsToForeignUrls { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool KeepAnEyeOnTheClientsIP { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool IgnoreCertificateErrors { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public bool TryToKeepUserLoggedIn { get; set; } = true;

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string ConfigHash { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string PreferredEditor { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public string TelegramAppDirectory { get; set; }

        /// <summary>
        /// An example of software application setting that will be stored in the
        /// Windows registry.
        /// </summary>
        public ushort InspectOverlayOpacity { get; set; } = 50;

        /// <summary>
        /// InspectOverlayOpacityMin property.
        /// </summary>
        public ushort InspectOverlayOpacityMin { get; private set; }

        /// <summary>
        /// InspectOverlayOpacityMax property.
        /// </summary>
        public ushort InspectOverlayOpacityMax { get; private set; } = 100;

        /// <summary>
        /// Loads the software application settings from the Windows registry.
        /// </summary>
        private void Load() {
            acceptLanguage = persistentSettings.Load("AcceptLanguage", acceptLanguage);
            userAgent = persistentSettings.Load("UserAgent", userAgent);

            IntToActivePanels(persistentSettings.Load("ActivePanels", ActivePanelsToInt()));
            IntToBitSettings(persistentSettings.Load("BitSettings", BitSettingsToInt()));
            IntToByteSettings(persistentSettings.Load("ByteSettings", ByteSettingsToInt()));
            IntToWordSettings1(persistentSettings.Load("WordSettings1", WordSettings1ToInt()));
            IntToWordSettings2(persistentSettings.Load("WordSettings2", WordSettings2ToInt()));

            ConfigHash = persistentSettings.Load("ConfigHash", ConfigHash);
            LastExportDirectory = persistentSettings.Load("LastExportDir", LastExportDirectory);
            PreferredEditor = persistentSettings.Load("PreferredEditor", PreferredEditor);
            TelegramAppDirectory = persistentSettings.Load("TelegramAppDir", TelegramAppDirectory);
            BookmakerDefaultColor = persistentSettings.Load("BookmakerDefaultColor", BookmakerDefaultColor);
            BookmakerSelectedColor = persistentSettings.Load("BookmakerSelectedColor", BookmakerSelectedColor);
            SportInfo1DefaultColor = persistentSettings.Load("SportInfo1DefaultColor", SportInfo1DefaultColor);
            SportInfo1SelectedColor = persistentSettings.Load("SportInfo1SelectedColor", SportInfo1SelectedColor);
            SportInfo2DefaultColor = persistentSettings.Load("SportInfo2DefaultColor", SportInfo2DefaultColor);
            SportInfo2SelectedColor = persistentSettings.Load("SportInfo2SelectedColor", SportInfo2SelectedColor);
            DashboardDefaultColor = persistentSettings.Load("DashboardDefaultColor", DashboardDefaultColor);
            DashboardSelectedColor = persistentSettings.Load("DashboardSelectedColor", DashboardSelectedColor);
            CalculatorDefaultColor = persistentSettings.Load("CalculatorDefaultColor", CalculatorDefaultColor);
            CalculatorSelectedColor = persistentSettings.Load("CalculatorSelectedColor", CalculatorSelectedColor);
            OverlayBackgroundColor = persistentSettings.Load("OverlayBackgroundColor", OverlayBackgroundColor);
            OverlayCrosshairColor = persistentSettings.Load("OverlayCrosshairColor", OverlayCrosshairColor);
        }

        /// <summary>
        /// Saves the software application settings into the Windows registry.
        /// </summary>
        public void Save() {
            persistentSettings.Save("AcceptLanguage", acceptLanguage);
            persistentSettings.Save("UserAgent", userAgent);

            persistentSettings.Save("ActivePanels", ActivePanelsToInt());
            persistentSettings.Save("BitSettings", BitSettingsToInt());
            persistentSettings.Save("ByteSettings", ByteSettingsToInt());
            persistentSettings.Save("WordSettings1", WordSettings1ToInt());
            persistentSettings.Save("WordSettings2", WordSettings2ToInt());

            persistentSettings.Save("ConfigHash", ConfigHash);
            persistentSettings.Save("LastExportDir", LastExportDirectory);
            persistentSettings.Save("PreferredEditor", PreferredEditor);
            persistentSettings.Save("TelegramAppDir", TelegramAppDirectory);
            persistentSettings.Save("BookmakerDefaultColor", BookmakerDefaultColor);
            persistentSettings.Save("BookmakerSelectedColor", BookmakerSelectedColor);
            persistentSettings.Save("SportInfo1DefaultColor", SportInfo1DefaultColor);
            persistentSettings.Save("SportInfo1SelectedColor", SportInfo1SelectedColor);
            persistentSettings.Save("SportInfo2DefaultColor", SportInfo2DefaultColor);
            persistentSettings.Save("SportInfo2SelectedColor", SportInfo2SelectedColor);
            persistentSettings.Save("DashboardDefaultColor", DashboardDefaultColor);
            persistentSettings.Save("DashboardSelectedColor", DashboardSelectedColor);
            persistentSettings.Save("CalculatorDefaultColor", CalculatorDefaultColor);
            persistentSettings.Save("CalculatorSelectedColor", CalculatorSelectedColor);
            persistentSettings.Save("OverlayBackgroundColor", OverlayBackgroundColor);
            persistentSettings.Save("OverlayCrosshairColor", OverlayCrosshairColor);
            Saved?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Expands an integer value into some boolean settings.
        /// </summary>
        private void IntToBitSettings(int i) {
            BitArray bitArray = new BitArray(new int[] { i });
            bool[] bitSettings = new bool[bitArray.Count];
            bitArray.CopyTo(bitSettings, 0);
            RightPaneCollapsed = bitSettings[31];
            RightPaneWidth = bitSettings[30];
            TabsBoldFont = bitSettings[29];
            TabsBackgroundColor = bitSettings[28];
            UseDecimalPrefix = bitSettings[27];
            EnableCache = bitSettings[26];
            PersistSessionCookies = bitSettings[25];
            PersistUserPreferences = bitSettings[24];
            EnablePrintPreview = bitSettings[23];
            EnableDrmContent = bitSettings[22];
            EnableAudio = bitSettings[21];
            AudioEnabled = bitSettings[21];
            OutlineSearchResults = bitSettings[20];
            LogForeignUrls = bitSettings[19];
            LogPopUpFrameHandler = bitSettings[18];
            ShowLoadErrors = bitSettings[17];
            LogLoadErrors = bitSettings[16];
            ShowConsoleMessages = bitSettings[15];
            LogConsoleMessages = bitSettings[14];
            EnableLogPreloadLimit = bitSettings[13];
            RestrictForLargeLogs = bitSettings[12];
            AutoAdjustRightPaneWidth = bitSettings[11];
            AutoLogInAfterInitialLoad = bitSettings[10];
            DisplayPromptBeforeClosing = bitSettings[9];
            SortBookmarks = bitSettings[8];
            TruncateBookmarkTitles = bitSettings[7];
            BlockRequestsToForeignUrls = bitSettings[6];
            KeepAnEyeOnTheClientsIP = bitSettings[5];
            IgnoreCertificateErrors = bitSettings[4];
            PrintSoftMargins = bitSettings[3];
            CheckForUpdates = bitSettings[2];
            StatusBarNotifOnly = bitSettings[1];
            DisableThemes = bitSettings[0];
        }

        /// <summary>
        /// Compacts some boolean settings into an integer value.
        /// </summary>
        private int BitSettingsToInt() {
            StringBuilder stringBuilder = new StringBuilder(32)
                .Append(RightPaneCollapsed ? 1 : 0)
                .Append(RightPaneWidth ? 1 : 0)
                .Append(TabsBoldFont ? 1 : 0)
                .Append(TabsBackgroundColor ? 1 : 0)
                .Append(UseDecimalPrefix ? 1 : 0)
                .Append(EnableCache ? 1 : 0)
                .Append(PersistSessionCookies ? 1 : 0)
                .Append(PersistUserPreferences ? 1 : 0)
                .Append(EnablePrintPreview ? 1 : 0)
                .Append(EnableDrmContent ? 1 : 0)
                .Append(EnableAudio ? 1 : 0)
                .Append(OutlineSearchResults ? 1 : 0)
                .Append(LogForeignUrls ? 1 : 0)
                .Append(LogPopUpFrameHandler ? 1 : 0)
                .Append(ShowLoadErrors ? 1 : 0)
                .Append(LogLoadErrors ? 1 : 0)
                .Append(ShowConsoleMessages ? 1 : 0)
                .Append(LogConsoleMessages ? 1 : 0)
                .Append(EnableLogPreloadLimit ? 1 : 0)
                .Append(RestrictForLargeLogs ? 1 : 0)
                .Append(AutoAdjustRightPaneWidth ? 1 : 0)
                .Append(AutoLogInAfterInitialLoad ? 1 : 0)
                .Append(DisplayPromptBeforeClosing ? 1 : 0)
                .Append(SortBookmarks ? 1 : 0)
                .Append(TruncateBookmarkTitles ? 1 : 0)
                .Append(BlockRequestsToForeignUrls ? 1 : 0)
                .Append(KeepAnEyeOnTheClientsIP ? 1 : 0)
                .Append(IgnoreCertificateErrors ? 1 : 0)
                .Append(PrintSoftMargins ? 1 : 0)
                .Append(CheckForUpdates ? 1 : 0)
                .Append(StatusBarNotifOnly ? 1 : 0)
                .Append(DisableThemes ? 1 : 0);
            return Convert.ToInt32(stringBuilder.ToString(), 2);
        }

        /// <summary>
        /// Expands an integer value into active panel idexes and opacity value.
        /// </summary>
        private void IntToActivePanels(int i) {
            byte[] bytes = IntToByteArray(i);
            ActivePanelLeft = bytes[0];
            ActivePanelRight = bytes[1];
            ActivePreferencesPanel = bytes[2];
            TryToKeepUserLoggedIn = bytes[3] > 0;
        }

        /// <summary>
        /// Compacts some active panel idexes and opacity value into an integer
        /// value.
        /// </summary>
        private int ActivePanelsToInt() {
            byte[] bytes = new byte[] {
                (byte)ActivePanelLeft,
                (byte)ActivePanelRight,
                (byte)ActivePreferencesPanel,
                (byte)(TryToKeepUserLoggedIn ? 1 : 0)
            };
            return ByteArrayToInt(bytes);
        }

        /// <summary>
        /// Expands an integer value into Border3DStyle and InspectOverlayOpacity.
        /// </summary>
        private void IntToWordSettings1(int i) {
            ushort[] values = IntToUShortArray(i);
            Border3DStyle = (Border3DStyle)values[0];
            InspectOverlayOpacity = values[1] > InspectOverlayOpacityMax
                ? InspectOverlayOpacityMax
                : values[1] < InspectOverlayOpacityMin ? InspectOverlayOpacityMin : values[1];
        }

        /// <summary>
        /// Compacts Border3DStyle and InspectOverlayOpacity into an integer value.
        /// </summary>
        private int WordSettings1ToInt() {
            ushort[] values = new ushort[] {
                (ushort)Border3DStyle,
                InspectOverlayOpacity
            };
            return UShortArrayToInt(values);
        }

        /// <summary>
        /// Expands an integer value into log preload limit and large logs limit.
        /// </summary>
        private void IntToWordSettings2(int i) {
            ushort[] values = IntToUShortArray(i);
            LogPreloadLimit = values[0] > LogPreloadLimitMax
                ? LogPreloadLimitMax
                : values[0] < LogPreloadLimitMin ? LogPreloadLimitMin : values[0];
            LargeLogsLimit = values[1] > LargeLogsLimitMax
                ? LargeLogsLimitMax
                : values[1] < LargeLogsLimitMin ? LargeLogsLimitMin : values[1];
        }

        /// <summary>
        /// Compacts log preload limit and large logs limit into an integer value.
        /// </summary>
        private int WordSettings2ToInt() {
            ushort[] values = new ushort[] {
                LogPreloadLimit,
                LargeLogsLimit
            };
            return UShortArrayToInt(values);
        }

        /// <summary>
        /// Expands an integer value into byte values.
        /// </summary>
        private void IntToByteSettings(int i) {
            byte[] bytes = IntToByteArray(i);
            TabAppearance = (TabAppearance)(bytes[0] > 2 ? 1 : bytes[0]);
            ExtensionFilterIndex = bytes[1];
            StripRenderMode = (ToolStripRenderMode)(bytes[2] > 0 ? bytes[2] > 3 ? 1 : bytes[2] : 1);
            NumberFormatInt = numberFormatHandler.AdjustIndex(bytes[3]);
        }

        /// <summary>
        /// Compacts some byte values into an integer value.
        /// </summary>
        private int ByteSettingsToInt() {
            byte[] bytes = new byte[] {
                (byte)TabAppearance,
                (byte)ExtensionFilterIndex,
                (byte)StripRenderMode,
                (byte)NumberFormatInt
            };
            return ByteArrayToInt(bytes);
        }

        /// <summary>
        /// This setting will not be directly stored in the Windows registry.
        /// </summary>
        public bool RenderWithVisualStyles { get; set; }

        /// <summary>
        /// This setting will not be stored in the Windows registry.
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// Loads remote configuration file.
        /// </summary>
        public void LoadConfig() {
            if (string.IsNullOrEmpty(ConfigHash)) {
                if (SingleInstance.FocusRunning(Application.ExecutablePath, Program.GetTitle())) {
                    Environment.Exit(0);
                } else {
                    ConfigHashForm configHashForm = new ConfigHashForm(this);
                    if (configHashForm.ShowDialog().Equals(DialogResult.OK)) {
                        configHandler = new ConfigHandler(this);
                    } else {
                        Environment.Exit(0);
                    }
                }
            } else {
                try {
                    configHandler = new ConfigHandler(this);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    StringBuilder title = new StringBuilder()
                        .Append(Program.GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionError);
                    MessageBox.Show(exception.Message, title.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Saves remote configuration file.
        /// </summary>
        public async Task<bool> SaveConfigAsync() {
            string responseString = await configHandler.SaveConfigAsync();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseString);
            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName(Constants.XmlElementStatus);
            string status = string.Empty;
            foreach (XmlElement xmlElement in xmlNodeList) {
                status = xmlElement.InnerText;
            }
            return Constants.StatusOk.Equals(status);
        }

        /// <summary>
        /// Clears the software application values from the Windows registry.
        /// </summary>
        public void Clear() {
            persistentSettings.Clear();
            allowedAddrHandler.Delete();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose() {
            allowedAddrHandler.Dispose();
            persistentSettings.Dispose();
        }

        /// <summary>
        /// Hardware-independent static method for conversion of byte array into
        /// an integer value.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <returns>An integer value to store in the Windows registry.</returns>
        public static int ByteArrayToInt(byte[] bytes) {
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Hardware-independent static method for conversion of an integer value
        /// into a byte array.
        /// </summary>
        /// <param name="val">An integer value stored in the registry.</param>
        public static byte[] IntToByteArray(int val) {
            byte[] bytes = BitConverter.GetBytes(val);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// Hardware-independent static method for conversion of two ushort
        /// values into an integer value.
        /// </summary>
        /// <param name="values">An array of ushort values.</param>
        /// <returns>An integer value to store in the Windows registry.</returns>
        public static int UShortArrayToInt(ushort[] values) {
            byte[] bytes = new byte[4];
            Array.Copy(
                BitConverter.GetBytes(values[0]),
                0,
                bytes,
                BitConverter.IsLittleEndian ? 0 : 2,
                2);
            Array.Copy(
                BitConverter.GetBytes(values[1]),
                0,
                bytes,
                BitConverter.IsLittleEndian ? 2 : 0,
                2);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Hardware-independent static method for conversion of an integer value
        /// into two ushort values.
        /// </summary>
        /// <param name="val">An integer value stored in the registry.</param>
        public static ushort[] IntToUShortArray(int val) {
            byte[] bytes = BitConverter.GetBytes(val);
            return new ushort[] {
                BitConverter.ToUInt16(bytes, BitConverter.IsLittleEndian ? 0 : 2),
                BitConverter.ToUInt16(bytes, BitConverter.IsLittleEndian ? 2 : 0)
            };
        }
    }
}
