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
    class IT4_UserInterfaceCookController
    {
        private CookController _cook;
        private UserInterface _uut;
        private Timer _time;
        private Display _disp;
        private PowerTube _powertube;
        private Light _light;

        
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startcancelButton;
        private IOutput _output;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _door = Substitute.For<IDoor>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startcancelButton = Substitute.For<IButton>();

            _time = new Timer();
            _disp = new Display(_output);
            _powertube = new PowerTube(_output);
            _light = new Light(_output);

            _cook = new CookController(_time, _disp, _powertube);
            _uut = new UserInterface(_powerButton, _timeButton, _startcancelButton, _door, _disp, _light, _cook);
        }
        

        [Test]
        public void onStartCancelPressed_SetTimeState_Output()
        {
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"PowerTube works with 50")));
        }

        [Test]
        public void onStartCancelPressed_setCookingState_Output()
        {
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);
            _uut.OnStartCancelPressed(this, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"PowerTube turned off")));
        }

        
        [Test]
        public void onDoorOpened_setCookingState_Output()
        {
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);
            _uut.OnStartCancelPressed(this, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _uut.OnDoorOpened(this, EventArgs.Empty);

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"PowerTube turned off")));
          
        }







    }
}
