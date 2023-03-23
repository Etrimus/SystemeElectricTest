using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using SystemeElectric.Core;
using SystemeElectric.Core.Timer;
using Xunit;

namespace SystemeElectric.Tests.Unit;

public class TimerCoreServiceUnitTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly Mock<ILogger> _logger = new(MockBehavior.Strict);
    private readonly Mock<IRandomDataService> _randomDataService = new(MockBehavior.Strict);
    private readonly Mock<ITimerService> _timerService = new(MockBehavior.Strict);

    private readonly ITimerCoreService _timerCoreService;

    public TimerCoreServiceUnitTests()
    {
        _timerCoreService = new TimerCoreService(_logger.Object, _randomDataService.Object, _timerService.Object);
    }

    [Fact]
    public void StartDriversThread_IsDriversThreadActive_true()
    {
        _setupLogger(_logger);

        _timerCoreService.StartDriversThread();

        _timerCoreService.IsDriversThreadActive.Should().BeTrue();

        _logger.VerifyAll();
    }

    [Theory]
    [InlineData(7, 3, 2)]
    [InlineData(11, 5, 3)]
    [InlineData(17, 8, 5)]
    public async Task Init_ExpectedDataCountBySeconds(int totalSeconds, int expectedCarsCount, int expectedDriversCount)
    {
        _setupLogger(_logger);
        _randomDataService.Setup(x => x.GetRandomCarModel()).Returns(_fixture.Create<string>());
        _randomDataService.Setup(x => x.GetRandomDriverName()).Returns(_fixture.Create<string>());

        var cancellationTokenSource = new CancellationTokenSource();

        _timerService.Setup(x => x.Start(It.IsAny<double>())).Callback(async () =>
        {
            await new TaskFactory().StartNew(() =>
            {
                var currentTime = new TimeSpan();

                while (currentTime.TotalSeconds < totalSeconds)
                {
                    Task.Delay(10).Wait();

                    currentTime = currentTime.Add(TimeSpan.FromSeconds(1));

                    _timerService.Raise(service => service.Elapsed += null, new TimerElapsedEventArgs(new DateTime(currentTime.Ticks)));
                }

                cancellationTokenSource.Cancel();
                _timerService.Raise(service => service.Elapsed += null, new TimerElapsedEventArgs(new DateTime(currentTime.Ticks)));
            });
        });

        _timerService.Setup(x => x.Stop());

        var carsCount = 0;
        var driversCount = 0;

        _timerCoreService.CarArrived += (_, _) => carsCount++;
        _timerCoreService.DriverArrived += (_, _) => driversCount++;

        _timerCoreService.Init(cancellationTokenSource.Token);
        _timerCoreService.StartCarsThread();
        _timerCoreService.StartDriversThread();

        await Task.Delay(1000);

        cancellationTokenSource.Dispose();

        using (new AssertionScope())
        {
            carsCount.Should().Be(expectedCarsCount);
            driversCount.Should().Be(expectedDriversCount);
        }

        _logger.VerifyAll();
        _randomDataService.VerifyAll();
        _timerService.VerifyAll();
    }

    [Fact]
    public void Init_AlreadyInit()
    {
        _timerService.Setup(x => x.Start(It.IsAny<double>())).Callback(() => { });

        _timerCoreService.Init(CancellationToken.None);

        _timerCoreService.Invoking(x => x.Init(It.IsAny<CancellationToken>())).Should().Throw<SystemeElectricException>();

        _logger.VerifyAll();
        _randomDataService.VerifyAll();
        _timerService.VerifyAll();
    }

    [Fact]
    public void Dispose_AllThreadStopped()
    {
        _setupLogger(_logger);

        _timerCoreService.Dispose();

        using (new AssertionScope())
        {
            _timerCoreService.IsDriversThreadActive.Should().BeFalse();
            _timerCoreService.IsCarsThreadActive.Should().BeFalse();
        }

        _logger.VerifyAll();
    }

    private static void _setupLogger(Mock<ILogger> logger)
    {
        logger.Setup(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            )
        );
    }
}