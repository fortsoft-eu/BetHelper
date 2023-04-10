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
 * Version 1.1.0.0
 */

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args) {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                StringBuilder title = new StringBuilder()
                    .Append(GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                MessageBox.Show(Properties.Resources.MessageApplicationCannotRun, title.ToString(), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

                    StringBuilder title = new StringBuilder()
                        .Append(GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionEncoderDecoder);
                    SingleInstance.Run(new EncDecForm(settings), title.ToString());

                } else if (args.Length.Equals(2)
                        && (args[0].Equals(Constants.CommandLineSwitchUD) || args[0].Equals(Constants.CommandLineSwitchWD))) {

                    Application.Run(new EncDecForm(settings, args[1]));

                } else if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUE) || args[0].Equals(Constants.CommandLineSwitchWE))) {

                    settings.LoadConfig();
                    StringBuilder title = new StringBuilder()
                        .Append(GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionEditRemoteConfig);
                    SingleInstance.Run(new ConfigEditForm(settings), title.ToString());

                } else if (args.Length.Equals(1)
                        && (args[0].Equals(Constants.CommandLineSwitchUL) || args[0].Equals(Constants.CommandLineSwitchWL))) {

                    StringBuilder title = new StringBuilder()
                        .Append(GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionLogViewer);
                    SingleInstance.Run(new LogViewerForm(settings), title.ToString());

                } else {
                    noSwitch = true;
                    settings.LoadConfig();
                    SingleInstance.Run(new MainForm(settings), GetTitle(), StringComparison.InvariantCulture);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                MessageBox.Show(exception.Message, title.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (noSwitch) {
                    MessageBox.Show(Properties.Resources.MessageApplicationError, GetTitle(), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        public static string GetTitle() {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = null;
            if (attributes.Length > 0) {
                AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)attributes[0];
                title = assemblyTitleAttribute.Title;
            }
            return string.IsNullOrEmpty(title) ? Application.ProductName : title;
        }

        public static bool IsDebugging {
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
