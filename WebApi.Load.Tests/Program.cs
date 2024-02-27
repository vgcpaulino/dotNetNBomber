using NBomber.CSharp;

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

NBomberRunner.RegisterScenarios(scenario).Run();