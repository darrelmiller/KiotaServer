using BenchmarkDotNet.Running;
using KiotaFruit;


//await GetSizes();
BenchmarkRunner.Run<BenchmarkHarness>(args:args);

async Task GetSizes() {
    Console.WriteLine($"Size of fruits payload");
    Console.WriteLine("==========");
    await SummarizeSizes();
}
async Task SummarizeSizes() {

    var httpClient = new HttpClient();
    
    await GetSize(httpClient,"application/json","identity");
    await GetSize(httpClient,"application/cbor","identity");
    await GetSize(httpClient,"application/json","gzip");
    await GetSize(httpClient,"application/cbor","gzip");
    await GetSize(httpClient,"application/json","br");
    await GetSize(httpClient,"application/cbor","br");
}

async Task GetSize(HttpClient client, string contentType, string encoding) {
    var request = new HttpRequestMessage() {
        RequestUri = new Uri("https://localhost:7271/fruits")
    };
    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType));
    request.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(encoding));
    request.Version = new Version(2,0);
    request.VersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    using var response = await client.SendAsync(request);
    Console.WriteLine($"Content Type: {contentType} Encoding: {encoding} Length: {response.Content.Headers.ContentLength}");
}