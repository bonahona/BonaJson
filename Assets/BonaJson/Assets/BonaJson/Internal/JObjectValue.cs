﻿/*
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
    public class JObjectValue : JObject
    {

        public override JObject Add(object element)
        {
            throw new InvalidOperationException();
        }
        public override JObject Add(JObject element)
        {
            throw new InvalidOperationException();
        }
        public override JObject Add(string name, object element)
        {
            throw new InvalidOperationException();
        }
        public override JObject Add(string name, JObject child)
        {
            throw new InvalidOperationException();
        }

        public override bool Remove(JObject element)
        {
            throw new InvalidOperationException();
        }

        public override bool Remove(string name)
        {
            throw new InvalidOperationException();
        }

        public override JObject this[string key]
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override void AddNodeToPrettyPrint(Internal.PrettyPrintData data)
        {
            data.Text += m_value.ToString();
        }
    }
}
