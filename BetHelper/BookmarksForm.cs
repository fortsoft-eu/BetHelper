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
 * Version 1.1.8.0
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetHelper {
    public partial class BookmarksForm : Form {
        private BookmarkManager bookmarkManager;
        private bool alreadySorted;
        private Form dialog;
        private ListViewItem clickedListViewItem;
        private ListViewSorter listViewSorter;
        private PersistWindowState persistWindowState;
        private Settings settings;

        public event EventHandler F7Pressed;

        public BookmarksForm(Settings settings, BookmarkManager bookmarkManager) {
            this.settings = settings;
            this.bookmarkManager = bookmarkManager;

            persistWindowState = new PersistWindowState();
            persistWindowState.DisableSaveWindowState = true;
            persistWindowState.Parent = this;

            InitializeComponent();
            BuildContextMenuAsync();
            BuildListViewHeaderAsync();

            List<ListViewItem> list = new List<ListViewItem>(bookmarkManager.Bookmarks.Count);
            foreach (KeyValuePair<string, string> keyValuePair in bookmarkManager.Bookmarks) {
                list.Add(SetBookmark(keyValuePair));
            }
            listView.Items.AddRange(list.ToArray());

        }

        private async void BuildContextMenuAsync() {
            await Task.Run(new Action(() => {
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
                    contextMenu.MenuItems[0].Enabled = listView.SelectedItems.Count > 0;
                    contextMenu.MenuItems[1].Enabled = listView.SelectedItems.Count > 0;
                    contextMenu.MenuItems[3].Enabled = clickedListViewItem != null;
                    contextMenu.MenuItems[4].Enabled = listView.Items.Count > 0;
                    contextMenu.MenuItems[5].Enabled = listView.Items.Count > listView.SelectedItems.Count;
                    if (clickedListViewItem != null) {
                        contextMenu.MenuItems[3].Text = clickedListViewItem.Selected
                            ? Properties.Resources.MenuItemUnselect
                            : Properties.Resources.MenuItemSelect;
                    } else {
                        contextMenu.MenuItems[3].Text = Properties.Resources.MenuItemSelect;
                    }
                });
                listView.ContextMenu = contextMenu;
            }));
        }

        private async void BuildListViewHeaderAsync() {
            await Task.Run(new Action(() => {
                listView.Columns.AddRange(new ColumnHeader[] {
                    new ColumnHeader() {
                        Text = Properties.Resources.CaptionTitle,
                        TextAlign = HorizontalAlignment.Left
                    },
                    new ColumnHeader() {
                        Text = Properties.Resources.CaptionUrl,
                        TextAlign = HorizontalAlignment.Left
                    }
                });
                listView.FullRowSelect = true;
                listView.GridLines = true;
                listView.HeaderStyle = ColumnHeaderStyle.Clickable;
                listView.HideSelection = false;
                listView.SmallImageList = GetImageList();
                listViewSorter = new ListViewSorter();
                listView.ListViewItemSorter = listViewSorter;
                listView.MultiSelect = true;
                listView.View = View.Details;
                listView.ColumnClick += new ColumnClickEventHandler((sender, e) => {
                    if (listViewSorter.SortColumn.Equals(e.Column)) {
                        if (!alreadySorted && e.Column.Equals(0)) {
                            listViewSorter.SortOrder = SortOrder.Descending;
                        } else {
                            listViewSorter.SortOrder = listViewSorter.SortOrder.Equals(SortOrder.Ascending)
                                ? SortOrder.Descending
                                : SortOrder.Ascending;
                        }
                        alreadySorted = true;
                    } else {
                        listViewSorter.SortColumn = e.Column;
                        listViewSorter.SortOrder = SortOrder.Ascending;
                    }
                    listView.Sort();
                });
            }));
        }

        private void CopyAsText(object sender, EventArgs e) {
            List<string> items = new List<string>(listView.SelectedItems.Count);
            foreach (ListViewItem listViewItem in listView.SelectedItems) {
                List<string> line = new List<string>(listView.Columns.Count);
                foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems) {
                    line.Add(subItem.Text);
                }
                items.Add(string.Join(Constants.VerticalTab.ToString(), line));
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
            List<string> items = new List<string>(listView.SelectedItems.Count);
            if (listView.SelectedItems.Count > 0) {
                string message = listView.SelectedItems.Count > 1
                    ? string.Format(Properties.Resources.MessageDeleteBookmarks, listView.SelectedItems.Count)
                    : Properties.Resources.MessageDeleteBookmark;
                MessageForm messageForm = new MessageForm(this, message, Properties.Resources.CaptionQuestion, MessageForm.Buttons.YesNo,
                    MessageForm.BoxIcon.Question);
                messageForm.F7Pressed += new EventHandler((s, t) => F7Pressed?.Invoke(s, t));
                dialog = messageForm;
                if (messageForm.ShowDialog(this).Equals(DialogResult.Yes)) {
                    foreach (ListViewItem listViewItem in listView.SelectedItems) {
                        listView.Items.Remove(listViewItem);
                        items.Add(listViewItem.SubItems[1].Text);
                    }
                    bookmarkManager.Remove(items.ToArray());
                }
            }
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void ToggleSelect(object sender, EventArgs e) {
            if (clickedListViewItem != null) {
                clickedListViewItem.Selected = !clickedListViewItem.Selected;
            }
        }

        private void InvertSelection(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listView.Items) {
                listViewItem.Selected = !listViewItem.Selected;
            }
        }

        private void SelectAll(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in listView.Items) {
                listViewItem.Selected = true;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode.Equals(Keys.A)) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is ListView || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    SelectAll(sender, e);
                }
            } else if (e.Control && e.KeyCode.Equals(Keys.C)) {
                if (sender is ListView || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    CopyAsText(sender, e);
                }
            } else if (e.KeyCode.Equals(Keys.Delete)) {
                if (sender is ListView || sender is Button && ((Button)sender).Name.Equals(Constants.ButtonRemoveName)) {
                    DeleteSelected(sender, e);
                }
            } else if (e.KeyCode.Equals(Keys.F7)) {
                F7Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnListViewMouseClick(object sender, MouseEventArgs e) {
            if (e.Button.Equals(MouseButtons.Right)) {
                clickedListViewItem = ((ListView)sender).GetItemAt(e.X, e.Y);
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) => buttonRemove.Enabled = listView.SelectedItems.Count > 0;

        private ListViewItem SetBookmark(KeyValuePair<string, string> keyValuePair) {
            ListViewItem listViewItem = new ListViewItem(
                new ListViewItem.ListViewSubItem[] {
                    new ListViewItem.ListViewSubItem() {
                        Text = keyValuePair.Value,
                        Tag = keyValuePair.Value
                    },
                    new ListViewItem.ListViewSubItem() {
                        Text = keyValuePair.Key,
                        Tag = keyValuePair.Key
                    }
                },
                0);
            return listViewItem;
        }

        private void SetColumnsSize(object sender, EventArgs e) {
            if (listView.Columns.Count > 0) {
                int columnWidth = (listView.Width - SystemInformation.VerticalScrollBarWidth - SystemInformation.Border3DSize.Width * 2) / 2;
                listView.Columns[0].Width = columnWidth;
                listView.Columns[1].Width = columnWidth;
            }
        }

        private static ImageList GetImageList() {
            ImageList imageList = new ImageList();
            imageList.Images.Add(Properties.Resources.Bookmark.ToBitmap());
            return imageList;
        }
    }
}
