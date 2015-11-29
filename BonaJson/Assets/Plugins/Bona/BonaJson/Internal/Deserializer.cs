using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaJson.Internal
{
    public class Deserializer
    {
        private JObject JObject { get; set; }

        public Deserializer(JObject jObject)
        {
            JObject = jObject;
        }

        public T Deserialize<T>()
        {
            return DeserializeObject<T>(JObject);
        }

        private T DeserializeObject<T>(JObject current)
        {
            try {
                return (T)DeserializeType(typeof(T), current);
            } catch (Exception e) {
                throw new BonaJsonException("Failed to deserialize to object of type " + typeof(T).ToString() + ". See inner exception", e);
            }
        }

        private object DeserializeType(Type type, JObject current)
        {
            object result = Activator.CreateInstance(type);

            if (result is IList) {
                return DeserializeCollection(type, current);
            } else {

                var properties = result.GetType().GetProperties();
                var fields = result.GetType().GetFields();

                foreach (var property in properties) {
                    var innerObject = current[property.Name];
                    if (innerObject != null) {

                        if (property.PropertyType == typeof(Int64)) {
                            property.SetValue(result, Convert.ToInt64(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(UInt64)) {
                            property.SetValue(result, Convert.ToInt64(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(Int32)) {
                            property.SetValue(result, Convert.ToInt32(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(UInt32)) {
                            property.SetValue(result, Convert.ToUInt32(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(UInt16)) {
                            property.SetValue(result, Convert.ToUInt16(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(Int16)) {
                            property.SetValue(result, Convert.ToInt16(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(float)) {
                            property.SetValue(result, Convert.ToSingle(innerObject.RawValue), null);
                        } else if (property.PropertyType == typeof(double)) {
                            property.SetValue(result, Convert.ToDouble(innerObject.RawValue), null);
                        } else if (innerObject is JNullObject) {
                            property.SetValue(result, null, null);
                        } else if (property.PropertyType == typeof(String)) {
                            property.SetValue(result, innerObject.RawValue, null);
                        } else if (property.PropertyType.IsSubclassOf(typeof(IList))) {
                            property.SetValue(result, DeserializeType(property.PropertyType, innerObject), null);
                        } else if (property.PropertyType.IsSubclassOf(typeof(object))) {
                            property.SetValue(result, DeserializeType(property.PropertyType, innerObject), null);
                        } else {
                            property.SetValue(result, innerObject.RawValue, null);
                        }
                    }
                }

                foreach (var field in fields) {
                    var innerObject = current[field.Name];
                    if (innerObject != null) {

                        if (field.FieldType == typeof(Int64)) {
                            field.SetValue(result, Convert.ToInt64(innerObject.RawValue));
                        } else if (field.FieldType == typeof(UInt64)) {
                            field.SetValue(result, Convert.ToInt64(innerObject.RawValue));
                        } else if (field.FieldType == typeof(Int32)) {
                            field.SetValue(result, Convert.ToInt32(innerObject.RawValue));
                        } else if (field.FieldType == typeof(UInt32)) {
                            field.SetValue(result, Convert.ToUInt32(innerObject.RawValue));
                        } else if (field.FieldType == typeof(UInt16)) {
                            field.SetValue(result, Convert.ToUInt16(innerObject.RawValue));
                        } else if (field.FieldType == typeof(Int16)) {
                            field.SetValue(result, Convert.ToInt16(innerObject.RawValue));
                        } else if (field.FieldType == typeof(float)) {
                            field.SetValue(result, Convert.ToSingle(innerObject.RawValue));
                        } else if (field.FieldType == typeof(double)) {
                            field.SetValue(result, Convert.ToDouble(innerObject.RawValue));
                        } else if (innerObject is JNullObject) {
                            field.SetValue(result, null);
                        } else if (field.FieldType == typeof(String)) {
                            field.SetValue(result, innerObject.RawValue);
                        } else if (field.FieldType.IsSubclassOf(typeof(object))) {
                            field.SetValue(result, DeserializeType(field.FieldType, innerObject));
                        } else {
                            field.SetValue(result, innerObject.RawValue);
                        }
                    }
                }
            }

            return result;
        }

        private IList DeserializeCollection(Type type, JObject current)
        {
            var genericType = type.GetGenericArguments()[0];
            var listType = typeof(List<>);
            var genericListType = listType.MakeGenericType(genericType);
            var createdList = Activator.CreateInstance(genericListType);

            var result = (IList)createdList;

            foreach(var item in current){
                result.Add(DeserializeType(genericType, item));
            }

            return result;
        }
    }
}
