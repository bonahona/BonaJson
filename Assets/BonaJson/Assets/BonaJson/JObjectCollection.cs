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

namespace BonaJson
{
    public class JObjectCollection : JObject 
    {
        protected Dictionary<string, JObject> m_collection;
        protected List<String> m_keyOrder;

        public JObjectCollection()
            : base()
        {
            m_collection = new Dictionary<string, JObject>();
            m_keyOrder = new List<string>();
            m_value = m_collection;
        }

        public override JObject Add(object elemet)
        {
            throw new InvalidOperationException();
        }
        public override JObject Add(JObject element)
        {
            throw new InvalidOperationException();
        }

        public override JObject Add(string name, JObject child)
        {
            m_collection.Add(name, child);
            m_keyOrder.Add(name);

            return child;
        }

        public override JObject Add(string name, object element)
        {
            if (element == null)
            {
                return Add(name, new JNullObject());
            }
            else if (element is bool)
            {
                return Add(name, new JBoolObject((bool)element));
            }
            else if(element is float)
            {
                return Add(name, new JValueObject((float)element));
            }
            else if (element is int)
            {
                return Add(name, new JIntObject((int)element));
            }
            else if (element is string)
            {
                return Add(name, new JStringObject((string)element));
            }
            else if (element is JObject)
            {
                return Add(name, (JObject)element);
            }
            else if (element is ISavable)
            {
                ISavable tmpObject = (ISavable)element;
                return Add(name, tmpObject.Save());
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public override bool Remove(JObject element)
        {
            throw new InvalidOperationException();
        }
        public override bool Remove(string name)
        {
            m_keyOrder.Remove(name);
            return m_collection.Remove(name);
        }

        public override JObject this[string key]
        {
            get
            {
                if (m_collection.ContainsKey(key))
                {
                    var tmp = m_collection[key];
                    return tmp;
                }
                else
                {
                    return null;
                }
            }
        }

        public override JObject this[int index]
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override IEnumerator<JObject> GetEnumerator()
        {
            return m_collection.Values.ToList().GetEnumerator();
        }

        public override int Count
        {
            get { return m_collection.Count; }
        }

        public override string ToString()
        {
            String result = "{";

            int count = 0;
            int length = m_keyOrder.Count -1;
            foreach (var key in m_keyOrder)
            {
                if (m_collection[key] != null)
                {
                    result += "\"" + key + "\":" + m_collection[key].ToString();
                }
                else
                {
                    result += "\"" + key + "\":" + "null";
                }

                if (count < length)
                {
                    result += ",";
                }

                count++;
            }

            result += "}";

            return result;
        }

        public override void AddNodeToPrettyPrint(Internal.PrettyPrintData data)
        {
            data.Text += "{\n";
            data.CurrentTab++;

            int count = 0;
            int length = m_collection.Count - 1;
            foreach (var key in m_keyOrder) {
                data.Text += "\n" + GetTabs(data.CurrentTab) + "\"" + key + "\":";

                if (m_collection[key] == null) {
                    data.Text += "null";
                } else {
                    m_collection[key].AddNodeToPrettyPrint(data);
                }

                if (count < length) {
                    data.Text += ",";
                }

                count++;
            }

            data.CurrentTab--;
            data.Text += "\n";

            data.Text += GetTabs(data.CurrentTab) + "}\n";
        }
    }
}
