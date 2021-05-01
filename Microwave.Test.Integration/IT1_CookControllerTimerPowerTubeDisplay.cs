using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using Microwave.Classes.Boundary;
using System.Threading;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    class IT1_CookControllerTimerPowerTubeDisplay
    {
        private IUserInterface _userInterface;
        private IOutput _output;
        private Timer _timer;
        private Display _display;
        private PowerTube _powerTube;
        private CookController _cookController;

        [SetUp]
        public void Setup()
        {
            _userInterface = Substitute.For<IUserInterface>();
            _output = Substitute.For<IOutput>();
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
        }

        [TestCase(50)]
        [TestCase(350)]
        [TestCase(700)]
        public void StartCooking_PowerSetToCorrectValues_PowerTubeLogsWorkingCorrectly(int power)
        {
            _cookController.StartCooking(power, 60);
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains($"PowerTube works with {power}")));
        }

        [Test]
        public void StartCooking_TimerSetTo60_DisplayLogsNewTimeAfterTick()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.TimerTick += (sender, args) => pause.Set();

            _cookController.StartCooking(50, 60);
            pause.WaitOne();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("Display shows: 00:59")));
        }

        [Test]
        public void StartCooking_TimerExpired_PowerTubeLogsTurnOff()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.Expired += (sender, args) => pause.Set();

            _cookController.StartCooking(50, 1);

            pause.WaitOne();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("PowerTube turned off")));
        }

        [Test]
        public void StartCooking_TimerExpired_UserIterfaceIsInformedThatCookingIsDone()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.Expired += (sender, args) => pause.Set();

            _cookController.StartCooking(50, 1);

            pause.WaitOne();

            _userInterface.Received(1).CookingIsDone();
        }

        [Test]
        public void StopCooking_StopWhileRunning_PowerTubeLogsTurnOff()
        {
            _cookController.StartCooking(50, 60);
            _cookController.Stop();
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("PowerTube turned off")));
        }

        [Test]
        public void StopCooking_StopWhileRunning_TimerIsNotCreatingTimerTickEvents()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.TimerTick += (sender, args) => pause.Set();

            _cookController.StartCooking(50, 60);

            _cookController.Stop();

            Assert.That(!pause.WaitOne(1100));
        }
    }
}
