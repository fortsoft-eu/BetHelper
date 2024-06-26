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
 * Version 1.1.17.10
 */

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace BetHelper {
    public partial class MatchControl : UserControl {
        private Game game;
        private int textBoxClicks;
        private Point location;
        private Settings settings;

        public event EventHandler<MatchControlEventArgs> GameNameChanged;

        public MatchControl(Settings settings) {
            this.settings = settings;

            InitializeComponent();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            BuildContextMenuAsync();

            StringBuilder customFormat = new StringBuilder()
                .Append(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortDatePattern)
                .Append(Constants.Space)
                .Append(settings.NumberFormat.cultureInfo.DateTimeFormat.ShortTimePattern);
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = customFormat.ToString();
        }

        public Game Game {
            get {
                if (game == null) {
                    game = new Game(
                        dateTimePicker.Value,
                        textBoxSport.Text,
                        textBoxLeague.Text,
                        textBoxMatch.Text,
                        textBoxOpportunity.Text);
                } else {
                    game.DateTime = dateTimePicker.Value;
                    game.Sport = textBoxSport.Text;
                    game.Match = textBoxMatch.Text;
                    game.League = textBoxLeague.Text;
                    game.Opportunity = textBoxOpportunity.Text;
                }
                return game;
            }
            set {
                game = value;
                dateTimePicker.Value = game.DateTime;
                textBoxSport.Text = game.Sport;
                textBoxMatch.Text = game.Match;
                textBoxLeague.Text = game.League;
                textBoxOpportunity.Text = game.Opportunity;
            }
        }

        public int Ordinal { get; set; }

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
                foreach (Control control in Controls) {
                    if (control is TextBox) {
                        control.ContextMenu = contextMenu;
                    }
                }
            }));
        }

        public void FocusSportField() {
            textBoxSport.Focus();
            textBoxSport.SelectAll();
        }

        public void FocusMatchField() {
            textBoxMatch.Focus();
            textBoxMatch.SelectAll();
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void OnTextBoxClicksTimerTick(object sender, EventArgs e) {
            textBoxClicksTimer.Stop();
            textBoxClicks = 0;
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

        private void OnTextChanged(object sender, EventArgs e) {
            GameNameChanged?.Invoke(sender, new MatchControlEventArgs(textBoxMatch.Text, Ordinal, (TabPage)Parent));
        }
    }
}
