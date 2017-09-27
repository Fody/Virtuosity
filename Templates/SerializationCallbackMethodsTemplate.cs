namespace Templates
{
    using System.Runtime.Serialization;

    public class SerializationCallbackMethodsTemplate
    {
        [OnDeserializing]
        public void Deserializing() { }

        [OnDeserialized]
        public void Deserialized(StreamingContext context) { }

        [OnSerialized]
        public void Serialized(StreamingContext context) { }

        [OnSerializing]
        public void Serializing(StreamingContext context) { }
    }
}