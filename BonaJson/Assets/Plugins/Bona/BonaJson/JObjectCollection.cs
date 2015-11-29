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

        public override void Add(object elemet)
        {
            throw new InvalidOperationException();
        }
        public override void Add(JObject element)
        {
            throw new InvalidOperationException();
        }

        public override void Add(string name, JObject child)
        {
            m_collection.Add(name, child);
            m_keyOrder.Add(name);
        }

        public override void Add(string name, object element)
        {
            if (element == null)
            {
                Add(name, new JNullObject());
            }
            else if (element is bool)
            {
                Add(name, new JBoolObject((bool)element));
            }
            else if(element is float)
            {
                Add(name, new JValueObject((float)element));
            }
            else if (element is int)
            {
                Add(name, new JIntObject((int)element));
            }
            else if (element is string)
            {
                Add(name, new JStringObject((string)element));
            }
            else if (element is JObject)
            {
                Add(name, (JObject)element);
            }
            else if (element is ISavable)
            {
                ISavable tmpObject = (ISavable)element;
                Add(name, tmpObject.Save());
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
