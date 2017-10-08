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
using System.Reflection;

namespace BonaJson.Internal
{
    public sealed class Serializer
    {

        public JObject SerializeObject(object o)
        {
            try {
                // Hashset to keep track of what objects has already been added. Is used to make sure cyclical data structures are added
                var objects = new HashSet<object>();
                return SerializeAnyObject(o, objects);
            } catch (Exception e) {
                throw new BonaJsonException("Failed to serialize object, see inner exception", e);
            }
        }

        private bool IsInt(object o)
        {
            if( o is byte ||
                o is short || 
                o is int || 
                o is long ||
                o is UInt16 ||
                o is UInt32 ||
                o is UInt64 ||
                o is Int16 ||
                o is UInt16 ||
                o is UInt32
            ){
                return true;
            }else{
                return false;
            }
        }

        private JObject SerializeAnyObject(object o, HashSet<object> objects)
        {

            if (o == null) {
                return new JNullObject();
            }

            if (IsInt(o)) {
                return Serialize((Convert.ToInt64(o)));
            } else if (o is bool) {
                return Serialize((bool)o);
            } else if (o is float) {
                return Serialize((float)o);
            }else if(o is String){
                return Serialize((String)o);
            } else if (o is IList) {
                return Serialize((IList)o, objects);
            } else if (o is object) {
                return Serialize(o, objects);
            } else {
                return null;
            }
        }

        private JObject Serialize(object o, HashSet<object> objects)
        {
            if (o.GetType().IsSubclassOf(typeof(object))) {
                return new JNullObject();
            } else if (o is object) {
                return SerializeCustomObject(o, objects);
            } else {
                throw new BonaJsonException("Failed to serialize object. Is of non-object type " + o.GetType().ToString(), o);
            }
        }

        private JObject SerializeCustomObject(object o, HashSet<object> objects)
        {
            if (objects.Contains(o)) {
                throw new BonaJsonException("Failed to serialize object. Cyclical data structure detected", o);
            }

            objects.Add(o);
            var result = new JObjectCollection();

            var properties = o.GetType().GetProperties();
            var fields = o.GetType().GetFields();
            foreach (var property in properties) {
                if (property.PropertyType.IsArray) {
                    var value = property.GetValue(o, null);
                    result.Add(property.Name, SerializeArray(value, objects));
                } else {
                    result.Add(property.Name, SerializeAnyObject(property.GetValue(o, null), objects));
                }
            }
            foreach (var field in fields) {
                if (field.FieldType.IsArray) {
                    var value = field.GetValue(o);
                    result.Add(field.Name, SerializeArray(value, objects));
                } else {
                    result.Add(field.Name, SerializeAnyObject(field.GetValue(o), objects));
                }
            }


            objects.Remove(o);
            return result;
        }

        private JObject Serialize(int i)
        {
            return new JIntObject(i);
        }

        private JObject Serialize(float f)
        {
            return new JValueObject(f);
        }

        private JObject Serialize(bool b)
        {
            return new JBoolObject(b);
        }

        private JObject Serialize(String s)
        {
            return new JStringObject(s);
        }

        private JObject Serialize(IList collection, HashSet<object> objects)
        {
            if (objects.Contains(collection)) {
                throw new BonaJsonException("Failed to serialize object. Cyclical data structure detected", collection);
            }

            objects.Add(collection);

            var result = new JObjectArray();

            foreach (var item in collection) {
                result.Add(SerializeAnyObject(item, objects));
            }

            objects.Remove(collection);
            return result;  
        }

        private JObject SerializeArray(object o, HashSet<object> objects)
        {
            var array = o as Array;
            var result = new JObjectArray();

            foreach (var item in array) {
                result.Add(SerializeAnyObject(item, objects));
            }

            return result;
        }
    }
}