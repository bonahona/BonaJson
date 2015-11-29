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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BonaJson.Internal;

namespace BonaJson
{
    public abstract class JObject
    {
        public static JObject Parse(string jsonData)
        {
            Internal.Parser parser = new Internal.Parser(jsonData);

            object result = parser.ParseObject();

            if (result is JObjectCollection)
            {
                return result as JObjectCollection;
            }
            else if (result is JObjectArray)
            {
                return result as JObjectArray;
            }
            else
            {
                throw new Internal.JsonFormatException("File does not start with Object or Array", 0, 0, "");
            }
        }

        public static JObject Serialize(object o)
        {
            var serializer = new Internal.Serializer();

            return serializer.SerializeObject(o);
        }

        protected object m_value;

        public object RawValue
        {
            get
            {
                return m_value;
            }
        }

        public virtual T Value<T>()
        {

            if (m_value is List<JObject> || m_value is Dictionary<String, JObject>)
            {
                T result = Activator.CreateInstance<T>();
                if (result is ISavable)
                {
                    ISavable savable = result as ISavable;
                    savable.Load(this);
                    return (T)result;
                }

                return default(T);
            }
            else
            {
                var typeTest = default(T);
                if (typeTest is float) {
                    return (T)m_value;
                }

                return (T)m_value;
            }
        }

        public virtual T Deserialize<T>()
        {
            var deserializer = new Internal.Deserializer(this);
            return deserializer.Deserialize<T>();
        }

        // Needed as the generic function dont want to work with floats
        public virtual float GetAsFloat()
        {
            return 0;
        }

        public JObject()
        {
            m_value = null;
        }

        public abstract JObject Add(object element);
        public abstract JObject Add(JObject element);
        public abstract JObject Add(string name, JObject child);
        public abstract JObject Add(string name, object element);
        public abstract bool Remove(JObject element);
        public abstract bool Remove(string name);
        public virtual IEnumerator<JObject> GetEnumerator()
        {
            return null;
        }
        public virtual JObject this[string key]
        {
            get { throw new InvalidOperationException(); }
        }
        public virtual JObject this[int index]
        {
            get { throw new InvalidOperationException(); }
        }
        public virtual int Count
        {
            get { throw new InvalidOperationException(); }
        }

        public String PrettyPrint()
        {
            PrettyPrintData data = new PrettyPrintData();
            this.AddNodeToPrettyPrint(data);
            return data.Text;
        }

        public abstract void AddNodeToPrettyPrint(PrettyPrintData data);

        protected string GetTabs(int tabs)
        {
            String result = "";

            for (int i = 0; i < tabs; i++)
            {
                result += "\t";
            }

            return result;
        }
    }
}
