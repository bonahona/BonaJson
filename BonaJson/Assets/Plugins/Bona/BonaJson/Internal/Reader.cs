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
