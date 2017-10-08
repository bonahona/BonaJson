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
    public class JObjectArray : JObject
    {
        protected List<JObject> m_collection;

        public JObjectArray()
            : base()
        {
            m_collection = new List<JObject>();
            m_value = m_collection;
        }

        public override JObject Add(object element)
        {
            if (element == null) {
                return Add(new JNullObject());
            } else if (element is bool) {
                return Add(new JBoolObject((bool)element)); 
            } else if (element is float) {
                return Add(new JValueObject((float)element));
            } else if (element is int) {
                return Add(new JIntObject((int)element));
            } else if (element is string) {
                return Add(new JStringObject((string)element));
            } else if (element is JObject) {
                return Add((JObject)element);
            } else if (element is ISavable) {
                ISavable tmpObject = (ISavable)element;
                return Add(tmpObject.Save());
            } else {
                throw new InvalidCastException();
            }
        }

        public override JObject Add(JObject element)
        {
            m_collection.Add(element);
            return element;
        }

        public override JObject Add(string name, object child)
        {
            return Add(child);
        }

        public override JObject Add(string name, JObject child)
        {
 	        m_collection.Add(child);
            return child;
        }

        public override bool Remove(string name)
        {
 	        throw new InvalidOperationException();
        }

        public override bool Remove(JObject element)
        {
            return m_collection.Remove(element);
        }

        public override JObject this[string key]
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override JObject this[int index]
        {
            get
            {
                if (index < m_collection.Count) {
                    return m_collection[index];
                } else {
                    return null;
                }
            }
        }

        public override IEnumerator<JObject> GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }

        public override int Count
        {
            get{ return m_collection.Count; }
        }

        public override string ToString()
        {
            String result = "[";

            int count = 0;
            int length = m_collection.Count - 1;
            foreach (var element in m_collection) {
                if (element == null) {
                    result += "null";
                } else {
                    result += element.ToString();
                }

                if (count < length) {
                    result += ",";
                }

                count++;
            }

            result += "]";

            return result;
        }

        public override void AddNodeToPrettyPrint(Internal.PrettyPrintData data)
        {
            data.Text += "[\n";
            data.CurrentTab++;

            int count = 0;
            int length = m_collection.Count - 1;
            foreach (var element in m_collection) {
                data.Text += "\n" + GetTabs(data.CurrentTab);

                if (element == null) {
                    data.Text += "null";
                } else {
                    element.AddNodeToPrettyPrint(data);
                }

                if (count < length) {
                    data.Text += ",";
                }

                count++;
            }

            data.CurrentTab--;
            data.Text += "\n";

            data.Text += GetTabs(data.CurrentTab) + "]\n";
        }
    }
}
