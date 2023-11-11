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
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BetHelper {

    [Serializable]
    public sealed class Service : ISerializable {
        private Thread thread;
        private ServiceForm serviceForm;
        private ServiceStatus serviceStatus;

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

        public Service(
                string name,
                decimal price,
                int span,
                SpanUnit unit,
                DateTime expiration,
                DateTime subscribed,
                ServiceStatus serviceStatus) {

            Expiration = expiration;
            Name = name;
            Price = price;
            Span = span;
            this.serviceStatus = serviceStatus;
            Subscribed = subscribed;
            Unit = unit;
            SetUid();
        }

        private Service(SerializationInfo info, StreamingContext ctxt) {
            Expiration = (DateTime)info.GetValue("Expiration", typeof(DateTime));
            Name = (string)info.GetValue("Name", typeof(string));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            Span = (int)info.GetValue("Span", typeof(int));
            serviceStatus = (ServiceStatus)info.GetValue("Status", typeof(ServiceStatus));
            Subscribed = (DateTime)info.GetValue("Subscribed", typeof(DateTime));
            Uid = (string)info.GetValue("Uid", typeof(string));
            Unit = (SpanUnit)info.GetValue("Unit", typeof(SpanUnit));
        }

        public bool IsOpened => thread != null && thread.IsAlive;

        public DateTime Expiration { get; set; }

        public DateTime Subscribed { get; set; }

        public decimal Price { get; set; }

        public int Span { get; set; }

        public ListViewItem ListViewItem { get; set; }

        public ServiceStatus Status {
            get {
                return serviceStatus;
            }
            set {
                serviceStatus = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public PersistWindowState PersistWindowState { get; set; }

        public Settings Settings { get; set; }

        public SpanUnit Unit { get; set; }

        public string Name { get; set; }

        public string Uid { get; private set; }

        public void Close() {
            if (IsOpened) {
                try {
                    serviceForm.SafeClose();
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void Edit() {
            if (IsOpened) {
                try {
                    serviceForm.SafeSelect();
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
            serviceForm = new ServiceForm(Settings, PersistWindowState);
            serviceForm.AltCtrlShiftEPressed += new EventHandler((sender, e) => AltCtrlShiftEPressed?.Invoke(sender, e));
            serviceForm.AltCtrlShiftPPressed += new EventHandler((sender, e) => AltCtrlShiftPPressed?.Invoke(sender, e));
            serviceForm.AltF10Pressed += new EventHandler((sender, e) => AltF10Pressed?.Invoke(sender, e));
            serviceForm.AltF11Pressed += new EventHandler((sender, e) => AltF11Pressed?.Invoke(sender, e));
            serviceForm.AltF12Pressed += new EventHandler((sender, e) => AltF12Pressed?.Invoke(sender, e));
            serviceForm.AltF5Pressed += new EventHandler((sender, e) => AltF5Pressed?.Invoke(sender, e));
            serviceForm.AltF7Pressed += new EventHandler((sender, e) => AltF7Pressed?.Invoke(sender, e));
            serviceForm.AltF8Pressed += new EventHandler((sender, e) => AltF8Pressed?.Invoke(sender, e));
            serviceForm.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
            serviceForm.AltF9Pressed += new EventHandler((sender, e) => AltF9Pressed?.Invoke(sender, e));
            serviceForm.AltHomePressed += new EventHandler((sender, e) => AltHomePressed?.Invoke(sender, e));
            serviceForm.AltLeftPressed += new EventHandler((sender, e) => AltLeftPressed?.Invoke(sender, e));
            serviceForm.AltLPressed += new EventHandler((sender, e) => AltLPressed?.Invoke(sender, e));
            serviceForm.AltRightPressed += new EventHandler((sender, e) => AltRightPressed?.Invoke(sender, e));
            serviceForm.CtrlDPressed += new EventHandler((sender, e) => CtrlDPressed?.Invoke(sender, e));
            serviceForm.CtrlEPressed += new EventHandler((sender, e) => CtrlEPressed?.Invoke(sender, e));
            serviceForm.CtrlFPressed += new EventHandler((sender, e) => CtrlFPressed?.Invoke(sender, e));
            serviceForm.CtrlF5Pressed += new EventHandler((sender, e) => CtrlF5Pressed?.Invoke(sender, e));
            serviceForm.CtrlGPressed += new EventHandler((sender, e) => CtrlGPressed?.Invoke(sender, e));
            serviceForm.CtrlIPressed += new EventHandler((sender, e) => CtrlIPressed?.Invoke(sender, e));
            serviceForm.CtrlMinusPressed += new EventHandler((sender, e) => CtrlMinusPressed?.Invoke(sender, e));
            serviceForm.CtrlMPressed += new EventHandler((sender, e) => CtrlMPressed?.Invoke(sender, e));
            serviceForm.CtrlOPressed += new EventHandler((sender, e) => CtrlOPressed?.Invoke(sender, e));
            serviceForm.CtrlPlusPressed += new EventHandler((sender, e) => CtrlPlusPressed?.Invoke(sender, e));
            serviceForm.CtrlPPressed += new EventHandler((sender, e) => CtrlPPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftDelPressed += new EventHandler((sender, e) => CtrlShiftDelPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftEPressed += new EventHandler((sender, e) => CtrlShiftEPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftMinusPressed += new EventHandler((sender, e) => CtrlShiftMinusPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftNPressed += new EventHandler((sender, e) => CtrlShiftNPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftPlusPressed += new EventHandler((sender, e) => CtrlShiftPlusPressed?.Invoke(sender, e));
            serviceForm.CtrlShiftPPressed += new EventHandler((sender, e) => CtrlShiftPPressed?.Invoke(sender, e));
            serviceForm.CtrlTPressed += new EventHandler((sender, e) => CtrlTPressed?.Invoke(sender, e));
            serviceForm.CtrlUPressed += new EventHandler((sender, e) => CtrlUPressed?.Invoke(sender, e));
            serviceForm.CtrlZeroPressed += new EventHandler((sender, e) => CtrlZeroPressed?.Invoke(sender, e));
            serviceForm.DownPressed += new EventHandler((sender, e) => DownPressed?.Invoke(sender, e));
            serviceForm.EndPressed += new EventHandler((sender, e) => EndPressed?.Invoke(sender, e));
            serviceForm.F11Pressed += new EventHandler((sender, e) => F11Pressed?.Invoke(sender, e));
            serviceForm.F12Pressed += new EventHandler((sender, e) => F12Pressed?.Invoke(sender, e));
            serviceForm.F2Pressed += new EventHandler((sender, e) => F2Pressed?.Invoke(sender, e));
            serviceForm.F3Pressed += new EventHandler((sender, e) => F3Pressed?.Invoke(sender, e));
            serviceForm.F5Pressed += new EventHandler((sender, e) => F5Pressed?.Invoke(sender, e));
            serviceForm.F7Pressed += new EventHandler((sender, e) => F7Pressed?.Invoke(sender, e));
            serviceForm.F8Pressed += new EventHandler((sender, e) => F8Pressed?.Invoke(sender, e));
            serviceForm.F9Pressed += new EventHandler((sender, e) => F9Pressed?.Invoke(sender, e));
            serviceForm.HomePressed += new EventHandler((sender, e) => HomePressed?.Invoke(sender, e));
            serviceForm.PageDownPressed += new EventHandler((sender, e) => PageDownPressed?.Invoke(sender, e));
            serviceForm.PageUpPressed += new EventHandler((sender, e) => PageUpPressed?.Invoke(sender, e));
            serviceForm.ShiftF3Pressed += new EventHandler((sender, e) => ShiftF3Pressed?.Invoke(sender, e));
            serviceForm.UpPressed += new EventHandler((sender, e) => UpPressed?.Invoke(sender, e));
            serviceForm.Service = this;
            if (serviceForm.ShowDialog().Equals(DialogResult.OK)) {
                Update?.Invoke(this, EventArgs.Empty);
            }
        }

        public IntPtr GetFormHandle() {
            return IsOpened ? (IntPtr)serviceForm.Invoke(new HandleCallback(() => serviceForm.Handle)) : IntPtr.Zero;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Expiration", Expiration);
            info.AddValue("Name", Name);
            info.AddValue("Price", Price);
            info.AddValue("Span", Span);
            info.AddValue("Status", serviceStatus);
            info.AddValue("Subscribed", Subscribed);
            info.AddValue("Uid", Uid);
            info.AddValue("Unit", Unit);
        }

        private void SetUid() => Uid = Hash.MD5(Name, Encoding.UTF8);

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder()
                .Append("Name")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Name)
                .Append("Price")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Price.ToString(Constants.TwoDecimalDigitsFormat, CultureInfo.InvariantCulture))
                .Append("Span")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .Append(Span.ToString())
                .Append(Constants.Space)
                .AppendLine(Unit.ToString())
                .Append("Expiration")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Expiration.ToString(Constants.ToStringTimeFormat, CultureInfo.InvariantCulture))
                .Append("Subscribed")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Subscribed.ToString(Constants.ToStringTimeFormat, CultureInfo.InvariantCulture))
                .Append("Status")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .AppendLine(Status.ToString())
                .Append("Uid")
                .Append(Constants.Colon)
                .Append(Constants.Space)
                .Append(Uid);
            return stringBuilder.ToString();
        }

        public enum ServiceStatus {
            Active,
            Expired
        }

        public enum SpanUnit {
            Day,
            Month,
            Year
        }
    }
}
