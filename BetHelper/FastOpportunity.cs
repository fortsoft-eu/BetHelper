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
 * Version 1.1.14.0
 */

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BetHelper {
    public class FastOpportunity {
        private List<LiveGame> games;

        public FastOpportunity(string html) {
            Regex scoreRegex = new Regex(Constants.ScorePattern);
            XmlReader xmlReader = XmlReader.Create(new StringReader(new StringBuilder()
                .Append(Constants.XmlOpenTag)
                .Append(Regex.Replace(html, Constants.XmlRemovePattern, string.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .Append(Constants.XmlClosingTag)
                .ToString()));
            bool inMatch = false;
            LiveGame liveGame = new LiveGame();
            games = new List<LiveGame>();
            int n = 0;
            while (xmlReader.Read()) {
                if (xmlReader.NodeType.Equals(XmlNodeType.Element)) {
                    string l = xmlReader.GetAttribute(Constants.XmlAttributeLId);
                    if (!string.IsNullOrWhiteSpace(l)) {
                        if (inMatch) {
                            inMatch = false;
                            games.Add(liveGame);
                            liveGame = new LiveGame();
                            n = 0;
                        }
                        string[] spl = l.Split(new char[] { Constants.Period }, 2);
                        liveGame.league = spl[0];
                        if (spl.Length > 0) {
                            liveGame.sport = spl[1];
                        }
                        inMatch = true;
                    } else if (inMatch) {
                        if (string.Compare(xmlReader.LocalName, Constants.XmlElementA, true).Equals(0)) {
                            liveGame.id = xmlReader.GetAttribute(Constants.XmlAttributeHref);
                        }
                    }
                } else if (xmlReader.NodeType.Equals(XmlNodeType.Text)) {
                    switch (++n) {
                        case 1:
                            liveGame.match = xmlReader.Value.Trim();
                            break;
                        case 2:
                            liveGame.match += Constants.Space + xmlReader.Value.Trim();
                            break;
                        case 4:
                            liveGame.score += scoreRegex.Replace(xmlReader.Value, new StringBuilder()
                                .Append(Constants.ReplaceFirst)
                                .Append(Constants.Colon)
                                .Append(Constants.ReplaceSecond)
                                .ToString());
                            break;
                        case 5:
                            liveGame.time += xmlReader.Value.Trim();
                            break;
                    }

                }
            }
            if (inMatch) {
                inMatch = false;
                games.Add(liveGame);
            }
        }

        public string[] GetFOMatchingGameIds() {
            List<string> list = new List<string>();
            foreach (LiveGame game in games) {
                if (game.league.Contains(Constants.FOLeague)
                        && game.sport.Contains(Constants.FOSport)
                        && game.score.Equals(Constants.FOScore)
                        && game.time.StartsWith(Constants.FOTime)) {

                    list.Add(game.id);
                }
            }
            return list.ToArray();
        }
    }
}
