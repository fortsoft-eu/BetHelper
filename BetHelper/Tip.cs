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
 * Version 1.1.16.2
 */

using FortSoft.Tools;
using System;
using System.Collections.Generic;
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

        private delegate IntPtr HandleCallback();

        public event EventHandler StatusChanged;
        public event EventHandler Update;

        public event EventHandler AltCtrlShiftEPressed;
        public event EventHandler AltCtrlShiftPPressed;
        public event EventHandler AltF10Pressed;
        public event EventHandler AltF11Pressed;
        public event EventHandler AltF12Pressed;
        public event EventHandler AltF5Pressed;
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
        public event EventHandler F5Pressed;
        public event EventHandler F7Pressed;
        public event EventHandler F8Pressed;
        public event EventHandler F9Pressed;
        public event EventHandler HomePressed;
        public event EventHandler PageDownPressed;
        public event EventHandler PageUpPressed;
        public event EventHandler ShiftF3Pressed;
        public event EventHandler UpPressed;

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
            tipForm.AltCtrlShiftEPressed += new EventHandler((sender, e) => AltCtrlShiftEPressed?.Invoke(sender, e));
            tipForm.AltCtrlShiftPPressed += new EventHandler((sender, e) => AltCtrlShiftPPressed?.Invoke(sender, e));
            tipForm.AltF10Pressed += new EventHandler((sender, e) => AltF10Pressed?.Invoke(sender, e));
            tipForm.AltF11Pressed += new EventHandler((sender, e) => AltF11Pressed?.Invoke(sender, e));
            tipForm.AltF12Pressed += new EventHandler((sender, e) => AltF12Pressed?.Invoke(sender, e));
            tipForm.AltF5Pressed += new EventHandler((sender, e) => AltF5Pressed?.Invoke(sender, e));
            tipForm.AltF7Pressed += new EventHandler((sender, e) => AltF7Pressed?.Invoke(sender, e));
            tipForm.AltF8Pressed += new EventHandler((sender, e) => AltF8Pressed?.Invoke(sender, e));
            tipForm.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
            tipForm.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
            tipForm.AltHomePressed += new EventHandler((sender, e) => AltHomePressed?.Invoke(sender, e));
            tipForm.AltLeftPressed += new EventHandler((sender, e) => AltLeftPressed?.Invoke(sender, e));
            tipForm.AltLPressed += new EventHandler((sender, e) => AltLPressed?.Invoke(sender, e));
            tipForm.AltRightPressed += new EventHandler((sender, e) => AltRightPressed?.Invoke(sender, e));
            tipForm.CtrlDPressed += new EventHandler((sender, e) => CtrlDPressed?.Invoke(sender, e));
            tipForm.CtrlEPressed += new EventHandler((sender, e) => CtrlEPressed?.Invoke(sender, e));
            tipForm.CtrlFPressed += new EventHandler((sender, e) => CtrlFPressed?.Invoke(sender, e));
            tipForm.CtrlF5Pressed += new EventHandler((sender, e) => CtrlF5Pressed?.Invoke(sender, e));
            tipForm.CtrlGPressed += new EventHandler((sender, e) => CtrlGPressed?.Invoke(sender, e));
            tipForm.CtrlIPressed += new EventHandler((sender, e) => CtrlIPressed?.Invoke(sender, e));
            tipForm.CtrlMinusPressed += new EventHandler((sender, e) => CtrlMinusPressed?.Invoke(sender, e));
            tipForm.CtrlMPressed += new EventHandler((sender, e) => CtrlMPressed?.Invoke(sender, e));
            tipForm.CtrlOPressed += new EventHandler((sender, e) => CtrlOPressed?.Invoke(sender, e));
            tipForm.CtrlPlusPressed += new EventHandler((sender, e) => CtrlPlusPressed?.Invoke(sender, e));
            tipForm.CtrlPPressed += new EventHandler((sender, e) => CtrlPPressed?.Invoke(sender, e));
            tipForm.CtrlShiftDelPressed += new EventHandler((sender, e) => CtrlShiftDelPressed?.Invoke(sender, e));
            tipForm.CtrlShiftEPressed += new EventHandler((sender, e) => CtrlShiftEPressed?.Invoke(sender, e));
            tipForm.CtrlShiftMinusPressed += new EventHandler((sender, e) => CtrlShiftMinusPressed?.Invoke(sender, e));
            tipForm.CtrlShiftNPressed += new EventHandler((sender, e) => CtrlShiftNPressed?.Invoke(sender, e));
            tipForm.CtrlShiftPlusPressed += new EventHandler((sender, e) => CtrlShiftPlusPressed?.Invoke(sender, e));
            tipForm.CtrlShiftPPressed += new EventHandler((sender, e) => CtrlShiftPPressed?.Invoke(sender, e));
            tipForm.CtrlTPressed += new EventHandler((sender, e) => CtrlTPressed?.Invoke(sender, e));
            tipForm.CtrlUPressed += new EventHandler((sender, e) => CtrlUPressed?.Invoke(sender, e));
            tipForm.CtrlZeroPressed += new EventHandler((sender, e) => CtrlZeroPressed?.Invoke(sender, e));
            tipForm.DownPressed += new EventHandler((sender, e) => DownPressed?.Invoke(sender, e));
            tipForm.EndPressed += new EventHandler((sender, e) => EndPressed?.Invoke(sender, e));
            tipForm.F11Pressed += new EventHandler((sender, e) => F11Pressed?.Invoke(sender, e));
            tipForm.F12Pressed += new EventHandler((sender, e) => F12Pressed?.Invoke(sender, e));
            tipForm.F2Pressed += new EventHandler((sender, e) => F2Pressed?.Invoke(sender, e));
            tipForm.F3Pressed += new EventHandler((sender, e) => F3Pressed?.Invoke(sender, e));
            tipForm.F5Pressed += new EventHandler((sender, e) => F5Pressed?.Invoke(sender, e));
            tipForm.F7Pressed += new EventHandler((sender, e) => F7Pressed?.Invoke(sender, e));
            tipForm.F8Pressed += new EventHandler((sender, e) => F8Pressed?.Invoke(sender, e));
            tipForm.F9Pressed += new EventHandler((sender, e) => F9Pressed?.Invoke(sender, e));
            tipForm.HomePressed += new EventHandler((sender, e) => HomePressed?.Invoke(sender, e));
            tipForm.PageDownPressed += new EventHandler((sender, e) => PageDownPressed?.Invoke(sender, e));
            tipForm.PageUpPressed += new EventHandler((sender, e) => PageUpPressed?.Invoke(sender, e));
            tipForm.ShiftF3Pressed += new EventHandler((sender, e) => ShiftF3Pressed?.Invoke(sender, e));
            tipForm.UpPressed += new EventHandler((sender, e) => UpPressed?.Invoke(sender, e));
            tipForm.Tip = this;
            if (tipForm.ShowDialog().Equals(DialogResult.OK)) {
                Update?.Invoke(this, EventArgs.Empty);
            }
        }

        public IntPtr GetFormHandle() => IsOpened ? (IntPtr)tipForm.Invoke(new HandleCallback(() => tipForm.Handle)) : IntPtr.Zero;

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
            List<string> gameUids = new List<string>(Games.Length);
            foreach (Game game in Games) {
                gameUids.Add(game.Uid);
            }
            gameUids.Sort();
            StringBuilder stringBuilder = new StringBuilder(string.Join(string.Empty, gameUids));
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
