using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class IT2_UserInterfaceDisplay
    {
        private IUserInterface _IuserInterface;
        private IDisplay _display;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ILight _light;
        private ICookController _cookController;
        private IOutput _output ;
        private IPowerTube _powerTube;
        private ITimer _timer;

        

        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _output = Substitute.For<IOutput>();
            _light = Substitute.For<ILight>();

            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _display = new Display(_output);
            
            _cookController = Substitute.For<CookController>(_timer,_display,_powerTube);
            _IuserInterface = new UserInterface(_powerButton,_timeButton,_startCancelButton,_door,_display, _light,_cookController);

        }


        [Test]
        public void OnPowerPressed_ReadyStateOnDisplay_Output()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 50 W")));
        }

        [Test]
        public void OnTimePressed_SetPowerStateOnDisplay_Output()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 01:00")));
        }

        [Test]
        public void OnStartCancelPressed_SetPowerOnDisplay_Output()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

    }
}
