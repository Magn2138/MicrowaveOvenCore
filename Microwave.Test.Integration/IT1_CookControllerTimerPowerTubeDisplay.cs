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
        private CookController _cookController;
        private Timer _timer;
        private Display _display;
        private PowerTube _powerTube;


        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
	}
}
