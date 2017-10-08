using UnityEngine;
using System.Collections;
using BonaJson;
using System;

namespace BonaJson.Example
{
    // Example of a Monobehaviour class that can be serialized and deserialized, through the use of the method T Value<T>.
    public class ISaveableExample : MonoBehaviour, ISavable
    {
        // Custom fields we want to serialize and later deserialize in this object
        public string CustomName;
        public int[] CustomIntArray;

        // Use this for initialization
        void Start()
        {
            CustomName = "This is a json Test";
            CustomIntArray = new int[]{ 1, 2, 3, 4, 5 };
        }

        JObject ISavable.Save()
        {
            var result = new JObjectCollection();

            // Add the CustomName field in the JSON stream with the name CustomName
            result.Add("CustomName", CustomName);

            // As CustomInArray is an array, it must be saved as a Json Array. First we'll create one. The add method will then return it to us so we can add stuff to it
            var customIntArrayObject = result.Add("CustomIntArray", new JObjectArray());

            // Add each element in the CustomInArray to our Json Array
            foreach (var item in CustomIntArray) {
                customIntArrayObject.Add(item);
            }

            // Return the newly constructed
            return result;
        }

        public void Load(JObject source)
        {
            // The paratameter source will be the JsonObject we want to deserialize. If serialized and loaded correctly, it will look exacly like the one Save returned

            // Load the CustomName object in the Json and store it in this objects CustomName field. The Value<T> method will load the value and try to convert it to your required type.
            // If you've serialised an int and tries to read is as a string it will result in an InvalidCastException. Make sure the types match!.
            // Value<T> works for most primitives C# provides and for classes that implements the ISavable interface.
            this.CustomName = source["CustomName"].Value<String>();

            // The JsonObjectArray knows the count of items it contains. We'll use that to create a new array of the correct size. Then we simply iterate over the array, fetching the valeus one by one
            this.CustomIntArray = new int[source["CustomIntArray"].Count];
            var count = 0;
            foreach (var item in source["CustomIntArray"]) {
                this.CustomIntArray[count ++] = item.Value<int>();
            }
        }
    }
}
