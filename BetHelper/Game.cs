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
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BetHelper {

    [Serializable()]
    public class Game : ISerializable {

        public Game(DateTime dateTime, string sport, string league, string match, string opportunity) {
            DateTime = dateTime;
            League = league;
            Match = match;
            Opportunity = opportunity;
            Sport = sport;
            SetUid();
        }

        public Game(SerializationInfo info, StreamingContext ctxt) {
            DateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
            League = (string)info.GetValue("League", typeof(string));
            Match = (string)info.GetValue("Match", typeof(string));
            Opportunity = (string)info.GetValue("Opportunity", typeof(string));
            Sport = (string)info.GetValue("Sport", typeof(string));
            Uid = (string)info.GetValue("Uid", typeof(string));
        }

        public DateTime DateTime { get; set; }

        public string League { get; set; }

        public string Match { get; set; }

        public string Opportunity { get; set; }

        public string Sport { get; set; }

        public string Uid { get; private set; }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("DateTime", DateTime);
            info.AddValue("League", League);
            info.AddValue("Match", Match);
            info.AddValue("Opportunity", Opportunity);
            info.AddValue("Sport", Sport);
            info.AddValue("Uid", Uid);
        }

        private void SetUid() {
            StringBuilder stringBuilder = new StringBuilder(DateTime.ToString(Constants.TimeFormatForUid, CultureInfo.InvariantCulture));
            stringBuilder.Append(Sport);
            stringBuilder.Append(League);
            stringBuilder.Append(Match);
            stringBuilder.Append(Opportunity);
            Uid = Hash.MD5(stringBuilder.ToString(), Encoding.UTF8);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DateTime");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.AppendLine(DateTime.ToString(Constants.ToStringTimeFormat, CultureInfo.InvariantCulture));
            stringBuilder.Append("Sport");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.AppendLine(Sport);
            stringBuilder.Append("League");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.AppendLine(League);
            stringBuilder.Append("Match");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.AppendLine(Match);
            stringBuilder.Append("Opportunity");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.AppendLine(Opportunity);
            stringBuilder.Append("Uid");
            stringBuilder.Append(Constants.Colon);
            stringBuilder.Append(Constants.Space);
            stringBuilder.Append(Uid);
            return stringBuilder.ToString();
        }
    }
}
