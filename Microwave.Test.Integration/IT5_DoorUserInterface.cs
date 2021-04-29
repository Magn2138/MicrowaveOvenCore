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
    class IT5_DoorUserInterface
    {
        private CookController _cook;
        private UserInterface _uut;
        private Timer _time;
        private Display _disp;
        private PowerTube _powertube;
        private Light _light;

        //Stubs
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startcancelButton;
        private IOutput _output;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startcancelButton = Substitute.For<IButton>();

            _time = new Timer();
            _disp = new Display(_output);
            _powertube = new PowerTube(_output);
            _light = new Light(_output);
            _door = new Door();

            _cook = new CookController(_time, _disp, _powertube);
            _uut = new UserInterface(_powerButton, _timeButton, _startcancelButton, _door, _disp, _light, _cook);
        }

		[Test]
		public void onDoorOpened_Open_Output()
		{
            _door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Light is turned on")));
        }

        [Test]
        public void onDoorClosed_Closed_Output()
        {
            _door.Open();
            _door.Close();

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains($"Light is turned off")));
        }



    }
}
