using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SystemeElectric.HubClient;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/hub")
            .Build();

        connection.On<DateTime, string>("DriverArrived", (time, name) => { Console.WriteLine($"[Driver] {time}: {name}"); });
        connection.On<DateTime, string>("CarArrived", (time, model) => { Console.WriteLine($"[Car] {time}: {model}"); });

        await connection.StartAsync();

        Console.ReadLine();
    }
}