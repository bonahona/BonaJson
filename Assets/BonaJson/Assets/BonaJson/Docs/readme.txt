 Bona Json Library for Unity
 Version 1, 29 November 2015
 ---------------------------

 This package is a library extension for Unity that allows for JSON serializing and deserializing for C# objects.
 It can work in two ways. The first is to use reflection for serialization/deserializaing. It will simply write down
 all public properties and fields down to a JSON stream. This works great for specially written classes made for this,
 but quite poor for larger,complex C# types like MonoBeheaviours and the likes. The other way is to make classes
 implement the ISavable interface to explicitly define what data is to be written and read be the serializer/deserialiser.
 This works great for Monobehaviours where you only wan't to save and load a few, self defined, properties and ignore
 Unity's inbuilt ones.
 Don't mix these two methods. It's not tested and will result in undefined behaviour.
 
 To parse a json string into a JObject use the method static JObject JObject.Parse(string jsonString);
 To get a json string from a JObject you can either use ToString() to get it unformatted or PrettyPrint() to get it nicely tabulated and with proper line breaks.
 The entire package lies withing the BonaJson namespace to avoid cluttering down the global namespace.
 To use the package's classes remeber to add it with "using BonaJson;" in each class file.
 
 The JSON implementation quite bare but can do the basics, please see the example scenes for more information on how to use the package.
 It's RFC-4627 compliant (https://www.ietf.org/rfc/rfc4627.txt). 
 
 License
 -------
 This package is Open-source under the MIT license, please read the LICENSE.txt for for information.
 Source is available from github on https://github.com/bonahona/BonaJson
 
 Author
 ------
 Written and published by Björn Fyrvall
 
 Version history
 ---------------
 Version 1, 29 November 2015.

 Initial release