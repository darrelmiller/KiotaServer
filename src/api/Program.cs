using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Kiota.Serialization.Cbor;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using KiotaFruit.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using System.Runtime.ExceptionServices;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(7271, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
    
});

builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add(new GzipCompressionProvider(
        new GzipCompressionProviderOptions(){
            Level = System.IO.Compression.CompressionLevel.Fastest
        })
    ); 
    options.MimeTypes = new[] { "application/cbor", "application/json" };
});

var app = builder.Build();

app.UseResponseCompression();
//app.UseHttpsRedirection();

var parseNodeRegistry = new ParseNodeFactoryRegistry();
parseNodeRegistry.ContentTypeAssociatedFactories["application/cbor"] = new CborParseNodeFactory();
parseNodeRegistry.ContentTypeAssociatedFactories["application/json"] = new JsonParseNodeFactory();

var supportedMediaTypes = new List<string>() {
    "application/json",
    "application/cbor"
};
SerializationWriterFactoryRegistry.DefaultInstance.ContentTypeAssociatedFactories["application/cbor"] = new CborSerializationWriterFactory();
SerializationWriterFactoryRegistry.DefaultInstance.ContentTypeAssociatedFactories["application/json"] = new JsonSerializationWriterFactory();

// Create a list of fruits as demo data using the Fruit class from the models folder



var fruits = new Dictionary<string,Fruit>() {
    { "apple", new Fruit() { Name = "Apple", Color = "Red", Weight = 100, Citrus = false, Description="A delicious apple", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "banana", new Fruit() { Name = "Banana", Color = "Yellow", Weight = 200, Citrus = false, Description="A bendy fruit", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "orange", new Fruit() { Name = "Orange", Color = "Orange", Weight = 150, Citrus = true, Description="Round, shiny and fun", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "grape", new Fruit() { Name = "Grape", Color = "Green", Weight = 50, Citrus = false, Description="Best when crushed", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },  // Thanks copilot for the rest
    { "lemon", new Fruit() { Name = "Lemon", Color = "Yellow", Weight = 50, Citrus = true, Description="So good in coleslaw", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "lime", new Fruit() { Name = "Lime", Color = "Green", Weight = 50, Citrus = true, Description="What guacamole was made for", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "strawberry", new Fruit() { Name = "Strawberry", Color = "Red", Weight = 25, Citrus = false, Description="And cream", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "watermelon", new Fruit() { Name = "Watermelon", Color = "Green", Weight = 5000, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "pear", new Fruit() { Name = "Pear", Color = "Green", Weight = 150, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "kiwi", new Fruit() { Name = "Kiwi", Color = "Green", Weight = 100, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "pineapple", new Fruit() { Name = "Pineapple", Color = "Yellow", Weight = 500, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "mango", new Fruit() { Name = "Mango", Color = "Orange", Weight = 200, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "blueberry", new Fruit() { Name = "Blueberry", Color = "Blue", Weight = 10, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "blackberry", new Fruit() { Name = "Blackberry", Color = "Black", Weight = 10, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "raspberry", new Fruit() { Name = "Raspberry", Color = "Red", Weight = 10, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "cherry", new Fruit() { Name = "Cherry", Color = "Red", Weight = 10, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "papaya", new Fruit() { Name = "Papaya", Color = "Orange", Weight = 1000, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "mangosteen", new Fruit() {Id = Guid.NewGuid(), Name = "Mangosteen", Color = "Purple", Weight = 100, Citrus = false, Description="This is a tasty Mangosteen", CreatedDateTime = DateTimeOffset.Parse("2022-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2022-02-21 11:45:00am") } },
    { "durian", new Fruit() {Id = Guid.NewGuid(), Name = "Durian", Color = "Yellow", Weight = 1000, Citrus = false, Description="This is a tasty Durian", CreatedDateTime = DateTimeOffset.Parse("2020-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2021-03-21 1:45:00pm") } },
    { "lychee", new Fruit() {Id = Guid.NewGuid(), Name = "Lychee", Color = "Red", Weight = 100, Citrus = false, Description="This is a tasty Lychee", CreatedDateTime = DateTimeOffset.Parse("2021-07-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2021-09-21 10:45:00am") } },
    { "jackfruit", new Fruit() {Id = Guid.NewGuid(), Name = "Jackfruit", Color = "Yellow", Weight = 1000, Citrus = false, Description="This is a tasty Jackfruit", CreatedDateTime = DateTimeOffset.Parse("2021-03-21 10:45:00am"), LastModifiedDateTime = DateTimeOffset.Parse("2021-03-21 10:45:00am") } }
   
};
// Add more fruits




app.MapGet("/fruits/{name}", (HttpContext context) =>
{
    string? contentType = NegotiateContentType(context, supportedMediaTypes);
    if (context.Request.RouteValues.TryGetValue("name", out var name) && fruits.ContainsKey(name as string ?? ""))
    {
        var fruitName = name as string ?? "";
        return new KiotaResult<Fruit>(fruits[fruitName], contentType);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapGet("/fruits", (HttpContext context) => {
    string? contentType = NegotiateContentType(context, supportedMediaTypes);
    return new KiotaResult<Fruit>(fruits.Values, contentType);
});

app.MapPost("/echofruitAsJson",(HttpContext context) => {
    var node = parseNodeRegistry.GetRootParseNode("application/json",context.Request.Body);
    var fruit = node.GetObjectValue(Fruit.CreateFromDiscriminatorValue) ?? throw new Exception("Something went wrong");

    string? contentType = NegotiateContentType(context, supportedMediaTypes);
    return new KiotaResult<Fruit>(fruit, contentType);
});

app.MapPost("/echofruitAsCbor",(HttpContext context) => {
    var node = parseNodeRegistry.GetRootParseNode("application/cbor",context.Request.Body);
    var fruit = node.GetObjectValue(Fruit.CreateFromDiscriminatorValue) ?? throw new Exception("Something went wrong");

    string? contentType = NegotiateContentType(context, supportedMediaTypes);
    return new KiotaResult<Fruit>(fruit, contentType);
});


app.Run();

static string NegotiateContentType(HttpContext context, List<string> supportedMediaTypes)
{
    // if query parameter called format exists, then use that as the mediatype
    if (context.Request.Query.TryGetValue("format", out var format))
    {
        // Map cbor to application/cbor and json to application/json
        if (format == "cbor")
        {
            return "application/cbor";
        }
        else if (format == "json")
        {
            return "application/json";
        }
    }
    var contentType = context.Request.Headers.Accept.First();
    if (contentType == null || !supportedMediaTypes.Any(s => s == contentType))
    {
        contentType = MediaTypeNames.Application.Json;
    }

    return contentType;
}