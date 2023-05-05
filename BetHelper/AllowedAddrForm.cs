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
 * Version 1.1.7.0
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class AllowedAddrForm : Form {
        private AllowedAddrHandler allowedAddrHandler;
        private Form dialog;
        private int clickedIndex, textBoxClicks;
        private PersistWindowState persistWindowState;
        private Point location;
        private Timer textBoxClicksTimer;

        public event EventHandler F7Pressed;

        public AllowedAddrForm(Settings settings) {
            allowedAddrHandler = settings.AllowedAddrHandler;
            allowedAddrHandler.Added += new EventHandler((sender, e) => {
                listBox.Items.Add(textBox.Text);
                textBox.Clear();
            });

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.Parent = this;

            InitializeComponent();
            BuildContextMenuAsync();

            listBox.Items.AddRange(allowedAddrHandler.Items);
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo,
                    new EventHandler((sender, e) => textBox.Undo())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut,
                    new EventHandler((sender, e) => textBox.Cut())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy,
                    new EventHandler((sender, e) => textBox.Copy())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste,
                    new EventHandler((sender, e) => textBox.Paste())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete,
                    new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysDelete))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler((sender, e) => textBox.SelectAll())));
                contextMenu.Popup += new EventHandler((sender, e) => {
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
                textBox.ContextMenu = contextMenu;
            }))
            .ContinueWith(new Action<Task>(task => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyAsText,
                    new EventHandler(CopyAsText)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemRemoveSelected,
                    new EventHandler(DeleteSelected)));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(string.Empty, new EventHandler(ToggleSelect)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemInvertSelection,
                    new EventHandler(InvertSelection)));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll,
                    new EventHandler(SelectAll)));
                contextMenu.Popup += new EventHandler((sender, e) => {
                    contextMenu.MenuItems[0].Enabled = listBox.SelectedIndex >= 0;
                    contextMenu.MenuItems[1].Enabled = listBox.SelectedIndex >= 0;
                    contextMenu.MenuItems[3].Enabled = clickedIndex >= 0;
                    contextMenu.MenuItems[4].Enabled = listBox.Items.Count > 0;
                    contextMenu.MenuItems[5].Enabled = listBox.Items.Count > listBox.SelectedItems.Count;
                    if (clickedIndex >= 0) {
                        contextMenu.MenuItems[3].Text = listBox.GetSelected(clickedIndex)
                            ? Properties.Resources.MenuItemUnselect
                            : Properties.Resources.MenuItemSelect;
                    } else {
                        contextMenu.MenuItems[3].Text = Properties.Resources.MenuItemSelect;
                    }
                });
                listBox.ContextMenu = contextMenu;
            }));
        }

        private void CopyAsText(object sender, EventArgs e) {
            List<string> items = new List<string>(listBox.SelectedItems.Count);
            foreach (string item in listBox.SelectedItems) {
                items.Add(item);
            }
            if (items.Count > 0) {
                try {
                    Clipboard.SetText(string.Join(Environment.NewLine, items));
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void DeleteSelected(object sender, EventArgs e) {
            if (listBox.SelectedIndex >= 0) {
                string message = listBox.SelectedItems.Count > 1
                    ? string.Format(Properties.Resources.MessageDeleteIpAddresses, listBox.SelectedItems.Count)
                    : Properties.Resources.MessageDeleteIpAddress;
                MessageForm messageForm = new MessageForm(this, message, Properties.Resources.CaptionQuestion, MessageForm.Buttons.YesNo,
                    MessageForm.BoxIcon.Question);
                messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                dialog = messageForm;
                if (messageForm.ShowDialog(this).Equals(DialogResult.Yes)) {
                    while (listBox.SelectedIndex >= 0) {
                        listBox.Items.RemoveAt(listBox.SelectedIndex);
                    }
                    string[] items = new string[listBox.Items.Count];
                    for (int i = 0; i < listBox.Items.Count; i++) {
                        items[i] = (string)listBox.Items[i];
                    }
                    allowedAddrHandler.Items = items;
                }
            }
        }

        private void InvertSelection(object sender, EventArgs e) {
            for (int i = 0; i < listBox.Items.Count; i++) {
                listBox.SetSelected(i, !listBox.GetSelected(i));
            }
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnAddClick(object sender, EventArgs e) => allowedAddrHandler.Add(textBox.Text);

        private void OnFormClosing(object sender, FormClosingEventArgs e) => textBoxClicksTimer.Dispose();

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is ListBox || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    SelectAll(sender, e);
                }
            } else if (e.Control && e.KeyCode.Equals(Keys.C)) {
                if (sender is ListBox || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    CopyAsText(sender, e);
                }
            } else if (e.KeyCode.Equals(Keys.Delete)) {
                if (sender is ListBox || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    DeleteSelected(sender, e);
                }
            } else if (e.KeyCode.Equals(Keys.F7)) {
                F7Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnListBoxMouseDown(object sender, MouseEventArgs e) {
            if (e.Button.Equals(MouseButtons.Right)) {
                clickedIndex = listBox.IndexFromPoint(e.Location);
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

        private void OnTextChanged(object sender, EventArgs e) => buttonAdd.Enabled = allowedAddrHandler.CheckIpSimple(textBox.Text);

        private void OnSelectedIndexChanged(object sender, EventArgs e) => buttonRemove.Enabled = listBox.SelectedIndex >= 0;

        private void SelectAll(object sender, EventArgs e) {
            for (int i = 0; i < listBox.Items.Count; i++) {
                listBox.SetSelected(i, true);
            }
        }

        private void ToggleSelect(object sender, EventArgs e) {
            if (clickedIndex >= 0) {
                listBox.SetSelected(clickedIndex, !listBox.GetSelected(clickedIndex));
            }
        }
    }
}
