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
    class IT3_UserInterfaceLight
    {
        private IUserInterface _userInterface;
        private ILight _light;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timerButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private CookController _cookController;

        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timerButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _cookController = Substitute.For<CookController>(_timer, _display, _powerTube, _userInterface);
            _light = new Light(_output);
            _userInterface = new UserInterface(_powerButton, _timerButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void UI_OnStartCancelPressed_SETTIME_test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void UI_OnStartCancelPressed_COOKING_test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void UI_OnDoorOpened_READY_test()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void UI_OnDoorOpened_SETPOWER_test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void UI_OnDoorOpened_SETTIME_test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void UI_OnDoorClosed_DOOROPEN_test()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void UI_CookingIsDone_COOKING_test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _userInterface.CookingIsDone();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }
    }
}
