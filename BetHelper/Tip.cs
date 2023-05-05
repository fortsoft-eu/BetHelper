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
 * Version 1.1.8.0
 */

using FortSoft.Tools;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BetHelper {

    [Serializable]
    public sealed class Tip : ISerializable {
        private Thread thread;
        private TipForm tipForm;
        private TipStatus tipStatus;

        public event EventHandler F7Pressed, StatusChanged, Update;

        public Tip(
                DateTime dateTime,
                Game[] games,
                string bookmaker,
                float odd,
                float trustDegree,
                string service,
                TipStatus tipStatus) {

            Bookmaker = bookmaker;
            DateTime = dateTime;
            Games = games;
            Odd = odd;
            Service = service;
            this.tipStatus = tipStatus;
            TrustDegree = trustDegree;
            SetUid();
        }

        private Tip(SerializationInfo info, StreamingContext ctxt) {
            Bookmaker = (string)info.GetValue("Bookmaker", typeof(string));
            DateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
            Games = (Game[])info.GetValue("Games", typeof(Game[]));
            Odd = (float)info.GetValue("Odd", typeof(float));
            Service = (string)info.GetValue("Service", typeof(string));
            tipStatus = (TipStatus)info.GetValue("Status", typeof(TipStatus));
            TrustDegree = (float)info.GetValue("TrustDegree", typeof(float));
            Uid = (string)info.GetValue("Uid", typeof(string));
        }

        public bool IsOpened => thread != null && thread.IsAlive;

        public DateTime DateTime { get; set; }

        public float Odd { get; set; }

        public float TrustDegree { get; set; }

        public Game[] Games { get; set; }

        public ListViewItem ListViewItem { get; set; }

        public PersistWindowState PersistWindowState { get; set; }

        public Settings Settings { get; set; }

        public string Bookmaker { get; set; }

        public string Service { get; set; }

        public string Uid { get; private set; }

        public TipStatus Status {
            get {
                return tipStatus;
            }
            set {
                tipStatus = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Close() {
            if (IsOpened) {
                try {
                    tipForm.SafeClose();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void Edit() {
            if (IsOpened) {
                try {
                    tipForm.SafeSelect();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            } else {
                thread = new Thread(new ThreadStart(EditThread));
                thread.Start();
            }
        }

        private void EditThread() {
            tipForm = new TipForm(Settings, PersistWindowState);
            tipForm.F7Pressed += new EventHandler((sender, e) => F7Pressed?.Invoke(sender, e));
            tipForm.Tip = this;
            if (tipForm.ShowDialog().Equals(DialogResult.OK)) {
                Update?.Invoke(this, EventArgs.Empty);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Bookmaker", Bookmaker);
            info.AddValue("DateTime", DateTime);
            info.AddValue("Games", Games);
            info.AddValue("Odd", Odd);
            info.AddValue("Service", Service);
            info.AddValue("Status", tipStatus);
            info.AddValue("TrustDegree", TrustDegree);
            info.AddValue("Uid", Uid);
        }

        private void SetUid() {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Game game in Games) {
                stringBuilder.Append(game.Uid);
            }
            stringBuilder.Append(Bookmaker);
            stringBuilder.Append(Odd.ToString(Constants.TwoDecimalDigitsFormat, CultureInfo.InvariantCulture));
            stringBuilder.Append(TrustDegree.ToString(Constants.TwoDecimalDigitsFormat, CultureInfo.InvariantCulture));
            stringBuilder.Append(Service);
            Uid = Hash.MD5(stringBuilder.ToString(), Encoding.UTF8);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder()
                .Append("DateTime")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(DateTime.ToString(Constants.ToStringTimeFormat, CultureInfo.InvariantCulture))
                .Append("Bookmaker")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Bookmaker)
                .Append("Odd")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Odd.ToString(Constants.TwoDecimalDigitsFormat, CultureInfo.InvariantCulture))
                .Append("TrustDegree")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(TrustDegree.ToString(Constants.TwoDecimalDigitsFormat, CultureInfo.InvariantCulture))
                .Append("Service")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Service)
                .Append("Status")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(tipStatus.ToString())
                .Append("Uid")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Uid);
            foreach (Game game in Games) {
                for (int i = 0; i < 10; i++) {
                    stringBuilder.Append(Constants.Hyphen);
                }
                stringBuilder.AppendLine()
                    .AppendLine(game.ToString());
            }
            return stringBuilder.ToString();
        }

        ///<summary>
        ///Possible states of the tip.
        ///</summary>
        public enum TipStatus {
            ///<summary>
            ///Tip received from the tip service. One of the possible initial
            ///states of the tip.
            ///</summary>
            Received,
            ///<summary>
            ///Own created and published tip. One of the possible initial states
            ///of the tip.
            ///</summary>
            Published,
            ///<summary>
            ///Skipped tip.
            ///</summary>
            Skipped,
            ///<summary>
            ///Successfully placed tip.
            ///</summary>
            Placed,
            ///<summary>
            ///Winning tip.
            ///</summary>
            Win,
            ///<summary>
            ///Void tip.
            ///</summary>
            Void,
            ///<summary>
            ///Losing tip.
            ///</summary>
            Lose
        }
    }
}
