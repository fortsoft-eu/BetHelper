﻿/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2023-2024 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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
 * Version 1.1.17.9
 */

using FortSoft.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class ServiceForm : Form {
        private decimal price;
        private Form dialog;
        private int textBoxClicks;
        private PersistWindowState persistWindowState;
        private Point location;
        private Service service;
        private Settings settings;
        private string text;
        private Timer textBoxClicksTimer;

        private delegate void ServiceFormFormCallback();

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
        public event EventHandler CtrlIPressed;
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

        public ServiceForm(Settings settings) : this(settings, null) { }

        public ServiceForm(Settings settings, PersistWindowState persistWindowState) {
            this.settings = settings;
            this.persistWindowState = persistWindowState;

            Icon = Properties.Resources.Service;
            Text = Properties.Resources.CaptionNewService;
            text = Properties.Resources.CaptionNewService;

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            if (this.persistWindowState == null) {
                this.persistWindowState = new PersistWindowState();
                this.persistWindowState.AllowSaveTopMost = true;
                this.persistWindowState.FixHeight = true;
                this.persistWindowState.SavingOptions = PersistWindowState.PersistWindowStateSavingOptions.None;
            }
            this.persistWindowState.Parent = this;
            this.persistWindowState.Loaded += new EventHandler<PersistWindowStateEventArgs>((sender, e) => {
                checkBoxTopMost.Checked = TopMost;
            });

            InitializeComponent();
            BuildContextMenuAsync();

            StringBuilder customFormat = new StringBuilder()
                .Append(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern)
                .Append(Constants.Space)
                .Append(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortTimePattern);
            dateTimePickerExpiration.Format = DateTimePickerFormat.Custom;
            dateTimePickerExpiration.CustomFormat = customFormat.ToString();
            dateTimePickerSubscribed.Format = DateTimePickerFormat.Custom;
            dateTimePickerSubscribed.CustomFormat = customFormat.ToString();
            textBoxPrice.Text = ShowPrice(0m);
            comboBoxSpan.DataSource = Enum.GetValues(typeof(Service.SpanUnit));
            comboBoxStatus.DataSource = Enum.GetValues(typeof(Service.ServiceStatus));
        }

        public Service Service {
            get {
                return service;
            }
            set {
                service = value;
                text = Properties.Resources.CaptionEditService;
                textBoxName.Text = service.Name;
                if (string.IsNullOrWhiteSpace(textBoxName.Text)) {
                    Text = text;
                } else {
                    Text = new StringBuilder(text)
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(textBoxName.Text.Trim())
                        .ToString();
                }
                price = service.Price;
                textBoxPrice.Text = ShowPrice(service.Price);
                numericUpDownSpan.Value = service.Span;
                comboBoxSpan.SelectedIndex = (int)service.Unit;
                dateTimePickerExpiration.Value = service.Expiration;
                dateTimePickerSubscribed.Value = service.Subscribed;
                comboBoxStatus.SelectedIndex = (int)service.Status;
                service.PersistWindowState = persistWindowState;
                service.Settings = settings;
                service.StatusChanged += new EventHandler(OnStatusChanged);
            }
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Undo())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Cut())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Copy())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Paste())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysDelete))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.CanUndo;
                    bool enabled = textBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = enabled;
                    contextMenu.MenuItems[3].Enabled = enabled;
                    contextMenu.MenuItems[5].Enabled = enabled;
                    contextMenu.MenuItems[7].Enabled = textBox.SelectionLength < textBox.TextLength;
                    try {
                        contextMenu.MenuItems[4].Enabled = Clipboard.ContainsText();
                    } catch (Exception exception) {
                        contextMenu.MenuItems[4].Enabled = true;
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
                textBoxName.ContextMenu = contextMenu;
                textBoxPrice.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Undo())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CUT,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_COPY,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_PASTE,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => NativeMethods.PostMessage(
                        ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Handle,
                        Constants.WM_CLEAR,
                        IntPtr.Zero,
                        IntPtr.Zero))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    TextBox textBox = (TextBox)contextMenu.SourceControl;
                    if (!textBox.Focused) {
                        textBox.Focus();
                    }
                    contextMenu.MenuItems[0].Enabled = textBox.CanUndo;
                    bool enabled = textBox.SelectionLength > 0;
                    contextMenu.MenuItems[2].Enabled = enabled;
                    contextMenu.MenuItems[3].Enabled = enabled;
                    contextMenu.MenuItems[5].Enabled = enabled;
                    contextMenu.MenuItems[7].Enabled = textBox.SelectionLength < textBox.TextLength;
                    try {
                        contextMenu.MenuItems[4].Enabled = Clipboard.ContainsText();
                    } catch (Exception exception) {
                        contextMenu.MenuItems[4].Enabled = true;
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                });
                numericUpDownSpan.ContextMenu = contextMenu;
            }));
        }

        private void EnableSave() {
            buttonSave.Enabled = !string.IsNullOrWhiteSpace(textBoxName.Text)
                && decimal.TryParse(
                    Regex.Replace(textBoxPrice.Text.Replace(Constants.Comma, Constants.Period),
                        Constants.JSBalancePattern, string.Empty),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out price);
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
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is NumericUpDown) {
                    NumericUpDown numericUpDown = (NumericUpDown)sender;
                    numericUpDown.Select(0, numericUpDown.Text.Length);
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
            } else if (e.Control && e.KeyCode.Equals(Keys.I)) {
                CtrlIPressed?.Invoke(this, EventArgs.Empty);
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

        private void OnFormLoad(object sender, EventArgs e) {
            textBoxName.Focus();
            textBoxName.SelectAll();
        }

        private void OnNameChanged(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(textBoxName.Text)) {
                Text = text;
            } else {
                Text = new StringBuilder(text)
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(textBoxName.Text.Trim())
                    .ToString();
            }
            EnableSave();
        }

        private void OnPriceChanged(object sender, EventArgs e) => EnableSave();

        private void OnStatusChanged(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OnStatusChanged), sender, e);
            } else {
                comboBoxStatus.SelectedIndex = (int)((Service)sender).Status;
            }
        }

        private void OnTextBoxMouseDown(object sender, MouseEventArgs e) {
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

        private void OnTextBoxPriceLeave(object sender, EventArgs e) {
            decimal.TryParse(
                Regex.Replace(textBoxPrice.Text.Replace(Constants.Comma, Constants.Period), Constants.JSBalancePattern, string.Empty),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out price);
            textBoxPrice.Text = ShowPrice(price);
        }

        private void OnTopMostCheckedChanged(object sender, EventArgs e) {
            TopMost = checkBoxTopMost.Checked;
            if (!TopMost) {
                SendToBack();
            }
        }

        private void OpenHelp(object sender, EventArgs e) {
            try {
                StringBuilder url = new StringBuilder()
                    .Append(Properties.Resources.Website.TrimEnd(Constants.Slash).ToLowerInvariant())
                    .Append(Constants.Slash)
                    .Append(Application.ProductName.ToLowerInvariant())
                    .Append(Constants.Slash);
                Process.Start(url.ToString());
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

        private static decimal ParsePrice(string str) {
            return decimal.Parse(
                Regex.Replace(str.Replace(Constants.Comma, Constants.Period), Constants.JSBalancePattern, string.Empty),
                CultureInfo.InvariantCulture);
        }

        private void Save(object sender, EventArgs e) {
            try {
                if (service == null) {
                    service = new Service(
                        textBoxName.Text,
                        ParsePrice(textBoxPrice.Text),
                        (int)numericUpDownSpan.Value,
                        (Service.SpanUnit)comboBoxSpan.SelectedValue,
                        dateTimePickerExpiration.Value,
                        dateTimePickerSubscribed.Value,
                        (Service.ServiceStatus)comboBoxStatus.SelectedValue);
                } else {
                    service.Price = ParsePrice(textBoxPrice.Text);
                    service.Name = textBoxName.Text;
                    service.Span = (int)numericUpDownSpan.Value;
                    service.Unit = (Service.SpanUnit)comboBoxSpan.SelectedValue;
                    service.Expiration = dateTimePickerExpiration.Value;
                    service.Subscribed = dateTimePickerSubscribed.Value;
                    service.Status = (Service.ServiceStatus)comboBoxStatus.SelectedValue;
                }
                DialogResult = DialogResult.OK;
            } catch (Exception exception) {
                ShowException(exception);
                textBoxPrice.Focus();
                textBoxPrice.SelectAll();
            }
        }

        public void SafeClose() {
            if (InvokeRequired) {
                Invoke(new ServiceFormFormCallback(SafeClose));
            } else {
                Close();
            }
        }

        public void SafeHide() {
            if (InvokeRequired) {
                Invoke(new ServiceFormFormCallback(SafeHide));
            } else if (!WindowState.Equals(FormWindowState.Minimized)) {
                WindowState = FormWindowState.Minimized;
            }
        }

        public void SafeShow() {
            if (InvokeRequired) {
                Invoke(new ServiceFormFormCallback(SafeShow));
            } else {
                persistWindowState.Restore();
            }
        }

        public void SafeSelect() {
            if (InvokeRequired) {
                Invoke(new ServiceFormFormCallback(SafeSelect));
            } else {
                persistWindowState.Restore();
                persistWindowState.BringToFront();
            }
        }

        private void ShowException(Exception exception) {
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
            messageForm.F7Pressed += new EventHandler((sender, e) => F7Pressed?.Invoke(sender, e));
            messageForm.HelpRequested += new HelpEventHandler(OpenHelp);
            dialog = messageForm;
            messageForm.ShowDialog(this);
        }

        private string ShowPrice(decimal price) {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(price.ToString(Constants.TwoDecimalPlaces, settings.NumberFormat.cultureInfo))
                .Append(Constants.Space)
                .Append(Properties.Resources.LabelCurrencySymbol);
            return stringBuilder.ToString();
        }
    }
}
