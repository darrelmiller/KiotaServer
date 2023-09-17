using Microsoft.Kiota.Abstractions.Serialization;

public class KiotaResult<T>:IResult where T : IParsable   {
    private readonly T _value;
    private ISerializationWriter _writer;
    private IEnumerable<T> _values;
    private readonly string contentType;

    public KiotaResult(T value, string contentType)
    {
        _value = value;
        this.contentType = contentType;
        _writer = SerializationWriterFactoryRegistry.DefaultInstance.GetSerializationWriter(contentType);
    }

    public KiotaResult(IEnumerable<T> values, string contentType) 
    {
        _values = values;
        this.contentType = contentType;
        _writer = SerializationWriterFactoryRegistry.DefaultInstance.GetSerializationWriter(contentType);
    }

    public Task ExecuteAsync(HttpContext httpContext) {
        httpContext.Response.ContentType = this.contentType;

        if (_value != null) {
            _writer.WriteObjectValue(null, _value);
        } else if (_values != null) { 
            _writer.WriteCollectionOfObjectValues<T>(null, _values);        
        } 
        var stream = _writer.GetSerializedContent();
        httpContext.Response.ContentLength = stream.Length;
        return stream.CopyToAsync(httpContext.Response.Body);
    }
     
}
