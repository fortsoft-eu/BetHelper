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
 * Version 1.0.0.0
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class TipForm : Form {
        private float odd, trustDegree;
        private Form dialog;
        private int matchOrdinal, textBoxClicks;
        private MatchControl matchControl;
        private PersistWindowState persistWindowState;
        private Point location;
        private Settings settings;
        private string text;
        private Timer focusTimer, textBoxClicksTimer;
        private Tip tip;

        public TipForm(Settings settings) {
            this.settings = settings;
            matchOrdinal = 1;

            focusTimer = new Timer();
            focusTimer.Interval = Constants.FormLoadToFocusDelay;
            focusTimer.Tick += new EventHandler((sender, e) => {
                focusTimer.Stop();
                matchControl.FocusSportField();
            });

            textBoxClicksTimer = new Timer();
            textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
            textBoxClicksTimer.Tick += new EventHandler((sender, e) => {
                textBoxClicksTimer.Stop();
                textBoxClicks = 0;
            });

            text = Properties.Resources.CaptionNewTip;
            Text = Properties.Resources.CaptionNewTip;

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.FixHeight = true;
            persistWindowState.Parent = this;

            InitializeComponent();
            BuildContextMenuAsync();

            textBoxOdd.Text = ShowOdd(1);
            textBoxTrustDegree.Text = ShowTrustDegree(10);
            comboBoxStatus.DataSource = Enum.GetValues(typeof(Tip.TipStatus));

            MouseWheel += new MouseEventHandler(OnMouseWheel);
            MouseMove += new MouseEventHandler(OnMouseWheel);
        }

        public Tip Tip {
            get {
                return tip;
            }
            set {
                tip = value;
                text = Properties.Resources.CaptionEditTip;
                Text = Properties.Resources.CaptionEditTip;
                odd = tip.Odd;
                textBoxOdd.Text = ShowOdd(tip.Odd);
                textBoxBookmaker.Text = tip.Bookmaker;
                trustDegree = tip.TrustDegree;
                textBoxTrustDegree.Text = ShowTrustDegree(tip.TrustDegree);
                textBoxService.Text = tip.Service;
                comboBoxStatus.SelectedIndex = (int)tip.Status;
                foreach (Game game in tip.Games) {
                    MatchControl matchControl = new MatchControl(settings) {
                        Dock = DockStyle.Fill,
                        Game = game,
                        Ordinal = matchOrdinal++
                    };
                    matchControl.GameNameChanged += new EventHandler<MatchControlEventArgs>(OnGameNameChanged);
                    TabPage tabPage = new TabPage() { Text = game.Match.Trim(), UseVisualStyleBackColor = true };
                    tabPage.Controls.Add(matchControl);
                    if (this.matchControl == null) {
                        this.matchControl = matchControl;
                        Text = string.IsNullOrWhiteSpace(game.Match) ? text : text + Constants.Space + Constants.EnDash + Constants.Space + game.Match.Trim();
                    }
                    tabControl.TabPages.Add(tabPage);
                }
                buttonAddGame.Enabled = tabControl.TabPages.Count < Constants.GameTabsMaxCounts;
                buttonRemoveGame.Enabled = tabControl.TabPages.Count > 1;
            }
        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemUndo, new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Undo())));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCut, new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Cut())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy, new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Copy())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemPaste, new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).Paste())));
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemDelete, new EventHandler((sender, e) => SendKeys.Send(Constants.SendKeysDelete))));
                contextMenu.MenuItems.Add(Constants.Hyphen.ToString());
                contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemSelectAll, new EventHandler((sender, e) => ((TextBox)((MenuItem)sender).GetContextMenu().SourceControl).SelectAll())));
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
                textBoxBookmaker.ContextMenu = contextMenu;
                textBoxOdd.ContextMenu = contextMenu;
                textBoxTrustDegree.ContextMenu = contextMenu;
            }));
        }

        private void AddGame(object sender, EventArgs e) {
            if (tabControl.TabPages.Count < Constants.GameTabsMaxCounts) {
                AddGame().FocusSportField();
            }
        }

        private MatchControl AddGame() {
            MatchControl matchControl = new MatchControl(settings) {
                Dock = DockStyle.Fill,
                Ordinal = matchOrdinal
            };
            matchControl.GameNameChanged += new EventHandler<MatchControlEventArgs>(OnGameNameChanged);
            TabPage tabPage = new TabPage() { Text = Constants.ColumnHeaderMatch + Constants.Space + matchOrdinal++, UseVisualStyleBackColor = true };
            tabPage.Controls.Add(matchControl);
            tabControl.TabPages.Add(tabPage);
            tabControl.SelectedIndex = tabControl.TabPages.Count - 1;
            return matchControl;
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => textBoxClicksTimer.Dispose();

        private void OnFormLoad(object sender, EventArgs e) {
            if (tabControl.TabPages.Count == 0 && tabControl.TabPages.Count < Constants.GameTabsMaxCounts) {
                matchControl = AddGame();
            }
            focusTimer.Start();
        }

        private void OnGameNameChanged(object sender, MatchControlEventArgs e) {
            e.TabPage.Text = string.IsNullOrWhiteSpace(e.GameName) ? Constants.ColumnHeaderMatch + Constants.Space + e.Ordinal : e.GameName.Trim();
            if (e.Ordinal.Equals(1)) {
                Text = string.IsNullOrWhiteSpace(e.GameName) ? text : text + Constants.Space + Constants.EnDash + Constants.Space + e.GameName.Trim();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e) {
            if (ActiveForm == (Form)sender) {
                Control control = StaticMethods.FindControlAtCursor((Form)sender);
                if (control is TabControl) {
                    TabControl tabControl = (TabControl)control;
                    if (StaticMethods.IsHoverTabRectangle(tabControl) && e.Delta != 0) {
                        StaticMethods.TabControlScroll(tabControl, e.Delta);
                    }
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            buttonAddGame.Enabled = tabControl.TabPages.Count < Constants.GameTabsMaxCounts;
            buttonRemoveGame.Enabled = tabControl.TabPages.Count > 1;
        }

        private void OnTextBoxMouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
            if (textBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks == 0 || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks == 3) {
                textBoxClicks = 0;
                NativeMethods.MouseEvent(Constants.MOUSEEVENTF_LEFTUP, Convert.ToUInt32(Cursor.Position.X), Convert.ToUInt32(Cursor.Position.Y), 0, 0);
                Application.DoEvents();
                if (textBox.Multiline) {
                    char[] chars = textBox.Text.ToCharArray();
                    int selectionEnd = Math.Min(Array.IndexOf(chars, Constants.CarriageReturn, textBox.SelectionStart), Array.IndexOf(chars, Constants.LineFeed, textBox.SelectionStart));
                    if (selectionEnd < 0) {
                        selectionEnd = textBox.TextLength;
                    }
                    selectionEnd = Math.Max(textBox.SelectionStart + textBox.SelectionLength, selectionEnd);
                    int selectionStart = Math.Min(textBox.SelectionStart, selectionEnd);
                    while (--selectionStart > 0 && chars[selectionStart] != Constants.LineFeed && chars[selectionStart] != Constants.CarriageReturn) { }
                    textBox.Select(selectionStart, selectionEnd - selectionStart);
                } else {
                    textBox.SelectAll();
                }
                textBox.Focus();
            } else {
                textBoxClicksTimer.Start();
            }
        }

        private static float ParseOdd(string str) => float.Parse(Regex.Replace(str.Replace(Constants.Comma, Constants.Period), Constants.OddPattern, string.Empty), CultureInfo.InvariantCulture);

        private static float ParseTrustDegree(string str) => float.Parse(Regex.Replace(str, Constants.TrustDegreePattern, Constants.ReplaceFirst));

        private void RemoveGame(object sender, EventArgs e) {
            if (tabControl.TabPages.Count > 1) {
                int index = tabControl.SelectedIndex - 1;
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                tabControl.SelectedIndex = index < 0 ? 0 : index;
                MatchControl matchControl = (MatchControl)tabControl.SelectedTab.Controls[0];
                Text = string.IsNullOrWhiteSpace(matchControl.Game.Match) ? text : text + Constants.Space + Constants.EnDash + Constants.Space + matchControl.Game.Match.Trim();
            }
        }

        private void OnTextBoxOddLeave(object sender, EventArgs e) {
            float.TryParse(Regex.Replace(textBoxOdd.Text.Replace(Constants.Comma, Constants.Period), Constants.OddPattern, string.Empty), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out odd);
            textBoxOdd.Text = ShowOdd(odd);
        }

        private void OnTextBoxTrustDegreeLeave(object sender, EventArgs e) {
            float.TryParse(Regex.Replace(textBoxTrustDegree.Text, Constants.TrustDegreePattern, Constants.ReplaceFirst), NumberStyles.None, CultureInfo.InvariantCulture, out trustDegree);
            textBoxTrustDegree.Text = ShowTrustDegree(trustDegree);
        }

        private void Save(object sender, EventArgs e) {
            List<Game> list = new List<Game>(tabControl.TabPages.Count);
            foreach (TabPage tabPage in tabControl.TabPages) {
                MatchControl matchControl = (MatchControl)tabPage.Controls[0];
                if (string.IsNullOrWhiteSpace(matchControl.Game.Match)) {
                    dialog = new MessageForm(this, Properties.Resources.MessageMatchEmptyError, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                    dialog.ShowDialog(this);
                    matchControl.FocusMatchField();
                    return;
                }
                list.Add(matchControl.Game);
            }
            try {
                odd = ParseOdd(textBoxOdd.Text);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.Space + Constants.EnDash + Constants.Space + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog(this);
                textBoxOdd.Focus();
                textBoxOdd.SelectAll();
            }
            try {
                trustDegree = ParseTrustDegree(textBoxTrustDegree.Text);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.Space + Constants.EnDash + Constants.Space + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog(this);
                textBoxTrustDegree.Focus();
                textBoxTrustDegree.SelectAll();
            }
            if (tip == null) {
                tip = new Tip(DateTime.Now, list.ToArray(), textBoxBookmaker.Text, odd, trustDegree, textBoxService.Text, (Tip.TipStatus)comboBoxStatus.SelectedValue);
            } else {
                tip.Games = list.ToArray();
                tip.Bookmaker = textBoxBookmaker.Text;
                tip.Odd = odd;
                tip.TrustDegree = trustDegree;
                tip.Service = textBoxService.Text;
                tip.Status = (Tip.TipStatus)comboBoxStatus.SelectedValue;
            }
            DialogResult = DialogResult.OK;
        }

        private string ShowOdd(float price) => price.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo);

        private string ShowTrustDegree(float trustDegree) => trustDegree.ToString(Constants.ZeroDecimalDigitsFormat, settings.NumberFormat.cultureInfo) + Constants.Slash + 10.ToString();

    }
}
