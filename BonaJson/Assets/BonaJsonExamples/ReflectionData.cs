using UnityEngine;
using System.Collections;
using System;

// Most important this with this class: There is nothing inherited. No intergaces and no attributes attatched. It just works.
public class ReflectionData
{
    // Field we want to serialize and deserialize into JSON.
    // With reflective serialization, all public fields and properties will be serialize. The just have to exists.
    public String CustomName;
    public int[] CustomIntArray;

    public ReflectionData()
    {
        // Give the fields some data thats not their default values.
        CustomName = "This is a custom object";
        CustomIntArray = new int[] { 0, 1, 2, 3, 4, 5 };
    }

    // These two methods are not neccesary for the packate to work but rather shows a way of moving data between the MonoBehaviours and the JSON classes.
    // The names of the fields in the two classes don't have to match. Both classes don't even have to have all fields. This is just for the sake of simplicity during this example.
    // In proper projects, it's always up to the application developer.
    public void FromMonoBehavior(ReflectionExample relfectionExample)
    {
        CustomName = relfectionExample.CustomName;
        CustomIntArray = relfectionExample.CustomIntArray;
    }
    public void ToMonoBehavior(ReflectionExample relfectionExample)
    {
        relfectionExample.CustomName = CustomName;
        relfectionExample.CustomIntArray = CustomIntArray;
    }
}
