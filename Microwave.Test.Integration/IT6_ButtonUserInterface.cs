using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class IT6_ButtonUserInterface
    {

        private IOutput _output;
        private Timer _timer;
        private Display _display;
        private PowerTube _powerTube;
        private Light _light;
        private Door _door;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private CookController _cookController;
        private UserInterface _userInterface;

        [SetUp]
        public void Setup()
        {
            
            _output = Substitute.For<IOutput>();
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void PowerButtonPressed_OvenIsReady_DisplayShowsPower50()
        {
            _powerButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("Display shows: 50 W")));
        }

        [Test]
        public void TimeButtonPressed_PowerIsSet_DisplayShows01_00()
        {
            _powerButton.Press();
            _timeButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("Display shows: 01:00")));
        }

        [Test]
        public void StartCancelButtonPressed_PowerIsSet_DisplayCleared()
        {
            _powerButton.Press();
            _startCancelButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("Display cleared")));
        }
    }
}
