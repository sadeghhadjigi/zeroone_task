using System.Diagnostics;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZeroOneTask.Application;
using ZeroOneTask.Application.Interfaces;
using ZeroOneTask.Infrastructure.Persistence;
using ZeroOneTask.Infrastructure.Persistence.Repositories;

var startDate = DateTime.ParseExact(args[0], "yyyy-mm-dd", CultureInfo.InvariantCulture);
var endDate = DateTime.ParseExact(args[1], "yyyy-mm-dd", CultureInfo.InvariantCulture);
var agencyId = int.Parse(args[2]);

Console.WriteLine("Starting...");

var stopWatch = new Stopwatch();
stopWatch.Start();

try
{
    var serviceProvider = new ServiceCollection()
    .AddDbContext<ZeroOneTaskDbContext>(options =>
        options.UseSqlServer("Server=.; Database=ZeroOneTaskDB; Trusted_Connection=True; TrustServerCertificate=True"))
    .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
    .AddScoped<IFlightRepository, FlightRepository>()
    .AddSingleton<ZeroOneTaskApp>()
    .BuildServiceProvider();

    var app = serviceProvider.GetService<ZeroOneTaskApp>();
    await app?.Run(startDate, endDate, agencyId);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

stopWatch.Stop();
Console.WriteLine("Finished");
Console.WriteLine($"Total time: {stopWatch.Elapsed}");

Console.ReadLine();