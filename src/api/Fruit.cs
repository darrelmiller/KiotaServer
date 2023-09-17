// using Microsoft.Kiota.Abstractions.Serialization;

// public class Fruit : IParsable
// {
//     public static Fruit CreateFromDiscriminatorValue(IParseNode node)
//     {
//         _ = node ?? throw new ArgumentNullException(nameof(node));
//         return new Fruit();
//     }
//     public string? Name { get; set; }
//     public string? Color { get; set; }
//     public int? Weight { get; set; }

//     public IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
//     {
//         return new Dictionary<string, Action<IParseNode>> {
//             {
//                 "name", n => { Name = n.GetStringValue(); }
//             },
//             {
//                 "color", n => { Color = n.GetStringValue(); }
//             },
//             {
//                 "weight", n => { Weight = n.GetIntValue(); }
//             },
//         };
//     }

//     public void Serialize(ISerializationWriter writer)
//     {
//         writer.WriteStringValue("name", Name);
//         writer.WriteStringValue("color", Color);
//         writer.WriteIntValue("weight", Weight);

//     }
// }