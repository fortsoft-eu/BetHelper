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
 * Version 1.1.17.4
 */

using FortSoft.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class WebInfoForm : Form {
        private int textBoxClicks;
        private PersistWindowState persistWindowState;
        private Point location;
        private Timer textBoxClicksTimer;
        private WebInfo webInfo;

        private delegate void WebInfoFormCallback();
        private delegate void CopyCallback(string str);

        public event EventHandler AltCtrlShiftEPressed;
        public event EventHandler AltCtrlShiftPPressed;
        public event EventHandler AltF10Pressed;
        public event EventHandler AltF11Pressed;
        public event EventHandler AltF12Pressed;
        public event EventHandler AltF5Pressed;
        public event EventHandler AltF7Pressed;
        public event EventHandler AltF8Pressed;
        public event EventHandler AltF9Pressed;
        public event EventHandler AltHomePressed;
        public event EventHandler AltLeftPressed;
        public event EventHandler AltLPressed;
        public event EventHandler AltRightPressed;
        public event EventHandler CtrlDPressed;
        public event EventHandler CtrlEPressed;
        public event EventHandler CtrlFPressed;
        public event EventHandler CtrlF5Pressed;
        public event EventHandler CtrlGPressed;
        public event EventHandler CtrlMinusPressed;
        public event EventHandler CtrlMPressed;
        public event EventHandler CtrlOPressed;
        public event EventHandler CtrlPlusPressed;
        public event EventHandler CtrlPPressed;
        public event EventHandler CtrlShiftDelPressed;
        public event EventHandler CtrlShiftEPressed;
        public event EventHandler CtrlShiftMinusPressed;
        public event EventHandler CtrlShiftNPressed;
        public event EventHandler CtrlShiftPlusPressed;
        public event EventHandler CtrlShiftPPressed;
        public event EventHandler CtrlTPressed;
        public event EventHandler CtrlUPressed;
        public event EventHandler CtrlZeroPressed;
        public event EventHandler DownPressed;
        public event EventHandler EndPressed;
        public event EventHandler F11Pressed;
        public event EventHandler F12Pressed;
        public event EventHandler F2Pressed;
        public event EventHandler F3Pressed;
        public event EventHandler F5Pressed;
        public event EventHandler F7Pressed;
        public event EventHandler F8Pressed;
        public event EventHandler F9Pressed;
        public event EventHandler HomePressed;
        public event EventHandler PageDownPressed;
        public event EventHandler PageUpPressed;
        public event EventHandler ShiftF3Pressed;
        public event EventHandler UpPressed;

        public WebInfoForm(WebInfo webInfo, PersistWindowState persistWindowState) {
            this.webInfo = webInfo;
            this.persistWindowState = persistWindowState;

            Icon = Properties.Resources.Form;
            Text = new StringBuilder()
                .Append(Properties.Resources.CaptionWebInfo)
                .Append(Constants.Space)
                .Append(Constants.NumberSign)
                .Append(Constants.Space)
                .Append(webInfo.Ordinal)
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(webInfo.Title)
                .ToString();

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            persistWindowState.Loaded += new EventHandler<PersistWindowStateEventArgs>((sender, e) => checkBoxTopMost.Checked = TopMost);

            InitializeComponent();
            BuildContextMenuAsync();

            SuspendLayout();
            textBoxTitle.Text = string.IsNullOrEmpty(webInfo.Title) ? Properties.Resources.CaptionNotSet : webInfo.Title;
            textBoxUrl.Text = string.IsNullOrEmpty(webInfo.Url) ? Properties.Resources.CaptionNotSet : webInfo.Url;
            textBoxUrlLive.Text = string.IsNullOrEmpty(webInfo.UrlLive) ? Properties.Resources.CaptionNotSet : webInfo.UrlLive;
            textBoxUrlNext.Text = string.IsNullOrEmpty(webInfo.UrlNext) ? Properties.Resources.CaptionNotSet : webInfo.UrlNext;
            textBoxTips.Text = string.IsNullOrEmpty(webInfo.UrlTips) ? Properties.Resources.CaptionNotSet : webInfo.UrlTips;
            textBoxUserName.Text = string.IsNullOrEmpty(webInfo.UserName) ? Properties.Resources.CaptionNotSet : webInfo.UserName;
            maskedTextBoxPassword.Text = webInfo.Password;
            maskedTextBoxPassword.Visible = !string.IsNullOrEmpty(webInfo.Password);
            textBoxPassword.Text = Properties.Resources.CaptionNotSet;
            textBoxPassword.Visible = string.IsNullOrEmpty(webInfo.Password);
            textBoxScript.Text = string.IsNullOrEmpty(webInfo.Script) ? Properties.Resources.CaptionNotSet : webInfo.Script;
            textBoxPattern.Text = string.IsNullOrEmpty(webInfo.Pattern) ? Properties.Resources.CaptionNotSet : webInfo.Pattern;
            textBoxFields.Text = string.IsNullOrEmpty(webInfo.Fields) ? Properties.Resources.CaptionNotSet : webInfo.Fields;
            textBoxDisplayName.Text = string.IsNullOrEmpty(webInfo.DisplayName)
                ? Properties.Resources.CaptionNotSet
                : webInfo.DisplayName;
            checkBoxIsService.Checked = webInfo.IsService;
            checkBoxIsActuallyService.Checked = webInfo.IsActuallyService;
            checkBoxAudioMutedByDefault.Checked = webInfo.AudioMutedByDefault;
            checkBoxHandlePopUps.Checked = webInfo.HandlePopUps;
            checkBoxTabNavigation.Checked = webInfo.TabNavigation;
            checkBoxWillHandlePopUps.Checked = webInfo.WillActuallyHandlePopUps;
            checkBoxWillTryToKeep.Checked = webInfo.WillTryToKeepUserLoggedIn;
            textBoxPopUpWidth.Text = webInfo.PopUpWidth.Equals(0)
                ? Properties.Resources.CaptionNotSet
                : webInfo.PopUpWidth.ToString().Replace(Constants.Hyphen, Constants.MinusSign);
            textBoxPopUpHeight.Text = webInfo.PopUpHeight.Equals(0)
                ? Properties.Resources.CaptionNotSet
                : webInfo.PopUpHeight.ToString().Replace(Constants.Hyphen, Constants.MinusSign);
            textBoxPopUpLeft.Text = webInfo.PopUpLeft.Equals(0)
                ? Properties.Resources.CaptionNotSet
                : webInfo.PopUpLeft.ToString().Replace(Constants.Hyphen, Constants.MinusSign);
            textBoxPopUpTop.Text = webInfo.PopUpTop.Equals(0)
                ? Properties.Resources.CaptionNotSet
                : webInfo.PopUpTop.ToString().Replace(Constants.Hyphen, Constants.MinusSign);
            textBoxIetfLanguageTag.Text = string.IsNullOrEmpty(webInfo.IetfLanguageTag)
                ? Properties.Resources.CaptionNotSet
                : webInfo.IetfLanguageTag;
            textBoxBackNavigation.Text = webInfo.BackNavigation.ToString();
            if (webInfo.AllowedHosts == null) {
                textBoxAllowedHosts.Text = Properties.Resources.CaptionNotSet;
            } else {
                webInfo.AllowedHosts.Sort();
                textBoxAllowedHosts.Text = string.Join(Constants.Comma.ToString() + Constants.Space.ToString(), webInfo.AllowedHosts);
            }
            if (webInfo.ChatHosts == null) {
                textBoxChatHosts.Text = Properties.Resources.CaptionNotSet;
            } else {
                webInfo.ChatHosts.Sort();
                textBoxChatHosts.Text = string.Join(Constants.Comma.ToString() + Constants.Space.ToString(), webInfo.ChatHosts);
            }

            buttonCopyUrl.Enabled = !string.IsNullOrEmpty(webInfo.Url);
            buttonCopyUserName.Enabled = !string.IsNullOrEmpty(webInfo.UserName);
            buttonCopyPassword.Enabled = !string.IsNullOrWhiteSpace(webInfo.Password);
            ResumeLayout(false);
            PerformLayout();
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy, new EventHandler(Copy)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll, new EventHandler(SelectAll)));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    if (contextMenu.SourceControl is TextBox) {
                        TextBox textBox = (TextBox)contextMenu.SourceControl;
                        if (!textBox.Focused) {
                            textBox.Focus();
                        }
                        contextMenu.MenuItems[0].Enabled = textBox.SelectionLength > 0;
                        contextMenu.MenuItems[2].Enabled = textBox.SelectionLength < textBox.TextLength;
                    } else {
                        MaskedTextBox maskedTextBox = (MaskedTextBox)contextMenu.SourceControl;
                        if (!maskedTextBox.Focused) {
                            maskedTextBox.Focus();
                        }
                        contextMenu.MenuItems[0].Enabled = false;
                        contextMenu.MenuItems[2].Enabled = maskedTextBox.SelectionLength < maskedTextBox.TextLength;
                    }
                });
                foreach (Control control in Controls) {
                    if (control is TextBox || control is MaskedTextBox) {
                        control.ContextMenu = contextMenu;
                    }
                }
                textBoxAllowedHosts.ContextMenu = contextMenu;
                textBoxChatHosts.ContextMenu = contextMenu;
            }));
        }

        private void Copy(string str) {
            try {
                if (webInfo.Parent.Form.InvokeRequired) {
                    webInfo.Parent.Form.Invoke(new CopyCallback(Copy), str);
                } else {
                    Clipboard.SetText(str);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void Copy(object sender, EventArgs e) {
            Control control = ((MenuItem)sender).GetContextMenu().SourceControl;
            if (control is TextBox) {
                ((TextBox)control).Copy();
            } else {
                ((MaskedTextBox)control).Copy();
            }
        }

        private void GripStyle(object sender, EventArgs e) {
            SizeGripStyle = WindowState.Equals(FormWindowState.Normal) ? SizeGripStyle.Show : SizeGripStyle.Hide;
        }

        private void SelectAll(object sender, EventArgs e) {
            Control control = ((MenuItem)sender).GetContextMenu().SourceControl;
            if (control is TextBox) {
                ((TextBox)control).SelectAll();
            } else {
                ((MaskedTextBox)control).SelectAll();
            }
        }

        public void SafeClose() {
            if (InvokeRequired) {
                Invoke(new WebInfoFormCallback(SafeClose));
            } else {
                Close();
            }
        }

        public void SafeHide() {
            if (InvokeRequired) {
                Invoke(new WebInfoFormCallback(SafeHide));
            } else if (!WindowState.Equals(FormWindowState.Minimized)) {
                WindowState = FormWindowState.Minimized;
            }
        }

        public void SafeShow() {
            if (InvokeRequired) {
                Invoke(new WebInfoFormCallback(SafeShow));
            } else {
                persistWindowState.Restore();
            }
        }

        public void SafeSelect() {
            if (InvokeRequired) {
                Invoke(new WebInfoFormCallback(SafeSelect));
            } else {
                persistWindowState.Restore();
                persistWindowState.BringToFront();
            }
        }

        private void OnCheckedChanged(object sender, EventArgs e) {
            checkBoxIsService.Checked = webInfo.IsService;
            checkBoxIsActuallyService.Checked = webInfo.IsActuallyService;
            checkBoxAudioMutedByDefault.Checked = webInfo.AudioMutedByDefault;
            checkBoxHandlePopUps.Checked = webInfo.HandlePopUps;
            checkBoxTabNavigation.Checked = webInfo.TabNavigation;
            checkBoxWillHandlePopUps.Checked = webInfo.WillActuallyHandlePopUps;
            checkBoxWillTryToKeep.Checked = webInfo.WillTryToKeepUserLoggedIn;
        }

        private void OnCopyPasswordClick(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(webInfo.Password)) {
                Copy(webInfo.Password);
            }
        }

        private void OnCopyUrlClick(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(webInfo.Url)) {
                Copy(webInfo.Url);
            }
        }

        private void OnCopyUserNameClick(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(webInfo.UserName)) {
                Copy(webInfo.UserName);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => textBoxClicksTimer.Dispose();

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is MaskedTextBox) {
                    ((TextBox)sender).SelectAll();
                }
            } else if (e.Alt && e.Control && e.Shift && e.KeyCode.Equals(Keys.E)) {
                AltCtrlShiftEPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.Control && e.Shift && e.KeyCode.Equals(Keys.P)) {
                AltCtrlShiftPPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F5)) {
                AltF5Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F7)) {
                AltF7Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F8)) {
                AltF8Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F9)) {
                AltF9Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F10)) {
                AltF10Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F11)) {
                AltF11Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.F12)) {
                AltF12Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.Home)) {
                AltHomePressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.Left)) {
                AltLeftPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.Right)) {
                AltRightPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.KeyCode.Equals(Keys.L)) {
                AltLPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.Home)) {
                e.SuppressKeyPress = true;
                HomePressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.End)) {
                e.SuppressKeyPress = true;
                EndPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.PageUp)) {
                e.SuppressKeyPress = true;
                PageUpPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.PageDown)) {
                e.SuppressKeyPress = true;
                PageDownPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.Up)) {
                e.SuppressKeyPress = true;
                UpPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.Down)) {
                e.SuppressKeyPress = true;
                DownPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && (e.KeyCode.Equals(Keys.Add) || e.KeyCode.Equals(Keys.Oemplus))) {
                CtrlShiftPlusPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && (e.KeyCode.Equals(Keys.Subtract) || e.KeyCode.Equals(Keys.OemMinus))) {
                CtrlShiftMinusPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && e.KeyCode.Equals(Keys.Delete)) {
                CtrlShiftDelPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && e.KeyCode.Equals(Keys.E)) {
                CtrlShiftEPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && e.KeyCode.Equals(Keys.N)) {
                CtrlShiftNPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.Shift && e.KeyCode.Equals(Keys.P)) {
                CtrlShiftPPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && (e.KeyCode.Equals(Keys.Add) || e.KeyCode.Equals(Keys.Oemplus))) {
                CtrlPlusPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && (e.KeyCode.Equals(Keys.Subtract) || e.KeyCode.Equals(Keys.OemMinus))) {
                CtrlMinusPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.NumPad0)) {
                CtrlZeroPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.D)) {
                CtrlDPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.E)) {
                CtrlEPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.F)) {
                CtrlFPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.G)) {
                CtrlGPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.M)) {
                CtrlMPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.O)) {
                CtrlOPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.P)) {
                CtrlPPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.T)) {
                CtrlTPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.U)) {
                CtrlUPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Control && e.KeyCode.Equals(Keys.F5)) {
                CtrlF5Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Shift && e.KeyCode.Equals(Keys.F3)) {
                ShiftF3Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F2)) {
                F2Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F3)) {
                F3Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F5)) {
                F5Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F7)) {
                F7Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F8)) {
                F8Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F9)) {
                F9Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F11)) {
                F11Pressed?.Invoke(this, EventArgs.Empty);
            } else if (e.KeyCode.Equals(Keys.F12)) {
                F12Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
            if (textBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks.Equals(0) || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks.Equals(3)) {
                textBoxClicks = 0;
                NativeMethods.MouseEvent(Constants.MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                Application.DoEvents();
                if (textBox.Multiline) {
                    char[] chars = textBox.Text.ToCharArray();
                    int selectionEnd = Math.Min(
                        Array.IndexOf(chars, Constants.CarriageReturn, textBox.SelectionStart),
                        Array.IndexOf(chars, Constants.LineFeed, textBox.SelectionStart));
                    if (selectionEnd < 0) {
                        selectionEnd = textBox.TextLength;
                    }
                    selectionEnd = Math.Max(textBox.SelectionStart + textBox.SelectionLength, selectionEnd);
                    int selectionStart = Math.Min(textBox.SelectionStart, selectionEnd);
                    while (--selectionStart > 0
                        && !chars[selectionStart].Equals(Constants.LineFeed)
                        && !chars[selectionStart].Equals(Constants.CarriageReturn)) { }
                    textBox.Select(selectionStart, selectionEnd - selectionStart);
                } else {
                    textBox.SelectAll();
                }
                textBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private void OnPasswordMouseDown(object sender, MouseEventArgs e) {
            if (!e.Button.Equals(MouseButtons.Left)) {
                textBoxClicks = 0;
                return;
            }
            MaskedTextBox maskedTextBox = (MaskedTextBox)sender;
            textBoxClicksTimer.Stop();
            if (maskedTextBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks.Equals(0) || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks.Equals(3)) {
                textBoxClicks = 0;
                NativeMethods.MouseEvent(Constants.MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                maskedTextBox.SelectAll();
                maskedTextBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private void OnTopMostCheckedChanged(object sender, EventArgs e) {
            TopMost = checkBoxTopMost.Checked;
            if (!TopMost) {
                SendToBack();
            }
        }
    }
}
