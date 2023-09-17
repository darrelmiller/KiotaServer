using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using KiotaFruit.Models;

namespace KiotaFruit
{
    [AllStatisticsColumn] 
    [ShortRunJob]
    public class BenchmarkHarness
    {
        [Params(100)]
        public int Iterations;

        [Params(1,10)]
        public int ConcurrentRequests;


        [Params("application/json", "application/cbor")]
        public string ContentType;

        [Params("gzip", "identity")]
        public string Encoding;

        [Params(FruitService.HttpVersion.V1_1, FruitService.HttpVersion.V2_0, FruitService.HttpVersion.V3_0)]
        public static FruitService.HttpVersion HttpVersion;

        private Dictionary<string, FruitService> fruitServices;

        [GlobalSetup]
        public void GlobalSetup() {
            fruitServices = new Dictionary<string, FruitService>() {
                ["application/json-identity-V1_1"] = new FruitService("application/json","identity", FruitService.HttpVersion.V1_1),
                ["application/cbor-identity-V1_1"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V1_1),
                ["application/json-gzip-V1_1"] = new FruitService("application/cbor","identity",FruitService.HttpVersion.V1_1),
                ["application/cbor-gzip-V1_1"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V1_1),
                ["application/json-identity-V2_0"] = new FruitService("application/json","identity", FruitService.HttpVersion.V2_0),
                ["application/cbor-identity-V2_0"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V2_0),
                ["application/json-gzip-V2_0"] = new FruitService("application/cbor","identity",FruitService.HttpVersion.V2_0),
                ["application/cbor-gzip-V2_0"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V2_0),
                ["application/json-identity-V3_0"] = new FruitService("application/json","identity", FruitService.HttpVersion.V3_0),
                ["application/cbor-identity-V3_0"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V3_0),
                ["application/json-gzip-V3_0"] = new FruitService("application/cbor","identity",FruitService.HttpVersion.V3_0),
                ["application/cbor-gzip-V3_0"] = new FruitService("application/cbor","identity", FruitService.HttpVersion.V3_0),
            };
             
        }


        [Benchmark]
        public async Task GetFruits()
        {
            var fruitService = fruitServices[$"{ContentType}-{Encoding}-{HttpVersion}"];
            for (int i = 0; i < Iterations; i+=ConcurrentRequests)
            {
                var tasks = new Task[ConcurrentRequests];
                for(int j = 0; j < ConcurrentRequests; j++) {
                    tasks[j] = fruitService.GetFruitsAsync();
                }
                await Task.WhenAll(tasks);
            }
        }



        [Benchmark]
        public async Task EchoJsonFruit()
        {
            var fruitService = fruitServices[$"{ContentType}-{Encoding}-{HttpVersion}"];
            var fruit = new Fruit() {
                Id = Guid.NewGuid(),
                Name = "Raisin",
                Color = "purple",
                Weight = 0.5,
                Citrus = false,
                Description = "A dried grape",
                CreatedDateTime = System.DateTimeOffset.UtcNow,
                LastModifiedDateTime = System.DateTimeOffset.UtcNow

            };

            for (int i = 0; i < Iterations; i+=ConcurrentRequests)
            {
                var newfruit = await fruitService.EchoFruitAsJsonAsync(fruit);
            }
        }

        // Benchmark Get Fruits as CBOR
        [Benchmark]
        public async Task EchoCborFruit()
        {
            var fruitService = fruitServices[$"{ContentType}-{Encoding}-{HttpVersion}"];
             var fruit = new Fruit() {
                Id = Guid.NewGuid(),
                Name = "Raisin",
                Color = "purple",
                Weight = 0.5,
                Citrus = false,
                Description = "A dried grape",
                CreatedDateTime = System.DateTimeOffset.UtcNow,
                LastModifiedDateTime = System.DateTimeOffset.UtcNow
            };
            
            for (int i = 0; i < Iterations; i+=ConcurrentRequests)
            {
                var tasks = new Task[ConcurrentRequests];
                for(int j = 0; j < ConcurrentRequests; j++) {
                    tasks[j] = fruitService.EchoFruitAsCborAsync(fruit);
                }
                await Task.WhenAll(tasks);
            }
        }

        [Benchmark]
        public async Task GetAFruit()
        {
            var fruitService = fruitServices[$"{ContentType}-{Encoding}-{HttpVersion}"];
            for (int i = 0; i < Iterations; i+=ConcurrentRequests)
            {
                var tasks = new Task[ConcurrentRequests];
                for(int j = 0; j < ConcurrentRequests; j++) {
                    tasks[j] = fruitService.GetFruitAsync("apple");
                }
                await Task.WhenAll(tasks);
            }
        }

    }

    public class AllowNonOptimized : ManualConfig
    {
        public AllowNonOptimized()
        {
            //Add(MemoryDiagnoser.Default);
            // Add(JitOptimizationsValidator.DontFailOnError);

            // Add(DefaultConfig.Instance.GetLoggers().ToArray());
            // Add(DefaultConfig.Instance.GetExporters().ToArray());
            // Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
        }
    }
}
