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
 * Version 1.1.8.0
 */

using FortSoft.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class OpenForm : Form {
        private Form dialog;
        private int index, textBoxClicks;
        private PersistWindowState persistWindowState;
        private Point location;
        private Timer textBoxClicksTimer;
        private TypedUrlsHandler typedUrlsHandler;
        private WebInfoHandler webInfoHandler;

        public event EventHandler F7Pressed;

        public OpenForm(WebInfoHandler webInfoHandler) {
            this.webInfoHandler = webInfoHandler;

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.FixHeight = true;
            persistWindowState.Parent = this;

            InitializeComponent();

            BuildContextMenuAsync();

            typedUrlsHandler = new TypedUrlsHandler();
            typedUrlsHandler.Saved += new EventHandler(UpdateUrlComboBoxList);
            typedUrlsHandler.Loaded += new EventHandler(UpdateUrlComboBoxList);
            typedUrlsHandler.Load();

            comboBoxUrl.MaxDropDownItems = typedUrlsHandler.MaximumTypedUrls;
            comboBoxUrl.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxUrl.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysCtrlZ))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CUT,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_COPY,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_PASTE,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CLEAR,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((ComboBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    ComboBox comboBox = (ComboBox)contextMenu.SourceControl;
                    if (!comboBox.Focused) {
                        comboBox.Focus();
                    }
                    bool enabled = comboBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = enabled;
                    contextMenu.MenuItems[3].Enabled = enabled;
                    contextMenu.MenuItems[5].Enabled = enabled;
                    contextMenu.MenuItems[7].Enabled = comboBox.SelectionLength < comboBox.Text.Length;
                    try {
                        contextMenu.MenuItems[4].Enabled = Clipboard.ContainsText();
                    } catch (Exception exception) {
                        contextMenu.MenuItems[4].Enabled = true;
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
                comboBoxUrl.ContextMenu = contextMenu;
            }));
        }

        private void UpdateUrlComboBoxList(object sender, EventArgs e) {
            comboBoxUrl.Items.Clear();
            comboBoxUrl.Items.AddRange(((TypedUrlsHandler)sender).Get());
        }

        private void OnButtonOpenClick(object sender, EventArgs e) {
            string url = comboBoxUrl.Text;
            try {
                if (!Regex.IsMatch(url, Constants.SchemePresenceTestPattern, RegexOptions.IgnoreCase)) {
                    url = new StringBuilder()
                        .Append(Constants.SchemeHttps)
                        .Append(Constants.Colon)
                        .Append(Constants.Slash)
                        .Append(Constants.Slash)
                        .Append(url.TrimStart(Constants.Colon, Constants.Slash))
                        .ToString();
                }
                Uri uri = new Uri(url);
                if (uri.Scheme.Equals(Constants.SchemeHttp)) {
                    UriBuilder uriBuilder = new UriBuilder(uri);
                    uriBuilder.Scheme = Constants.SchemeHttps;
                    uri = uriBuilder.Uri;
                }
                if (uri.Scheme.Equals(Constants.SchemeHttps)) {
                    if (uri.HostNameType.Equals(UriHostNameType.Dns)) {
                        index = webInfoHandler.LoadUrl(uri.AbsoluteUri);
                        if (index >= 0) {
                            typedUrlsHandler.Add(uri.AbsoluteUri);
                            typedUrlsHandler.Save();
                            DialogResult = DialogResult.OK;
                        } else {
                            StringBuilder message = new StringBuilder()
                                .AppendFormat(Properties.Resources.MessageCannotOpen, comboBoxUrl.Text)
                                .Append(Constants.Space)
                                .Append(Properties.Resources.MessageUrlOutOfUrlSet);
                            StringBuilder title = new StringBuilder()
                                .Append(Program.GetTitle())
                                .Append(Constants.Space)
                                .Append(Constants.EnDash)
                                .Append(Constants.Space)
                                .Append(Properties.Resources.CaptionWarning);
                            MessageForm messageForm = new MessageForm(this, message.ToString(), title.ToString(), MessageForm.Buttons.OK,
                                MessageForm.BoxIcon.Warning);
                            messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                            dialog = messageForm;
                            messageForm.ShowDialog(this);
                        }
                    } else {
                        StringBuilder title = new StringBuilder()
                            .Append(Program.GetTitle())
                            .Append(Constants.Space)
                            .Append(Constants.EnDash)
                            .Append(Constants.Space)
                            .Append(Properties.Resources.CaptionWarning);
                        MessageForm messageForm = new MessageForm(this,
                            string.Format(Properties.Resources.MessageUnsupportedHostNameType, comboBoxUrl.Text), title.ToString(),
                            MessageForm.Buttons.OK, MessageForm.BoxIcon.Warning);
                        messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                        dialog = messageForm;
                        messageForm.ShowDialog(this);
                    }
                } else {
                    StringBuilder title = new StringBuilder()
                        .Append(Program.GetTitle())
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(Properties.Resources.CaptionWarning);
                    MessageForm messageForm = new MessageForm(this, string.Format(Properties.Resources.MessageUnsupportedScheme,
                        comboBoxUrl.Text, uri.Scheme), title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Warning);
                    messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                    dialog = messageForm;
                    messageForm.ShowDialog(this);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                MessageForm messageForm = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK,
                    MessageForm.BoxIcon.Error);
                messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                dialog = messageForm;
                messageForm.ShowDialog(this);
            }
        }

        private void OnDropDown(object sender, EventArgs e) {
            ComboBox comboBox = (ComboBox)sender;
            Graphics graphics = comboBox.CreateGraphics();
            int scrollBarWidth = comboBox.Items.Count > comboBox.MaxDropDownItems ? SystemInformation.VerticalScrollBarWidth : 0;
            int dropDownWidth = comboBox.Width;
            foreach (object obj in comboBox.Items) {
                string str = obj.ToString();
                int newWidth = (int)graphics.MeasureString(str, comboBox.Font).Width + scrollBarWidth;
                if (newWidth > dropDownWidth) {
                    dropDownWidth = newWidth;
                }
            }
            comboBox.DropDownWidth = dropDownWidth;
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => textBoxClicksTimer.Dispose();

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is ComboBox) {
                    ((ComboBox)sender).SelectAll();
                }
            } else if (e.KeyCode.Equals(Keys.F7)) {
                F7Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            ComboBox comboBox = (ComboBox)sender;
            textBoxClicksTimer.Stop();
            if (comboBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks.Equals(0) || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks.Equals(3)) {
                textBoxClicks = 0;
                NativeMethods.MouseEvent(
                    Constants.MOUSEEVENTF_LEFTUP,
                    Convert.ToUInt32(Cursor.Position.X),
                    Convert.ToUInt32(Cursor.Position.Y),
                    0,
                    0);
                comboBox.SelectAll();
                comboBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            comboBoxUrl.Focus();
            comboBoxUrl.SelectAll();
        }
    }
}
