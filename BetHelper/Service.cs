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

using FortSoft.Tools;
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace BetHelper {

    [Serializable()]
    public class Service : ISerializable {
        public Service(
                string name,
                decimal price,
                int span,
                SpanUnit unit,
                DateTime expiration,
                DateTime subscribed,
                ServiceStatus status) {

            Expiration = expiration;
            Name = name;
            Price = price;
            Span = span;
            Status = status;
            Subscribed = subscribed;
            Unit = unit;
            SetUid();
        }

        public Service(SerializationInfo info, StreamingContext ctxt) {
            Expiration = (DateTime)info.GetValue("Expiration", typeof(DateTime));
            Name = (string)info.GetValue("Name", typeof(string));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            Span = (int)info.GetValue("Span", typeof(int));
            Status = (ServiceStatus)info.GetValue("Status", typeof(ServiceStatus));
            Subscribed = (DateTime)info.GetValue("Subscribed", typeof(DateTime));
            Uid = (string)info.GetValue("Uid", typeof(string));
            Unit = (SpanUnit)info.GetValue("Unit", typeof(SpanUnit));
        }

        public DateTime Expiration { get; set; }

        public DateTime Subscribed { get; set; }

        public decimal Price { get; set; }

        public int Span { get; set; }

        public ServiceStatus Status { get; set; }

        public SpanUnit Unit { get; set; }

        public string Name { get; set; }

        public string Uid { get; private set; }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Expiration", Expiration);
            info.AddValue("Name", Name);
            info.AddValue("Price", Price);
            info.AddValue("Span", Span);
            info.AddValue("Status", Status);
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
