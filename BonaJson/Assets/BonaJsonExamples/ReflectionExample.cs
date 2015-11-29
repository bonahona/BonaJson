using UnityEngine;
using System.Collections;
using System;
using BonaJson;

// Monobehaviour to be used on the game objects. Delegates loading and saving to the ReflectionData class (Which is not a Monobehaviour).
public class ReflectionExample : MonoBehaviour {

    // Fields where we will want to sertialize data to end up. 
    // Note that this object itseld will never be serialized but will delegate that to an instance of the class ReflectionData.
    public String CustomName;
    public int[] CustomIntArray;

    public void LoadFromJson(JObject source)
    {
        // This is where the automatic deserialization takes place. We just tell the Jobject that we want an object of the type RelfectionData and it will handle the rest
        var reflectionDataObject = source.Deserialize<ReflectionData>();

        // This is just a simple method a created to read the data from reflectionDataObject back into this object
        reflectionDataObject.ToMonoBehavior(this);
    }

    public JObject SaveToJson()
    {
        // Creates a new instance of reflectionData that will hold the data to be saved
        var reflectionDataObject = new ReflectionData();

        // This is just a simple method created to copy data from this object to the reflectionData instance.
        reflectionDataObject.FromMonoBehavior(this);

        // With JObject.Serialize(object o) we have now created a Json Object from this C# objects.
        var jObjectResult = JObject.Serialize(reflectionDataObject);
        return jObjectResult;
    }
}
