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
 * Version 1.1.0.0
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

        public ServiceForm(Settings settings) {
            this.settings = settings;

            text = Properties.Resources.CaptionNewService;
            Text = Properties.Resources.CaptionNewService;

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveSize = true;
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.Parent = this;

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
                NativeMethods.MouseEvent(
                    Constants.MOUSEEVENTF_LEFTUP,
                    Convert.ToUInt32(Cursor.Position.X),
                    Convert.ToUInt32(Cursor.Position.Y),
                    0,
                    0);
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
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                StringBuilder title = new StringBuilder()
                    .Append(Program.GetTitle())
                    .Append(Constants.Space)
                    .Append(Constants.EnDash)
                    .Append(Constants.Space)
                    .Append(Properties.Resources.CaptionError);
                dialog = new MessageForm(this, exception.Message, title.ToString(), MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog(this);
                textBoxPrice.Focus();
                textBoxPrice.SelectAll();
            }
        }

        private string ShowPrice(decimal price) {
            StringBuilder stringBuilder = new StringBuilder()
                .Append(price.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                .Append(Constants.Space)
                .Append(Properties.Resources.LabelCurrencySymbol);
            return stringBuilder.ToString();
        }
    }
}
