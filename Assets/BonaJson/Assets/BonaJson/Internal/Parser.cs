/*
The MIT License(MIT)

Copyright(c) 2015 Björn Fyrvall

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
