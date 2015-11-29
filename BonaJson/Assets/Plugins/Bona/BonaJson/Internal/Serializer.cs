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