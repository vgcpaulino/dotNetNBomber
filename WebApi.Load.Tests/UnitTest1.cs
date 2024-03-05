using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace WebApi.Load.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        string baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:7117";
        Console.WriteLine($"Base URL: {baseUrl}");

        using var httpClient = new HttpClient();
        var scenario = Scenario.Create("Load Test", async context =>
            {
                var response = await httpClient.GetAsync($"{baseUrl}/products/99999999-9999-9999-9999-999999999999");
                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(
                    rate: 2,
                    interval: TimeSpan.FromSeconds(10),
                    during: TimeSpan.FromSeconds(30)
                )
            );

        NodeStats result = NBomberRunner.RegisterScenarios(scenario).Run();

        Assert.Equal(6, result.ScenarioStats[0].AllOkCount);
        double requestPerSeconds = result.ScenarioStats[0].Ok.Request.RPS;
        Assert.True(requestPerSeconds >= 0.2, $"The RPC is below expected ({requestPerSeconds})");
        double meanMs = result.ScenarioStats[0].Ok.Latency.MeanMs;
        Assert.True(meanMs <= 8944.3, $"The mean request execution time above expected ({meanMs})");
    }
}