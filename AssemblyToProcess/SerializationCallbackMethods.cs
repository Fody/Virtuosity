using System;
using System.Runtime.Serialization;

[Serializable]
public class SerializationCallbackMethods
{
    [OnSerializing]
    public void Serializing(StreamingContext context) { }

    [OnSerialized]
    public void Serialized(StreamingContext context) { }

    [OnDeserializing]
    public void Deserializing(StreamingContext context) { }

    [OnDeserialized]
    public void Deserialized(StreamingContext context) { }
}
