/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2022-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.6
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
        /// Fields.
        /// </summary>
        private AllowedAddrHandler allowedAddrHandler;
        private ConfigHandler configHandler;
        private NumberFormatHandler numberFormatHandler;
        private PersistentSettings persistentSettings;
        private string[] acceptLanguages, userAgents;
        private string acceptLanguage, externalEditor, userAgent;

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
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.89 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.89 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.89 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.6261.90 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.51 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.51 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/122.0.6261.51 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.6167.178 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/120.0.6099.101 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/120.0.6099.101 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/120.0.6099.101 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.6099.43 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/119.0.6045.109 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/119.0.6045.109 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/119.0.6045.109 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.66 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/118.0.5993.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/118.0.5993.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/118.0.5993.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.65 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/117.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/117.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/117.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/116.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/116.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/116.0.5993.58 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5938.153 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/115.0.5790.130 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/115.0.5790.130 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/115.0.5790.130 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5790.136 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/114.0.5735.50 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/114.0.5735.50 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/114.0.5735.50 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5735.57 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_3_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/113.0.5672.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPad; CPU OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/113.0.5672.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPod; CPU iPhone OS 16_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/113.0.5672.69 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A205U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-A102U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; SM-N960U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-X420) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 10; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5672.76 Mobile Safari/537.36",
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

            NumberFormatInt = numberFormatHandler.NumberFormats.Length - 1;

            ExternalEditor = GetExternalEditor();

            Load();
        }

        /// <summary>
        /// Gets the instance of an AllowedAddrHandler object.
        /// </summary>
        public AllowedAddrHandler AllowedAddrHandler => allowedAddrHandler;

        /// <summary>
        /// True if the embedded Chromium browser is currently enabled to play
        /// audio. Otherwise false.
        /// </summary>
        public bool AudioEnabled { get; private set; }

        /// <summary>
        /// Represents the setting if the width of the right pane will be set
        /// automatically. The default value is true.
        /// </summary>
        public bool AutoAdjustRightPaneWidth { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether auto log in will be performed after
        /// the initial load of the website. The default value is true.
        /// </summary>
        public bool AutoLogInAfterInitialLoad { get; set; } = true;

        /// <summary>
        /// Represents the setting to block all requests to URLs not belonging to
        /// the URL set by their second-level domains or the exception list by
        /// their hostname. The default value is true.
        /// </summary>
        public bool BlockRequestsToForeignUrls { get; set; } = true;

        /// <summary>
        /// Represents the setting for displaying bold bell status in the
        /// StatusStrip when applicable. The default value is true.
        /// </summary>
        public bool BoldBellStatus { get; set; } = true;

        /// <summary>
        /// Represents the setting if the application should check for updates.
        /// The default value is true.
        /// </summary>
        public bool CheckForUpdates { get; set; } = true;

        /// <summary>
        /// Represents whether visual styles will be used when rendering
        /// application windows. The default value is false.
        /// </summary>
        public bool DisableThemes { get; set; }

        /// <summary>
        /// Represents the setting of whether a pop-up warning about closing the
        /// application will appear before closing the main application window.
        /// The default value is true.
        /// </summary>
        public bool DisplayPromptBeforeClosing { get; set; } = true;

        /// <summary>
        /// Represents the setting to allow the embedded Chromium browser to play
        /// audio. The default value is true.
        /// </summary>
        public bool EnableAudio { get; set; } = true;

        /// <summary>
        /// Represents the setting if the bell will be enabled when an
        /// opportunity comes. The default value is false.
        /// </summary>
        public bool EnableOpportunityBell { get; set; } = true;

        /// <summary>
        /// Represents the setting if the bell will be enabled when a new tip is
        /// received from the service. The default value is true.
        /// </summary>
        public bool EnableTipArrivalBell { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether the embedded Chromium browser will
        /// be allowed to use the disk cache. The default value is true.
        /// </summary>
        public bool EnableCache { get; set; } = true;

        /// <summary>
        /// Represents the setting to allow the embedded Chromium browser to play
        /// premium media content. The default value is false.
        /// </summary>
        public bool EnableDrmContent { get; set; }

        /// <summary>
        /// Represents the setting if the log viewer will load only a limited
        /// length of the log into the window. If not, it will try to load the
        /// entire log. The default value is true.
        /// </summary>
        public bool EnableLogPreloadLimit { get; set; } = true;

        /// <summary>
        /// Represents the setting if the embedded Chromium browser will display
        /// a preview before printing. The default value is false.
        /// </summary>
        public bool EnablePrintPreview { get; set; }

        /// <summary>
        /// Represents the setting if the proxy server for the embedded Chromium
        /// browser will be enabled. The default value is false.
        /// </summary>
        public bool EnableProxy { get; set; }

        /// <summary>
        /// Represents the setting if F3 pressed at the MainForm gives the focus
        /// to opened FindForm. The default value is true.
        /// </summary>
        public bool F3MainFormFocusesFindForm { get; set; } = true;

        /// <summary>
        /// Represents the setting if certificate errors from websites running on
        /// port 443 will be ignored. The default value is false.
        /// </summary>
        public bool IgnoreCertificateErrors { get; set; }

        /// <summary>
        /// Represents the setting to always keep an eye on the client's IP
        /// address and block all outgoing requests if the client's IP address
        /// changes. If this setting is enabled, the server infrastructure is
        /// overloaded. The default value is false.
        /// </summary>
        public bool KeepAnEyeOnTheClientsIP { get; set; }

        /// <summary>
        /// Represents the setting of whether Chromium embedded browser
        /// JavaScript console messages will be logged or not. The default value
        /// is false.
        /// </summary>
        public bool LogConsoleMessages { get; set; }

        /// <summary>
        /// Represents the setting to log all requests to URLs not belonging to
        /// the URL set by their second-level domains or the exception list by
        /// their hostname. The default value is true.
        /// </summary>
        public bool LogForeignUrls { get; set; } = true;

        /// <summary>
        /// Represents the setting if load errors of the embedded Chromium
        /// browser will be logged or not. The default value is false.
        /// </summary>
        public bool LogLoadErrors { get; set; }

        /// <summary>
        /// Represents the setting to log the functionality of the
        /// PopUpFrameHandler object instance. The default value is false.
        /// </summary>
        public bool LogPopUpFrameHandler { get; set; }

        /// <summary>
        /// Represents the setting of whether to outline search results in the
        /// displayed web page in the Chromium embedded browser. The default
        /// value is true.
        /// </summary>
        public bool OutlineSearchResults { get; set; } = true;

        /// <summary>
        /// Represents the setting if the embedded Chromium browser will be
        /// allowed to permanently store session cookies in the disk cache. The
        /// default value is true.
        /// </summary>
        public bool PersistSessionCookies { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether the embedded Chromium browser will
        /// be allowed to permanently store the user preferences. The default
        /// value is true.
        /// </summary>
        public bool PersistUserPreferences { get; set; } = true;

        /// <summary>
        /// Represents the printing setting, whether to use soft margins (larger)
        /// or hard margins (smaller). This setting does not apply to the
        /// embedded Chromium browser. The default value is true.
        /// </summary>
        public bool PrintSoftMargins { get; set; } = true;

        /// <summary>
        /// Represents the setting if the restrictions of some features for large
        /// log files will be applied. The default value is true.
        /// </summary>
        public bool RestrictForLargeLogs { get; set; } = true;

        /// <summary>
        /// Represents the state of the right panel, whether it is collapsed or
        /// not. The default value is false.
        /// </summary>
        public bool RightPaneCollapsed { get; set; }

        /// <summary>
        /// Represents the display mode of the riht panel, whether the right
        /// panel will be set to the default or minimum width to display its
        /// elements. The default value is false.
        /// </summary>
        public bool RightPaneWidth { get; set; }

        /// <summary>
        /// Represents the setting of whether Chromium embedded browser
        /// JavaScript console messages will be shown in the status bar. The
        /// default value is false.
        /// </summary>
        public bool ShowConsoleMessages { get; set; }

        /// <summary>
        /// Represents the setting if load errors of the embedded Chromium
        /// browser will be displayed in the status bar. The default value is
        /// false.
        /// </summary>
        public bool ShowLoadErrors { get; set; }

        /// <summary>
        /// Represents the setting of whether the bookmarks will be displayed in
        /// the list sorted. The default value is true.
        /// </summary>
        public bool SortBookmarks { get; set; } = true;

        /// <summary>
        /// Represents the setting if the application should inform the user
        /// about available updates in the status bar only. If not, a pop-up
        /// window will appear. The default value is false.
        /// </summary>
        public bool StatusBarNotifOnly { get; set; }

        /// <summary>
        /// Represents the setting of whether the tab headers in the main
        /// application window will be rendered in color. The default value is
        /// true.
        /// </summary>
        public bool TabsBackgroundColor { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether the tab headers in the main
        /// application window will be bold. The default value is false.
        /// </summary>
        public bool TabsBoldFont { get; set; }

        /// <summary>
        /// Represents the setting if the titles of bookmarks in the list will be
        /// abbreviated. The default value is true.
        /// </summary>
        public bool TruncateBookmarkTitles { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether attempts will be made to keep a
        /// user logged in if the user is inactive on a specific site. The
        /// default value is true.
        /// </summary>
        public bool TryToKeepUserLoggedIn { get; set; } = true;

        /// <summary>
        /// Represents the setting of whether decimal prefixes will be used when
        /// displaying data size units. If not, the binary unit prefixes will be
        /// used. The default value is false.
        /// </summary>
        public bool UseDecimalPrefix { get; set; }

        /// <summary>
        /// Represents how the 3D border of the labels will be rendered on the
        /// status strip in the main application window. The default value is
        /// Border3DStyle.Adjust.
        /// </summary>
        public Border3DStyle Border3DStyle { get; set; } = Border3DStyle.Adjust;

        /// <summary>
        /// The time interval in seconds after which the monitors will be turned
        /// off after entering the command to turn off the monitors. The default
        /// value is five seconds.
        /// </summary>
        public byte TurnOffTheMonitorsInterval { get; set; } = 5;

        /// <summary>
        /// The time interval in seconds after which the monitors will be turned
        /// off after entering the command to turn off the monitors maximum
        /// value. The default value is 255 seconds.
        /// </summary>
        public byte TurnOffTheMonitorsIntervalMax { get; private set; } = byte.MaxValue;

        /// <summary>
        /// The time interval in seconds after which the monitors will be turned
        /// off after entering the command to turn off the monitors minimum
        /// value. The default value is zero.
        /// </summary>
        public byte TurnOffTheMonitorsIntervalMin { get; private set; } = byte.MinValue;

        /// <summary>
        /// Default tab color with bookmaker website. The default value is
        /// Color.Pink.
        /// </summary>
        public Color BookmakerDefaultColor { get; set; } = Color.Pink;

        /// <summary>
        /// Default selected tab color with bookmaker website. The default value
        /// is Color.MistyRose.
        /// </summary>
        public Color BookmakerSelectedColor { get; set; } = Color.MistyRose;

        /// <summary>
        /// Default bet calculator tab color. The default value is
        /// Color.LightCyan.
        /// </summary>
        public Color CalculatorDefaultColor { get; set; } = Color.LightCyan;

        /// <summary>
        /// Default bet calculator selected tab color. The default value is
        /// Color.Azure.
        /// </summary>
        public Color CalculatorSelectedColor { get; set; } = Color.Azure;

        /// <summary>
        /// Default dashboard tab color. The default value is Color.LightGreen.
        /// </summary>
        public Color DashboardDefaultColor { get; set; } = Color.LightGreen;

        /// <summary>
        /// Default dashboard selected tab color. The default value is
        /// Color.PaleGreen.
        /// </summary>
        public Color DashboardSelectedColor { get; set; } = Color.PaleGreen;

        /// <summary>
        /// DOM element inspection overlay form background color. The default
        /// value is Color.SteelBlue.
        /// </summary>
        public Color OverlayBackgroundColor { get; set; } = Color.SteelBlue;

        /// <summary>
        /// DOM element inspection overlay form crosshair color. The default
        /// value is Color.Yellow.
        /// </summary>
        public Color OverlayCrosshairColor { get; set; } = Color.Yellow;

        /// <summary>
        /// Sport info 1 website default tab color. The default value is
        /// Color.LemonChiffon.
        /// </summary>
        public Color SportInfo1DefaultColor { get; set; } = Color.LemonChiffon;

        /// <summary>
        /// Sport info 1 website default selected tab color. The default value is
        /// Color.LightYellow.
        /// </summary>
        public Color SportInfo1SelectedColor { get; set; } = Color.LightYellow;

        /// <summary>
        /// Sport info 2 website default tab color. The default value is
        /// Color.PeachPuff.
        /// </summary>
        public Color SportInfo2DefaultColor { get; set; } = Color.PeachPuff;

        /// <summary>
        /// Sport info 2 website default selected tab color. The default value is
        /// Color.PapayaWhip.
        /// </summary>
        public Color SportInfo2SelectedColor { get; set; } = Color.PapayaWhip;

        /// <summary>
        /// Represents the index of the active tab of the left tab panel. The
        /// default value is zero.
        /// </summary>
        public int ActivePanelLeft { get; set; }

        /// <summary>
        /// Represents the index of the active tab of the right tab panel. The
        /// default value is zero.
        /// </summary>
        public int ActivePanelRight { get; set; }

        /// <summary>
        /// Represents the index of the active tab of the preferences tab panel.
        /// The default value is zero.
        /// </summary>
        public int ActivePreferencesPanel { get; set; }

        /// <summary>
        /// Last export extension index used. The default value is four.
        /// </summary>
        public int ExtensionFilterIndex { get; set; } = 4;

        /// <summary>
        /// Represents the integer index of the number display format used. The
        /// default value is zero.
        /// </summary>
        public int NumberFormatInt { get; set; }

        /// <summary>
        /// Represents the index of the selected bell sound for opportunity. The
        /// default value is five.
        /// </summary>
        public int OpportunityBellIndex { get; set; } = 5;

        /// <summary>
        /// Represents the index of the selected bell sound for tip arrival. The
        /// default value is two.
        /// </summary>
        public int TipArrivalBellIndex { get; set; } = 2;

        /// <summary>
        /// Gets an instance of an NumberFormatComboBox object.
        /// </summary>
        public NumberFormatComboBox NumberFormat => numberFormatHandler.GetNumberFormat(NumberFormatInt);

        /// <summary>
        /// Gets the instance of an NumberFormatHandler object.
        /// </summary>
        public NumberFormatHandler NumberFormatHandler => numberFormatHandler;

        /// <summary>
        /// Accept-Language string for embedded Chromium browser.
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
        /// The config hash string. The default value is null.
        /// </summary>
        public string ConfigHash { get; set; }

        /// <summary>
        /// Full path to the external editor executable for viewing log files
        /// outside the application. The default value is null.
        /// </summary>
        public string ExternalEditor {
            get {
                return string.IsNullOrEmpty(externalEditor) ? GetExternalEditor() : externalEditor;
            }
            set {
                externalEditor = value == null || value.Equals(GetExternalEditor()) ? string.Empty : value;
            }
        }

        /// <summary>
        /// Last export directory path used. The default value is null.
        /// </summary>
        public string LastExportDirectory { get; set; }

        /// <summary>
        /// Full path to the Telegram Desktop executable. The default value is
        /// null.
        /// </summary>
        public string TelegramAppDirectory { get; set; }

        /// <summary>
        /// User-Agent string for embedded Chromium browser.
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
        /// Gets an array of preset Accept-Language strings.
        /// </summary>
        public string[] AcceptLanguages => acceptLanguages;

        /// <summary>
        /// Gets an array of preset User-Agent strings.
        /// </summary>
        public string[] UserAgents => userAgents;

        /// <summary>
        /// Represents the appearance settings of the tabs in the main program
        /// window. The default value is TabAppearance.Buttons.
        /// </summary>
        public TabAppearance TabAppearance { get; set; } = TabAppearance.Buttons;

        /// <summary>
        /// Represents the rendering mode of the status strip in the main
        /// application window. The default value is ToolStripRenderMode.System.
        /// </summary>
        public ToolStripRenderMode StripRenderMode { get; set; } = ToolStripRenderMode.System;

        /// <summary>
        /// DOM element inspection overlay form opacity. The default value is
        /// 50%.
        /// </summary>
        public ushort InspectOverlayOpacity { get; set; } = 50;

        /// <summary>
        /// DOM element inspection overlay form opacity maximum value. The
        /// default value is 100%.
        /// </summary>
        public ushort InspectOverlayOpacityMax { get; private set; } = 100;

        /// <summary>
        /// DOM element inspection overlay form opacity minimum value. The
        /// default value is 0%.
        /// </summary>
        public ushort InspectOverlayOpacityMin { get; private set; }

        /// <summary>
        /// Log length to limit some features for large log files. The default
        /// value is 512M.
        /// </summary>
        public ushort LargeLogsLimit { get; set; } = 512;

        /// <summary>
        /// The maximum log length to limit some features for large log files.
        /// The value is 8192M.
        /// </summary>
        public ushort LargeLogsLimitMax { get; private set; } = 8192;

        /// <summary>
        /// The minimum log length to limit some features for large log files.
        /// The value is 0M.
        /// </summary>
        public ushort LargeLogsLimitMin { get; private set; }

        /// <summary>
        /// The length of the log that will be loaded into the log viewer window
        /// if the limit is applied. The default value is 64K.
        /// </summary>
        public ushort LogPreloadLimit { get; set; } = 64;

        /// <summary>
        /// The maximum log length limit that will be loaded into the log viewer
        /// window if the limit is applied. The value is 8192K.
        /// </summary>
        public ushort LogPreloadLimitMax { get; private set; } = 8192;

        /// <summary>
        /// The minimum log length limit that will be loaded into the log viewer
        /// window if the limit is applied. The value is 0K.
        /// </summary>
        public ushort LogPreloadLimitMin { get; private set; }

        /// <summary>
        /// Loads the software application settings from the Windows registry.
        /// </summary>
        private void Load() {
            acceptLanguage = persistentSettings.Load("AcceptLanguage", acceptLanguage);
            externalEditor = persistentSettings.Load("ExternalEditor", externalEditor);
            userAgent = persistentSettings.Load("UserAgent", userAgent);

            IntToActivePanels(persistentSettings.Load("ActivePanels", ActivePanelsToInt()));
            IntToBitSettings1(persistentSettings.Load("BitSettings1", BitSettings1ToInt()));
            IntToBitSettings2(persistentSettings.Load("BitSettings2", BitSettings2ToInt()));
            IntToByteSettings1(persistentSettings.Load("ByteSettings1", ByteSettings1ToInt()));
            IntToByteSettings2(persistentSettings.Load("ByteSettings2", ByteSettings2ToInt()));
            IntToWordSettings1(persistentSettings.Load("WordSettings1", WordSettings1ToInt()));
            IntToWordSettings2(persistentSettings.Load("WordSettings2", WordSettings2ToInt()));

            ConfigHash = persistentSettings.Load("ConfigHash", ConfigHash);
            LastExportDirectory = persistentSettings.Load("LastExportDir", LastExportDirectory);
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
            persistentSettings.Save("ExternalEditor", externalEditor);
            persistentSettings.Save("UserAgent", userAgent);

            persistentSettings.Save("ActivePanels", ActivePanelsToInt());
            persistentSettings.Save("BitSettings1", BitSettings1ToInt());
            persistentSettings.Save("BitSettings2", BitSettings2ToInt());
            persistentSettings.Save("ByteSettings1", ByteSettings1ToInt());
            persistentSettings.Save("ByteSettings2", ByteSettings2ToInt());
            persistentSettings.Save("WordSettings1", WordSettings1ToInt());
            persistentSettings.Save("WordSettings2", WordSettings2ToInt());

            persistentSettings.Save("ConfigHash", ConfigHash);
            persistentSettings.Save("LastExportDir", LastExportDirectory);
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
        private void IntToBitSettings1(int i) {
            BitArray bitArray = new BitArray(new int[] { i });
            bool[] bitSettings = new bool[bitArray.Count];
            bitArray.CopyTo(bitSettings, 0);
            i = bitSettings.Length;

            TryToKeepUserLoggedIn = bitSettings[--i];
            AutoLogInAfterInitialLoad = bitSettings[--i];
            BlockRequestsToForeignUrls = bitSettings[--i];
            KeepAnEyeOnTheClientsIP = bitSettings[--i];
            IgnoreCertificateErrors = bitSettings[--i];
            LogConsoleMessages = bitSettings[--i];
            ShowConsoleMessages = bitSettings[--i];
            LogPopUpFrameHandler = bitSettings[--i];
            LogLoadErrors = bitSettings[--i];
            ShowLoadErrors = bitSettings[--i];
            LogForeignUrls = bitSettings[--i];

            EnableAudio = bitSettings[--i];
            AudioEnabled = EnableAudio;

            EnableDrmContent = bitSettings[--i];
            EnablePrintPreview = bitSettings[--i];
            PersistUserPreferences = bitSettings[--i];
            PersistSessionCookies = bitSettings[--i];
            EnableCache = bitSettings[--i];
            EnableProxy = bitSettings[--i];
            F3MainFormFocusesFindForm = bitSettings[--i];
            OutlineSearchResults = bitSettings[--i];
            TruncateBookmarkTitles = bitSettings[--i];
            SortBookmarks = bitSettings[--i];
            UseDecimalPrefix = bitSettings[--i];
            TabsBoldFont = bitSettings[--i];
            TabsBackgroundColor = bitSettings[--i];
            RestrictForLargeLogs = bitSettings[--i];
            EnableLogPreloadLimit = bitSettings[--i];
            DisplayPromptBeforeClosing = bitSettings[--i];
            DisableThemes = bitSettings[--i];
            PrintSoftMargins = bitSettings[--i];
            StatusBarNotifOnly = bitSettings[--i];
            CheckForUpdates = bitSettings[--i];
        }

        /// <summary>
        /// Compacts some boolean settings into an integer value.
        /// </summary>
        private int BitSettings1ToInt() {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(TryToKeepUserLoggedIn ? 1 : 0)
                .Append(AutoLogInAfterInitialLoad ? 1 : 0)
                .Append(BlockRequestsToForeignUrls ? 1 : 0)
                .Append(KeepAnEyeOnTheClientsIP ? 1 : 0)
                .Append(IgnoreCertificateErrors ? 1 : 0)
                .Append(LogConsoleMessages ? 1 : 0)
                .Append(ShowConsoleMessages ? 1 : 0)
                .Append(LogPopUpFrameHandler ? 1 : 0)
                .Append(LogLoadErrors ? 1 : 0)
                .Append(ShowLoadErrors ? 1 : 0)
                .Append(LogForeignUrls ? 1 : 0)
                .Append(EnableAudio ? 1 : 0)
                .Append(EnableDrmContent ? 1 : 0)
                .Append(EnablePrintPreview ? 1 : 0)
                .Append(PersistUserPreferences ? 1 : 0)
                .Append(PersistSessionCookies ? 1 : 0)
                .Append(EnableCache ? 1 : 0)
                .Append(EnableProxy ? 1 : 0)
                .Append(F3MainFormFocusesFindForm ? 1 : 0)
                .Append(OutlineSearchResults ? 1 : 0)
                .Append(TruncateBookmarkTitles ? 1 : 0)
                .Append(SortBookmarks ? 1 : 0)
                .Append(UseDecimalPrefix ? 1 : 0)
                .Append(TabsBoldFont ? 1 : 0)
                .Append(TabsBackgroundColor ? 1 : 0)
                .Append(RestrictForLargeLogs ? 1 : 0)
                .Append(EnableLogPreloadLimit ? 1 : 0)
                .Append(DisplayPromptBeforeClosing ? 1 : 0)
                .Append(DisableThemes ? 1 : 0)
                .Append(PrintSoftMargins ? 1 : 0)
                .Append(StatusBarNotifOnly ? 1 : 0)
                .Append(CheckForUpdates ? 1 : 0);
            return Convert.ToInt32(stringBuilder.ToString(), 2);
        }

        /// <summary>
        /// Expands an integer value into some boolean settings.
        /// </summary>
        private void IntToBitSettings2(int i) {
            BitArray bitArray = new BitArray(new int[] { i });
            bool[] bitSettings = new bool[bitArray.Count];
            bitArray.CopyTo(bitSettings, 0);
            i = bitSettings.Length - 26;

            BoldBellStatus = bitSettings[--i];
            EnableOpportunityBell = bitSettings[--i];
            EnableTipArrivalBell = bitSettings[--i];
            AutoAdjustRightPaneWidth = bitSettings[--i];
            RightPaneWidth = bitSettings[--i];
            RightPaneCollapsed = bitSettings[--i];
        }

        /// <summary>
        /// Compacts some boolean settings into an integer value.
        /// </summary>
        private int BitSettings2ToInt() {
            StringBuilder stringBuilder = new StringBuilder(string.Empty.PadRight(26, Constants.Zero))
                .Append(BoldBellStatus ? 1 : 0)
                .Append(EnableOpportunityBell ? 1 : 0)
                .Append(EnableTipArrivalBell ? 1 : 0)
                .Append(AutoAdjustRightPaneWidth ? 1 : 0)
                .Append(RightPaneWidth ? 1 : 0)
                .Append(RightPaneCollapsed ? 1 : 0);
            return Convert.ToInt32(stringBuilder.ToString(), 2);
        }

        /// <summary>
        /// Expands an integer value into active panel idexes.
        /// </summary>
        private void IntToActivePanels(int i) {
            byte[] bytes = IntToByteArray(i);
            ActivePanelLeft = bytes[0];
            ActivePanelRight = bytes[1];
            ActivePreferencesPanel = bytes[2];
        }

        /// <summary>
        /// Compacts some active panel idexes into an integer value.
        /// </summary>
        private int ActivePanelsToInt() {
            byte[] bytes = new byte[] {
                (byte)ActivePanelLeft,
                (byte)ActivePanelRight,
                (byte)ActivePreferencesPanel,
                0
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
        private void IntToByteSettings1(int i) {
            byte[] bytes = IntToByteArray(i);
            TabAppearance = (TabAppearance)(bytes[0] > 2 ? 1 : bytes[0]);
            ExtensionFilterIndex = bytes[1];
            StripRenderMode = (ToolStripRenderMode)(bytes[2] > 0 ? bytes[2] > 3 ? 1 : bytes[2] : 1);
            NumberFormatInt = numberFormatHandler.AdjustIndex(bytes[3]);
        }

        /// <summary>
        /// Compacts some byte values into an integer value.
        /// </summary>
        private int ByteSettings1ToInt() {
            byte[] bytes = new byte[] {
                (byte)TabAppearance,
                (byte)ExtensionFilterIndex,
                (byte)StripRenderMode,
                (byte)NumberFormatInt
            };
            return ByteArrayToInt(bytes);
        }

        /// <summary>
        /// Expands an integer value into byte values.
        /// </summary>
        private void IntToByteSettings2(int i) {
            byte[] bytes = IntToByteArray(i);
            TipArrivalBellIndex = bytes[0];
            OpportunityBellIndex = bytes[1];
            TurnOffTheMonitorsInterval = bytes[2];
        }

        /// <summary>
        /// Compacts some byte values into an integer value.
        /// </summary>
        private int ByteSettings2ToInt() {
            byte[] bytes = new byte[] {
                (byte)TipArrivalBellIndex,
                (byte)OpportunityBellIndex,
                TurnOffTheMonitorsInterval,
                0
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

        /// <summary>Loads remote configuration file.</summary>
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

        /// <summary>Saves remote configuration file.</summary>
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

        /// <summary>Clean up any resources being used.</summary>
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

        /// <summary>
        /// Gets the path to the external editor executable for viewing log
        /// files outside the application.
        /// </summary>
        private static string GetExternalEditor() {
            string parentFolder = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            string[] directoryPaths = new string[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Path.Combine(parentFolder, Constants.GenericProgramFilesDirName),
                Path.Combine(parentFolder, Constants.GenericProgramFilesX86DirName)
            };
            foreach (string directoryPath in directoryPaths) {
                try {
                    string path = Path.Combine(directoryPath, Constants.ExternalEditorApplicationName, Constants.ExternalEditorFileName);
                    if (File.Exists(path)) {
                        return path;
                    }
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
            return null;
        }
    }
}
