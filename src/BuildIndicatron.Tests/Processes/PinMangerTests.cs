using System.Collections.Generic;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class PinMangerTests
	{

		private PinManager _pinManager;

		#region Setup/Teardown

		public void Setup()
		{
			_pinManager = new PinManager(new GpioConfiguration(
				                             new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO11, true),
				                             new GpioConfiguration.Target(PinName.SecondaryLightRed, GpIO.GPIO7, true),
				                             new GpioConfiguration.Target(PinName.SecondaryLightGreen, GpIO.GPIO8, true),
				                             new GpioConfiguration.Target(PinName.MainLightRed, GpIO.GPIO11, true),
				                             new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO25, true),
				                             new GpioConfiguration.Target(PinName.MainLightBlue, GpIO.GPIO9, true),
				                             new GpioConfiguration.Target(PinName.Spinner, GpIO.GPIO22, true),
				                             new GpioConfiguration.Target(PinName.Fire, GpIO.GPIO10, true)
				                             )
				{
					Buttons =
						new List<GpioConfiguration.Button>() {new GpioConfiguration.Button() {Pin = GpIO.GPIO24, IsToggle = false}}
				});
		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_pinManager.Should().NotBeNull();
		}

		 
	}

}