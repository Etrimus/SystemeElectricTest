using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace SystemeElectic.Client;

internal class Program
{
    private static readonly AutoResetEvent _autoEventCars = new(false);
    private static readonly AutoResetEvent _autoEventDrivers = new(false);
    private static DateTime _currentTime;

    private static void Main(string[] args)
    {
        var carsThread = new Thread(o =>
        {
            while (true)
            {
                _autoEventCars.WaitOne();
                Console.WriteLine(Thread.CurrentThread.Name);
                // Console.WriteLine(_currentTime.Ticks);
            }
        })
        {
            Name = "Cars thread"
        };

        var driversThread = new Thread(o =>
        {
            while (true)
            {
                _autoEventDrivers.WaitOne();
                Console.WriteLine(Thread.CurrentThread.Name);
                // Console.WriteLine(_currentTime.Ticks);
            }
        })
        {
            Name = "Drivers thread"
        };

        carsThread.Start();
        driversThread.Start();

        var elapsedTime = new TimeSpan();

        var timer = new Timer(1000);

        timer.Elapsed += (sender, e) =>
        {
            _currentTime = DateTime.Now;
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));

            Console.WriteLine();
            Console.WriteLine($"Total: {elapsedTime.TotalSeconds}");

            if (elapsedTime.TotalSeconds % 2 == 0)
            {
                Console.Write(2);
                _autoEventCars.Set();
            }

            if (elapsedTime.TotalSeconds % 3 == 0)
            {
                Console.Write(" 3");
                _autoEventDrivers.Set();
            }

            Console.WriteLine();
        };

        timer.Enabled = true;
        timer.AutoReset = true;
        timer.Start();

        Console.ReadLine();
    }
}