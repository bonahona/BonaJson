/*
    Copyright 2014 Björn Fyrvall

    This file is part of BonaJson

    BonaJson is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BonaJson is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BonaJson.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BonaJson.Internal
{
    public sealed class Parser
    {
        public const char START_CURL = '{';
        public const char END_CURL = '}';
        public const char START_ARRAY = '[';
        public const char END_ARRAY = ']';
        public const char QUOTE = '\"';
        public const char COMMA = ',';
        public const char COLON = ':';
        public const char DOT = '.';
        public const char MINUS = '-';
        public const char EOF = '\0';

        public const string FALSE = "false";
        public const string TRUE = "true";
        public const string NULL = "null";

        public Reader m_reader;

        public Parser(string jsonData)
        {
            m_reader = new Reader(jsonData);
        }


        public object ParseObject()
        {
            if (m_reader.Peek() == START_CURL)
            {
                return ParseJObjectCollection();
            }
            else if (m_reader.Peek() == START_ARRAY)
            {
                return ParseJObjectArray();
            }
            else if(m_reader.Peek() == QUOTE)
            {
                return ParseString();
            }
            else if(Char.IsLetterOrDigit(m_reader.Peek()) || m_reader.Peek() == MINUS)
            {
                return ParsePrimitive();
            }
            else
            {
                throw new JsonFormatException("Invalid object start", m_reader.Index(), m_reader.Line, m_reader.Context);
            }
        }

        public JObjectCollection ParseJObjectCollection()
        {
            JObjectCollection result = new JObjectCollection();
            m_reader.Next();

            if (m_reader.Peek() == END_CURL)
            {
                m_reader.Next();
                return result;
            }

            bool run = true;

            while(run)
            {
                string name;
                object value;

                if (m_reader.Peek() == QUOTE)
                {
                    name = ParseString();
                }
                else
                {
                    throw new JsonFormatException("Expected name", m_reader.Index(), m_reader.Line, m_reader.Context);
                }

                if (m_reader.Peek() == COLON)
                {
                    m_reader.Next();
                }
                else
                {
                    throw new JsonFormatException("Expected colon", m_reader.Index(), m_reader.Line, m_reader.Context);
                }

                value = ParseObject();

                if (m_reader.Peek() == COMMA)
                {
                    m_reader.Next();
                }
                else
                {
                    run = false;
                }

                result.Add(name, value);

            }

            m_reader.Next();

            return result;
        }

        public JObjectArray ParseJObjectArray()
        {
            m_reader.Next();

            JObjectArray result = new JObjectArray();

            if (m_reader.Peek() == END_ARRAY)
            {
                m_reader.Next();
                return result;
            }

            bool run = true;

            while (run)
            {

                object value = ParseObject();

                if (m_reader.Peek() == COMMA)
                {
                    m_reader.Next();
                }
                else
                {
                    run = false;
                }

                result.Add(value);
            }

            m_reader.Next();

            return result;
        }

        public object ParsePrimitive()
        {
            StringBuilder buffer = new StringBuilder(20);

            int index = 0;

            while(Char.IsLetterOrDigit(m_reader.Peek()) || m_reader.Peek() == '.' || (index == 0 && m_reader.Peek() == MINUS))
            {
                m_reader.Next();
                buffer.Append(m_reader.Current());
                index++;
            }

            String resultString = buffer.ToString();
            float resultFloat = 0;
            int resultInt = 0;

            if (resultString.ToLower() == FALSE)
            {
                return false;
            }
            else if (resultString.ToLower() == TRUE)
            {
                return true;
            }
            else if (resultString.ToLower() == NULL)
            {
                return null;
            }
            else if (resultString.Contains("."))
            {
                if (float.TryParse(resultString, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out resultFloat))
                {
                    return resultFloat;
                }
                else
                {
                    throw new JsonFormatException("Expexted float", m_reader.Index(), m_reader.Line, m_reader.Context);
                }

            }
            else if (int.TryParse(resultString, out resultInt))
            {
                return int.Parse(resultString);
            }
            else
            {
                throw new JsonFormatException("Unexpected character found", m_reader.Index(), m_reader.Line, m_reader.Context);
            }
        }

        public string ParseString()
        {
            StringBuilder buffer = new StringBuilder(10);

            m_reader.Next();

            while (m_reader.PeekRaw() != QUOTE)
            {
                m_reader.NextRaw();
                if (m_reader.Current() == EOF)
                {
                    throw new JsonFormatException("Unexpected end-of-file", m_reader.Index(), m_reader.Line, m_reader.Context);
                }

                buffer.Append(m_reader.Current());
            }

            m_reader.Next();
            return buffer.ToString();
        }

        public bool ParseBool()
        {
            return false;
        }

        public float ParseFloat()
        {
            return 0;
        }
    }
}
