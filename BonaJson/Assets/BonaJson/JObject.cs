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

            if (result is JObjectCollection){
                return result as JObjectCollection;
            }else if (result is JObjectArray){
                return result as JObjectArray;
            }else{
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
            get { return m_value; }
        }

        public virtual T Value<T>()
        {
            if (m_value is List<JObject> || m_value is Dictionary<String, JObject>){
                T result = Activator.CreateInstance<T>();
                if (result is ISavable)
                {
                    ISavable savable = result as ISavable;
                    savable.Load(this);
                    return (T)result;
                }

                return default(T);
            }else{
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
