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
 * Version 1.1.9.0
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
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

        private delegate void TipFormFormCallback();

        public event EventHandler AltCtrlShiftEPressed;
        public event EventHandler AltCtrlShiftPPressed;
        public event EventHandler AltF10Pressed;
        public event EventHandler AltF11Pressed;
        public event EventHandler AltF12Pressed;
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
        public event EventHandler F4Pressed;
        public event EventHandler F5Pressed;
        public event EventHandler F7Pressed;
        public event EventHandler F8Pressed;
        public event EventHandler F9Pressed;
        public event EventHandler HomePressed;
        public event EventHandler PageDownPressed;
        public event EventHandler PageUpPressed;
        public event EventHandler ShiftF3Pressed;
        public event EventHandler UpPressed;

        public TipForm(Settings settings) : this(settings, null) { }

        public TipForm(Settings settings, PersistWindowState persistWindowState) {
            this.settings = settings;
            this.persistWindowState = persistWindowState;
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

            Icon = Properties.Resources.Tip;
            Text = Properties.Resources.CaptionNewTip;
            text = Properties.Resources.CaptionNewTip;

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
                    TabPage tabPage = new TabPage() {
                        Text = StaticMethods.AbbreviateMatchName(game.Match, tabControl.Font, 180),
                        UseVisualStyleBackColor = true
                    };
                    tabPage.Controls.Add(matchControl);
                    if (this.matchControl == null) {
                        this.matchControl = matchControl;
                        if (string.IsNullOrWhiteSpace(game.Match)) {
                            Text = text;
                        } else {
                            Text = new StringBuilder(text)
                                .Append(Constants.Space)
                                .Append(Constants.EnDash)
                                .Append(Constants.Space)
                                .Append(game.Match.Trim())
                                .ToString();
                        }
                    }
                    tabControl.TabPages.Add(tabPage);
                }
                buttonAddGame.Enabled = tabControl.TabPages.Count < Constants.GameTabsMaxCounts;
                buttonRemoveGame.Enabled = tabControl.TabPages.Count > 1;
                tip.PersistWindowState = persistWindowState;
                tip.Settings = settings;
                tip.StatusChanged += new EventHandler(OnStatusChanged);
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
                textBoxBookmaker.ContextMenu = contextMenu;
                textBoxOdd.ContextMenu = contextMenu;
                textBoxService.ContextMenu = contextMenu;
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
            TabPage tabPage = new TabPage() {
                Text = new StringBuilder()
                    .Append(Properties.Resources.CaptionMatch)
                    .Append(Constants.Space)
                    .Append(matchOrdinal++)
                    .ToString(),
                UseVisualStyleBackColor = true
            };
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

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            focusTimer.Dispose();
            textBoxClicksTimer.Dispose();
        }

        private void OnFormLoad(object sender, EventArgs e) {
            if (tabControl.TabPages.Count.Equals(0) && tabControl.TabPages.Count < Constants.GameTabsMaxCounts) {
                matchControl = AddGame();
            }
            focusTimer.Start();
        }

        private void OnGameNameChanged(object sender, MatchControlEventArgs e) {
            if (string.IsNullOrWhiteSpace(e.GameName)) {
                e.TabPage.Text = new StringBuilder()
                    .Append(Properties.Resources.CaptionMatch)
                    .Append(Constants.Space)
                    .Append(e.Ordinal)
                    .ToString();
            } else {
                e.TabPage.Text = e.GameName.Trim();
            }
            if (e.Ordinal.Equals(1)) {
                if (string.IsNullOrWhiteSpace(e.GameName)) {
                    Text = text;
                } else {
                    Text = new StringBuilder(text)
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(e.GameName.Trim())
                        .ToString();
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                }
            } else if (e.Alt && e.Control && e.Shift && e.KeyCode.Equals(Keys.E)) {
                AltCtrlShiftEPressed?.Invoke(this, EventArgs.Empty);
            } else if (e.Alt && e.Control && e.Shift && e.KeyCode.Equals(Keys.P)) {
                AltCtrlShiftPPressed?.Invoke(this, EventArgs.Empty);
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
            } else if (e.KeyCode.Equals(Keys.F4)) {
                F4Pressed?.Invoke(this, EventArgs.Empty);
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

        private void OnMouseWheel(object sender, MouseEventArgs e) {
            if (((Form)sender).Equals(ActiveForm)) {
                Control control = StaticMethods.FindControlAtCursor((Form)sender);
                if (control is TabControl) {
                    TabControl tabControl = (TabControl)control;
                    if (StaticMethods.IsHoverTabRectangle(tabControl) && !e.Delta.Equals(0)) {
                        StaticMethods.TabControlScroll(tabControl, e.Delta);
                    }
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            buttonAddGame.Enabled = tabControl.TabPages.Count < Constants.GameTabsMaxCounts;
            buttonRemoveGame.Enabled = tabControl.TabPages.Count > 1;
        }

        private void OnStatusChanged(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new EventHandler(OnStatusChanged), sender, e);
            } else {
                comboBoxStatus.SelectedIndex = (int)((Tip)sender).Status;
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

        private void OnTextBoxOddLeave(object sender, EventArgs e) {
            float.TryParse(
                Regex.Replace(textBoxOdd.Text.Replace(Constants.Comma, Constants.Period), Constants.OddPattern, string.Empty),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out odd);
            textBoxOdd.Text = ShowOdd(odd);
        }

        private void OnTextBoxTrustDegreeLeave(object sender, EventArgs e) {
            float.TryParse(
                Regex.Replace(textBoxTrustDegree.Text, Constants.TrustDegreePattern, Constants.ReplaceFirst),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out trustDegree);
            textBoxTrustDegree.Text = ShowTrustDegree(trustDegree);
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

        private static float ParseOdd(string str) {
            return float.Parse(Regex.Replace(str.Replace(Constants.Comma, Constants.Period), Constants.OddPattern, string.Empty),
                CultureInfo.InvariantCulture);
        }

        private static float ParseTrustDegree(string str) {
            return float.Parse(Regex.Replace(str, Constants.TrustDegreePattern, Constants.ReplaceFirst));
        }

        private void RemoveGame(object sender, EventArgs e) {
            if (tabControl.TabPages.Count > 1) {
                int index = tabControl.SelectedIndex - 1;
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                tabControl.SelectedIndex = index < 0 ? 0 : index;
                MatchControl matchControl = (MatchControl)tabControl.SelectedTab.Controls[0];
                if (string.IsNullOrWhiteSpace(matchControl.Game.Match)) {
                    Text = text;
                } else {
                    Text = new StringBuilder(text)
                        .Append(Constants.Space)
                        .Append(Constants.EnDash)
                        .Append(Constants.Space)
                        .Append(matchControl.Game.Match.Trim())
                        .ToString();
                }
            }
        }

        public void SafeClose() {
            if (InvokeRequired) {
                Invoke(new TipFormFormCallback(SafeClose));
            } else {
                Close();
            }
        }

        public void SafeHide() {
            if (InvokeRequired) {
                Invoke(new TipFormFormCallback(SafeHide));
            } else if (!WindowState.Equals(FormWindowState.Minimized)) {
                WindowState = FormWindowState.Minimized;
            }
        }

        public void SafeShow() {
            if (InvokeRequired) {
                Invoke(new TipFormFormCallback(SafeShow));
            } else {
                persistWindowState.Restore();
            }
        }

        public void SafeSelect() {
            if (InvokeRequired) {
                Invoke(new TipFormFormCallback(SafeSelect));
            } else {
                persistWindowState.Restore();
                persistWindowState.BringToFront();
            }
        }

        private void Save(object sender, EventArgs e) {
            List<Game> list = new List<Game>(tabControl.TabPages.Count);
            foreach (TabPage tabPage in tabControl.TabPages) {
                MatchControl matchControl = (MatchControl)tabPage.Controls[0];
                if (string.IsNullOrWhiteSpace(matchControl.Game.Match)) {
                    MessageForm messageForm = new MessageForm(this, Properties.Resources.MessageMatchEmptyError, null,
                        MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                    messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                    dialog = messageForm;
                    messageForm.ShowDialog(this);
                    matchControl.FocusMatchField();
                    return;
                }
                list.Add(matchControl.Game);
            }
            try {
                odd = ParseOdd(textBoxOdd.Text);
            } catch (Exception exception) {
                ShowException(exception);
                textBoxOdd.Focus();
                textBoxOdd.SelectAll();
            }
            try {
                trustDegree = ParseTrustDegree(textBoxTrustDegree.Text);
            } catch (Exception exception) {
                ShowException(exception);
                textBoxTrustDegree.Focus();
                textBoxTrustDegree.SelectAll();
            }
            if (tip == null) {
                tip = new Tip(
                    DateTime.Now,
                    list.ToArray(),
                    textBoxBookmaker.Text,
                    odd,
                    trustDegree,
                    textBoxService.Text,
                    (Tip.TipStatus)comboBoxStatus.SelectedValue);
            } else {
                tip.Games = list.ToArray();
                tip.Bookmaker = textBoxBookmaker.Text;
                tip.Odd = odd;
                tip.TrustDegree = trustDegree;
                tip.Service = textBoxService.Text;
                tip.Status = (Tip.TipStatus)comboBoxStatus.SelectedValue;
            }
            tip.PersistWindowState = persistWindowState;
            tip.Settings = settings;
            DialogResult = DialogResult.OK;
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

        private string ShowOdd(float price) {
            return price.ToString(Constants.TwoDecimalDigitsFormat, settings.NumberFormat.cultureInfo);
        }

        private string ShowTrustDegree(float trustDegree) {
            return new StringBuilder()
                .Append(trustDegree.ToString(Constants.ZeroDecimalDigitsFormat, settings.NumberFormat.cultureInfo))
                .Append(Constants.Slash)
                .Append(10)
                .ToString();
        }
    }
}
