/**
 * This is open-source software licensed under the terms of the MIT License.
 *
 * Copyright (c) 2012-2023 Petr Červinka - FortSoft <cervinka@fortsoft.eu>
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

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FortSoft.Tools {

    /// <summary>
    /// Contains static methods needed to work with hash operations.
    /// </summary>
    public static class Hash {

        /// <summary>
        /// Checks if the provided string matches the hash of the appropriate
        /// algorithm.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="algorithm">Hash algorithm.</param>
        public static bool IsHash(string str, Algorithm algorithm) {
            switch (algorithm) {
                case Algorithm.MD5:
                    return Regex.IsMatch(str, "^[0-9a-f]{32}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                case Algorithm.SHA1:
                    return Regex.IsMatch(str, "^[0-9a-f]{40}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                case Algorithm.SHA256:
                    return Regex.IsMatch(str, "^[0-9a-f]{64}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                case Algorithm.SHA384:
                    return Regex.IsMatch(str, "^[0-9a-f]{96}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                case Algorithm.SHA512:
                    return Regex.IsMatch(str, "^[0-9a-f]{128}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the provided string matches the hash of a known algorithm
        /// within this class.
        /// </summary>
        /// <param name="hash">Input hash.</param>
        public static Algorithm GetAlgorithm(string hash) {
            switch (hash.Length) {
                case 32:
                    return Regex.IsMatch(hash, "^[0-9a-f]*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                        ? Algorithm.MD5
                        : Algorithm.Undefined;
                case 40:
                    return Regex.IsMatch(hash, "^[0-9a-f]*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                        ? Algorithm.SHA1
                        : Algorithm.Undefined;
                case 64:
                    return Regex.IsMatch(hash, "^[0-9a-f]*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                        ? Algorithm.SHA256
                        : Algorithm.Undefined;
                case 96:
                    return Regex.IsMatch(hash, "^[0-9a-f]*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                        ? Algorithm.SHA384
                        : Algorithm.Undefined;
                case 128:
                    return Regex.IsMatch(hash, "^[0-9a-f]*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                        ? Algorithm.SHA512
                        : Algorithm.Undefined;
                default:
                    return Algorithm.Undefined;
            }
        }

        /// <summary>
        /// Converts an array of bytes to a string of sequences of hexadecimal
        /// values.
        /// </summary>
        /// <param name="bytes">Input array of bytes.</param>
        private static string ToHexString(byte[] bytes) {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes) {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Calculates hash using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        public static string MD5(string str, Encoding encoding) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return ToHexString(md5.ComputeHash(encoding.GetBytes(str)));
        }

        /// <summary>
        /// Calculates hash using the SHA-1 (Secure Hash Algorithm 1).
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        public static string SHA1(string str, Encoding encoding) {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            return ToHexString(sha1.ComputeHash(encoding.GetBytes(str)));
        }

        /// <summary>
        /// Calculates hash using the 256bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        public static string SHA256(string str, Encoding encoding) {
            SHA1CryptoServiceProvider sha256 = new SHA1CryptoServiceProvider();
            return ToHexString(sha256.ComputeHash(encoding.GetBytes(str)));
        }

        /// <summary>
        /// Calculates hash using the 384bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        public static string SHA384(string str, Encoding encoding) {
            SHA1CryptoServiceProvider sha384 = new SHA1CryptoServiceProvider();
            return ToHexString(sha384.ComputeHash(encoding.GetBytes(str)));
        }

        /// <summary>
        /// Calculates hash using the 512bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        public static string SHA512(string str, Encoding encoding) {
            SHA1CryptoServiceProvider sha512 = new SHA1CryptoServiceProvider();
            return ToHexString(sha512.ComputeHash(encoding.GetBytes(str)));
        }

        /// <summary>
        /// Calculates hash using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public static string MD5(Stream stream) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return ToHexString(md5.ComputeHash(stream));
        }

        /// <summary>
        /// Calculates hash using the SHA-1 (Secure Hash Algorithm 1).
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public static string SHA1(Stream stream) {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            return ToHexString(sha1.ComputeHash(stream));
        }

        /// <summary>
        /// Calculates hash using the 256bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public static string SHA256(Stream stream) {
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            return ToHexString(sha256.ComputeHash(stream));
        }

        /// <summary>
        /// Calculates hash using the 384bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public static string SHA384(Stream stream) {
            SHA384CryptoServiceProvider sha384 = new SHA384CryptoServiceProvider();
            return ToHexString(sha384.ComputeHash(stream));
        }

        /// <summary>
        /// Calculates hash using the 512bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public static string SHA512(Stream stream) {
            SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
            return ToHexString(sha512.ComputeHash(stream));
        }

        /// <summary>
        /// Calculates hash using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static string MD5(string filePath) {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                if (new FileInfo(filePath).Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        md5.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    md5.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(md5.Hash);
                } else {
                    return ToHexString(md5.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the SHA-1 (Secure Hash Algorithm 1).
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static string SHA1(string filePath) {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                if (new FileInfo(filePath).Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha1.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha1.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha1.Hash);
                } else {
                    return ToHexString(sha1.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 256bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static string SHA256(string filePath) {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                if (new FileInfo(filePath).Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha256.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha256.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha256.Hash);
                } else {
                    return ToHexString(sha256.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 384bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static string SHA384(string filePath) {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA384CryptoServiceProvider sha384 = new SHA384CryptoServiceProvider();
                if (new FileInfo(filePath).Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha384.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha384.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha384.Hash);
                } else {
                    return ToHexString(sha384.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 512bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static string SHA512(string filePath) {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
                if (new FileInfo(filePath).Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha512.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha512.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha512.Hash);
                } else {
                    return ToHexString(sha512.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public static string MD5(FileInfo fileInfo) {
            using (FileStream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                if (fileInfo.Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        md5.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    md5.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(md5.Hash);
                } else {
                    return ToHexString(md5.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the SHA-1 (Secure Hash Algorithm 1).
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public static string SHA1(FileInfo fileInfo) {
            using (FileStream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                if (fileInfo.Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha1.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha1.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha1.Hash);
                } else {
                    return ToHexString(sha1.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 256bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public static string SHA256(FileInfo fileInfo) {
            using (FileStream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                if (fileInfo.Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha256.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha256.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha256.Hash);
                } else {
                    return ToHexString(sha256.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 384bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public static string SHA384(FileInfo fileInfo) {
            using (FileStream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA384CryptoServiceProvider sha384 = new SHA384CryptoServiceProvider();
                if (fileInfo.Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha384.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha384.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha384.Hash);
                } else {
                    return ToHexString(sha384.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using the 512bits SHA-2 (Secure Hash Algorithm 2).
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public static string SHA512(FileInfo fileInfo) {
            using (FileStream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
                if (fileInfo.Length >= 8192) {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                        sha512.TransformBlock(buffer, 0, read, buffer, 0);
                    }
                    sha512.TransformFinalBlock(buffer, 0, 0);
                    return ToHexString(sha512.Hash);
                } else {
                    return ToHexString(sha512.ComputeHash(fileStream));
                }
            }
        }

        /// <summary>
        /// Calculates hash using selected algorithm.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="encoding">Encoding the text in the string.</param>
        /// <param name="algorithm">Hash algorithm.</param>
        public static string Compute(string str, Encoding encoding, Algorithm algorithm) {
            switch (algorithm) {
                case Algorithm.MD5:
                    return MD5(str, encoding);
                case Algorithm.SHA1:
                    return SHA1(str, encoding);
                case Algorithm.SHA256:
                    return SHA256(str, encoding);
                case Algorithm.SHA384:
                    return SHA384(str, encoding);
                case Algorithm.SHA512:
                    return SHA512(str, encoding);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates hash using selected algorithm.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="algorithm">Hash algorithm.</param>
        public static string Compute(Stream stream, Algorithm algorithm) {
            switch (algorithm) {
                case Algorithm.MD5:
                    return MD5(stream);
                case Algorithm.SHA1:
                    return SHA1(stream);
                case Algorithm.SHA256:
                    return SHA256(stream);
                case Algorithm.SHA384:
                    return SHA384(stream);
                case Algorithm.SHA512:
                    return SHA512(stream);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates hash using selected algorithm.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="algorithm">Hash algorithm.</param>
        public static string Compute(string filePath, Algorithm algorithm) {
            switch (algorithm) {
                case Algorithm.MD5:
                    return MD5(filePath);
                case Algorithm.SHA1:
                    return SHA1(filePath);
                case Algorithm.SHA256:
                    return SHA256(filePath);
                case Algorithm.SHA384:
                    return SHA384(filePath);
                case Algorithm.SHA512:
                    return SHA512(filePath);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates hash using selected algorithm.
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        /// <param name="algorithm">Hash algorithm.</param>
        public static string Compute(FileInfo fileInfo, Algorithm algorithm) {
            switch (algorithm) {
                case Algorithm.MD5:
                    return MD5(fileInfo);
                case Algorithm.SHA1:
                    return SHA1(fileInfo);
                case Algorithm.SHA256:
                    return SHA256(fileInfo);
                case Algorithm.SHA384:
                    return SHA384(fileInfo);
                case Algorithm.SHA512:
                    return SHA512(fileInfo);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Hash algorithms.
        /// </summary>
        public enum Algorithm {
            /// <summary>
            /// Undefined hash algorithm.
            /// </summary>
            Undefined,
            /// <summary>
            /// MD5 message-digest algorithm.
            /// </summary>
            MD5,
            /// <summary>
            /// SHA-1 (Secure Hash Algorithm 1).
            /// </summary>
            SHA1,
            /// <summary>
            /// 256bits SHA-2 (Secure Hash Algorithm 2).
            /// </summary>
            SHA256,
            /// <summary>
            /// 384bits SHA-2 (Secure Hash Algorithm 2).
            /// </summary>
            SHA384,
            /// <summary>
            /// 512bits SHA-2 (Secure Hash Algorithm 2).
            /// </summary>
            SHA512
        }
    }
}
