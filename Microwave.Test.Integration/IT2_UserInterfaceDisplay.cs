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
        //private Display _display;
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
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _display = new Display(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            
            _IuserInterface = new UserInterface(_powerButton,_timeButton,_startCancelButton,_door,_display, _light,_cookController);

        }


        [Test]
        public void OnPowerPressedReadyStateOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this,EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 50 W")));
        }

        [Test]
        public void OnPowerPressedSetPowerStateOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.OnPowerPressed(this,EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 100 W")));
        }

        [Test]
        public void OnTimePressedSetPowerStateOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this,EventArgs.Empty);
            _IuserInterface.OnTimePressed(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 01:00")));
        }

        [Test]
        public void OnTimePressedSetTimeStateOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.OnTimePressed(this, EventArgs.Empty);
            _IuserInterface.OnTimePressed(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display shows: 02:00")));
        }

        [Test]
        public void OnStartCancelPressedSetPowerOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this,EventArgs.Empty);
            _IuserInterface.OnStartCancelPressed(this,EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

        //[Test]
        //public void OnStartCancelPressedSetTimeDisplay()
        //{
        //    //Kan ikke testes
        //}

        [Test]
        public void OnStartCancelPressedCookingOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.CookingIsDone();
            _IuserInterface.OnStartCancelPressed(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

        [Test]
        public void OnDoorOpenedSetPowerOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.OnDoorOpened(this,EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

        [Test]
        public void OnDoorOpenedSetTimeOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.OnTimePressed(this, EventArgs.Empty);
            _IuserInterface.OnDoorOpened(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

        [Test]
        public void OnDoorOpenedCookingOnDisplay()
        {
            _IuserInterface.OnPowerPressed(this, EventArgs.Empty);
            _IuserInterface.OnTimePressed(this, EventArgs.Empty);
            _IuserInterface.OnDoorOpened(this, EventArgs.Empty);
            _IuserInterface.CookingIsDone();
            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Display cleared")));
        }

    }
}
