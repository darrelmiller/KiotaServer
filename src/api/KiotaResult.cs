using Microsoft.Kiota.Abstractions.Serialization;

public class KiotaResult : IResult {
    private readonly IParsable _value;
    private ISerializationWriter writer;
  
    public KiotaResult(IParsable value, ISerializationWriter writer)
    {
        this._value = value;
        this.writer = writer;
    }
    public Task ExecuteAsync(HttpContext httpContext) {
        httpContext.Response.ContentType = "application/cbor";
        writer.WriteObjectValue(null, _value);
        var stream = writer.GetSerializedContent();
        return stream.CopyToAsync(httpContext.Response.Body);
    }
     
}
