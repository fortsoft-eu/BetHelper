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
 * Version 1.1.4.0
 */

using FortSoft.Tools;
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BetHelper {

    [Serializable]
    public sealed class Game : ISerializable {

        public Game(DateTime dateTime, string sport, string league, string match, string opportunity) {
            DateTime = dateTime;
            League = league;
            Match = match;
            Opportunity = opportunity;
            Sport = sport;
            SetUid();
        }

        private Game(SerializationInfo info, StreamingContext ctxt) {
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
            StringBuilder stringBuilder = new StringBuilder()
                .Append(DateTime.ToString(Constants.TimeFormatForUid, CultureInfo.InvariantCulture))
                .Append(Sport)
                .Append(League)
                .Append(Match)
                .Append(Opportunity);
            Uid = Hash.MD5(stringBuilder.ToString(), Encoding.UTF8);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder()
                .Append("DateTime")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(DateTime.ToString(Constants.ToStringTimeFormat, CultureInfo.InvariantCulture))
                .Append("Sport")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Sport)
                .Append("League")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(League)
                .Append("Match")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Match)
                .Append("Opportunity")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Opportunity)
                .Append("Uid")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .Append(Uid);
            return stringBuilder.ToString();
        }
    }
}
