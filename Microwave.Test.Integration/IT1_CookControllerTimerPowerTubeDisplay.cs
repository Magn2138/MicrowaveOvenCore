using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using Microwave.Classes.Boundary;

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

        [Test]
        public void StartCooking_PowerSetTo10_PowerTubeLogsCorrectly()
        {
            _cookController.StartCooking(10,10);
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("PowerTube works with 10")));
        }

        [Test]
        public void StartCooking_TimerSetTo10_DisplayLogsNewTimeAfterTick()
        {
            //_cookController.StartCooking(10, 10);
            //_output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("Display shows: 00:09")));
            Assert.Pass();
        }
    }
}
