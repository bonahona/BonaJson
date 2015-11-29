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
