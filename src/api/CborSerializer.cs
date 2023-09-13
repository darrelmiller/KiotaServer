using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Formats.Cbor;

    public class CborSerializer : ISerializationWriter
    {
        public Action<IParsable>? OnBeforeObjectSerialization { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<IParsable>? OnAfterObjectSerialization { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<IParsable, ISerializationWriter>? OnStartObjectSerialization { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private CborWriter writer = new CborWriter();
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Stream GetSerializedContent()
        {
            return new MemoryStream(writer.Encode());
        }

        public void WriteAdditionalData(IDictionary<string, object> value)
        {
            throw new NotImplementedException();
        }

        public void WriteBoolValue(string? key, bool? value)
        {
            throw new NotImplementedException();
        }

        public void WriteByteArrayValue(string? key, byte[]? value)
        {
            throw new NotImplementedException();
        }

        public void WriteByteValue(string? key, byte? value)
        {
            throw new NotImplementedException();
        }

        public void WriteCollectionOfEnumValues<T>(string? key, IEnumerable<T?>? values) where T : struct, Enum
        {
            throw new NotImplementedException();
        }

        public void WriteCollectionOfObjectValues<T>(string? key, IEnumerable<T>? values) where T : IParsable
        {
            throw new NotImplementedException();
        }

        public void WriteCollectionOfPrimitiveValues<T>(string? key, IEnumerable<T>? values)
        {
            throw new NotImplementedException();
        }

        public void WriteDateTimeOffsetValue(string? key, DateTimeOffset? value)
        {
            throw new NotImplementedException();
        }

        public void WriteDateValue(string? key, Date? value)
        {
            throw new NotImplementedException();
        }

        public void WriteDecimalValue(string? key, decimal? value)
        {
            throw new NotImplementedException();
        }

        public void WriteDoubleValue(string? key, double? value)
        {
            throw new NotImplementedException();
        }

        public void WriteEnumValue<T>(string? key, T? value) where T : struct, Enum
        {
            throw new NotImplementedException();
        }

        public void WriteFloatValue(string? key, float? value)
        {
            throw new NotImplementedException();
        }

        public void WriteGuidValue(string? key, Guid? value)
        {
            throw new NotImplementedException();
        }

        public void WriteIntValue(string? key, int? value)
        {
            writer.WriteTextString(key);
            if (value.HasValue)
                writer.WriteInt64(value.Value);
            else
                writer.WriteNull();
        }

        public void WriteLongValue(string? key, long? value)
        {
            throw new NotImplementedException();
        }

        public void WriteNullValue(string? key)
        {
            throw new NotImplementedException();
        }

        public void WriteObjectValue<T>(string? key, T? value, params IParsable?[] additionalValuesToMerge) where T : IParsable
        {
            if (key != null)
                writer.WriteTextString(key);
            
            writer.WriteStartMap(null);
            value?.Serialize(this);
            writer.WriteEndMap();
            
        }

        public void WriteSbyteValue(string? key, sbyte? value)
        {
            throw new NotImplementedException();
        }

        public void WriteStringValue(string? key, string? value)
        {
            writer.WriteTextString(key);
            writer.WriteTextString(value);
        }

        public void WriteTimeSpanValue(string? key, TimeSpan? value)
        {
            throw new NotImplementedException();
        }

        public void WriteTimeValue(string? key, Time? value)
        {
            throw new NotImplementedException();
        }

        internal void Reset()
        {
            writer.Reset();
        }
    }
