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
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace BetHelper {

    [Serializable()]
    public struct Data {
        public decimal balance;
        public decimal[] balances;
        public decimal[] trustDegrees;
        public Dictionary<DateTime, decimal> movements;
        public int sortColumnServices;
        public int sortColumnTips;
        public Service[] services;
        public SortOrder sortOrderServices;
        public SortOrder sortOrderTips;
        public string notes;
        public Tip[] tips;

        public Data(
                decimal balance,
                decimal[] balances,
                decimal[] trustDegrees,
                Dictionary<DateTime, decimal> movements,
                string notes,
                Tip[] tips,
                int sortColumnTips,
                SortOrder sortOrderTips,
                Service[] services,
                int sortColumnServices,
                SortOrder sortOrderServices) {

            this.balance = balance;
            this.balances = balances;
            this.movements = movements;
            this.notes = notes;
            this.services = services;
            this.sortColumnServices = sortColumnServices;
            this.sortColumnTips = sortColumnTips;
            this.sortOrderServices = sortOrderServices;
            this.sortOrderTips = sortOrderTips;
            this.tips = tips;
            this.trustDegrees = trustDegrees;
        }

        public Data(SerializationInfo info, StreamingContext ctxt) {
            balance = (decimal)info.GetValue("Balance", typeof(decimal));
            balances = (decimal[])info.GetValue("Balances", typeof(decimal[]));
            movements = (Dictionary<DateTime, decimal>)info.GetValue("Movements", typeof(Dictionary<DateTime, decimal>));
            notes = (string)info.GetValue("Notes", typeof(string));
            services = (Service[])info.GetValue("Services", typeof(Service[]));
            tips = (Tip[])info.GetValue("Tips", typeof(Tip[]));
            trustDegrees = (decimal[])info.GetValue("TrustDegrees", typeof(decimal[]));
            sortColumnServices = (int)info.GetValue("SortColumnServices", typeof(int));
            sortOrderServices = (SortOrder)info.GetValue("SortOrderServices", typeof(SortOrder));
            sortColumnTips = (int)info.GetValue("SortColumnTips", typeof(int));
            sortOrderTips = (SortOrder)info.GetValue("SortOrderTips", typeof(SortOrder));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Balance", balance);
            info.AddValue("Balances", balances);
            info.AddValue("Movements", movements);
            info.AddValue("Notes", notes);
            info.AddValue("Services", services);
            info.AddValue("Tips", tips);
            info.AddValue("TrustDegrees", trustDegrees);
            info.AddValue("SortColumnServices", sortColumnServices);
            info.AddValue("SortOrderServices", sortOrderServices);
            info.AddValue("SortColumnTips", sortColumnTips);
            info.AddValue("SortOrderTips", sortOrderTips);
        }
    }
}
