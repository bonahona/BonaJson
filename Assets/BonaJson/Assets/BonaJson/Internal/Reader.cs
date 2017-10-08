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
using System.Linq;
using System.Text;

namespace BonaJson.Internal
{
    public sealed class Reader
    {
        public static readonly char[] IGNORE_CHARS = { ' ', '\t', '\n', '\r'};
        public const char EOF = '\0';

        public char m_peek;
        public string m_jsonData;
        public int m_index;
        public int m_line;

        public char CurrentValue
        {
            get { return m_jsonData[m_index]; }
        }

        public int IndexValue
        {
            get { return m_index; }
        }

        public char PeekValue
        {
            get { return m_peek; }
        }

        public int Line
        {
            get { return m_line; }
        }

        public string Context
        {
            get
            {
                int start = m_index - 40;
                if (start < 0)
                    start = 0;

                return m_jsonData.Substring(start, 40);
            }
        }

        public bool IsValid(char c1)
        {
            foreach(var c2 in IGNORE_CHARS)
            {
                if (c1 == '\n')
                    m_line++;

                if(c1 == c2)
                    return false;
            }
            
            return true;
        }

        public Reader(string jsonData)
        {
            m_jsonData = jsonData;
            m_index = -1;
        }

        public char Peek()
        {
            int startIndex = m_index +1;

            while(!IsValid(m_jsonData[startIndex]))
            {
                startIndex ++;
            }

            m_peek = m_jsonData[startIndex];
            return m_jsonData[startIndex];
        }

        public char Next()
        {
            int startIndex = m_index + 1;

            while (!IsValid(m_jsonData[startIndex]))
            {
                startIndex++;
            }

            m_index = startIndex;
            return m_jsonData[startIndex];
        }

        public char PeekRaw()
        {
            m_peek = m_jsonData[m_index + 1];
            return m_peek;
        }

        public char NextRaw()
        {
            m_index++;

            if(m_jsonData[m_index] == '\n')
                    m_line++;

            return m_jsonData[m_index];
        }

        public char Current()
        {
            return m_jsonData[m_index];
        }

        public int Index()
        {
            return m_index;
        }
    }
}
