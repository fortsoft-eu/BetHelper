﻿/**
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
 * Version 1.1.14.0
 */

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BetHelper {
    public class MainFindForm : FindForm {
        public MainFindForm(Search search) : base(search) {
            Name = GetType().Name;
            Text = new StringBuilder()
                .Append(Program.GetTitle())
                .Append(Constants.Space)
                .Append(Constants.EnDash)
                .Append(Constants.Space)
                .Append(Properties.Resources.CaptionFind)
                .ToString();

            searchDelayTimer = new Timer();
            searchDelayTimer.Interval = 150;
            searchDelayTimer.Tick += new EventHandler((sender, e) => {
                searchDelayTimer.Stop();
                cannotSearch = false;
            });
            HideExtCheckBoxes();
            ShowTrimCheckBox();

            searchHandler = new SearchHandler(Constants.BrowserSearchFileName);
            searchHandler.Saved += new EventHandler(UpdateFindComboBoxList);
            searchHandler.Loaded += new EventHandler(UpdateFindComboBoxList);
            searchHandler.Load();

            SetMaxDropDownItems(searchHandler.MaximumSearches);

            MinimumSize = new Size(280, 144);
        }
    }
}
