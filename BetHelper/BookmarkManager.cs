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
 * Version 1.1.0.0
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BetHelper {
    public class BookmarkManager {
        private Dictionary<string, string> bookmarks;
        private MenuItem menuItem;
        private Regex regex;
        private Settings settings;
        private string bookmarksFilePath;

        public event EventHandler<UrlEventArgs> Activated;
        public event EventHandler Added, Deleted, Loaded, Removed, Saved;

        public BookmarkManager(Settings settings) {
            this.settings = settings;
            regex = new Regex(Constants.TrimBookmarkPattern);
            bookmarks = new Dictionary<string, string>();
            bookmarksFilePath = Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), Constants.BookmarksFileName);
            Load();
        }

        public MenuItem MenuItem {
            get {
                return menuItem;
            }
            set {
                menuItem = value;
                Populate();
            }
        }

        public void Add(string url, string title) {
            if (!url.Equals(Constants.BlankPageUri) && !Contains(url)) {
                Uri uri = new Uri(url);
                if (string.IsNullOrWhiteSpace(title)) {
                    title = uri.GetComponents(UriComponents.Host, UriFormat.Unescaped);
                }
                bookmarks.Add(uri.GetLeftPart(UriPartial.Query), title);
                Populate();
                Save(Action.Add);
            }
        }

        private void AddMenuItem(string url, string title) {
            MenuItem item = new MenuItem(settings.TruncateBookmarkTitles
                ? TruncateBookmarkTitle(title)
                : title, new EventHandler(OnMenuItemClicked));
            item.Tag = url;
            menuItem.MenuItems.Add(item);
        }

        public bool Contains(string url) {
            if (string.IsNullOrEmpty(url)) {
                return false;
            }
            foreach (string key in bookmarks.Keys) {
                if (EqualsUrl(key, url)) {
                    return true;
                }
            }
            return false;
        }

        public void Delete() {
            try {
                if (File.Exists(bookmarksFilePath)) {
                    File.Delete(bookmarksFilePath);
                }
                if (!File.Exists(bookmarksFilePath)) {
                    Deleted?.Invoke(this, null);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            while (menuItem.MenuItems.Count > 2) {
                menuItem.MenuItems.RemoveAt(2);
            }
        }

        private void Load() {
            try {
                if (File.Exists(bookmarksFilePath)) {
                    using (FileStream fileStream = File.OpenRead(bookmarksFilePath)) {
                        bookmarks = (Dictionary<string, string>)new BinaryFormatter().Deserialize(fileStream);
                    }
                    Loaded?.Invoke(this, EventArgs.Empty);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnMenuItemClicked(object sender, EventArgs e) {
            Activated?.Invoke(sender, new UrlEventArgs((string)(((MenuItem)sender).Tag)));
        }

        public void Populate() {
            while (menuItem.MenuItems.Count > 2) {
                menuItem.MenuItems.RemoveAt(2);
            }
            if (bookmarks.Count > 0) {
                menuItem.MenuItems.Add(Constants.Hyphen.ToString());
            }
            List<KeyValuePair<string, string>> list = bookmarks.ToList();
            if (settings.SortBookmarks) {
                list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            }
            foreach (KeyValuePair<string, string> keyValuePair in list) {
                AddMenuItem(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void Remove(string url) {
            if (bookmarks.ContainsKey(url)) {
                bookmarks.Remove(url);
                Populate();
                Save(Action.Remove);
            }
        }

        private void Save(Action action) {
            try {
                using (FileStream fileStream = File.Create(bookmarksFilePath)) {
                    new BinaryFormatter().Serialize(fileStream, bookmarks);
                }
                Saved?.Invoke(this, EventArgs.Empty);
                switch (action) {
                    case Action.Add:
                        Added?.Invoke(this, EventArgs.Empty);
                        break;
                    case Action.Remove:
                        Removed?.Invoke(this, EventArgs.Empty);
                        break;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private string TruncateBookmarkTitle(string title) => regex.Replace(title, Constants.ReplaceFirst);

        /// <summary>
        /// Method to compare new URL with already bookmarked URL.
        /// </summary>
        /// <param name="url1">URL to check.</param>
        /// <param name="url2">URL already contained.</param>
        /// <returns>True if URL to bookmark equals to already bookmarked URL.
        /// Otherwise false.</returns>
        private static bool EqualsUrl(string url1, string url2) {
            if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2)) {
                return false;
            }
            try {
                Uri uri1 = new Uri(url1);
                Uri uri2 = new Uri(url2);
                bool ep = uri2.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped)
                    .Equals(uri1.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped), StringComparison.Ordinal);
                if (uri1.HostNameType.Equals(UriHostNameType.Dns) && uri2.HostNameType.Equals(UriHostNameType.Dns)) {
                    Regex regex = new Regex(Constants.SecondLevelDomainPattern, RegexOptions.RightToLeft);
                    return ep && regex.Replace(uri2.Host, Constants.ReplaceSecond)
                        .Equals(regex.Replace(uri1.Host, Constants.ReplaceSecond), StringComparison.Ordinal);
                } else if (uri1.HostNameType == uri2.HostNameType) {
                    return ep && uri2.GetComponents(UriComponents.Host, UriFormat.UriEscaped)
                        .Equals(uri1.GetComponents(UriComponents.Host, UriFormat.UriEscaped), StringComparison.Ordinal);
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return false;
        }

        private enum Action {
            Add,
            Remove
        }
    }
}
