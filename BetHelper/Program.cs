﻿/**
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
 * Version 1.1.9.0
 */

using CefSharp;
using FortSoft.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main(string[] args) {
            if (!Environment.OSVersion.Platform.Equals(PlatformID.Win32NT)) {
                MessageBox.Show(Properties.Resources.MessageApplicationCannotRun, GetTitle(Properties.Resources.CaptionError),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Settings settings = new Settings();
            if (!settings.DisableThemes) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                settings.RenderWithVisualStyles = Application.RenderWithVisualStyles;
            }

            bool noSwitch = false;
            try {
                if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUC) || args[0].Equals(Constants.CommandLineSwitchWC))) {

                    Application.Run(new CalculatorForm(settings));

                } else if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUD) || args[0].Equals(Constants.CommandLineSwitchWD))) {

                    SingleInstance.Run(new EncDecForm(settings), GetTitle(Properties.Resources.CaptionEncoderDecoder));

                } else if (args.Length.Equals(2)
                        && (args[0].Equals(Constants.CommandLineSwitchUD) || args[0].Equals(Constants.CommandLineSwitchWD))) {

                    Application.Run(new EncDecForm(settings, args[1]));

                } else if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUE) || args[0].Equals(Constants.CommandLineSwitchWE))) {

                    settings.LoadConfig();
                    SingleInstance.Run(new ConfigEditForm(settings), GetTitle(Properties.Resources.CaptionEditRemoteConfig));

                } else if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUL) || args[0].Equals(Constants.CommandLineSwitchWL))) {

                    SingleInstance.Run(new LogViewerForm(settings), GetTitle(Properties.Resources.CaptionLogViewer));

                } else {
                    noSwitch = true;
                    settings.LoadConfig();
                    MainForm mainForm = new MainForm(settings);
                    SingleInstance.Run(mainForm, GetTitle(), StringComparison.InvariantCulture);
                    string dataFilePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath),
                        Constants.RightPaneDataFileName);
                    try {
                        if (!mainForm.SuppressSaveData) {
                            using (FileStream fileStream = File.Create(dataFilePath)) {
                                new BinaryFormatter().Serialize(fileStream, mainForm.Data);
                            }
                        }
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                    try {
                        Cef.PreShutdown();
                        Cef.Shutdown();
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                    if (mainForm.Restart) {
                        Application.Restart();
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                MessageBox.Show(exception.Message, GetTitle(Properties.Resources.CaptionError), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                if (noSwitch) {
                    MessageBox.Show(Properties.Resources.MessageApplicationError, GetTitle(), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        internal static string GetTitle() {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = null;
            if (attributes.Length > 0) {
                AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)attributes[0];
                title = assemblyTitleAttribute.Title;
            }
            return string.IsNullOrEmpty(title) ? Application.ProductName : title;
        }

        internal static string GetTitle(string title) {
            return new StringBuilder()
                .Append(GetTitle())
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(title)
                .ToString();
        }

        internal static bool IsDebugging {
            get {
                bool isDebugging = false;
                Debugging(ref isDebugging);
                return isDebugging;
            }
        }

        [Conditional("DEBUG")]
        private static void Debugging(ref bool isDebugging) => isDebugging = true;
    }
}
