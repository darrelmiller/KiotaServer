using Microsoft.Kiota.Abstractions.Serialization;

public class Fruit : IParsable
{
    public string Name { get; set; }
    public string Color { get; set; }
    public int Weight { get; set; }

    public IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
    {
        throw new NotImplementedException();
    }

    public void Serialize(ISerializationWriter writer)
    {
        writer.WriteStringValue("name", Name);
        writer.WriteStringValue("color", Color);
        writer.WriteIntValue("weight", Weight);

    }
}