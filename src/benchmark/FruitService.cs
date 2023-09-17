
using KiotaFruit;
using KiotaFruit.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;

public static class KiotaClientFactoryExtensions {
    public static HttpClient Create(IList<DelegatingHandler> handlers, HttpMessageHandler finalHandler = null) {
        if (finalHandler == null) {
            finalHandler = KiotaClientFactory.GetDefaultHttpMessageHandler();
        }
        var pipeline = KiotaClientFactory.ChainHandlersCollectionAndGetFirstLink(finalHandler, handlers.ToArray());
        return KiotaClientFactory.Create(pipeline);
    }
}

public class FruitService
{
    public enum HttpVersion {
        V1_1,
        V2_0,
        V3_0
    }
    private FruitClient fruitClient;
    public FruitService(string contentType, string encoding, HttpVersion httpVersion)
    {
        var handlers = KiotaClientFactory.CreateDefaultHandlers();
        handlers.Insert(0, new CompressionHandler());
        Version version = new Version(1,1);
        switch (httpVersion) {
            case HttpVersion.V1_1:
                version = new Version(1,1);
                break;
            case HttpVersion.V2_0:
                version = new Version(2,0);
                break;
            case HttpVersion.V3_0:
                version = new Version(3,0);
                break;
        }
        handlers.Insert(0, new HttpVersionMessageHandler(version));
        var client = KiotaClientFactoryExtensions.Create(handlers);
        client.DefaultRequestVersion = new Version(2,0);
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
        client.DefaultRequestHeaders.Add("Accept", contentType);
        client.DefaultRequestHeaders.Add("Accept-Encoding", encoding);    
        var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient:client);
        fruitClient = new FruitClient(requestAdapter);
    }
    public async Task<Fruit> GetFruitAsync(string name)
    {
        return await fruitClient.Fruits[name].GetAsync();
    }

    public async Task<IEnumerable<Fruit>> GetFruitsAsync()
    {
        return await fruitClient.Fruits.GetAsync();
    }

    // EchoFruit
    public async Task<Fruit> EchoFruitAsJsonAsync(Fruit fruit)
    {
        return  await fruitClient.EchofruitAsJson.PostAsync(fruit);
    }

    public async Task<Fruit> EchoFruitAsCborAsync(Fruit fruit)
    {
        return  await fruitClient.EchofruitAsCbor.PostAsync(fruit);
    }
}

public class HttpVersionMessageHandler : DelegatingHandler
{
    private readonly Version httpVersion;

    public HttpVersionMessageHandler(Version httpVersion)
    {
        this.httpVersion = httpVersion;
    }
    override protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Version = httpVersion;
        request.VersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        return await base.SendAsync(request, cancellationToken);
    }
}